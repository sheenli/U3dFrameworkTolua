---@class UnityEngine.Physics : object
---@field public gravity UnityEngine.Vector3
---@field public defaultContactOffset float
---@field public bounceThreshold float
---@field public defaultSolverIterations int
---@field public defaultSolverVelocityIterations int
---@field public sleepThreshold float
---@field public queriesHitTriggers bool
---@field public queriesHitBackfaces bool
---@field public IgnoreRaycastLayer int
---@field public DefaultRaycastLayers int
---@field public AllLayers int
local m = {}
---@param origin UnityEngine.Vector3
---@param direction UnityEngine.Vector3
---@param maxDistance float
---@param layerMask int
---@return bool
function m.Raycast(origin, direction, maxDistance, layerMask) end
---@param ray UnityEngine.Ray
---@param maxDistance float
---@param layerMask int
---@return table
function m.RaycastAll(ray, maxDistance, layerMask) end
---@param ray UnityEngine.Ray
---@param results table
---@param maxDistance float
---@param layerMask int
---@return int
function m.RaycastNonAlloc(ray, results, maxDistance, layerMask) end
---@param start UnityEngine.Vector3
---@param end UnityEngine.Vector3
---@param layerMask int
---@return bool
function m.Linecast(start, end, layerMask) end
---@param position UnityEngine.Vector3
---@param radius float
---@param layerMask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return table
function m.OverlapSphere(position, radius, layerMask, queryTriggerInteraction) end
---@param position UnityEngine.Vector3
---@param radius float
---@param results table
---@param layerMask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return int
function m.OverlapSphereNonAlloc(position, radius, results, layerMask, queryTriggerInteraction) end
---@param point0 UnityEngine.Vector3
---@param point1 UnityEngine.Vector3
---@param radius float
---@param layerMask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return table
function m.OverlapCapsule(point0, point1, radius, layerMask, queryTriggerInteraction) end
---@param point0 UnityEngine.Vector3
---@param point1 UnityEngine.Vector3
---@param radius float
---@param results table
---@param layerMask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return int
function m.OverlapCapsuleNonAlloc(point0, point1, radius, results, layerMask, queryTriggerInteraction) end
---@param point1 UnityEngine.Vector3
---@param point2 UnityEngine.Vector3
---@param radius float
---@param direction UnityEngine.Vector3
---@param maxDistance float
---@param layerMask int
---@return bool
function m.CapsuleCast(point1, point2, radius, direction, maxDistance, layerMask) end
---@param origin UnityEngine.Vector3
---@param radius float
---@param direction UnityEngine.Vector3
---@param hitInfo UnityEngine.RaycastHit
---@param maxDistance float
---@param layerMask int
---@return bool
function m.SphereCast(origin, radius, direction, hitInfo, maxDistance, layerMask) end
---@param point1 UnityEngine.Vector3
---@param point2 UnityEngine.Vector3
---@param radius float
---@param direction UnityEngine.Vector3
---@param maxDistance float
---@param layermask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return table
function m.CapsuleCastAll(point1, point2, radius, direction, maxDistance, layermask, queryTriggerInteraction) end
---@param point1 UnityEngine.Vector3
---@param point2 UnityEngine.Vector3
---@param radius float
---@param direction UnityEngine.Vector3
---@param results table
---@param maxDistance float
---@param layermask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return int
function m.CapsuleCastNonAlloc(point1, point2, radius, direction, results, maxDistance, layermask, queryTriggerInteraction) end
---@param origin UnityEngine.Vector3
---@param radius float
---@param direction UnityEngine.Vector3
---@param maxDistance float
---@param layerMask int
---@return table
function m.SphereCastAll(origin, radius, direction, maxDistance, layerMask) end
---@param origin UnityEngine.Vector3
---@param radius float
---@param direction UnityEngine.Vector3
---@param results table
---@param maxDistance float
---@param layerMask int
---@return int
function m.SphereCastNonAlloc(origin, radius, direction, results, maxDistance, layerMask) end
---@param position UnityEngine.Vector3
---@param radius float
---@param layerMask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return bool
function m.CheckSphere(position, radius, layerMask, queryTriggerInteraction) end
---@param start UnityEngine.Vector3
---@param end UnityEngine.Vector3
---@param radius float
---@param layermask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return bool
function m.CheckCapsule(start, end, radius, layermask, queryTriggerInteraction) end
---@param center UnityEngine.Vector3
---@param halfExtents UnityEngine.Vector3
---@param orientation UnityEngine.Quaternion
---@param layermask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return bool
function m.CheckBox(center, halfExtents, orientation, layermask, queryTriggerInteraction) end
---@param center UnityEngine.Vector3
---@param halfExtents UnityEngine.Vector3
---@param orientation UnityEngine.Quaternion
---@param layerMask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return table
function m.OverlapBox(center, halfExtents, orientation, layerMask, queryTriggerInteraction) end
---@param center UnityEngine.Vector3
---@param halfExtents UnityEngine.Vector3
---@param results table
---@param orientation UnityEngine.Quaternion
---@param layerMask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return int
function m.OverlapBoxNonAlloc(center, halfExtents, results, orientation, layerMask, queryTriggerInteraction) end
---@param center UnityEngine.Vector3
---@param halfExtents UnityEngine.Vector3
---@param direction UnityEngine.Vector3
---@param orientation UnityEngine.Quaternion
---@param maxDistance float
---@param layermask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return table
function m.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, layermask, queryTriggerInteraction) end
---@param center UnityEngine.Vector3
---@param halfExtents UnityEngine.Vector3
---@param direction UnityEngine.Vector3
---@param results table
---@param orientation UnityEngine.Quaternion
---@param maxDistance float
---@param layermask int
---@param queryTriggerInteraction UnityEngine.QueryTriggerInteraction
---@return int
function m.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, maxDistance, layermask, queryTriggerInteraction) end
---@param center UnityEngine.Vector3
---@param halfExtents UnityEngine.Vector3
---@param direction UnityEngine.Vector3
---@param orientation UnityEngine.Quaternion
---@param maxDistance float
---@param layerMask int
---@return bool
function m.BoxCast(center, halfExtents, direction, orientation, maxDistance, layerMask) end
---@param collider1 UnityEngine.Collider
---@param collider2 UnityEngine.Collider
---@param ignore bool
function m.IgnoreCollision(collider1, collider2, ignore) end
---@param layer1 int
---@param layer2 int
---@param ignore bool
function m.IgnoreLayerCollision(layer1, layer2, ignore) end
---@param layer1 int
---@param layer2 int
---@return bool
function m.GetIgnoreLayerCollision(layer1, layer2) end
---@param colliderA UnityEngine.Collider
---@param positionA UnityEngine.Vector3
---@param rotationA UnityEngine.Quaternion
---@param colliderB UnityEngine.Collider
---@param positionB UnityEngine.Vector3
---@param rotationB UnityEngine.Quaternion
---@param direction UnityEngine.Vector3
---@param distance float
---@return bool
function m.ComputePenetration(colliderA, positionA, rotationA, colliderB, positionB, rotationB, direction, distance) end
---@param point UnityEngine.Vector3
---@param collider UnityEngine.Collider
---@param position UnityEngine.Vector3
---@param rotation UnityEngine.Quaternion
---@return UnityEngine.Vector3
function m.ClosestPoint(point, collider, position, rotation) end
return m