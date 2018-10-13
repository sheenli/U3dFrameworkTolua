---@class StateMachine : object
---@field public CurState IState
---@field public CurStateId uint
---@field public BetweenSwitchStateCallBack StateMachine.BetweenSwitchState
local m = {}
---@param state IState
---@return bool
function m:RegisterState(state) end
---@param stateId uint
---@return bool
function m:RemoveState(stateId) end
---@param stateId uint
---@return IState
function m:GetState(stateId) end
---@param param1 object
---@param param2 object
function m:StopState(param1, param2) end
---@param newStateId uint
---@param param1 object
---@param param2 object
---@return bool
function m:SwitchState(newStateId, param1, param2) end
---@param stateId uint
---@return bool
function m:InState(stateId) end
function m:Update() end
function m:FixedUpdate() end
function m:LateUpdate() end
function m:Release() end
return m