---@class FairyGUI.GGraph : FairyGUI.GObject
---@field public color UnityEngine.Color
---@field public shape FairyGUI.Shape
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
---@param target FairyGUI.GObject
function m:ReplaceMe(target) end
---@param target FairyGUI.GObject
function m:AddBeforeMe(target) end
---@param target FairyGUI.GObject
function m:AddAfterMe(target) end
---@param obj FairyGUI.DisplayObject
function m:SetNativeObject(obj) end
---@param aWidth float
---@param aHeight float
---@param lineSize int
---@param lineColor UnityEngine.Color
---@param fillColor UnityEngine.Color
function m:DrawRect(aWidth, aHeight, lineSize, lineColor, fillColor) end
---@param aWidth float
---@param aHeight float
---@param fillColor UnityEngine.Color
---@param corner table
function m:DrawRoundRect(aWidth, aHeight, fillColor, corner) end
---@param aWidth float
---@param aHeight float
---@param fillColor UnityEngine.Color
function m:DrawEllipse(aWidth, aHeight, fillColor) end
---@param aWidth float
---@param aHeight float
---@param points table
---@param fillColor UnityEngine.Color
function m:DrawPolygon(aWidth, aHeight, points, fillColor) end
---@param xml FairyGUI.Utils.XML
function m:Setup_BeforeAdd(xml) end
return m