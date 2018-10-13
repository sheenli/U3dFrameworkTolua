---@class YKSupportLua.LuaTaskBase : object
---@field public IsFailure bool
---@field public IsFinished bool
---@field public mTaskName string
---@field public mFailureInfo string
---@field public isFailure bool
---@field public isFinished bool
local m = {}
---@param peerTable LuaInterface.LuaTable
function m:ConnectLua(peerTable) end
---@return string
function m:TaskName() end
function m:OnExecute() end
---@return string
function m:FailureInfo() end
function m:Rest() end
return m