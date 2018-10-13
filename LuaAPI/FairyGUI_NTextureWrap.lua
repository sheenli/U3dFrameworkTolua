---@class FairyGUI.NTexture : object
---@field public Empty FairyGUI.NTexture
---@field public width int
---@field public height int
---@field public nativeTexture UnityEngine.Texture
---@field public alphaTexture FairyGUI.NTexture
---@field public root FairyGUI.NTexture
---@field public uvRect UnityEngine.Rect
---@field public rotated bool
---@field public materialManagers table
---@field public refCount int
---@field public disposed bool
---@field public lastActive float
---@field public storedODisk bool
local m = {}
function m.DisposeEmpty() end
function m:DestroyMaterials() end
function m:Dispose() end
return m