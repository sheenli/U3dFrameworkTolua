---@class FairyGUI.GObject : FairyGUI.EventDispatcher
---@field public id string
---@field public relations FairyGUI.Relations
---@field public parent FairyGUI.GComponent
---@field public displayObject FairyGUI.DisplayObject
---@field public onClick FairyGUI.EventListener
---@field public onRightClick FairyGUI.EventListener
---@field public onTouchBegin FairyGUI.EventListener
---@field public onTouchMove FairyGUI.EventListener
---@field public onTouchEnd FairyGUI.EventListener
---@field public onRollOver FairyGUI.EventListener
---@field public onRollOut FairyGUI.EventListener
---@field public onAddedToStage FairyGUI.EventListener
---@field public onRemovedFromStage FairyGUI.EventListener
---@field public onKeyDown FairyGUI.EventListener
---@field public onClickLink FairyGUI.EventListener
---@field public onPositionChanged FairyGUI.EventListener
---@field public onSizeChanged FairyGUI.EventListener
---@field public onDragStart FairyGUI.EventListener
---@field public onDragMove FairyGUI.EventListener
---@field public onDragEnd FairyGUI.EventListener
---@field public OnGearStop FairyGUI.EventListener
---@field public draggingObject FairyGUI.GObject
---@field public x float
---@field public y float
---@field public z float
---@field public xy UnityEngine.Vector2
---@field public position UnityEngine.Vector3
---@field public pixelSnapping bool
---@field public width float
---@field public height float
---@field public size UnityEngine.Vector2
---@field public actualWidth float
---@field public actualHeight float
---@field public xMin float
---@field public yMin float
---@field public scaleX float
---@field public scaleY float
---@field public scale UnityEngine.Vector2
---@field public skew UnityEngine.Vector2
---@field public pivotX float
---@field public pivotY float
---@field public pivot UnityEngine.Vector2
---@field public pivotAsAnchor bool
---@field public touchable bool
---@field public grayed bool
---@field public enabled bool
---@field public rotation float
---@field public rotationX float
---@field public rotationY float
---@field public alpha float
---@field public visible bool
---@field public sortingOrder int
---@field public focusable bool
---@field public focused bool
---@field public tooltips string
---@field public filter FairyGUI.IFilter
---@field public blendMode FairyGUI.BlendMode
---@field public gameObjectName string
---@field public inContainer bool
---@field public onStage bool
---@field public resourceURL string
---@field public gearXY FairyGUI.GearXY
---@field public gearSize FairyGUI.GearSize
---@field public gearLook FairyGUI.GearLook
---@field public group FairyGUI.GGroup
---@field public root FairyGUI.GRoot
---@field public text string
---@field public icon string
---@field public draggable bool
---@field public dragging bool
---@field public asImage FairyGUI.GImage
---@field public asCom FairyGUI.GComponent
---@field public asButton FairyGUI.GButton
---@field public asLabel FairyGUI.GLabel
---@field public asProgress FairyGUI.GProgressBar
---@field public asSlider FairyGUI.GSlider
---@field public asComboBox FairyGUI.GComboBox
---@field public asTextField FairyGUI.GTextField
---@field public asRichTextField FairyGUI.GRichTextField
---@field public asTextInput FairyGUI.GTextInput
---@field public asLoader FairyGUI.GLoader
---@field public asList FairyGUI.GList
---@field public asGraph FairyGUI.GGraph
---@field public asGroup FairyGUI.GGroup
---@field public asMovieClip FairyGUI.GMovieClip
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
---@param xv float
---@param yv float
function m:SetXY(xv, yv) end
---@param xv float
---@param yv float
---@param zv float
function m:SetPosition(xv, yv, zv) end
function m:Center() end
function m:MakeFullScreen() end
---@param wv float
---@param hv float
function m:SetSize(wv, hv) end
---@param wv float
---@param hv float
function m:SetScale(wv, hv) end
---@param xv float
---@param yv float
function m:SetPivot(xv, yv) end
function m:RequestFocus() end
---@param obj FairyGUI.GObject
function m:SetHome(obj) end
---@param index int
---@return FairyGUI.GearBase
function m:GetGear(index) end
function m:InvalidateBatchingState() end
---@param c FairyGUI.Controller
function m:HandleControllerChanged(c) end
---@param target FairyGUI.GObject
---@param relationType FairyGUI.RelationType
function m:AddRelation(target, relationType) end
---@param target FairyGUI.GObject
---@param relationType FairyGUI.RelationType
function m:RemoveRelation(target, relationType) end
function m:RemoveFromParent() end
function m:StartDrag() end
function m:StopDrag() end
---@param pt UnityEngine.Vector2
---@return UnityEngine.Vector2
function m:LocalToGlobal(pt) end
---@param pt UnityEngine.Vector2
---@return UnityEngine.Vector2
function m:GlobalToLocal(pt) end
---@param pt UnityEngine.Vector2
---@param r FairyGUI.GRoot
---@return UnityEngine.Vector2
function m:LocalToRoot(pt, r) end
---@param pt UnityEngine.Vector2
---@param r FairyGUI.GRoot
---@return UnityEngine.Vector2
function m:RootToLocal(pt, r) end
---@param pt UnityEngine.Vector3
---@return UnityEngine.Vector2
function m:WorldToLocal(pt) end
---@param pt UnityEngine.Vector2
---@param targetSpace FairyGUI.GObject
---@return UnityEngine.Vector2
function m:TransformPoint(pt, targetSpace) end
---@param rect UnityEngine.Rect
---@param targetSpace FairyGUI.GObject
---@return UnityEngine.Rect
function m:TransformRect(rect, targetSpace) end
function m:Dispose() end
function m:ConstructFromResource() end
---@param xml FairyGUI.Utils.XML
function m:Setup_BeforeAdd(xml) end
---@param xml FairyGUI.Utils.XML
function m:Setup_AfterAdd(xml) end
---@param endValue UnityEngine.Vector2
---@param duration float
---@return DG.Tweening.Tweener
function m:TweenMove(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:TweenMoveX(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:TweenMoveY(endValue, duration) end
---@param endValue UnityEngine.Vector2
---@param duration float
---@return DG.Tweening.Tweener
function m:TweenScale(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:TweenScaleX(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:TweenScaleY(endValue, duration) end
---@param endValue UnityEngine.Vector2
---@param duration float
---@return DG.Tweening.Tweener
function m:TweenResize(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:TweenFade(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:TweenRotate(endValue, duration) end
return m