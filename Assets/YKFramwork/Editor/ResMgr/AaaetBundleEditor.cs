using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AaaetBundleEditor 
{
    public static int selectBuilds = 0;
    public static Editor show = null;
    public static void OnGUI()
    {
        var assets = AssetDatabase.GetAllAssetPaths()
              .Where(x =>
              {
                  return x.StartsWith("Assets/YKFramwork", StringComparison.InvariantCultureIgnoreCase);
              });

        List<BuildCollectionResInfo> list = new List<BuildCollectionResInfo>();
        List<string> dis = new List<string>();
        foreach (var assetPath in assets)
        {
            BuildCollectionResInfo info = AssetDatabase.LoadAssetAtPath(assetPath, typeof(BuildCollectionResInfo)) as BuildCollectionResInfo;
            if (info != null)
            {
                list.Add(info);
                dis.Add(Path.GetFileNameWithoutExtension( assetPath));
            }
        }
        EditorGUILayout.BeginHorizontal();
        {
           
            GUILayout.Space(5);
            GUIStyle st = new GUIStyle("LargePopup");
            //st.CalcScreenSize(new Vector2(20,200));
            st.margin = new RectOffset(0, 0, 0, 20);
//             st.fixedHeight = 20;
//             st.fixedWidth = 200;
            st.alignment = TextAnchor.MiddleCenter;

            GUIStyle la = new GUIStyle("Label");
            la.margin = new RectOffset(0, 0, 0, 20);
            la.fontSize = 20;
            la.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label(new GUIContent("准备生成的配置："), la);
            selectBuilds = EditorGUILayout.Popup(selectBuilds,
           dis.ToArray(), st);
            //GUI.color = Color.green;
            if (GUILayout.Button("Add", st))
            {
                string path = EditorUtility.SaveFilePanel("保存文件位置", "Assets/YKFramwork/Editor/BuildAbInfo/Collections", "","asset").Replace("\\", "/");
                path = FileUtil.GetProjectRelativePath(path);
                if (!string.IsNullOrEmpty(path))
                {
                    BuildCollectionResInfo data = ScriptableObject.CreateInstance<BuildCollectionResInfo>();
                    Debug.LogError(path);
                    AssetDatabase.CreateAsset(data, path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
        if (list.Count > selectBuilds)
        {
           
            BuildCollectionResInfo selectinfo = list[selectBuilds];
            if (show == null)
            {
                show = Editor.CreateEditor(selectinfo);
            }
            else
            {
                if(show.target != selectinfo)
                {
                    show = Editor.CreateEditor(selectinfo);
                }
            }
        }
        show.OnInspectorGUI();
        

    }
}
