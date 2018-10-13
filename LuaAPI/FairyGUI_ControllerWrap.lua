---@class FairyGUI.Controller : FairyGUI.EventDispatcher
---@field public onChanged FairyGUI.EventListener
---@field public selectedIndex int
---@field public selectedPage string
---@field public previsousIndex int
---@field public previousPage string
---@field public pageCount int
---@field public name string
local m = {}
function m:Dispose() end
---@param value int
function m:SetSelectedIndex(value) end
---@param value string
function m:SetSelectedPage(value) end
---@param index int
---@return string
function m:GetPageName(index) end
---@param aName string
---@return string
function m:GetPageIdByName(aName) end
---@param name string
function m:AddPage(name) end
---@param name string
---@param index int
function m:AddPageAt(name, index) end
---@param name string
function m:RemovePage(name) end
---@param index int
function m:RemovePageAt(index) end
function m:ClearPages() end
---@param aName string
---@return bool
function m:HasPage(aName) end
function m:RunActions() end
---@param xml FairyGUI.Utils.XML
function m:Setup(xml) end
return m