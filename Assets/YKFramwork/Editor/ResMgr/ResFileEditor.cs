using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class ResFileEditor : EditorWindow
{
    [HideInInspector]
    [SerializeField]
    public AssetGroupMgr m_ManageTab;
    [HideInInspector]
    public AaaetBundleEditor mAaaetBundleEditor;

    [MenuItem("Tools/资源管理器", priority = 2050)]
    static void ShowWindow()
    {
        var window = GetWindow<ResFileEditor>();
        window.titleContent = new GUIContent("资源管理器");
        window.Show();
    }
    string[] m_ButtonStr = new string[2] { "资源组","生成assetbundle"};
    
    protected void OnEnable()
    {
        Rect subPos = GetSubWindowArea();
        if (m_ManageTab == null) m_ManageTab = new AssetGroupMgr();
        m_ManageTab.OnEnable(subPos, this);
    }
    public enum SelectedPage
    {
        ResGroup,
        BuildAssetbundle
    }
    
    [HideInInspector]
    public SelectedPage m_SelectedPage;

    protected void OnGUI()
    {
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        {
            m_SelectedPage = (SelectedPage)GUILayout.Toolbar((int)m_SelectedPage, new string[] { "资源管理", "生成" }, GUILayout.Height(40));
        }
        GUILayout.EndHorizontal();
        if (m_SelectedPage == SelectedPage.ResGroup)
        {
            m_ManageTab.OnGUI(GetSubWindowArea());
        }
        else
        {
            AaaetBundleEditor.OnGUI();
            //base.OnGUI();
        }

    }
    private static float m_UpdateDelay = 0f;
    private void Update()
    {
        if (Time.realtimeSinceStartup - m_UpdateDelay > 0.1f)
        {
            m_UpdateDelay = Time.realtimeSinceStartup;

            if (AssetMode.Update())
            {
                Repaint();
            }
        }
    }

    /// <summary>
    /// 获取当前窗口区域
    /// </summary>
    /// <returns></returns>
    private Rect GetSubWindowArea()
    {
        Rect subPos = new Rect(0, 50, position.width, position.height - 10);
        return subPos;
    }

}

