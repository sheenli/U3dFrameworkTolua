---@class TweenUtils
local m = {}
---@param start float
---@param end float
---@param duration float
---@param OnUpdate LuaInterface.LuaFunction
---@return DG.Tweening.Tweener
function m.TweenFloat(start, end, duration, OnUpdate) end
---@param start UnityEngine.Vector2
---@param end UnityEngine.Vector2
---@param duration float
---@param OnUpdate LuaInterface.LuaFunction
---@return DG.Tweening.Tweener
function m.TweenVector2(start, end, duration, OnUpdate) end
---@param start UnityEngine.Vector3
---@param end UnityEngine.Vector3
---@param duration float
---@param OnUpdate LuaInterface.LuaFunction
---@return DG.Tweening.Tweener
function m.TweenVector3(start, end, duration, OnUpdate) end
---@param tweener DG.Tweening.Tweener
---@param ease DG.Tweening.Ease
function m.SetEase(tweener, ease) end
---@param tweener DG.Tweening.Tweener
---@param func LuaInterface.LuaFunction
function m.OnComplete(tweener, func) end
---@param tweener DG.Tweening.Tweener
---@param delay float
function m.SetDelay(tweener, delay) end
---@param tweener DG.Tweening.Tweener
---@param loops int
function m.SetLoops(tweener, loops) end
---@param tweener DG.Tweening.Tweener
---@param target object
function m.SetTarget(tweener, target) end
return m