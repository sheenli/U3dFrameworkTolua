using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetInfoEditor
{

    public AssetGroupMgr mController;

    public AssetMode.AssetInfo mCurrentSelectAssets = null;

    public AssetInfoEditor(AssetGroupMgr ctrl)
    {
        mController = ctrl;
    }

    /// <summary>
    /// 从新加载
    /// </summary>
    public void Reload()
    {
        mCurrentSelectAssets = null;
    }

    private float TitleWidth = 95;
    private float offset = 20;

    public void OnGUI(Rect rect)
    {
        
        Rect PreviewRect = new Rect(
            rect.x + rect.width * 0.6f -10, rect.y + 10,
            rect.width * 0.4f, rect.height - 15
            );
        GUIStyle inputSt = new GUIStyle(GUI.skin.GetStyle("CN Box"));
        inputSt.fixedWidth = PreviewRect.x - rect.x - TitleWidth - 10;
        inputSt.fixedHeight = 30;
        inputSt.contentOffset = new Vector2(3, 0);
        inputSt.alignment = TextAnchor.MiddleLeft;

        GUIStyle labelSt = new GUIStyle(GUI.skin.GetStyle("LODRendererAddButton"));
        labelSt.fixedWidth = TitleWidth;
        labelSt.fixedHeight = 30;
        labelSt.alignment = TextAnchor.MiddleRight;

        GUIStyle checkBoxSt = new GUIStyle(GUI.skin.GetStyle("BoldToggle"));
        checkBoxSt.alignment = TextAnchor.MiddleLeft;
        checkBoxSt.fixedWidth = 30; 
        checkBoxSt.fixedHeight = 30;

        GUILayout.BeginArea(rect, GUI.skin.GetStyle("CN Box"));
        GUILayout.BeginVertical(); 
        GUILayout.Space(offset);
        if (this.mCurrentSelectAssets != null)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(new GUIContent("资源名称："), labelSt);


                if (mCurrentSelectAssets != null)
                {
                    GUILayout.TextField(mCurrentSelectAssets.Name, inputSt);
                }
                else
                {
                    GUILayout.Label("", inputSt);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(offset);
            GUILayout.BeginHorizontal();
            {
                string type = "";
                if (mCurrentSelectAssets != null)
                {
                    type = mCurrentSelectAssets.data.type;
                }
                GUILayout.Label(new GUIContent("类型："), labelSt);
                GUILayout.Label(type, inputSt);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(offset);
            GUILayout.BeginHorizontal();
            {
                string path = "";
                if (mCurrentSelectAssets != null)
                {
                    path = mCurrentSelectAssets.data.path;
                }
                GUILayout.Label(new GUIContent("路径："), labelSt);
                GUILayout.Label(path, inputSt);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(offset);
            GUILayout.BeginHorizontal();
            {
                bool flag = false;
                if (mCurrentSelectAssets != null)
                {
                    flag = mCurrentSelectAssets.data.isResourcesPath;
                }
                GUILayout.Label(new GUIContent("resource资源："), labelSt);

                GUILayout.Label(flag ? "是" : "否", inputSt);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(offset);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(new GUIContent("常驻内存："), labelSt);
                if (mCurrentSelectAssets != null)
                {
                    mCurrentSelectAssets.data.isKeepInMemory = GUILayout.Toggle(mCurrentSelectAssets.data.isKeepInMemory, "", checkBoxSt);
                }
                else
                {
                    GUILayout.Toggle(false, "", checkBoxSt);
                }



                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(offset);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(new GUIContent("FGUI包："), labelSt);
                if (mCurrentSelectAssets != null)
                {
                    mCurrentSelectAssets.data.isFairyGuiPack = GUILayout.Toggle(mCurrentSelectAssets.data.isFairyGuiPack, "", checkBoxSt);
                }
                else
                {
                    GUILayout.Toggle(false, "", checkBoxSt);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();


        GUILayout.EndArea();

        
        //GUILayout.BeginArea(PreviewRect, GUI.skin.GetStyle("preBackground"));
        if (this.mCurrentSelectAssets != null)
        {
           
            Texture texture = EditorGUIUtility.FindTexture("Refresh");
            Type t = AssetDatabase.GetMainAssetTypeAtPath(this.mCurrentSelectAssets.data.path);
            Editor edit = Editor.CreateEditor(AssetDatabase.LoadAssetAtPath(this.mCurrentSelectAssets.data.path, t));
            if (edit != null && edit.HasPreviewGUI())
            {
                edit.OnPreviewGUI(PreviewRect, GUI.skin.GetStyle("preBackground"));
            }
            else
            {
                texture = AssetDatabase.GetCachedIcon(this.mCurrentSelectAssets.data.path);
            }
        }
        //GUILayout.EndArea();
    }

    internal void SelectedAssets(List<AssetMode.AssetInfo> list)
    {
        if (list.Count != 1)
        {
            Reload();
        }
        else
        {
            mCurrentSelectAssets = list[0];
        }
    }
}
