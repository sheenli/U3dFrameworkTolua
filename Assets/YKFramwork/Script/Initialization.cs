using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Initialization : EventDispatcherNode
{
    public Canvas canvas;
    public UnityEngine.UI.Text verText = null;
    public GameObject mLogo = null;

    public static Initialization Instance
    {
        get;
        private set;
    }

    [Header("设计分辨X")]
    public int designResolutionX = 1242;
    [Header("设计分辨Y")]
    public int designResolutionY = 2208;

    [Header("当前是不是debug环境")]
    [SerializeField]
    /// <summary>
    /// 当前是不是debug环境
    /// </summary>
    public bool DebugMode = true;

    public void Awake()
    {
        Instance = this;
        /*Screen.orientation = ScreenOrientation.Landscape;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;*/
        Application.targetFrameRate = 30;
        DebugMode = DebugMode && Application.isEditor;
        AppConst.DebugMode = DebugMode;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);
        gameObject.AddComponent<LogicCore>();
        gameObject.AddComponent<ABMgr>();
        gameObject.AddComponent<ResMgr>();
        gameObject.AddComponent<GameMode>();
        gameObject.AddComponent<SoundMgr>();
        gameObject.AddComponent<BGMMgr>();
        gameObject.AddComponent<SceneMgr>();
        gameObject.AddComponent<UIMgr>();
        ResMgr.Intstance.Init();
        GameCfgMgr.Instance.Init();
        UIMgr.Instance.Init(designResolutionX, designResolutionY);
        SceneMgr.Instance.GotoScene("entrance", typeof(EntranceScene),null);
        
    }

    public override void OnDestroy()
    {
        LuaMgr.Instance.OnDestroy();
    }

    public void Update()
    {
        if(verText != null)
        {
            verText.text = GameCfgMgr.Instance.localGameCfg.version;//.Version;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            LuaMgr.Instance.ShowExit();
        }
    }   

    public void LogValid(bool flag)
    {

        LuaInterface.Debugger.useLog = flag;
        if (flag)
        {
            Debug.logger.filterLogType = LogType.Log;
        }
        else
        {
            GameObject req = GameObject.Find("Reporter");
            if(req != null)
            {
                GameObject.Destroy(req);
            }
            Debug.logger.filterLogType = LogType.Exception;// = LogType.Error;
        }
    }
}
