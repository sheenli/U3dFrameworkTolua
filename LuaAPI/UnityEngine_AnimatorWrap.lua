---@class UnityEngine.Animator : UnityEngine.Behaviour
---@field public isOptimizable bool
---@field public isHuman bool
---@field public hasRootMotion bool
---@field public humanScale float
---@field public isInitialized bool
---@field public deltaPosition UnityEngine.Vector3
---@field public deltaRotation UnityEngine.Quaternion
---@field public velocity UnityEngine.Vector3
---@field public angularVelocity UnityEngine.Vector3
---@field public rootPosition UnityEngine.Vector3
---@field public rootRotation UnityEngine.Quaternion
---@field public applyRootMotion bool
---@field public linearVelocityBlending bool
---@field public updateMode UnityEngine.AnimatorUpdateMode
---@field public hasTransformHierarchy bool
---@field public gravityWeight float
---@field public bodyPosition UnityEngine.Vector3
---@field public bodyRotation UnityEngine.Quaternion
---@field public stabilizeFeet bool
---@field public layerCount int
---@field public parameters table
---@field public parameterCount int
---@field public feetPivotActive float
---@field public pivotWeight float
---@field public pivotPosition UnityEngine.Vector3
---@field public isMatchingTarget bool
---@field public speed float
---@field public targetPosition UnityEngine.Vector3
---@field public targetRotation UnityEngine.Quaternion
---@field public cullingMode UnityEngine.AnimatorCullingMode
---@field public playbackTime float
---@field public recorderStartTime float
---@field public recorderStopTime float
---@field public recorderMode UnityEngine.AnimatorRecorderMode
---@field public runtimeAnimatorController UnityEngine.RuntimeAnimatorController
---@field public hasBoundPlayables bool
---@field public avatar UnityEngine.Avatar
---@field public playableGraph UnityEngine.Experimental.Director.PlayableGraph
---@field public layersAffectMassCenter bool
---@field public leftFeetBottomHeight float
---@field public rightFeetBottomHeight float
---@field public logWarnings bool
---@field public fireEvents bool
local m = {}
---@param name string
---@return float
function m:GetFloat(name) end
---@param name string
---@param value float
function m:SetFloat(name, value) end
---@param name string
---@return bool
function m:GetBool(name) end
---@param name string
---@param value bool
function m:SetBool(name, value) end
---@param name string
---@return int
function m:GetInteger(name) end
---@param name string
---@param value int
function m:SetInteger(name, value) end
---@param name string
function m:SetTrigger(name) end
---@param name string
function m:ResetTrigger(name) end
---@param name string
---@return bool
function m:IsParameterControlledByCurve(name) end
---@param goal UnityEngine.AvatarIKGoal
---@return UnityEngine.Vector3
function m:GetIKPosition(goal) end
---@param goal UnityEngine.AvatarIKGoal
---@param goalPosition UnityEngine.Vector3
function m:SetIKPosition(goal, goalPosition) end
---@param goal UnityEngine.AvatarIKGoal
---@return UnityEngine.Quaternion
function m:GetIKRotation(goal) end
---@param goal UnityEngine.AvatarIKGoal
---@param goalRotation UnityEngine.Quaternion
function m:SetIKRotation(goal, goalRotation) end
---@param goal UnityEngine.AvatarIKGoal
---@return float
function m:GetIKPositionWeight(goal) end
---@param goal UnityEngine.AvatarIKGoal
---@param value float
function m:SetIKPositionWeight(goal, value) end
---@param goal UnityEngine.AvatarIKGoal
---@return float
function m:GetIKRotationWeight(goal) end
---@param goal UnityEngine.AvatarIKGoal
---@param value float
function m:SetIKRotationWeight(goal, value) end
---@param hint UnityEngine.AvatarIKHint
---@return UnityEngine.Vector3
function m:GetIKHintPosition(hint) end
---@param hint UnityEngine.AvatarIKHint
---@param hintPosition UnityEngine.Vector3
function m:SetIKHintPosition(hint, hintPosition) end
---@param hint UnityEngine.AvatarIKHint
---@return float
function m:GetIKHintPositionWeight(hint) end
---@param hint UnityEngine.AvatarIKHint
---@param value float
function m:SetIKHintPositionWeight(hint, value) end
---@param lookAtPosition UnityEngine.Vector3
function m:SetLookAtPosition(lookAtPosition) end
---@param weight float
---@param bodyWeight float
---@param headWeight float
---@param eyesWeight float
function m:SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight) end
---@param humanBoneId UnityEngine.HumanBodyBones
---@param rotation UnityEngine.Quaternion
function m:SetBoneLocalRotation(humanBoneId, rotation) end
---@param layerIndex int
---@return string
function m:GetLayerName(layerIndex) end
---@param layerName string
---@return int
function m:GetLayerIndex(layerName) end
---@param layerIndex int
---@return float
function m:GetLayerWeight(layerIndex) end
---@param layerIndex int
---@param weight float
function m:SetLayerWeight(layerIndex, weight) end
---@param layerIndex int
---@return UnityEngine.AnimatorStateInfo
function m:GetCurrentAnimatorStateInfo(layerIndex) end
---@param layerIndex int
---@return UnityEngine.AnimatorStateInfo
function m:GetNextAnimatorStateInfo(layerIndex) end
---@param layerIndex int
---@return UnityEngine.AnimatorTransitionInfo
function m:GetAnimatorTransitionInfo(layerIndex) end
---@param layerIndex int
---@return int
function m:GetCurrentAnimatorClipInfoCount(layerIndex) end
---@param layerIndex int
---@return table
function m:GetCurrentAnimatorClipInfo(layerIndex) end
---@param layerIndex int
---@return int
function m:GetNextAnimatorClipInfoCount(layerIndex) end
---@param layerIndex int
---@return table
function m:GetNextAnimatorClipInfo(layerIndex) end
---@param layerIndex int
---@return bool
function m:IsInTransition(layerIndex) end
---@param index int
---@return UnityEngine.AnimatorControllerParameter
function m:GetParameter(index) end
---@param matchPosition UnityEngine.Vector3
---@param matchRotation UnityEngine.Quaternion
---@param targetBodyPart UnityEngine.AvatarTarget
---@param weightMask UnityEngine.MatchTargetWeightMask
---@param startNormalizedTime float
---@param targetNormalizedTime float
function m:MatchTarget(matchPosition, matchRotation, targetBodyPart, weightMask, startNormalizedTime, targetNormalizedTime) end
---@param completeMatch bool
function m:InterruptMatchTarget(completeMatch) end
---@param stateName string
---@param transitionDuration float
---@param layer int
function m:CrossFadeInFixedTime(stateName, transitionDuration, layer) end
---@param stateName string
---@param transitionDuration float
---@param layer int
function m:CrossFade(stateName, transitionDuration, layer) end
---@param stateName string
---@param layer int
function m:PlayInFixedTime(stateName, layer) end
---@param stateName string
---@param layer int
function m:Play(stateName, layer) end
---@param targetIndex UnityEngine.AvatarTarget
---@param targetNormalizedTime float
function m:SetTarget(targetIndex, targetNormalizedTime) end
---@param humanBoneId UnityEngine.HumanBodyBones
---@return UnityEngine.Transform
function m:GetBoneTransform(humanBoneId) end
function m:StartPlayback() end
function m:StopPlayback() end
---@param frameCount int
function m:StartRecording(frameCount) end
function m:StopRecording() end
---@param layerIndex int
---@param stateID int
---@return bool
function m:HasState(layerIndex, stateID) end
---@param name string
---@return int
function m.StringToHash(name) end
---@param deltaTime float
function m:Update(deltaTime) end
function m:Rebind() end
function m:ApplyBuiltinRootMotion() end
return m