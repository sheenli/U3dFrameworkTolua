using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ILogic
{
    /// <summary>
    /// 当前节点是否有效
    /// </summary>
    bool IsValid { get;}

    /// <summary>
    /// 父节点
    /// </summary>
    LogicNode Parent { get; set; }

    void OnUpdate();
    void OnLateUpdate();
    void OnFixedUpdate();
    void OnApplicationQuit();
    void OnApplicationPause(bool pauseStatus);

    void OnDestroy();

    void OnEnable();

    void OnDisable();
}
/// <summary>
/// 游戏逻辑节点
/// 1.AttachNode到上层节点后会进入统一的消息转发时序
/// 2.AttachNode到上层节点后会进入统一的帧逻辑时序
/// </summary>
public class LogicNode : MonoBehaviour, ILogic
{
    #region 线程安全区

    /// <summary>
    /// 当前逻辑节点是否需要工作在线程安全模式下
    /// ------！警告！------
    /// AttachNode后不能修改
    /// ------！警告！------
    /// </summary>
    public bool threadSafe = false;

    #endregion

    #region 节点

    /// <summary>
    /// 用于维护logicList队列
    /// </summary>
    private struct LogicPack
    {
        public ILogic logic;
        public bool addOrRemove;

        public LogicPack(ILogic lo, bool addOrRe)
        {
            logic = lo;
            addOrRemove = addOrRe;
        }
    }

    /// <summary>
    /// 用于识别根节点进行
    /// OnLogicFrame
    /// OnFixedUpdate
    /// OnUpdate
    /// OnLateUpdate
    /// 等方法
    /// </summary>
    protected static bool isInProcessing = false;

    private int nodePriority = 0;

    public int NodePriority
    {
        get { return nodePriority; }
        set { nodePriority = value; }
    }



    /// <summary>
    /// 用于在每逻辑帧开始的时候维护逻辑节点序列
    /// </summary>
    private struct NodePack
    {
        public LogicNode logicNode;
        public bool addOrRemove;

        public NodePack(LogicNode node, bool addOrRe)
        {
            logicNode = node;
            addOrRemove = addOrRe;
        }
    }

    /// <summary>
    /// 用于保存在逻辑帧开始的时候需要更新的NodePack
    /// </summary>
    private Queue nodeToUpdate = new Queue();

    /// <summary>
    /// 储存子逻辑节点
    /// </summary>
    private List<LogicNode> nodeList = new List<LogicNode>();


    /// <summary>
    /// 线程安全
    /// 挂接一个逻辑节点
    /// 节点将在下一逻辑帧执行前挂入
    /// </summary>
    /// <param name="node"></param>
    public void AttachNode(LogicNode node)
    {
        if (threadSafe)
        {
            lock (nodeToUpdate)
            {
                if (isInProcessing)
                {
                    nodeToUpdate.Enqueue(new NodePack(node, true));
                }
                else
                {
                    AttachNodeNow(node);
                }
            }
        }
        else
        {
            if (isInProcessing)
            {
                nodeToUpdate.Enqueue(new NodePack(node, true));
            }
            else
            {
                AttachNodeNow(node);
            }
        }
    }


    /// <summary>
    /// 线程安全
    /// 摘下一个逻辑节点
    /// 节点将在下一逻辑帧执行前摘除
    /// </summary>
    /// <param name="node"></param>
    public void DetachNode(LogicNode node)
    {
        if (threadSafe)
        {
            lock (nodeToUpdate)
            {
                if (isInProcessing)
                {
                    nodeToUpdate.Enqueue(new NodePack(node, false));
                }
                else
                {
                    DetachNodeNow(node);
                }
            }
        }
        else
        {
            if (isInProcessing)
            {
                nodeToUpdate.Enqueue(new NodePack(node, false));
            }
            else
            {
                DetachNodeNow(node);
            }
        }
    }


    /// <summary>
    /// 具体执行一次LogicNode挂接
    /// </summary>
    /// <param name="node"></param>
    private void AttachNodeNow(LogicNode node)
    {
        if (null == node) return;

        if (!nodeList.Contains(node))
        {
            int pos = 0;
            int count = nodeList.Count;
            for (int n = 0; n < count; n++)
            {
                if (node.NodePriority > nodeList[n].NodePriority)
                {
                    break;
                }
                pos++;
            }
            nodeList.Insert(pos, node);
        }
    }


    /// <summary>
    /// 具体执行一次LogicNode的摘除
    /// </summary>
    /// <param name="node"></param>
    private void DetachNodeNow(LogicNode node)
    {
        if (nodeList.Contains(node))
        {
            nodeList.Remove(node);
        }
    }


    /// <summary>
    /// 由UpdateTree调用,更新当前逻辑节点的下携逻辑节点列表
    /// </summary>
    private void UpdateNodeList()
    {
        if (threadSafe)
        {
            lock (nodeToUpdate)
            {
                _UpdateNodeList();
            }
        }
        else
        {
            _UpdateNodeList();
        }
    }
    /// <summary>
    /// 更新当前逻辑节点的下携逻辑节点列表的实现
    /// </summary>
    private void _UpdateNodeList()
    {
        int count = nodeToUpdate.Count;

        while (count != 0)
        {
            count--;
            NodePack pack = (NodePack)nodeToUpdate.Dequeue();
            if (pack.addOrRemove)
            {
                AttachNodeNow(pack.logicNode);
            }
            else
            {
                DetachNodeNow(pack.logicNode);
            }
        }
    }

    #endregion

    #region 逻辑节点

    /// <summary>
    /// 用于存放其它挂入本逻辑节点时序下的ILogic对象
    /// </summary>
    private List<ILogic> logicList = new List<ILogic>();

    /// <summary>
    /// 用于在下一逻辑帧开始时进logicList的更新
    /// </summary>
    private Queue logicsToUpdate = new Queue();
    /// <summary>
    /// 挂接一个ILogic
    /// </summary>
    /// <param name="logic"></param>
    private void AttachLogicNow(ILogic logic)
    {
        if (logic == null)
        {
            Debug.LogError("LogicNode, AttachLogicNow: failed due to logic null.");
            return;
        }

        if (logicList.Contains(logic))
        {
            Debug.LogError("LogicNode, AttachLogicNow: " + logic.ToString() + " is already in list");
            return;
        }

        logicList.Add(logic);
    }
    /// <summary>
    /// 摘下一个ILogic
    /// </summary>
    /// <param name="logic"></param>
    private void DetachLogicNow(ILogic logic)
    {
        logicList.Remove(logic);
    }

    public bool HasLogic(ILogic logic)
    {
        return logicList.Contains(logic);
    }

    /// <summary>
    /// 将一个ILogic挂入当前LogicNode的时序控制下
    /// logic会在下一逻辑帧开始时挂入
    /// 执行顺序是挂入顺序
    /// </summary>
    /// <param name="logic"></param>
    public void AttachLogic(ILogic logic)
    {
        if (threadSafe)
        {
            lock (logicsToUpdate)
            {
                if (isInProcessing)
                {
                    logicsToUpdate.Enqueue(new LogicPack(logic, true));
                }
                else
                {
                    AttachLogicNow(logic);
                }
            }
        }
        else
        {
            if (isInProcessing)
            {
                logicsToUpdate.Enqueue(new LogicPack(logic, true));
            }
            else
            {
                AttachLogicNow(logic);
            }
        }
    }

    /// <summary>
    /// 将一个ILogic从当前LogicNode的时序控制下剔除
    /// logic会在下一逻辑帧开始时摘除
    /// </summary>
    /// <param name="logic"></param>
    public void DetachLogic(ILogic logic)
    {
        if (threadSafe)
        {
            lock (logicsToUpdate)
            {
                if (isInProcessing)
                {
                    logicsToUpdate.Enqueue(new LogicPack(logic, false));
                }
                else
                {
                    DetachLogicNow(logic);
                }
            }
        }
        else
        {
            if (isInProcessing)
            {
                logicsToUpdate.Enqueue(new LogicPack(logic, false));
            }
            else
            {
                DetachLogicNow(logic);
            }
        }
    }

    /// <summary>
    /// 更新逻辑列表
    /// </summary>
    private void UpdateLogicList()
    {
        if (threadSafe)
        {
            lock (logicsToUpdate)
            {
                _UpdateLogicList();
            }
        }
        else
        {
            _UpdateLogicList();
        }
    }

    /// <summary>
    /// 更新逻辑列表的实现
    /// </summary>
    private void _UpdateLogicList()
    {
        int countLogicPack = logicsToUpdate.Count;
        while (countLogicPack != 0)
        {
            LogicPack pack = (LogicPack)logicsToUpdate.Dequeue();
            countLogicPack--;
            if (pack.addOrRemove)
            {
                AttachLogicNow(pack.logic);
            }
            else
            {
                DetachLogicNow(pack.logic);
            }
        }
    }


    #endregion
    
    #region 实现接口
    protected bool mIsValid = true;
    /// <summary>
    /// 当前节点是不是有效
    /// </summary>
    public bool IsValid
    {
        get
        {
            return this.gameObject != null && this.enabled && this.gameObject.activeInHierarchy;
        }
        set
        {
        }
    }
    void FixedUpdate()
    {
        isInProcessing = true;
        if (this.IsValid)
        {
            OnFixedUpdate();
        }
        isInProcessing = false;
    }

    void Update()
    {
        isInProcessing = true;
        if (this.IsValid)
        {
            UpdateNodeList();
            UpdateLogicList();
            OnUpdate();
        }
        isInProcessing = false;
    }

    void LateUpdate()
    {
        isInProcessing = true;
        if (this.IsValid)
        {
            OnLateUpdate();
        }
        isInProcessing = false;
    }
    /// <summary>
    /// 父对象
    /// </summary>
    public LogicNode Parent { set; get; }

    public virtual void OnDestroy()
    {
        int count = nodeList.Count;
        int i = 0;
        LogicNode node = null;
        for (i = 0; i < count; i++)
        {
            node = nodeList[i];
            if (node.IsValid) node.OnDestroy();
        }

        count = logicList.Count;
        for (i = 0; i < count; i++)
        {
            if (logicList[i].IsValid)
            {
                logicList[i].OnDestroy();
            }
        }
    }

    public virtual void OnDisable()
    {
        int count = nodeList.Count;
        int i = 0;
        LogicNode node = null;
        for (i = 0; i < count; i++)
        {
            node = nodeList[i];
            if (node.IsValid) node.OnDisable();
        }

        count = logicList.Count;
        for (i = 0; i < count; i++)
        {
            if (logicList[i].IsValid)
            {
                logicList[i].OnDisable();
            }
        }
    }

    public virtual void OnEnable()
    {
        int count = nodeList.Count;
        int i = 0;
        LogicNode node = null;
        for (i = 0; i < count; i++)
        {
            node = nodeList[i];
            if (node.IsValid) node.OnEnable();
        }

        count = logicList.Count;
        for (i = 0; i < count; i++)
        {
            if (logicList[i].IsValid)
            {
                logicList[i].OnEnable();
            }
        }
    }

    public virtual void OnApplicationPause(bool pauseStatus)
    {
        int count = nodeList.Count;
        int i = 0;
        LogicNode node = null;
        for (i = 0; i < count; i++)
        {
            node = nodeList[i];
            if (node.IsValid) node.OnApplicationPause(pauseStatus);
        }

        count = logicList.Count;
        for (i = 0; i < count; i++)
        {
            if (logicList[i].IsValid)
            {
                logicList[i].OnApplicationPause(pauseStatus);
            }
        }

        //子EventDispacher//
        nodeList.Clear();
        //add dispatcher 的 cache//
        nodeToUpdate.Clear();

        //帧逻辑//
        logicList.Clear();
        logicsToUpdate.Clear();
    }

    public virtual void OnApplicationQuit()
    {
        int count = nodeList.Count;
        int i = 0;
        LogicNode node = null;
        for (i = 0; i < count; i++)
        {
            node = nodeList[i];
            if (node.IsValid) node.OnApplicationQuit();
        }

        count = logicList.Count;
        for (i = 0; i < count; i++)
        {
            if (logicList[i].IsValid)
            {
                logicList[i].OnApplicationQuit();
            }
        }
    }

    public virtual void OnFixedUpdate()
    {
        int count = nodeList.Count;
        int i = 0;
        LogicNode node = null;
        for (i = 0; i < count; i++)
        {
            node = nodeList[i];
            if (node.IsValid) node.OnFixedUpdate();
        }

        count = logicList.Count;
        for (i = 0; i < count; i++)
        {
            if (logicList[i].IsValid)
            {
                logicList[i].OnFixedUpdate();
            }
        }
    }

    public virtual void OnLateUpdate()
    {
        int count = nodeList.Count;
        int i = 0;
        LogicNode node = null;
        for (i = 0; i < count; i++)
        {
            node = nodeList[i];
            if (node.IsValid) node.OnLateUpdate();
        }

        count = logicList.Count;
        for (i = 0; i < count; i++)
        {
            if (logicList[i].IsValid)
                logicList[i].OnLateUpdate();
        }
    }

    public virtual void OnUpdate()
    {
        int count = nodeList.Count;
        int i = 0;
        LogicNode node = null;
        for (i = 0; i < count; i++)
        {
            node = nodeList[i];
            if (node.IsValid) node.OnUpdate();
        }

        count = logicList.Count;
        for (i = 0; i < count; i++)
        {
            if (logicList[i].IsValid)
                logicList[i].OnUpdate();
        }
    }
    #endregion
}
