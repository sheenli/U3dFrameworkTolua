using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUI
{
    
    private GButton mBtnStart = null;
    //private GButton mBtnSound = null;
    private Controller mLoadCtrl = null;
    private const string fguiPack = "Loading";
    private const string comName = "Start";
    private static LoadingUI mInstance;
    private GComponent mRoot;
    public static LoadingUI Instance
    {
        get
        {
            mInstance = mInstance ?? new LoadingUI();
            return mInstance;
        }
    }

    public void Show(Action callback)
    {
        ResMgr.Intstance.LoadGroup("Loading", () =>
         {
             _Show();
             if (callback != null)
             {
                 callback();
             }
         });
    }

    public void Destroy()
    {
        SceneMgr.Instance.DetachListener(AppConst.CoreDef.LoadLuaFinished, Loaded);
        GRoot.inst.RemoveChild(mRoot);
        mRoot.Dispose();
    }

    private void _Show()
    {
        SceneMgr.Instance.AttachListener(AppConst.CoreDef.LoadLuaFinished, Loaded);
//         UIPackage.AddPackage(fguiPack, (string name, string extension, System.Type type, out DestroyMethod destroyMethod) =>
//         {
//             destroyMethod = DestroyMethod.Destroy;
//             return ResMgr.Intstance.GetAsset(name);
//         });

        mRoot = UIPackage.CreateObject(fguiPack, comName).asCom;
        GRoot.inst.AddChild(mRoot);
        mRoot.container.cachedTransform.position = Vector3.zero;
        mRoot.container.cachedTransform.localScale = Vector3.one;
        this.mRoot.SetSize(GRoot.inst.width, GRoot.inst.height);
        this.mRoot.pivot = GamUtil.CenterPivot;
        mBtnStart = mRoot.GetChild("BtnStart").asButton;
        //mBtnSound = mRoot.GetChild("BtnSound").asButton;
        mLoadCtrl = mRoot.GetController("loadCtrl");
        mLoadCtrl.selectedIndex = 1;
        mBtnStart.onClick.Add(this.OnBtnStartClick);
        //mBtnSound.selected = !SoundMgr.Instance.IsOn;
        //mBtnSound.onClick.Add(this.OnBtnSoundClick);
    }

    private void OnBtnStartClick(EventContext context)
    {
        SceneMgr.Instance.QueueEvent(AppConst.CoreDef.StartGame);
    }

    private void OnBtnSoundClick()
    {
        //SoundMgr.Instance.IsOn = !mBtnSound.selected;
        //BGMMgr.Instance.IsOn = SoundMgr.Instance.IsOn;
    }
    private void Loaded(EventData ev)
    {
        mLoadCtrl.selectedIndex = 0;
    }
}
