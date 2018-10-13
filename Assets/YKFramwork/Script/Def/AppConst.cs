
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppConst
{
    /// <summary>
    /// AB的拓展名
    /// </summary>
    public const string ExtName = ".bytes";
    [Header("是不是测试环境")]
    public static bool DebugMode = true;
    [Header("服务器ip")]
    public static string serverIP = "";

    public static string AppExternalDataPath
    {
        get
        {
            string path = Application.persistentDataPath;
#if UNITY_EDITOR
            path = Application.dataPath +"/../AB";
#endif
            return path;
        }
    }

    public static string AppExternalDataPathUrl
    {
        get
        {
            return "file://" + AppExternalDataPath;
        }
    }

    /// <summary>
    /// 原文件夹的路径
    /// </summary>
    public static string SourceResPathUrl
    {
        get
        {
            string filepath = "";
#if UNITY_EDITOR
            filepath = "file:///"+Application.streamingAssetsPath;
#elif UNITY_ANDROID
            filepath = Application.streamingAssetsPath;
#elif UNITY_IOS
            filepath = "file://" + Application.streamingAssetsPath;
#else 
            filepath = "file:///"+Application.streamingAssetsPath;
#endif
            return filepath;
        }
    }

    public static string LuaPath
    {
        get
        {
            return Application.dataPath + "/../Lua";
        }
    }

    public static string ABPath
    {
        get
        {
            return Application.dataPath + "/AB";
        }
    }

    public static object TestAccount { get { return "YK006"; } }

    public class CoreDef
    {
        /// <summary>
        /// 读取本地文件完成
        /// </summary>
        public const string READINFOED = "READINFOED";
        /// <summary>
        /// 热更初始化完成
        /// </summary>
        public const string INITHOTUPDATA = "INITHOTUPDATA";
        /// <summary>
        /// 安装完成
        /// </summary>
        public const string INSTALLATIONED = "CHECHINSTALLATION";

        /// <summary>
        /// 下载完成
        /// </summary>
        public const string DOWNED = "CHECKDOWN";

        /// <summary>
        /// 加载lua完成
        /// </summary>
        public const string LoadLuaFinished = "LOADLUAFINISHED";
        /// <summary>
        /// 开始游戏
        /// </summary>
        public const string StartGame = "StartGame";

        public const string CfgABName = "gamecfg.bytes";
        public const string CfgAssetName = "defaultres.json";
        public static string EditorResCfgPath = "Assets/AB/gamecfg/defaultres.json";

        public static string CfgExternalFileName
        {
            get
            {
                return AppConst.AppExternalDataPath + "/" + CfgABName;
            }
        }
    }

    /// <summary>
    /// lua文件有效的后缀名称
    /// </summary>
    public static List<string> LuaExtNames = new List<string>()
    {
       ".lua",
        ".spb"
    };

    


    /// <summary>
    /// 根据路径名获取一个有效的lua文件名
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string ToValidFileName(string rootPath,string fileName)
    {
        fileName = fileName.Replace("\\", "/");
        fileName = fileName.ToLower().Replace(rootPath.ToLower()+"/", "").Replace("/", "_");
        fileName = Path.GetFileNameWithoutExtension(fileName);
        fileName = fileName.ToLower();
        return fileName;
    }
}
