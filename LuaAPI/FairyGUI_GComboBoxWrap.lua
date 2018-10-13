---@class FairyGUI.GComboBox : FairyGUI.GComponent
---@field public onChanged FairyGUI.EventListener
---@field public icon string
---@field public title string
---@field public text string
---@field public titleColor UnityEngine.Color
---@field public items table
---@field public icons table
---@field public values table
---@field public selectedIndex int
---@field public selectionController FairyGUI.Controller
---@field public value string
---@field public popupDirection string
---@field public visibleItemCount int
---@field public dropdown FairyGUI.GComponent
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
---@param c FairyGUI.Controller
function m:HandleControllerChanged(c) end
function m:Dispose() end
---@param cxml FairyGUI.Utils.XML
function m:ConstructFromXML(cxml) end
---@param cxml FairyGUI.Utils.XML
function m:Setup_AfterAdd(cxml) end
function m:UpdateDropdownList() end
return m