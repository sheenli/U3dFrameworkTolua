---@class UnityEngine.UI.Graphic : UnityEngine.EventSystems.UIBehaviour
---@field public defaultGraphicMaterial UnityEngine.Material
---@field public color UnityEngine.Color
---@field public raycastTarget bool
---@field public depth int
---@field public rectTransform UnityEngine.RectTransform
---@field public canvas UnityEngine.Canvas
---@field public canvasRenderer UnityEngine.CanvasRenderer
---@field public defaultMaterial UnityEngine.Material
---@field public material UnityEngine.Material
---@field public materialForRendering UnityEngine.Material
---@field public mainTexture UnityEngine.Texture
local m = {}
function m:SetAllDirty() end
function m:SetLayoutDirty() end
function m:SetVerticesDirty() end
function m:SetMaterialDirty() end
---@param update UnityEngine.UI.CanvasUpdate
function m:Rebuild(update) end
function m:LayoutComplete() end
function m:GraphicUpdateComplete() end
function m:SetNativeSize() end
---@param sp UnityEngine.Vector2
---@param eventCamera UnityEngine.Camera
---@return bool
function m:Raycast(sp, eventCamera) end
---@param point UnityEngine.Vector2
---@return UnityEngine.Vector2
function m:PixelAdjustPoint(point) end
---@return UnityEngine.Rect
function m:GetPixelAdjustedRect() end
---@param targetColor UnityEngine.Color
---@param duration float
---@param ignoreTimeScale bool
---@param useAlpha bool
function m:CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha) end
---@param alpha float
---@param duration float
---@param ignoreTimeScale bool
function m:CrossFadeAlpha(alpha, duration, ignoreTimeScale) end
---@param action UnityEngine.Events.UnityAction
function m:RegisterDirtyLayoutCallback(action) end
---@param action UnityEngine.Events.UnityAction
function m:UnregisterDirtyLayoutCallback(action) end
---@param action UnityEngine.Events.UnityAction
function m:RegisterDirtyVerticesCallback(action) end
---@param action UnityEngine.Events.UnityAction
function m:UnregisterDirtyVerticesCallback(action) end
---@param action UnityEngine.Events.UnityAction
function m:RegisterDirtyMaterialCallback(action) end
---@param action UnityEngine.Events.UnityAction
function m:UnregisterDirtyMaterialCallback(action) end
return m