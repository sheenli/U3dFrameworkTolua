---@class FairyGUI.DynamicFont : FairyGUI.BaseFont
---@field public mainTexture FairyGUI.NTexture
---@field public canTint bool
---@field public canLight bool
---@field public canOutline bool
---@field public hasChannel bool
---@field public customBold bool
---@field public customBoldAndItalic bool
---@field public shader string
---@field public keepCrisp bool
---@field public packageItem FairyGUI.PackageItem
local m = {}
---@param format FairyGUI.TextFormat
---@param fontSizeScale float
function m:SetFormat(format, fontSizeScale) end
---@param text string
function m:PrepareCharacters(text) end
---@param ch char
---@param width float
---@param height float
---@return bool
function m:GetGlyphSize(ch, width, height) end
---@param ch char
---@param glyph FairyGUI.GlyphInfo
---@return bool
function m:GetGlyph(ch, glyph) end
return m