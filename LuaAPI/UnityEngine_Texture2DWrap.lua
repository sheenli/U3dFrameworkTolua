---@class UnityEngine.Texture2D : UnityEngine.Texture
---@field public mipmapCount int
---@field public format UnityEngine.TextureFormat
---@field public whiteTexture UnityEngine.Texture2D
---@field public blackTexture UnityEngine.Texture2D
local m = {}
---@param width int
---@param height int
---@param format UnityEngine.TextureFormat
---@param mipmap bool
---@param linear bool
---@param nativeTex System.IntPtr
---@return UnityEngine.Texture2D
function m.CreateExternalTexture(width, height, format, mipmap, linear, nativeTex) end
---@param nativeTex System.IntPtr
function m:UpdateExternalTexture(nativeTex) end
---@param x int
---@param y int
---@param color UnityEngine.Color
function m:SetPixel(x, y, color) end
---@param x int
---@param y int
---@return UnityEngine.Color
function m:GetPixel(x, y) end
---@param u float
---@param v float
---@return UnityEngine.Color
function m:GetPixelBilinear(u, v) end
---@param colors table
function m:SetPixels(colors) end
---@param colors table
function m:SetPixels32(colors) end
---@param data table
---@param markNonReadable bool
---@return bool
function m:LoadImage(data, markNonReadable) end
---@param data table
function m:LoadRawTextureData(data) end
---@return table
function m:GetRawTextureData() end
---@return table
function m:GetPixels() end
---@param miplevel int
---@return table
function m:GetPixels32(miplevel) end
---@param updateMipmaps bool
---@param makeNoLongerReadable bool
function m:Apply(updateMipmaps, makeNoLongerReadable) end
---@param width int
---@param height int
---@param format UnityEngine.TextureFormat
---@param hasMipMap bool
---@return bool
function m:Resize(width, height, format, hasMipMap) end
---@param highQuality bool
function m:Compress(highQuality) end
---@param textures table
---@param padding int
---@param maximumAtlasSize int
---@param makeNoLongerReadable bool
---@return table
function m:PackTextures(textures, padding, maximumAtlasSize, makeNoLongerReadable) end
---@param sizes table
---@param padding int
---@param atlasSize int
---@param results table
---@return bool
function m.GenerateAtlas(sizes, padding, atlasSize, results) end
---@param source UnityEngine.Rect
---@param destX int
---@param destY int
---@param recalculateMipMaps bool
function m:ReadPixels(source, destX, destY, recalculateMipMaps) end
---@return table
function m:EncodeToPNG() end
---@param quality int
---@return table
function m:EncodeToJPG(quality) end
---@param flags UnityEngine.Texture2D.EXRFlags
---@return table
function m:EncodeToEXR(flags) end
return m