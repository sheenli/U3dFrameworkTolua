using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode.Custom;

[System.Serializable]
/// <summary>
/// Xcode导出时候的设置
/// </summary>
public class XcodeProjectSetting : ScriptableObject {
    
    //=================================================================================
    //所有key的常量
    //=================================================================================

    /// <summary>
    /// 工程根目录
    /// </summary>
    public const string PROJECT_ROOT = "$(PROJECT_DIR)/";

    /// <summary>
    /// Images.xcassets 默认的target
    /// </summary>

    public const string IMAGE_XCASSETS_DIRECTORY_NAME = "Unity-iPhone";

	/// <summary>
    /// LinkerFlags 所有链接标签
    /// </summary>
	public const string LINKER_FLAG_KEY            = "OTHER_LDFLAGS";
	public const string FRAMEWORK_SEARCH_PATHS_KEY = "FRAMEWORK_SEARCH_PATHS";
	public const string LIBRARY_SEARCH_PATHS_KEY   = "LIBRARY_SEARCH_PATHS";
	public const string ENABLE_BITCODE_KEY         = "ENABLE_BITCODE";
    
    /// <summary>
    /// infoplist 路径
    /// </summary>
    public const string INFO_PLIST_NAME = "Info.plist";

	/// <summary>
    /// 常用的key
    /// </summary>
	public const string URL_TYPES_KEY      = "CFBundleURLTypes";
	public const string URL_TYPE_ROLE_KEY  = "CFBundleTypeRole";
	public const string URL_IDENTIFIER_KEY = "CFBundleURLName";
	public const string URL_SCHEMES_KEY    = "CFBundleURLSchemes";

    public const string CLASS_PATH = "Classes/{0}";

    /// <summary>
    /// 开场动画
    /// </summary>
	public const string UI_LAUNCHI_IMAGES_KEY          = "UILaunchImages";
	public const string UI_LAUNCHI_STORYBOARD_NAME_KEY = "UILaunchStoryboardName~iphone";

	public const string ATS_KEY                    = "NSAppTransportSecurity";
	public const string ALLOWS_ARBITRARY_LOADS_KEY = "NSAllowsArbitraryLoads";

	public const string APPLICATION_QUERIES_SCHEMES_KEY = "LSApplicationQueriesSchemes";

	public const string STATUS_HIDDEN_KEY         = "UIStatusBarHidden";
	public const string STATUS_BAR_APPEARANCE_KEY = "UIViewControllerBasedStatusBarAppearance";

	//=================================================================================
	//各种常量key
	//=================================================================================

	/// <summary>
    /// 需要代入xcode的文件夹
    /// </summary>
	public string CopyDirectoryPath = "Assets/CopyToXcode";

	[System.Serializable]
	public class AddToInfoStringList 
	{
		public string Key;
		public string value;
	}

	public List<AddToInfoStringList> addStringKeyToPlist = new List<AddToInfoStringList> ();


    /// <summary>
    /// URL identifier  名字 以及每个的内容
    /// </summary>
    public List<URLIdentifierData> URLIdentifierList = new List<URLIdentifierData>();

    //URL identifier
    //public List<string> URLIdentifierList = new List<string>() { "Identifier.Identifier" };

    /// <summary>
    /// 需要关联的Framework
    /// </summary>
	public List<string> FrameworkList = new List<string>(){
		/*"Social.framework",*/
	};
    

    /// <summary>
    /// 需要添加的代码段
    /// </summary>
    public List<AddCodeData> AddCodeList = new List<AddCodeData>();

    /// <summary>
    /// 要添加的lib
    /// </summary>
    public List<string> LibList = new List<string>();

    /// <summary>
    /// 所有链接编译符
    /// </summary>
	public List<string> LinkerFlagArray = new List<string>
    {
        /*"-ObjC", "-all_load"*/ 
    };

    /// <summary>
    /// 工程要包含的一些特殊文件夹
    /// </summary>
	public List<string> FrameworkSearchPathArray = new List<string>{
		"$(inherited)",
		"$(PROJECT_DIR)/Frameworks"
	};

    [System.Serializable]
    public class AddCodeData
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName = "UnityAppController.mm";

        /// <summary>
        /// 开头需要添加的内容
        /// </summary>
        public string StartAddCode = "";

        /// <summary>
        /// 找到代码当中这段标示然后添加代码
        /// </summary>
        public List<AddCodeSet> addCodes = new List<AddCodeSet>();

        /// <summary>
        /// 需要替换的代码
        /// </summary>
        public List<ReplaceCodeSet> replaceCode = new List<ReplaceCodeSet>();
    }

    [System.Serializable]
    public class AddCodeSet
    {
        public string addFlag = "";

        public string AddCode = "";
    }

    [System.Serializable]
    public class ReplaceCodeSet
    {
        /// <summary>
        /// 以前的code
        /// </summary>
        public string oldCode = "";

        /// <summary>
        /// 新的代码
        /// </summary>
        public string newCode = "";
    }

    /// <summary>
    /// 需要导出的文件以及文件需要特殊的flags
    /// </summary>
    [System.Serializable]
	public class CompilerFlagsSet{
		public string Flags;
		public List<string> TargetPathList;

		public CompilerFlagsSet(string flags, List<string> targetPathList){
			Flags = flags;
			TargetPathList = targetPathList;
		}
	}

    [System.Serializable]
    public class URLIdentifierData
    {
        public string name;
        public List<string> URLSchemes;
        public URLIdentifierData()
        {
            name = "";
            URLSchemes = new List<string>();
        }
    }


    public List<CompilerFlagsSet> CompilerFlagsSetList = new List<CompilerFlagsSet> () {
		/*new CompilerFlagsSet ("-fno-objc-arc", new List<string> () {
			"Plugin/Plugin.mm"
		})*/ //初期設定例
	};

	/// <summary>
    /// 白名单设置
    /// </summary>
	public List<string>ApplicationQueriesSchemes = new List<string>(){

	};

        /// <summary>
        /// BitCode 是否开启
        /// </summary>
    public bool EnableBitCode = false;

    /// <summary>
    /// NSAppTransportSecurity 支持https兼容http
    /// </summary>
    public bool EnableATS = false;

	/// <summary>
    /// 显示进度条
    /// </summary>
	public bool EnableStatusBar = false;

	/// <summary>
    /// 是不是删除开场动画
    /// </summary>
	public bool NeedToDeleteLaunchiImagesKey = true;

}
/// Usage:
/// 
/// [System.Serializable]
/// class MyDictionary : SerializableDictionary<int, GameObject> {}
/// public MyDictionary dic;
///
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    // We save the keys and values in two lists because Unity does understand those.
    [SerializeField]
    private List<TKey> _keys;
    [SerializeField]
    private List<TValue> _values;

    // Before the serialization we fill these lists
    public void OnBeforeSerialize()
    {
        _keys = new List<TKey>(this.Count);
        _values = new List<TValue>(this.Count);
        foreach (var kvp in this)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }

    // After the serialization we create the dictionary from the two lists
    public void OnAfterDeserialize()
    {
        this.Clear();
        int count = Mathf.Min(_keys.Count, _values.Count);
        for (int i = 0; i < count; ++i)
        {
            this.Add(_keys[i], _values[i]);
        }
    }
}