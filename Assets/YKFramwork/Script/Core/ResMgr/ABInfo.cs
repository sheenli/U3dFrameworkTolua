using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ABInfo
{
    public string fileName;
    public string sha1;
    public long length;
    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
    public List<string> assets = new List<string>();


    public bool IsSame(ABInfo remotely)
    {
        ABInfo local = this;
        bool same = true;
        if (remotely.sha1 != local.sha1)
        {
            same = false;
        }
        else
        {
            if (!File.Exists(AppConst.AppExternalDataPath + "/" + local.fileName))
            {
                same = false;
            }
        }
        return same;
    }
}
