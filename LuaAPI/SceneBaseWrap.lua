---@class SceneBase : object
---@field public SceneName string
---@field public GroupName string
---@field public eventMgr EventListenerMgr
---@field public isLoadingShowWait bool
---@field public sceneTask AsynTask
---@field public parallelTask AsynTask
local m = {}
---@param task ITask
function m:AddTask(task) end
---@param task ITask
function m:AddParallelTask(task) end
---@param gropName string
function m:AddLoadGrop(gropName) end
function m:Loaded() end
---@param param object
function m:Enter(param) end
function m:OnFinished() end
function m:Leave() end
function m:ResStartTask() end
---@param dis EventDispatcherNode
---@param type string
---@param _priority int
---@param _dispatchOnce bool
function m:AddListener(dis, type, _priority, _dispatchOnce) end
function m:OnDestroy() end
return m