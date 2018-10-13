---@class Initialization : EventDispatcherNode
---@field public Instance Initialization
---@field public canvas UnityEngine.Canvas
---@field public verText UnityEngine.UI.Text
---@field public mLogo UnityEngine.GameObject
---@field public DebugMode bool
---@field public threadSafe bool
local m = {}
function m:Awake() end
function m:OnDestroy() end
function m:Update() end
---@param flag bool
function m:LogValid(flag) end
return m