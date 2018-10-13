---@class UnityEngine.Application : object
---@field public streamedBytes int
---@field public isPlaying bool
---@field public isFocused bool
---@field public isEditor bool
---@field public isWebPlayer bool
---@field public platform UnityEngine.RuntimePlatform
---@field public buildGUID string
---@field public isMobilePlatform bool
---@field public isConsolePlatform bool
---@field public runInBackground bool
---@field public dataPath string
---@field public streamingAssetsPath string
---@field public persistentDataPath string
---@field public temporaryCachePath string
---@field public srcValue string
---@field public absoluteURL string
---@field public unityVersion string
---@field public version string
---@field public installerName string
---@field public identifier string
---@field public installMode UnityEngine.ApplicationInstallMode
---@field public sandboxType UnityEngine.ApplicationSandboxType
---@field public productName string
---@field public companyName string
---@field public cloudProjectId string
---@field public targetFrameRate int
---@field public systemLanguage UnityEngine.SystemLanguage
---@field public backgroundLoadingPriority UnityEngine.ThreadPriority
---@field public internetReachability UnityEngine.NetworkReachability
---@field public genuine bool
---@field public genuineCheckAvailable bool
local m = {}
function m.Quit() end
function m.CancelQuit() end
function m.Unload() end
---@param levelIndex int
---@return float
function m.GetStreamProgressForLevel(levelIndex) end
---@param levelIndex int
---@return bool
function m.CanStreamedLevelBeLoaded(levelIndex) end
---@return table
function m.GetBuildTags() end
---@param filename string
---@param superSize int
function m.CaptureScreenshot(filename, superSize) end
---@return bool
function m.HasProLicense() end
---@param functionName string
---@param args table
function m.ExternalCall(functionName, args) end
---@param delegateMethod UnityEngine.Application.AdvertisingIdentifierCallback
---@return bool
function m.RequestAdvertisingIdentifierAsync(delegateMethod) end
---@param url string
function m.OpenURL(url) end
---@param logType UnityEngine.LogType
---@return UnityEngine.StackTraceLogType
function m.GetStackTraceLogType(logType) end
---@param logType UnityEngine.LogType
---@param stackTraceType UnityEngine.StackTraceLogType
function m.SetStackTraceLogType(logType, stackTraceType) end
---@param mode UnityEngine.UserAuthorization
---@return UnityEngine.AsyncOperation
function m.RequestUserAuthorization(mode) end
---@param mode UnityEngine.UserAuthorization
---@return bool
function m.HasUserAuthorization(mode) end
return m