---@class FairyGUI.InputTextField : FairyGUI.RichTextField
---@field public onFocusIn FairyGUI.EventListener
---@field public onFocusOut FairyGUI.EventListener
---@field public onChanged FairyGUI.EventListener
---@field public onSubmit FairyGUI.EventListener
---@field public maxLength int
---@field public keyboardInput bool
---@field public keyboardType int
---@field public editable bool
---@field public hideInput bool
---@field public text string
---@field public textFormat FairyGUI.TextFormat
---@field public restrict string
---@field public caretPosition int
---@field public promptText string
---@field public displayAsPassword bool
---@field public onCopy FairyGUI.InputTextField.CopyHandler
---@field public onPaste FairyGUI.InputTextField.PasteHandler
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
---@param start int
---@param length int
function m:SetSelection(start, length) end
---@param value string
function m:ReplaceSelection(value) end
---@param value string
function m:ReplaceText(value) end
---@param context FairyGUI.UpdateContext
function m:Update(context) end
function m:Dispose() end
return m