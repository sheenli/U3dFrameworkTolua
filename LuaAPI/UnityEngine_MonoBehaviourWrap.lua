---@class UnityEngine.MonoBehaviour : UnityEngine.Behaviour
---@field public useGUILayout bool
local m = {}
---@param methodName string
---@param time float
function m:Invoke(methodName, time) end
---@param methodName string
---@param time float
---@param repeatRate float
function m:InvokeRepeating(methodName, time, repeatRate) end
function m:CancelInvoke() end
---@param methodName string
---@return bool
function m:IsInvoking(methodName) end
---@param routine System.Collections.IEnumerator
---@return UnityEngine.Coroutine
function m:StartCoroutine(routine) end
---@param methodName string
function m:StopCoroutine(methodName) end
function m:StopAllCoroutines() end
---@param message object
function m.print(message) end
return m