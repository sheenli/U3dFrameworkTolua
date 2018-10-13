using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGameCfgData : ScriptableObject
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/创建一个本地配置")]
    [LuaInterface.NoToLua]
    public static LocalGameCfgData Create()
    {
        LocalGameCfgData data = CreateInstance<LocalGameCfgData>();
        UnityEditor.AssetDatabase.CreateAsset(data, "Assets/YKFramwork/Resources/gamecfg.asset");
        UnityEditor.AssetDatabase.SaveAssets();
        return data;
    }
#endif
    public const string openLogKEY      = "openLog";
    public string       version         = "1.0.0";
    public bool         isPublic        = true;
    public int          chanelType      = 1;
    public int          collectionID    = 8002;
    public bool         openLog         = true;
    public string       OSSROOT         = "https://xwoss.oss-cn-hangzhou.aliyuncs.com/u3dhotfixed";

    private const string LocalGameCfgISPUBLICKEY = "LocalGameCfgISPUBLICKEY";
    public string OSSCollectionPath
    {
        get
        {
            return OSSROOT + "/" + collectionID;
        }
    }

    public string OSSResRootPath
    {
        get
        {
            if (isPublic)
            {
                return OSSCollectionPath + "/" + chanelType + "/public";
            }
            else
            {
                return OSSCollectionPath + "/" + chanelType + "/test";
            }
        }
    }

    public string RemotelyResUrl
    {
        get
        {
            string allverinfoName = isPublic ? "allverinfo.json" : "allverinfoTest.json";
            return OSSResRootPath +"/" +allverinfoName;
        }
    }
    public string CollectionIDVerFileName
    {
        get
        {
            return collectionID + "ver.txt";
        }
    }


    public void SetPublic(bool flag)
    {
        isPublic = flag;
        PlayerPrefs.SetInt(LocalGameCfgISPUBLICKEY, isPublic ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetLog(bool isopen)
    {
        openLog = isopen;
        PlayerPrefs.SetInt(openLogKEY, openLog ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Init()
    {
        if (PlayerPrefs.HasKey(openLogKEY))
        {
            openLog = PlayerPrefs.GetInt(openLogKEY, 0) == 1;
        }
    }
}
