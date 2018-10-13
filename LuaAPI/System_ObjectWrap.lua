---@class System.Object
local m = {}
---@param obj object
---@return bool
function m:Equals(obj) end
---@return int
function m:GetHashCode() end
---@return System.Type
function m:GetType() end
---@return string
function m:ToString() end
---@param objA object
---@param objB object
---@return bool
function m.ReferenceEquals(objA, objB) end
return m