---@class YKSupportLua.LuaStateBase : object
---@field public StateId uint
local m = {}
---@param peerTable LuaInterface.LuaTable
function m:ConnectLua(peerTable) end
---@param prevState IState
---@param param1 object
---@param param2 object
function m:OnEnter(prevState, param1, param2) end
function m:OnFixedUpdate() end
function m:OnLateUpdate() end
---@param nextState IState
---@param param1 object
---@param param2 object
function m:OnLeave(nextState, param1, param2) end
function m:OnUpdate() end
function m:OnRelease() end
return m