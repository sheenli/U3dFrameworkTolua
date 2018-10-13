---@class FairyGUI.ScrollPane : FairyGUI.EventDispatcher
---@field public onScroll FairyGUI.EventListener
---@field public onScrollEnd FairyGUI.EventListener
---@field public onPullDownRelease FairyGUI.EventListener
---@field public onPullUpRelease FairyGUI.EventListener
---@field public draggingPane FairyGUI.ScrollPane
---@field public owner FairyGUI.GComponent
---@field public hzScrollBar FairyGUI.GScrollBar
---@field public vtScrollBar FairyGUI.GScrollBar
---@field public header FairyGUI.GComponent
---@field public footer FairyGUI.GComponent
---@field public bouncebackEffect bool
---@field public touchEffect bool
---@field public inertiaDisabled bool
---@field public softnessOnTopOrLeftSide bool
---@field public scrollStep float
---@field public snapToItem bool
---@field public pageMode bool
---@field public pageController FairyGUI.Controller
---@field public mouseWheelEnabled bool
---@field public decelerationRate float
---@field public percX float
---@field public percY float
---@field public posX float
---@field public posY float
---@field public isBottomMost bool
---@field public isRightMost bool
---@field public currentPageX int
---@field public currentPageY int
---@field public scrollingPosX float
---@field public scrollingPosY float
---@field public contentWidth float
---@field public contentHeight float
---@field public viewWidth float
---@field public viewHeight float
local m = {}
function m:Dispose() end
---@param value float
---@param ani bool
function m:SetPercX(value, ani) end
---@param value float
---@param ani bool
function m:SetPercY(value, ani) end
---@param value float
---@param ani bool
function m:SetPosX(value, ani) end
---@param value float
---@param ani bool
function m:SetPosY(value, ani) end
---@param value int
---@param ani bool
function m:SetCurrentPageX(value, ani) end
---@param value int
---@param ani bool
function m:SetCurrentPageY(value, ani) end
function m:ScrollTop() end
function m:ScrollBottom() end
function m:ScrollUp() end
function m:ScrollDown() end
function m:ScrollLeft() end
function m:ScrollRight() end
---@param obj FairyGUI.GObject
function m:ScrollToView(obj) end
---@param obj FairyGUI.GObject
---@return bool
function m:IsChildInView(obj) end
function m:CancelDragging() end
---@param size int
function m:LockHeader(size) end
---@param size int
function m:LockFooter(size) end
return m