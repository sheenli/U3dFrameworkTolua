---@class FairyGUI.CaptureCamera : UnityEngine.MonoBehaviour
---@field public layer int
---@field public hiddenLayer int
---@field public cachedTransform UnityEngine.Transform
---@field public cachedCamera UnityEngine.Camera
---@field public Name string
---@field public LayerName string
---@field public HiddenLayerName string
local m = {}
function m.CheckMain() end
---@param width int
---@param height int
---@param stencilSupport bool
---@return UnityEngine.RenderTexture
function m.CreateRenderTexture(width, height, stencilSupport) end
---@param target FairyGUI.DisplayObject
---@param texture UnityEngine.RenderTexture
---@param offset UnityEngine.Vector2
function m.Capture(target, texture, offset) end
return m