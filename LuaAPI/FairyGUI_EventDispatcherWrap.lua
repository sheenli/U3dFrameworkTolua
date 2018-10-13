---@class FairyGUI.EventDispatcher : object
local m = {}
---@param strType string
---@param callback FairyGUI.EventCallback1
function m:AddEventListener(strType, callback) end
---@param strType string
---@param callback FairyGUI.EventCallback1
function m:RemoveEventListener(strType, callback) end
function m:RemoveEventListeners() end
---@param strType string
---@return bool
function m:DispatchEvent(strType) end
---@param strType string
---@param data object
---@return bool
function m:BubbleEvent(strType, data) end
---@param strType string
---@param data object
---@return bool
function m:BroadcastEvent(strType, data) end
return m