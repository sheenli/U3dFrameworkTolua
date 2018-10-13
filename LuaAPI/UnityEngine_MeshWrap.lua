---@class UnityEngine.Mesh : UnityEngine.Object
---@field public isReadable bool
---@field public blendShapeCount int
---@field public vertexBufferCount int
---@field public bounds UnityEngine.Bounds
---@field public vertexCount int
---@field public subMeshCount int
---@field public boneWeights table
---@field public bindposes table
---@field public vertices table
---@field public normals table
---@field public tangents table
---@field public uv table
---@field public uv2 table
---@field public uv3 table
---@field public uv4 table
---@field public colors table
---@field public colors32 table
---@field public triangles table
local m = {}
---@param keepVertexLayout bool
function m:Clear(keepVertexLayout) end
---@param triangles table
---@param submesh int
function m:SetTriangles(triangles, submesh) end
---@param indices table
---@param topology UnityEngine.MeshTopology
---@param submesh int
function m:SetIndices(indices, topology, submesh) end
function m:ClearBlendShapes() end
---@param shapeIndex int
---@return string
function m:GetBlendShapeName(shapeIndex) end
---@param shapeIndex int
---@return int
function m:GetBlendShapeFrameCount(shapeIndex) end
---@param shapeIndex int
---@param frameIndex int
---@return float
function m:GetBlendShapeFrameWeight(shapeIndex, frameIndex) end
---@param shapeIndex int
---@param frameIndex int
---@param deltaVertices table
---@param deltaNormals table
---@param deltaTangents table
function m:GetBlendShapeFrameVertices(shapeIndex, frameIndex, deltaVertices, deltaNormals, deltaTangents) end
---@param shapeName string
---@param frameWeight float
---@param deltaVertices table
---@param deltaNormals table
---@param deltaTangents table
function m:AddBlendShapeFrame(shapeName, frameWeight, deltaVertices, deltaNormals, deltaTangents) end
---@param bufferIndex int
---@return System.IntPtr
function m:GetNativeVertexBufferPtr(bufferIndex) end
---@return System.IntPtr
function m:GetNativeIndexBufferPtr() end
function m:RecalculateBounds() end
function m:RecalculateNormals() end
function m:RecalculateTangents() end
---@param submesh int
---@return UnityEngine.MeshTopology
function m:GetTopology(submesh) end
---@param submesh int
---@return uint
function m:GetIndexStart(submesh) end
---@param submesh int
---@return uint
function m:GetIndexCount(submesh) end
---@param combine table
---@param mergeSubMeshes bool
---@param useMatrices bool
---@param hasLightmapData bool
function m:CombineMeshes(combine, mergeSubMeshes, useMatrices, hasLightmapData) end
function m:MarkDynamic() end
---@param markNoLogerReadable bool
function m:UploadMeshData(markNoLogerReadable) end
---@param blendShapeName string
---@return int
function m:GetBlendShapeIndex(blendShapeName) end
---@param vertices table
function m:GetVertices(vertices) end
---@param inVertices table
function m:SetVertices(inVertices) end
---@param normals table
function m:GetNormals(normals) end
---@param inNormals table
function m:SetNormals(inNormals) end
---@param tangents table
function m:GetTangents(tangents) end
---@param inTangents table
function m:SetTangents(inTangents) end
---@param colors table
function m:GetColors(colors) end
---@param inColors table
function m:SetColors(inColors) end
---@param channel int
---@param uvs table
function m:SetUVs(channel, uvs) end
---@param channel int
---@param uvs table
function m:GetUVs(channel, uvs) end
---@param submesh int
---@return table
function m:GetTriangles(submesh) end
---@param submesh int
---@return table
function m:GetIndices(submesh) end
---@param bindposes table
function m:GetBindposes(bindposes) end
---@param boneWeights table
function m:GetBoneWeights(boneWeights) end
return m