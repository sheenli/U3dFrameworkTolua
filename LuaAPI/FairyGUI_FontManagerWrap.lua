---@class FairyGUI.FontManager : object
local m = {}
---@param font FairyGUI.BaseFont
---@param alias string
function m.RegisterFont(font, alias) end
---@param font FairyGUI.BaseFont
function m.UnregisterFont(font) end
---@param name string
---@return FairyGUI.BaseFont
function m.GetFont(name) end
function m.Clear() end
return m