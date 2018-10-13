using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 下载文件任务
/// </summary>
public class DownLoadTask : ITask
{
    public ABInfo downInfo = null;
    private string savePath = "";
    public DownLoadTask(ABInfo fileName)
    {
        this.downInfo = fileName;
        savePath = AppConst.AppExternalDataPath + "/" + fileName.fileName;
    }


    public bool IsFailure
    {
        get;
        private set;
    }

    public bool IsFinished
    {
        get;
        private set;
    }

    public string FailureInfo()
    {
        return string.Format("下载{0}资源失败", downInfo.fileName);
    }

    public void OnExecute()
    {
        //SceneMgr.Instance.StopCoroutine("StartDown");
        SceneMgr.Instance.StartCoroutine(this.StartDown());
    }

    public IEnumerator StartDown()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
        if (!Directory.Exists(AppConst.AppExternalDataPath))
        {
            Directory.CreateDirectory(AppConst.AppExternalDataPath);
        }
        WWW www = new WWW(HotUpdateRessMgr.Instance.downLoadInfo.ResUrl + "/" + downInfo.fileName);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            IsFailure = true;
            Debug.LogErrorFormat("下载文件 {0} 出错:{1}", downInfo.fileName, www.error);
        }
        else
        {
            byte[] bytes = www.bytes;
            CompressHelper.DecompressBytesLZMA(bytes, savePath);
            IsFinished = true;
        }
        www.Dispose();
    }

    public void Rest()
    {
        IsFailure = false;
        IsFinished = false;
    }

    public string TaskName()
    {
        return "正在下载资源请耐心等待";
    }
}
