---@class DG.Tweening.DOTween : object
---@field public logBehaviour DG.Tweening.LogBehaviour
---@field public Version string
---@field public useSafeMode bool
---@field public showUnityEditorReport bool
---@field public timeScale float
---@field public useSmoothDeltaTime bool
---@field public drawGizmos bool
---@field public defaultUpdateType DG.Tweening.UpdateType
---@field public defaultTimeScaleIndependent bool
---@field public defaultAutoPlay DG.Tweening.AutoPlay
---@field public defaultAutoKill bool
---@field public defaultLoopType DG.Tweening.LoopType
---@field public defaultRecyclable bool
---@field public defaultEaseType DG.Tweening.Ease
---@field public defaultEaseOvershootOrAmplitude float
---@field public defaultEasePeriod float
local m = {}
---@param recycleAllByDefault System.Nullable
---@param useSafeMode System.Nullable
---@param logBehaviour System.Nullable
---@return DG.Tweening.IDOTweenInit
function m.Init(recycleAllByDefault, useSafeMode, logBehaviour) end
---@param tweenersCapacity int
---@param sequencesCapacity int
function m.SetTweensCapacity(tweenersCapacity, sequencesCapacity) end
---@param destroy bool
function m.Clear(destroy) end
function m.ClearCachedTweens() end
---@return int
function m.Validate() end
---@param getter DG.Tweening.Core.DOGetter
---@param setter DG.Tweening.Core.DOSetter
---@param endValue float
---@param duration float
---@return DG.Tweening.Core.TweenerCore
function m.To(getter, setter, endValue, duration) end
---@param getter DG.Tweening.Core.DOGetter
---@param setter DG.Tweening.Core.DOSetter
---@param endValue float
---@param duration float
---@param axisConstraint DG.Tweening.AxisConstraint
---@return DG.Tweening.Core.TweenerCore
function m.ToAxis(getter, setter, endValue, duration, axisConstraint) end
---@param getter DG.Tweening.Core.DOGetter
---@param setter DG.Tweening.Core.DOSetter
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m.ToAlpha(getter, setter, endValue, duration) end
---@param getter DG.Tweening.Core.DOGetter
---@param setter DG.Tweening.Core.DOSetter
---@param direction UnityEngine.Vector3
---@param duration float
---@param vibrato int
---@param elasticity float
---@return DG.Tweening.Core.TweenerCore
function m.Punch(getter, setter, direction, duration, vibrato, elasticity) end
---@param getter DG.Tweening.Core.DOGetter
---@param setter DG.Tweening.Core.DOSetter
---@param duration float
---@param strength float
---@param vibrato int
---@param randomness float
---@param ignoreZAxis bool
---@param fadeOut bool
---@return DG.Tweening.Core.TweenerCore
function m.Shake(getter, setter, duration, strength, vibrato, randomness, ignoreZAxis, fadeOut) end
---@param getter DG.Tweening.Core.DOGetter
---@param setter DG.Tweening.Core.DOSetter
---@param endValues table
---@param durations table
---@return DG.Tweening.Core.TweenerCore
function m.ToArray(getter, setter, endValues, durations) end
---@return DG.Tweening.Sequence
function m.Sequence() end
---@param withCallbacks bool
---@return int
function m.CompleteAll(withCallbacks) end
---@param targetOrId object
---@param withCallbacks bool
---@return int
function m.Complete(targetOrId, withCallbacks) end
---@return int
function m.FlipAll() end
---@param targetOrId object
---@return int
function m.Flip(targetOrId) end
---@param to float
---@param andPlay bool
---@return int
function m.GotoAll(to, andPlay) end
---@param targetOrId object
---@param to float
---@param andPlay bool
---@return int
function m.Goto(targetOrId, to, andPlay) end
---@param complete bool
---@return int
function m.KillAll(complete) end
---@param targetOrId object
---@param complete bool
---@return int
function m.Kill(targetOrId, complete) end
---@return int
function m.PauseAll() end
---@param targetOrId object
---@return int
function m.Pause(targetOrId) end
---@return int
function m.PlayAll() end
---@param targetOrId object
---@return int
function m.Play(targetOrId) end
---@return int
function m.PlayBackwardsAll() end
---@param targetOrId object
---@return int
function m.PlayBackwards(targetOrId) end
---@return int
function m.PlayForwardAll() end
---@param targetOrId object
---@return int
function m.PlayForward(targetOrId) end
---@param includeDelay bool
---@return int
function m.RestartAll(includeDelay) end
---@param targetOrId object
---@param includeDelay bool
---@return int
function m.Restart(targetOrId, includeDelay) end
---@param includeDelay bool
---@return int
function m.RewindAll(includeDelay) end
---@param targetOrId object
---@param includeDelay bool
---@return int
function m.Rewind(targetOrId, includeDelay) end
---@return int
function m.SmoothRewindAll() end
---@param targetOrId object
---@return int
function m.SmoothRewind(targetOrId) end
---@return int
function m.TogglePauseAll() end
---@param targetOrId object
---@return int
function m.TogglePause(targetOrId) end
---@param targetOrId object
---@return bool
function m.IsTweening(targetOrId) end
---@return int
function m.TotalPlayingTweens() end
---@return table
function m.PlayingTweens() end
---@return table
function m.PausedTweens() end
---@param id object
---@param playingOnly bool
---@return table
function m.TweensById(id, playingOnly) end
---@param target object
---@param playingOnly bool
---@return table
function m.TweensByTarget(target, playingOnly) end
return m