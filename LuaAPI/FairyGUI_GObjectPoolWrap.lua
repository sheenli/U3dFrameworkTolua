---@class FairyGUI.GObjectPool : object
---@field public count int
---@field public initCallback FairyGUI.GObjectPool.InitCallbackDelegate
local m = {}
function m:Clear() end
---@param url string
---@return FairyGUI.GObject
function m:GetObject(url) end
---@param obj FairyGUI.GObject
function m:ReturnObject(obj) end
return m