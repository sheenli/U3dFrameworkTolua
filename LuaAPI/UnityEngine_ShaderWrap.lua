---@class UnityEngine.Shader : UnityEngine.Object
---@field public isSupported bool
---@field public maximumLOD int
---@field public globalMaximumLOD int
---@field public globalRenderPipeline string
---@field public renderQueue int
local m = {}
---@param name string
---@return UnityEngine.Shader
function m.Find(name) end
---@param keyword string
function m.EnableKeyword(keyword) end
---@param keyword string
function m.DisableKeyword(keyword) end
---@param keyword string
---@return bool
function m.IsKeywordEnabled(keyword) end
---@param nameID int
---@param buffer UnityEngine.ComputeBuffer
function m.SetGlobalBuffer(nameID, buffer) end
---@param name string
---@return int
function m.PropertyToID(name) end
function m.WarmupAllShaders() end
---@param name string
---@param value float
function m.SetGlobalFloat(name, value) end
---@param name string
---@param value int
function m.SetGlobalInt(name, value) end
---@param name string
---@param value UnityEngine.Vector4
function m.SetGlobalVector(name, value) end
---@param name string
---@param value UnityEngine.Color
function m.SetGlobalColor(name, value) end
---@param name string
---@param value UnityEngine.Matrix4x4
function m.SetGlobalMatrix(name, value) end
---@param name string
---@param value UnityEngine.Texture
function m.SetGlobalTexture(name, value) end
---@param name string
---@param values table
function m.SetGlobalFloatArray(name, values) end
---@param name string
---@param values table
function m.SetGlobalVectorArray(name, values) end
---@param name string
---@param values table
function m.SetGlobalMatrixArray(name, values) end
---@param name string
---@return float
function m.GetGlobalFloat(name) end
---@param name string
---@return int
function m.GetGlobalInt(name) end
---@param name string
---@return UnityEngine.Vector4
function m.GetGlobalVector(name) end
---@param name string
---@return UnityEngine.Color
function m.GetGlobalColor(name) end
---@param name string
---@return UnityEngine.Matrix4x4
function m.GetGlobalMatrix(name) end
---@param name string
---@return UnityEngine.Texture
function m.GetGlobalTexture(name) end
---@param name string
---@param values table
function m.GetGlobalFloatArray(name, values) end
---@param name string
---@param values table
function m.GetGlobalVectorArray(name, values) end
---@param name string
---@param values table
function m.GetGlobalMatrixArray(name, values) end
return m