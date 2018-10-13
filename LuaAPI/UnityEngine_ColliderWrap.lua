---@class UnityEngine.Collider : UnityEngine.Component
---@field public enabled bool
---@field public attachedRigidbody UnityEngine.Rigidbody
---@field public isTrigger bool
---@field public contactOffset float
---@field public material UnityEngine.PhysicMaterial
---@field public sharedMaterial UnityEngine.PhysicMaterial
---@field public bounds UnityEngine.Bounds
local m = {}
---@param position UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:ClosestPointOnBounds(position) end
---@param position UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:ClosestPoint(position) end
---@param ray UnityEngine.Ray
---@param hitInfo UnityEngine.RaycastHit
---@param maxDistance float
---@return bool
function m:Raycast(ray, hitInfo, maxDistance) end
return m