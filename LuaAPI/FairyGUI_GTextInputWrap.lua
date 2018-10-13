---@class FairyGUI.GTextInput : FairyGUI.GTextField
---@field public onFocusIn FairyGUI.EventListener
---@field public onFocusOut FairyGUI.EventListener
---@field public onChanged FairyGUI.EventListener
---@field public onSubmit FairyGUI.EventListener
---@field public inputTextField FairyGUI.InputTextField
---@field public editable bool
---@field public hideInput bool
---@field public maxLength int
---@field public restrict string
---@field public displayAsPassword bool
---@field public caretPosition int
---@field public promptText string
---@field public keyboardInput bool
---@field public keyboardType int
---@field public emojies table
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
---@param start int
---@param length int
function m:SetSelection(start, length) end
---@param value string
function m:ReplaceSelection(value) end
---@param xml FairyGUI.Utils.XML
function m:Setup_BeforeAdd(xml) end
return m