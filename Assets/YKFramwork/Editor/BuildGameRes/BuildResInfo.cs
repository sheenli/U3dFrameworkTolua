using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
/// <summary>
/// 生成资源需要打包的文件
/// </summary>
public class BuildResInfo : ScriptableObject
{
    [Header("Lua资源名称")]
    public string LuaABName = "";
    [Header("资源信息")]
    public string ResName = "";

    [Header("资源的父目录")]
    public string ResParentPath = "";

    /// <summary>
    /// 要打包的lua目录
    /// </summary>
    [Header("要打包的lua目录")]
    public string LuaParentPath = "";

    [MenuItem("Assets/创建一个资源打包配置")]
    public static BuildResInfo CreateAsste()
    {
        var select = Selection.activeObject;
        var path = AssetDatabase.GetAssetPath(select);
        path = AssetDatabase.GenerateUniqueAssetPath(path+"/BuildeResInfo.asset");
        BuildResInfo data = ScriptableObject.CreateInstance<BuildResInfo>();
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return data;
    }

    /// <summary>
    /// 生成这个资源包
    /// </summary>
    public void Build(int flag)
    {
        BuildAB.BuildResByInfo(this, flag);
    }
}



[CustomEditor(typeof(BuildResInfo))]
public class BuildResInfoEditor : Editor
{
    public BuildResInfo Instance
    {
        get
        {
            return target as BuildResInfo;
        }
    }
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("资源名称：");
        Instance.ResName = EditorGUILayout.TextField(Instance.ResName);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("资源父目录：");
        EditorGUILayout.TextField(Instance.ResParentPath);
        if (GUILayout.Button("选择"))
        {
            string defPath = AppConst.ABPath + "/" + Instance.ResParentPath;
            string path = EditorUtility.OpenFolderPanel("选择资源打包的父目录", defPath, "").Replace("\\","/");
            if (!string.IsNullOrEmpty(path) && path.StartsWith(AppConst.ABPath))
            {
                Instance.ResParentPath = path.Replace(AppConst.ABPath,"");
            }
            else
            {
                Debug.LogError("选择的路径有误path="+path);
            }
        }
        
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("lua的根目录："));
        EditorGUILayout.TextField(Instance.LuaParentPath);
        if (GUILayout.Button("选择"))
        {
            string defPath = AppConst.LuaPath + "/" + Instance.LuaParentPath;
            string root = Path.GetFullPath(AppConst.LuaPath).Replace("\\", "/");
            string path = EditorUtility.OpenFolderPanel("选择lua打包的父目录", defPath, "").Replace("\\", "/");
            if (!string.IsNullOrEmpty(path) && path.StartsWith(root))
            {
                Instance.LuaParentPath = path.Replace(root, "");
            }
            else
            {
                Debug.LogError("选择的路径有误path=" + path);
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("lua资源名称：");
        Instance.LuaABName = EditorGUILayout.TextField(Instance.LuaABName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成lua"))
        {
            Instance.Build(0);
        }


        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成资源"))
        {
            Instance.Build(1);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成全部"))
        {
            Instance.Build(2);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        serializedObject.Update();
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
    }
}
