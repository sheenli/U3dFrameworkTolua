using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class BuildCollectionResInfo : ScriptableObject
{
    public string CollectionName = "3D砀山麻将";
    public int CollectionID = 8002;
    public List<BuildResInfo> builds = new List<BuildResInfo>();
    
    [MenuItem("Assets/创建一个合集资源打包配置")]
    public static BuildCollectionResInfo CreateAsste()
    {
        var select = Selection.activeObject;
        var path = AssetDatabase.GetAssetPath(select);
        path = AssetDatabase.GenerateUniqueAssetPath(path + "/BuildeResInfo.asset");
        BuildCollectionResInfo data = ScriptableObject.CreateInstance<BuildCollectionResInfo>();
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return data;
    }

    public void Build()
    {
        ProjectBuild.currentCollection = (ProjectBuild.CollectionType)CollectionID;
        BuildAB.BuildCollectionsResByInfo(this);
    }

    public static BuildCollectionResInfo Find(int CollectionID)
    {
        string[] bs = AssetDatabase.FindAssets("t:BuildCollectionResInfo");
        foreach (string b in bs)
        {
            BuildCollectionResInfo bc = AssetDatabase.LoadAssetAtPath<BuildCollectionResInfo>(AssetDatabase.GUIDToAssetPath(b));
            if (bc.CollectionID== CollectionID)
            {
                return bc;
            }
        }
        return null;
    }

    public static BuildCollectionResInfo Find(string collectionName)
    {
        string[] bs = AssetDatabase.FindAssets("t:BuildCollectionResInfo");
        foreach (string b in bs)
        {
            BuildCollectionResInfo bc = AssetDatabase.LoadAssetAtPath<BuildCollectionResInfo>(AssetDatabase.GUIDToAssetPath(b));
            if (bc.CollectionName == collectionName)
            {
                return bc;
            }
        }
        return null;
    }
}

[CustomEditor(typeof(BuildCollectionResInfo))]
public class BuildCollectionResEditor : Editor
{
    public BuildCollectionResInfo Instance
    {
        get
        {
            return target as BuildCollectionResInfo;
        }
    }
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("当前平台：" + EditorUserBuildSettings.activeBuildTarget);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(30);
        base.OnInspectorGUI();
        serializedObject.Update();
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        GUILayout.BeginVertical();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("要生成的版本号：");
        ProjectBuild.version = GUILayout.TextField(ProjectBuild.version);
        ProjectBuild.isPublic = GUILayout.Toggle(ProjectBuild.isPublic, "是否是发布版本");
        //GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("生成AssetBunld"))
            {
                Instance.Build();
            }
            if (GUILayout.Button("生成AB和当前平台的包"))
            {
                ProjectBuild.BuildPCALL();
            }
        }
        GUILayout.EndHorizontal();

        
        GUILayout.EndVertical();
        
    }
}