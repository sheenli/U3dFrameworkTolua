using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 解压任务
/// </summary>
public class DecompressionTask : ITask
{
    public ABInfo downLoadInfo = null;
    public DecompressionTask(ABInfo info)
    {
        downLoadInfo = info;
        mFileName = downLoadInfo.fileName;
    }
    public string mFileName = "";
    public bool IsFailure
    {
        get;
        private set;
    }

    public bool IsFinished { get; private set; }

    public string FailureInfo()
    {
        return string.Format("解压 {0} 文件失败", mFileName);
    }

    public void OnExecute()
    {
        Rest();
        SceneMgr.Instance.StartCoroutine(Start());
    }

    public IEnumerator Start()
    {
        Debug.Log("开始下载 " + mFileName);
        WWW www = new WWW(AppConst.SourceResPathUrl + "/" + mFileName);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("下载文件失败 " + www.error);
            IsFailure = true;
        }
        else
        {
            if (!Directory.Exists(AppConst.AppExternalDataPath))
            {
                Directory.CreateDirectory(AppConst.AppExternalDataPath);
            }
            CompressHelper.DecompressBytesLZMA(www.bytes, AppConst.AppExternalDataPath + "/" + mFileName);

            Debug.LogWarning("解压完成" + mFileName);
            yield return 10;
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
        return "正在解压资源此过程不消耗流量";
    }
}
