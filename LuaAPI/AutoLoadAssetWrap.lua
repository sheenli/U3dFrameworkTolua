---@class AutoLoadAsset : object
---@field public fileName string
---@field public loaded int
---@field public isKeepInMemory bool
local m = {}
---@param callback System.Action
function m:Load(callback) end
return m