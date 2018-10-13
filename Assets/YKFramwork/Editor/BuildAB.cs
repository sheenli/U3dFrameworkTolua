using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Threading;
using System.Text;

[InitializeOnLoad]
public class BuildAB
{
    public static List<string> mustNeedAddRes = new List<string>()
    {
        "gamecfg",
        "hall",
        "tolua",
    };
    public static bool BuildABFinish = false;
    
    static BuildAB()
    {
    }

    public static List<string> BuildSearchPattern = new List<string>()
    {
        ".json",
        ".txt",
        ".lua",
        ".jpg",
        ".png",
        ".mp3",
        ".prefab",
        ".bytes",
        ".wav"
    };

    static List<AssetBundleBuild> maps = new List<AssetBundleBuild>();


//    [MenuItem("Tools/生成当前平台下的ab", false, 101)]
//    public static void BuildAb()
//    {
//        BuildABFinish = false;
        
//        string streamPath = Application.streamingAssetsPath;
//        if (Directory.Exists(streamPath))
//        {
//            Directory.Delete(streamPath, true);
//        }
//        Directory.CreateDirectory(streamPath);
//        AssetDatabase.SaveAssets();
//        CopyLuaFile();

//        CopyToStreamingAssets();


//        maps.Clear();
//        HandleBundleAB(AppConst.ABPath);
//        string resPath = FileUtil.GetProjectRelativePath(Application.streamingAssetsPath);
//        BuildAssetBundleOptions options = BuildAssetBundleOptions.UncompressedAssetBundle |BuildAssetBundleOptions.DeterministicAssetBundle;

//        BuildTarget traget = BuildTarget.StandaloneWindows;
//#if UNITY_ANDROID
//        traget = BuildTarget.Android;
//#elif UNITY_IOS
//        traget = BuildTarget.iOS;
//#else
//        traget = BuildTarget.StandaloneWindows;
//#endif
       
//        BuildPipeline.BuildAssetBundles(resPath, maps.ToArray(), options, traget);
//        CompressFiles();
        

//    }

    private static Action mBulidFinish = null;
    public static void BulidAb(Action finish)
    {
        mBulidFinish = finish;
        //BuildAb();
    }

    private static Dictionary<string, string> resFiles = new Dictionary<string, string>();

    //[MenuItem("Tools/生成resource的引用关系", false, 102)]
    public static void BuildResIni()
    {
        resFiles.Clear();
        List<string> allText = new List<string>();
        HandleBundleRes(Application.dataPath + "/Resources/");
        foreach (KeyValuePair<string,string> kv in resFiles)
        {
            allText.Add(kv.Key + "|" + kv.Value);
        }
        File.WriteAllLines(Application.dataPath + "/Resources/AssetCorrespondPath.txt", allText.ToArray());
        AssetDatabase.Refresh();
        Debug.LogWarning("生成resource的对应关系完成");
    }

    /// <summary>
    /// 生成resource的对应关系
    /// </summary>
    /// <param name="path"></param>
    static void HandleBundleRes(string path)
    {
       
        string[] names = Directory.GetFiles(path);
        if (names.Length > 0)
        {
            string pathT = path.Replace("\\", "/").Replace(Application.dataPath + "/Resources/", "");
            foreach (string fileName in names)
            {
                if (Path.GetExtension(fileName) != ".meta")
                {
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    resFiles.Add(name, pathT);
                }
            }
            //AddBuildMap(Path.GetFileName(path + ".bytes"), BuildSearchPattern, path);
        }
        string[] dirs = Directory.GetDirectories(path);

        foreach (string dir in dirs)
        {
            HandleBundleRes(dir);
        }
    }

    /// <summary>
    /// 添加文件夹到要生成的字典里面
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="pattern"></param>
    /// <param name="path"></param>
    static void AddBuildMap(string bundleName, List<string> pattern, string path,ref Dictionary<string, AssetBundleBuild> dic)
    {
        string[] files = Directory.GetFiles(path);
        if (files.Length == 0) return;
        List<string> buileFiles = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            if (pattern.Contains(Path.GetExtension(files[i])))
            {
                buileFiles.Add(FileUtil.GetProjectRelativePath(files[i].Replace('\\', '/')));
            }            
        }
        if (buileFiles.Count > 0)
        {
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = bundleName.ToLower();
            build.assetNames = buileFiles.ToArray();
            if (!dic.ContainsKey(bundleName))
            {
                dic.Add(bundleName, build);
            }
            else
            {
                Debug.LogError("有重复的AB abName =" + bundleName);
            }
        }
       
    }

    public static void HandleBundleAB(string path,ref Dictionary<string, AssetBundleBuild> dic)
    {
        string[] names = Directory.GetFiles(path);
        if(names.Length > 0)
        {
            AddBuildMap(Path.GetFileName(path + ".bytes"), BuildSearchPattern, path,ref dic);
        }
        string[] dirs = Directory.GetDirectories(path);

        foreach (string dir in dirs)
        {
            HandleBundleAB(dir,ref dic);
        }
    }

    public static void BulidAssetCorrespondABFileName()
    {
        VerInfo infos = new VerInfo();
		infos.ver = ProjectBuild.version;
        foreach (AssetBundleBuild ab in maps)
        {
            ABInfo info = new ABInfo();
            info.fileName = ab.assetBundleName;
            info.sha1 = GenHashOne(Application.streamingAssetsPath + "/" + ab.assetBundleName, ref info.length);
            infos.files.Add(info);
            foreach (string str in ab.assetNames)
            {
                if (ab.assetBundleName != "lua")
                {
                    info.assets.Add(Path.GetFileName(str));
                }
            }
        }
        
        File.WriteAllText(Application.streamingAssetsPath + "/file.txt", JsonUtility.ToJson(infos,true));
    }


    static System.Security.Cryptography.SHA1CryptoServiceProvider osha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
    public static string GenHashOne(string filename,ref long lenth)
    {
        var shash = "";
        using (System.IO.Stream s = System.IO.File.OpenRead(filename))
        {
            var hash = osha1.ComputeHash(s);
            shash = Convert.ToBase64String(hash);
            lenth = s.Length;
        }
        return shash;
    }
    

    /// <summary>
    /// 文件夹拷贝
    /// </summary>
    /// <param name="srcFolderPath"></param>
    /// <param name="destFolderPath"></param>
    public static void FolderCopy(string srcFolderPath, string destFolderPath)
    {
        //检查目标目录是否以目标分隔符结束，如果不是则添加之
        if (destFolderPath[destFolderPath.Length - 1] != Path.DirectorySeparatorChar)
            destFolderPath += Path.DirectorySeparatorChar;
        //判断目标目录是否存在，如果不在则创建之
        if (!Directory.Exists(destFolderPath))
            Directory.CreateDirectory(destFolderPath);
        string[] fileList = Directory.GetFileSystemEntries(srcFolderPath);
        foreach (string file in fileList)
        {
            if (Directory.Exists(file))
                FolderCopy(file, destFolderPath + Path.GetFileName(file));
            else
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)//改变只读文件属性，否则删不掉
                    fi.Attributes = FileAttributes.Normal;
                try
                { File.Copy(file, destFolderPath + Path.GetFileName(file), true); }
                catch (Exception e)
                {

                }
            }

        }
    }


    /// <summary>
    /// 生成某个配置文件的AB
    /// </summary>
    /// <param name="info"></param>
    public static void BuildResByInfo(BuildResInfo info,int flag)
    {
        string fileName = Application.streamingAssetsPath + "/0ver.txt";
        VerInfo oldVer = null;
        if (File.Exists(fileName))
        {
            oldVer = JsonUtility.FromJson<VerInfo>(File.ReadAllText(fileName));
        }
        BuildInfoAB build = new BuildInfoAB(info);
        List<VerInfo> newInfo = build.BuildAb(flag);
        if (oldVer != null)
        {
            foreach (VerInfo verinfo in newInfo)
            {
                foreach (ABInfo ver in verinfo.files)
                {
                    bool has = false;
                    foreach (ABInfo _ver in oldVer.files)
                    {
                        if (ver.fileName == _ver.fileName)
                        {
                            _ver.length = ver.length;
                            _ver.assets = ver.assets;
                            _ver.sha1 = ver.sha1;
                            has = true;
                        }
                    }
                    if (!has)
                    {
                        oldVer.files.Add(ver);
                    }
                }
            }
        }
        else
        {
            VerInfo buildinfo = new VerInfo();
            foreach (VerInfo ver in newInfo)
            {
                buildinfo.ver = ver.ver;
                buildinfo.files.AddRange(ver.files);
            }
            oldVer = buildinfo;
        }
        string txtStr = JsonUtility.ToJson(oldVer, true);
        File.WriteAllText(fileName, txtStr);
    }

    public static void BuildCollectionsResByInfo(BuildCollectionResInfo info)
    {
        string fileName = Application.streamingAssetsPath + "/" + info.CollectionID + "ver.txt";
        List<VerInfo> allList = new List<VerInfo>();
        List<BuildResInfo> list = new List<BuildResInfo>(info.builds);
        for (int i = list.Count-1;i>=0;i--)
        {
            BuildInfoAB build = new BuildInfoAB(list[i]);
            List<VerInfo> infos = build.BuildAb(2);
            foreach (VerInfo _v in infos)
            {
                if (_v != null)
                allList.Add(_v);
            }
        }
        VerInfo buildinfo = new VerInfo();
        foreach (VerInfo ver in allList)
        {
            buildinfo.ver = ver.ver;
            buildinfo.files.AddRange(ver.files);
        }
        string txtStr = JsonUtility.ToJson(buildinfo, true);
        
        File.WriteAllText(fileName, txtStr);
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }
}



public class CompressBytesLZMAInfo
{
    public static float Progress
    {
        get { return (float)currentNum / (float)allCout; }
    }

    public CompressBytesLZMAInfo()
    {
        allCout++;
    }
    public static int allCout = 0;
    public static int currentNum = 0;
    public string fileName = "";

    public void CompressBytesLZMA()
    {
        CompressHelper.CompressBytesLZMA(File.ReadAllBytes(fileName), fileName);
        currentNum++;
    }
}
public class BuildInfoAB
{
    public string outPath = "";
    private string luaFileRoot = "";
    private string saveLuaFileRoot = "";
    public BuildInfoAB(BuildResInfo ins)
    {
        m_BuildResInfo = ins;
    }
    public BuildResInfo m_BuildResInfo = null;
    public static string luaPath = AppConst.ABPath + "/Lua";

    /// <summary>
    /// 需要生成的AB信息
    /// </summary>
    //private Dictionary<string,AssetBundleBuild> maps = new Dictionary<string,AssetBundleBuild>();

    /// <summary>
    /// lua文件
    /// </summary>
    private static List<string> luaFiles = new List<string>();

    public void CopyLuaFile()
    {
        if (Directory.Exists(luaPath))
        {
            Directory.Delete(luaPath, true);
        }
        Directory.CreateDirectory(luaPath);
        luaFiles.Clear();
        RecursivelyCopyLua(this.luaFileRoot);
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
    }

    public List<VerInfo> BuildAb(int all)
    {
        List<VerInfo> list = new List<VerInfo>();
        EditorUtility.ClearProgressBar();
        outPath = Application.streamingAssetsPath;

        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
            AssetDatabase.SaveAssets();
        }
       
        string resPath = FileUtil.GetProjectRelativePath(outPath);

        BuildAssetBundleOptions options = BuildAssetBundleOptions.UncompressedAssetBundle
            | BuildAssetBundleOptions.DeterministicAssetBundle
            | BuildAssetBundleOptions.ForceRebuildAssetBundle;

        BuildTarget traget = EditorUserBuildSettings.activeBuildTarget;
        if (all == 0)
        {
            VerInfo info = BuildLua(resPath, traget, options);
            list.Add(info);
        }
        else if (all == 1)
        {
            VerInfo info = BuildRes(resPath, traget, options);
            list.Add(info);
        }
        else
        {
            VerInfo resinfo = BuildRes(resPath, traget, options);
            VerInfo luainfo = BuildLua(resPath, traget, options);
            if (luainfo != null) list.Add(luainfo);
            if (resinfo != null) list.Add(resinfo);
        }
        return list;
    }

    public VerInfo BuildLua(string rootPath, BuildTarget traget,
        BuildAssetBundleOptions options)
    {
        if (string.IsNullOrEmpty(m_BuildResInfo.LuaParentPath))
        {
            return null;
        }

        //string luaABName = m_BuildResInfo.ResName.ToLower() + "lua";
        saveLuaFileRoot = AppConst.ABPath + "/Lua/" + m_BuildResInfo.LuaABName.ToLower();
        luaFileRoot = AppConst.LuaPath + "/" + m_BuildResInfo.LuaParentPath.ToLower();

        CopyLuaFile();
        Dictionary<string, AssetBundleBuild> maps = new Dictionary<string, AssetBundleBuild>();
        List<string> luaBuildFiles = new List<string>();
        foreach (string fileName in luaFiles)
        {
            string luaFileName = FileUtil.GetProjectRelativePath(saveLuaFileRoot + "/" + fileName.Replace('\\', '/') + ".txt");
            luaBuildFiles.Add(luaFileName);
        }
        if (luaFiles.Count > 0)
        {
            AssetBundleBuild luaab = new AssetBundleBuild();
            luaab.assetBundleName = m_BuildResInfo.LuaABName+".bytes";
            luaab.assetNames = luaBuildFiles.ToArray();
            maps.Add(m_BuildResInfo.LuaABName, luaab);
        }
        else
        {
            return null;
        }

        List<AssetBundleBuild> list = new List<AssetBundleBuild>();
        foreach (AssetBundleBuild abb in maps.Values)
        {
            list.Add(abb);
        }
        BuildPipeline.BuildAssetBundles(rootPath, list.ToArray(), options, traget);
        CompressFiles(list);
        return BuildFileTxt(list);
    }

    public VerInfo BuildRes(string rootPath, BuildTarget traget, BuildAssetBundleOptions options)
    {
        if (string.IsNullOrEmpty(m_BuildResInfo.ResParentPath))
        {
            return null;
        }
        Dictionary<string, AssetBundleBuild> maps = new Dictionary<string, AssetBundleBuild>();
        BuildAB.HandleBundleAB(AppConst.ABPath + m_BuildResInfo.ResParentPath, ref maps);
        List<AssetBundleBuild> list = new List<AssetBundleBuild>();
        ResCfg resCfg = null;
        if (File.Exists(AppConst.CoreDef.EditorResCfgPath))
        {
            TextAsset resCfgtext = AssetDatabase.LoadAssetAtPath<TextAsset>(AppConst.CoreDef.EditorResCfgPath);
            resCfg = JsonUtility.FromJson<ResCfg>(resCfgtext.text);
        }
        foreach (string abName in maps.Keys)
        {
            AssetBundleBuild abb = maps[abName];
            List<string> files = new List<string>();
            foreach (string file in abb.assetNames)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                if (name == "defaultres" ||( resCfg != null && resCfg.GetResInfo(name) != null))
                {
                    files.Add(file); 
                }
                else
                {
                    Debug.LogError("有文件放在ab文件夹下  但是没有加入到配置表" +
                        ",可能是无效的资源将不被打包到ab abName=" + name);
                }
            }
            abb.assetNames = files.ToArray();
            list.Add(abb);
        }

        BuildPipeline.BuildAssetBundles(rootPath, list.ToArray(), options, traget);
        CompressFiles(list);
        return BuildFileTxt(list);
    }


    public void CompressFiles(List<AssetBundleBuild> list)
    {
        //EditorUtility.ClearProgressBar();
        EditorUtility.DisplayProgressBar("压缩资源", "压缩中", 0.1f);
        foreach (AssetBundleBuild ab in list)
        {
            string fileName = outPath + "/" + ab.assetBundleName;
            CompressBytesLZMAInfo info = new CompressBytesLZMAInfo();
            info.fileName = fileName;
            info.CompressBytesLZMA();
        }
        EditorUtility.ClearProgressBar();
    }

    public VerInfo BuildFileTxt(List<AssetBundleBuild>list)
    {
        //string filePath = outPath + "/"+m_BuildResInfo.ResName+".txt";
        VerInfo infos = new VerInfo();
        infos.ver = ProjectBuild.version;
        foreach (AssetBundleBuild ab in list)
        {
            for (int i = infos.files.Count - 1; i >= 0; i--)
            {
                if (infos.files[i].fileName == ab.assetBundleName)
                {
                    infos.files.RemoveAt(i);
                }
            }
            ABInfo info = new ABInfo();
            info.fileName = ab.assetBundleName;
            info.sha1 = BuildAB.GenHashOne(outPath + "/" + ab.assetBundleName, ref info.length);
            infos.files.Add(info);
            foreach (string str in ab.assetNames)
            {
                info.assets.Add(Path.GetFileName(str));
            }
        }
        return infos;
        //File.WriteAllText(filePath , JsonUtility.ToJson(infos, true));
    }

    #region 公共函数
    public  void RecursivelyCopyLua(string rootDir)
    {
       
        if (!Directory.Exists(saveLuaFileRoot))
        {
            Directory.CreateDirectory(saveLuaFileRoot);
        }
        string[] files = Directory.GetFiles(rootDir);
        string[] dirs = Directory.GetDirectories(rootDir);

        foreach (string file in files)
        {
            string d = Path.GetExtension(file);
            if (AppConst.LuaExtNames.Contains(d))
            {
                
                string fileName = AppConst.ToValidFileName(luaFileRoot, file);
                if (luaFiles.Contains(fileName))
                {
                    Debug.LogError("发现重复文件 文件名为 file =" + file);
                }
                else
                {
                    luaFiles.Add(fileName);
                   
                    if (d == ".lua")
                    {
                        string bs = File.ReadAllText(file);
                        File.WriteAllText(saveLuaFileRoot + "/" + fileName + ".txt", bs, new UTF8Encoding(true));
                    }
                    else
                    {
                        File.Copy(file, saveLuaFileRoot + "/" + fileName + ".txt", true);
                    }
                }
            }
        }
        if (dirs == null || dirs.Length <= 0) return;
        foreach (string dir in dirs)
        {
            RecursivelyCopyLua(dir);
        }
    }
    #endregion
}