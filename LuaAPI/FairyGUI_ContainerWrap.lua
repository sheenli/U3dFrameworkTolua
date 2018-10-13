---@class FairyGUI.Container : FairyGUI.DisplayObject
---@field public numChildren int
---@field public clipRect System.Nullable
---@field public mask FairyGUI.DisplayObject
---@field public touchable bool
---@field public contentRect UnityEngine.Rect
---@field public fairyBatching bool
---@field public renderMode UnityEngine.RenderMode
---@field public renderCamera UnityEngine.Camera
---@field public opaque bool
---@field public clipSoftness System.Nullable
---@field public hitArea FairyGUI.IHitTest
---@field public touchChildren bool
---@field public onUpdate FairyGUI.EventCallback0
---@field public reversedMask bool
---@field public name string
---@field public onPaint FairyGUI.EventCallback0
---@field public gOwner FairyGUI.GObject
---@field public id uint
local m = {}
---@param child FairyGUI.DisplayObject
---@return FairyGUI.DisplayObject
function m:AddChild(child) end
---@param child FairyGUI.DisplayObject
---@param index int
---@return FairyGUI.DisplayObject
function m:AddChildAt(child, index) end
---@param child FairyGUI.DisplayObject
---@return bool
function m:Contains(child) end
---@param index int
---@return FairyGUI.DisplayObject
function m:GetChildAt(index) end
---@param name string
---@return FairyGUI.DisplayObject
function m:GetChild(name) end
---@param child FairyGUI.DisplayObject
---@return int
function m:GetChildIndex(child) end
---@param child FairyGUI.DisplayObject
---@return FairyGUI.DisplayObject
function m:RemoveChild(child) end
---@param index int
---@return FairyGUI.DisplayObject
function m:RemoveChildAt(index) end
function m:RemoveChildren() end
---@param child FairyGUI.DisplayObject
---@param index int
function m:SetChildIndex(child, index) end
---@param child1 FairyGUI.DisplayObject
---@param child2 FairyGUI.DisplayObject
function m:SwapChildren(child1, child2) end
---@param index1 int
---@param index2 int
function m:SwapChildrenAt(index1, index2) end
---@param indice table
---@param objs table
function m:ChangeChildrenOrder(indice, objs) end
---@param targetSpace FairyGUI.DisplayObject
---@return UnityEngine.Rect
function m:GetBounds(targetSpace) end
---@return UnityEngine.Camera
function m:GetRenderCamera() end
---@param stagePoint UnityEngine.Vector2
---@param forTouch bool
---@return FairyGUI.DisplayObject
function m:HitTest(stagePoint, forTouch) end
---@return UnityEngine.Vector2
function m:GetHitTestLocalPoint() end
---@param obj FairyGUI.DisplayObject
---@return bool
function m:IsAncestorOf(obj) end
---@param childrenChanged bool
function m:InvalidateBatchingState(childrenChanged) end
---@param value int
function m:SetChildrenLayer(value) end
---@param context FairyGUI.UpdateContext
function m:Update(context) end
function m:Dispose() end
return m