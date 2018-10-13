---@class FairyGUI.PlayState : object
---@field public reachEnding bool
---@field public reversed bool
---@field public repeatedCount int
---@field public currrentFrame int
---@field public ignoreTimeScale bool
local m = {}
---@param mc FairyGUI.MovieClip
---@param context FairyGUI.UpdateContext
function m:Update(mc, context) end
function m:Rewind() end
function m:Reset() end
---@param src FairyGUI.PlayState
function m:Copy(src) end
return m