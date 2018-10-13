---@class DG.Tweening.Core.TweenerCore<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.Options.VectorOptions> : DG.Tweening.Tweener
---@field public startValue UnityEngine.Vector3
---@field public endValue UnityEngine.Vector3
---@field public changeValue UnityEngine.Vector3
---@field public plugOptions DG.Tweening.Plugins.Options.VectorOptions
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