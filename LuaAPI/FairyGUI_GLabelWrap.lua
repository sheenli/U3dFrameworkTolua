---@class FairyGUI.GLabel : FairyGUI.GComponent
---@field public icon string
---@field public title string
---@field public text string
---@field public editable bool
---@field public titleColor UnityEngine.Color
---@field public titleFontSize int
---@field public color UnityEngine.Color
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