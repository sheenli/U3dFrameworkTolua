---@class UnityEngine.Renderer : UnityEngine.Component
---@field public isPartOfStaticBatch bool
---@field public worldToLocalMatrix UnityEngine.Matrix4x4
---@field public localToWorldMatrix UnityEngine.Matrix4x4
---@field public enabled bool
---@field public shadowCastingMode UnityEngine.Rendering.ShadowCastingMode
---@field public receiveShadows bool
---@field public material UnityEngine.Material
---@field public sharedMaterial UnityEngine.Material
---@field public materials table
---@field public sharedMaterials table
---@field public bounds UnityEngine.Bounds
---@field public lightmapIndex int
---@field public realtimeLightmapIndex int
---@field public lightmapScaleOffset UnityEngine.Vector4
---@field public motionVectorGenerationMode UnityEngine.MotionVectorGenerationMode
---@field public realtimeLightmapScaleOffset UnityEngine.Vector4
---@field public isVisible bool
---@field public lightProbeUsage UnityEngine.Rendering.LightProbeUsage
---@field public lightProbeProxyVolumeOverride UnityEngine.GameObject
---@field public probeAnchor UnityEngine.Transform
---@field public reflectionProbeUsage UnityEngine.Rendering.ReflectionProbeUsage
---@field public sortingLayerName string
---@field public sortingLayerID int
---@field public sortingOrder int
local m = {}
---@param properties UnityEngine.MaterialPropertyBlock
function m:SetPropertyBlock(properties) end
---@param dest UnityEngine.MaterialPropertyBlock
function m:GetPropertyBlock(dest) end
---@param result table
function m:GetClosestReflectionProbes(result) end
return m