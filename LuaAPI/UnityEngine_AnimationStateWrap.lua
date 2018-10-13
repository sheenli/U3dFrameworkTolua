---@class UnityEngine.AnimationState : UnityEngine.TrackedReference
---@field public enabled bool
---@field public weight float
---@field public wrapMode UnityEngine.WrapMode
---@field public time float
---@field public normalizedTime float
---@field public speed float
---@field public normalizedSpeed float
---@field public length float
---@field public layer int
---@field public clip UnityEngine.AnimationClip
---@field public name string
---@field public blendMode UnityEngine.AnimationBlendMode
local m = {}
---@param mix UnityEngine.Transform
---@param recursive bool
function m:AddMixingTransform(mix, recursive) end
---@param mix UnityEngine.Transform
function m:RemoveMixingTransform(mix) end
return m