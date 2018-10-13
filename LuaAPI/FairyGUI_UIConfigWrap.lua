---@class FairyGUI.UIConfig : UnityEngine.MonoBehaviour
---@field public defaultFont string
---@field public renderingTextBrighterOnDesktop bool
---@field public windowModalWaiting string
---@field public globalModalWaiting string
---@field public modalLayerColor UnityEngine.Color
---@field public buttonSound UnityEngine.AudioClip
---@field public buttonSoundVolumeScale float
---@field public horizontalScrollBar string
---@field public verticalScrollBar string
---@field public defaultScrollStep float
---@field public defaultScrollDecelerationRate float
---@field public defaultScrollBarDisplay FairyGUI.ScrollBarDisplayType
---@field public defaultScrollTouchEffect bool
---@field public defaultScrollBounceEffect bool
---@field public popupMenu string
---@field public popupMenu_seperator string
---@field public loaderErrorSign string
---@field public tooltipsWin string
---@field public defaultComboBoxVisibleItemCount int
---@field public touchScrollSensitivity int
---@field public touchDragSensitivity int
---@field public clickDragSensitivity int
---@field public allowSoftnessOnTopOrLeftSide bool
---@field public bringWindowToFrontOnClick bool
---@field public inputCaretSize int
---@field public inputHighlightColor UnityEngine.Color
---@field public frameTimeForAsyncUIConstruction float
---@field public depthSupportForPaintingMode bool
---@field public Items table
---@field public PreloadPackages table
---@field public soundLoader FairyGUI.UIConfig.SoundLoader
local m = {}
function m:Load() end
function m.ClearResourceRefs() end
function m:ApplyModifiedProperties() end
return m