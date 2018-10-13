---@class UnityEngine.Rigidbody : UnityEngine.Component
---@field public velocity UnityEngine.Vector3
---@field public angularVelocity UnityEngine.Vector3
---@field public drag float
---@field public angularDrag float
---@field public mass float
---@field public useGravity bool
---@field public maxDepenetrationVelocity float
---@field public isKinematic bool
---@field public freezeRotation bool
---@field public constraints UnityEngine.RigidbodyConstraints
---@field public collisionDetectionMode UnityEngine.CollisionDetectionMode
---@field public centerOfMass UnityEngine.Vector3
---@field public worldCenterOfMass UnityEngine.Vector3
---@field public inertiaTensorRotation UnityEngine.Quaternion
---@field public inertiaTensor UnityEngine.Vector3
---@field public detectCollisions bool
---@field public position UnityEngine.Vector3
---@field public rotation UnityEngine.Quaternion
---@field public interpolation UnityEngine.RigidbodyInterpolation
---@field public solverIterations int
---@field public solverVelocityIterations int
---@field public sleepThreshold float
---@field public maxAngularVelocity float
local m = {}
---@param endValue UnityEngine.Vector3
---@param duration float
---@param snapping bool
---@return DG.Tweening.Tweener
function m:DOMove(endValue, duration, snapping) end
---@param endValue float
---@param duration float
---@param snapping bool
---@return DG.Tweening.Tweener
function m:DOMoveX(endValue, duration, snapping) end
---@param endValue float
---@param duration float
---@param snapping bool
---@return DG.Tweening.Tweener
function m:DOMoveY(endValue, duration, snapping) end
---@param endValue float
---@param duration float
---@param snapping bool
---@return DG.Tweening.Tweener
function m:DOMoveZ(endValue, duration, snapping) end
---@param endValue UnityEngine.Vector3
---@param duration float
---@param mode DG.Tweening.RotateMode
---@return DG.Tweening.Tweener
function m:DORotate(endValue, duration, mode) end
---@param towards UnityEngine.Vector3
---@param duration float
---@param axisConstraint DG.Tweening.AxisConstraint
---@param up System.Nullable
---@return DG.Tweening.Tweener
function m:DOLookAt(towards, duration, axisConstraint, up) end
---@param endValue UnityEngine.Vector3
---@param jumpPower float
---@param numJumps int
---@param duration float
---@param snapping bool
---@return DG.Tweening.Sequence
function m:DOJump(endValue, jumpPower, numJumps, duration, snapping) end
---@param density float
function m:SetDensity(density) end
---@param force UnityEngine.Vector3
---@param mode UnityEngine.ForceMode
function m:AddForce(force, mode) end
---@param force UnityEngine.Vector3
---@param mode UnityEngine.ForceMode
function m:AddRelativeForce(force, mode) end
---@param torque UnityEngine.Vector3
---@param mode UnityEngine.ForceMode
function m:AddTorque(torque, mode) end
---@param torque UnityEngine.Vector3
---@param mode UnityEngine.ForceMode
function m:AddRelativeTorque(torque, mode) end
---@param force UnityEngine.Vector3
---@param position UnityEngine.Vector3
---@param mode UnityEngine.ForceMode
function m:AddForceAtPosition(force, position, mode) end
---@param explosionForce float
---@param explosionPosition UnityEngine.Vector3
---@param explosionRadius float
---@param upwardsModifier float
---@param mode UnityEngine.ForceMode
function m:AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, mode) end
---@param position UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:ClosestPointOnBounds(position) end
---@param relativePoint UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:GetRelativePointVelocity(relativePoint) end
---@param worldPoint UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:GetPointVelocity(worldPoint) end
---@param position UnityEngine.Vector3
function m:MovePosition(position) end
---@param rot UnityEngine.Quaternion
function m:MoveRotation(rot) end
function m:Sleep() end
---@return bool
function m:IsSleeping() end
function m:WakeUp() end
function m:ResetCenterOfMass() end
function m:ResetInertiaTensor() end
---@param direction UnityEngine.Vector3
---@param hitInfo UnityEngine.RaycastHit
---@param maxDistance float
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return bool
function m:SweepTest(direction, hitInfo, maxDistance, queryTriggerInteraction) end
---@param direction UnityEngine.Vector3
---@param maxDistance float
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return table
function m:SweepTestAll(direction, maxDistance, queryTriggerInteraction) end
return m