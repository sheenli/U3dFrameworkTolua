---@class LuaProfiler
---@field public list table
local m = {}
function m.Clear() end
---@param name string
---@return int
function m.GetID(name) end
---@param id int
function m.BeginSample(id) end
function m.EndSample() end
return m