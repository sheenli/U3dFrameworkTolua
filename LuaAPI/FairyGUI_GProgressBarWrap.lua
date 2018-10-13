---@class FairyGUI.GProgressBar : FairyGUI.GComponent
---@field public titleType FairyGUI.ProgressTitleType
---@field public max double
---@field public value double
---@field public reverse bool
---@field public name string
---@field public data object
---@field public sourceWidth int
---@field public sourceHeight int
---@field public initWidth int
---@field public initHeight int
---@field public minWidth int
---@field public maxWidth int
---@field public minHeight int
---@field public maxHeight int
---@field public dragBounds System.Nullable
---@field public packageItem FairyGUI.PackageItem
local m = {}
---@param value double
---@param duration float
---@return DG.Tweening.Tweener
function m:TweenValue(value, duration) end
---@param newValue double
function m:Update(newValue) end
---@param cxml FairyGUI.Utils.XML
function m:ConstructFromXML(cxml) end
---@param cxml FairyGUI.Utils.XML
function m:Setup_AfterAdd(cxml) end
function m:Dispose() end
return m