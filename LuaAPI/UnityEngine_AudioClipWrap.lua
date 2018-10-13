---@class UnityEngine.AudioClip : UnityEngine.Object
---@field public length float
---@field public samples int
---@field public channels int
---@field public frequency int
---@field public loadType UnityEngine.AudioClipLoadType
---@field public preloadAudioData bool
---@field public loadState UnityEngine.AudioDataLoadState
---@field public loadInBackground bool
local m = {}
---@return bool
function m:LoadAudioData() end
---@return bool
function m:UnloadAudioData() end
---@param data table
---@param offsetSamples int
---@return bool
function m:GetData(data, offsetSamples) end
---@param data table
---@param offsetSamples int
---@return bool
function m:SetData(data, offsetSamples) end
---@param name string
---@param lengthSamples int
---@param channels int
---@param frequency int
---@param stream bool
---@return UnityEngine.AudioClip
function m.Create(name, lengthSamples, channels, frequency, stream) end
return m