//  XcodeProjectUpdater.cs
//  ProductName Test
//
//  Created by kikuchikan on 2015.07.29.
#if UNITY_IPHONE
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using YK;
/// <summary>
/// Xcode 
/// </summary>
public class XcodeProjectUpdater : MonoBehaviour
{

	public static void OnPostprocessBuild(XcodeProjectSetting setting, string buildPath)
    {
        
        string pbxProjPath = PBXProject.GetPBXProjectPath(buildPath);
        UnityEditor.iOS.Xcode.Custom.PBXProject pbxProject = new UnityEditor.iOS.Xcode.Custom.PBXProject();
        pbxProject.ReadFromString(File.ReadAllText(pbxProjPath));

        //获取 target的Guid
        string targetGuid = pbxProject.TargetGuidByName(PBXProject.GetUnityTargetName());
        string projPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
        //LinkLibraries(projPath);
        //拷贝所有文件
        if (!string.IsNullOrEmpty(setting.CopyDirectoryPath))
        {
            DirectoryProcessor.CopyAndAddBuildToXcode(pbxProject, targetGuid, setting.CopyDirectoryPath, buildPath, "");
        }

        //设置编译器标志
        foreach (XcodeProjectSetting.CompilerFlagsSet compilerFlagsSet in setting.CompilerFlagsSetList)
        {

            foreach (string targetPath in compilerFlagsSet.TargetPathList)
            {
                if (!pbxProject.ContainsFileByProjectPath(targetPath))
                {
                    Debug.Log(targetPath + "编译器标志不能被设置，因为没有这个标志");
                    continue;
                }

                string fileGuid = pbxProject.FindFileGuidByProjectPath(targetPath);
                List<string> flagsList = pbxProject.GetCompileFlagsForFile(targetGuid, fileGuid);

                flagsList.Add(compilerFlagsSet.Flags);
                pbxProject.SetCompileFlagsForFile(targetGuid, fileGuid, flagsList);
            }

        }

        //添加所有的 Framework
        foreach (string framework in setting.FrameworkList)
        {
            string libStr = framework;
            bool weak = false;
            if (framework.Contains(":"))
            {
                string[] ss = framework.Split(':');
                if (ss.Length > 1)
                {
                    libStr = ss[0];
                    weak = ss[1] == "weak";
                }
            }
            pbxProject.AddFrameworkToProject(targetGuid, libStr, weak);
        }

        foreach (string lib in setting.LibList)
        {
            string libStr = lib;
            bool weak = false;
            if (lib.Contains(":"))
            {
                string [] ss = lib.Split(':');
                if (ss.Length > 1)
                {
                    libStr = ss[0];
                    weak = ss[1] == "weak";
                }
            }
            
            pbxProject.AddFrameworkToProject(targetGuid, libStr, weak);
        }
        pbxProject.UpdateBuildProperty(targetGuid, XcodeProjectSetting.LINKER_FLAG_KEY, setting.LinkerFlagArray, null);

        pbxProject.UpdateBuildProperty(targetGuid, XcodeProjectSetting.FRAMEWORK_SEARCH_PATHS_KEY, setting.FrameworkSearchPathArray, null);

        //BitCode
        pbxProject.SetBuildProperty(targetGuid, XcodeProjectSetting.ENABLE_BITCODE_KEY, setting.EnableBitCode ? "YES" : "NO");
        pbxProject.AddCapability(targetGuid, UnityEditor.iOS.Xcode.Custom.PBXCapabilityType.PushNotifications);
        //保存最终工程
        File.WriteAllText(pbxProjPath, pbxProject.WriteToString());

        InfoPlistProcessor.SetApplicationQueriesSchemes(buildPath, setting.ApplicationQueriesSchemes);

        //URL配置
        foreach (XcodeProjectSetting.URLIdentifierData urlData in setting.URLIdentifierList)
        {
            InfoPlistProcessor.SetURLSchemes(buildPath, urlData.name, urlData.URLSchemes);
        }
        
        //关闭启动图像的已设定默认的设定
        if (setting.NeedToDeleteLaunchiImagesKey)
        {
            InfoPlistProcessor.DeleteLaunchiImagesKey(buildPath);
        }

        //ATS配置
        InfoPlistProcessor.SetATS (buildPath, setting.EnableATS);

        //设置状态栏
        InfoPlistProcessor.SetStatusBar (buildPath, setting.EnableStatusBar);

		//添加特殊的key   比如麦克风//
		foreach (XcodeProjectSetting.AddToInfoStringList data in setting.addStringKeyToPlist) {
		
			InfoPlistProcessor.AddStringKey (buildPath,data.Key,data.value);
		}

        foreach (XcodeProjectSetting.AddCodeData data in setting.AddCodeList)
        {
            string fileName = GetCClassFileName(buildPath, data.FileName);
            
            if (!System.IO.File.Exists(fileName))
            {
                fileName = Path.Combine(buildPath, data.FileName);
            }
            Debug.LogError("需要替换的文件路径："+ fileName);
            if (System.IO.File.Exists(fileName))
            {
                XClass xc = new XClass(fileName);
                xc.WriteStart(data.StartAddCode);
                foreach (XcodeProjectSetting.AddCodeSet addSet in data.addCodes)
                {
                    Debug.LogError(addSet.addFlag + "/需要替换的文件／" + addSet.AddCode);
                    xc.WriteBelow(addSet.addFlag, addSet.AddCode);
                }
                foreach (XcodeProjectSetting.ReplaceCodeSet replaceSet in data.replaceCode)
                {
                    xc.Replace(replaceSet.oldCode, replaceSet.newCode);
                }
            }
        }
        PlayerPrefs.DeleteKey(SETTING_DATA_PATHKEY);
    }

    public static string GetCClassFileName(string buildPath, string fileName)
    {
        return Path.Combine(buildPath, string.Format(XcodeProjectSetting.CLASS_PATH, fileName));
    }
    public static void LinkLibraries(string projPath)
    {

        string contents = File.ReadAllText(projPath);

        // StoreKit.framework
        contents = contents.Replace("/* Bulk_Assembly-CSharp_0.cpp */; };",
            "/* Bulk_Assembly-CSharp_0.cpp */; };\n\t\t07B07E8E1EB2FB1D003DF680 /* CoreTelephony.framework in Frameworks */ = {isa = PBXBuildFile; fileRef = 07B07E8D1EB2FB1D003DF680 /* CoreTelephony.framework */; };\n\t\t2F71DBE21F30B176008131FD /* VideoToolbox.framework in Frameworks */ = {isa = PBXBuildFile; fileRef = 2F71DBE11F30B176008131FD /* VideoToolbox.framework */; };\n\t\t07B07E901EB2FB22003DF680 /* libresolv.tbd in Frameworks */ = {isa = PBXBuildFile; fileRef = 07B07E8F1EB2FB22003DF680 /* libresolv.tbd */; };");
        contents = contents.Replace("path = Classes/Native/Bulk_Generics_3.cpp; sourceTree = SOURCE_ROOT; };",
            "path = Classes/Native/Bulk_Generics_3.cpp; sourceTree = SOURCE_ROOT; };\n\t\t07B07E8D1EB2FB1D003DF680 /* CoreTelephony.framework */ = {isa = PBXFileReference; lastKnownFileType = wrapper.framework; name = CoreTelephony.framework; path = System/Library/Frameworks/CoreTelephony.framework; sourceTree = SDKROOT; };\n\t\t2F71DBE11F30B176008131FD /* VideoToolbox.framework */ = {isa = PBXFileReference; lastKnownFileType = wrapper.framework; name = VideoToolbox.framework; path = System/Library/Frameworks/VideoToolbox.framework; sourceTree = SDKROOT; };\n\t\t07B07E8F1EB2FB22003DF680 /* libresolv.tbd */ = {isa = PBXFileReference; lastKnownFileType = \"sourcecode.text-based-dylib-definition\"; name = libresolv.tbd; path = usr/lib/libresolv.tbd; sourceTree = SDKROOT; };");
        contents = contents.Replace("00000000008063A1000160D3 /* libiPhone-lib.a in Frameworks */,",
            "07B07E901EB2FB22003DF680 /* libresolv.tbd in Frameworks */,\n\t\t\t\t07B07E8E1EB2FB1D003DF680 /* CoreTelephony.framework in Frameworks */,\n\t\t\t\t2F71DBE21F30B176008131FD /* VideoToolbox.framework in Frameworks */,\n\t\t\t\t00000000008063A1000160D3 /* libiPhone-lib.a in Frameworks */,");
        contents = contents.Replace("AA5D99861AFAD3C800B27605 /* CoreText.framework */,",
            "07B07E8F1EB2FB22003DF680 /* libresolv.tbd */,\n\t\t\t\t07B07E8D1EB2FB1D003DF680 /* CoreTelephony.framework */,\n\t\t\t\t2F71DBE11F30B176008131FD /* VideoToolbox.framework */,\n\t\t\t\tAA5D99861AFAD3C800B27605 /* CoreText.framework */,");

        File.WriteAllText(projPath, contents);
    }
}
namespace YK
{
    public partial class XClass : System.IDisposable
    {

        private string filePath;

        public XClass(string fPath)
        {
            filePath = fPath;
            if (!System.IO.File.Exists(filePath))
            {
                Debug.LogError(filePath + "路径下文件不存在");
                return;
            }
        }

        public void WriteStart(string str)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string text_all = streamReader.ReadToEnd();
            streamReader.Close();
            text_all = str+"\n" + text_all;
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(text_all);
            streamWriter.Close();
        }

        public void WriteBelow(string below, string text)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string text_all = streamReader.ReadToEnd();
            streamReader.Close();

            int beginIndex = text_all.IndexOf(below);
            if (beginIndex == -1)
            {
                Debug.LogError(filePath + "中没有找到标致" + below);
                return;
            }

            int endIndex = text_all.LastIndexOf("\n", beginIndex + below.Length);

            text_all = text_all.Substring(0, endIndex) + "\n" + text + "\n" + text_all.Substring(endIndex);

            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(text_all);
            streamWriter.Close();
        }

        public void Replace(string below, string newText)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string text_all = streamReader.ReadToEnd();
            streamReader.Close();

            int beginIndex = text_all.IndexOf(below);
            if (beginIndex == -1)
            {
                Debug.LogError(filePath + "中没有找到标志" + below);
                return;
            }

            text_all = text_all.Replace(below, newText);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(text_all);
            streamWriter.Close();

        }

        public void Dispose()
        {

        }
       
    }
}
#endif