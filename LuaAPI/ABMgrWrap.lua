---@class ABMgr : EventDispatcherNode
---@field public Instance ABMgr
---@field public threadSafe bool
local m = {}
---@param ABName string
---@param callBack System.Action
---@param keepInMemory bool
---@param Async bool
function m:LoadAB(ABName, callBack, keepInMemory, Async) end
---@param ABName string
---@return ABMgr.PackInfo
function m:GetAB(ABName) end
function m:OnUpdate() end
function m:OnDestroy() end
---@param forced bool
function m:UnLoadAll(forced) end
---@param abName string
---@param immediate bool
function m:UnLoadAB(abName, immediate) end
---@return System.Collections.IEnumerator
function m:Test() end
return m