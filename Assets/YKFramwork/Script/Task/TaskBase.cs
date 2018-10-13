using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TaskBase : IDisposable
{
    /// <summary>
    /// 正在加载
    /// </summary>
    public string currentTaskName = "正在加载...";
    public TaskBase(bool failureStop, Action finished, Action<string, string> failure)
    {
        mFailureStop = failureStop;
        mFailure = failure;
        mFinished = finished;
        FairyGUI.Timers.inst.AddUpdate(this.Update, this);
    }

    protected bool mFailureStop = true;
    public float progress = 0;
    public int allStaskCount = 0;
    protected List<ITask> mTasks = new List<ITask>();
    public bool isFinished = false;
    public bool IsValid
    {
        get
        {
            return mIsRuning;
        }
    }

    public Action<ITask> taskItemFinished = null;
    public Action mFinished = null;
    public Action<string, string> mFailure = null;
    protected bool mIsRuning = false;
    public void Stop()
    {
        mIsRuning = false;
        FairyGUI.Timers.inst.Remove(this.Update);
    }

    public virtual void AddTask(ITask task)
    {
        if (!mTasks.Contains(task))
        {
            mTasks.Add(task);
        }
        allStaskCount = mTasks.Count;
    }

    public bool HasTask<T>() where T : ITask
    {
        foreach (ITask task in mTasks)
        {
            if (task is T && (!task.IsFinished && !task.IsFailure))
            {
                return true;
            }
        }
        return false;
    }

    public virtual void OnExecute()
    {
        mIsRuning = true;
        isFinished = false;
    }

    private void Update(object param)
    {
        if (param == null || !mIsRuning)
        {
            return;
        }
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }

    protected void Finished()
    {
        isFinished = true;
        FairyGUI.Timers.inst.Remove(this.Update);
        mIsRuning = false;
        progress = 100;
        if (mFinished != null) mFinished();
    }

    protected void Failureed(string taskName, string error)
    {
        if (mFailure != null)
        {
            mFailure(taskName, error);
        }
        mIsRuning = false;
    }

    public void Dispose()
    {
        Stop();
    }
}