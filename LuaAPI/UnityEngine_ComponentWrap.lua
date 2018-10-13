---@class UnityEngine.Component : UnityEngine.Object
---@field public transform UnityEngine.Transform
---@field public gameObject UnityEngine.GameObject
---@field public tag string
local m = {}
---@param withCallbacks bool
---@return int
function m:DOComplete(withCallbacks) end
---@param complete bool
---@return int
function m:DOKill(complete) end
---@return int
function m:DOFlip() end
---@param to float
---@param andPlay bool
---@return int
function m:DOGoto(to, andPlay) end
---@return int
function m:DOPause() end
---@return int
function m:DOPlay() end
---@return int
function m:DOPlayBackwards() end
---@return int
function m:DOPlayForward() end
---@param includeDelay bool
---@return int
function m:DORestart(includeDelay) end
---@param includeDelay bool
---@return int
function m:DORewind(includeDelay) end
---@return int
function m:DOSmoothRewind() end
---@return int
function m:DOTogglePause() end
---@param type System.Type
---@return UnityEngine.Component
function m:GetComponent(type) end
---@param t System.Type
---@param includeInactive bool
---@return UnityEngine.Component
function m:GetComponentInChildren(t, includeInactive) end
---@param t System.Type
---@return table
function m:GetComponentsInChildren(t) end
---@param t System.Type
---@return UnityEngine.Component
function m:GetComponentInParent(t) end
---@param t System.Type
---@return table
function m:GetComponentsInParent(t) end
---@param type System.Type
---@return table
function m:GetComponents(type) end
---@param tag string
---@return bool
function m:CompareTag(tag) end
---@param methodName string
---@param value object
---@param options UnityEngine.SendMessageOptions
function m:SendMessageUpwards(methodName, value, options) end
---@param methodName string
---@param value object
---@param options UnityEngine.SendMessageOptions
function m:SendMessage(methodName, value, options) end
---@param methodName string
---@param parameter object
---@param options UnityEngine.SendMessageOptions
function m:BroadcastMessage(methodName, parameter, options) end
return m