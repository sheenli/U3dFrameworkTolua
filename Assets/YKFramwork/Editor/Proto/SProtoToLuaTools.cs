using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

public class SProtoToLuaTools : MonoBehaviour
{
    [MenuItem("Tools/将sproto文件转化为lua文件", priority = 2050)]
    public static void SprotoToLuaScript()
    {
        string pathStr = Application.dataPath + "/../Lua/HallLua/Proto/sproto/sproto_sources";//文件路径
        if (!Directory.Exists(pathStr))
        {
            Debug.LogError("pathStr不存在 " + pathStr);
            return;
        }

        List<ProtoDataBase> allProto = SprotoToDataByDir(pathStr);
        string lua = GetSprotoToLua(allProto);
        string savePath = Application.dataPath + "/../Lua/HallLua/Proto/SprotoToLua.lua";
        SaveToLua(savePath, lua);
    }
    /*[MenuItem("Tools/生成Lua提示文件", priority = 2050)]
    public static void BuildSprotoApi()
    {
        string pathStr = Application.dataPath + "/../Lua/HallLua/Proto/sproto/sproto_sources";//文件路径
        if (!Directory.Exists(pathStr))
        {
            Debug.LogError("pathStr不存在 " + pathStr);
            return;
        }
        Debug.Log("生成Wrap提示 Start");
        Filter<Type> baseFilter = new GeneralFilter<Type>(ToLuaMenu.baseType);
        Filter<Type> dropFilter = new GeneralFilter<Type>(ToLuaMenu.dropType);

        var collection = new BindTypeCollection(CustomSettings.customTypeList);
        var bindTypes = collection.CollectBindType(baseFilter, dropFilter);
        foreach (var bindType in bindTypes)
        {
            var generator = new LuaAPIGenerator();
            generator.Gen(bindType);
        }
        Debug.Log("生成Wrap提示 End");
        Debug.Log("生成Sproto提示 Start");

        List<ProtoDataBase> allProto = SprotoToDataByDir(pathStr);
        string lua = GetSprotoToLuaApi(allProto);
        string savePath = Application.dataPath + "/../LuaAPI/SprotoToLuaTips.lua";
        SaveToLua(savePath, lua);
        Debug.Log("生成Sproto提示 End");

        Debug.Log("打包成ZIP Start");
        string luaApiPath = Application.dataPath + "/../LuaAPI";
        string luaApiPathZip = Application.dataPath + "/../LuaApi.zip";
        ZipHelper.CreateZip(@luaApiPath, @luaApiPathZip);
        Debug.Log("打包成ZIP End");
        Debug.Log("全部完成");
        //CompressHelper.CompressBytesLZMA(File.ReadAllBytes(fileName), fileName);
    }*/

    #region 解析成lua
    public static string scriptName = "SprotoPack";
    public static Dictionary<string, string> allType;
    /// <summary>
    /// 保存到路径
    /// </summary>
    /// <param name="savePath"></param>
    /// <param name="content"></param>
    public static void SaveToLua(string savePath, string content)
    {
        Encoding encodingUTF8 = new UTF8Encoding(true);
        File.WriteAllText(@savePath, content, encodingUTF8);
    }

    /// <summary>
    /// 将所有数据包生成lua
    /// </summary>
    /// <param name="dataList"></param>
    /// <returns></returns>
    public static string GetSprotoToLua(List<ProtoDataBase> dataList)
    {
        StringBuilder results = new StringBuilder();
        results.Append(scriptName + "={}\n");
        GetSprotoAllType(dataList);
        foreach (ProtoDataBase protoData in dataList)
        {
            if (protoData is ProtoPackData)
            {
                string packName = scriptName + "." + protoData.name;
                results.Append(string.Format("-------------------------------{0}模块-----------------------------------\n", packName));
                ProtoPackData packData = protoData as ProtoPackData;
                results.Append(GetSprotoPackAllInfoToLua(packData));
                results.Append(packName + " = {}\n\n");
                results.Append(GetSprotoInfoToLua(packData.infos, protoData.name));
            }
            else if (protoData is ProtoInfoData)
            {
                results.Append(string.Format("-------------------------------{0}对象-----------------------------------\n", protoData.name));
                var infoData = protoData as ProtoInfoData;
                results.Append(GetSprotoInfoToLua(new List<ProtoInfoData> { infoData }));
            }
        }
        return results.ToString();
    }
    /// <summary>
    /// 获取协议包所有注释
    /// </summary>
    /// <returns></returns>
    public static string GetSprotoPackAllInfoToLua(ProtoPackData packData)
    {
        StringBuilder results = new StringBuilder();
        string packName = scriptName + "." + packData.name;

        results.Append(string.Format("---@type {0} \n", packName));
        return results.ToString();
    }
    /// <summary>
    /// 获取协议组合lua
    /// </summary>
    /// <param name="dataList"></param>
    /// <param name="packName"></param>
    /// <returns></returns>
    public static string GetSprotoInfoToLua(List<ProtoInfoData> dataList, string packName = "")
    {
        StringBuilder results = new StringBuilder();
        foreach (ProtoInfoData info in dataList)
        {
            string infoName = scriptName + "." + info.name;
            if (!string.IsNullOrEmpty(packName))
            {
                infoName = scriptName + "." + packName + "." + info.name;
            }

            results.Append(string.Format("---@type {0} \n", infoName));
            results.Append(infoName + " = {}");

            results.Append("\n\n");
            results.Append(string.Format("---@return {0}\n", infoName));
            results.Append(string.Format("function {0}.New()\n", infoName));
            results.Append("\tlocal data = { \n");
            //填充参数2
            foreach (ProtoParamData param in info.parameters)
            {
                results.Append(SetParametersDefaultValue(param));
                //results.Append(string.Format("\t\t{0} = nil,\n",param.name));
            }
            results.Append("\t}\n");
            results.Append("\treturn data;\n");
            results.Append("end \n\n\n");
        }
        return results.ToString();
    }
    /// <summary>
    /// 设置协议参数默认值并返回
    /// </summary>
    public static string SetParametersDefaultValue(ProtoParamData param)
    {
        string str = "\t\t{0} = {1},\n";//string.Format("\t\t{0} = nil,\n", param.name);
        string type = "nil";
        if (!param.isArr)
        {
            if (param.type.Equals("integer"))
            {
                type = "0";
            }
            else if (param.type.Equals("string"))
            {
                type = "\"\"";
            }
            else if (param.type.Equals("boolean"))
            {
                type = "false";
            }

        }

        str = string.Format(str, param.name, type);
        return str;
    }
    /// <summary>
    /// 缓存所有类型
    /// </summary>
    /// <param name="dataList"></param>
    public static void GetSprotoAllType(List<ProtoDataBase> dataList)
    {
        allType = new Dictionary<string, string>();
        foreach (ProtoDataBase protoData in dataList)
        {

            if (protoData is ProtoPackData)
            {
                string packName = scriptName + "." + protoData.name;
                ProtoPackData data = protoData as ProtoPackData;
                foreach (ProtoInfoData info in data.infos)
                {
                    string infoPackName = packName + "." + info.name;
                    string key = protoData.name + "." + info.name;
                    if (!allType.ContainsKey(key))
                        allType.Add(key, infoPackName);
                }
            }
            else if (protoData is ProtoInfoData)
            {
                string packName = scriptName + "." + protoData.name;
                if (!allType.ContainsKey(protoData.name))
                    allType.Add(protoData.name, packName);
            }
        }

    }
    #endregion

    #region 解析成LuaAPI

    /// <summary>
    /// 将所有数据包生成lua
    /// </summary>
    /// <param name="dataList"></param>
    /// <returns></returns>
    public static string GetSprotoToLuaApi(List<ProtoDataBase> dataList)
    {
        StringBuilder results = new StringBuilder();
        //results.Append(scriptName + "={}\n");
        GetSprotoAllType(dataList);
        foreach (ProtoDataBase protoData in dataList)
        {
            if (protoData is ProtoPackData)
            {
                string packName = scriptName + "." + protoData.name;
                results.Append(string.Format("-------------------------------{0}模块-----------------------------------\n", packName));
                ProtoPackData packData = protoData as ProtoPackData;
                results.Append(GetSprotoPackAllInfoToLuaApi(packData));
                //results.Append(packName + " = {}\n\n");
                results.Append(GetSprotoInfoToLuaApi(packData.infos, protoData.name));
            }
            else if (protoData is ProtoInfoData)
            {
                results.Append(string.Format("-------------------------------{0}对象-----------------------------------\n", protoData.name));
                var infoData = protoData as ProtoInfoData;
                results.Append(GetSprotoInfoToLuaApi(new List<ProtoInfoData> { infoData }));
            }
        }
        return results.ToString();
    }
    /// <summary>
    /// 获取协议包所有注释
    /// </summary>
    /// <returns></returns>
    public static string GetSprotoPackAllInfoToLuaApi(ProtoPackData packData)
    {
        StringBuilder results = new StringBuilder();
        string packName = scriptName + "." + packData.name;
        results.Append(string.Format("---@class {0}\n", packName));
        foreach (ProtoInfoData info in packData.infos)
        {
            results.Append(string.Format("---@field {0} {1} \n", info.name, packName + "." + info.name));
        }
        results.Append("\n\n\n");
        return results.ToString();
    }
    /// <summary>
    /// 获取协议组合lua
    /// </summary>
    /// <param name="dataList"></param>
    /// <param name="packName"></param>
    /// <returns></returns>
    public static string GetSprotoInfoToLuaApi(List<ProtoInfoData> dataList, string packName = "")
    {
        StringBuilder results = new StringBuilder();
        foreach (ProtoInfoData info in dataList)
        {
            string infoName = scriptName + "." + info.name;
            string fastName = string.Empty;
            if (!string.IsNullOrEmpty(packName))
            {
                infoName = scriptName + "." + packName + "." + info.name;
                fastName = packName + ".";
            }

            results.Append(string.Format("---@class {0}\n", infoName));
            //填充参数
            foreach (ProtoParamData param in info.parameters)
            {
                string type = fastName + param.type;
                if (type.Contains("integer"))
                {
                    type = "number";
                }
                else if (type.Contains("boolean"))
                {
                    type = "bool";
                }
                if (allType.ContainsKey(type))
                {
                    type = allType[type];
                }
                if (param.isArr)
                {
                    type += "[]";
                }
                results.Append(string.Format("---@field {0} {1} \n", param.name, type));
            }

            //results.Append(infoName + " = {}");
            results.Append("\n\n");
            //results.Append(string.Format("---@return {0}\n", infoName));
            //results.Append(string.Format("function {0}.New() end\n", infoName));
        }
        return results.ToString();
    }


    #endregion

    #region 解析成数据类
    public static List<ProtoDataBase> SprotoToDataByDir(string directoryPath)
    {
        //E:/u3d/DSMahjong/Project/Lua/HallLua/Proto/sproto/sproto_sources
        DirectoryInfo rootDirInfo = new DirectoryInfo(@directoryPath);
        FileInfo[] files = rootDirInfo.GetFiles();
        List<ProtoDataBase> protoList = new List<ProtoDataBase>();
        foreach (FileInfo file in files)
        {
            Console.WriteLine(file.FullName);
            var data = SprotoToDataByFile(file.FullName);
            if (file.FullName.Contains("ddz"))
            {
                Console.WriteLine("123");
            }
            protoList.AddRange(data);
        }

        return protoList;
    }
    /// <summary>
    /// 通过文件路径将sproto转换成数据类
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static List<ProtoDataBase> SprotoToDataByFile(string filePath)
    {
        var content = string.Empty;
        string[] arr = File.ReadAllLines(@filePath);
        List<string> list = new List<string>();

        foreach (var s in arr)
        {
            //1 移除一些非法的行
            if (s.Trim().StartsWith("#") || s.Trim() == "")
            {
                continue;
            }
            string str = s;
            if (str.Contains("#"))
            {
                str = str.Split('#')[0].TrimEnd();
            }
            list.Add(str);
            content += str + "\r\n";
        }
        List<ProtoDataBase> protoList = new List<ProtoDataBase>();
        //Console.WriteLine(content);
        var word = GetStrContent(content);
        Console.WriteLine(word.Count);
        foreach (var s in word)
        {
            int count = 0;
            foreach (char c in s)
            {
                if (c.Equals('{'))
                {
                    count++;
                }
            }
            if (count > 1)
            {
                //是协议包
                ProtoPackData pack = new ProtoPackData();
                pack.name = GetProtoName(s);
                pack.infos = new List<ProtoInfoData>();
                var word2 = GetStrContent(s);
                foreach (var s2 in word2)
                {
                    ProtoInfoData info = new ProtoInfoData();
                    info.name = GetProtoName(s2);
                    info.parameters = GetParameters(s2);
                    pack.infos.Add(info);
                }
                protoList.Add(pack);
            }
            else
            {
                ProtoInfoData info = new ProtoInfoData();
                info.name = GetProtoName(s);
                info.parameters = GetParameters(s);
                protoList.Add(info);
            }
        }
        return protoList;
    }
    /// <summary>
    /// 解析协议字段
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static List<ProtoParamData> GetParameters(string content)
    {
        List<ProtoParamData> paramList = new List<ProtoParamData>();
        var paramArr = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var p in paramArr)
        {
            if (!p.Contains(":"))
            {
                continue;
            }
            ProtoParamData paramData = new ProtoParamData();
            paramData.isArr = false;
            string con = p.Trim();
            if (p.Contains("#"))//去掉注释
            {
                con = con.Split('#')[0].Trim();
            }
            var nameOrType = con.Split(':');
            //处理名字
            string name = nameOrType[0];
            string type = nameOrType[1].Trim();
            name = RemoveStrNumber(name).Trim();//利用正则过滤掉序号
            if (type.Contains("*"))//是数组或字典
            {
                if (type.Contains("("))//字典
                {
                    var typeArr = type.Split('(');
                    type = typeArr[0].Trim();
                }
                type = type.Replace("*", "");
                paramData.isArr = true;

            }
            paramData.name = name;
            paramData.type = type;
            paramList.Add(paramData);
        }
        return paramList;
    }
    /// <summary>
    /// 去掉字符串中的数字
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string RemoveStrNumber(string key)
    {
        return System.Text.RegularExpressions.Regex.Replace(key, @"\d", "");
    }
    public static string GetProtoName(string line)
    {
        var arr = line.Split('{');
        return arr[0].Trim();
    }
    public static List<string> GetStrContent(string str)
    {
        List<string> word = new List<string>();
        int m = 0, n = 0;
        int count = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '.')
            {
                if (count == 0)
                {
                    m = i;
                }
                count++;
            }
            if (str[i] == '}')
            {
                count--;
                if (count == 0)
                {
                    n = i;
                    int start = m + 1;
                    word.Add(str.Substring(start, (n + 1) - m));
                }
            }
        }
        return word;
    }
    #endregion

    [MenuItem("Tools/修改所有lua文件编码")]
    public static void ChangeAllLuauTF8()
    {
        RecursivelyCopyLua(AppConst.LuaPath);
        Debug.LogError("所有文件已经编码已经修改完成");
    }

    public static void RecursivelyCopyLua(string rootDir)
    {
        string[] files = Directory.GetFiles(rootDir);
        string[] dirs = Directory.GetDirectories(rootDir);

        foreach (string file in files)
        {
            
            string d = Path.GetExtension(file);
            if (d == ".lua")
            {
                Debug.LogWarning("所有文件已经编码已经修改完成" + file);
                string bs = File.ReadAllText(file);
                File.WriteAllText(file, bs, new UTF8Encoding(false));
            }
        }
        if (dirs == null || dirs.Length <= 0) return;
        foreach (string dir in dirs)
        {
            RecursivelyCopyLua(dir);
        }
    }

}

public class ZipHelper
{
    /// <summary>
    /// 压缩文件
    /// </summary>
    /// <param name="sourceFilePath"></param>
    /// <param name="destinationZipFilePath"></param>
    public static void CreateZip(string sourceFilePath, string destinationZipFilePath)
    {
        if (sourceFilePath[sourceFilePath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
            sourceFilePath += System.IO.Path.DirectorySeparatorChar;
        ZipOutputStream zipStream = new ZipOutputStream(File.Create(destinationZipFilePath));
        zipStream.SetLevel(6);  // 压缩级别 0-9
        CreateZipFiles(sourceFilePath, zipStream);
        zipStream.Finish();
        zipStream.Close();
    }
    /// <summary>
    /// 递归压缩文件
    /// </summary>
    /// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
    /// <param name="zipStream">打包结果的zip文件路径（类似 D:\WorkSpace\a.zip）,全路径包括文件名和.zip扩展名
    /// <param name="staticFile"></param>
    private static void CreateZipFiles(string sourceFilePath, ZipOutputStream zipStream)
    {
        Crc32 crc = new Crc32();
        string[] filesArray = Directory.GetFileSystemEntries(sourceFilePath);
        foreach (string file in filesArray)
        {
            if (Directory.Exists(file))                     //如果当前是文件夹，递归
            {
                CreateZipFiles(file, zipStream);
            }
            else                                            //如果是文件，开始压缩
            {
                FileStream fileStream = File.OpenRead(file);
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                string tempFile = file.Substring(sourceFilePath.LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(tempFile);
                entry.DateTime = DateTime.Now;
                entry.Size = fileStream.Length;
                fileStream.Close();
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                zipStream.PutNextEntry(entry);
                zipStream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
public class ProtoDataBase
{
    public string name;
}
public class ProtoPackData : ProtoDataBase
{
    public List<ProtoInfoData> infos { get; set; }
}
public class ProtoInfoData : ProtoDataBase
{
    public List<ProtoParamData> parameters { get; set; }
}
public class ProtoParamData : ProtoDataBase
{
    public string type;
    public bool isArr = false;
}
