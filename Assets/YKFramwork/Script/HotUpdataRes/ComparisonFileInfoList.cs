using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 对比后的信息
/// </summary>
public class ComparisonInfo
{
    /// <summary>
    /// 下载版本的对比信息
    /// </summary>
    public int flag = 0;

    public bool isNotHotUpdata
    {
        get
        {
            return flag == 1;
        }
    }


    /// <summary>
    /// 需要下载的任务列表
    /// </summary>
    public List<ABInfo> needDecompressionList = new List<ABInfo>();

    public float Size;

    /// <summary>
    /// 字符
    /// </summary>
    public string SizeString
    {
        get
        {
            return Size.ToString("F2") + "MB";
        }
    }
}
