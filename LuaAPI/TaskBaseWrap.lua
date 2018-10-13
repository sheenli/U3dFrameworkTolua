---@class TaskBase : object
---@field public IsValid bool
---@field public currentTaskName string
---@field public progress float
---@field public allStaskCount int
---@field public isFinished bool
---@field public taskItemFinished System.Action
---@field public mFinished System.Action
---@field public mFailure System.Action
local m = {}
function m:Stop() end
---@param task ITask
function m:AddTask(task) end
function m:OnExecute() end
return m