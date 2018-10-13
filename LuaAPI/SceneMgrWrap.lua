---@class SceneMgr : EventDispatcherNode
---@field public Instance SceneMgr
---@field public ChangeSceneFinished string
---@field public OpenLoadUI System.Action
---@field public CloseLoadUI System.Action
---@field public oldScenes table
---@field public currentScene SceneBase
---@field public threadSafe bool
local m = {}
function m:Awake() end
---@param scneneType string
---@param t System.Type
---@param func System.Func
---@param param object
function m:GotoScene(scneneType, t, func, param) end
function m:OnDestroy() end
---@param sceneName string
---@return SceneBase
function m:GetScnene(sceneName) end
---@param data EventData
function m:OnHanderEvent(data) end
return m