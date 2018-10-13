---@class AppConst : object
---@field public AppExternalDataPath string
---@field public AppExternalDataPathUrl string
---@field public SourceResPathUrl string
---@field public LuaPath string
---@field public ABPath string
---@field public TestAccount object
---@field public ExtName string
---@field public DebugMode bool
---@field public serverIP string
---@field public LuaExtNames table
local m = {}
---@param rootPath string
---@param fileName string
---@return string
function m.ToValidFileName(rootPath, fileName) end
return m