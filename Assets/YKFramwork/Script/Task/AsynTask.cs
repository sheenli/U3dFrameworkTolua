using System;
using UnityEngine;

public class AsynTask : TaskBase
{
    private ITask current;
    public AsynTask(bool failureStop, Action finished, Action<string, string> failure)
        : base(failureStop, finished, failure)
    {
    }

    public override void OnExecute()
    {
        base.OnExecute();
        if (current != null)
        {
            current.Rest();
        }
        if (mTasks.Count > 0)
        {
            current = mTasks[0];
            base.currentTaskName = current.TaskName();
            current.OnExecute();
        }
        else
        {
            Finished();
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (current != null)
        {
            if (current.IsFailure || current.IsFinished)
            {
                if (current.IsFailure && mFailureStop)
                {
                    this.Failureed(current.TaskName(), current.FailureInfo());
                }
                else
                {
                    mTasks.RemoveAt(0);
                    if (taskItemFinished != null)
                    {
                        taskItemFinished(current);
                    }
                    OnExecute();
                    progress = ((allStaskCount - mTasks.Count) / (float)allStaskCount) * 100;
                }
            }
        }
    }
}