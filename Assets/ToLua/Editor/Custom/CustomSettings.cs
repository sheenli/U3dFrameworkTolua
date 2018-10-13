using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEditor;

using BindType = ToLuaMenu.BindType;
using System.Reflection;
using System.Linq;



public static class CustomSettings
{
    public class CustomStaticClassAttribute : System.Attribute
    {

    }

    public class CustomDelegateAttribute : System.Attribute
    { }

    public class CustomTypeAttribute : System.Attribute
    {

    }

    public class CustomDynamicAttribute : System.Attribute
    {

    }

    public class CustomOutAttribute : System.Attribute
    {

    }
    public class CustomSealedAttribute : System.Attribute
    {

    }
    public static string saveBaseDir = Application.dataPath + "/Source/BaseGenerate/";
    public static string saveDir = Application.dataPath + "/Source/Generate/";    
    public static string toluaBaseType = Application.dataPath + "/ToLua/BaseType/";
    public static string baseLuaDir = Application.dataPath + "/Tolua/Lua/";
    public static string injectionFilesPath = Application.dataPath + "/ToLua/Injection/";

    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    [CustomStaticClass]
    public static List<Type> _staticClassTypes = new List<Type>
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
    };

    
    public static List<Type> staticClassTypes
    {
        get
        {
            List<Type> list = GetcustomTypesList<CustomStaticClassAttribute,Type>();
            return list;
        }
    }

    public static List<T2> GetcustomTypesList<T, T2>() where T : System.Attribute where T2 : class
    {
        List<T2> list = new List<T2>();
        var assemblies = Assembly.GetExecutingAssembly().GetTypes();
        foreach (Type t in assemblies)
        {
            var fields = t.GetFields(BindingFlags.Static
                | BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.DeclaredOnly);
            if (typeof(T2) is Type)
            {
                if (t.IsDefined(typeof(T), false))
                {
                    if (!list.Contains(t as T2))
                        list.Add(t as T2);
                }
            }

            foreach (var f in fields)
            {
                if (f.IsDefined(typeof(T), false))
                {
                    object o = f.GetValue(null);
                    if (o is T2)
                    {
                        if (!list.Contains(o as T2))
                            list.Add(o as T2);
                    }
                    else if (o is IEnumerable<T2>)
                    {
                        IEnumerable<T2> types = o as IEnumerable<T2>;
                        foreach (T2 tt in types)
                        {
                            if (!list.Contains(tt))
                                list.Add(tt);
                        }
                    }
                    else if (o is T2[])
                    {
                        T2[] types = o as T2[];
                        foreach (T2 tt in types)
                        {
                            if (!list.Contains(tt))
                                list.Add(tt);
                        }
                    }
                }
            }

        }
        //Debug.LogError(list.Count + "," + typeof(T) + "," + typeof(T2));
        return list;
    }

    [CustomDelegate]
    //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
    public static List<DelegateType> _customDelegateList = new List<DelegateType>()
    {        
        _DT(typeof(Action)),                
        _DT(typeof(UnityEngine.Events.UnityAction)),
        _DT(typeof(System.Predicate<int>)),
        _DT(typeof(System.Action<int>)),
        _DT(typeof(System.Comparison<int>)),
        _DT(typeof(System.Func<int, int>)),
    };

    public static DelegateType[] customDelegateList
    {
        get
        {
            List<DelegateType> types = GetcustomTypesList<CustomDelegateAttribute, DelegateType>();
            List<DelegateType> list = new List<DelegateType>();
            foreach (DelegateType bt in types)
            {
                if (!list.Exists(a => { return a.type == bt.type; }))
                {
                    list.Add(bt);
                }
            }
            return list.ToArray();
        }
    }
    [CustomSettings.CustomType]
    //在这里添加你要导出注册到lua的类型列表
    public static BindType[] _customTypeList =
    {                
        //------------------------为例子导出--------------------------------
        //_GT(typeof(TestEventListener)),
        //_GT(typeof(TestProtol)),
        //_GT(typeof(TestAccount)),
        //_GT(typeof(Dictionary<int, TestAccount>)).SetLibName("AccountMap"),
        //_GT(typeof(KeyValuePair<int, TestAccount>)),
        //_GT(typeof(Dictionary<int, TestAccount>.KeyCollection)),
        //_GT(typeof(Dictionary<int, TestAccount>.ValueCollection)),
        //_GT(typeof(TestExport)),
        //_GT(typeof(TestExport.Space)),
        //-------------------------------------------------------------------        
                        
        _GT(typeof(LuaInjectionStation)),
        _GT(typeof(InjectType)),
        _GT(typeof(Debugger)).SetNameSpace(null),          

#if USING_DOTWEENING
        _GT(typeof(DG.Tweening.DOTween)),
        _GT(typeof(DG.Tweening.Tween)).SetBaseType(typeof(System.Object)).AddExtendType(typeof(DG.Tweening.TweenExtensions)),
        _GT(typeof(DG.Tweening.Sequence)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.Tweener)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.LoopType)),
        _GT(typeof(DG.Tweening.PathMode)),
        _GT(typeof(DG.Tweening.PathType)),
        _GT(typeof(DG.Tweening.RotateMode)),
        _GT(typeof(Component)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Transform)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Light)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Material)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Rigidbody)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Camera)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(AudioSource)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        //_GT(typeof(LineRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        //_GT(typeof(TrailRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),    
#else
                                         
        _GT(typeof(Component)),
        _GT(typeof(Transform)),
        _GT(typeof(Material)),
        _GT(typeof(Light)),
        _GT(typeof(Rigidbody)),
        _GT(typeof(Camera)),
        _GT(typeof(AudioSource)),
        //_GT(typeof(LineRenderer))
        //_GT(typeof(TrailRenderer))
#endif
      
        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),        
        _GT(typeof(GameObject)),
        _GT(typeof(TrackedReference)),
        _GT(typeof(Application)),
        _GT(typeof(Physics)),
        _GT(typeof(Collider)),
        _GT(typeof(Time)),        
        _GT(typeof(Texture)),
        _GT(typeof(Texture2D)),
        _GT(typeof(Shader)),        
        _GT(typeof(Renderer)),
        _GT(typeof(WWW)),
        _GT(typeof(Screen)),        
        _GT(typeof(CameraClearFlags)),
        _GT(typeof(AudioClip)),        
        _GT(typeof(AssetBundle)),
        _GT(typeof(ParticleSystem)),
        _GT(typeof(AsyncOperation)).SetBaseType(typeof(System.Object)),        
        _GT(typeof(LightType)),
        _GT(typeof(SleepTimeout)),
#if UNITY_5_3_OR_NEWER && !UNITY_5_6_OR_NEWER
        _GT(typeof(UnityEngine.Experimental.Director.DirectorPlayer)),
#endif
        _GT(typeof(Animator)),
        _GT(typeof(Input)),
        _GT(typeof(KeyCode)),
        _GT(typeof(SkinnedMeshRenderer)),
        _GT(typeof(Space)),      
       

        _GT(typeof(MeshRenderer)),
#if !UNITY_5_4_OR_NEWER
        _GT(typeof(ParticleEmitter)),
        _GT(typeof(ParticleRenderer)),
        _GT(typeof(ParticleAnimator)), 
#endif

        _GT(typeof(BoxCollider)),
        _GT(typeof(MeshCollider)),
        _GT(typeof(SphereCollider)),        
        _GT(typeof(CharacterController)),
        _GT(typeof(CapsuleCollider)),
        
        _GT(typeof(Animation)),        
        _GT(typeof(AnimationClip)).SetBaseType(typeof(UnityEngine.Object)),        
        _GT(typeof(AnimationState)),
        _GT(typeof(AnimationBlendMode)),
        _GT(typeof(QueueMode)),  
        _GT(typeof(PlayMode)),
        _GT(typeof(WrapMode)),

        _GT(typeof(QualitySettings)),
        _GT(typeof(RenderSettings)),                                                   
        _GT(typeof(BlendWeights)),           
        _GT(typeof(RenderTexture)),
        _GT(typeof(Resources)),     
        _GT(typeof(LuaProfiler)),
    };

    public static BindType[] customTypeList 
    {
        get
        {
            List<BindType> binds = GetcustomTypesList<CustomTypeAttribute, BindType>();
            List<BindType> list =new List<BindType>();
            foreach (BindType bt in binds)
            {
                if (!list.Exists(a => { return a.type == bt.type; }))
                {
                    list.Add(bt);
                }
            }
            return list.ToArray();
        }
    }
    public static List<Type> dynamicList
    {
        get
        {
            return GetcustomTypesList<CustomDynamicAttribute, Type>();
        }
    }

    [CustomDynamic]
    public static List<Type> _dynamicList = new List<Type>()
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

    //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
    //使用方法参见例子14
    [CustomOut]
    public static List<Type> _outList = new List<Type>()
    {
        
    };

    public static List<Type> outList
    {
        get
        {
            return GetcustomTypesList<CustomOutAttribute, Type>();
        }
    }

    [CustomSealed]
    //ngui优化，下面的类没有派生类，可以作为sealed class
    public static List<Type> _sealedList = new List<Type>()
    {
        /*typeof(Transform),
        typeof(UIRoot),
        typeof(UICamera),
        typeof(UIViewport),
        typeof(UIPanel),
        typeof(UILabel),
        typeof(UIAnchor),
        typeof(UIAtlas),
        typeof(UIFont),
        typeof(UITexture),
        typeof(UISprite),
        typeof(UIGrid),
        typeof(UITable),
        typeof(UIWrapGrid),
        typeof(UIInput),
        typeof(UIScrollView),
        typeof(UIEventListener),
        typeof(UIScrollBar),
        typeof(UICenterOnChild),
        typeof(UIScrollView),        
        typeof(UIButton),
        typeof(UITextList),
        typeof(UIPlayTween),
        typeof(UIDragScrollView),
        typeof(UISpriteAnimation),
        typeof(UIWrapContent),
        typeof(TweenWidth),
        typeof(TweenAlpha),
        typeof(TweenColor),
        typeof(TweenRotation),
        typeof(TweenPosition),
        typeof(TweenScale),
        typeof(TweenHeight),
        typeof(TypewriterEffect),
        typeof(UIToggle),
        typeof(Localization),*/
    };

    public static List<Type> sealedList
    {
        get
        {
            return GetcustomTypesList<CustomSealedAttribute, Type>();
        }
    }

    public static BindType _GT(Type t)
    {
        return new BindType(t);
    }

    public static DelegateType _DT(Type t)
    {
        return new DelegateType(t);
    }    


    [MenuItem("Lua/Attach Profiler", false, 151)]
    static void AttachProfiler()
    {
        if (!Application.isPlaying)
        {
            EditorUtility.DisplayDialog("警告", "请在运行时执行此功能", "确定");
            return;
        }

        LuaClient.Instance.AttachProfiler();
    }

    [MenuItem("Lua/Detach Profiler", false, 152)]
    static void DetachProfiler()
    {
        if (!Application.isPlaying)
        {            
            return;
        }

        LuaClient.Instance.DetachProfiler();
    }
}
