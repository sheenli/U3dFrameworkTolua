---@class UnityEngine.SkinnedMeshRenderer : UnityEngine.Renderer
---@field public bones table
---@field public rootBone UnityEngine.Transform
---@field public quality UnityEngine.SkinQuality
---@field public sharedMesh UnityEngine.Mesh
---@field public updateWhenOffscreen bool
---@field public skinnedMotionVectors bool
---@field public localBounds UnityEngine.Bounds
local m = {}
---@param mesh UnityEngine.Mesh
function m:BakeMesh(mesh) end
---@param index int
---@return float
function m:GetBlendShapeWeight(index) end
---@param index int
---@param value float
function m:SetBlendShapeWeight(index, value) end
return m