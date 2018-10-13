---@class UnityEngine.SceneManagement.SceneManager : object
---@field public sceneCount int
---@field public sceneCountInBuildSettings int
local m = {}
---@return UnityEngine.SceneManagement.Scene
function m.GetActiveScene() end
---@param scene UnityEngine.SceneManagement.Scene
---@return bool
function m.SetActiveScene(scene) end
---@param scenePath string
---@return UnityEngine.SceneManagement.Scene
function m.GetSceneByPath(scenePath) end
---@param name string
---@return UnityEngine.SceneManagement.Scene
function m.GetSceneByName(name) end
---@param buildIndex int
---@return UnityEngine.SceneManagement.Scene
function m.GetSceneByBuildIndex(buildIndex) end
---@param index int
---@return UnityEngine.SceneManagement.Scene
function m.GetSceneAt(index) end
---@param sceneName string
function m.LoadScene(sceneName) end
---@param sceneName string
---@return UnityEngine.AsyncOperation
function m.LoadSceneAsync(sceneName) end
---@param sceneName string
---@return UnityEngine.SceneManagement.Scene
function m.CreateScene(sceneName) end
---@param sceneBuildIndex int
---@return UnityEngine.AsyncOperation
function m.UnloadSceneAsync(sceneBuildIndex) end
---@param sourceScene UnityEngine.SceneManagement.Scene
---@param destinationScene UnityEngine.SceneManagement.Scene
function m.MergeScenes(sourceScene, destinationScene) end
---@param go UnityEngine.GameObject
---@param scene UnityEngine.SceneManagement.Scene
function m.MoveGameObjectToScene(go, scene) end
return m