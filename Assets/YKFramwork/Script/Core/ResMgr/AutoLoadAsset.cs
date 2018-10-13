using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自动化加载资源这个加载 如果是resource里面的资源那么从resource里面加载  如果是ab里面的资源会先加载AB后加载资源
/// </summary>
public class AutoLoadAsset : ISceneLoad
{
    public AutoLoadAsset(string assetName,
        bool satrtLoad = false, Action callBack = null)
    {
        fileName = assetName;
        if (satrtLoad) Load(callBack);
    }

    public string fileName
    {
        get;
        set;
    }

    public int loaded { get; set; }

    public bool isKeepInMemory
    {
        get;set;
    }

    public void Load(Action callback)
    {
        ResMgr.Intstance.LoadAsset(fileName,  a=> 
        {
            if (a == null)
            {
                loaded = -1;
            }
            else
            {
                loaded = 1;
            }
            callback();
        });
    }
}
