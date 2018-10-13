---@class DG.Tweening.Core.TweenerCore<double,double,DG.Tweening.Plugins.Options.NoOptions> : DG.Tweening.Tweener
---@field public startValue double
---@field public endValue double
---@field public changeValue double
---@field public plugOptions DG.Tweening.Plugins.Options.NoOptions
---@field public getter DG.Tweening.Core.DOGetter
---@field public setter DG.Tweening.Core.DOSetter
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
---@param snapStartValue bool
---@return DG.Tweening.Tweener
function m:ChangeEndValue(newEndValue, snapStartValue) end
---@param newStartValue object
---@param newEndValue object
---@param newDuration float
---@return DG.Tweening.Tweener
function m:ChangeValues(newStartValue, newEndValue, newDuration) end
return m