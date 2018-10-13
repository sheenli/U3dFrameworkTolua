#if usetolua
using LuaInterface;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : EventDispatcherNode
{
    /// <summary>
    /// 所有数据层
    /// </summary>
    private List<IMode> mModes = new List<IMode>();

    public static GameMode Instance
    {
        get;
        private set;
    }

    /// <summary>
    /// 是否是审核模式
    /// </summary>
    public bool AuditMode = false;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 登录的到时候要发送的消息
    /// </summary>
    private List<IMode> loginMsgs = new List<IMode>();

    /// <summary>
    /// 添加一个mode
    /// </summary>
    /// <param name="mode"></param>
    public void AddMode(IMode mode)
    {
        mModes.Add(mode);
    }

    public void InitDtata()
    {
        mIsLoginSendingFlag = false;
        foreach (IMode mode in mModes)
        {
            mode.OnInitData();
        }
    }

    /// <summary>
    /// 正在发送登录消息
    /// </summary>
    private bool mIsLoginSendingFlag = false;
    

    public int SendLoginMsgs()
    {
        mIsLoginSendingFlag = true;
        this.loginMsgs.Clear();
        foreach (IMode mode in mModes)
        {
            this.loginMsgs.Add(mode);
            mode.OnLogin();
        }
        return loginMsgs.Count;
    }

    public void ClearData()
    {
        foreach (IMode mode in this.mModes)
        {
            mode.OnClear();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mIsLoginSendingFlag)
        {
            if (this.loginMsgs.Count > 0)
            {
                for (int i = 0; i < this.loginMsgs.Count; i++)
                {
                    if (this.loginMsgs[i].IsLoginDataOk)
                    {
                        this.loginMsgs.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                QueueEvent(EventDef.SendLoginCompleted);
                this.mIsLoginSendingFlag = false;
            }
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        foreach (IMode mode in this.mModes)
        {
            mode.OnDestroy();
        }
        this.mModes.Clear();
        ClearData();
    }
}

public interface IMode
{
    void OnInitData();
    void OnLogin();
    void OnClear();
    void OnDestroy();

    bool IsLoginDataOk { set; get; }
}

public class BaseMode : IMode
{
    public InterchangeableEventListenerMgr eventMgr;

    public BaseMode()
    {
        eventMgr = new InterchangeableEventListenerMgr(this.OnHandler,99);
    }

    public bool IsLoginDataOk { set; get; }

    public virtual void OnClear()
    {
    }

    public virtual void OnDestroy()
    {
        eventMgr.RemoveAll();
    }

    public virtual void OnInitData()
    {
        IsLoginDataOk = false;
    }

    public virtual void OnLogin()
    {
        IsLoginDataOk = true;
    }

    protected virtual void OnHandler(EventData ev)
    {

    }
}
