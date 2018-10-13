---@class VerInfo : object
---@field public verName string
---@field public ver string
---@field public files table
local m = {}
---@return string
function m:ToString() end
---@param _ver string
---@return int
function m:CompareTo(_ver) end
---@param fileName string
---@return ABInfo
function m:GetABDownLoadInfo(fileName) end
---@param ab ABInfo
function m:Save(ab) end
---@param local VerInfo
---@param remoteVer VerInfo
---@return ComparisonInfo
function m.ComparisonVer(local, remoteVer) end
return m