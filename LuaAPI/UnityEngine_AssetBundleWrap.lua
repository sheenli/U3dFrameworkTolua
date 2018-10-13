---@class UnityEngine.AssetBundle : UnityEngine.Object
---@field public mainAsset UnityEngine.Object
---@field public isStreamedSceneAssetBundle bool
local m = {}
---@param path string
---@param crc uint
---@param offset ulong
---@return UnityEngine.AssetBundleCreateRequest
function m.LoadFromFileAsync(path, crc, offset) end
---@param path string
---@param crc uint
---@param offset ulong
---@return UnityEngine.AssetBundle
function m.LoadFromFile(path, crc, offset) end
---@param binary table
---@param crc uint
---@return UnityEngine.AssetBundleCreateRequest
function m.LoadFromMemoryAsync(binary, crc) end
---@param binary table
---@param crc uint
---@return UnityEngine.AssetBundle
function m.LoadFromMemory(binary, crc) end
---@param name string
---@return bool
function m:Contains(name) end
---@param name string
---@return UnityEngine.Object
function m:LoadAsset(name) end
---@param name string
---@return UnityEngine.AssetBundleRequest
function m:LoadAssetAsync(name) end
---@param name string
---@return table
function m:LoadAssetWithSubAssets(name) end
---@param name string
---@return UnityEngine.AssetBundleRequest
function m:LoadAssetWithSubAssetsAsync(name) end
---@return table
function m:LoadAllAssets() end
---@return UnityEngine.AssetBundleRequest
function m:LoadAllAssetsAsync() end
---@param unloadAllLoadedObjects bool
function m:Unload(unloadAllLoadedObjects) end
---@return table
function m:GetAllAssetNames() end
---@return table
function m:GetAllScenePaths() end
return m