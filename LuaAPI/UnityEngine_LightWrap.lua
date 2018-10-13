---@class UnityEngine.Light : UnityEngine.Behaviour
---@field public type UnityEngine.LightType
---@field public color UnityEngine.Color
---@field public colorTemperature float
---@field public intensity float
---@field public bounceIntensity float
---@field public shadows UnityEngine.LightShadows
---@field public shadowStrength float
---@field public shadowResolution UnityEngine.Rendering.LightShadowResolution
---@field public shadowCustomResolution int
---@field public shadowBias float
---@field public shadowNormalBias float
---@field public shadowNearPlane float
---@field public range float
---@field public spotAngle float
---@field public cookieSize float
---@field public cookie UnityEngine.Texture
---@field public flare UnityEngine.Flare
---@field public renderMode UnityEngine.LightRenderMode
---@field public alreadyLightmapped bool
---@field public isBaked bool
---@field public cullingMask int
---@field public commandBufferCount int
local m = {}
---@param endValue UnityEngine.Color
---@param duration float
---@return DG.Tweening.Tweener
function m:DOColor(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:DOIntensity(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:DOShadowStrength(endValue, duration) end
---@param endValue UnityEngine.Color
---@param duration float
---@return DG.Tweening.Tweener
function m:DOBlendableColor(endValue, duration) end
---@param evt UnityEngine.Rendering.LightEvent
---@param buffer UnityEngine.Rendering.CommandBuffer
function m:AddCommandBuffer(evt, buffer) end
---@param evt UnityEngine.Rendering.LightEvent
---@param buffer UnityEngine.Rendering.CommandBuffer
function m:RemoveCommandBuffer(evt, buffer) end
---@param evt UnityEngine.Rendering.LightEvent
function m:RemoveCommandBuffers(evt) end
function m:RemoveAllCommandBuffers() end
---@param evt UnityEngine.Rendering.LightEvent
---@return table
function m:GetCommandBuffers(evt) end
---@param type UnityEngine.LightType
---@param layer int
---@return table
function m.GetLights(type, layer) end
return m