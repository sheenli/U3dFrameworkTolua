---@class FairyGUI.NGraphics : object
---@field public vertices table
---@field public uv table
---@field public colors table
---@field public triangles table
---@field public vertCount int
---@field public meshFilter UnityEngine.MeshFilter
---@field public meshRenderer UnityEngine.MeshRenderer
---@field public mesh UnityEngine.Mesh
---@field public gameObject UnityEngine.GameObject
---@field public texture FairyGUI.NTexture
---@field public shader string
---@field public material UnityEngine.Material
---@field public materialKeywords table
---@field public enabled bool
---@field public sortingOrder int
---@field public alpha float
---@field public grayed bool
---@field public blendMode FairyGUI.BlendMode
---@field public dontClip bool
---@field public maskFrameId uint
---@field public vertexMatrix System.Nullable
---@field public cameraPosition System.Nullable
---@field public meshModifier FairyGUI.NGraphics.MeshModifier
---@field public TRIANGLES table
---@field public TRIANGLES_9_GRID table
---@field public TRIANGLES_4_GRID table
local m = {}
---@param shader string
---@param texture FairyGUI.NTexture
function m:SetShaderAndTexture(shader, texture) end
---@param value int
function m:SetStencilEraserOrder(value) end
function m:Dispose() end
---@param context FairyGUI.UpdateContext
function m:UpdateMaterial(context) end
---@param vertCount int
function m:Alloc(vertCount) end
function m:UpdateMesh() end
---@param vertRect UnityEngine.Rect
---@param uvRect UnityEngine.Rect
---@param color UnityEngine.Color
function m:DrawRect(vertRect, uvRect, color) end
---@param vertRect UnityEngine.Rect
---@param uvRect UnityEngine.Rect
---@param fillColor UnityEngine.Color
---@param topLeftRadius float
---@param topRightRadius float
---@param bottomLeftRadius float
---@param bottomRightRadius float
function m:DrawRoundRect(vertRect, uvRect, fillColor, topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius) end
---@param vertRect UnityEngine.Rect
---@param uvRect UnityEngine.Rect
---@param fillColor UnityEngine.Color
function m:DrawEllipse(vertRect, uvRect, fillColor) end
---@param vertRect UnityEngine.Rect
---@param uvRect UnityEngine.Rect
---@param points table
---@param fillColor UnityEngine.Color
function m:DrawPolygon(vertRect, uvRect, points, fillColor) end
---@param vertRect UnityEngine.Rect
---@param uvRect UnityEngine.Rect
---@param fillColor UnityEngine.Color
---@param method FairyGUI.FillMethod
---@param amount float
---@param origin int
---@param clockwise bool
function m:DrawRectWithFillMethod(vertRect, uvRect, fillColor, method, amount, origin, clockwise) end
---@param index int
---@param rect UnityEngine.Rect
function m:FillVerts(index, rect) end
---@param index int
---@param rect UnityEngine.Rect
function m:FillUV(index, rect) end
---@param value UnityEngine.Color
function m:FillColors(value) end
function m:FillTriangles() end
function m:ClearMesh() end
---@param value UnityEngine.Color
function m:Tint(value) end
---@param verts table
---@param index int
---@param rect UnityEngine.Rect
function m.FillVertsOfQuad(verts, index, rect) end
---@param uv table
---@param index int
---@param rect UnityEngine.Rect
function m.FillUVOfQuad(uv, index, rect) end
---@param uv table
---@param baseUVRect UnityEngine.Rect
function m.RotateUV(uv, baseUVRect) end
return m