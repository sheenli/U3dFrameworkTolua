---@class UnityEngine.UI.Text : UnityEngine.UI.MaskableGraphic
---@field public cachedTextGenerator UnityEngine.TextGenerator
---@field public cachedTextGeneratorForLayout UnityEngine.TextGenerator
---@field public mainTexture UnityEngine.Texture
---@field public font UnityEngine.Font
---@field public text string
---@field public supportRichText bool
---@field public resizeTextForBestFit bool
---@field public resizeTextMinSize int
---@field public resizeTextMaxSize int
---@field public alignment UnityEngine.TextAnchor
---@field public alignByGeometry bool
---@field public fontSize int
---@field public horizontalOverflow UnityEngine.HorizontalWrapMode
---@field public verticalOverflow UnityEngine.VerticalWrapMode
---@field public lineSpacing float
---@field public fontStyle UnityEngine.FontStyle
---@field public pixelsPerUnit float
---@field public minWidth float
---@field public preferredWidth float
---@field public flexibleWidth float
---@field public minHeight float
---@field public preferredHeight float
---@field public flexibleHeight float
---@field public layoutPriority int
local m = {}
function m:FontTextureChanged() end
---@param extents UnityEngine.Vector2
---@return UnityEngine.TextGenerationSettings
function m:GetGenerationSettings(extents) end
---@param anchor UnityEngine.TextAnchor
---@return UnityEngine.Vector2
function m.GetTextAnchorPivot(anchor) end
function m:CalculateLayoutInputHorizontal() end
function m:CalculateLayoutInputVertical() end
return m