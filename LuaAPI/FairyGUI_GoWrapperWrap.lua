---@class FairyGUI.GoWrapper : FairyGUI.DisplayObject
---@field public wrapTarget UnityEngine.GameObject
---@field public renderingOrder int
---@field public layer int
---@field public supportStencil bool
---@field public name string
---@field public onPaint FairyGUI.EventCallback0
---@field public gOwner FairyGUI.GObject
---@field public id uint
local m = {}
---@param target UnityEngine.GameObject
---@param cloneMaterial bool
function m:setWrapTarget(target, cloneMaterial) end
function m:CacheRenderers() end
---@param context FairyGUI.UpdateContext
function m:Update(context) end
function m:Dispose() end
return m