---@class FairyGUI.Timers : object
---@field public inst FairyGUI.Timers
---@field public repeat int
---@field public time float
---@field public gameObject UnityEngine.GameObject
local m = {}
---@param interval float
---@param repeat int
---@param callback FairyGUI.TimerCallback
function m:Add(interval, repeat, callback) end
---@param callback FairyGUI.TimerCallback
function m:CallLater(callback) end
---@param callback FairyGUI.TimerCallback
function m:AddUpdate(callback) end
---@param routine System.Collections.IEnumerator
function m:StartCoroutine(routine) end
---@param callback FairyGUI.TimerCallback
---@return bool
function m:Exists(callback) end
---@param callback FairyGUI.TimerCallback
function m:Remove(callback) end
function m:Update() end
return m