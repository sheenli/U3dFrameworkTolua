---@class UnityEngine.RenderTexture : UnityEngine.Texture
---@field public width int
---@field public height int
---@field public depth int
---@field public isPowerOfTwo bool
---@field public sRGB bool
---@field public format UnityEngine.RenderTextureFormat
---@field public useMipMap bool
---@field public autoGenerateMips bool
---@field public dimension UnityEngine.Rendering.TextureDimension
---@field public volumeDepth int
---@field public antiAliasing int
---@field public enableRandomWrite bool
---@field public colorBuffer UnityEngine.RenderBuffer
---@field public depthBuffer UnityEngine.RenderBuffer
---@field public active UnityEngine.RenderTexture
local m = {}
---@param width int
---@param height int
---@param depthBuffer int
---@param format UnityEngine.RenderTextureFormat
---@param readWrite UnityEngine.RenderTextureReadWrite
---@param antiAliasing int
---@return UnityEngine.RenderTexture
function m.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing) end
---@param temp UnityEngine.RenderTexture
function m.ReleaseTemporary(temp) end
---@return bool
function m:Create() end
function m:Release() end
---@return bool
function m:IsCreated() end
function m:DiscardContents() end
function m:MarkRestoreExpected() end
function m:GenerateMips() end
---@return System.IntPtr
function m:GetNativeDepthBufferPtr() end
---@param propertyName string
function m:SetGlobalShaderProperty(propertyName) end
---@param rt UnityEngine.RenderTexture
---@return bool
function m.SupportsStencil(rt) end
return m