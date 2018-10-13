using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using FairyGUI;
using FairyGUI.Utils;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResMgr : EventDispatcherNode
{
    public static bool ResIsFUIPack(byte[] data)
    {
        bool flag = false;
        try
        {
            ByteBuffer buffer = new ByteBuffer(data);
            flag = buffer.ReadUint() == 0x46475549;
        }
        catch (System.Exception ex)
        {
            flag = false;
        }
        return flag;
    }

    public void Awake()
    {
        Intstance = this;
    }
    /// <summary>
    /// 所有资源
    /// </summary>
    private Dictionary<string, ResInfo> mDic = new Dictionary<string, ResInfo>();

    public static ResMgr Intstance
    {
        get;
        private set;
    }

  

#region 加载资源

    /// <summary>
    /// 添加资源到字典
    /// </summary>
    /// <param name="assetName"></param>
    private void AddResToDic (string assetName, UnityEngine.Object obj)
    {
        if (obj == null)
        {
            return;
        }
        if (!mDic.ContainsKey(assetName))
        {
            ResInfoData data = GameCfgMgr.Instance.GetResInfo(assetName);
            if (data== null)
            {
                Debug.LogError("加载了无效的资源 assetName ="+ assetName);
            }
            else
            {
                ResInfo info = new ResInfo(obj, data);
                mDic[assetName] = info;
            }
        }
    }

#region 加载单一资源
    

    /// <summary>
    /// 获取一个已经加载了的资源
    /// </summary>
    /// <param name="Name">资源名称</param>
    /// <returns></returns>
    public UnityEngine.Object GetAsset(string assetName)
    {
        UnityEngine.Object obj = null;
        if (mDic.ContainsKey(assetName))
        {
            obj = mDic[assetName].asset;
        }
        return obj;
    }

    /// <summary>
    /// 获取ab的引用数
    /// </summary>
    /// <param name="abName"></param>
    /// <returns></returns>
    public int GetABRefCount(string abName)
    {
        int count = 0;

        foreach (ResInfo info in mDic.Values)
        {
            if (!info.isResourcesPath && info.ABName != abName)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// 异步获取一个资源 如果没加载的资源那么会自动加载
    /// </summary>
    /// <param name="assetName">资源名称</param>
    /// <param name="callBack">加载完成</param>
    public void GetAssetAsync(string assetName, Action<UnityEngine.Object> callBack)
    {
        new AutoLoadAsset(assetName, true, () =>
           {
               if (callBack != null)
               {
                   callBack(GetAsset(assetName));
               }
           });
    }
    

    /// <summary>
    /// 从AB里面加载资源 这里一定要保证AB是先加载过的  不会主动加载AB
    /// </summary>
    /// <param name="assteName">资源名称</param>
    /// <param name="callBaclk">加载完成后的回调</param>
    /// <param name="keepInMemory">是否常驻内存</param>
    public void LoadAsset(string assteName, Action<UnityEngine.Object> callBaclk = null)
    {
        LoadAsset(assteName, typeof(UnityEngine.Object), callBaclk);
    }

    /// <summary>
    /// 从AB里面加载资源 这里一定要保证AB是先加载过的  不会主动加载AB
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="assteName">资源名称</param>
    /// <param name="callBaclk">回调</param>
    /// <param name="keepInMemory">是否常驻内存</param>
    public void LoadAsset<T>(string assteName, Action<T> callBaclk = null) 
        where T : UnityEngine.Object
    {
        LoadAsset(assteName, typeof(T), callBaclk as Action<UnityEngine.Object>);
    }
    /// <summary>
    /// 从AB里面加载资源 这里一定要保证AB是先加载过的  不会主动加载AB
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="assteName">资源名称</param>
    /// <param name="type">资源类型</param>
    /// <param name="callBaclk">回调</param>
    /// <param name="keepInMemory">是否常驻内存</param>
    public void LoadAsset(string assetName, Type type, Action<UnityEngine.Object> callBaclk = null)
    {
        if (mDic.ContainsKey(assetName))
        {
            if (callBaclk != null)
            {
                callBaclk(mDic[assetName].asset);
            }
            return;
        }
        UnityEngine.Object obj = null;
        ResInfoData data = GameCfgMgr.Instance.GetResInfo(assetName);
        if (data != null)
        {
            if (data.isResourcesPath)
            {
                obj = Resources.Load(data.ABName+"/"+ assetName, type);
                AddResToDic(assetName, obj);
                if (callBaclk != null)
                {
                    callBaclk(obj);
                }
            }
            else
            {
                bool isEditorLoad = false;
#if UNITY_EDITOR
                if (AppConst.DebugMode)
                {
                    isEditorLoad = true;
                    obj = UnityEditor.AssetDatabase.LoadAssetAtPath(data.path, type);
                    AddResToDic(assetName, obj);
                    if (callBaclk != null)
                    {
                        callBaclk(obj);
                    }
                }
#endif
                Action<UnityEngine.Object> loadAsseted = assetObj =>
                {
                    AddResToDic(assetName, obj);
                    if (callBaclk != null)
                    {
                        callBaclk(obj);
                    }
                };
                if (!isEditorLoad)
                {
                    if (!string.IsNullOrEmpty(data.ABName))
                    {
                        ABMgr.Instance.LoadAB(data.ABName,ab => 
                        {
                            if (ab != null)
                            {
                                obj = ab.LoadAsset(data.name + data.type, type);
                                loadAsseted(obj);
                            }
                            else
                            {
                                Debug.LogError("加载ab失败：" + data.ABName);
                                if (callBaclk != null)
                                {
                                    callBaclk(obj);
                                }
                            }
                        });
                    }
                    else
                    {
                        Debug.LogError("想要加载的AB名字为空：" + assetName);
                        if (callBaclk != null)
                        {
                            callBaclk(obj);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("要加载的资源未配置：" + assetName);
        }
    }

    public IEnumerator LoadAssetAsync(AssetBundle ab, string assteName, Type type, Action<UnityEngine.Object> callBack)
    {
        AssetBundleRequest req = ab.LoadAssetAsync(assteName, type);
        while(!req.isDone)
        {
            yield return 1;
        }
        AudioClip clip = req.asset as AudioClip;
        clip.LoadAudioData();
        while(clip.loadState == AudioDataLoadState.Loading)
        {
            yield return 1;
        }
        if (callBack != null)
        {
            callBack(req.asset);
        }
    }

    /// <summary>
    /// 预加载音效文件
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    public IEnumerator PrLoadAudioClip(AudioClip audioClip,Action callBack)
    {
        audioClip.LoadAudioData();
        while (audioClip.loadState == AudioDataLoadState.Loading)
        {
            yield return null;
        }
        if(audioClip.loadState == AudioDataLoadState.Loaded)
        {
            if(callBack != null)
            {
                callBack();
            }
        }
        else
        {
            Debug.LogError("音频解码失败 "+ audioClip.name);
            if (callBack != null)
            {
                callBack();
            }
        }
    }

#endregion

#region 批量加载

#region 加载这个AB的所有资源并且缓存
    /// <summary>
    /// 加载这个AB的所有资源并且缓存
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="callBack"></param>
    /// <param name="keepInMemory">是否永久缓存</param>
    public void LoadAll(string abName, Action callBack = null)
    {
        AssetBundle ab = ABMgr.Instance.GetAB(abName).ab;
        UnityEngine.Object[] objs = ab.LoadAllAssets();
        foreach (UnityEngine.Object obj in objs)
        {
            string assetName = Path.GetFileNameWithoutExtension(obj.name);
            AddResToDic(assetName, obj);
//             ResInfo res = new ResInfo();
//             res.asset = obj;
//             res.abName = abName;
//             res.assetName = Path.GetFileNameWithoutExtension(obj.name);
//           
//             ResInfoData dat = GameCfgMgr.Instance.GetResInfo(res.assetName);
//             if (dat != null)
//             {
//                 res.keepInMemory = dat.isKeepInMemory;
//                 res.isFairyGuiPack = dat.isFairyGuiPack;
//                 if (!this.mDic.ContainsKey(abName))
//                 {
//                     this.mDic.Add(res.assetName, res);
//                 }
//             }
//             else
//             {
//                 Debug.LogError("有资源被加载了但是从未被使用" + res.assetName);
//             }
           
        }
        if (callBack != null)
        {
            callBack();
        }
    }
#endregion

#region 加载一个资源组
    /// <summary>
    /// 加载一个资源组加载完成后会发送事件回调
    /// </summary>
    /// <param name="groupName">资源组名称</param>
    public void LoadGroup(string groupName,Action callback = null)
    {
        LoadGroup(GameCfgMgr.Instance.GetGroupInfo(groupName), groupName, callback);
    }

    /// <summary>
    /// 加载一个资源组加载完成后会发送事件回调
    /// </summary>
    /// <param name="groupData">资源组信息</param>
    public void LoadGroup(ResGroupCfg groupData,string groupName, Action callback=null)
    {
        if (groupData == null)
        {
            this.QueueEvent(new ResGroupLoadError(groupName));
            Debug.LogError("资源配置信息不存在资源组名称:" + groupName);
        }
        else
        {
            new LoadGroup(groupData).Load(callback);
        }
    }
#endregion

    public void LogAll()
    {
        foreach (string key in mDic.Keys)
        {
            Debug.LogError("key=" + key);
        }
    }

#endregion

#endregion

#region 资源释放

    private Queue<string> mOldRes = new Queue<string>();
    public void PushRes()
    {
        foreach (string str in mDic.Keys)
        {
            if (!mDic[str].isKeepInMemory)
            {
                mOldRes.Enqueue(str);
            }
        }
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="assetName">资源名称</param>
    /// <param name="immediate">是否立即释放</param>
    public void UnLoadAsset(string assetName,bool immediate = false)
    {
        if (mDic.ContainsKey(assetName))
        {
            mDic.Remove(assetName);
        }
        TryUnLoadFariryGUIRes(assetName);
        if (immediate)
        {
            GC();
        }
        //GC();
    }

    /// <summary>
    /// 释放加载过的资源
    /// </summary>
    /// <param name="forced"></param>
    public void UnLoadAll(bool forced = false)
    {
        if (forced)
        {
            TryUnLoadFariryGUIRes(null, forced);
            mDic.Clear();
            mOldRes.Clear();
        }
        else
        {
            if (mOldRes.Count > 0)
            {
                while (mOldRes.Count > 0)
                {
                    string resName = mOldRes.Dequeue();
                    if (mDic.ContainsKey(resName))
                    {
                        TryUnLoadFariryGUIRes(resName);
                        mDic.Remove(resName);
                    }
                }
            }
        }
        ABMgr.Instance.UnLoadAll(forced);
        StartCoroutine(GC());
    }

    /// <summary>
    /// 释放GC
    /// </summary>
    public IEnumerator GC()
    {
        System.GC.Collect();
        LuaMgr.Instance.GC();
        yield return 10;
        Resources.UnloadUnusedAssets();
    }

    public void TryAddFairyGuiRes()
    {
        foreach (ResInfo info in mDic.Values)
        {
            if (info.isFairyGuiPack)
            {
                FairyGUI.UIPackage pack = FairyGUI.UIPackage.GetByName(info.name);
                if (pack == null)
                {
                    string packName = info.name;
                    if (packName.EndsWith("_fui"))
                    {
                        packName = packName.Substring(0, packName.LastIndexOf("_fui"));
                    }
                    FairyGUI.UIPackage.AddPackage(packName, (string name, string extension, System.Type type, out DestroyMethod destroyMethod) =>
                    {
                        UnityEngine.Object obj = GetAsset(name);
                        destroyMethod = DestroyMethod.None;
                        return obj;
                    });
                }
            }
        }
    }

    public void TryUnLoadFariryGUIRes(string assetName = null,bool all= false)
    {
        if (all)
        {
            FairyGUI.UIPackage.RemoveAllPackages();
        }
        else
        {
            if (mDic.ContainsKey(assetName))
            {
                ResInfo info = mDic[assetName];
                if (info != null&& info.isFairyGuiPack)
                {
                    FairyGUI.UIPackage pack = FairyGUI.UIPackage.GetByName(info.name);
                    if (pack != null)
                    {
                        FairyGUI.UIPackage.RemovePackage(info.name);
                    }
                }
            }
        }
    }
#endregion

    public void Init()
    {
       Intstance = this;
    }

#if UNITY_EDITOR
    [LuaInterface.NoToLua]
    public string FindAssetPath(string assetName)
    {
        string filePath = "";
        string [] strs = AssetDatabase.FindAssets(assetName,new string[] { FileUtil.GetProjectRelativePath(AppConst.ABPath) });
        if (strs.Length > 1)
        {
            foreach (string str in strs)
            {
                if (Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(str)) == assetName)
                {
                    filePath = AssetDatabase.GUIDToAssetPath(str);
                    if (AssetDatabase.IsValidFolder(filePath)
                        || Path.GetDirectoryName(filePath) != AppConst.ABPath +"/Lua")
                    {
                        continue;
                    }
                    break;
                }
            }
            //Debug.LogError("存在重复的资源 资源名称：" + assetName);
        }
        else
        {
            if (strs.Length == 0)
            {
                Debug.LogError("不存在该资源" + assetName);
            }
            else
            {
                filePath = AssetDatabase.GUIDToAssetPath(strs[0]);
            }
        }
        return filePath;
    }
#endif

    /// <summary>
    /// 资源信息
    /// </summary>
    public class ResInfo : ResInfoData
    {
        public UnityEngine.Object asset;
        public ResInfo(UnityEngine.Object obj,ResInfoData data)
        {
            asset = obj;
            base.ABName = data.ABName;
            base.isFairyGuiPack = data.isFairyGuiPack;
            base.isKeepInMemory = data.isKeepInMemory;
            base.isResourcesPath = data.isResourcesPath;
            base.name = data.name;
        }
    }
}