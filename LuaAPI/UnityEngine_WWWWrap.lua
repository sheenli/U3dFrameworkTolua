---@class UnityEngine.WWW : object
---@field public responseHeaders table
---@field public text string
---@field public bytes table
---@field public size int
---@field public error string
---@field public texture UnityEngine.Texture2D
---@field public textureNonReadable UnityEngine.Texture2D
---@field public isDone bool
---@field public progress float
---@field public uploadProgress float
---@field public bytesDownloaded int
---@field public url string
---@field public assetBundle UnityEngine.AssetBundle
---@field public threadPriority UnityEngine.ThreadPriority
local m = {}
function m:Dispose() end
---@param url string
---@param postData table
---@param iHeaders table
function m:InitWWW(url, postData, iHeaders) end
---@param s string
---@return string
function m.EscapeURL(s) end
---@param s string
---@return string
function m.UnEscapeURL(s) end
---@param tex UnityEngine.Texture2D
function m:LoadImageIntoTexture(tex) end
---@param url string
---@param version int
---@return UnityEngine.WWW
function m.LoadFromCacheOrDownload(url, version) end
return m