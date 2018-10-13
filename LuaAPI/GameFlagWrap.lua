---@class GameFlag : object
---@field public Value long
local m = {}
---@param flag long
---@return long
function m:AddFlag(flag) end
---@param flag long
---@return long
function m:RemoveFlag(flag) end
---@param remove bool
---@param flag long
---@return long
function m:ModifyFlag(remove, flag) end
---@param flag long
---@return bool
function m:HasFlag(flag) end
return m