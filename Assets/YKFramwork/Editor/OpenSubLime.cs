//  http://blog.csdn.net/rickshaozhiheng
//  OpenLuaHelper.cs
//  Created by zhiheng.shao
//  Copyright  2017年 zhiheng.shao. All rights reserved.
//
//  Description
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System;
using System.Linq;

public class OpenLuaHelper : Editor
{
    private const string EXTERNAL_EDITOR_PATH_KEY = "mTv8";
    private const string LUA_PROJECT_ROOT_FOLDER_PATH_KEY = "obUd";

    [UnityEditor.Callbacks.OnOpenAssetAttribute(2)]
    public static bool OnOpenAsset2(int instanceID, int line)
    {
        string logText = GetLogText();
        if (string.IsNullOrEmpty(logText) || logText.IndexOf(": [") == -1 || logText.IndexOf("]:") == -1)
            return false;
        string st;
        string filePath;
        int luaLine;
        bool ret = false;
        try
        {
            if (logText.IndexOf(": [string") != -1)
            {
                st = logText.Substring(logText.IndexOf(": [string ") + 10, logText.IndexOf("]:") - logText.IndexOf(": [string ") - 11).Replace("\"","");
                st = Path.GetFileName(st);
                filePath = st.Replace("\\", "");
				int start = logText.IndexOf("]:")+ 2;
				st = logText.Substring(start,logText.Length - start);
				st = st.Substring(0,st.IndexOf(":"));
				luaLine = int.Parse(st);
            }
            else
            {
                st = logText.Substring(logText.IndexOf(": [") + 3, logText.IndexOf("]:") - logText.IndexOf(": [") - 3);
                filePath = st.Split(':')[0];
                luaLine = int.Parse(st.Split(':')[1]);
            }

            string luaFolderRoot = EditorUserSettings.GetConfigValue(LUA_PROJECT_ROOT_FOLDER_PATH_KEY);
            if (string.IsNullOrEmpty(luaFolderRoot))
            {
                SetLuaProjectRoot();
                luaFolderRoot = EditorUserSettings.GetConfigValue(LUA_PROJECT_ROOT_FOLDER_PATH_KEY);
            }
            filePath = GetAsset(luaFolderRoot, Path.GetFileNameWithoutExtension(filePath));
            filePath = filePath.Replace("\\", "/");
            return OpenFileAtLineExternal(filePath, luaLine);
        }
        catch (System.Exception)
        {

            ret = false;
        }
        

        return ret;
    }

    [UnityEditor.Callbacks.OnOpenAssetAttribute(3)]
    public static bool OnOpenAsset3(int instanceID, int line)
    {

        string luaFolderRoot = EditorUserSettings.GetConfigValue(LUA_PROJECT_ROOT_FOLDER_PATH_KEY);
        string filePath;
        int luaLine;
        string logText = GetLogText();
        string startIndex = "LuaException: ";
        if (string.IsNullOrEmpty(logText) || logText.IndexOf(startIndex) == -1)
        {
            return false;
        }
        int si = logText.LastIndexOf(startIndex) + startIndex.Length;
        logText = logText.Substring(si, logText.Length - si);
        logText = logText.Substring(0, logText.IndexOf(": "));
        filePath = logText.Split(':')[0];
        filePath = filePath.Replace(".", "/") + ".lua";
        luaLine = int.Parse(logText.Split(':')[1]);
        filePath = GetAsset(luaFolderRoot, Path.GetFileNameWithoutExtension(filePath));
        filePath = filePath.Replace("\\", "/");
        return OpenFileAtLineExternal(filePath, luaLine);
    }

    static string GetAsset(string path , string filename)
    {
        string [] files =  Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach(string str in files)
        {
            if(Path.GetFileNameWithoutExtension(str) == filename)
            {
                return str;
            }
        }

        foreach(string str in dirs)
        {
            string file = GetAsset(str, filename);
            if(file != "")
            {
                return file;
            }
        }
        return "";
    }


    static bool OpenFileAtLineExternal(string fileName, int line)
    {
        string editorPath = EditorUserSettings.GetConfigValue(EXTERNAL_EDITOR_PATH_KEY);
        if (string.IsNullOrEmpty(editorPath) || !File.Exists(editorPath))
        {   // 没有path就弹出面板设置
            SetExternalEditorPath();
        }
        OpenFileWith(fileName, line);
        return true;
    }
    

    static void OpenFileWith(string fileName, int line)
    {
        string editorPath = EditorUserSettings.GetConfigValue(EXTERNAL_EDITOR_PATH_KEY);
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = editorPath;
        proc.StartInfo.Arguments = string.Format("{0} --line {1} {2}", EditorUserSettings.GetConfigValue(LUA_PROJECT_ROOT_FOLDER_PATH_KEY), line, fileName);
        proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
        proc.Start();
        
    }

    private static System.Diagnostics.Process GetFileNameProcess(string fileName)
    {
        var process = System.Diagnostics.Process.GetProcesses().FirstOrDefault(p =>
        {
            string processName;
            try
            {
                processName = p.ProcessName; // some processes like kaspersky antivirus throw exception on attempt to get ProcessName
            }
            catch (Exception)
            {
                return false;
            }

            return !p.HasExited && processName.ToLower().Contains(fileName);
        });
        return process;
    }

    static class User32Dll
    {

        /// <summary>
        /// Gets the ID of the process that owns the window.
        /// Note that creating a <see cref="Process"/> wrapper for that is very expensive because it causes an enumeration of all the system processes to happen.
        /// </summary>
        public static int GetWindowProcessId(IntPtr hwnd)
        {
            uint dwProcessId;
            GetWindowThreadProcessId(hwnd, out dwProcessId);
            return unchecked((int)dwProcessId);
        }

        /// <summary>
        /// Lists the handles of all the top-level windows currently available in the system.
        /// </summary>
        public static List<IntPtr> GetTopLevelWindowHandles()
        {
            var retval = new List<IntPtr>();
            EnumWindowsProc callback = (hwnd, param) =>
            {
                retval.Add(hwnd);
                return 1;
            };
            EnumWindows(Marshal.GetFunctionPointerForDelegate(callback), IntPtr.Zero);
            GC.KeepAlive(callback);
            return retval;
        }

        public delegate Int32 EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true,
          ExactSpelling = true)]
        public static extern Int32 EnumWindows(IntPtr lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true,
          ExactSpelling = true)]
        public static extern Int32 SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true,
          ExactSpelling = true)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);


        [DllImport("user32.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true,
          ExactSpelling = true)]
        public static extern UInt32 ShowWindow(IntPtr hWnd, Int32 nCmdShow);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true,
          ExactSpelling = true)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
    }
    [MenuItem("Tools/SetExternalEditorPath")]
    static void SetExternalEditorPath()
    {
        string path = EditorUserSettings.GetConfigValue(EXTERNAL_EDITOR_PATH_KEY);
        path = EditorUtility.OpenFilePanel(
                    "SetExternalEditorPath",
                    path,
                    "exe");

        if (path != "")
        {
            EditorUserSettings.SetConfigValue(EXTERNAL_EDITOR_PATH_KEY, path);
            Debug.Log("Set Editor Path: " + path);
        }
    }

    [MenuItem("Tools/SetLuaProjectRoot")]
    static void SetLuaProjectRoot()
    {
        string path = EditorUserSettings.GetConfigValue(LUA_PROJECT_ROOT_FOLDER_PATH_KEY);
        path = EditorUtility.OpenFolderPanel(
                    "SetLuaProjectRoot",
                    path,
                    "");

        if (path != "")
        {
            EditorUserSettings.SetConfigValue(LUA_PROJECT_ROOT_FOLDER_PATH_KEY, path);
            Debug.Log("Set Editor Path: " + path);
        }
    }

    static string GetLogText()
    {
        // 找到UnityEditor.EditorWindow的assembly
        var assembly_unity_editor = Assembly.GetAssembly(typeof(UnityEditor.EditorWindow));
        if (assembly_unity_editor == null) return null;

        // 找到类UnityEditor.ConsoleWindow
        var type_console_window = assembly_unity_editor.GetType("UnityEditor.ConsoleWindow");
        if (type_console_window == null) return null;
        // 找到UnityEditor.ConsoleWindow中的成员ms_ConsoleWindow
        var field_console_window = type_console_window.GetField("ms_ConsoleWindow", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        if (field_console_window == null) return null;
        // 获取ms_ConsoleWindow的值
        var instance_console_window = field_console_window.GetValue(null);
        if (instance_console_window == null) return null;

        // 如果console窗口时焦点窗口的话，获取stacktrace
        if ((object)UnityEditor.EditorWindow.focusedWindow == instance_console_window)
        {
            // 通过assembly获取类ListViewState
            var type_list_view_state = assembly_unity_editor.GetType("UnityEditor.ListViewState");
            if (type_list_view_state == null) return null;

            // 找到类UnityEditor.ConsoleWindow中的成员m_ListView
            var field_list_view = type_console_window.GetField("m_ListView", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (field_list_view == null) return null;

            // 获取m_ListView的值
            var value_list_view = field_list_view.GetValue(instance_console_window);
            if (value_list_view == null) return null;

            // 下面是stacktrace中一些可能有用的数据、函数和使用方法，这里就不一一说明了，我们这里暂时还用不到
            /*
            var field_row = type_list_view_state.GetField("row", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (field_row == null) return null;

            var field_total_rows = type_list_view_state.GetField("totalRows", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (field_total_rows == null) return null;

            var type_log_entries = assembly_unity_editor.GetType("UnityEditorInternal.LogEntries");
            if (type_log_entries == null) return null;

            var method_get_entry = type_log_entries.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
            if (method_get_entry == null) return null;

            var type_log_entry = assembly_unity_editor.GetType("UnityEditorInternal.LogEntry");
            if (type_log_entry == null) return null;

            var field_instance_id = type_log_entry.GetField("instanceID", BindingFlags.Instance | BindingFlags.Public);
            if (field_instance_id == null) return null;

            var field_line = type_log_entry.GetField("line", BindingFlags.Instance | BindingFlags.Public);
            if (field_line == null) return null;

            var field_condition = type_log_entry.GetField("condition", BindingFlags.Instance | BindingFlags.Public);
            if (field_condition == null) return null;

            object instance_log_entry = Activator.CreateInstance(type_log_entry);
            int value_row = (int)field_row.GetValue(value_list_view);
            int value_total_rows = (int)field_total_rows.GetValue(value_list_view);
            int log_by_this_count = 0;
            for (int i = value_total_rows – 1; i > value_row; i–) {
            method_get_entry.Invoke(null, new object[] { i, instance_log_entry });
            string value_condition = field_condition.GetValue(instance_log_entry) as string;
            if (value_condition.Contains("[SDebug]")) {
            log_by_this_count++;
            }
            }
            */

            // 找到类UnityEditor.ConsoleWindow中的成员m_ActiveText
            var field_active_text = type_console_window.GetField("m_ActiveText", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (field_active_text == null) return null;

            // 获得m_ActiveText的值，就是我们需要的stacktrace
            string value_active_text = field_active_text.GetValue(instance_console_window).ToString();
            return value_active_text;
        }
        return null;
    }
}