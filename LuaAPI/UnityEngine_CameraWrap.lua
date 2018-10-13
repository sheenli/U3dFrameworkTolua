---@class UnityEngine.Camera : UnityEngine.Behaviour
---@field public fieldOfView float
---@field public nearClipPlane float
---@field public farClipPlane float
---@field public renderingPath UnityEngine.RenderingPath
---@field public actualRenderingPath UnityEngine.RenderingPath
---@field public allowHDR bool
---@field public forceIntoRenderTexture bool
---@field public allowMSAA bool
---@field public orthographicSize float
---@field public orthographic bool
---@field public opaqueSortMode UnityEngine.Rendering.OpaqueSortMode
---@field public transparencySortMode UnityEngine.TransparencySortMode
---@field public transparencySortAxis UnityEngine.Vector3
---@field public depth float
---@field public aspect float
---@field public cullingMask int
---@field public eventMask int
---@field public backgroundColor UnityEngine.Color
---@field public rect UnityEngine.Rect
---@field public pixelRect UnityEngine.Rect
---@field public targetTexture UnityEngine.RenderTexture
---@field public activeTexture UnityEngine.RenderTexture
---@field public pixelWidth int
---@field public pixelHeight int
---@field public cameraToWorldMatrix UnityEngine.Matrix4x4
---@field public worldToCameraMatrix UnityEngine.Matrix4x4
---@field public projectionMatrix UnityEngine.Matrix4x4
---@field public nonJitteredProjectionMatrix UnityEngine.Matrix4x4
---@field public useJitteredProjectionMatrixForTransparentRendering bool
---@field public velocity UnityEngine.Vector3
---@field public clearFlags UnityEngine.CameraClearFlags
---@field public stereoEnabled bool
---@field public stereoSeparation float
---@field public stereoConvergence float
---@field public cameraType UnityEngine.CameraType
---@field public stereoMirrorMode bool
---@field public stereoTargetEye UnityEngine.StereoTargetEyeMask
---@field public stereoActiveEye UnityEngine.Camera.MonoOrStereoscopicEye
---@field public targetDisplay int
---@field public main UnityEngine.Camera
---@field public current UnityEngine.Camera
---@field public allCameras table
---@field public allCamerasCount int
---@field public useOcclusionCulling bool
---@field public cullingMatrix UnityEngine.Matrix4x4
---@field public layerCullDistances table
---@field public layerCullSpherical bool
---@field public depthTextureMode UnityEngine.DepthTextureMode
---@field public clearStencilAfterLightingPass bool
---@field public commandBufferCount int
---@field public onPreCull UnityEngine.Camera.CameraCallback
---@field public onPreRender UnityEngine.Camera.CameraCallback
---@field public onPostRender UnityEngine.Camera.CameraCallback
local m = {}
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:DOAspect(endValue, duration) end
---@param endValue UnityEngine.Color
---@param duration float
---@return DG.Tweening.Tweener
function m:DOColor(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:DOFarClipPlane(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:DOFieldOfView(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:DONearClipPlane(endValue, duration) end
---@param endValue float
---@param duration float
---@return DG.Tweening.Tweener
function m:DOOrthoSize(endValue, duration) end
---@param endValue UnityEngine.Rect
---@param duration float
---@return DG.Tweening.Tweener
function m:DOPixelRect(endValue, duration) end
---@param endValue UnityEngine.Rect
---@param duration float
---@return DG.Tweening.Tweener
function m:DORect(endValue, duration) end
---@param duration float
---@param strength float
---@param vibrato int
---@param randomness float
---@param fadeOut bool
---@return DG.Tweening.Tweener
function m:DOShakePosition(duration, strength, vibrato, randomness, fadeOut) end
---@param duration float
---@param strength float
---@param vibrato int
---@param randomness float
---@param fadeOut bool
---@return DG.Tweening.Tweener
function m:DOShakeRotation(duration, strength, vibrato, randomness, fadeOut) end
---@param colorBuffer UnityEngine.RenderBuffer
---@param depthBuffer UnityEngine.RenderBuffer
function m:SetTargetBuffers(colorBuffer, depthBuffer) end
function m:ResetWorldToCameraMatrix() end
function m:ResetProjectionMatrix() end
function m:ResetAspect() end
---@param eye UnityEngine.Camera.StereoscopicEye
---@return UnityEngine.Matrix4x4
function m:GetStereoViewMatrix(eye) end
---@param eye UnityEngine.Camera.StereoscopicEye
---@param matrix UnityEngine.Matrix4x4
function m:SetStereoViewMatrix(eye, matrix) end
function m:ResetStereoViewMatrices() end
---@param eye UnityEngine.Camera.StereoscopicEye
---@return UnityEngine.Matrix4x4
function m:GetStereoProjectionMatrix(eye) end
---@param eye UnityEngine.Camera.StereoscopicEye
---@param matrix UnityEngine.Matrix4x4
function m:SetStereoProjectionMatrix(eye, matrix) end
---@param viewport UnityEngine.Rect
---@param z float
---@param eye UnityEngine.Camera.MonoOrStereoscopicEye
---@param outCorners table
function m:CalculateFrustumCorners(viewport, z, eye, outCorners) end
function m:ResetStereoProjectionMatrices() end
function m:ResetTransparencySortSettings() end
---@param position UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:WorldToScreenPoint(position) end
---@param position UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:WorldToViewportPoint(position) end
---@param position UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:ViewportToWorldPoint(position) end
---@param position UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:ScreenToWorldPoint(position) end
---@param position UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:ScreenToViewportPoint(position) end
---@param position UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:ViewportToScreenPoint(position) end
---@param position UnityEngine.Vector3
---@return UnityEngine.Ray
function m:ViewportPointToRay(position) end
---@param position UnityEngine.Vector3
---@return UnityEngine.Ray
function m:ScreenPointToRay(position) end
---@param cameras table
---@return int
function m.GetAllCameras(cameras) end
function m:Render() end
---@param shader UnityEngine.Shader
---@param replacementTag string
function m:RenderWithShader(shader, replacementTag) end
---@param shader UnityEngine.Shader
---@param replacementTag string
function m:SetReplacementShader(shader, replacementTag) end
function m:ResetReplacementShader() end
function m:ResetCullingMatrix() end
function m:RenderDontRestore() end
---@param cur UnityEngine.Camera
function m.SetupCurrent(cur) end
---@param cubemap UnityEngine.Cubemap
---@return bool
function m:RenderToCubemap(cubemap) end
---@param other UnityEngine.Camera
function m:CopyFrom(other) end
---@param evt UnityEngine.Rendering.CameraEvent
---@param buffer UnityEngine.Rendering.CommandBuffer
function m:AddCommandBuffer(evt, buffer) end
---@param evt UnityEngine.Rendering.CameraEvent
---@param buffer UnityEngine.Rendering.CommandBuffer
function m:RemoveCommandBuffer(evt, buffer) end
---@param evt UnityEngine.Rendering.CameraEvent
function m:RemoveCommandBuffers(evt) end
function m:RemoveAllCommandBuffers() end
---@param evt UnityEngine.Rendering.CameraEvent
---@return table
function m:GetCommandBuffers(evt) end
---@param clipPlane UnityEngine.Vector4
---@return UnityEngine.Matrix4x4
function m:CalculateObliqueMatrix(clipPlane) end
return m