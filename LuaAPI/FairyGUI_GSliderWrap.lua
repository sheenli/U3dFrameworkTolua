---@class FairyGUI.GSlider : FairyGUI.GComponent
---@field public onChanged FairyGUI.EventListener
---@field public onGripTouchEnd FairyGUI.EventListener
---@field public titleType FairyGUI.ProgressTitleType
---@field public max double
---@field public value double
---@field public changeOnClick bool
---@field public canDrag bool
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
---@param cxml FairyGUI.Utils.XML
function m:ConstructFromXML(cxml) end
---@param cxml FairyGUI.Utils.XML
function m:Setup_AfterAdd(cxml) end
return m