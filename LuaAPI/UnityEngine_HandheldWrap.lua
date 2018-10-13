---@class UnityEngine.Handheld : object
local m = {}
---@param path string
---@param bgColor UnityEngine.Color
---@param controlMode UnityEngine.FullScreenMovieControlMode
---@param scalingMode UnityEngine.FullScreenMovieScalingMode
---@return bool
function m.PlayFullScreenMovie(path, bgColor, controlMode, scalingMode) end
function m.Vibrate() end
---@return int
function m.GetActivityIndicatorStyle() end
function m.StartActivityIndicator() end
function m.StopActivityIndicator() end
function m.ClearShaderCache() end
return m