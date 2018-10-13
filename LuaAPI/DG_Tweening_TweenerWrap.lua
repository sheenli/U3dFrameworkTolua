---@class DG.Tweening.Tweener : DG.Tweening.Tween
---@field public timeScale float
---@field public isBackwards bool
---@field public id object
---@field public target object
---@field public easeOvershootOrAmplitude float
---@field public easePeriod float
local m = {}
---@param newStartValue object
---@param newDuration float
---@return DG.Tweening.Tweener
function m:ChangeStartValue(newStartValue, newDuration) end
---@param newEndValue object
---@param newDuration float
---@param snapStartValue bool
---@return DG.Tweening.Tweener
function m:ChangeEndValue(newEndValue, newDuration, snapStartValue) end
---@param newStartValue object
---@param newEndValue object
---@param newDuration float
---@return DG.Tweening.Tweener
function m:ChangeValues(newStartValue, newEndValue, newDuration) end
return m