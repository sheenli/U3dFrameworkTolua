---@class FairyGUI.Window : FairyGUI.GComponent
---@field public bringToFontOnClick bool
---@field public contentPane FairyGUI.GComponent
---@field public frame FairyGUI.GComponent
---@field public closeButton FairyGUI.GObject
---@field public dragArea FairyGUI.GObject
---@field public contentArea FairyGUI.GObject
---@field public modalWaitingPane FairyGUI.GObject
---@field public isShowing bool
---@field public isTop bool
---@field public modal bool
---@field public modalWaiting bool
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
---@param source FairyGUI.IUISource
function m:AddUISource(source) end
function m:Show() end
---@param r FairyGUI.GRoot
function m:ShowOn(r) end
function m:Hide() end
function m:HideImmediately() end
---@param r FairyGUI.GRoot
---@param restraint bool
function m:CenterOn(r, restraint) end
function m:ToggleStatus() end
function m:BringToFront() end
function m:ShowModalWait() end
---@return bool
function m:CloseModalWait() end
function m:Init() end
function m:Dispose() end
return m