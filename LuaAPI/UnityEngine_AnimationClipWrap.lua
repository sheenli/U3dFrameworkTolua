---@class UnityEngine.AnimationClip : UnityEngine.Object
---@field public length float
---@field public frameRate float
---@field public wrapMode UnityEngine.WrapMode
---@field public localBounds UnityEngine.Bounds
---@field public legacy bool
---@field public humanMotion bool
---@field public events table
local m = {}
---@param go UnityEngine.GameObject
---@param time float
function m:SampleAnimation(go, time) end
---@param relativePath string
---@param type System.Type
---@param propertyName string
---@param curve UnityEngine.AnimationCurve
function m:SetCurve(relativePath, type, propertyName, curve) end
function m:EnsureQuaternionContinuity() end
function m:ClearCurves() end
---@param evt UnityEngine.AnimationEvent
function m:AddEvent(evt) end
return m