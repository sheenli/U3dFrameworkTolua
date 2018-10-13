---@class UnityEngine.Animation : UnityEngine.Behaviour
---@field public clip UnityEngine.AnimationClip
---@field public playAutomatically bool
---@field public wrapMode UnityEngine.WrapMode
---@field public isPlaying bool
---@field public Item UnityEngine.AnimationState
---@field public animatePhysics bool
---@field public cullingType UnityEngine.AnimationCullingType
---@field public localBounds UnityEngine.Bounds
local m = {}
function m:Stop() end
---@param name string
function m:Rewind(name) end
function m:Sample() end
---@param name string
---@return bool
function m:IsPlaying(name) end
---@return bool
function m:Play() end
---@param animation string
---@param fadeLength float
---@param mode UnityEngine.PlayMode
function m:CrossFade(animation, fadeLength, mode) end
---@param animation string
---@param targetWeight float
---@param fadeLength float
function m:Blend(animation, targetWeight, fadeLength) end
---@param animation string
---@param fadeLength float
---@param queue UnityEngine.QueueMode
---@param mode UnityEngine.PlayMode
---@return UnityEngine.AnimationState
function m:CrossFadeQueued(animation, fadeLength, queue, mode) end
---@param animation string
---@param queue UnityEngine.QueueMode
---@param mode UnityEngine.PlayMode
---@return UnityEngine.AnimationState
function m:PlayQueued(animation, queue, mode) end
---@param clip UnityEngine.AnimationClip
---@param newName string
function m:AddClip(clip, newName) end
---@param clip UnityEngine.AnimationClip
function m:RemoveClip(clip) end
---@return int
function m:GetClipCount() end
---@param layer int
function m:SyncLayer(layer) end
---@return System.Collections.IEnumerator
function m:GetEnumerator() end
---@param name string
---@return UnityEngine.AnimationClip
function m:GetClip(name) end
return m