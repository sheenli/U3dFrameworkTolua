using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HotUpdateRessMgr
{
    /// <summary>
    /// 本地版本信息
    /// </summary>
    public VerInfo verInfo = null;

    private static HotUpdateRessMgr mIns;
    public static HotUpdateRessMgr Instance
    {
        get
        {
            return mIns = mIns ?? new HotUpdateRessMgr();
        }
    }
  

    public RemotelyVersionInfo.RemotelyInfo downLoadInfo;

    public void Init(Action callBack)
    {
        Debug.LogWarning("准备获取下载网址");
        RemotelyVersionInfo allinfo = null;

        //string allverinfoName = LocalGameCfg.IsPublic ? "allverinfo.json" : "allverinfoTest.json";

        string url = GameCfgMgr.Instance.localGameCfg.RemotelyResUrl;
        ComUtil.WWWLoad(url, a =>
        {
            if (a != null && string.IsNullOrEmpty(a.error))
            {
                try
                {
                    allinfo = JsonUtility.FromJson<RemotelyVersionInfo>(a.text);
                    Debug.LogWarning("远程版本信息" + a.text);
                    if (allinfo.vers.Count > GameCfgMgr.Instance.localGameCfg.chanelType)
                    {
                        downLoadInfo = allinfo.vers[GameCfgMgr.Instance.localGameCfg.chanelType];
                    }
                }
                catch (Exception)
                {
                    Debug.LogError("请求下载网址出错 错误：" + a.text);
                }

                Debug.Log("资源下载地址：" + url);
                a.Dispose();
            }
            else
            {
                Debug.LogError("请求下载网址出错 错误：" + url);
            }
            RefreshLocalVerInfo(callBack);
        });
    }

    public void RefreshLocalVerInfo(Action callBack)
    {
        string loaclFileName = AppConst.AppExternalDataPath + "/" + GameCfgMgr.Instance.localGameCfg.CollectionIDVerFileName;
        if (File.Exists(loaclFileName))
        {
            string content = File.ReadAllText(loaclFileName);
            try
            {

                verInfo = JsonUtility.FromJson<VerInfo>(content);
                Debug.LogWarning("本地有配置信息 版本为：" + verInfo.ver);
                verInfo.verName = Path.GetFileNameWithoutExtension(GameCfgMgr.Instance.localGameCfg.CollectionIDVerFileName);
                callBack();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        else
        {
            verInfo = new VerInfo();
            verInfo.ver = "0.0.0";
            verInfo.verName = Path.GetFileNameWithoutExtension(GameCfgMgr.Instance.localGameCfg.CollectionIDVerFileName);
            if (callBack != null)
            {
                callBack();
            }
        }
    }

    #region 检查要下载的文件
    public void GetDownList(VerInfo localVer, Action<DecompressionOrDownInfo> callBack)
    {
        

        if (downLoadInfo == null)
        {
            DecompressionOrDownInfo downInfo = new DecompressionOrDownInfo();
            downInfo.localVer = localVer;
            downInfo.remoteVer = null;
            downInfo.comparisonInfo = VerInfo.ComparisonVer(localVer, downInfo.remoteVer);
            if (callBack != null)
            {
                callBack(downInfo);
            }
            return;
        }
        string remoturl = downLoadInfo.ResUrl + "/" + localVer.verName + ".txt";
        VerInfo remoteVer = null;

        Debug.LogWarning("准备校验远程文件：" + remoturl);
        Action<WWW> wwwed = www =>
        {
            if (www != null && string.IsNullOrEmpty(www.error))
            {
                try
                {
                    remoteVer = JsonUtility.FromJson<VerInfo>(www.text);

                }
                catch (System.Exception ex)
                {
                    Debug.LogError("远程文件解析失败：text=" + www.text);
                }
            }
            else
            {
                Debug.LogError("从远程下载文件失败：url=" + remoturl);
            }

            ComparisonInfo info = VerInfo.ComparisonVer(localVer, remoteVer);
            if (localVer != null)
            {
                Debug.LogWarning("检查完成 本地版本：" + localVer.ver);
            }
            if (remoteVer != null)
            {
                Debug.LogWarning("检查完成 远程版本：" + remoteVer.ver);
            }
            DecompressionOrDownInfo dord = new DecompressionOrDownInfo();
            dord.remoteVer = remoteVer;
            dord.localVer = localVer;
            dord.comparisonInfo = info;
            if (callBack != null)
            {
                callBack(dord);
            }
            www.Dispose();
        };

        Debug.LogWarning("从远程下载文件" + remoturl);
        ComUtil.WWWLoad(remoturl, wwwed);
    }
    #endregion

    /// <summary>
    /// 获取要解压的文件
    /// </summary>
    /// <param name="verTextName"></param>
    /// <param name="callBack"></param>
    public void GetDecompressionTaskList(VerInfo localVer, Action<DecompressionOrDownInfo> callBack)
    {
        string sourceUrl = AppConst.SourceResPathUrl + "/" + localVer.verName + ".txt";
        VerInfo remoteVer = null;

        Action<WWW> loadlocaled = www =>
        {
            if (www != null && string.IsNullOrEmpty(www.error))
            {
                try
                {
                    remoteVer = JsonUtility.FromJson<VerInfo>(www.text);
                }
                catch (Exception)
                {
                    Debug.LogError("本地没有这个文件:" + sourceUrl);
                }
            }
            else
            {
                Debug.LogError("本地没有这个文件:" + sourceUrl);
            }
            ComparisonInfo info = VerInfo.ComparisonVer(localVer, remoteVer);
            DecompressionOrDownInfo dord = new DecompressionOrDownInfo();
            dord.remoteVer = remoteVer;
            dord.localVer = localVer;
            dord.comparisonInfo = info;
            if (callBack != null)
            {
                callBack(dord);
            }
        };
        ComUtil.WWWLoad(sourceUrl, loadlocaled);
    }

    public class DecompressionOrDownInfo
    {
        public ComparisonInfo comparisonInfo;
        public VerInfo remoteVer;
        public VerInfo localVer;
    }

    public void Save(DecompressionOrDownInfo info,bool saveVer)
    {
        if (!Directory.Exists(AppConst.AppExternalDataPath))
        {
            Directory.CreateDirectory(AppConst.AppExternalDataPath);
        }
        string saveFilePath = AppConst.AppExternalDataPath + "/" + info.localVer.verName + ".txt";
        if (!saveVer)
        {
            if (info.comparisonInfo.flag != 0)
            {
                info.localVer.ver = info.remoteVer.ver;
            }
        }
        File.WriteAllText(saveFilePath, info.localVer.ToString());
    }
}
[System.Serializable]
public class RemotelyVersionInfo
{
    /// <summary>
    /// 版本信息
    /// </summary>
    public List<RemotelyInfo> vers = new List<RemotelyInfo>();

    [System.Serializable]
    public class RemotelyInfo
    {
        /// <summary>
        /// 更新信息
        /// </summary>
        public string notic;

        /// <summary>
        /// 版本信息
        /// </summary>
        public string version;

        /// <summary>
        /// 资源路径
        /// </summary>
        public string ResUrl
        {
            get
            {
                string root = GameCfgMgr.Instance.localGameCfg.OSSResRootPath + "/"+version;
#if UNITY_EDITOR
                root += "/pc";
#elif UNITY_ANDROID
                root += "/and";
#elif UNITY_IOS
                root += "/ios";
#else
                root += "/pc";
#endif
                return root;
            }
        }

        /// <summary>
        /// 完整包的路径
        /// </summary>
        public string FullPackUrl
        {
            get
            {
                string root = ResUrl;
#if UNITY_EDITOR
                root += "/1.apk";
#elif UNITY_ANDROID
                root += "/1.apk";
#elif UNITY_IOS
                root ="itms-services://?action=download-manifest&url="+root+"/Install.plist";
#else
                root += "/1.apk";
#endif
                return root;
            }
        }
    }
}