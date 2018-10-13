---@class FairyGUI.PopupMenu : object
---@field public itemCount int
---@field public contentPane FairyGUI.GComponent
---@field public list FairyGUI.GList
local m = {}
---@param caption string
---@param callback FairyGUI.EventCallback0
---@return FairyGUI.GButton
function m:AddItem(caption, callback) end
---@param caption string
---@param index int
---@param callback FairyGUI.EventCallback0
---@return FairyGUI.GButton
function m:AddItemAt(caption, index, callback) end
function m:AddSeperator() end
---@param index int
---@return string
function m:GetItemName(index) end
---@param name string
---@param caption string
function m:SetItemText(name, caption) end
---@param name string
---@param visible bool
function m:SetItemVisible(name, visible) end
---@param name string
---@param grayed bool
function m:SetItemGrayed(name, grayed) end
---@param name string
---@param checkable bool
function m:SetItemCheckable(name, checkable) end
---@param name string
---@param check bool
function m:SetItemChecked(name, check) end
---@param name string
---@return bool
function m:isItemChecked(name) end
---@param name string
---@return bool
function m:RemoveItem(name) end
function m:ClearItems() end
function m:Dispose() end
function m:Show() end
return m