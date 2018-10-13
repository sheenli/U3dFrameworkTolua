using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EntranceScene : SceneBase
{
    private List<string> mNeedLoadLuaAb = new List<string>()
    {
        "ToLua",
        "YKFramwork",
    };
    protected override void OnEnter()
    {
        base.OnEnter();
        this.eventMgr.AddSceneEvent(AppConst.CoreDef.INITHOTUPDATA);
        this.eventMgr.AddSceneEvent(AppConst.CoreDef.INSTALLATIONED);
        this.eventMgr.AddSceneEvent(AppConst.CoreDef.DOWNED);
        this.eventMgr.AddSceneEvent(AppConst.CoreDef.LoadLuaFinished);
        this.eventMgr.AddSceneEvent(AppConst.CoreDef.StartGame);
        LoadingUI.Instance.Show(() => 
        {
            InitHotFixed();
        });
    }


    private void InitHotFixed()
    {
        HotUpdateRessMgr.Instance.Init(() =>
                {
                    SceneMgr.Instance.QueueEvent(AppConst.CoreDef.INITHOTUPDATA);
                });
    }

    protected override void OnHandler(EventData ev)
    {
        switch (ev.name)
        {
            case AppConst.CoreDef.INITHOTUPDATA:
                InstallationApp();
                break;
            case AppConst.CoreDef.INSTALLATIONED:
                DownLoadFile();
                break;
            case AppConst.CoreDef.DOWNED:
                LoadLua();
                break;
            case AppConst.CoreDef.LoadLuaFinished:
                //StartGame();
                break;
            case AppConst.CoreDef.StartGame:
                StartGame();
                break;
        }
    }


    #region 安装游戏
    /// <summary>
    /// 安装游戏
    /// </summary>
    private void InstallationApp()
    {
        Debug.LogWarning("开始检查安装");
        Action<HotUpdateRessMgr.DecompressionOrDownInfo> checkFileed = a =>
        {
            AsynTask task = new AsynTask(true, () =>
            {
                HotUpdateRessMgr.Instance.Save(a, true);
                HotUpdateRessMgr.Instance.RefreshLocalVerInfo(() =>
                {
                    SceneMgr.Instance.QueueEvent(AppConst.CoreDef.INSTALLATIONED);
                });
            }, this.InstallationFailue);
            task.taskItemFinished = (it) =>
            {
                DecompressionTask dt = it as DecompressionTask;
                ABInfo abDown = dt.downLoadInfo;
                a.localVer.Save(abDown);
                HotUpdateRessMgr.Instance.Save(a, false);
            };
            Debug.Log("检查完成需要安装的文件个数为：" + a.comparisonInfo.needDecompressionList.Count);
            foreach (ABInfo ab in a.comparisonInfo.needDecompressionList)
            {
                task.AddTask(new DecompressionTask(ab));
            }
            task.OnExecute();
        };
        HotUpdateRessMgr.Instance.GetDecompressionTaskList(HotUpdateRessMgr.Instance.verInfo, checkFileed);
    }
    private void InstallationFailue(string arg1, string arg2)
    {
        Debug.LogError("安装文件失败：" + arg1);

    }
    #endregion

    #region 下载文件
    private void DownLoadFile()
    {
        Action<HotUpdateRessMgr.DecompressionOrDownInfo> checkFileed = downloadInfo =>
        {
            Debug.Log("检查需要下载的文件个数:"+ downloadInfo.comparisonInfo.flag+"个数为："+ downloadInfo.comparisonInfo.needDecompressionList.Count);
            if (downloadInfo.comparisonInfo.flag == 1)
            {
                DeleteAllKey(true);
                Application.OpenURL(HotUpdateRessMgr.Instance.downLoadInfo.FullPackUrl);
                //TODO:下载整包
                //                 if (Application.platform == RuntimePlatform.IPhonePlayer)
                //                 {
                //                     Application.OpenURL(HotUpdateRessMgr.Instance.downLoadInfo.FullPackUrl);
                //                     Application.Quit();
                //                 }
                //                 else
                //                 {
                //                     DownLoadApk();
                //                 }
            }
            else
            {
                DeleteAllKey(false);
                AsynTask task = new AsynTask(true, () =>
                 {
                     HotUpdateRessMgr.Instance.Save(downloadInfo, true);
                     SceneMgr.Instance.QueueEvent(AppConst.CoreDef.DOWNED);
                 }, DownLoadFailue);

                task.taskItemFinished = it =>
                {
                    DownLoadTask dt = it as DownLoadTask;
                    ABInfo downab = dt.downInfo;
                    downloadInfo.localVer.Save(downab);
                    HotUpdateRessMgr.Instance.Save(downloadInfo, false);

                    Debug.Log("开始下载完成：" + downab.fileName);
                };

                foreach (ABInfo ab in downloadInfo.comparisonInfo.needDecompressionList)
                {
                    task.AddTask(new DownLoadTask(ab));
                }
                task.OnExecute();
            }
        };

        HotUpdateRessMgr.Instance.GetDownList(HotUpdateRessMgr.Instance.verInfo, checkFileed);
    }

    public void DownLoadApk()
    {

    }

    public void DownLoadFailue(string arg1, string arg2)
    {
        Debug.LogError("下载文件失败：" + arg1);
    }
    #endregion

    private void LoadLua()
    {
        if (HotUpdateRessMgr.Instance.verInfo.ver!= "0.0.0")
        {
            GameCfgMgr.Instance.localGameCfg.version = HotUpdateRessMgr.Instance.verInfo.ver;
        }
        
        AsynTask task = new AsynTask(false, () =>
        {
            Debug.LogWarning("加载lua完成");
            SceneMgr.Instance.QueueEvent(AppConst.CoreDef.LoadLuaFinished);
        }, (a, b) =>
        {
            Debug.LogError("加载lua失败");
        });

        foreach (string str in mNeedLoadLuaAb)
        {
            task.AddTask(new LoadLuaABTask(str));
        }
        task.OnExecute();
    }

    public void StartGame()
    {
        /*LoadingUI.Instance.Destroy();*/
        Debug.LogWarning("开始游戏");
        GameCfgMgr.Instance.Init();
        LuaMgr.Instance.Init();
        LuaMgr.Instance.StartGame();
    }

    #region 删除缓存文件
    public void DeleteAllKey(bool delAllFile = false)
    {
        try
        {
            if (delAllFile)
            {
                string path = Application.temporaryCachePath;
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                path = Application.persistentDataPath;
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                path = Path.GetFullPath(Application.persistentDataPath + "/../");
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
        }
        catch (Exception)
        {
        }



        PlayerPrefs.DeleteAll();
        //ReadBuildInfo();
    }
    #endregion
}
public class LoadLuaABTask : ITask
{
    public AssetBundle ab = null;
    public string abName = "";
    public LoadLuaABTask(string _abName)
    {
        this.abName = _abName;
    }

    private bool mIsFailure = false;
    bool ITask.IsFailure
    {
        get { return mIsFailure; }
    }

    private bool mIsFinished = false;
    bool ITask.IsFinished
    {
        get { return mIsFinished; }
    }

    string ITask.FailureInfo()
    {
        Debug.LogError("加载资源失败 abName=" + abName);
        return "加载资源失败:" + abName;
    }

    void ITask.OnExecute()
    {
        bool isEditor = false;
#if UNITY_EDITOR
        isEditor = AppConst.DebugMode;
#endif
        if (isEditor)
        {
            LuaMgr.Instance.AddFile(abName, null);
            mIsFinished = true;
        }
        else
        {
            ABMgr.Instance.LoadAB(Path.GetFileName(abName).ToLower(), a =>
            {
                if (a == null)
                {
                    mIsFailure = true;
                    Debug.LogError("加载资源失败 无法加载AB abName=" + abName);
                }
                else
                {
                    ab = a;
                    LuaMgr.Instance.AddFile(abName, ab);
                    mIsFinished = true;
                }
            });
        }

    }

    void ITask.Rest()
    {
        mIsFailure = false;
        mIsFinished = false;
    }

    string ITask.TaskName()
    {
        return "加载资源";
    }
}