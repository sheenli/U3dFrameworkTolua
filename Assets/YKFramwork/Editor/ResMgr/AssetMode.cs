using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class AssetMode
{
    public class GroupInfo
    {
        public int NameHashCode = 0;
        public string Name;
        public GroupInfo(int id,string name)
        {
            NameHashCode = id;
            this.Name = name;
        }
    }

    /// <summary>
    /// 资源信息
    /// </summary>
    public class AssetInfo
    {
        public ResInfoData data;
        public int NameHashCode = 0;
        public string Name;
        public long size;
        public AssetInfo(int id, string name)
        {
            NameHashCode = id;
            this.Name = name;
            data = resInfo.GetResInfo(name);
            if (File.Exists(data.path))
            {
                FileInfo info = new FileInfo(data.path);
                size = info.Length;
            }
        }
        public string GetSizeString()
        {
            if (size == 0)
                return "--";
            return EditorUtility.FormatBytes(size);
        }
    }


    /// <summary>
    /// 当前组
    /// </summary>
    public static List<AssetMode.GroupInfo> gropsEditorInfo = new List<AssetMode.GroupInfo>();

    public static AssetMode.GroupInfo GetGroupInfo(string groupName)
    {
        AssetMode.GroupInfo info = null;
        foreach (AssetMode.GroupInfo group in gropsEditorInfo)
        {
            if (group.Name == groupName)
            {
                info = group;
                break;
            }
        }
        return info;
    }

    public static AssetMode.GroupInfo GetGroupInfo(int id)
    {
        AssetMode.GroupInfo info = null;
        foreach (AssetMode.GroupInfo group in gropsEditorInfo)
        {
            if (group.NameHashCode == id)
            {
                info = group;
                break;
            }
        }
        return info;
    }

    /// <summary>
    /// 本地配置表的文件
    /// </summary>
    public static ResCfg loaclFileresInfo;

    public static /*const*/ Color k_LightGrey = Color.grey * 1.5f;
    public static string ResCfgFilePath
    {
        get
        {
            return AppConst.CoreDef.EditorResCfgPath;
        }
    }
    /// <summary>
    /// 资源信息
    /// </summary>
    public static ResCfg resInfo;

    /// <summary>
    /// 重新查找所有资源
    /// </summary>
    public static void Rebuild()
    {
        
        if (File.Exists(ResCfgFilePath))
        {
            string json = File.ReadAllText(ResCfgFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                loaclFileresInfo = JsonUtility.FromJson<ResCfg>(json);
            }
            else
            {
                resInfo = new ResCfg();
            }
            Refresh();
        }
        else
        {
            resInfo = new ResCfg();
        }
        RefreshGroupList();
    }

    public static bool Update()
    {
        bool ret = false;
        if (resInfo == null)
        {
            return ret;
        }
        if (loaclFileresInfo == null || loaclFileresInfo.groups.Count != resInfo.groups.Count || loaclFileresInfo.resources.Count != resInfo.resources.Count)
        {
            ret = true;
        }
        else
        {
            if (loaclFileresInfo == null || loaclFileresInfo.groups.Count == resInfo.groups.Count)
            {
                foreach (ResGroupCfg da in loaclFileresInfo.groups)
                {
                    bool flag = false;
                    foreach (ResGroupCfg da2 in resInfo.groups)
                    {
                        if (da.sceneName == da2.sceneName)
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        ret = true;
                        break;
                    }
                }
            }
            if(loaclFileresInfo == null || loaclFileresInfo.resources.Count == resInfo.resources.Count)
            {
                foreach (ResInfoData da in loaclFileresInfo.resources)
                {
                    ResInfoData da2 = resInfo.GetResInfo(da.name);
                    if (da.IsDirty(da2))
                    {
                        ret = true;break;
                    }
                }
            }
        }

        if (ret)
        {
            string jsonstr = JsonUtility.ToJson(resInfo,true);
            loaclFileresInfo = JsonUtility.FromJson<ResCfg>(jsonstr);
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(ResCfgFilePath)))
            {
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(ResCfgFilePath));
            }
            File.WriteAllText(ResCfgFilePath, jsonstr);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            RefreshGroupList();
        }

        return ret;
    }   

    /// <summary>
    /// 是否能改名
    /// </summary>
    /// <param name="item"></param>
    /// <param name="newName"></param>
    /// <returns></returns>
    public static bool HandleGroupRename(string groupName, string newName)
    {
        bool ret = resInfo != null && resInfo.groups.Exists(a => a.sceneName == groupName)
            && !resInfo.groups.Exists(a => a.sceneName == newName);
        if (ret)
        {
            foreach (ResGroupCfg sceneData in resInfo.groups)
            {
                if (sceneData.sceneName == groupName)
                {
                    sceneData.sceneName = newName;
                    break;
                }
            }
        }
        Update();
        return ret;
    }

    /// <summary>
    /// 删除所有某个资源组
    /// </summary>
    /// <param name="b"></param>
    internal static void HandleGroupsDelete(List<AssetMode.GroupInfo> b)
    {
        List<string> delAssets = new List<string>();
        foreach (AssetMode.GroupInfo groupName in b)
        {
            ResGroupCfg load = null;
            foreach (ResGroupCfg da in resInfo.groups)
            {
                if (da.sceneName == groupName.Name)
                {
                    load = da;
                    break;
                }
            }
            if (load != null)
            {
                resInfo.groups.Remove(load);
                delAssets.AddRange(load.keys);
            }
        }
        foreach (string assetName in delAssets)
        {
            resInfo.resources.Remove(resInfo.GetResInfo(assetName));
        }
        Update();
        RefreshGroupList();
    }

    /// <summary>
    /// 创建一个资源组
    /// </summary>
    internal static string HandleGroupCreate()
    {
        ResGroupCfg data = new ResGroupCfg();
        data.keys = new List<string>();
        data.sceneName = GetGetUniqueName();
        resInfo.groups.Add(data);
        RefreshGroupList();
        return data.sceneName;
    }

    /// <summary>
    /// 拖了一个文件到资源里面
    /// </summary>
    /// <param name="paths"></param>
    internal static List<string> AddAssetToGroup(string[] paths,string groupName)
    {
        List<string> fileNames = new List<string>();
        ResGroupCfg data = resInfo.GetGroupInfo(groupName);
        if (data == null)
        {
            return fileNames;
        }
        foreach (string str in paths)
        {
            if(AssetDatabase.IsValidFolder(str))
            {
                string[] files = Directory.GetFiles(str);
                foreach (string fileName in files)
                {
                    if (Path.GetExtension(fileName) == ".meta") continue;
                    string assetName = AddAssetToGroup(fileName, groupName);
                    if (!string.IsNullOrEmpty(assetName))
                    {
                        fileNames.Add(assetName);
                    }
                }
            }
            else
            {
                string assetName = AddAssetToGroup(str,groupName);
                if (!string.IsNullOrEmpty(assetName))
                {
                    fileNames.Add(assetName);
                }
            }
        }
        return fileNames;
    }

    /// <summary>
    /// 添加文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="groupName"></param>
    public static string AddAssetToGroup(string path, string groupName)
    {
        ResGroupCfg data = resInfo.GetGroupInfo(groupName);
        if (data == null)
        {
            return "";
        }

        string assetName = Path.GetFileNameWithoutExtension(path);
        if (!data.keys.Contains(assetName))
        {
            data.keys.Add(assetName);
            if (!resInfo.resources.Exists(a => a.name == assetName))
            {
                ResInfoData da = new ResInfoData();
                da.name = assetName;
                da.path = path.Replace("\\", "/");
                da.type = Path.GetExtension(path);
                da.isFairyGuiPack = false;
                if (da.type == ".bytes")
                {
                    TextAsset text = AssetDatabase.LoadAssetAtPath<TextAsset>(da.path);
                    byte [] descBytes = text.bytes;
                    
                    if (ResMgr.ResIsFUIPack(descBytes))
                    {
                        da.isFairyGuiPack = true;
                    }
                }
                
                da.isKeepInMemory = true;
                
                da.isResourcesPath = da.path.Contains("Assets/Resources");
                if (da.isResourcesPath)
                {
                    string rootPath = da.path
                        .Replace("Assets/Resources/","");
                    rootPath = Path.GetDirectoryName(rootPath);
                    da.ABName = rootPath;
                    //da.ABName = da.path.Replace(Application.dataPath + "/Resources")
                }
                else
                {
                    string rootPath = Path.GetDirectoryName(da.path);
                    rootPath = rootPath.Substring(rootPath.LastIndexOf("/")+1);
                    da.ABName = rootPath.ToLower();
                }
                resInfo.resources.Add(da);
            }
        }
        return assetName;
    }


    internal static void RemoveAssets(List<string> reAssets, string name)
    {
        ResGroupCfg data = resInfo.GetGroupInfo(name);
        if (data == null)
        {
            return;
        }
        foreach (string assetName in reAssets)
        {
            RemoveAsset(assetName, name);
        }
        Update();
    }

    /// <summary>
    /// 在某个资源组里面移出某个资源
    /// </summary>
    /// <param name="assetName">资源名称</param>
    /// <param name="name">资源组名称</param>
    public static void RemoveAsset(string assetName, string name)
    {
        ResGroupCfg data = resInfo.GetGroupInfo(name);
        if (data != null)
        {
            if (data.keys.Contains(assetName))
            {
                data.keys.Remove(assetName);
            }
        }
        foreach (ResGroupCfg sld in resInfo.groups)
        {
            if (sld.keys.Contains(assetName))
            {
                return;
            }
        }
        if (resInfo.resources.Exists(a => a.name == assetName))
        {
            ResInfoData da = resInfo.GetResInfo(assetName);
            if (da != null)
            {
                resInfo.resources.Remove(da);
            }
        }
    }

    /// <summary>
    /// 获取一个唯一的组名
    /// </summary>
    /// <returns></returns>
    public static string GetGetUniqueName()
    {
        string name = "NewGroupName";
        int index = 0;
        bool foundExisting = resInfo.HasGroup(name);
        while(foundExisting)
        {
            index++;
            foundExisting = resInfo.HasGroup(name + " "+ index);
        }
        if (index > 0)
        {
            name = name + " " + index;
        }
        return name;
    }


    public static void RefreshGroupList()
    {
        gropsEditorInfo.Clear();
        if (resInfo != null)
        {
            int id = 0;
            foreach (string name in resInfo.GetAllGroupNames())
            {
                gropsEditorInfo.Add(new GroupInfo(id, name));
                id++;
            }
        }
    }

    public static void Refresh()
    {
        if (loaclFileresInfo != null)
        {
            resInfo = JsonUtility.FromJson<ResCfg>(JsonUtility.ToJson(loaclFileresInfo));
        }
        RefreshGroupList();
    }
}
