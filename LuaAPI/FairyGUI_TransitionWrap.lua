---@class FairyGUI.Transition : object
---@field public name string
---@field public autoPlay bool
---@field public playing bool
---@field public timeScale float
---@field public autoPlayRepeat int
---@field public autoPlayDelay float
---@field public invalidateBatchingEveryFrame bool
local m = {}
function m:Play() end
function m:PlayReverse() end
---@param value int
function m:ChangeRepeat(value) end
function m:Stop() end
function m:Dispose() end
---@param label string
---@param aParams table
function m:SetValue(label, aParams) end
---@param label string
---@param callback FairyGUI.TransitionHook
function m:SetHook(label, callback) end
function m:ClearHooks() end
---@param label string
---@param newTarget FairyGUI.GObject
function m:SetTarget(label, newTarget) end
---@param label string
---@param value float
function m:SetDuration(label, value) end
---@param xml FairyGUI.Utils.XML
function m:Setup(xml) end
return m