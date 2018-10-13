---@class UnityEngine.AudioSource : UnityEngine.Behaviour
---@field public volume float
---@field public pitch float
---@field public time float
---@field public timeSamples int
---@field public clip UnityEngine.AudioClip
---@field public outputAudioMixerGroup UnityEngine.Audio.AudioMixerGroup
---@field public isPlaying bool
---@field public isVirtual bool
---@field public loop bool
---@field public ignoreListenerVolume bool
---@field public playOnAwake bool
---@field public ignoreListenerPause bool
---@field public velocityUpdateMode UnityEngine.AudioVelocityUpdateMode
---@field public panStereo float
---@field public spatialBlend float
---@field public spatialize bool
---@field public spatializePostEffects bool
---@field public reverbZoneMix float
---@field public bypassEffects bool
---@field public bypassListenerEffects bool
---@field public bypassReverbZones bool
---@field public dopplerLevel float
---@field public spread float
---@field public priority int
---@field public mute bool
---@field public minDistance float
---@field public maxDistance float
---@field public rolloffMode UnityEngine.AudioRolloffMode
local m = {}
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:DOFade(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:DOPitch(endValue, duration) end
---@param delay ulong
function m:Play(delay) end
---@param delay float
function m:PlayDelayed(delay) end
---@param time double
function m:PlayScheduled(time) end
---@param time double
function m:SetScheduledStartTime(time) end
---@param time double
function m:SetScheduledEndTime(time) end
function m:Stop() end
function m:Pause() end
function m:UnPause() end
---@param clip UnityEngine.AudioClip
---@param volumeScale float
function m:PlayOneShot(clip, volumeScale) end
---@param clip UnityEngine.AudioClip
---@param position UnityEngine.Vector3
function m.PlayClipAtPoint(clip, position) end
---@param type UnityEngine.AudioSourceCurveType
---@param curve UnityEngine.AnimationCurve
function m:SetCustomCurve(type, curve) end
---@param type UnityEngine.AudioSourceCurveType
---@return UnityEngine.AnimationCurve
function m:GetCustomCurve(type) end
---@param samples table
---@param channel int
function m:GetOutputData(samples, channel) end
---@param samples table
---@param channel int
---@param window UnityEngine.FFTWindow
function m:GetSpectrumData(samples, channel, window) end
---@param index int
---@param value float
---@return bool
function m:SetSpatializerFloat(index, value) end
---@param index int
---@param value float
---@return bool
function m:GetSpatializerFloat(index, value) end
return m