---@class UnityEngine.EventSystems.PointerEventData : UnityEngine.EventSystems.BaseEventData
---@field public pointerEnter UnityEngine.GameObject
---@field public lastPress UnityEngine.GameObject
---@field public rawPointerPress UnityEngine.GameObject
---@field public pointerDrag UnityEngine.GameObject
---@field public pointerCurrentRaycast UnityEngine.EventSystems.RaycastResult
---@field public pointerPressRaycast UnityEngine.EventSystems.RaycastResult
---@field public eligibleForClick bool
---@field public pointerId int
---@field public position UnityEngine.Vector2
---@field public delta UnityEngine.Vector2
---@field public pressPosition UnityEngine.Vector2
---@field public clickTime float
---@field public clickCount int
---@field public scrollDelta UnityEngine.Vector2
---@field public useDragThreshold bool
---@field public dragging bool
---@field public button UnityEngine.EventSystems.PointerEventData.InputButton
---@field public enterEventCamera UnityEngine.Camera
---@field public pressEventCamera UnityEngine.Camera
---@field public pointerPress UnityEngine.GameObject
---@field public hovered table
local m = {}
---@return bool
function m:IsPointerMoving() end
---@return bool
function m:IsScrolling() end
---@return string
function m:ToString() end
return m