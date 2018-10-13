---@class FairyGUI.EventContext : object
---@field public sender FairyGUI.EventDispatcher
---@field public initiator object
---@field public inputEvent FairyGUI.InputEvent
---@field public isDefaultPrevented bool
---@field public type string
---@field public data object
local m = {}
function m:StopPropagation() end
function m:PreventDefault() end
function m:CaptureTouch() end
return m