---@class FairyGUI.EventListener : object
---@field public owner FairyGUI.EventDispatcher
---@field public type string
---@field public isEmpty bool
---@field public isDispatching bool
local m = {}
---@param callback FairyGUI.EventCallback1
function m:AddCapture(callback) end
---@param callback FairyGUI.EventCallback1
function m:RemoveCapture(callback) end
---@param callback FairyGUI.EventCallback1
function m:Add(callback) end
---@param callback FairyGUI.EventCallback1
function m:Remove(callback) end
---@param callback FairyGUI.EventCallback0
function m:Set(callback) end
function m:Clear() end
---@return bool
function m:Call() end
---@param data object
---@return bool
function m:BubbleCall(data) end
---@param data object
---@return bool
function m:BroadcastCall(data) end
return m