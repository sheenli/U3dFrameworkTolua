---@class UnityEngine.Texture : UnityEngine.Object
---@field public masterTextureLimit int
---@field public anisotropicFiltering UnityEngine.AnisotropicFiltering
---@field public width int
---@field public height int
---@field public dimension UnityEngine.Rendering.TextureDimension
---@field public filterMode UnityEngine.FilterMode
---@field public anisoLevel int
---@field public wrapMode UnityEngine.TextureWrapMode
---@field public mipMapBias float
---@field public texelSize UnityEngine.Vector2
local m = {}
---@param forcedMin int
---@param globalMax int
function m.SetGlobalAnisotropicFilteringLimits(forcedMin, globalMax) end
---@return System.IntPtr
function m:GetNativeTexturePtr() end
return m