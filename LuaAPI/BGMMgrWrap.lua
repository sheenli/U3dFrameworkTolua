---@class BGMMgr : UnityEngine.MonoBehaviour
---@field public Volume float
---@field public IsOn bool
---@field public Instance BGMMgr
---@field public IsValid bool
---@field public scourceCtrls table
local m = {}
function m:Awake() end
function m:OnDestroy() end
---@param strBGMName string
function m:PlayBGMAsync(strBGMName) end
function m:StopBGM() end
function m:PauseBGM() end
function m:Resume() end
function m:Release() end
function m:FixedUpdate() end
return m