---@class YKSupportLua.LuaWindow : BaseUI
---@field public ResName string
---@field public PackName string
---@field public resName string
---@field public packName string
---@field public isNeedHideAnimation bool
---@field public isNeedShowAnimation bool
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
---@param peerTable LuaInterface.LuaTable
function m:ConnectLua(peerTable) end
function m:Dispose() end
return m