---@class FairyGUI.GLoader : FairyGUI.GObject
---@field public url string
---@field public icon string
---@field public align FairyGUI.AlignType
---@field public verticalAlign FairyGUI.VertAlignType
---@field public fill FairyGUI.FillType
---@field public autoSize bool
---@field public playing bool
---@field public frame int
---@field public material UnityEngine.Material
---@field public shader string
---@field public color UnityEngine.Color
---@field public fillMethod FairyGUI.FillMethod
---@field public fillOrigin int
---@field public fillClockwise bool
---@field public fillAmount float
---@field public image FairyGUI.Image
---@field public movieClip FairyGUI.MovieClip
---@field public texture FairyGUI.NTexture
---@field public filter FairyGUI.IFilter
---@field public blendMode FairyGUI.BlendMode
---@field public showErrorSign bool
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
function m:Dispose() end
---@param xml FairyGUI.Utils.XML
function m:Setup_BeforeAdd(xml) end
return m