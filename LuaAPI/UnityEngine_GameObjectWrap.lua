---@class UnityEngine.GameObject : UnityEngine.Object
---@field public transform UnityEngine.Transform
---@field public layer int
---@field public activeSelf bool
---@field public activeInHierarchy bool
---@field public isStatic bool
---@field public tag string
---@field public scene UnityEngine.SceneManagement.Scene
---@field public gameObject UnityEngine.GameObject
local m = {}
---@param type UnityEngine.PrimitiveType
---@return UnityEngine.GameObject
function m.CreatePrimitive(type) end
---@param type System.Type
---@return UnityEngine.Component
function m:GetComponent(type) end
---@param type System.Type
---@param includeInactive bool
---@return UnityEngine.Component
function m:GetComponentInChildren(type, includeInactive) end
---@param type System.Type
---@return UnityEngine.Component
function m:GetComponentInParent(type) end
---@param type System.Type
---@return table
function m:GetComponents(type) end
---@param type System.Type
---@return table
function m:GetComponentsInChildren(type) end
---@param type System.Type
---@return table
function m:GetComponentsInParent(type) end
---@param value bool
function m:SetActive(value) end
---@param tag string
---@return bool
function m:CompareTag(tag) end
---@param tag string
---@return UnityEngine.GameObject
function m.FindGameObjectWithTag(tag) end
---@param tag string
---@return UnityEngine.GameObject
function m.FindWithTag(tag) end
---@param tag string
---@return table
function m.FindGameObjectsWithTag(tag) end
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
---@param componentType System.Type
---@return UnityEngine.Component
function m:AddComponent(componentType) end
---@param name string
---@return UnityEngine.GameObject
function m.Find(name) end
return m