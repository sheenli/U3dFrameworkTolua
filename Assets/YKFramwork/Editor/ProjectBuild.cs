using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System;
using System.Reflection;


public class ProjectBuild
{
    /// <summary>
    /// 所有要打包的场景
    /// </summary>
    /// <value>The get build scenes.</value>
    public static string[] GetBuildScenes
    {
        get
        {
            List<string> list = new List<string>();
            foreach (EditorBuildSettingsScene es in EditorBuildSettings.scenes)
            {
                if (es != null && es.enabled)
                {
                    string fileName = Path.GetFileNameWithoutExtension(es.path);
                    list.Add(es.path);
                }
            }
            return list.ToArray();
        }
    }

    static BuildTargetGroup CurrenBuildTarget
    {
        get
        {
            string[] strs = System.Environment.GetCommandLineArgs();
            foreach (string str in strs)
            {
                if (str.StartsWith("type"))
                {
                    string stra = str.Replace("type=", "");

                    switch (stra)
                    {
                        case "AND":
                            return BuildTargetGroup.Android;
                        case "IOS":
                            return BuildTargetGroup.iOS;
                        default:
                            return BuildTargetGroup.Android;
                    }
                }
            }
#if UNITY_ANDROID
            return BuildTargetGroup.Android;
#elif UNITY_IOS
            return BuildTargetGroup.iOS;
#else
            return BuildTargetGroup.Standalone;
#endif
        }
    }

    private static string mDefVersion = "1.0.0";
	public static string version
    {
        get
        {
            string[] strs = System.Environment.GetCommandLineArgs();
            foreach (string str in strs)
            {
                if (str.StartsWith("version"))
                {
                    return str.Replace("version=", "");
                }

            }
            return mDefVersion;
        }
        set
        {
            mDefVersion = value;
        }
    }

    private static string mInstallVersion = "1.0.0";
    public static string InstallVersion
    {
        get
        {
            string[] strs = System.Environment.GetCommandLineArgs();
            foreach (string str in strs)
            {
                if (str.StartsWith("InstallVersion"))
                {
                    return str.Replace("InstallVersion=", "");
                }

            }
            return mInstallVersion;
        }
    }
    

    private static bool misPublic = false;
    /// <summary>
    /// 是否是发布版本
    /// </summary>
    public static bool isPublic
    {
        get
        {
            string[] strs = System.Environment.GetCommandLineArgs();
            foreach (string str in strs)
            {
                if (str.StartsWith("ispublic"))
                {
					return bool.Parse(str.Replace("ispublic=", ""));
                }

            }
            return misPublic;
        }
        set
        {
            misPublic = value;
        }
    }

    /// <summary>
    /// 生成到到路径
    /// </summary>
    /// <value>The outpath.</value>
    static string outpath
    {
        get
        {
            string[] strs = System.Environment.GetCommandLineArgs();
            foreach (string str in strs)
            {
                if (str.StartsWith("outpath"))
                {
                    return str.Replace("outpath=", "");
                }
            }
            string url = Path.GetFullPath(Application.dataPath + "/../Release");
#if UNITY_IOS
            return url+"/pro";
#elif UNITY_ANDROID
            return url + "/1.apk";
#else
            return url +"/1.exe";
#endif
        }
    }
    

    public enum ChannelType
    {
        Other = 1,
        AppStore,
    }

    public enum CollectionType
    {
        DSMJ = 8002,
        SQMJ = 8003,
        PXMJ = 8004,
        TEST = 0,
    }
    private static CollectionType mCurrentCollection = CollectionType.TEST;
    public static CollectionType currentCollection
    {
        get
        {
            string[] strs = System.Environment.GetCommandLineArgs();
            foreach (string str in strs)
            {
                if (str.StartsWith("Collection"))
                {
                    string channelStr = str.Replace("Collection=", "");
                    CollectionType type =(CollectionType)Enum.Parse(typeof(CollectionType), channelStr);
                    return type;
                }
            }
            return mCurrentCollection;
        }
        set
        {
            mCurrentCollection = value;
        }
    }

    /// <summary>
    /// 渠道类型
    /// </summary>
    static int Channel
    {
        get
        {
           
            string[] strs = System.Environment.GetCommandLineArgs();
            foreach (string str in strs)
            {
                if (str.StartsWith("channel"))
                {
                    string channelStr = str.Replace("channel=", "");
                    if(channelStr == "Other")
                    {
                        return (int)ChannelType.Other;
                    }
                    else if (channelStr == "AppStore")
                    {
                        return (int)ChannelType.AppStore;
                    }
                }
            }
            return (int)ChannelType.Other;
        }
    }

    public static void ClearLuaFiles()
    {
        ToLuaMenu.ClearLuaFiles();
    }

    public static void GenLuaAll()
    {

        ToLuaMenu.GenLuaAll();
    }


    public const string savecollection = "savecollectionkey";
    //[UnityEditor.MenuItem("Tools/bulid")]
    public static void BuildProjected()
    {
        //AssetDatabase.FindAssets(currentCollection +"t:"+ BuildCollectionResInfo.)

        BuildCollectionResInfo builfCfg = BuildCollectionResInfo.Find((int)currentCollection);
        if (builfCfg == null)
        {
            Debug.LogError("加载不到builfCfg    "+currentCollection);
            return;
        }

        if (builfCfg)
        {
            builfCfg.Build();
            SetLocalGameCfg(builfCfg);
        }
        Debug.Log("生成ab完成");
        BuildAB.BuildResIni();
        Debug.Log("准备生成APK");
        EditorPrefs.SetInt(savecollection, (int)currentCollection);
        //BuildAB.BuildAb();
        BuildPlayer(builfCfg, CurrenBuildTarget);
    }

    public static void BuildPCALL()
    {
        BuildProjected();
        System.Diagnostics.Process.Start("explorer", outpath.Substring(0, outpath.LastIndexOf("/")));
    }


    public static void DelectDir(string srcPath)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)            //判断是否文件夹
                {
                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                    subdir.Delete(true);          //删除子目录和文件
                }
                else
                {
                    File.Delete(i.FullName);      //删除指定文件
                }
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }

    //[MenuItem("Tools/Test")]
    public static void BuildDSMJ()
    {
        //         MethodInfo info = t.GetMethod("test");
        //         info.Invoke(null,new object[] { "aa" });
        //mBuildHall = true;
        //mIsBuilddsmj = true;
        //         string path = "Assets/YKFramwork/Editor/BuildAbInfo/Collections/" + 8002 + ".asset";
        //         BuildCollectionResInfo builfCfg = AssetDatabase.LoadAssetAtPath<BuildCollectionResInfo>(path);
        //         SetUmengXinXI(builfCfg);
    }

    public static void BuildPlayer(BuildCollectionResInfo buildInfo, BuildTargetGroup target)
    {
        BuildTarget ta = BuildTarget.StandaloneWindows;
        switch (target)
        {
            case BuildTargetGroup.Standalone:
                ta = BuildTarget.StandaloneWindows;
                break;
            case BuildTargetGroup.Android:
                ta = BuildTarget.Android;
                break;
            case BuildTargetGroup.iOS:
                ta = BuildTarget.iOS;
                break;
            default:
                ta = BuildTarget.StandaloneWindows;
                break;
        }
        BuildCfgAttributeInfo baf = null;
        List<BuildCfgAttributeInfo> buildcfgs = BuildCfgAttribute.GetAllAttribute(ta, Channel,
            buildInfo.CollectionID, out baf);
        string[] scs = null;
        BuildOptions bop = BuildOptions.None;
        if (baf != null)
        {
            PropertyInfo buildpro = baf.type.GetProperty("BuildScenes");
            scs = buildpro.GetValue(baf.obj, null) as string[];
            PropertyInfo op = baf.type.GetProperty("BuildOptions");
            bop = (BuildOptions)op.GetValue(baf.obj, null);
        }
        foreach (BuildCfgAttributeInfo cfg in buildcfgs)
        {
            cfg.obj.BuildBefore();
        }
        scs = scs == null ? GetBuildScenes : scs;
        
        BuildPipeline.BuildPlayer(scs, outpath, ta, bop);
    }

    public static void SetLocalGameCfg (BuildCollectionResInfo buildInfo)
    {
        LocalGameCfgData localdata = Resources.Load<LocalGameCfgData>("gamecfg");
        localdata.chanelType = Channel;
        localdata.isPublic = isPublic;
        localdata.collectionID = buildInfo.CollectionID;
        EditorUtility.SetDirty(localdata);
        AssetDatabase.SaveAssets();
    } 
    [UnityEditor.Callbacks.PostProcessBuild(100)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath)
    {
        BuildCfgAttributeInfo baf = null;
        int collection =  EditorPrefs.GetInt(savecollection, -1);
        if (collection == -1)
        {
            return;
        }
        List<BuildCfgAttributeInfo> buildcfgs = BuildCfgAttribute.GetAllAttribute(buildTarget, Channel,
            collection, out baf);
        foreach (BuildCfgAttributeInfo cfg in buildcfgs)
        {
            cfg.obj.BuildAfter(buildPath);
        }
    }
}
/*
public class BuildPC : EditorWindow
{
    [MenuItem("Tools/打开版本发布")]
    public static void CreateWind()
    {
        BuildPC pc = EditorWindow.CreateInstance<BuildPC>();
        pc.Show(true);
    }
    public String mDefVersion = "1.0.0";
    public bool misPublic = false;
    public BuildCollectionResInfo buildCfg = null;
    public void OnGUI()
    {
        GUILayout.BeginVertical();
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("要生成的版本号:");
                mDefVersion = GUILayout.TextField(mDefVersion);
            }GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                misPublic = GUILayout.Toggle(misPublic, "是否是发布版本");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                buildCfg = EditorGUILayout.ObjectField(new GUIContent("要生成的版本信息"), buildCfg, typeof(BuildCollectionResInfo),false) as BuildCollectionResInfo;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("生成"))
            {
                if (buildCfg == null)
                {
                    if(UnityEditor.EditorUtility.DisplayDialog("错误", "请选择要发布的东西", "确认"))
                    {
                        Debug.LogError("------------------");
                    }
                }
                else
                {
                    ProjectBuild.BuildPCALL(mDefVersion, misPublic, buildCfg);
                }
             }

        } GUILayout.EndVertical();
    }
}*/