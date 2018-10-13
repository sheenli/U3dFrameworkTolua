---@class ResMgr : EventDispatcherNode
---@field public Intstance ResMgr
---@field public threadSafe bool
local m = {}
function m:Awake() end
---@param assetName string
---@return UnityEngine.Object
function m:GetAsset(assetName) end
---@param abName string
---@return int
function m:GetABRefCount(abName) end
---@param assetName string
---@param callBack System.Action
function m:GetAssetAsync(assetName, callBack) end
---@param assteName string
---@param callBaclk System.Action
function m:LoadAsset(assteName, callBaclk) end
---@param ab UnityEngine.AssetBundle
---@param assteName string
---@param type System.Type
---@param callBack System.Action
---@return System.Collections.IEnumerator
function m:LoadAssetAsync(ab, assteName, type, callBack) end
---@param audioClip UnityEngine.AudioClip
---@param callBack System.Action
---@return System.Collections.IEnumerator
function m:PrLoadAudioClip(audioClip, callBack) end
---@param abName string
---@param callBack System.Action
function m:LoadAll(abName, callBack) end
---@param groupName string
function m:LoadGroup(groupName) end
function m:LogAll() end
function m:PushRes() end
---@param assetName string
---@param immediate bool
function m:UnLoadAsset(assetName, immediate) end
---@param forced bool
function m:UnLoadAll(forced) end
---@return System.Collections.IEnumerator
function m:GC() end
function m:TryAddFairyGuiRes() end
---@param assetName string
---@param all bool
function m:TryUnLoadFariryGUIRes(assetName, all) end
function m:Init() end
return m