// **********************************************************************
// 
// 文件名(File Name)：             Log.cs
// 
// 作者(Author)：                  Sheen
// 
// 创建时间(CreateTime):           2015/9/21 22:45:14
//
// **********************************************************************

using UnityEngine;
using System.Collections;

public class Log
{ 
    public delegate void LogFunc(object obj);
    public static LogFunc Error = UnityEngine.Debug.LogError;
#if UNITY_EDITOR
    public static LogFunc Debug = UnityEngine.Debug.Log;
    public static LogFunc Warning = UnityEngine.Debug.LogWarning;
#else
    public static void Debug(object obj)
    {

    }

    public static void Warning(object obj)
    {

    }
#endif

}
