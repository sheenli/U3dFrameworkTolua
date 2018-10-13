---@class UnityEngine.ParticleSystem : UnityEngine.Component
---@field public isPlaying bool
---@field public isEmitting bool
---@field public isStopped bool
---@field public isPaused bool
---@field public time float
---@field public particleCount int
---@field public randomSeed uint
---@field public useAutoRandomSeed bool
---@field public main UnityEngine.ParticleSystem.MainModule
---@field public emission UnityEngine.ParticleSystem.EmissionModule
---@field public shape UnityEngine.ParticleSystem.ShapeModule
---@field public velocityOverLifetime UnityEngine.ParticleSystem.VelocityOverLifetimeModule
---@field public limitVelocityOverLifetime UnityEngine.ParticleSystem.LimitVelocityOverLifetimeModule
---@field public inheritVelocity UnityEngine.ParticleSystem.InheritVelocityModule
---@field public forceOverLifetime UnityEngine.ParticleSystem.ForceOverLifetimeModule
---@field public colorOverLifetime UnityEngine.ParticleSystem.ColorOverLifetimeModule
---@field public colorBySpeed UnityEngine.ParticleSystem.ColorBySpeedModule
---@field public sizeOverLifetime UnityEngine.ParticleSystem.SizeOverLifetimeModule
---@field public sizeBySpeed UnityEngine.ParticleSystem.SizeBySpeedModule
---@field public rotationOverLifetime UnityEngine.ParticleSystem.RotationOverLifetimeModule
---@field public rotationBySpeed UnityEngine.ParticleSystem.RotationBySpeedModule
---@field public externalForces UnityEngine.ParticleSystem.ExternalForcesModule
---@field public noise UnityEngine.ParticleSystem.NoiseModule
---@field public collision UnityEngine.ParticleSystem.CollisionModule
---@field public trigger UnityEngine.ParticleSystem.TriggerModule
---@field public subEmitters UnityEngine.ParticleSystem.SubEmittersModule
---@field public textureSheetAnimation UnityEngine.ParticleSystem.TextureSheetAnimationModule
---@field public lights UnityEngine.ParticleSystem.LightsModule
---@field public trails UnityEngine.ParticleSystem.TrailModule
---@field public customData UnityEngine.ParticleSystem.CustomDataModule
local m = {}
---@param particles table
---@param size int
function m:SetParticles(particles, size) end
---@param particles table
---@return int
function m:GetParticles(particles) end
---@param customData table
---@param streamIndex UnityEngine.ParticleSystemCustomData
function m:SetCustomParticleData(customData, streamIndex) end
---@param customData table
---@param streamIndex UnityEngine.ParticleSystemCustomData
---@return int
function m:GetCustomParticleData(customData, streamIndex) end
---@param t float
---@param withChildren bool
---@param restart bool
function m:Simulate(t, withChildren, restart) end
function m:Play() end
---@param withChildren bool
function m:Stop(withChildren) end
function m:Pause() end
function m:Clear() end
---@return bool
function m:IsAlive() end
---@param count int
function m:Emit(count) end
return m