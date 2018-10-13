---@class LuaMgr : object
---@field public Instance LuaMgr
local m = {}
function m:Init() end
function m:OnDestroy() end
---@param path string
---@param ab UnityEngine.AssetBundle
function m:AddFile(path, ab) end
---@param fileName string
---@return LuaInterface.LuaByteBuffer
function m:GetLuaFileLuaByteBuffer(fileName) end
---@param fileName string
---@return table
function m:GetLuaFileByte(fileName) end
function m:StartGame() end
function m:GC() end
return m