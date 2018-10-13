---@class SceneLoadTask : object
---@field public IsFailure bool
---@field public IsFinished bool
local m = {}
---@return string
function m:FailureInfo() end
function m:OnExecute() end
function m:Rest() end
---@return string
function m:TaskName() end
return m