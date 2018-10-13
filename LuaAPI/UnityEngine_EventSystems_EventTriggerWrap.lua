---@class UnityEngine.EventSystems.EventTrigger : UnityEngine.MonoBehaviour
---@field public triggers table
local m = {}
---@param eventData UnityEngine.EventSystems.PointerEventData
function m:OnPointerEnter(eventData) end
---@param eventData UnityEngine.EventSystems.PointerEventData
function m:OnPointerExit(eventData) end
---@param eventData UnityEngine.EventSystems.PointerEventData
function m:OnDrag(eventData) end
---@param eventData UnityEngine.EventSystems.PointerEventData
function m:OnDrop(eventData) end
---@param eventData UnityEngine.EventSystems.PointerEventData
function m:OnPointerDown(eventData) end
---@param eventData UnityEngine.EventSystems.PointerEventData
function m:OnPointerUp(eventData) end
---@param eventData UnityEngine.EventSystems.PointerEventData
function m:OnPointerClick(eventData) end
---@param eventData UnityEngine.EventSystems.BaseEventData
function m:OnSelect(eventData) end
---@param eventData UnityEngine.EventSystems.BaseEventData
function m:OnDeselect(eventData) end
---@param eventData UnityEngine.EventSystems.PointerEventData
function m:OnScroll(eventData) end
---@param eventData UnityEngine.EventSystems.AxisEventData
function m:OnMove(eventData) end
---@param eventData UnityEngine.EventSystems.BaseEventData
function m:OnUpdateSelected(eventData) end
---@param eventData UnityEngine.EventSystems.PointerEventData
function m:OnInitializePotentialDrag(eventData) end
---@param eventData UnityEngine.EventSystems.PointerEventData
function m:OnBeginDrag(eventData) end
---@param eventData UnityEngine.EventSystems.PointerEventData
function m:OnEndDrag(eventData) end
---@param eventData UnityEngine.EventSystems.BaseEventData
function m:OnSubmit(eventData) end
---@param eventData UnityEngine.EventSystems.BaseEventData
function m:OnCancel(eventData) end
return m