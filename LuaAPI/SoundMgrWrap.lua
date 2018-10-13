---@class SoundMgr : UnityEngine.MonoBehaviour
---@field public Volume float
---@field public IsOn bool
---@field public Instance SoundMgr
---@field public IsValid bool
local m = {}
function m:Awake() end
function m:OnDestroy() end
---@param audioClipName string
---@param isLoop bool
---@param listener System.Action
function m:PlayAudioClipAsync(audioClipName, isLoop, listener) end
---@param clip UnityEngine.AudioClip
---@param isLoop bool
---@return UnityEngine.AudioSource
function m:PlayAudioClip(clip, isLoop) end
function m:Release() end
return m