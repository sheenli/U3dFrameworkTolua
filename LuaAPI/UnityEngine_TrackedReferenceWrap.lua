---@class UnityEngine.TrackedReference : object
local m = {}
---@param x UnityEngine.TrackedReference
---@param y UnityEngine.TrackedReference
---@return bool
function m.op_Equality(x, y) end
---@param x UnityEngine.TrackedReference
---@param y UnityEngine.TrackedReference
---@return bool
function m.op_Inequality(x, y) end
---@param o object
---@return bool
function m:Equals(o) end
---@return int
function m:GetHashCode() end
---@param exists UnityEngine.TrackedReference
---@return bool
function m.op_Implicit(exists) end
return m