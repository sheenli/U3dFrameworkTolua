---@class UnityEngine.PlayerPrefs : object
local m = {}
---@param key string
---@param value int
function m.SetInt(key, value) end
---@param key string
---@param defaultValue int
---@return int
function m.GetInt(key, defaultValue) end
---@param key string
---@param value float
function m.SetFloat(key, value) end
---@param key string
---@param defaultValue float
---@return float
function m.GetFloat(key, defaultValue) end
---@param key string
---@param value string
function m.SetString(key, value) end
---@param key string
---@param defaultValue string
---@return string
function m.GetString(key, defaultValue) end
---@param key string
---@return bool
function m.HasKey(key) end
---@param key string
function m.DeleteKey(key) end
function m.DeleteAll() end
function m.Save() end
return m