using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[System.Serializable]
public class AssetGroupMgr 
{
    /// <summary>
    /// 资源组树状图列表
    /// </summary>
    [SerializeField]
    TreeViewState m_GroupTreeState;


    /// <summary>
    /// 资源多列表标签
    /// </summary>
    [SerializeField]
    MultiColumnHeaderState m_GroupTreeMCHState;

    /// <summary>
    /// 资源树状图列表
    /// </summary>
    [SerializeField]
    TreeViewState m_AssetListState;

    /// <summary>
    /// 资源多列表标签
    /// </summary>
    [SerializeField]
    MultiColumnHeaderState m_AssetListMCHState;

    /// <summary>
    /// 调整了水平方向的分割方式
    /// </summary>
    bool m_ResizingHorizontalSplitter = false;

    /// <summary>
    /// 调整了垂直方向上
    /// </summary>
    bool m_ResizingVerticalSplitter = false;

    /// <summary>
    /// 水平方向和垂直方向上的分割矩形
    /// </summary>
    Rect m_HorizontalSplitterRect, m_VerticalSplitterRect;

    /// <summary>
    /// 水平方向占比
    /// </summary>
    [SerializeField]
    float m_HorizontalSplitterPercent;

    /// <summary>
    /// 垂直方向占比
    /// </summary>
    [SerializeField]
    float m_VerticalSplitterPercent;
    /// <summary>
    /// 当前窗口位置
    /// </summary>
    public Rect m_Position;

    /// <summary>
    /// 资源组树状图
    /// </summary>
    ResGroupTreeEditor m_ResGroupTree;
    ResAssetsTreeEitor mResAssetsTree;
    AssetInfoEditor mAssetInfoEditor;


    const float k_SplitterWidth = 3f;
    private static float m_UpdateDelay = 0f;

    /// <summary>
    /// 父窗体
    /// </summary>
    EditorWindow m_Parent = null;

    public  AssetGroupMgr()
    {
        m_HorizontalSplitterPercent = 0.3f;
        m_VerticalSplitterPercent = 0.4f;
    }

    public void OnEnable(Rect pos, EditorWindow parent)
    {
        m_Parent = parent;
        m_Position = pos;
        m_HorizontalSplitterRect = new Rect(
                (int)(m_Position.x + m_Position.width * m_HorizontalSplitterPercent),
                m_Position.y,
                k_SplitterWidth,
                m_Position.height);

        m_VerticalSplitterRect = new Rect(
               m_HorizontalSplitterRect.x,
               (int)(m_Position.y + m_HorizontalSplitterRect.height * m_VerticalSplitterPercent),
               (m_Position.width - m_HorizontalSplitterRect.width) - k_SplitterWidth,
               k_SplitterWidth);
    }
    public void Update()
    {
       
    }
    private TreeViewState testTreeState;
    public void OnGUI(Rect pos)
    {
        m_Position = pos;
        if (m_ResGroupTree == null)
        {

            if (m_GroupTreeState == null) m_GroupTreeState = new TreeViewState();

            var headerState = ResGroupTreeEditor.CreateDefaultMultiColumnHeaderState();// 
            if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_GroupTreeMCHState, headerState))
            {
                MultiColumnHeaderState.OverwriteSerializedFields(m_GroupTreeMCHState, headerState);
            }
            m_GroupTreeMCHState = headerState;
            m_ResGroupTree = new ResGroupTreeEditor(m_GroupTreeState, this, m_GroupTreeMCHState);
            if (mResAssetsTree == null)
            {
                if (m_AssetListState == null) m_AssetListState = new TreeViewState();
                var assetHeaderState = ResAssetsTreeEitor.CreateDefaultMultiColumnHeaderState();
                if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_AssetListMCHState, assetHeaderState))
                {
                    MultiColumnHeaderState.OverwriteSerializedFields(m_AssetListMCHState, assetHeaderState);
                }
                m_AssetListMCHState = assetHeaderState;
                mResAssetsTree = new ResAssetsTreeEitor(m_AssetListState, m_AssetListMCHState, this);
            }
            if (mAssetInfoEditor == null)
            {
                mAssetInfoEditor = new AssetInfoEditor(this);
            }
            m_ResGroupTree.Reload();
            m_ResGroupTree.Refresh();
            mResAssetsTree.Reload();
            mAssetInfoEditor.Reload();
            m_Parent.Repaint();
        }

        

        HandleHorizontalResize();
        HandleVerticalResize();

        var groupTreeRect = new Rect(
                   m_Position.x + k_SplitterWidth,
                   m_Position.y,
                   m_HorizontalSplitterRect.x,
                   m_HorizontalSplitterRect.height - m_Position.y);
        m_GroupTreeMCHState.columns[0].width = groupTreeRect.width;
        m_ResGroupTree.OnGUI(groupTreeRect);

        var assetTreeRect = new Rect(
            m_HorizontalSplitterRect.x + k_SplitterWidth*3,m_VerticalSplitterRect.y,
            m_VerticalSplitterRect.width - k_SplitterWidth * 5,
            m_HorizontalSplitterRect.height  - m_VerticalSplitterRect.y
            ); 

        mResAssetsTree.OnGUI(assetTreeRect);

        var AssetInfoRect = new Rect(
            assetTreeRect.x, m_HorizontalSplitterRect.y,
            assetTreeRect.width-1, assetTreeRect.y - m_HorizontalSplitterRect.y
            );
        mAssetInfoEditor.OnGUI(AssetInfoRect);
        if (m_ResizingHorizontalSplitter || m_ResizingVerticalSplitter)
        {
            m_Parent.Repaint();
            mAssetInfoEditor.Reload();
        }
            

    }

    public void UpdateSelectedGroup(string name)
    {
        if (mResAssetsTree != null)
        {
            mResAssetsTree.SetSelectedGroups(name);
        }
    }

    private void HandleHorizontalResize()
    {
        m_HorizontalSplitterRect.x = m_Position.width * m_HorizontalSplitterPercent;
        m_HorizontalSplitterRect.height = m_Position.height;

        EditorGUIUtility.AddCursorRect(m_HorizontalSplitterRect, MouseCursor.ResizeHorizontal);
        if (Event.current.type == EventType.mouseDown && m_HorizontalSplitterRect.Contains(Event.current.mousePosition))
            m_ResizingHorizontalSplitter = true;

        if (m_ResizingHorizontalSplitter)
        {
            m_HorizontalSplitterPercent = Mathf.Clamp(Event.current.mousePosition.x / m_Position.width, 0.1f, 0.9f);
            m_HorizontalSplitterRect.x = (int)(m_Position.width * m_HorizontalSplitterPercent);
        }

        if (Event.current.type == EventType.MouseUp)
            m_ResizingHorizontalSplitter = false;
    }

    private void HandleVerticalResize()
    {
        m_VerticalSplitterRect = new Rect(
              m_HorizontalSplitterRect.x,
              (int)(m_Position.y + m_HorizontalSplitterRect.height * m_VerticalSplitterPercent),
              (m_Position.width - m_HorizontalSplitterRect.width) - k_SplitterWidth,
              k_SplitterWidth);

        m_VerticalSplitterRect.x = m_HorizontalSplitterRect.x;
        m_VerticalSplitterRect.y = m_HorizontalSplitterRect.height * m_VerticalSplitterPercent;

        m_VerticalSplitterRect.width = m_Position.width - m_HorizontalSplitterRect.x;
      


        EditorGUIUtility.AddCursorRect(m_VerticalSplitterRect, MouseCursor.ResizeVertical);
        if (Event.current.type == EventType.mouseDown && m_VerticalSplitterRect.Contains(Event.current.mousePosition))
            m_ResizingVerticalSplitter = true;

      


        if (m_ResizingVerticalSplitter)
        {
            m_VerticalSplitterPercent = Mathf.Clamp(Event.current.mousePosition.y / m_HorizontalSplitterRect.height, 0.2f, 0.98f);
            m_VerticalSplitterRect.y = (int)(m_HorizontalSplitterRect.height * m_VerticalSplitterPercent);
        }


        if (Event.current.type == EventType.MouseUp)
        {
            m_ResizingVerticalSplitter = false;
        }
    }

    internal void UpdateSelectedAssets(List<AssetMode.AssetInfo> list)
    {
        if (this.mAssetInfoEditor != null)
        {
            this.mAssetInfoEditor.SelectedAssets(list);
        }
    }
}
