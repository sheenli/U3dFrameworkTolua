---@class UnityEngine.Resources : object
local m = {}
---@param type System.Type
---@return table
function m.FindObjectsOfTypeAll(type) end
---@param path string
---@return UnityEngine.Object
function m.Load(path) end
---@param path string
---@return UnityEngine.ResourceRequest
function m.LoadAsync(path) end
---@param path string
---@param systemTypeInstance System.Type
---@return table
function m.LoadAll(path, systemTypeInstance) end
---@param type System.Type
---@param path string
---@return UnityEngine.Object
function m.GetBuiltinResource(type, path) end
---@param assetToUnload UnityEngine.Object
function m.UnloadAsset(assetToUnload) end
---@return UnityEngine.AsyncOperation
function m.UnloadUnusedAssets() end
return m