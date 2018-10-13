---@class UnityEngine.Object : object
---@field public name string
---@field public hideFlags UnityEngine.HideFlags
local m = {}
---@param obj UnityEngine.Object
---@param t float
function m.Destroy(obj, t) end
---@param obj UnityEngine.Object
---@param allowDestroyingAssets bool
function m.DestroyImmediate(obj, allowDestroyingAssets) end
---@param type System.Type
---@return table
function m.FindObjectsOfType(type) end
---@param target UnityEngine.Object
function m.DontDestroyOnLoad(target) end
---@param obj UnityEngine.Object
---@param t float
function m.DestroyObject(obj, t) end
---@return string
function m:ToString() end
---@return int
function m:GetInstanceID() end
---@return int
function m:GetHashCode() end
---@param other object
---@return bool
function m:Equals(other) end
---@param exists UnityEngine.Object
---@return bool
function m.op_Implicit(exists) end
---@param original UnityEngine.Object
---@param position UnityEngine.Vector3
---@param rotation UnityEngine.Quaternion
---@return UnityEngine.Object
function m.Instantiate(original, position, rotation) end
---@param type System.Type
---@return UnityEngine.Object
function m.FindObjectOfType(type) end
---@param x UnityEngine.Object
---@param y UnityEngine.Object
---@return bool
function m.op_Equality(x, y) end
---@param x UnityEngine.Object
---@param y UnityEngine.Object
---@return bool
function m.op_Inequality(x, y) end
return m