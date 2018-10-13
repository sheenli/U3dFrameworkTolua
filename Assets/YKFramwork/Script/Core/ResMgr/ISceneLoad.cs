using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneLoad
{
    /// <summary>
    /// 文件名称
    /// </summary>
    string fileName { get; set; }

    /// <summary>
    /// 是否常驻内存
    /// </summary>
    bool isKeepInMemory { get; set; }

    /// <summary>
    /// 是否加载完成
    /// </summary>
    int loaded { get; set; }

    /// <summary>
    /// 开始加载以及加载后的回调
    /// </summary>
    /// <param name="callback"></param>
    void Load(Action callback);
}
