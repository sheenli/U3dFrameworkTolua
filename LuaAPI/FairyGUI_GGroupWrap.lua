---@class FairyGUI.GGroup : FairyGUI.GObject
---@field public layout FairyGUI.GroupLayoutType
---@field public lineGap int
---@field public columnGap int
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
---@param childSizeChanged bool
function m:SetBoundsChangedFlag(childSizeChanged) end
function m:EnsureBoundsCorrect() end
---@param xml FairyGUI.Utils.XML
function m:Setup_BeforeAdd(xml) end
---@param xml FairyGUI.Utils.XML
function m:Setup_AfterAdd(xml) end
return m