---@class GameCfgMgr : object
---@field public Instance GameCfgMgr
---@field public localGameCfg LocalGameCfgData
---@field public resCfg ResCfg
local m = {}
---@param assetName string
---@return ResInfoData
function m:GetResInfo(assetName) end
---@param groupName string
---@return ResGroupCfg
function m:GetGroupInfo(groupName) end
---@param groupData ResGroupCfg
---@return table
function m:GetGroupABs(groupData) end
function m:Init() end
function m:LoadResCfg() end
return m