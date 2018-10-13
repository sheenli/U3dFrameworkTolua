---@class UnityEngine.CharacterController : UnityEngine.Collider
---@field public isGrounded bool
---@field public velocity UnityEngine.Vector3
---@field public collisionFlags UnityEngine.CollisionFlags
---@field public radius float
---@field public height float
---@field public center UnityEngine.Vector3
---@field public slopeLimit float
---@field public stepOffset float
---@field public skinWidth float
---@field public minMoveDistance float
---@field public detectCollisions bool
---@field public enableOverlapRecovery bool
local m = {}
---@param speed UnityEngine.Vector3
---@return bool
function m:SimpleMove(speed) end
---@param motion UnityEngine.Vector3
---@return UnityEngine.CollisionFlags
function m:Move(motion) end
return m