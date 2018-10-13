---@class FairyGUI.TextFormat : object
---@field public size int
---@field public font string
---@field public color UnityEngine.Color
---@field public lineSpacing int
---@field public letterSpacing int
---@field public bold bool
---@field public underline bool
---@field public italic bool
---@field public gradientColor table
---@field public align FairyGUI.AlignType
---@field public specialStyle FairyGUI.TextFormat.SpecialStyle
local m = {}
---@param value uint
function m:SetColor(value) end
---@param aFormat FairyGUI.TextFormat
---@return bool
function m:EqualStyle(aFormat) end
---@param source FairyGUI.TextFormat
function m:CopyFrom(source) end
return m