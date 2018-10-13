---@class UnityEngine.Screen : object
---@field public resolutions table
---@field public currentResolution UnityEngine.Resolution
---@field public width int
---@field public height int
---@field public dpi float
---@field public fullScreen bool
---@field public autorotateToPortrait bool
---@field public autorotateToPortraitUpsideDown bool
---@field public autorotateToLandscapeLeft bool
---@field public autorotateToLandscapeRight bool
---@field public orientation UnityEngine.ScreenOrientation
---@field public sleepTimeout int
local m = {}
---@param width int
---@param height int
---@param fullscreen bool
---@param preferredRefreshRate int
function m.SetResolution(width, height, fullscreen, preferredRefreshRate) end
return m