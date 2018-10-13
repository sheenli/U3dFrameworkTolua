---@class FairyGUI.Relations : object
---@field public isEmpty bool
---@field public handling FairyGUI.GObject
local m = {}
---@param target FairyGUI.GObject
---@param relationType FairyGUI.RelationType
function m:Add(target, relationType) end
---@param target FairyGUI.GObject
---@param relationType FairyGUI.RelationType
function m:Remove(target, relationType) end
---@param target FairyGUI.GObject
---@return bool
function m:Contains(target) end
---@param target FairyGUI.GObject
function m:ClearFor(target) end
function m:ClearAll() end
---@param source FairyGUI.Relations
function m:CopyFrom(source) end
function m:Dispose() end
---@param dWidth float
---@param dHeight float
---@param applyPivot bool
function m:OnOwnerSizeChanged(dWidth, dHeight, applyPivot) end
---@param xml FairyGUI.Utils.XML
function m:Setup(xml) end
return m