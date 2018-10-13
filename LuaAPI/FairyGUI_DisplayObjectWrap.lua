---@class FairyGUI.DisplayObject : FairyGUI.EventDispatcher
---@field public parent FairyGUI.Container
---@field public gameObject UnityEngine.GameObject
---@field public cachedTransform UnityEngine.Transform
---@field public graphics FairyGUI.NGraphics
---@field public paintingGraphics FairyGUI.NGraphics
---@field public onClick FairyGUI.EventListener
---@field public onRightClick FairyGUI.EventListener
---@field public onTouchBegin FairyGUI.EventListener
---@field public onTouchMove FairyGUI.EventListener
---@field public onTouchEnd FairyGUI.EventListener
---@field public onRollOver FairyGUI.EventListener
---@field public onRollOut FairyGUI.EventListener
---@field public onMouseWheel FairyGUI.EventListener
---@field public onAddedToStage FairyGUI.EventListener
---@field public onRemovedFromStage FairyGUI.EventListener
---@field public onKeyDown FairyGUI.EventListener
---@field public onClickLink FairyGUI.EventListener
---@field public alpha float
---@field public grayed bool
---@field public visible bool
---@field public x float
---@field public y float
---@field public z float
---@field public xy UnityEngine.Vector2
---@field public position UnityEngine.Vector3
---@field public width float
---@field public height float
---@field public size UnityEngine.Vector2
---@field public scaleX float
---@field public scaleY float
---@field public scale UnityEngine.Vector2
---@field public rotation float
---@field public rotationX float
---@field public rotationY float
---@field public skew UnityEngine.Vector2
---@field public perspective bool
---@field public focalLength int
---@field public pivot UnityEngine.Vector2
---@field public location UnityEngine.Vector3
---@field public material UnityEngine.Material
---@field public shader string
---@field public renderingOrder int
---@field public layer int
---@field public isDisposed bool
---@field public topmost FairyGUI.Container
---@field public stage FairyGUI.Stage
---@field public worldSpaceContainer FairyGUI.Container
---@field public touchable bool
---@field public paintingMode bool
---@field public cacheAsBitmap bool
---@field public filter FairyGUI.IFilter
---@field public blendMode FairyGUI.BlendMode
---@field public home UnityEngine.Transform
---@field public name string
---@field public onPaint FairyGUI.EventCallback0
---@field public gOwner FairyGUI.GObject
---@field public id uint
local m = {}
---@param xv float
---@param yv float
function m:SetXY(xv, yv) end
---@param xv float
---@param yv float
---@param zv float
function m:SetPosition(xv, yv, zv) end
---@param wv float
---@param hv float
function m:SetSize(wv, hv) end
function m:EnsureSizeCorrect() end
---@param xv float
---@param yv float
function m:SetScale(xv, yv) end
---@param requestorId int
---@param margin System.Nullable
function m:EnterPaintingMode(requestorId, margin) end
---@param requestorId int
function m:LeavePaintingMode(requestorId) end
---@param targetSpace FairyGUI.DisplayObject
---@return UnityEngine.Rect
function m:GetBounds(targetSpace) end
---@param point UnityEngine.Vector2
---@return UnityEngine.Vector2
function m:GlobalToLocal(point) end
---@param point UnityEngine.Vector2
---@return UnityEngine.Vector2
function m:LocalToGlobal(point) end
---@param worldPoint UnityEngine.Vector3
---@param direction UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:WorldToLocal(worldPoint, direction) end
---@param point UnityEngine.Vector2
---@param targetSpace FairyGUI.DisplayObject
---@return UnityEngine.Vector2
function m:TransformPoint(point, targetSpace) end
---@param rect UnityEngine.Rect
---@param targetSpace FairyGUI.DisplayObject
---@return UnityEngine.Rect
function m:TransformRect(rect, targetSpace) end
function m:RemoveFromParent() end
function m:InvalidateBatchingState() end
---@param context FairyGUI.UpdateContext
function m:Update(context) end
function m:Dispose() end
return m