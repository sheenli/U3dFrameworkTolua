---@class LocalGameCfgData : UnityEngine.ScriptableObject
---@field public OSSCollectionPath string
---@field public OSSResRootPath string
---@field public RemotelyResUrl string
---@field public CollectionIDVerFileName string
---@field public openLogKEY string
---@field public version string
---@field public isPublic bool
---@field public chanelType int
---@field public collectionID int
---@field public openLog bool
---@field public OSSROOT string
local m = {}
---@param flag bool
function m:SetPublic(flag) end
---@param isopen bool
function m:SetLog(isopen) end
function m:Init() end
return m