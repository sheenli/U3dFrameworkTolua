---@class HotUpdateRessMgr : object
---@field public Instance HotUpdateRessMgr
---@field public verInfo VerInfo
---@field public downLoadInfo RemotelyVersionInfo.RemotelyInfo
local m = {}
---@param callBack System.Action
function m:Init(callBack) end
---@param callBack System.Action
function m:RefreshLocalVerInfo(callBack) end
---@param localVer VerInfo
---@param callBack System.Action
function m:GetDownList(localVer, callBack) end
---@param localVer VerInfo
---@param callBack System.Action
function m:GetDecompressionTaskList(localVer, callBack) end
---@param info HotUpdateRessMgr.DecompressionOrDownInfo
---@param saveVer bool
function m:Save(info, saveVer) end
return m