---@class FairyGUI.GComponent : FairyGUI.GObject
---@field public rootContainer FairyGUI.Container
---@field public container FairyGUI.Container
---@field public scrollPane FairyGUI.ScrollPane
---@field public onDrop FairyGUI.EventListener
---@field public fairyBatching bool
---@field public opaque bool
---@field public margin FairyGUI.Margin
---@field public childrenRenderOrder FairyGUI.ChildrenRenderOrder
---@field public apexIndex int
---@field public numChildren int
---@field public Controllers table
---@field public clipSoftness UnityEngine.Vector2
---@field public mask FairyGUI.DisplayObject
---@field public reversedMask bool
---@field public viewWidth float
---@field public viewHeight float
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
---@param childChanged bool
function m:InvalidateBatchingState(childChanged) end
---@param child FairyGUI.GObject
---@return FairyGUI.GObject
function m:AddChild(child) end
---@param child FairyGUI.GObject
---@param index int
---@return FairyGUI.GObject
function m:AddChildAt(child, index) end
---@param child FairyGUI.GObject
---@return FairyGUI.GObject
function m:RemoveChild(child) end
---@param index int
---@return FairyGUI.GObject
function m:RemoveChildAt(index) end
function m:RemoveChildren() end
---@param index int
---@return FairyGUI.GObject
function m:GetChildAt(index) end
---@param name string
---@return FairyGUI.GObject
function m:GetChild(name) end
---@param name string
---@return FairyGUI.GObject
function m:GetVisibleChild(name) end
---@param group FairyGUI.GGroup
---@param name string
---@return FairyGUI.GObject
function m:GetChildInGroup(group, name) end
---@return table
function m:GetChildren() end
---@param child FairyGUI.GObject
---@return int
function m:GetChildIndex(child) end
---@param child FairyGUI.GObject
---@param index int
function m:SetChildIndex(child, index) end
---@param child FairyGUI.GObject
---@param index int
---@return int
function m:SetChildIndexBefore(child, index) end
---@param child1 FairyGUI.GObject
---@param child2 FairyGUI.GObject
function m:SwapChildren(child1, child2) end
---@param index1 int
---@param index2 int
function m:SwapChildrenAt(index1, index2) end
---@param obj FairyGUI.GObject
---@return bool
function m:IsAncestorOf(obj) end
---@param controller FairyGUI.Controller
function m:AddController(controller) end
---@param index int
---@return FairyGUI.Controller
function m:GetControllerAt(index) end
---@param name string
---@return FairyGUI.Controller
function m:GetController(name) end
---@param c FairyGUI.Controller
function m:RemoveController(c) end
---@param index int
---@return FairyGUI.Transition
function m:GetTransitionAt(index) end
---@param name string
---@return FairyGUI.Transition
function m:GetTransition(name) end
---@param child FairyGUI.GObject
---@return bool
function m:IsChildInView(child) end
---@return int
function m:GetFirstChildInView() end
---@param c FairyGUI.Controller
function m:HandleControllerChanged(c) end
function m:SetBoundsChangedFlag() end
function m:EnsureBoundsCorrect() end
function m:ConstructFromResource() end
---@param xml FairyGUI.Utils.XML
function m:ConstructFromXML(xml) end
---@param xml FairyGUI.Utils.XML
function m:Setup_AfterAdd(xml) end
return m