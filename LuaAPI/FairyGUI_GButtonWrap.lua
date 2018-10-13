---@class FairyGUI.GButton : FairyGUI.GComponent
---@field public pageOption FairyGUI.PageOption
---@field public onChanged FairyGUI.EventListener
---@field public icon string
---@field public title string
---@field public text string
---@field public selectedIcon string
---@field public selectedTitle string
---@field public titleColor UnityEngine.Color
---@field public titleFontSize int
---@field public selected bool
---@field public mode FairyGUI.ButtonMode
---@field public relatedController FairyGUI.Controller
---@field public sound UnityEngine.AudioClip
---@field public soundVolumeScale float
---@field public changeStateOnClick bool
---@field public linkedPopup FairyGUI.GObject
---@field public UP string
---@field public DOWN string
---@field public OVER string
---@field public SELECTED_OVER string
---@field public DISABLED string
---@field public SELECTED_DISABLED string
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
---@param downEffect bool
function m:FireClick(downEffect) end
---@param c FairyGUI.Controller
function m:HandleControllerChanged(c) end
---@param cxml FairyGUI.Utils.XML
function m:ConstructFromXML(cxml) end
---@param cxml FairyGUI.Utils.XML
function m:Setup_AfterAdd(cxml) end
return m