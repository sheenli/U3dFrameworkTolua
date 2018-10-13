---@class System.Enum
local m = {}
---@return System.TypeCode
function m:GetTypeCode() end
---@param enumType System.Type
---@return table
function m.GetValues(enumType) end
---@param enumType System.Type
---@return table
function m.GetNames(enumType) end
---@param enumType System.Type
---@param value object
---@return string
function m.GetName(enumType, value) end
---@param enumType System.Type
---@param value object
---@return bool
function m.IsDefined(enumType, value) end
---@param enumType System.Type
---@return System.Type
function m.GetUnderlyingType(enumType) end
---@param enumType System.Type
---@param value string
---@return object
function m.Parse(enumType, value) end
---@param target object
---@return int
function m:CompareTo(target) end
---@return string
function m:ToString() end
---@param enumType System.Type
---@param value byte
---@return object
function m.ToObject(enumType, value) end
---@param obj object
---@return bool
function m:Equals(obj) end
---@return int
function m:GetHashCode() end
---@param enumType System.Type
---@param value object
---@param format string
---@return string
function m.Format(enumType, value, format) end
return m