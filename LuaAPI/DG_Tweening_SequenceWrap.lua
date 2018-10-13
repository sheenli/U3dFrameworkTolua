---@class DG.Tweening.Sequence : DG.Tweening.Tween
---@field public timeScale float
---@field public isBackwards bool
---@field public id object
---@field public target object
---@field public easeOvershootOrAmplitude float
---@field public easePeriod float
local m = {}
---@param t DG.Tweening.Tween
---@return DG.Tweening.Sequence
function m:Append(t) end
---@param t DG.Tweening.Tween
---@return DG.Tweening.Sequence
function m:Prepend(t) end
---@param t DG.Tweening.Tween
---@return DG.Tweening.Sequence
function m:Join(t) end
---@param atPosition float
---@param t DG.Tweening.Tween
---@return DG.Tweening.Sequence
function m:Insert(atPosition, t) end
---@param interval float
---@return DG.Tweening.Sequence
function m:AppendInterval(interval) end
---@param interval float
---@return DG.Tweening.Sequence
function m:PrependInterval(interval) end
---@param callback DG.Tweening.TweenCallback
---@return DG.Tweening.Sequence
function m:AppendCallback(callback) end
---@param callback DG.Tweening.TweenCallback
---@return DG.Tweening.Sequence
function m:PrependCallback(callback) end
---@param atPosition float
---@param callback DG.Tweening.TweenCallback
---@return DG.Tweening.Sequence
function m:InsertCallback(atPosition, callback) end
return m