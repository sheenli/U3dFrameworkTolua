#if usetolua
using LuaInterface;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMgr : EventDispatcherNode
{
    public const string ChangeSceneFinished = "ChangeSceneFinished";
    /// <summary>
    /// 跳转场景显示中间界面
    /// </summary>
    public Action<List<TaskBase>> OpenLoadUI = null;

    /// <summary>
    /// 关闭loadUI
    /// </summary>
    public Action CloseLoadUI = null;

    private Dictionary<string, SceneBase> mScenes = new Dictionary<string, SceneBase>();

    public static SceneMgr Instance
    {
        get;
        private set;
    }

    public void Awake()
    {
        Instance = this;
        
        AttachListener(ChangeSceneFinished, this.OnHanderEvent);
    }

    /// <summary>
    /// 以前的场景
    /// </summary>
    public Queue<SceneBase> oldScenes = new Queue<SceneBase>();

    /// <summary>
    /// 现在场景
    /// </summary>
    public SceneBase currentScene = null;
    
    public void GotoScene(string scneneType, Type t = null,Func<SceneBase> func = null,object param = null)
    {
        FairyGUI.GRoot.inst.touchable = false;
        SceneBase scene = GetScnene(scneneType);
        if(func == null)
        {
            func = () =>
            {
                return Activator.CreateInstance(t) as SceneBase;
            };
        }
        if (scene == null)
        {
            scene = func();
            scene.SceneName = scneneType;
            mScenes.Add(scneneType, scene);
        }
        if (currentScene != null && scneneType == currentScene.SceneName)
        {
            Debug.LogError("当前场景和要到的目标场景重复"+scneneType);
            currentScene.Enter(param);
        }
        else
        {
            if (currentScene != null)
            {
                currentScene.Leave();
                oldScenes.Enqueue(currentScene);
            }
            currentScene = mScenes[scneneType];
            currentScene.Enter(param);
        }
    }
    public override void OnDestroy()
    {
        foreach (SceneBase scene in mScenes.Values)
        {
            scene.OnDestroy();
        }
        base.OnDestroy();
    }

    /// <summary>
    /// 获取一个场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public SceneBase GetScnene(string sceneName)
    {
        if (mScenes.ContainsKey(sceneName))
        {
            return mScenes[sceneName];
        }
        return null;
    }

    public void OnHanderEvent(EventData data)
    {
        if(data.name == ChangeSceneFinished)
        {
            while(oldScenes.Count > 0)
            {
                SceneBase scene = oldScenes.Dequeue();
                
                scene.OnDestroy();
                if (mScenes.ContainsKey(scene.SceneName))
                {
                    mScenes.Remove(scene.SceneName);
                }
            }
            FairyGUI.GRoot.inst.touchable = true;
            //FairyGUI.GRoot.inst.CloseModalWait();
            ResMgr.Intstance.UnLoadAll();
        }
    }
}
