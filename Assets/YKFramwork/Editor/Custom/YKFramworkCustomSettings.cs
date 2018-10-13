
using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEditor;

using BindType = ToLuaMenu.BindType;
using System.Reflection;
using FairyGUI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class SelfCustomSettings
{

    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    [CustomSettings.CustomStaticClass]
    public static List<Type> staticClassTypes = new List<Type>
    {        
        typeof(UnityEngine.Application),
        typeof(UnityEngine.Time),
        typeof(UnityEngine.Screen),
        typeof(UnityEngine.SleepTimeout),
        typeof(UnityEngine.Input),
        typeof(UnityEngine.Resources),
        typeof(UnityEngine.Physics),
        typeof(UnityEngine.RenderSettings),
        typeof(UnityEngine.QualitySettings),
        typeof(UnityEngine.GL),
        typeof(UnityEngine.Graphics),
        typeof(UnityEngine.Random),
        typeof(EventDef),

    };
    [CustomSettings.CustomDelegate]
    public static List<DelegateType> customDelegateList = new List<DelegateType>()
    {
        CustomSettings._DT(typeof(System.Action<int>)),
        CustomSettings._DT(typeof(System.Action<string>)),
        CustomSettings._DT(typeof(System.Comparison<int>)),
        CustomSettings._DT(typeof(System.Func<int, int>)),
        CustomSettings._DT(typeof(System.Func<GObject, bool>)),
        CustomSettings._DT(typeof(EventCallback1)),
        CustomSettings._DT(typeof(EventCallback0)),
        CustomSettings._DT(typeof(EventDispatcherNode.EventListenerDele)),
        CustomSettings._DT(typeof(System.Action<GetGPSFinishEvent.GPSLocationInfo>)),
        CustomSettings._DT(typeof(Action<UnityEngine.Object>)),
        CustomSettings._DT(typeof(Action<LocationAddressResultData>)),
        CustomSettings._DT(typeof(Action<UnityEngine.AssetBundle>)),
        CustomSettings._DT(typeof(Action<AsynTask>)),
        CustomSettings._DT(typeof(Action<List<TaskBase>>)),
        CustomSettings._DT(typeof(ListItemRenderer)),
        CustomSettings._DT(typeof(ListItemProvider)),
    };
    [CustomSettings.CustomType]
    //在这里添加你要导出注册到lua的类型列表
    public static BindType[] customTypeList =
    {
        CustomSettings._GT(typeof(DG.Tweening.Core.TweenerCore<float,float,DG.Tweening.Plugins.Options.FloatOptions>)),
        CustomSettings._GT(typeof(List<string>)),
        CustomSettings._GT(typeof(List<int>)),
        CustomSettings._GT(typeof(List<Transform>)),
        CustomSettings._GT(typeof(List<GameObject>)),
        CustomSettings._GT(typeof(List <TaskBase>)),
        CustomSettings._GT(typeof(System.Collections.Generic.List<FairyGUI.Controller>)),
        CustomSettings._GT(typeof(Dictionary<string, string>)),
        CustomSettings._GT(typeof(Behaviour)),
        CustomSettings._GT(typeof(MonoBehaviour)),
        CustomSettings._GT(typeof(GameObject)),
        CustomSettings._GT(typeof(TrackedReference)),
        CustomSettings._GT(typeof(Application)),
        CustomSettings._GT(typeof(Physics)),
        CustomSettings._GT(typeof(Collider)),
        CustomSettings._GT(typeof(Time)),
        CustomSettings._GT(typeof(Texture)),
        CustomSettings._GT(typeof(Texture2D)),
        CustomSettings._GT(typeof(Text)),
        CustomSettings._GT(typeof(Shader)),
        CustomSettings._GT(typeof(Renderer)),
        CustomSettings._GT(typeof(WWW)),
        CustomSettings._GT(typeof(Screen)),
        CustomSettings._GT(typeof(CameraClearFlags)),
        CustomSettings._GT(typeof(AudioClip)),
        CustomSettings._GT(typeof(AssetBundle)),
        CustomSettings._GT(typeof(ParticleSystem)),
        CustomSettings._GT(typeof(AsyncOperation)),
        CustomSettings._GT(typeof(LightType)),
        CustomSettings._GT(typeof(SleepTimeout)),
        CustomSettings._GT(typeof(Animator)),
        CustomSettings._GT(typeof(Input)),
        CustomSettings._GT(typeof(KeyCode)),
        CustomSettings._GT(typeof(SkinnedMeshRenderer)),
        CustomSettings._GT(typeof(Space)),
        CustomSettings._GT(typeof(LoadingUI)),

        CustomSettings._GT(typeof(MeshRenderer)),
        CustomSettings._GT(typeof(MeshFilter)),
        CustomSettings._GT(typeof(Mesh)),
        CustomSettings._GT(typeof(BoxCollider)),
        CustomSettings._GT(typeof(MeshCollider)),
        CustomSettings._GT(typeof(SphereCollider)),
        CustomSettings._GT(typeof(CharacterController)),
        CustomSettings._GT(typeof(CapsuleCollider)),
        CustomSettings._GT(typeof(Animation)),
        CustomSettings._GT(typeof(UnityEngine.Object)),
        CustomSettings._GT(typeof(AnimationClip)),
        CustomSettings._GT(typeof(AnimationState)),
        CustomSettings._GT(typeof(AnimationBlendMode)),
        CustomSettings._GT(typeof(QueueMode)),
        CustomSettings._GT(typeof(PlayMode)),
        CustomSettings._GT(typeof(WrapMode)),
        CustomSettings._GT(typeof(UnityEngine.SceneManagement.SceneManager)),

        CustomSettings._GT(typeof(QualitySettings)),
        CustomSettings._GT(typeof(RenderSettings)),
        CustomSettings._GT(typeof(BlendWeights)),
        CustomSettings._GT(typeof(RenderTexture)),
        CustomSettings._GT(typeof(Resources)),

        CustomSettings._GT(typeof(PlayerPrefs)),
        CustomSettings._GT(typeof(AppConst)),
        CustomSettings._GT(typeof(EventContext)),
        CustomSettings._GT(typeof(EventDispatcher)),
        CustomSettings._GT(typeof(EventListener)),
        CustomSettings._GT(typeof(InputEvent)),
        CustomSettings._GT(typeof(DisplayObject)),
        CustomSettings._GT(typeof(Container)),
        CustomSettings._GT(typeof(Stage)),
        CustomSettings._GT(typeof(Controller)),
        CustomSettings._GT(typeof(GObject)),
        CustomSettings._GT(typeof(GGraph)),
        CustomSettings._GT(typeof(GGroup)),
        CustomSettings._GT(typeof(GImage)),
        CustomSettings._GT(typeof(GLoader)),
        CustomSettings._GT(typeof(FairyGUIGLoaderExtension)),
        CustomSettings._GT(typeof(FairyGUI.Stats)),
        CustomSettings._GT(typeof(GMovieClip)),
        CustomSettings._GT(typeof(TextFormat)),
        CustomSettings._GT(typeof(GTextField)),
        CustomSettings._GT(typeof(GRichTextField)),
        CustomSettings._GT(typeof(GTextInput)),
        CustomSettings._GT(typeof(InputTextField)),
        CustomSettings._GT(typeof(GComponent)),
        CustomSettings._GT(typeof(GList)),
        CustomSettings._GT(typeof(GRoot)),
        CustomSettings._GT(typeof(GLabel)),
        CustomSettings._GT(typeof(GButton)),
        CustomSettings._GT(typeof(GComboBox)),
        CustomSettings._GT(typeof(GProgressBar)),
        CustomSettings._GT(typeof(GSlider)),
        CustomSettings._GT(typeof(PopupMenu)),
        CustomSettings._GT(typeof(ScrollPane)),
        CustomSettings._GT(typeof(Transition)),
        CustomSettings._GT(typeof(UIPackage)),
        CustomSettings._GT(typeof(Window)),
        CustomSettings._GT(typeof(GObjectPool)),
        CustomSettings._GT(typeof(Relations)),
        CustomSettings._GT(typeof(RelationType)),
        CustomSettings._GT(typeof(Timers)),
        CustomSettings._GT(typeof(FontManager)),
        CustomSettings._GT(typeof(GoWrapper)),
        CustomSettings._GT(typeof(FairyGUI.CaptureCamera)),
        CustomSettings._GT(typeof(FairyGUI.LuaUIHelper)),
        CustomSettings._GT(typeof(FairyGUI.GLuaButton)),
        CustomSettings._GT(typeof(FairyGUI.DragDropComManger)),
        CustomSettings._GT(typeof(FairyGUI.DragDropManager)),

        CustomSettings._GT(typeof(NTexture)),
        CustomSettings._GT(typeof(UIConfig)),
        CustomSettings._GT(typeof(TweenUtils)),
        CustomSettings._GT(typeof(EventDef)),
        CustomSettings._GT(typeof(EventDispatcherNode)),
        CustomSettings._GT(typeof(EventData)),
        CustomSettings._GT(typeof(EventDataLua)),
        CustomSettings._GT(typeof(UIMgr)),
        CustomSettings._GT(typeof(SceneBase)),
        CustomSettings._GT(typeof(BaseUI)),
        CustomSettings._GT(typeof(ResMgr)),
        CustomSettings._GT(typeof(ABMgr)),
        CustomSettings._GT(typeof(LoadGroupProgress)),
        CustomSettings._GT(typeof(GroupProgress)),
        CustomSettings._GT(typeof(ResGroupLoadError)),
        CustomSettings._GT(typeof(ResLoadGroupEvent)),
        CustomSettings._GT(typeof(GamUtil)),
        CustomSettings._GT(typeof(GameMode)),
        CustomSettings._GT(typeof(AsynTask)),
        CustomSettings._GT(typeof(AutoLoadAsset)),
        CustomSettings._GT(typeof(LoadGroup)),
        CustomSettings._GT(typeof(SceneLoadTask)),
        CustomSettings._GT(typeof(SceneMgr)),
        CustomSettings._GT(typeof(ComUtil)),
        CustomSettings._GT(typeof(GetGPSFinishEvent)),
        CustomSettings._GT(typeof(GetGPSFinishEvent.GPSLocationInfo)),
        CustomSettings._GT(typeof(GameFlag)),
        CustomSettings._GT(typeof(SoundMgr)),
        CustomSettings._GT(typeof(BGMMgr)),
        CustomSettings._GT(typeof(Initialization)),
        CustomSettings._GT(typeof(RuntimePlatform)),
        CustomSettings._GT(typeof(LocationAddressResultData)),
        CustomSettings._GT(typeof(LocationAddress)),
        CustomSettings._GT(typeof(LocationAddressComponent)),
        CustomSettings._GT(typeof(LuaMgr)),
        CustomSettings._GT(typeof(FairyGUI.DynamicFont)),
        CustomSettings._GT(typeof(ABInfo)),
        CustomSettings._GT(typeof(VerInfo)),
        CustomSettings._GT(typeof(ComparisonInfo)),
        CustomSettings._GT(typeof(TaskBase)),
        CustomSettings._GT(typeof(ParallelTask)),
        CustomSettings._GT(typeof(StateMachine)),
        CustomSettings._GT(typeof(LocalGameCfgData)),
        CustomSettings._GT(typeof(SystemInfo)),
        CustomSettings._GT(typeof(RemotelyVersionInfo)),
        CustomSettings._GT(typeof(RemotelyVersionInfo.RemotelyInfo)),
        CustomSettings._GT(typeof(DownAPKTask)),
        CustomSettings._GT(typeof(GameCfgMgr)),
        CustomSettings._GT(typeof(WXConstant)),
        CustomSettings._GT(typeof(HotUpdateRessMgr)),
        CustomSettings._GT(typeof(HotUpdateRessMgr.DecompressionOrDownInfo)),
        CustomSettings._GT(typeof(NGraphics)),
        
        CustomSettings._GT(typeof(YKSupportLua)),
        CustomSettings._GT(typeof(YKSupportLua.LuaSceneBase)),
        CustomSettings._GT(typeof(YKSupportLua.LuaStateBase)),
        CustomSettings._GT(typeof(YKSupportLua.LuaTaskBase)),
        CustomSettings._GT(typeof(YKSupportLua.LuaUBBParser)),
        CustomSettings._GT(typeof(YKSupportLua.LuaWindow)),
        CustomSettings._GT(typeof(YKSupportLua.LuaMode)),
        CustomSettings._GT(typeof(GLuaComponent)),
        CustomSettings._GT(typeof(GLuaButton)),
        CustomSettings._GT(typeof(GLuaComboBox)),
        CustomSettings._GT(typeof(GLuaLabel)),
        CustomSettings._GT(typeof(GLuaProgressBar)),
        CustomSettings._GT(typeof(GLuaSlider)),
        CustomSettings._GT(typeof(EventListenerMgr)),
        CustomSettings._GT(typeof(KeyCode)),
        CustomSettings._GT(typeof(TextAsset))
    };
    [CustomSettings.CustomDynamic]
    public static List<Type> dynamicList = new List<Type>()
    {
        typeof(MeshRenderer),
#if !UNITY_5_4_OR_NEWER
        typeof(ParticleEmitter),
        typeof(ParticleRenderer),
        typeof(ParticleAnimator),
#endif

        typeof(BoxCollider),
        typeof(MeshCollider),
        typeof(SphereCollider),
        typeof(CharacterController),
        typeof(CapsuleCollider),

        typeof(Animation),
        typeof(AnimationClip),
        typeof(AnimationState),

        typeof(BlendWeights),
        typeof(RenderTexture),
        typeof(Rigidbody),
    };

    [CustomSettings.CustomOut]
    //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
    //使用方法参见例子14
    public static List<Type> outList = new List<Type>()
    {
        
    };
    [CustomSettings.CustomSealed]
    //ngui优化，下面的类没有派生类，可以作为sealed class
    public static List<Type> sealedList = new List<Type>()
    {
        typeof(Transform),
        typeof(UIMgr),
        typeof(ResMgr),
        typeof(ABMgr),
        typeof(GRoot),
    };
}
