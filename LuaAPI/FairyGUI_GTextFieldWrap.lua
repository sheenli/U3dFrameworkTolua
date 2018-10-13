---@class FairyGUI.GTextField : FairyGUI.GObject
---@field public text string
---@field public textFormat FairyGUI.TextFormat
---@field public color UnityEngine.Color
---@field public align FairyGUI.AlignType
---@field public verticalAlign FairyGUI.VertAlignType
---@field public singleLine bool
---@field public stroke int
---@field public strokeColor UnityEngine.Color
---@field public shadowOffset UnityEngine.Vector2
---@field public UBBEnabled bool
---@field public autoSize FairyGUI.AutoSizeType
---@field public textWidth float
---@field public textHeight float
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
---@param xml FairyGUI.Utils.XML
function m:Setup_BeforeAdd(xml) end
---@param xml FairyGUI.Utils.XML
function m:Setup_AfterAdd(xml) end
return m