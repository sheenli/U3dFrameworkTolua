---@class LuaInterface.Debugger
---@field public useLog bool
---@field public threadStack string
---@field public logger LuaInterface.ILogger
local m = {}
---@param str string
function m.Log(str) end
---@param str string
function m.LogWarning(str) end
---@param str string
function m.LogError(str) end
---@param e System.Exception
function m.LogException(e) end
return m