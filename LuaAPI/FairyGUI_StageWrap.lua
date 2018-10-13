---@class FairyGUI.Stage : FairyGUI.Container
---@field public stageHeight int
---@field public stageWidth int
---@field public soundVolume float
---@field public onStageResized FairyGUI.EventListener
---@field public inst FairyGUI.Stage
---@field public touchScreen bool
---@field public keyboardInput bool
---@field public isTouchOnUI bool
---@field public touchTarget FairyGUI.DisplayObject
---@field public focus FairyGUI.DisplayObject
---@field public touchPosition UnityEngine.Vector2
---@field public touchCount int
---@field public keyboard FairyGUI.IKeyboard
---@field public renderMode UnityEngine.RenderMode
---@field public renderCamera UnityEngine.Camera
---@field public opaque bool
---@field public clipSoftness System.Nullable
---@field public hitArea FairyGUI.IHitTest
---@field public touchChildren bool
---@field public onUpdate FairyGUI.EventCallback0
---@field public reversedMask bool
---@field public name string
---@field public onPaint FairyGUI.EventCallback0
---@field public gOwner FairyGUI.GObject
---@field public id uint
local m = {}
function m.Instantiate() end
---@param touchId int
---@return UnityEngine.Vector2
function m:GetTouchPosition(touchId) end
---@param result table
---@return table
function m:GetAllTouch(result) end
function m:ResetInputState() end
---@param touchId int
function m:CancelClick(touchId) end
function m:EnableSound() end
function m:DisableSound() end
---@param clip UnityEngine.AudioClip
---@param volumeScale float
function m:PlayOneShotSound(clip, volumeScale) end
---@param text string
---@param autocorrection bool
---@param multiline bool
---@param secure bool
---@param alert bool
---@param textPlaceholder string
---@param keyboardType int
---@param hideInput bool
function m:OpenKeyboard(text, autocorrection, multiline, secure, alert, textPlaceholder, keyboardType, hideInput) end
function m:CloseKeyboard() end
---@param value string
function m:InputString(value) end
---@param screenPos UnityEngine.Vector2
---@param buttonDown bool
function m:SetCustomInput(screenPos, buttonDown) end
---@param target FairyGUI.Container
function m:ApplyPanelOrder(target) end
---@param panelSortingOrder int
function m:SortWorldSpacePanelsByZOrder(panelSortingOrder) end
---@param texture FairyGUI.NTexture
function m:MonitorTexture(texture) end
---@param touchId int
---@param target FairyGUI.EventDispatcher
function m:AddTouchMonitor(touchId, target) end
---@param target FairyGUI.EventDispatcher
function m:RemoveTouchMonitor(target) end
return m