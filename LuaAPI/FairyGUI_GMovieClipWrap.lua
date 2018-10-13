---@class FairyGUI.GMovieClip : FairyGUI.GObject
---@field public onPlayEnd FairyGUI.EventListener
---@field public playing bool
---@field public frame int
---@field public color UnityEngine.Color
---@field public flip FairyGUI.FlipType
---@field public material UnityEngine.Material
---@field public shader string
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
---@param end int
---@param times int
---@param endAt int
function m:SetPlaySettings(start, end, times, endAt) end
function m:ConstructFromResource() end
---@param xml FairyGUI.Utils.XML
function m:Setup_BeforeAdd(xml) end
return m