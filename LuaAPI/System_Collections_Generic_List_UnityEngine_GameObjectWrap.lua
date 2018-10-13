---@class System.Collections.Generic.List<UnityEngine.GameObject> : object
---@field public Capacity int
---@field public Count int
---@field public Item UnityEngine.GameObject
local m = {}
---@param item UnityEngine.GameObject
function m:Add(item) end
---@param collection System.Collections.Generic.IEnumerable
function m:AddRange(collection) end
---@return table
function m:AsReadOnly() end
---@param item UnityEngine.GameObject
---@return int
function m:BinarySearch(item) end
function m:Clear() end
---@param item UnityEngine.GameObject
---@return bool
function m:Contains(item) end
---@param array table
function m:CopyTo(array) end
---@param match System.Predicate
---@return bool
function m:Exists(match) end
---@param match System.Predicate
---@return UnityEngine.GameObject
function m:Find(match) end
---@param match System.Predicate
---@return table
function m:FindAll(match) end
---@param match System.Predicate
---@return int
function m:FindIndex(match) end
---@param match System.Predicate
---@return UnityEngine.GameObject
function m:FindLast(match) end
---@param match System.Predicate
---@return int
function m:FindLastIndex(match) end
---@param action System.Action
function m:ForEach(action) end
---@return System.Collections.Generic.List
function m:GetEnumerator() end
---@param index int
---@param count int
---@return table
function m:GetRange(index, count) end
---@param item UnityEngine.GameObject
---@return int
function m:IndexOf(item) end
---@param index int
---@param item UnityEngine.GameObject
function m:Insert(index, item) end
---@param index int
---@param collection System.Collections.Generic.IEnumerable
function m:InsertRange(index, collection) end
---@param item UnityEngine.GameObject
---@return int
function m:LastIndexOf(item) end
---@param item UnityEngine.GameObject
---@return bool
function m:Remove(item) end
---@param match System.Predicate
---@return int
function m:RemoveAll(match) end
---@param index int
function m:RemoveAt(index) end
---@param index int
---@param count int
function m:RemoveRange(index, count) end
function m:Reverse() end
function m:Sort() end
---@return table
function m:ToArray() end
function m:TrimExcess() end
---@param match System.Predicate
---@return bool
function m:TrueForAll(match) end
return m