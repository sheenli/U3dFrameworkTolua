using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 游戏配置信息
/// </summary>
public class GameCfgMgr
{
    public LocalGameCfgData localGameCfg;
    /// <summary>
    /// 资源配置信息
    /// </summary>
    public ResCfg resCfg = new ResCfg();

    private static GameCfgMgr mInstance;
    public static GameCfgMgr Instance
    {
        get { return mInstance = mInstance ?? new GameCfgMgr(); }
    }

    #region 辅助函数
    
    /// <summary>
    /// 获取资源信息
    /// </summary>
    /// <param name="assetName">资源名称</param>
    /// <returns></returns>
    public ResInfoData GetResInfo(string assetName)
    {
        ResInfoData data = null;
        if (this.resCfg != null)
        {
            foreach (ResInfoData info in resCfg.resources)
            {
                if (info.name == assetName)
                {
                    data = info;
                    break;
                }
            }
        }
        return data;
    }

    public ResGroupCfg GetGroupInfo(string groupName)
    {
        ResGroupCfg cfg = null;
        if (this.resCfg != null)
        {
            foreach (ResGroupCfg da in this.resCfg.groups)
            {
                if (da.sceneName == groupName)
                {
                    cfg = da;
                    break;
                }
            }
        }
        return cfg;
    }

    /// <summary>
    /// 获得这个资源组里面的所有AB
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, List<ResInfoData>> GetGroupABs(ResGroupCfg groupData)
    {
        Dictionary<string, List<ResInfoData>> aBToAssets = new Dictionary<string, List<ResInfoData>>();
        foreach (string str in groupData.keys)
        {
            ResInfoData data = GetResInfo(str);
            if (data != null)
            {
                if (!data.isResourcesPath)
                {
                    if (!string.IsNullOrEmpty(data.ABName))
                    {
                        if (aBToAssets.ContainsKey(data.ABName))
                        {
                            aBToAssets[data.ABName].Add(data);
                        }
                        else
                        {
                            aBToAssets[data.ABName] = new List<ResInfoData>() { data };
                        }
                    }
                    else
                    {
                        if(!AppConst.DebugMode)
                        Debug.LogError("这个资源不存在ab里面 资源名称：" + str+"abName:"+ data.ABName);
                    }
                }
            }
            else
            {
                Debug.LogError("资源未配置到资源设置，请打开 Tool->资源配置 资源名称：" + str);
            }
        }
        return aBToAssets;
    }
    #endregion



    /// <summary>
    /// 初始化配置
    /// </summary>
    public void Init()
    {
        if (!AppConst.DebugMode)
        {
            LoadResCfg();
        }
        else
        {
#if UNITY_EDITOR
            TextAsset resCfgtext = AssetDatabase.LoadAssetAtPath<TextAsset>(AppConst.CoreDef.EditorResCfgPath);
            if (resCfgtext == null)
            {
                Debug.LogError("无法加载到资源配置表2");
            }
            else
            {
                resCfg = JsonUtility.FromJson<ResCfg>(resCfgtext.text);
            }
#endif
        }
        if (resCfg != null)
        {
            foreach (ResInfoData info in resCfg.resources)
            {
                if(!string.IsNullOrEmpty(info.ABName))
                info.ABName = info.ABName.ToLower();
            }
        }
        localGameCfg = Resources.Load<LocalGameCfgData>("gamecfg");
        localGameCfg.Init();
    }

    /// <summary>
    /// 加载资源需要的配置信息
    /// </summary>
    public void LoadResCfg()
    {
        if (File.Exists(AppConst.CoreDef.CfgExternalFileName))
        {
            if (resCfg == null)
            {
                Debug.LogError("无法加载到资源配置表");
            }
            else
            {
                AssetBundle ab = AssetBundle.LoadFromFile(AppConst.CoreDef.CfgExternalFileName);
                TextAsset txt = ab.LoadAsset<TextAsset>(AppConst.CoreDef.CfgAssetName);
                string resCfgtext = txt.text;
                if (resCfgtext == null)
                {
                    Debug.LogError("无法加载到资源配置表2");
                }
                else
                {
                    resCfg = JsonUtility.FromJson<ResCfg>(resCfgtext);
                }
                ab.Unload(true);
            }
        }
        else
        {
            Debug.LogError("不存在资源配置表");
        }
    }
}