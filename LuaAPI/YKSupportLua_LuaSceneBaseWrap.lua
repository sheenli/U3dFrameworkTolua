---@class YKSupportLua.LuaSceneBase : SceneBase
---@field public eventMgr EventListenerMgr
---@field public isLoadingShowWait bool
---@field public sceneTask AsynTask
---@field public parallelTask AsynTask
local m = {}
---@param peerTable LuaInterface.LuaTable
function m:ConnectLua(peerTable) end
function m:OnDestroy() end
return m