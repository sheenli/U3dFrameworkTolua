---@class FairyGUI.GRoot : FairyGUI.GComponent
---@field public contentScaleFactor float
---@field public inst FairyGUI.GRoot
---@field public modalLayer FairyGUI.GGraph
---@field public hasModalWindow bool
---@field public modalWaiting bool
---@field public touchTarget FairyGUI.GObject
---@field public hasAnyPopup bool
---@field public focus FairyGUI.GObject
---@field public soundVolume float
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
---@param designResolutionX int
---@param designResolutionY int
function m:SetContentScaleFactor(designResolutionX, designResolutionY) end
function m:ApplyContentScaleFactor() end
---@param win FairyGUI.Window
function m:ShowWindow(win) end
---@param win FairyGUI.Window
function m:HideWindow(win) end
---@param win FairyGUI.Window
function m:HideWindowImmediately(win) end
---@param win FairyGUI.Window
function m:BringToFront(win) end
function m:ShowModalWait() end
function m:CloseModalWait() end
function m:CloseAllExceptModals() end
function m:CloseAllWindows() end
---@return FairyGUI.Window
function m:GetTopWindow() end
---@param obj FairyGUI.DisplayObject
---@return FairyGUI.GObject
function m:DisplayObjectToGObject(obj) end
---@param popup FairyGUI.GObject
function m:ShowPopup(popup) end
---@param popup FairyGUI.GObject
---@param target FairyGUI.GObject
---@param downward object
---@return UnityEngine.Vector2
function m:GetPoupPosition(popup, target, downward) end
---@param popup FairyGUI.GObject
function m:TogglePopup(popup) end
function m:HidePopup() end
---@param msg string
function m:ShowTooltips(msg) end
---@param tooltipWin FairyGUI.GObject
function m:ShowTooltipsWin(tooltipWin) end
function m:HideTooltips() end
function m:EnableSound() end
function m:DisableSound() end
---@param clip UnityEngine.AudioClip
---@param volumeScale float
function m:PlayOneShotSound(clip, volumeScale) end
return m