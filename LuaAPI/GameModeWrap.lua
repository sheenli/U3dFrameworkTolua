---@class GameMode : EventDispatcherNode
---@field public pos string
---@field public OpenId string
---@field public CardBack int
---@field public DeskBack int
---@field public Instance GameMode
---@field public IP string
---@field public latitude float
---@field public longitude float
---@field public AuditMode bool
---@field public threadSafe bool
local m = {}
---@param mode IMode
function m:AddMode(mode) end
function m:InitDtata() end
---@return int
function m:SendLoginMsgs() end
function m:ClearData() end
function m:OnUpdate() end
function m:OnDestroy() end
return m