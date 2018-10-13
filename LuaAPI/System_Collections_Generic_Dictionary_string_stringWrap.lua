---@class System.Collections.Generic.Dictionary<string,string> : object
---@field public Count int
---@field public Item string
---@field public Comparer System.Collections.Generic.IEqualityComparer
---@field public Keys table
---@field public Values table
local m = {}
---@param key string
---@param value string
function m:Add(key, value) end
function m:Clear() end
---@param key string
---@return bool
function m:ContainsKey(key) end
---@param value string
---@return bool
function m:ContainsValue(value) end
---@param info System.Runtime.Serialization.SerializationInfo
---@param context System.Runtime.Serialization.StreamingContext
function m:GetObjectData(info, context) end
---@param sender object
function m:OnDeserialization(sender) end
---@param key string
---@return bool
function m:Remove(key) end
---@param key string
---@param value string
---@return bool
function m:TryGetValue(key, value) end
---@return System.Collections.Generic.Dictionary
function m:GetEnumerator() end
return m