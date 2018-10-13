---@class LogicNode : UnityEngine.MonoBehaviour
---@field public NodePriority int
---@field public IsValid bool
---@field public Parent LogicNode
---@field public threadSafe bool
local m = {}
---@param node LogicNode
function m:AttachNode(node) end
---@param node LogicNode
function m:DetachNode(node) end
---@param logic ILogic
---@return bool
function m:HasLogic(logic) end
---@param logic ILogic
function m:AttachLogic(logic) end
---@param logic ILogic
function m:DetachLogic(logic) end
function m:OnDestroy() end
function m:OnDisable() end
function m:OnEnable() end
---@param pauseStatus bool
function m:OnApplicationPause(pauseStatus) end
function m:OnApplicationQuit() end
function m:OnFixedUpdate() end
function m:OnLateUpdate() end
function m:OnUpdate() end
return m