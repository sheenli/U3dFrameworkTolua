---@class YKSupportLua : object
local m = {}
---@param uiName string
---@param param object
---@param lunFun LuaInterface.LuaTable
---@param hideAll bool
---@param hideDotDel bool
function m.ShowWindow(uiName, param, lunFun, hideAll, hideDotDel) end
---@param sceneName string
---@param param object
---@param lunFun LuaInterface.LuaTable
function m.GotoScene(sceneName, param, lunFun) end
return m