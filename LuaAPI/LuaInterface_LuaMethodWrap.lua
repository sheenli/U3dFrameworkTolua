---@class LuaInterface.LuaMethod : object
local m = {}
---@param L System.IntPtr
---@return int
function m:Call(L) end
function m:Destroy() end
return m