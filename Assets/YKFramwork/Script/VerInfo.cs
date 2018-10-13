using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class VerInfo
{
    public string verName;
    public string ver;
    public List<ABInfo> files = new List<ABInfo>();
    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }


    /// <summary>
    /// 对比版本不一样的地方
    /// </summary>
    /// <param name="_ver"></param>
    /// <returns></returns>
    public int CompareTo(string _ver)
    {
        if (ver == "0.0.0")
        {
            return 1;
        }
        int[] _vers = new int[3] { 0, 0, 0 };
        int[] vers = new int[3] { 0, 0, 0 };

        int num = 0;
        if (_ver.Contains("."))
        {
            foreach (string s in _ver.Split('.'))
            {
                _vers[num] = int.Parse(s);
                num++;
            }
        }

        num = 0;

        if (ver.Contains("."))
        {
            foreach (string s in ver.Split('.'))
            {
                vers[num] = int.Parse(s);
                num++;
            }
        }

        int num0 = vers[0] - _vers[0];
        int num1 = vers[1] - _vers[1];
        int num2 = vers[2] - _vers[2];
        if (num0 < 0)
        {
            return 1;
        }
        else if (num0 == 0)
        {
            if (num1 < 0)
            {
                return 2;
            }
            else if (num1 == 0)
            {
                if (num2 < 0)
                {
                    return 3;
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// 获取本地这个文件的信息
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public ABInfo GetABDownLoadInfo(string fileName)
    {
        foreach (var info in files)
        {
            if (info.fileName == fileName)
            {
                return info;
            }
        }
        return null;
    }

    public void Save(ABInfo ab)
    {
        ABInfo local = GetABDownLoadInfo(ab.fileName);
        if (local != null)
        {
            local.assets = ab.assets;
            local.length = ab.length;
            local.sha1 = ab.sha1;
        }
        else
        {
            files.Add(ab);
        }
    }

    public static ComparisonInfo ComparisonVer(VerInfo local, VerInfo remoteVer)
    {
        ComparisonInfo comparisonInfo = new ComparisonInfo();
        if (remoteVer == null)
        {
            comparisonInfo.flag = 0;
            return comparisonInfo;
        }
        if (local == null || local.ver == "0.0.0")
        {
            comparisonInfo.flag = -1;
        }
        else
        {
            comparisonInfo.flag = local.CompareTo(remoteVer.ver);
        }
        if (comparisonInfo.flag != 0)
        {
            foreach (ABInfo info in remoteVer.files)
            {
                bool need = false;
                if (comparisonInfo.flag == -1)
                {
                    need = true;
                }
                else
                {
                    var localInfo = local.GetABDownLoadInfo(info.fileName);
                    if (localInfo == null || !localInfo.IsSame(info))
                    {
                        need = true;
                    }
                }

                if (need)
                {
                    comparisonInfo.Size += (info.length / 1024.0f / 1024.0f);
                    comparisonInfo.needDecompressionList.Add(info);
                }
            }
        }
        return comparisonInfo;
    }
}