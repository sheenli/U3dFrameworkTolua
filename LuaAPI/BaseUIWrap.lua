---@class BaseUI : FairyGUI.Window
---@field public PackName string
---@field public ResName string
---@field public eventMgr EventListenerMgr
---@field public isDotDel bool
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
---@param paramData object
function m:SetData(paramData) end
---@return object
function m:GetData() end
---@param type string
---@param dis EventDispatcherNode
---@param _priority int
---@param _dispatchOnce bool
function m:AddListener(type, dis, _priority, _dispatchOnce) end
function m:Dispose() end
function m:Refresh() end
return m