using System;
using UnityEngine;

public class DownAPKTask : TaskBase
{
    private string mUrl = "";
    private string mSaveFile = "";
    private WWW mwww = null;
    public DownAPKTask(string url,string save,bool failureStop, Action finished, Action<string, string> failure)
        : base(failureStop, finished, failure)
    {
        mUrl = url;
        mSaveFile = save;
    }

    public override void OnExecute()
    {
        if (mwww != null)
        {
            mwww.Dispose();
        }
        currentTaskName = "正在下载APK文件请耐心等待...";
        mwww = new WWW(mUrl);
        base.OnExecute();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (mwww != null)
        {
            if (!string.IsNullOrEmpty(mwww.error))
            {
                Debug.LogError("mwww.error:" + mUrl);
                base.Failureed("下载APK", mwww.error);
            }
            else
            {
                if (mwww.isDone)
                {
                    System.IO.File.WriteAllBytes(mSaveFile, mwww.bytes);
                    mwww.Dispose();
                    progress = 100;
                    base.Finished();
                }
                else
                {
                    progress = mwww.progress * 100;
                }
            }
        }
    }
}