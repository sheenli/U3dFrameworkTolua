---@class UnityEngine.QualitySettings : UnityEngine.Object
---@field public names table
---@field public pixelLightCount int
---@field public shadows UnityEngine.ShadowQuality
---@field public shadowProjection UnityEngine.ShadowProjection
---@field public shadowCascades int
---@field public shadowDistance float
---@field public shadowResolution UnityEngine.ShadowResolution
---@field public shadowNearPlaneOffset float
---@field public shadowCascade2Split float
---@field public shadowCascade4Split UnityEngine.Vector3
---@field public masterTextureLimit int
---@field public anisotropicFiltering UnityEngine.AnisotropicFiltering
---@field public lodBias float
---@field public maximumLODLevel int
---@field public particleRaycastBudget int
---@field public softParticles bool
---@field public softVegetation bool
---@field public realtimeReflectionProbes bool
---@field public billboardsFaceCameraPosition bool
---@field public maxQueuedFrames int
---@field public vSyncCount int
---@field public antiAliasing int
---@field public desiredColorSpace UnityEngine.ColorSpace
---@field public activeColorSpace UnityEngine.ColorSpace
---@field public blendWeights UnityEngine.BlendWeights
---@field public asyncUploadTimeSlice int
---@field public asyncUploadBufferSize int
local m = {}
---@return int
function m.GetQualityLevel() end
---@param index int
---@param applyExpensiveChanges bool
function m.SetQualityLevel(index, applyExpensiveChanges) end
---@param applyExpensiveChanges bool
function m.IncreaseLevel(applyExpensiveChanges) end
---@param applyExpensiveChanges bool
function m.DecreaseLevel(applyExpensiveChanges) end
return m