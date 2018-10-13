using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelTask : TaskBase
{
    public ParallelTask(bool failureStop, Action finished, Action<string, string> failure)
        : base(failureStop, finished, failure)
    {
    }

    public override void OnExecute()
    {
        base.OnExecute();
        base.currentTaskName = "正在加载资源";
        if (mTasks.Count > 0)
        {
            foreach (ITask task in mTasks)
            {
                task.Rest();
                task.OnExecute();
            }
        }
        else
        {
            Finished();
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        for (int i = 0;i < this.mTasks.Count;i++)
        {
            if (this.mTasks[i].IsFailure || this.mTasks[i].IsFinished)
            {
                
                if (this.mTasks[i].IsFailure && base.mFailureStop)
                {
                    this.Failureed(this.mTasks[i].TaskName(), this.mTasks[i].FailureInfo());
                }
                else
                {
                    if (taskItemFinished != null)
                        taskItemFinished(mTasks[i]);
                    this.mTasks.RemoveAt(i);
                    i--;
                }
            }
            progress = ((allStaskCount - mTasks.Count) / (float)allStaskCount) * 100;
        }
        if (this.mTasks.Count == 0)
        {
            this.Finished();
        }
    }
}
