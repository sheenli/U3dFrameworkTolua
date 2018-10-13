---@class FairyGUI.RichTextField : FairyGUI.Container
---@field public htmlPageContext FairyGUI.Utils.IHtmlPageContext
---@field public htmlParseOptions FairyGUI.Utils.HtmlParseOptions
---@field public emojies table
---@field public textField FairyGUI.TextField
---@field public text string
---@field public htmlText string
---@field public textFormat FairyGUI.TextFormat
---@field public htmlElementCount int
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
---@param name string
---@return FairyGUI.Utils.HtmlElement
function m:GetHtmlElement(name) end
---@param index int
---@return FairyGUI.Utils.HtmlElement
function m:GetHtmlElementAt(index) end
---@param index int
---@param show bool
function m:ShowHtmlObject(index, show) end
function m:EnsureSizeCorrect() end
---@param context FairyGUI.UpdateContext
function m:Update(context) end
function m:Dispose() end
return m