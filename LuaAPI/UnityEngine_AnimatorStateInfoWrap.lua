---@class UnityEngine.AnimatorStateInfo
---@field public fullPathHash int
---@field public shortNameHash int
---@field public normalizedTime float
---@field public length float
---@field public speed float
---@field public speedMultiplier float
---@field public tagHash int
---@field public loop bool
local m = {}
---@param name string
---@return bool
function m:IsName(name) end
---@param tag string
---@return bool
function m:IsTag(tag) end
return m