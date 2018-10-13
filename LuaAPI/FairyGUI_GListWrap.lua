---@class FairyGUI.GList : FairyGUI.GComponent
---@field public onClickItem FairyGUI.EventListener
---@field public onRightClickItem FairyGUI.EventListener
---@field public layout FairyGUI.ListLayoutType
---@field public lineCount int
---@field public columnCount int
---@field public lineGap int
---@field public columnGap int
---@field public align FairyGUI.AlignType
---@field public verticalAlign FairyGUI.VertAlignType
---@field public autoResizeItem bool
---@field public itemPool FairyGUI.GObjectPool
---@field public selectedIndex int
---@field public selectionController FairyGUI.Controller
---@field public touchItem FairyGUI.GObject
---@field public isVirtual bool
---@field public numItems int
---@field public defaultItem string
---@field public foldInvisibleItems bool
---@field public selectionMode FairyGUI.ListSelectionMode
---@field public itemRenderer FairyGUI.ListItemRenderer
---@field public itemProvider FairyGUI.ListItemProvider
---@field public scrollItemToViewOnClick bool
---@field public name string
---@field public data object
---@field public sourceWidth int
---@field public sourceHeight int
---@field public initWidth int
---@field public initHeight int
---@field public minWidth int
---@field public maxWidth int
---@field public minHeight int
---@field public maxHeight int
---@field public dragBounds System.Nullable
---@field public packageItem FairyGUI.PackageItem
local m = {}
function m:Dispose() end
---@param url string
---@return FairyGUI.GObject
function m:GetFromPool(url) end
---@return FairyGUI.GObject
function m:AddItemFromPool() end
---@param child FairyGUI.GObject
---@param index int
---@return FairyGUI.GObject
function m:AddChildAt(child, index) end
---@param index int
---@param dispose bool
---@return FairyGUI.GObject
function m:RemoveChildAt(index, dispose) end
---@param index int
function m:RemoveChildToPoolAt(index) end
---@param child FairyGUI.GObject
function m:RemoveChildToPool(child) end
function m:RemoveChildrenToPool() end
---@return table
function m:GetSelection() end
---@param index int
---@param scrollItToView bool
function m:AddSelection(index, scrollItToView) end
---@param index int
function m:RemoveSelection(index) end
function m:ClearSelection() end
function m:SelectAll() end
function m:SelectNone() end
function m:SelectReverse() end
---@param dir int
function m:HandleArrowKey(dir) end
---@param itemCount int
function m:ResizeToFit(itemCount) end
---@param c FairyGUI.Controller
function m:HandleControllerChanged(c) end
---@param index int
function m:ScrollToView(index) end
---@return int
function m:GetFirstChildInView() end
---@param index int
---@return int
function m:ChildIndexToItemIndex(index) end
---@param index int
---@return int
function m:ItemIndexToChildIndex(index) end
function m:SetVirtual() end
function m:SetVirtualAndLoop() end
function m:RefreshVirtualList() end
---@param xml FairyGUI.Utils.XML
function m:Setup_BeforeAdd(xml) end
---@param xml FairyGUI.Utils.XML
function m:Setup_AfterAdd(xml) end
return m