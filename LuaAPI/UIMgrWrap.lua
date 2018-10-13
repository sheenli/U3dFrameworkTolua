---@class UIMgr : EventDispatcherNode
---@field public Instance UIMgr
---@field public threadSafe bool
local m = {}
function m:Init() end
---@param uiName string
---@return BaseUI
function m:GetWindow(uiName) end
---@return table
function m:GetAllOpenWindows() end
---@param uiName string
---@param type System.Type
---@param _createFun System.Func
---@param param object
---@param hideWinds table
---@param hideDotDel bool
function m:ShowWindow(uiName, type, _createFun, param, hideWinds, hideDotDel) end
---@param uiName string
function m:CloseWindow(uiName) end
---@param uiName string
function m:DeleteWind(uiName) end
---@param uiName string
---@return bool
function m:IsOpenWindow(uiName) end
---@param isDotDel bool
function m:DeleteAllWindows(isDotDel) end
---@param isDotDel bool
function m:HideAllWindows(isDotDel) end
function m:OnUpdate() end
return m