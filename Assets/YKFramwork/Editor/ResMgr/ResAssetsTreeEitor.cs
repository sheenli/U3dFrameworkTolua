using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class ResAssetsTreeEitor : TreeView
{
    private List<Type> CanDropType = new List<Type>()
    {
        typeof(UnityEngine.TextAsset),
        typeof(UnityEngine.Texture),
        typeof(UnityEngine.Texture2D),
        typeof(UnityEngine.Texture3D),
        typeof(UnityEngine.AudioClip),
        typeof(UnityEngine.Material),
        typeof(UnityEngine.GameObject),
    };

    /// <summary>
    /// 当前所选资源组名称
    /// </summary>
    public AssetMode.GroupInfo group = null;
    public List<AssetMode.AssetInfo> assets = new List<AssetMode.AssetInfo>();


    public AssetGroupMgr mController;
    private bool mContextOnItem = false;
    public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
    {
        return new MultiColumnHeaderState(GetColumns());
    }

    private static MultiColumnHeaderState.Column[] GetColumns()
    {
        var retVal = new MultiColumnHeaderState.Column[] {
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column()
            };
        retVal[0].headerContent = new GUIContent("文件名称");
        retVal[0].minWidth = 0;
        retVal[0].width = 100;
        retVal[0].maxWidth = 300;
        retVal[0].headerTextAlignment = TextAlignment.Center;
        retVal[0].canSort = false;
        retVal[0].autoResize = true;
        retVal[0].allowToggleVisibility = false;

        retVal[1].headerContent = new GUIContent("文件大小");
        retVal[1].minWidth = 50;
        retVal[1].width = 75;
        retVal[1].maxWidth = 75;
        retVal[1].headerTextAlignment = TextAlignment.Center;
        retVal[1].canSort = false;
        retVal[1].autoResize = true;
        retVal[1].allowToggleVisibility = true;

        retVal[2].headerContent = new GUIContent("路径");
        retVal[2].minWidth = 30;
        retVal[2].width = 200;
        retVal[2].maxWidth = 1000;
        retVal[2].headerTextAlignment = TextAlignment.Center;
        retVal[2].canSort = false;
        retVal[2].autoResize = true;
        retVal[2].allowToggleVisibility = true;

        return retVal;
    }

    enum MyColumns
    {
        Asset,
        Size,
        Path
    }

    public ResAssetsTreeEitor(TreeViewState state, MultiColumnHeaderState mchs, AssetGroupMgr ctrl)
        : base(state, new MultiColumnHeader(mchs))
    {
        mController = ctrl;
        showBorder = true;
        showAlternatingRowBackgrounds = true;
        DefaultStyles.label.richText = true;
        Reload();
    }
    public void Update()
    {
        if (AssetMode.Update())
        {
            Reload();
        }

    }

    protected override bool CanMultiSelect(TreeViewItem item)
    {
        return true;
    }

    internal void SetSelectedGroups(string groupNames)
    {
        assets.Clear();
        if (string.IsNullOrEmpty(groupNames))
        {
            this.group = null;
        }
        else
        {
            this.group = AssetMode.GetGroupInfo(groupNames);
            string[] datas = AssetMode.resInfo.GetAssetsNames(this.group.Name);
            int id = 0;
            foreach (string str in datas)
            {
                AssetMode.AssetInfo info = new AssetMode.AssetInfo(id++, str);
                assets.Add(info);
            }
        }
        //m_Controller.SetSelectedItems(null);
        //m_SourceBundles = bundles.ToList();
        SetSelection(new List<int>());
        Reload();
    }

    public class AssetTreeViewItem : TreeViewItem
    {
        private AssetMode.AssetInfo m_asset;
        public AssetMode.AssetInfo asset
        {
            get { return m_asset; }
        }

        public AssetTreeViewItem(AssetMode.AssetInfo info) : base(info.NameHashCode, info.NameHashCode, info.Name)
        {
            m_asset = info;
            icon = AssetDatabase.GetCachedIcon(info.data.path) as Texture2D;
        }
    }


    protected override TreeViewItem BuildRoot()
    {
        var root = new TreeViewItem();
        root.children = new System.Collections.Generic.List<UnityEditor.IMGUI.Controls.TreeViewItem>();
        root.id = -1;
        root.depth = -1;
        foreach (AssetMode.AssetInfo asset in assets)
        {
            root.children.Add(new AssetTreeViewItem(asset));
        }
        return root;
    }
    public override void OnGUI(Rect rect)
    {
        base.OnGUI(rect);
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
        {
            SetSelection(new int[0], TreeViewSelectionOptions.FireSelectionChanged);
        }
    }

    protected override void RowGUI(RowGUIArgs args)
    {
        for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            CellGUI(args.GetCellRect(i), args.item as AssetTreeViewItem, args.GetColumn(i), ref args);
    }

    private void CellGUI(Rect cellRect, AssetTreeViewItem item, int column, ref RowGUIArgs args)
    {
        Color oldColor = GUI.color;
        CenterRectUsingSingleLineHeight(ref cellRect);
        //if (column != 3)
        //    GUI.color = item.itemColor;
        if (!File.Exists(item.asset.data.path))
        {
            GUI.color = Color.red;
        }
        switch ((MyColumns)column)
        {
            case MyColumns.Asset:
                {
                    var iconRect = new Rect(cellRect.x + 1, cellRect.y + 1, cellRect.height - 2, cellRect.height - 2);
                    if (item.icon != null)
                    {
                        GUI.DrawTexture(iconRect, item.icon, ScaleMode.ScaleToFit);
                    }
                    
                    DefaultGUI.Label(
                        new Rect(cellRect.x + iconRect.xMax + 1, cellRect.y, cellRect.width - iconRect.width, cellRect.height),
                        item.displayName,
                        args.selected,
                        args.focused);
                }
                break;
            case MyColumns.Size:
                DefaultGUI.Label(cellRect, item.asset.GetSizeString(), args.selected, args.focused);
                break;
            case MyColumns.Path:
                DefaultGUI.Label(cellRect, item.asset.data.path, args.selected, args.focused);
                break;

        }
        GUI.color = oldColor;
    }

    protected override void DoubleClickedItem(int id)
    {
        var assetItem = FindItem(id, rootItem) as AssetTreeViewItem;
        if (assetItem != null)
        {
            UnityEngine.Object o = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetItem.asset.data.path);
            EditorGUIUtility.PingObject(o);
            Selection.activeObject = o;
        }
    }

    protected override bool CanBeParent(TreeViewItem item)
    {
        return false;
    }

    /// <summary>
    /// 有东西被拖动到了这
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
    {
        if (IsValidDragDrop(args))
        {
            if (args.performDrop)
            {
                List<string> name = AssetMode.AddAssetToGroup(DragAndDrop.paths, group.Name);
                foreach (string path in DragAndDrop.paths)
                {
                    Debug.Log("SetupDragAndDrop 2" + path);
                }
                ReloadAndSelect(name);
            }
            
            return DragAndDropVisualMode.Copy;//Move;
        }


        return DragAndDropVisualMode.Rejected;
    }

    /// <summary>
    /// 这个文件是否能拖过来
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    protected bool IsValidDragDrop(DragAndDropArgs args)
    {
        if (group == null)
        {
            return false;
        }

        if (DragAndDrop.paths == null || DragAndDrop.paths.Length == 0)
            return false;

        //从场景拖过来的不行//
        foreach (var assetPath in DragAndDrop.paths)
        {
            if (!assetPath.StartsWith(FileUtil.GetProjectRelativePath(assetPath)) && !assetPath.StartsWith("Assets/Resources"))
            {
                return false;
            }

            Type t = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
   
            if (!CanDropType.Contains(t) && !AssetDatabase.IsValidFolder(assetPath))
            {
                Debug.LogError("Type=" + t);
                return false;
            }
        }
        return true;
    }
    

    protected override void SelectionChanged(IList<int> selectedIds)
    {
        List<AssetMode.AssetInfo> list = new List<AssetMode.AssetInfo>();
        foreach (int nodeId in selectedIds)
        {
            AssetTreeViewItem item = FindItem(nodeId, rootItem) as AssetTreeViewItem;
            if (item != null)
            {
                list.Add(item.asset);
            }
        }

        if (list.Count == 1)
        {
            mController.UpdateSelectedAssets(list);
        }

        
    }

    protected override void ContextClickedItem(int id)
    {
        List<AssetTreeViewItem> selectedNodes = new List<AssetTreeViewItem>();
        foreach (var nodeID in GetSelection())
        {
            selectedNodes.Add(FindItem(nodeID, rootItem) as AssetTreeViewItem);
        }
        if (selectedNodes.Count > 0)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("从资源组中移出选中的资源"), false, RemoveAssets, selectedNodes);
            menu.ShowAsContext();
        }
    }

    void RemoveAssets(object obj)
    {
        List<AssetTreeViewItem> list = obj as List<AssetTreeViewItem>;
        List<string> reAssets = new List<string>();
        foreach (AssetTreeViewItem item in list)
        {
            reAssets.Add(item.asset.data.name);
        }
        AssetMode.RemoveAssets(reAssets, group.Name);
        ReloadAndSelect(new int[0]);
    }

    protected override void KeyEvent()
    {
        if (group != null && Event.current.keyCode == KeyCode.Delete && GetSelection().Count > 0)
        {
            List<AssetTreeViewItem> selectedNodes = new List<AssetTreeViewItem>();
            foreach (var nodeID in GetSelection())
            {
                selectedNodes.Add(FindItem(nodeID, rootItem) as AssetTreeViewItem);
            }
            RemoveAssets(selectedNodes);
        }
    }

    private void ReloadAndSelect(IList<int> hashCodes)
    {
        SetSelectedGroups(group.Name);
        Reload();
        SetSelection(hashCodes);
        SelectionChanged(hashCodes);
    }

    private void ReloadAndSelect(IList<string> names)
    {
        SetSelectedGroups(group.Name);
        Reload();
        List<int> assetNames = new List<int>();
        foreach (string asName in names)
        {
            AssetMode.AssetInfo info = assets.Find(a => a.Name == asName);
            assetNames.Add(info.NameHashCode);
        }
        ReloadAndSelect(assetNames);
    }
}