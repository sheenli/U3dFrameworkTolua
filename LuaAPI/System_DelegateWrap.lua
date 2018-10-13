---@class System.Delegate : object
---@field public Method System.Reflection.MethodInfo
---@field public Target object
local m = {}
---@param type System.Type
---@param firstArgument object
---@param method System.Reflection.MethodInfo
---@param throwOnBindFailure bool
---@return System.Delegate
function m.CreateDelegate(type, firstArgument, method, throwOnBindFailure) end
---@param args table
---@return object
function m:DynamicInvoke(args) end
---@return object
function m:Clone() end
---@param obj object
---@return bool
function m:Equals(obj) end
---@return int
function m:GetHashCode() end
---@param info System.Runtime.Serialization.SerializationInfo
---@param context System.Runtime.Serialization.StreamingContext
function m:GetObjectData(info, context) end
---@return table
function m:GetInvocationList() end
---@param a System.Delegate
---@param b System.Delegate
---@return System.Delegate
function m.Combine(a, b) end
---@param source System.Delegate
---@param value System.Delegate
---@return System.Delegate
function m.Remove(source, value) end
---@param source System.Delegate
---@param value System.Delegate
---@return System.Delegate
function m.RemoveAll(source, value) end
---@param d1 System.Delegate
---@param d2 System.Delegate
---@return bool
function m.op_Equality(d1, d2) end
---@param d1 System.Delegate
---@param d2 System.Delegate
---@return bool
function m.op_Inequality(d1, d2) end
return m