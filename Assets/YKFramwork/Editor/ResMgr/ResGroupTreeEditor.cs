using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class ResGroupTreeEditor : TreeView
{
   

    public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
    {
        return new MultiColumnHeaderState(GetColumns());
    }

    private static MultiColumnHeaderState.Column[] GetColumns()
    {
        var retVal = new MultiColumnHeaderState.Column[] {
                new MultiColumnHeaderState.Column()
            };
        retVal[0].headerContent = new GUIContent("资源组名", "每个场景对应一个资源组");
        retVal[0].minWidth = 0;
        retVal[0].width = 100;
        retVal[0].maxWidth = 300;
        retVal[0].headerTextAlignment = TextAlignment.Center;
        retVal[0].canSort = false;
        retVal[0].autoResize = true;
        retVal[0].allowToggleVisibility = false;
        return retVal;
    }
    /// <summary>
    /// 当前选中了
    /// </summary>
    private bool mContextOnItem = false;
    private AssetGroupMgr mController;
    public ResGroupTreeEditor(TreeViewState state, AssetGroupMgr ctrl, MultiColumnHeaderState mchs) : base(state,new MultiColumnHeader(mchs))
    {
        showBorder = true;
        
        showAlternatingRowBackgrounds = false;
        DefaultStyles.label.richText = true;
        AssetMode.Rebuild();
        mController = ctrl;
        Refresh();
        Reload();
    }
    
    /// <summary>
    /// 是否能多选
    /// </summary>
    /// <param name="item">选中的文件</param>
    /// <returns></returns>
    protected override bool CanMultiSelect(TreeViewItem item)
    {
        return false;
    }

    /// <summary>
    /// 能否重新命名
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    protected override bool CanRename(TreeViewItem item)
    {
        return item.displayName.Length > 0;
    }

    /// <summary>
    /// 绘制行
    /// </summary>
    /// <param name="args"></param>
    protected override void RowGUI(RowGUIArgs args)
    {
        for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            CellGUI(args.GetCellRect(i), args.item, args.GetColumn(i), ref args);
    }
    private void CellGUI(Rect cellRect, TreeViewItem item, int column, ref RowGUIArgs args)
    {
        Color oldColor = GUI.color; 
        CenterRectUsingSingleLineHeight(ref cellRect);
        if (column == 0)
        {
            DefaultGUI.Label(
                           new Rect(cellRect.x , cellRect.y, cellRect.width , cellRect.height),
                           item.displayName,
                           args.selected,
                           args.focused);
        }
        GUI.color = oldColor;
    }

    private TreeViewItem mRoot = null;

    protected override TreeViewItem BuildRoot()
    {
        mRoot = new TreeViewItem();
        mRoot.id = -1;
        mRoot.depth = -1;
        mRoot.children = new List<TreeViewItem>();
        foreach (AssetMode.GroupInfo name in AssetMode.gropsEditorInfo)
        {
            var t = new TreeViewItem(name.NameHashCode, name.NameHashCode, name.Name);
            mRoot.AddChild(t);
        }
        return mRoot;
    }

    internal void Refresh()
    {
        var selection = GetSelection();
        
        SelectionChanged(selection);
    }
    

    /// <summary>
    /// 更改名称完成
    /// </summary>
    /// <param name="args"></param>
    protected override void RenameEnded(RenameEndedArgs args)
    {
        base.RenameEnded(args);

        if (args.newName != null && args.newName.Length > 0 && args.newName != args.originalName)
        {
            //args.newName = args.newName.ToLower();
            args.acceptedRename = true;

            args.acceptedRename = AssetMode.HandleGroupRename(args.originalName, args.newName);
            if (args.acceptedRename)
            {
                ReloadAndSelect(args.itemID, false);
            }
        }
        else
        {
            args.acceptedRename = false;
        }
    }



    private void ReloadAndSelect(int hashCode, bool rename)
    {
        var selection = new List<int>();
        selection.Add(hashCode);
        ReloadAndSelect(selection);
        if (rename)
        {
            BeginRename(FindItem(hashCode, rootItem), 0.1f);
        }
    }


    private void ReloadAndSelect(IList<int> hashCodes)
    {
        Reload();
        SetSelection(hashCodes, TreeViewSelectionOptions.RevealAndFrame);
        SelectionChanged(hashCodes);
    }

    protected override void SelectionChanged(IList<int> selectedIds)
    {
        var selectedBundles = new List<string>();
        foreach (var id in selectedIds)
        {
            var item = FindItem(id, rootItem);
            if(item != null)
            selectedBundles.Add(item.displayName);
        }
        if (selectedBundles.Count > 0)
        {
            mController.UpdateSelectedGroup(selectedBundles[0]);
        }
    }
    protected override Rect GetRenameRect(Rect rowRect, int row, TreeViewItem item)
    {
        return rowRect;
    }

    protected override float GetCustomRowHeight(int row, TreeViewItem item)
    {
        return 30;
    }
    public override void OnGUI(Rect rect)
    {
        
        base.OnGUI(rect);
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
        {
            SetSelection(new int[0], TreeViewSelectionOptions.FireSelectionChanged);
        }
    }

    /// <summary>
    /// 点击的时候
    /// </summary>
    protected override void ContextClicked()
    {
        if (HasSelection())
        {
            mContextOnItem = false;
            return;
        }

        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("添加资源组 "),false, CreateResGroup, null);
        menu.ShowAsContext();
    }

    protected override void ContextClickedItem(int id)
    {
        if (mContextOnItem)
        {
            mContextOnItem = false;
            return;
        }
        mContextOnItem = true;
        List<AssetMode.GroupInfo> selectedNodes = new List<AssetMode.GroupInfo>();
        foreach (var nodeID in GetSelection())
        {
            selectedNodes.Add(AssetMode.GetGroupInfo(nodeID));
        }
        GenericMenu menu = new GenericMenu();
        if (selectedNodes.Count == 1)
        {
            
            menu.AddItem(new GUIContent("删除 " + selectedNodes[0].Name + "资源组"), false, DeleteGroups, selectedNodes);
            menu.AddItem(new GUIContent("Rename"), false, RenameGroupName, selectedNodes);
            menu.AddItem(new GUIContent("添加资源组 "), false, CreateResGroup, null);
        }
        menu.ShowAsContext();
    }



    void CreateResGroup(object context)
    {
        string name = AssetMode.HandleGroupCreate();
        ReloadAndSelect(AssetMode.GetGroupInfo(name).NameHashCode, true);
    }

    void DeleteGroups(object b)
    {
        var selectedNodes = b as List<AssetMode.GroupInfo>;

        AssetMode.HandleGroupsDelete(selectedNodes);
        ReloadAndSelect(new List<int>());
    }

    void RenameGroupName(object context)
    {
        var selectedNodes = context as List<AssetMode.GroupInfo>;
        if (selectedNodes != null && selectedNodes.Count > 0)
        {
            TreeViewItem item = FindItem(selectedNodes[0].NameHashCode, rootItem);
            BeginRename(item,0.25f);
        }
    }
}