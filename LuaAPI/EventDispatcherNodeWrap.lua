---@class EventDispatcherNode : LogicNode
---@field public threadSafe bool
local m = {}
---@param type string
---@param _listener EventDispatcherNode.EventListenerDele
---@return bool
function m:HasListener(type, _listener) end
---@param type int
---@param _listener EventDispatcherNode.EventListenerDele
---@param _priority int
---@param _dispatchOnce bool
function m:AttachListener(type, _listener, _priority, _dispatchOnce) end
---@param type int
---@param _listener EventDispatcherNode.EventListenerDele
function m:DetachListener(type, _listener) end
function m:OnUpdate() end
---@param key int
function m:QueueEvent(key) end
---@param key string
---@param data object
function m:QueueEventLua(key, data) end
---@param key string
function m:QueueEventNow(key) end
return m