using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABMgr : EventDispatcherNode
{
    public static ABMgr Instance
    {
        get;
        private set;
    }

    #region 缓存信息
    /// <summary>
    /// 等待加载的ab
    /// </summary>
    private Queue<LoadPack> mWaitLoad = new Queue<LoadPack>();

    /// <summary>
    /// 正在加载的ab
    /// </summary>
    private List<LoadPack> mLoading = new List<LoadPack>();

    /// <summary>
    /// ab缓存合集
    /// </summary>
    private Dictionary<string, PackInfo> mCacheABDic = new Dictionary<string, PackInfo>();
    #endregion

    #region 加载

    /// <summary>
    /// 加载一个AB
    /// </summary>
    /// <param name="ABName">资源名称</param>
    /// <param name="callBack">回调函数</param>
    /// <param name="keepInMemory">是否常驻内存</param>
    /// <param name="Async">是否异步加载</param>
    public void LoadAB(string ABName, System.Action<AssetBundle> callBack = null
        , bool keepInMemory = false, bool Async = false)
    {
        if (mCacheABDic.ContainsKey(ABName))
        {
            if (callBack != null)
            {
                callBack(mCacheABDic[ABName].ab);
            }
        }
        else
        {
            if (Async)
            {
                foreach (LoadPack load in mLoading)
                {
                    if (load.info.abName == ABName)
                    {
                        load.AddListener(callBack);
                        return;
                    }
                }
                foreach (LoadPack load in mWaitLoad)
                {
                    if (load.info.abName == ABName)
                    {
                        load.AddListener(callBack);
                        return;
                    }
                }
                LoadPack loadPack = new LoadPack();
                loadPack.info = new PackInfo(name, null, keepInMemory);
                loadPack.AddListener(callBack);
                mWaitLoad.Enqueue(loadPack);
            }
            else
            {
                AssetBundle ab = AssetBundle.LoadFromFile(AppConst.AppExternalDataPath + "/" + ABName + AppConst.ExtName);
                PackInfo info = new PackInfo(ABName, ab, keepInMemory);
                mCacheABDic[info.abName] = info;
                if (callBack != null)
                {
                    callBack(mCacheABDic[ABName].ab);
                }
            }
        }
    }

    public PackInfo GetAB(string ABName)
    {
        return mCacheABDic.ContainsKey(ABName) ? mCacheABDic[ABName] : null;
    }
    #endregion

    private void Awake()
    {
        Instance = this;
        mCacheABDic = new Dictionary<string, PackInfo>();
    }

    public override void OnUpdate()
    {
        while (mWaitLoad.Count > 0 && mLoading.Count <= SystemInfo.processorCount)
        {
            LoadPack pack = mWaitLoad.Dequeue();
            mLoading.Add(pack);
            pack.Load();
        }
        if (mLoading.Count > 0)
        {
            for (int i = mLoading.Count - 1; i >= 0; i--)
            {
                if (mLoading[i].IsDone)
                {
                    mLoading[i].info.ab = mLoading[i].request.assetBundle;
                    mLoading[i].CallBack();
                    mCacheABDic.Add(mLoading[i].info.abName, mLoading[i].info);
                    mLoading.RemoveAt(i);
                }
            }
        }
    }

    #region 释放资源
    /// <summary>
    /// 当被销毁的时候销毁
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        UnLoadAll(true);
    }
    /// <summary>
    /// 释放所有资源
    /// </summary>
    /// <param name="forced">强制释放全部</param>
    public void UnLoadAll(bool forced = false)
    {
        
        List<PackInfo> removes = new List<PackInfo>();
        foreach (PackInfo info in mCacheABDic.Values)
        {
            if (!info.keepInMemory || forced)
            {
                if (info.ab != null)
                {
                    info.ab.Unload(false);
                }
                removes.Add(info);
            }
            //if (!info.abName.EndsWith("sound"))
            //{
            //    if (!info.keepInMemory || forced)
            //    {
            //        info.ab.Unload(false);
            //        removes.Add(info);
            //    }
            //}
            //else
            //{
            //    if (ResMgr.Intstance.GetABRefCount(info.abName) == 0)
            //    {
            //        info.ab.Unload(false);
            //        removes.Add(info);
            //    }
            //}
        }
        if(forced)
        mCacheABDic.Clear();
        else
        {
            foreach (PackInfo info in removes)
            {
                mCacheABDic.Remove(info.abName);
            }
        }
    }

    /// <summary>
    /// 释放指定的AB
    /// </summary>
    /// <param name="abName">ab名称</param>
    /// <param name="immediate">是否立即释放</param>
    public void UnLoadAB(string abName, bool immediate = false)
    {
        if (mCacheABDic.ContainsKey(abName) && !abName.EndsWith("sound"))
        {
            mCacheABDic[abName].ab.Unload(false);
            mCacheABDic.Remove(abName);
        }
        if (immediate)
            
        StartCoroutine(Test());
    }


    public IEnumerator Test()
    {
        yield return 10;
        ResMgr.Intstance.GC();
    }

    #endregion

    #region 信息类
    public class PackInfo
    {
        public string abName;
        public AssetBundle ab;
        public bool keepInMemory;
        public PackInfo(string name, AssetBundle _Ab, bool keepInMemory = false)
        {
            this.ab = _Ab;
            this.abName = name;
            this.keepInMemory = keepInMemory;
        }
    }

    public class LoadPack
    {
        public AssetBundleCreateRequest request;
        public PackInfo info;
        public List<Action<AssetBundle>> listeners = new List<Action<AssetBundle>>();

        public void Load()
        {
            request = AssetBundle.LoadFromFileAsync(AppConst.AppExternalDataPath);
        }

        public bool IsDone
        {
            get
            {
                return request != null && request.isDone;
            }
        }

        /// <summary>
        /// 添加一个监听
        /// </summary>
        /// <param name="listener"></param>
        public void AddListener(Action<AssetBundle> listener)
        {
            if (listener != null && !listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void CallBack()
        {
            foreach (Action<AssetBundle> callBack in listeners)
            {
                callBack(request.assetBundle);
            }
            listeners.Clear();
            listeners = null;
        }
    }
    #endregion
}
