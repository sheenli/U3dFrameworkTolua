---@class System.String : object
---@field public Length int
---@field public Empty string
local m = {}
---@param a string
---@param b string
---@return bool
function m.Equals(a, b) end
---@return object
function m:Clone() end
---@return System.TypeCode
function m:GetTypeCode() end
---@param sourceIndex int
---@param destination table
---@param destinationIndex int
---@param count int
function m:CopyTo(sourceIndex, destination, destinationIndex, count) end
---@return table
function m:ToCharArray() end
---@param separator table
---@return table
function m:Split(separator) end
---@param startIndex int
---@return string
function m:Substring(startIndex) end
---@return string
function m:Trim() end
---@param trimChars table
---@return string
function m:TrimStart(trimChars) end
---@param trimChars table
---@return string
function m:TrimEnd(trimChars) end
---@param strA string
---@param strB string
---@return int
function m.Compare(strA, strB) end
---@param value object
---@return int
function m:CompareTo(value) end
---@param strA string
---@param strB string
---@return int
function m.CompareOrdinal(strA, strB) end
---@param value string
---@return bool
function m:EndsWith(value) end
---@param anyOf table
---@return int
function m:IndexOfAny(anyOf) end
---@param value string
---@param comparisonType System.StringComparison
---@return int
function m:IndexOf(value, comparisonType) end
---@param value string
---@param comparisonType System.StringComparison
---@return int
function m:LastIndexOf(value, comparisonType) end
---@param anyOf table
---@return int
function m:LastIndexOfAny(anyOf) end
---@param value string
---@return bool
function m:Contains(value) end
---@param value string
---@return bool
function m.IsNullOrEmpty(value) end
---@return string
function m:Normalize() end
---@return bool
function m:IsNormalized() end
---@param startIndex int
---@return string
function m:Remove(startIndex) end
---@param totalWidth int
---@return string
function m:PadLeft(totalWidth) end
---@param totalWidth int
---@return string
function m:PadRight(totalWidth) end
---@param value string
---@return bool
function m:StartsWith(value) end
---@param oldChar char
---@param newChar char
---@return string
function m:Replace(oldChar, newChar) end
---@return string
function m:ToLower() end
---@return string
function m:ToLowerInvariant() end
---@return string
function m:ToUpper() end
---@return string
function m:ToUpperInvariant() end
---@return string
function m:ToString() end
---@param format string
---@param arg0 object
---@return string
function m.Format(format, arg0) end
---@param str string
---@return string
function m.Copy(str) end
---@param arg0 object
---@return string
function m.Concat(arg0) end
---@param startIndex int
---@param value string
---@return string
function m:Insert(startIndex, value) end
---@param str string
---@return string
function m.Intern(str) end
---@param str string
---@return string
function m.IsInterned(str) end
---@param separator string
---@param value table
---@return string
function m.Join(separator, value) end
---@return System.CharEnumerator
function m:GetEnumerator() end
---@return int
function m:GetHashCode() end
---@param a string
---@param b string
---@return bool
function m.op_Equality(a, b) end
---@param a string
---@param b string
---@return bool
function m.op_Inequality(a, b) end
return m