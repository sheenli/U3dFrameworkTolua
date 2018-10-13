---@class LuaInterface.EventObject : object
local m = {}
---@param a LuaInterface.EventObject
---@param b System.Delegate
---@return LuaInterface.EventObject
function m.op_Addition(a, b) end
---@param a LuaInterface.EventObject
---@param b System.Delegate
---@return LuaInterface.EventObject
function m.op_Subtraction(a, b) end
return m