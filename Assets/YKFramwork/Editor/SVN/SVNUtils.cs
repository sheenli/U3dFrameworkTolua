/**  SVNUtils
 *   @Author  稻草人
 *   @Blog    www.dcr91.com
 *   @Email   1003227483@qq.com
 **/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class SVNUtils
{
    private static List<string> drives = new List<string>() { "c:", "d:", "e:", "f:" };
    private static string svnPath = @"\Program Files\TortoiseSVN\bin\";
    private static string svnProc = @"TortoiseProc.exe";
    private static string svnProcPath = "";

    [MenuItem("Assets/SVN更新 %&e")]
    public static void UpdateFromSVN()
    {
        if (string.IsNullOrEmpty(svnProcPath))
            svnProcPath = GetSvnProcPath();
        var dir = new DirectoryInfo(Application.dataPath);
        var path = dir.Parent.FullName.Replace('/', '\\');
        var para = "/command:update /path:\"" + path + "\" /closeonend:0";
        Start(para);
    }

    private static void Start(string para)
    {
        try
        {
            System.Diagnostics.Process.Start(svnProcPath, para);
        }
        catch
        {
            svnProcPath = "";
            PlayerPrefs.SetString("svnProcPath", svnProcPath);
            //UpdateFromSVN();
        }
    }

    [MenuItem("Assets/SVN提交 %&r")]
    public static void CommitToSVN()
    {
        if (string.IsNullOrEmpty(svnProcPath))
            svnProcPath = GetSvnProcPath();
        var dataPath = Application.dataPath.Replace("Assets","");
        var path = dataPath.Replace('/', '\\');
        var para = "/command:commit /path:\"" + path + "\"";
        //var para1 = "/command:update /path:\"" + path + "\" /closeonend:0";
        //Start(para1);
        
        System.Diagnostics.Process.Start(svnProcPath, para);
    }
    [MenuItem("Assets/SVN添加 %&u")]
    public static void AddToSVN()
    {
        if (string.IsNullOrEmpty(svnProcPath))
            svnProcPath = GetSvnProcPath();
        var path = Application.dataPath.Replace('/', '\\');
        var para = "/command:add /path:\"" + path + "\"";
        System.Diagnostics.Process.Start(svnProcPath, para);
    }

    private static string GetSvnProcPath()
    {
        string n = PlayerPrefs.GetString("svnProcPath");
        if (n != null && n != "")
        {
            return n;
        }
        foreach (var item in drives)
        {
            var path = string.Concat(item, svnPath, svnProc);
            if (File.Exists(path))
                return path;
        }
        n = EditorUtility.OpenFilePanel("Select TortoiseProc.exe", "c:\\", "exe");
        //Debug.LogError(n);
        PlayerPrefs.SetString("svnProcPath", n);
        return n;
    }
}