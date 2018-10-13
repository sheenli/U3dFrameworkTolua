using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 加载场景的配置信息
/// </summary>
[System.Serializable]
public class ResGroupCfg
{
    /// <summary>
    /// 场景名称
    /// </summary>
    public string sceneName;

    /// <summary>
    /// 该场景的所有要加载的资源
    /// </summary>
    public List<string> keys = new List<string>();
}

/// <summary>
/// 资源信息
/// </summary>
[System.Serializable]
public class ResInfoData
{
    /// <summary>
    /// 所在AB的名字或者resources路径名称
    /// </summary>
    public string ABName;
    /// <summary>
    /// 资源路径  编辑器下用
    /// </summary>
    public string path;
    /// <summary>
    /// 资源名称 不带后缀
    /// </summary>
    public string name;
    /// <summary>
    /// 资源类型
    /// </summary>
    public string type;

    /// <summary>
    /// 资源是否常驻内存
    /// </summary>
    public bool isKeepInMemory = true;

    /// <summary>
    /// 是否是Resources资源
    /// </summary>
    public bool isResourcesPath = false;

    public bool IsDirty(ResInfoData data)
    {
        return data.isKeepInMemory != isKeepInMemory
            || data.isResourcesPath != isResourcesPath
            || data.name != name
            || data.path != path
            || data.isFairyGuiPack != isFairyGuiPack;
    }

    /// <summary>
    /// 是否是farigui的包
    /// </summary>
    public bool isFairyGuiPack = false;
}

/// <summary>
/// 所有资源和资源组的配置信息
/// </summary>
[System.Serializable]
public class ResCfg
{
    /// <summary>
    /// 所有资源组
    /// </summary>
    public List<ResGroupCfg> groups = new List<ResGroupCfg>();

    /// <summary>
    /// 每个资源的信息配置
    /// </summary>
    public List<ResInfoData> resources = new List<ResInfoData>();

    /// <summary>
    /// 当前是否存在该资源组
    /// </summary>
    /// <param name="groupName">资源组名称</param>
    /// <returns></returns>
    public bool HasGroup(string groupName)
    {
        string[] strs = GetAllGroupNames();
        foreach (string name in strs)
        {
            if (name == groupName)
            {
                return true;
            }
        }
        return false;
    }

    public ResGroupCfg GetGroupInfo(string groupName)
    {
        foreach (ResGroupCfg scene in groups)
        {
            if (groupName == scene.sceneName)
            {
                return scene;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取所有的组名字
    /// </summary>
    /// <returns></returns>
    public string [] GetAllGroupNames()
    {
        List<string> list = new List<string>();
        foreach (ResGroupCfg scene in groups)
        {
            list.Add(scene.sceneName);
        }
        return list.ToArray();
    }

    /// <summary>
    /// 获取某个组包含的资源列表
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public string[] GetAssetsNames(string groupName)
    {
        List<string> list = new List<string>();
        if (!string.IsNullOrEmpty(groupName))
        {
            foreach (ResGroupCfg scene in groups)
            {
                if (scene.sceneName == groupName)
                {
                    return scene.keys.ToArray();
                }
            }
        }
        return list.ToArray();
    }

    /// <summary>
    /// 获取一个资源信息根据资源名称
    /// </summary>
    /// <param name="assetName">资源名称</param>
    /// <returns></returns>
    public ResInfoData GetResInfo(string assetName)
    {
        ResInfoData data = null;
        foreach (ResInfoData info in this.resources)
        {
            if (assetName == info.name)
            {
                data = info;
                break;
            }
        }
        return data;
    }
}

