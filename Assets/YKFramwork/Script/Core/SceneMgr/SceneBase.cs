using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneBase
{
    public SceneBase()
    {
        eventMgr = new InterchangeableEventListenerMgr(this.OnHandler,2);
    }

    public InterchangeableEventListenerMgr eventMgr;
    public bool isLoadingShowWait = false;
    /// <summary>
    /// 场景需要加载的资源组
    /// </summary>
    private List<string> mLoadGroups = new List<string>();

    public virtual string SceneName { set; get; }

    /// <summary>
    /// 场景本身的任务
    /// </summary>
    public AsynTask sceneTask;

    /// <summary>
    /// 场景加载任务
    /// </summary>
    public AsynTask parallelTask;

    /// <summary>
    /// 添加任务到场景
    /// </summary>
    /// <param name="task"></param>
    public void AddTask(ITask task)
    {
        this.sceneTask.AddTask(task);
    }

    /// <summary>
    /// 添加一个平行任务
    /// </summary>
    /// <param name="task"></param>
    public void AddParallelTask(ITask task)
    {
        this.parallelTask.AddTask(task);
    }

    /// <summary>
    /// 添加一个需要加载的资源组
    /// </summary>
    /// <param name="gropName"></param>
    public void AddLoadGrop(string gropName)
    {
        if (string.IsNullOrEmpty(gropName))
        {
            Debug.LogError("资源组名称不能为空");
        }
        else
        {
            if (mLoadGroups.Contains(gropName))
            {
                Debug.LogError("已经存在这个资源组");
            }
            else
            {
                mLoadGroups.Add(gropName);
            }
        }
    }

    /// <summary>
    /// 加载场景资源完成
    /// </summary>
    public void Loaded()
    {
        OnResLoaded();
    }

    private bool mLoaded = false;
    private object mParam;
    /// <summary>
    /// 在进入场景的时候要做的事情
    /// </summary>
    public void Enter(object param)
    {
        ResMgr.Intstance.PushRes();
        mLoaded = false;
        
        if (sceneTask != null)
        {
			sceneTask.Stop ();
		}

        if (parallelTask != null)
        {
            parallelTask.Stop();
        }
       
        sceneTask = new AsynTask(true, this.OnFinished, OnTaskFailure);
        parallelTask = new AsynTask(true,this.OnFinished,OnTaskFailure);
       
        this.mParam = param;
        this.mLoadGroups.Clear();
        if (!string.IsNullOrEmpty(GroupName))
        {
            this.mLoadGroups.Add(GroupName);
        }
        this.OnInit(this.mParam);

        List<TaskBase> list = new List<TaskBase>();
        if (sceneTask != null)
        {
            list.Add(sceneTask);
        }

        if (parallelTask != null)
        {
            list.Add(parallelTask);
        }
        foreach (string gropname in this.mLoadGroups)
        {
            AddParallelTask(new SceneLoadTask(gropname, false));
        }
        if (isLoadingShowWait)
        {
            if (SceneMgr.Instance.OpenLoadUI != null)
            {
                SceneMgr.Instance.OpenLoadUI(list);
            }
        }
        else
        {
            FairyGUI.GRoot.inst.ShowModalWait();
        }
       
        Action<ITask> loaditem = a =>
        {
            if (!parallelTask.HasTask<SceneLoadTask>() && !mLoaded && !sceneTask.HasTask<SceneLoadTask>())
            {
                mLoaded = true;
                Loaded();
            }
        };
        parallelTask.taskItemFinished = loaditem;
        sceneTask.taskItemFinished = loaditem;
        parallelTask.OnExecute();
        sceneTask.OnExecute();       
    }

    public void OnFinished()
    {
        if ((sceneTask == null ||sceneTask.isFinished)
            && (parallelTask == null || parallelTask.isFinished))
        {
            OnEnter();
            //Debug.LogError("场景加载完成");
            SceneMgr.Instance.QueueEvent(SceneMgr.ChangeSceneFinished);
        }
    }

    /// <summary>
    /// 离开场景要做的事情
    /// </summary>
    public void Leave()
    {
        OnLeave();
    }

    public void ResStartTask()
    {
        if (this.sceneTask != null)
        {
            this.sceneTask.OnExecute();
        }

        if (this.parallelTask != null)
        {
            this.parallelTask.OnExecute();
        }
    }

    public virtual void OnDestroy()
    {
        if (eventMgr != null)
        {
            eventMgr.RemoveAll();
        }
    }

    #region 外部实现接口
    /// <summary>
    /// 初始化场景
    /// </summary>
    /// <param name="param"></param>
    protected virtual void OnInit(object param) { }


    /// <summary>
    /// 进入场景
    /// </summary>
    protected virtual void OnEnter() { }

    /// <summary>
    /// 离开场景
    /// </summary>
    protected virtual void OnLeave() { }

    /// <summary>
    /// 消息监听
    /// </summary>
    protected virtual void OnHandler(EventData ev) { }

    /// <summary>
    /// 场景名称
    /// </summary>
    public virtual string GroupName
    {
        get;
        set;
    }

    /// <summary>
    /// 加载完成后的回调
    /// </summary>
    protected virtual void OnResLoaded()
    {

    }

    protected virtual void OnTaskFailure(string taskName, string error)
    {

    }
    #endregion
}

public class SceneLoadTask :  ITask
{
    private ISceneLoad loadType = null;
    private bool mIsGrop = false;
    public SceneLoadTask(string assetName, bool _isKeepInMemory) 
    {
        ResGroupCfg cfg = GameCfgMgr.Instance.GetGroupInfo(assetName);
        mIsGrop = cfg != null;
        loadType = mIsGrop ? new LoadGroup(cfg) : new AutoLoadAsset(assetName, _isKeepInMemory) as ISceneLoad;
    }

    public bool IsFailure
    {
        get { return loadType != null && loadType.loaded == -1; }
    }

    public bool IsFinished
    {
        get { return loadType != null && loadType.loaded == 1; }
    }

    public string FailureInfo()
    {
        return string.Format("{0}资源加载失败", loadType.fileName);
    }

    public void OnExecute()
    {
        
        Rest();
        loadType.Load(null);
    }

    public void Rest()
    {
        loadType.loaded = 0;
    }

    public string TaskName()
    {
        return "资源加载中...";
    }
}
