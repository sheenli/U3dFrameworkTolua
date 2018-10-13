
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using LuaInterface;

public class LuaMgr //: EventDispatcherNode
{
    /// <summary>
    /// 目录对应的lua文件
    /// </summary>
    private Dictionary<string, Dictionary<string, byte[]>> mLuaFiles = new Dictionary<string, Dictionary<string, byte[]>>();

    //private Dictionary<string, byte[]> allLua = new Dictionary<string, byte[]>();

    private static LuaMgr mInstance;
    public static LuaMgr Instance
    {
        get
        {
            return mInstance = mInstance ?? new LuaMgr();
        }
    }

    public void Init()
    {
        new YKLoadLua();
    }

    public void OnDestroy()
    {
        if (mMainLua != null)
        {
            mMainLua.GetLuaFunction("OnDestroy").Call();
        }


        if (mMainLua != null) mMainLua.Dispose();
        if (this.mLua != null)
        {
            this.mLua.Dispose();
        }
    }

    internal void ShowExit()
    {
        if (mLua != null)
        {
            mMainLua.GetLuaFunction("OnShowExit").Call();
        }
    }

    public void AddFile(string path,AssetBundle ab)
    {
        path = path.ToLower();
#if UNITY_EDITOR
        if (AppConst.DebugMode)
        {
            string root = AppConst.LuaPath + "/" + path;
            Dictionary<string, byte[]> dic = new Dictionary<string, byte[]>();
            EeditorLoadLuaFile(root, root,ref dic);
            mLuaFiles[path] = dic;
            return;
        }
#endif
        path = Path.GetFileName(path);
        if (ab != null)
        {
            TextAsset[] text = ab.LoadAllAssets<TextAsset>();
            Dictionary<string, byte[]> dic = new Dictionary<string, byte[]>();
            foreach (TextAsset t in text)
            {
                dic.Add(t.name.ToLower(), t.bytes);
                Resources.UnloadAsset(t);
            }
            mLuaFiles[path] = dic;
            ABMgr.Instance.UnLoadAB(path);
        }
    }

  

    

    public byte[] GetLuaFileByte(string fileName)
    {
        foreach (Dictionary<string,byte[]> dic in this.mLuaFiles.Values)
        {
            if (dic.ContainsKey(fileName))
            {
                return dic[fileName];
            }
        }

        Debug.LogWarning("lua文件查找失败 fileName="+fileName);
        return null;
    }

    public LuaState mLua = null;
    private LuaTable mMainLua = null;
    public void StartGame()
    {
        Debug.Log("开始游戏");
        mLua = new LuaState();
        mLua.Start();

        mLua.OpenLibs(LuaDLL.luaopen_pb);
        mLua.OpenLibs(LuaDLL.luaopen_struct);
        mLua.OpenLibs(LuaDLL.luaopen_lpeg);
        OpenCJson();
        mLua.LuaSetTop(0);
        LuaBinder.Bind(mLua);
        DelegateFactory.Init();
        LuaCoroutine.Register(mLua, UIMgr.Instance);
        mLua.DoFile("Main.lua");
        mMainLua = mLua.Require<LuaTable>("Main");
        LuaFunction luafun = mMainLua.GetLuaFunction("Main");
        luafun.BeginPCall();
        luafun.PCall();
        luafun.EndPCall();
        luafun.Dispose();
//         mLua.AddLoader((ref string filename) =>
//         {
//              string fileName = filename.ToLower();
//             if (fileName.EndsWith(".lua"))
//             {
//                 fileName = fileName.Substring(0, fileName.Length - 4);
//             }
//             fileName = fileName.Replace("/", "_").Replace(".", "_");
// 
//             //fileName = Path.GetFileNameWithoutExtension(fileName);
// 
//             byte[] buffer = LuaMgr.Instance.GetLuaFileByte(fileName);
//             return buffer;
//         });
//         mLua.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
//         mLua.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
//         mLua.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);
// 
//         object[] obj = mLua.DoString("return require(\"Main\")");
//         mMainLua = obj[0] as LuaTable;
//          mMainLua.Get<LuaFunction>("Main").Call();
//lua.Dispose();
    }

    [NoToLua]
    public void EeditorLoadLuaFile(string root,string luaPath,ref Dictionary<string,byte[]> dic)
    {
        string[] files = Directory.GetFiles(luaPath);
        foreach (string file in files)
        {
            
            if (!AppConst.LuaExtNames.Contains(Path.GetExtension(file)))
            {
                continue;
            }

            string fileName = AppConst.ToValidFileName(root, file);
            if (dic.ContainsKey(fileName))
            {
                Debug.LogError("发现重复的lua文件 名称为"+fileName);
            }
            dic[fileName] = File.ReadAllBytes(file);
        }

        string[] dirs = Directory.GetDirectories(luaPath);
        if (dirs == null || dirs.Length <= 0)
        {
            return;
        }
        foreach (string dir in dirs)
        {
            EeditorLoadLuaFile(root, dir,ref dic);
        }
    }
    protected void OpenCJson()
    {
        mLua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
        mLua.OpenLibs(LuaDLL.luaopen_cjson);
        mLua.LuaSetField(-2, "cjson");

        mLua.OpenLibs(LuaDLL.luaopen_cjson_safe);
        mLua.LuaSetField(-2, "cjson.safe");
    }
    public void GC()
    {
        if (mLua != null)
        {
            mLua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }
    }

}
public class YKLoadLua : LuaFileUtils
{
    public YKLoadLua()
    {
        instance = this;
        beZip = true;
    }

    public override byte[] ReadFile(string fileName)
    {
        fileName = fileName.ToLower();
        if (fileName.EndsWith(".lua"))
        {
            fileName = fileName.Substring(0, fileName.Length - 4);
        }
        fileName = fileName.Replace("/", "_").Replace(".", "_");

        //fileName = Path.GetFileNameWithoutExtension(fileName);

        byte[] buffer = LuaMgr.Instance.GetLuaFileByte(fileName);
        return buffer;
    }
}