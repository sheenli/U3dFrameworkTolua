using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcherNode : LogicNode
{
    /// <summary>
    /// 消息回调委托
    /// </summary>
    /// <param name="data">消息内容</param>
    /// <returns>是否中断消息发送</returns>
    public delegate void EventListenerDele(EventData data);

    /// <summary>
    /// 所有的消息
    /// </summary>
    private Dictionary<string, List<EventInfo>> mEventDic = new Dictionary<string, List<EventInfo>>();


    /// <summary>
    /// 消息对应的信息
    /// </summary>
    private class EventInfo
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public string eventType;

        /// <summary>
        /// 事件派发的优先级
        /// </summary>
        public int priority;

        /// <summary>
        /// 事件回调
        /// </summary>
        public EventListenerDele listener;

        /// <summary>
        /// 只发送一次
        /// </summary>
        public bool dispatchOnce;

        public EventInfo(string type, EventListenerDele _listener, int _priority = 0, bool _dispatchOnce = false)
        {
            this.priority = _priority;
            this.dispatchOnce = _dispatchOnce;
            this.eventType = type;
            this.listener = _listener;
        }
    }

    /// <summary>
    /// 用于在每逻辑帧开始的时候维护消息监听器序列
    /// </summary>
    private struct ListenerPack
    {
        public string eventKey;
        public EventInfo listener;
        public bool addOrRemove;

        public ListenerPack(EventInfo lis, string eKey, bool addOrRe)
        {
            listener = lis;
            eventKey = eKey;
            addOrRemove = addOrRe;
        }
    }

    /// <summary>
    /// 用于保存在逻辑帧开始的时候需要更新的ListenerPack
    /// </summary>
    private Queue m_listenersToUpdate = new Queue();

    /// <summary>
    /// 用于在线程安全状态下防止对当前消息队列的同时读写
    /// </summary>
    protected string evtQueueLock = "lock";

    /// <summary>
    /// 两个消息队列buffer
    /// </summary>
    protected Queue m_eventQueueNow = new Queue();
    protected Queue evtQueueNext = new Queue();

    /// <summary>
    /// 是否存在这个监听函数
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="_listener">回调函数</param>
    /// <returns></returns>
    public bool HasListener(string type, EventListenerDele _listener)
    {
        if (this.mEventDic.ContainsKey(type))
        {
            foreach (EventInfo listener in this.mEventDic[type])
            {
                if (listener.listener == _listener) return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 挂接一个监听eventKey消息的消息监听器
    /// 监听器将于下一逻辑帧执行前挂接
    /// </summary>
    /// <param name="type">消息id</param>
    /// <param name="_listener">回调函数</param>
    /// <param name="_priority">优先级</param>
    /// <param name="_dispatchOnce">是否只接受一次</param>
    public void AttachListener(int type, EventListenerDele _listener, int _priority = 0, bool _dispatchOnce = false)
    {
        AttachListener(type.ToString(), _listener, _priority, _dispatchOnce);
    }

    /// <summary>
    /// 挂接一个监听eventKey消息的消息监听器
    /// 监听器将于下一逻辑帧执行前挂接
    /// </summary>
    /// <param name="type">消息id</param>
    /// <param name="_listener">回调函数</param>
    /// <param name="_priority">优先级</param>
    /// <param name="_dispatchOnce">是否只接受一次</param>
    public void AttachListener(string type, EventListenerDele _listener, int _priority = 0, bool _dispatchOnce = false)
    {
        if (threadSafe)
        {
            lock (m_listenersToUpdate)
            {
                m_listenersToUpdate.Enqueue(new ListenerPack(new EventInfo(type, _listener, _priority, _dispatchOnce), type, true));
            }
        }
        else
        {
            m_listenersToUpdate.Enqueue(new ListenerPack(new EventInfo(type, _listener, _priority, _dispatchOnce), type, true));
        }
    }


    public void DetachListener(int type, EventListenerDele _listener)
    {
        DetachListener(type.ToString(), _listener);
    }

    /// <summary>
    /// 摘除一个监听eventKey消息的消息监听器
    /// 监听器将于下一逻辑帧执行前摘除
    /// </summary>
    /// <param name="listener"></param>
    /// <param name="eventKey"></param>
    public void DetachListener(string type, EventListenerDele _listener)
    {
        if (threadSafe)
        {
            lock (m_listenersToUpdate)
            {
                m_listenersToUpdate.Enqueue(new ListenerPack(new EventInfo(type, _listener), type, false));
            }
        }
        else
        {
            m_listenersToUpdate.Enqueue(new ListenerPack(new EventInfo(type, _listener), type, false));
        }
    }

    /// <summary>
    ///  执行一次针对eventKey的监听器挂接
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="_listener">回调函数</param>
    /// <param name="_priority">优先级</param>
    /// <param name="_dispatchOnce">是否只派发一次</param>
    private void AttachListenerNow(int type, EventListenerDele _listener, int _priority = 0, bool _dispatchOnce = false)
    {
        AttachListenerNow(type.ToString(), _listener, _priority, _dispatchOnce);
    }
    /// <summary>
    ///  执行一次针对eventKey的监听器挂接
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="_listener">回调函数</param>
    /// <param name="_priority">优先级</param>
    /// <param name="_dispatchOnce">是否只派发一次</param>
    private void AttachListenerNow(string type, EventListenerDele _listener, int _priority = 0, bool _dispatchOnce = false)
    {
        if (null == _listener || string.IsNullOrEmpty(type)) return;
        if (!mEventDic.ContainsKey(type)) mEventDic.Add(type, new List<EventInfo>());

        if (this.HasListener(type, _listener))
        {
            Debug.Log("LogicNode, AttachListenerNow: " + _listener + " is already in list for event: " + type.ToString());
            return;
        }

        List<EventInfo> listenerList = mEventDic[type];

        EventInfo ev = new EventInfo(type, _listener, _priority, _dispatchOnce);

        int pos = 0;
        int countListenerList = listenerList.Count;
        for (int n = 0; n < countListenerList; n++)
        {
            if (ev.priority > listenerList[n].priority)
            {
                break;
            }
            pos++;
        }
        listenerList.Insert(pos, ev);
    }

    /// <summary>
    /// 执行一次针对eventKey的监听器摘除
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="_listener">监听函数</param>
    /// <param name="_priority">优先级</param>
    /// <param name="_dispatchOnce">是否只执行一次</param>
    private void DetachListenerNow(string type, EventListenerDele _listener)
    {
        //listener == null is valid due to unexpected GameObject.Destroy
        if (string.IsNullOrEmpty(type) || _listener == null)
        {
            Debug.LogError("回调函数或者 事件类型不能为空");
            return;
        }
        if (!mEventDic.ContainsKey(type))
            return;

        List<EventInfo> listenerList = mEventDic[type];
        EventInfo ev = null;
        foreach (EventInfo ei in listenerList)
        {
            if (ei.listener == _listener)
            {
                ev = ei;
            }
        }
        if (ev != null)
        {
            listenerList.Remove(ev);
        }
    }

    /// <summary>
    /// 更新消息监听器表格
    /// </summary>
    private void UpdateListenerMap()
    {
        if (threadSafe)
        {
            lock (m_listenersToUpdate)
            {
                _UpdateListenerMap();
            }
        }
        else
        {
            _UpdateListenerMap();
        }
    }
    /// <summary>
    /// 更新消息监听器表格的实现
    /// </summary>
    private void _UpdateListenerMap()
    {
        int countListenerPack = m_listenersToUpdate.Count;
        while (countListenerPack != 0)
        {
            ListenerPack pack = (ListenerPack)m_listenersToUpdate.Dequeue();
            countListenerPack--;
            if (pack.addOrRemove)
            {
                AttachListenerNow(pack.eventKey, pack.listener.listener, pack.listener.priority, pack.listener.dispatchOnce);
            }
            else
            {
                DetachListenerNow(pack.eventKey, pack.listener.listener);
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        UpdateListenerMap();
        DispatchEvent();
    }

    #region 派发消息相关
    /// <summary>
    /// 发送无参数消息
    /// </summary>
    /// <param name="key">消息机制</param>
    public void QueueEvent(int key)
    {
        QueueEvent(key.ToString());
    }

    private List<EventData> mEventDataPools = new List<EventData>();

    public T GetEventData<T> () where T : EventData
    {
        if (mEventDataPools.Count > 0)
        {
            for(int i = 0;i< mEventDataPools.Count;i++)
            {
                if (mEventDataPools[i] is T)
                {
                    EventData ev = mEventDataPools[i];
                    mEventDataPools.Remove(ev);
                    return ev as T;
                }
            }
        }
        return Activator.CreateInstance(typeof(T)) as T;
    }
    /// <summary>
    /// 发送无参数消息
    /// </summary>
    /// <param name="key">消息机制</param>
    public void QueueEvent(string key)
    {
        if (mEventDataPools.Count > 0)
        {
        }
        QueueEvent(new EventData(key));
    }

    /// <summary>
    /// 派发消息
    /// </summary>
    /// <typeparam name="T">消息传入的参数</typeparam>
    /// <param name="key">要发送的消息id</param>
    /// <param name="value">要发的消息内容</param>
    public void QueueEvent<T>(int key, T value)
    {
        EventData data = GetEventData<EventData>();
        data.name = key.ToString();
        data.value = value;
        QueueEvent(data);
    }

    /// <summary>
    /// 发送一个默认的int类型值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void QueueEvent(int key, int value)
    {
        EventData data = GetEventData<EventData>();
        data.name = key.ToString();
        data.value = value;
        QueueEvent(data);
    }

    /// <summary>
    /// 发送默认的消息在
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void QueueEvent(int key, string value)
    {
        EventData data = GetEventData<EventData>();
        data.name = key.ToString();
        data.value = value;
        QueueEvent(data);
    }

    /// <summary>
    /// 派发消息
    /// </summary>
    /// <typeparam name="T">消息传入的参数</typeparam>
    /// <param name="key">要发送的消息id</param>
    /// <param name="value">要发的消息内容</param>
    public void QueueEvent<T>(string key, T value)
    {
        EventData data = GetEventData<EventData>();
        data.name = key.ToString();
        data.value = value;
        QueueEvent(data);
    }

    /// <summary>
    /// 线程安全
    /// 抛出消息进入下一逻辑帧
    /// </summary>
    public void QueueEvent(EventData data)
    {
        if (threadSafe)
        {
            lock (evtQueueLock)
            {
                evtQueueNext.Enqueue(data);
            }
        }
        else
        {
            evtQueueNext.Enqueue(data);
        }
    }

    public void QueueEventLua(string key,object data)
    {
        EventDataLua ev = new EventDataLua(key);
        ev.obj = data;
        if (threadSafe)
        {
            lock (evtQueueLock)
            {
                evtQueueNext.Enqueue(ev);
            }
        }
        else
        {
            evtQueueNext.Enqueue(ev);
        }
    }

    /// <summary>
    /// 只能在主线程调用 立即派发消息
    /// </summary>
    /// <param name="key">要发送的消息id</param>
    public void QueueEventNow(string key)
    {
        QueueEventNow(new EventData(key));
    }

    /// <summary>
    /// 只能在主线程调用 立即派发消息
    /// </summary>
    /// <typeparam name="T">消息传入的参数</typeparam>
    /// <param name="key">要发送的消息id</param>
    /// <param name="value">要发的消息内容</param>
    public void QueueEventNow<T>(string key, T value)
    {
        QueueEventNow(new EventData<T>(key, value));
    }

    /// <summary>
    /// 只能在主线程调用 立即发送
    /// </summary>
    /// <param name="data"></param>
    public void QueueEventNow(EventData data)
    {
        if (threadSafe)
        {
            lock (evtQueueLock)
            {
                TriggerEvent(data);
            }
        }
        else
        {
            TriggerEvent(data);
        }
    }


    /// <summary>
    /// 派发消息
    /// </summary>
    private void DispatchEvent()
    {
        if (threadSafe)
        {
            _DispatchEventLock();
        }
        else
        {
            _DispatchEventNoLock();
        }
    }

    private void _DispatchEventLock()
    {
        if (!this.IsValid) return;
        lock (evtQueueLock)
        {
            EventData evt = null;
            //发布自己的event
            int countEventQueue = m_eventQueueNow.Count;
            while (countEventQueue > 0)
            {
                evt = m_eventQueueNow.Dequeue() as EventData;
                countEventQueue--;
                if (evt != null)
                {
                    TriggerEvent(evt);
                }
            }
        }
        lock (evtQueueLock)
        {
            SwapEventQueue();
        }
    }

    private void _DispatchEventNoLock()
    {
        if (!this.IsValid) return;

        EventData evt = null;
        int countEventQueue = m_eventQueueNow.Count;
        while (countEventQueue > 0)
        {
            evt = m_eventQueueNow.Dequeue() as EventData;
            countEventQueue--;
            if (evt != null)
            {
                TriggerEvent(evt);
            }
        }

        //交换event缓冲//
        SwapEventQueue();
    }

    /// <summary>
    /// 交换消息队列
    /// </summary>
    private void SwapEventQueue()
    {
        Queue temp = m_eventQueueNow;
        m_eventQueueNow = evtQueueNext;
        evtQueueNext = temp;
    }

    /// <summary>
    /// 将消息派发给下携的消息监听器
    /// </summary>
    /// <param name="key"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <returns></returns>
    private void TriggerEvent(EventData data)
    {
        if (!mEventDic.ContainsKey(data.name))
        {
            return;
        }

        List<EventInfo> reList = new List<EventInfo>();

        List<EventInfo> listenerList = mEventDic[data.name];
        int countListenerList = listenerList.Count;
        for (int n = 0; n < countListenerList; n++)
        {
            if (listenerList[n].dispatchOnce)
            {
                reList.Add(listenerList[n]);
            }

            if (listenerList[n].listener != null)
            {
                listenerList[n].listener(data);
            }
            if(data.isBreak) break;
        }
        for (int n = reList.Count - 1; n >= 0; n--)
        {
            DetachListenerNow(reList[n].eventType, reList[n].listener);
        }
    }
    #endregion
}

public class EventListenerMgr
{
    public void AddListener(EventDispatcherNode dis, EventDispatcherNode.EventListenerDele dele, int type
       , int _priority = 0, bool _dispatchOnce = false)
    {
        AddListener(dis, dele, type.ToString(), _priority, _dispatchOnce);
    }

    public void AddListener(EventDispatcherNode dis, EventDispatcherNode.EventListenerDele dele, string type
        , int _priority = 0, bool _dispatchOnce = false)
    {
        if (!dis.HasListener(type, dele))
        {
            EventListenerData data = new EventListenerData(dis, dele, type);
            mlistener.Add(data);
            dis.AttachListener(type, dele, _priority, _dispatchOnce);
        }
        else
        {
            Debug.LogWarning("添加消息失败重复添加消息id=" + type);
        }
    }
    public void DetachListener(EventDispatcherNode dis, EventDispatcherNode.EventListenerDele dele,string type)
    {
        if (dis.HasListener(type, dele))
        {
            dis.DetachListener(type, dele);
            for (int i = 0; i < mlistener.Count; i++)
            {
                EventListenerData data = mlistener[i];
                if (data.dis == dis && data.type == type && data.dele == dele)
                {
                    mlistener.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public void DetachListenerAll()
    {
        foreach (EventListenerData data in mlistener)
        {
            data.DetachListener();
        }
        mlistener.Clear();
    }


    private List<EventListenerData> mlistener = new List<EventListenerData>();
    public class EventListenerData
    {
        public EventDispatcherNode dis;
        public EventDispatcherNode.EventListenerDele dele;
        public string type;
        public int priority;
        public bool dispatchOnce;
        public EventListenerData(EventDispatcherNode _dis,
            EventDispatcherNode.EventListenerDele _dele, string _type, 
            int _priority = 0, bool _dispatchOnce = false)
        {
            dis = _dis;
            dele = _dele;
            type = _type;
            priority = _priority;
            dispatchOnce = _dispatchOnce;
        }

        public void DetachListener()
        {
            dis.DetachListener(type, dele);
        }
    }
}

public class InterchangeableEventListenerMgr
{
    private EventListenerMgr mEventMgr = new EventListenerMgr();

    private EventListenerMgr.EventListenerData mDefdele = null;
    private EventListenerMgr.EventListenerData mNetworkDele = null;
    private EventListenerMgr.EventListenerData mSceneDele = null;
    private EventListenerMgr.EventListenerData mUIDele = null;
    private EventListenerMgr.EventListenerData mModeDele = null;
    public InterchangeableEventListenerMgr(EventDispatcherNode.EventListenerDele callback,int _priority = 0)
    {
        mDefdele = new EventListenerMgr.EventListenerData(null, callback, null, _priority);

        mNetworkDele = new EventListenerMgr.EventListenerData(null, callback, null, _priority);
        mSceneDele = new EventListenerMgr.EventListenerData(null, callback, null, _priority);
        mUIDele = new EventListenerMgr.EventListenerData(null, callback, null, _priority);
        mModeDele = new EventListenerMgr.EventListenerData(null, callback, null, _priority);
    }

    public InterchangeableEventListenerMgr SetDefCallback(EventDispatcherNode.EventListenerDele callback,
        int _priority = 0, bool _dispatchOnce = false)
    {
        mDefdele.dele = callback;
        mDefdele.priority = _priority;
        mDefdele.dispatchOnce = _dispatchOnce;
        return this;
    }

    public InterchangeableEventListenerMgr SetNetworkCallback(EventDispatcherNode.EventListenerDele callback,
        int _priority = 0, bool _dispatchOnce = false)
    {
        mNetworkDele.dele = callback;
        mNetworkDele.priority = _priority;
        mNetworkDele.dispatchOnce = _dispatchOnce;
        return this;
    }

    public InterchangeableEventListenerMgr SetSceneCallback(EventDispatcherNode.EventListenerDele callback,
        int _priority = 0, bool _dispatchOnce = false)
    {
        mSceneDele.dele = callback;
        mSceneDele.priority = _priority;
        mSceneDele.dispatchOnce = _dispatchOnce;
        return this;
    }

    public InterchangeableEventListenerMgr SetUICallback(EventDispatcherNode.EventListenerDele callback,
       int _priority = 0, bool _dispatchOnce = false)
    {
        mUIDele.dele = callback;
        mUIDele.priority = _priority;
        mUIDele.dispatchOnce = _dispatchOnce;
        return this;
    }

    public InterchangeableEventListenerMgr SetModeCallback(EventDispatcherNode.EventListenerDele callback,
      int _priority = 0, bool _dispatchOnce = false)
    {
        mModeDele.dele = callback;
        mModeDele.priority = _priority;
        mModeDele.dispatchOnce = _dispatchOnce;
        return this;
    }


    #region 添加移除网络消息
    public void AddNetEvent(string type, EventDispatcherNode.EventListenerDele callback = null,
          int _priority = -1, bool _dispatchOnce = false)
    {
        if (callback == null)
        {
            callback = this.mNetworkDele.dele;
        }
        if (_priority == -1)
        {
            _priority = this.mNetworkDele.priority;
        }
        mEventMgr.AddListener(Net.Instance, callback, type, _priority, _dispatchOnce);
    }

    public void RemoveNetEvent(string type, EventDispatcherNode.EventListenerDele callback = null)
    {
        if (callback == null)
        {
            callback = this.mNetworkDele.dele;
        }
        mEventMgr.DetachListener(Net.Instance, callback, type);
    }
    #endregion

    #region 添加移除UI消息
    public void AddUIEvent(string type, EventDispatcherNode.EventListenerDele callback = null,
      int _priority = -1, bool _dispatchOnce = false)
    {
        if (callback == null)
        {
            callback = this.mUIDele.dele;
        }
        if (_priority == -1)
        {
            _priority = this.mUIDele.priority;
        }
        mEventMgr.AddListener(UIMgr.Instance, callback, type, _priority, _dispatchOnce);
    }

    public void RemoveUIEvent(string type, EventDispatcherNode.EventListenerDele callback = null)
    {
        if (callback == null)
        {
            callback = this.mUIDele.dele;
        }
        mEventMgr.DetachListener(Net.Instance, callback, type);
    }
    #endregion

    #region 添加移除场景消息
    public void AddSceneEvent(string type, EventDispatcherNode.EventListenerDele callback = null,
      int _priority = -1, bool _dispatchOnce = false)
    {
        if (callback == null)
        {
            callback = this.mSceneDele.dele;
        }
        if (_priority == -1)
        {
            _priority = this.mSceneDele.priority;
        }
        mEventMgr.AddListener(SceneMgr.Instance, callback, type, _priority, _dispatchOnce);
    }

    public void RemoveSceneEvent(string type, EventDispatcherNode.EventListenerDele callback = null)
    {
        if (callback == null)
        {
            callback = this.mSceneDele.dele;
        }
        mEventMgr.DetachListener(SceneMgr.Instance, callback, type);
    }
    #endregion

    #region 添加移除场景消息
    public void AddModeEvent(string type, EventDispatcherNode.EventListenerDele callback = null,
      int _priority = -1, bool _dispatchOnce = false)
    {
        if (callback == null)
        {
            callback = this.mModeDele.dele;
        }
        if (_priority == -1)
        {
            _priority = this.mModeDele.priority;
        }
        mEventMgr.AddListener(GameMode.Instance, callback, type, _priority, _dispatchOnce);
    }

    public void RemoveModeEvent(string type, EventDispatcherNode.EventListenerDele callback = null)
    {
        if (callback == null)
        {
            callback = this.mModeDele.dele;
        }
        mEventMgr.DetachListener(GameMode.Instance, callback, type);
    }
    #endregion

    #region 添加移除其他消息
    public void AddEvent(EventDispatcherNode dis, string type, EventDispatcherNode.EventListenerDele callback = null,
      int _priority = -1, bool _dispatchOnce = false)
    {
        if (callback == null)
        {
            callback = this.mDefdele.dele;
        }
        if (_priority == -1)
        {
            _priority = this.mDefdele.priority;
        }
        mEventMgr.AddListener(dis, callback, type, _priority, _dispatchOnce);
    }

    public void RemoveEvent(EventDispatcherNode dis,string type, EventDispatcherNode.EventListenerDele callback = null)
    {
        if (callback == null)
        {
            callback = this.mModeDele.dele;
        }
        mEventMgr.DetachListener(dis, callback, type);
    }
    #endregion

    public void RemoveAll()
    {
        this.mEventMgr.DetachListenerAll();
    }
}


