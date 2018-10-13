---@class ABInfo : object
---@field public fileName string
---@field public sha1 string
---@field public length long
---@field public assets table
local m = {}
---@return string
function m:ToString() end
---@param remotely ABInfo
---@return bool
function m:IsSame(remotely) end
return m