---@class UnityEngine.Input : object
---@field public compensateSensors bool
---@field public gyro UnityEngine.Gyroscope
---@field public mousePosition UnityEngine.Vector3
---@field public mouseScrollDelta UnityEngine.Vector2
---@field public mousePresent bool
---@field public simulateMouseWithTouches bool
---@field public anyKey bool
---@field public anyKeyDown bool
---@field public inputString string
---@field public acceleration UnityEngine.Vector3
---@field public accelerationEvents table
---@field public accelerationEventCount int
---@field public touches table
---@field public touchCount int
---@field public touchPressureSupported bool
---@field public stylusTouchSupported bool
---@field public touchSupported bool
---@field public multiTouchEnabled bool
---@field public location UnityEngine.LocationService
---@field public compass UnityEngine.Compass
---@field public deviceOrientation UnityEngine.DeviceOrientation
---@field public imeCompositionMode UnityEngine.IMECompositionMode
---@field public compositionString string
---@field public imeIsSelected bool
---@field public compositionCursorPos UnityEngine.Vector2
---@field public backButtonLeavesApp bool
local m = {}
---@param axisName string
---@return float
function m.GetAxis(axisName) end
---@param axisName string
---@return float
function m.GetAxisRaw(axisName) end
---@param buttonName string
---@return bool
function m.GetButton(buttonName) end
---@param buttonName string
---@return bool
function m.GetButtonDown(buttonName) end
---@param buttonName string
---@return bool
function m.GetButtonUp(buttonName) end
---@param name string
---@return bool
function m.GetKey(name) end
---@param name string
---@return bool
function m.GetKeyDown(name) end
---@param name string
---@return bool
function m.GetKeyUp(name) end
---@return table
function m.GetJoystickNames() end
---@param button int
---@return bool
function m.GetMouseButton(button) end
---@param button int
---@return bool
function m.GetMouseButtonDown(button) end
---@param button int
---@return bool
function m.GetMouseButtonUp(button) end
function m.ResetInputAxes() end
---@param index int
---@return UnityEngine.AccelerationEvent
function m.GetAccelerationEvent(index) end
---@param index int
---@return UnityEngine.Touch
function m.GetTouch(index) end
return m