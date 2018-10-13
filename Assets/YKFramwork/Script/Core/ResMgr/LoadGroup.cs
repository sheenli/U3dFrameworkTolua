using System;
using UnityEngine;
using System.Collections.Generic;

public class LoadGroup : ISceneLoad
{
    public string fileName
    {
        set;
        get;
    }

    private Dictionary<string, List<ResInfoData>> mNeedLoadABs = new Dictionary<string, List<ResInfoData>>();

    private List<string> needLoadAssets = new List<string>();
    private ResGroupCfg groupInfo = null;

    public bool isKeepInMemory { get { return false; } set { throw new NotImplementedException(); } }
    public int loaded { get; set; }

    private Action mCallback = null;
    private float startTime = 0;
    public void Load(Action callback)
    {
        startTime = Time.time;
        mCallback = callback;
        LoadAllAB();
    }

    public LoadGroup(ResGroupCfg cfg)
    {
        //LuaInterface.Debugger.LogError("开始加载资源组：" + cfg.sceneName);
        mNeedLoadABs.Clear();
        needLoadAssets.Clear();
        this.groupInfo = cfg;
        List<ResInfoData> allAssets = new List<ResInfoData>();
        foreach (string assetName in this.groupInfo.keys)
        {
            ResInfoData info = GameCfgMgr.Instance.GetResInfo(assetName);
            if (info == null)
            {
                Debug.LogWarning("加载了重未为使用的资源 -----assetNane：" + assetName);
            }
            else
            {
                if (!allAssets.Contains(info))
                {
                    if (!ResMgr.Intstance.GetAsset(assetName))
                    {
                        allAssets.Add(info);
                    }
                }
                else
                {
                    Debug.LogWarning("重复的添加资源 -----assetNane：" + assetName);
                }
            }
        }

        if (!AppConst.DebugMode)
        {
            mNeedLoadABs = GameCfgMgr.Instance.GetGroupABs(cfg);
            List<string> loadedAb = new List<string>();
            foreach (KeyValuePair<string, List<ResInfoData>> kv in mNeedLoadABs)
            {
                for (int i = kv.Value.Count - 1; i >= 0; i--)
                {
                    if (ResMgr.Intstance.GetAsset(kv.Value[i].name))
                    {
                        kv.Value.RemoveAt(i);
                    }
                }
                if (kv.Value.Count == 0)
                {
                    loadedAb.Add(kv.Key);
                }
            }
            foreach (string ab in loadedAb)
            {
                mNeedLoadABs.Remove(ab);
            }
        }

        foreach (List<ResInfoData> assets in mNeedLoadABs.Values)
        {
            foreach (ResInfoData fileName in assets)
            {
                if (allAssets.Contains(fileName))
                {
                    allAssets.Remove(fileName);
                }
            }
        }

        foreach (ResInfoData assetName in allAssets)
        {
            needLoadAssets.Add(assetName.name);
        }
    }

    private void LoadAllAB()
    {
        if (mNeedLoadABs.Count > 0)
        {
            List<string> abs = new List<string>();
            
            foreach (string ABName in mNeedLoadABs.Keys)
            {
                abs.Add(ABName);
            }
            for(int i = abs.Count -1;i>=0;i--)
            {
                string abName = abs[i];
                ABMgr.Instance.LoadAB(abName, ab =>
                {
                    LoadABFinished(ab, abName);
                });
            }
        }
        else
        {
            CheckAllAbLoadFinished();
        }
    }

    private void LoadABFinished(AssetBundle AB, string abName)
    {
        if (AB == null)
        {
            loaded = -1;
            Debug.LogError("加载ab失败：" + abName);
            ResMgr.Intstance.QueueEvent(new LoadGroupItemError(groupInfo.sceneName, abName));
        }

        if (this.mNeedLoadABs[abName].Count > 1)
        {
            ResMgr.Intstance.LoadAll(abName, ()=> 
            {
                this.mNeedLoadABs.Remove(abName);
                CheckAllAbLoadFinished();
            });
        }
        else
        {
            if (this.mNeedLoadABs[abName].Count > 0)
            {
                ResInfoData data = this.mNeedLoadABs[abName][0];
                ResMgr.Intstance.LoadAsset(data.name, a => 
                {
                    this.mNeedLoadABs.Remove(abName);
                    CheckAllAbLoadFinished();
                });
            }
            else
            {
                CheckAllAbLoadFinished();
            }
        }

    }

    private void CheckAllAbLoadFinished()
    {
        if (this.mNeedLoadABs.Count == 0)
        {
            LoadAllAssets();
        }
    }

    private void LoadAllAssets()
    {
        if (needLoadAssets.Count == 0)
        {
            AllAssetLoadFinished();
        }
        else
        {
            for (int i = needLoadAssets.Count - 1; i >= 0; i--)
            {
                //string assetName = needLoadAssets[i];
                ResInfoData data = GameCfgMgr.Instance.GetResInfo(needLoadAssets[i]);
                ResMgr.Intstance.LoadAsset(data.name, a =>
                {
                    if (a == null)
                    {
                        LoadItemERROR(data.name);
                    }
                    else
                    {
                        LoadAssetFinished(data.name);
                    }
                });
            }
        }
    }

    private void LoadItemERROR(string assetName)
    {
        loaded = -1;
        Debug.LogError("有资源加载失败资源名称：" + assetName);
        if (mCallback != null)
        {
            mCallback();
        }
        ResMgr.Intstance.QueueEvent(new LoadGroupItemError(groupInfo.sceneName, assetName));
    }


    private void LoadAssetFinished(string assetName)
    {
        needLoadAssets.Remove(assetName);
        if (needLoadAssets.Count == 0)
        {
            AllAssetLoadFinished();
        }
        
    }

    private void AllAssetLoadFinished()
    {
        ResMgr.Intstance.TryAddFairyGuiRes();
        ResMgr.Intstance.QueueEvent(new ResLoadGroupEvent(groupInfo.sceneName));
        loaded = 1;
        if (mCallback != null)
        {
            mCallback();
        }
        float time = Time.time - startTime;
       
    }
}