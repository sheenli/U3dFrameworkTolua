using FairyGUI;
using FairyGUI.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LuaInterface;

public class YKSupportLua
{
    public interface ILuaNeedExtendCS
    {
        void SetLuaTalbe(LuaTable t);
    }


    public static void ShowWindow(string uiName, object param, LuaTable lunFun
           , bool hideAll = false, bool hideDotDel = false)
    {
        List<string> list = null;
        if (hideAll)
        {
            list = UIMgr.Instance.GetAllOpenWindows();
        }

        UIMgr.Instance.ShowWindow(uiName, null, ()=>
        {
            LuaFunction lunf = lunFun.GetLuaFunction("New");
            lunf.BeginPCall();
            lunf.PCall();
            LuaWindow wind = lunf.CheckObject(typeof(LuaWindow)) as LuaWindow;
            lunf.EndPCall();
            return wind;
        }, param, list, hideDotDel);
    }

    public static void GotoScene(string sceneName,object param , LuaTable lunFun)
    {
        SceneMgr.Instance.GotoScene(sceneName, null, () =>
          {
              LuaFunction lunf = lunFun.GetLuaFunction("New");
              lunf.BeginPCall();
              lunf.PCall();
              LuaSceneBase wind = lunf.CheckObject(typeof(LuaSceneBase)) as LuaSceneBase;
              lunf.EndPCall();
              return wind;
          }, param: param);
    }


    #region BaseWindow
    public class LuaWindow : BaseUI, ILuaNeedExtendCS
    {
        LuaTable mLuatable;
        private const string OnInitFunName = "OnInit";
        private const string DoHideAnimationFunName = "DoHideAnimation";
        private const string DoShowAnimationFunName = "DoShowAnimation";
        private const string OnShownFunName = "OnShown";
        private const string OnHideFunName = "OnHide";
        private const string OnDestroyFunName = "OnDestroy";
        private const string OnBtnCloseFunName = "OnBtnClose";
        private const string OnHandlerFunName = "OnHandler";
        private const string OnBtnClickFunName = "OnBtnClick";
        private Dictionary<string, LuaFunction> mCallBackfunc = new Dictionary<string, LuaFunction>
        {
            { OnInitFunName,null},
            { DoHideAnimationFunName,null},
            { DoShowAnimationFunName,null},
            { OnShownFunName,null},
            { OnHideFunName,null},
            { OnDestroyFunName,null},
            { OnBtnCloseFunName,null},
            { OnHandlerFunName,null},
            { OnBtnClickFunName,null},
        };
         public string packName = "";
        public override string PackName
        {
            get { return packName; }

        }

        public string resName = "";
        public override string ResName
        {
            get
            {
                return resName;
            }
        }
        public bool isNeedHideAnimation;
        protected override bool IsNeedHideAnimation
        {
            get
            {
                return isNeedHideAnimation;
            }
        }

        public bool isNeedShowAnimation;
        protected override bool IsNeedShowAnimation
        {
            get
            {
                return isNeedShowAnimation;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach(LuaFunction func in mCallBackfunc.Values)
            {
                if (func != null)
                {
                    func.Dispose();
                }
            }
            mCallBackfunc.Clear();
            if (mLuatable != null)
            {
                mLuatable.Dispose();
            }
        }

        protected override void OnInit()
        {
            base.OnInit();
            if (mLuatable != null)
            {
                LuaTable objs = LuaMgr.Instance.mLua.NewTable();
                LuaTable ctrls = LuaMgr.Instance.mLua.NewTable();
                foreach (KeyValuePair<string, FairyGUI.GObject> kv in this.UIObjs)
                {
                    objs.RawSet(kv.Key, kv.Value);
                }

                foreach (KeyValuePair<string, FairyGUI.Controller> kv in this.UICtrls)
                {
                    ctrls.RawSet(kv.Key, kv.Value);
                }
                mLuatable.RawSet("Objs", objs);
                mLuatable.RawSet("ctrls", ctrls);
            }
            if (mCallBackfunc[OnInitFunName] != null)
            {
                mCallBackfunc[OnInitFunName].BeginPCall();
                mCallBackfunc[OnInitFunName].Push(this);
                mCallBackfunc[OnInitFunName].PCall();
                mCallBackfunc[OnInitFunName].EndPCall();
            }
            

        }

        protected override void DoHideAnimation()
        {
            //base.DoHideAnimation();
            if (this.IsNeedHideAnimation)
            {
                if (mCallBackfunc[DoHideAnimationFunName] != null)
                {
                    mCallBackfunc[DoHideAnimationFunName].BeginPCall();
                    mCallBackfunc[DoHideAnimationFunName].Push(this);
                    mCallBackfunc[DoHideAnimationFunName].PCall();
                    mCallBackfunc[DoHideAnimationFunName].EndPCall();
                }
                else
                {
                    base.DoHideAnimation();
                }
            }
            else
            {
                this.HideImmediately();
            }
        }

        protected override void DoShowAnimation()
        {
            //base.DoShowAnimation();
            if (this.IsNeedShowAnimation)
            {
                if (mCallBackfunc[DoShowAnimationFunName] != null)
                {
                    mCallBackfunc[DoShowAnimationFunName].BeginPCall();
                    mCallBackfunc[DoShowAnimationFunName].Push(this);
                    mCallBackfunc[DoShowAnimationFunName].PCall();
                    mCallBackfunc[DoShowAnimationFunName].EndPCall();
                }
                else
                {
                    base.DoShowAnimation();
                }
            }
            else
            {
                this.OnShown();
            }


        }

        protected override void OnShown(object param)
        {
            base.OnShown(param);
            if (mCallBackfunc[OnShownFunName] != null)
            {
                mCallBackfunc[OnShownFunName].BeginPCall();
                mCallBackfunc[OnShownFunName].Push(this);
                mCallBackfunc[OnShownFunName].Push(param);
                mCallBackfunc[OnShownFunName].PCall();
                mCallBackfunc[OnShownFunName].EndPCall();
            }
        }

        protected override void OnHide()
        {
            if (mCallBackfunc[OnHideFunName] != null)
            {
                mCallBackfunc[OnHideFunName].BeginPCall();
                mCallBackfunc[OnHideFunName].Push(this);
                mCallBackfunc[OnHideFunName].PCall();
                mCallBackfunc[OnHideFunName].EndPCall(); ;
            }
            base.OnHide();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (mCallBackfunc[OnDestroyFunName] != null)
            {
                mCallBackfunc[OnDestroyFunName].BeginPCall();
                mCallBackfunc[OnDestroyFunName].Push(this);
                mCallBackfunc[OnDestroyFunName].PCall();
                mCallBackfunc[OnDestroyFunName].EndPCall();
            }
        }
        protected override void OnBtnClose()
        {
            if (mCallBackfunc[OnBtnCloseFunName] != null)
            {
                mCallBackfunc[OnBtnCloseFunName].BeginPCall();
                mCallBackfunc[OnBtnCloseFunName].Push(this);
                mCallBackfunc[OnBtnCloseFunName].PCall();
                mCallBackfunc[OnBtnCloseFunName].EndPCall();
            }
        }

        protected override void OnHandler(EventData ev)
        {
            base.OnHandler(ev);
            if (mCallBackfunc[OnHandlerFunName] != null)
            {
                mCallBackfunc[OnHandlerFunName].BeginPCall();
                mCallBackfunc[OnHandlerFunName].Push(this);
                mCallBackfunc[OnHandlerFunName].Push(ev);
                mCallBackfunc[OnHandlerFunName].PCall();
                mCallBackfunc[OnHandlerFunName].EndPCall();
            }
        }

        protected override void OnBtnClick(EventContext btn)
        {
            base.OnBtnClick(btn);
            if (mCallBackfunc[OnBtnClickFunName] != null)
            {
                mCallBackfunc[OnBtnClickFunName].BeginPCall();
                mCallBackfunc[OnBtnClickFunName].Push(this);
                mCallBackfunc[OnBtnClickFunName].Push(btn);
                mCallBackfunc[OnBtnClickFunName].PCall();
                mCallBackfunc[OnBtnClickFunName].EndPCall();
            }
        }
        public void SetLuaTalbe(LuaTable t)
        {
            mLuatable = t;
            List<string> keys = new List<string>();
            foreach (string key in mCallBackfunc.Keys)
            {
                keys.Add(key);
            }
            foreach (string key in keys)
            {
                mCallBackfunc[key] = t.GetLuaFunction(key);
            }
            
        }
    }
    #endregion

    #region SceneBase
    private class CONSTLuaSceneBaseFuncName
    {
        public const string OnInit="OnInit";
        public const string OnEnter="OnEnter";
        public const string OnLeave="OnLeave";
        public const string OnResLoaded="OnResLoaded";
        public const string OnTaskFailure="OnTaskFailure";
        public const string OnHandler="OnHandler";
        public const string OnDestroy="OnDestroy";
    }
    public class LuaSceneBase : SceneBase,ILuaNeedExtendCS
    {
        private LuaTable mLuaTable;
        private Dictionary<string,LuaFunction> callback = new Dictionary<string, LuaFunction>()
        {
            { CONSTLuaSceneBaseFuncName.OnDestroy,null},
            { CONSTLuaSceneBaseFuncName.OnInit,null},
            { CONSTLuaSceneBaseFuncName.OnEnter,null},
            { CONSTLuaSceneBaseFuncName.OnLeave,null},
            { CONSTLuaSceneBaseFuncName.OnResLoaded,null},
            { CONSTLuaSceneBaseFuncName.OnTaskFailure,null},
            { CONSTLuaSceneBaseFuncName.OnHandler,null},

        };
        public void SetLuaTalbe(LuaTable peerTable)
        {
            mLuaTable = peerTable;

            callback[CONSTLuaSceneBaseFuncName.OnDestroy] = mLuaTable.GetLuaFunction(CONSTLuaSceneBaseFuncName.OnDestroy);
            callback[CONSTLuaSceneBaseFuncName.OnInit] = mLuaTable.GetLuaFunction(CONSTLuaSceneBaseFuncName.OnInit);
            callback[CONSTLuaSceneBaseFuncName.OnEnter] = mLuaTable.GetLuaFunction(CONSTLuaSceneBaseFuncName.OnEnter);
            callback[CONSTLuaSceneBaseFuncName.OnLeave] = mLuaTable.GetLuaFunction(CONSTLuaSceneBaseFuncName.OnLeave);
            callback[CONSTLuaSceneBaseFuncName.OnResLoaded] = mLuaTable.GetLuaFunction(CONSTLuaSceneBaseFuncName.OnResLoaded);
            callback[CONSTLuaSceneBaseFuncName.OnTaskFailure] = mLuaTable.GetLuaFunction(CONSTLuaSceneBaseFuncName.OnTaskFailure);
            callback[CONSTLuaSceneBaseFuncName.OnHandler] = mLuaTable.GetLuaFunction(CONSTLuaSceneBaseFuncName.OnHandler);

        }
        protected override void OnInit(object param)
        {
            base.OnInit(param);

            if (callback[CONSTLuaSceneBaseFuncName.OnInit] != null)
            {
                callback[CONSTLuaSceneBaseFuncName.OnInit].BeginPCall();
                callback[CONSTLuaSceneBaseFuncName.OnInit].Push(this);
                callback[CONSTLuaSceneBaseFuncName.OnInit].Push(param);
                callback[CONSTLuaSceneBaseFuncName.OnInit].PCall();
                callback[CONSTLuaSceneBaseFuncName.OnInit].EndPCall();
            }
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            if (callback[CONSTLuaSceneBaseFuncName.OnEnter] != null)
            {
                callback[CONSTLuaSceneBaseFuncName.OnEnter].BeginPCall();
                callback[CONSTLuaSceneBaseFuncName.OnEnter].Push(this);
                callback[CONSTLuaSceneBaseFuncName.OnEnter].PCall();
                callback[CONSTLuaSceneBaseFuncName.OnEnter].EndPCall();
            }
        }

        protected override void OnHandler(EventData ev)
        {
            base.OnHandler(ev);
            if (callback[CONSTLuaSceneBaseFuncName.OnHandler] != null)
            {
                callback[CONSTLuaSceneBaseFuncName.OnHandler].BeginPCall();
                callback[CONSTLuaSceneBaseFuncName.OnHandler].Push(this);
                callback[CONSTLuaSceneBaseFuncName.OnHandler].Push(ev);
                callback[CONSTLuaSceneBaseFuncName.OnHandler].PCall();
                callback[CONSTLuaSceneBaseFuncName.OnHandler].EndPCall();
            }
        }

        protected override void OnLeave()
        {
            base.OnLeave();
            if (callback[CONSTLuaSceneBaseFuncName.OnLeave] != null)
            {
                callback[CONSTLuaSceneBaseFuncName.OnLeave].BeginPCall();
                callback[CONSTLuaSceneBaseFuncName.OnLeave].Push(this);
                callback[CONSTLuaSceneBaseFuncName.OnLeave].PCall();
                callback[CONSTLuaSceneBaseFuncName.OnLeave].EndPCall();
            }
        }

        protected override void OnResLoaded()
        {
            base.OnResLoaded();
            if (callback[CONSTLuaSceneBaseFuncName.OnResLoaded] != null)
            {
                callback[CONSTLuaSceneBaseFuncName.OnResLoaded].BeginPCall();
                callback[CONSTLuaSceneBaseFuncName.OnResLoaded].Push(this);
                callback[CONSTLuaSceneBaseFuncName.OnResLoaded].PCall();
                callback[CONSTLuaSceneBaseFuncName.OnResLoaded].EndPCall();
            }
        }

        protected override void OnTaskFailure(string taskName, string error)
        {
            base.OnTaskFailure(taskName, error);
            if (callback[CONSTLuaSceneBaseFuncName.OnTaskFailure] != null)
            {
                callback[CONSTLuaSceneBaseFuncName.OnTaskFailure].BeginPCall();
                callback[CONSTLuaSceneBaseFuncName.OnTaskFailure].Push(this);
                callback[CONSTLuaSceneBaseFuncName.OnTaskFailure].Push(taskName);
                callback[CONSTLuaSceneBaseFuncName.OnTaskFailure].Push(error);
                callback[CONSTLuaSceneBaseFuncName.OnTaskFailure].PCall();
                callback[CONSTLuaSceneBaseFuncName.OnTaskFailure].EndPCall();
            }
        }

        public override void OnDestroy()
        {
            if (mLuaTable != null) mLuaTable.Dispose();
            foreach (LuaFunction func in callback.Values)
            {
                if (func != null)
                {
                    func.Dispose();
                }
            }
            callback.Clear();
        }
    }
    #endregion

    #region LuaTaskBase
    public class LuaTaskBase : ITask,ILuaNeedExtendCS
    {

        private LuaTable mPeerTable = null;
        public string failureInfo = "";
        public string taskName = "";
        public bool isFailure = false;
        public bool isFinished = false;
        private LuaFunction _OnExecute;

        public bool IsFailure
        {
            get
            {
                return isFailure;
            }
        }

        public bool IsFinished
        {
            get
            {
                return isFinished;
            }
        }
        public void SetLuaTalbe(LuaTable peerTable)
        {
            mPeerTable = peerTable;
            _OnExecute = peerTable.GetLuaFunction("OnExecute");
        }

        ~LuaTaskBase()
        {
            if (mPeerTable != null)
            {
                mPeerTable.Dispose();
            }
            if (_OnExecute != null) _OnExecute.Dispose();
        }

        public string TaskName()
        {
            return taskName;
        }

        public void OnExecute()
        {
            if (_OnExecute != null)
            {
                _OnExecute.BeginPCall();
                _OnExecute.Push(this);
                _OnExecute.PCall();//.Call(this);
            }
        }

        public string FailureInfo()
        {
            return failureInfo;
        }

        public void Rest()
        {
            isFailure = false;
            isFinished = false;
        }
    }
    #endregion

    #region LuaStateBase
    private class ConstLuaStateFuncName
    {
        public const string OnEnter = "OnEnter";
        public const string OnLeave = "OnLeave";
        public const string OnRelease = "OnRelease";
        public const string OnUpdate = "OnUpdate";
        public const string OnFixedUpdate = "OnFixedUpdate";
        public const string OnLateUpdate = "OnLateUpdate";
    }
    public class LuaStateBase : IState , ILuaNeedExtendCS
    {

        private LuaTable mPeerTable = null;
        private Dictionary<string,LuaFunction> callback = new Dictionary<string, LuaFunction>()
        {
            { ConstLuaStateFuncName.OnEnter,null},
            { ConstLuaStateFuncName.OnLeave,null},
            { ConstLuaStateFuncName.OnRelease,null},
            { ConstLuaStateFuncName.OnUpdate,null},
            { ConstLuaStateFuncName.OnFixedUpdate,null},
            { ConstLuaStateFuncName.OnLateUpdate,null},
        };
        public void SetLuaTalbe(LuaTable peerTable)
        {
            mPeerTable = peerTable;
            foreach (string k in callback.Keys)
            {
                callback[k] = mPeerTable.GetLuaFunction(k);
            }
        }

        public uint StateId
        {
            get;
            set;
        }

        public void OnEnter(IState prevState, object param1 = null, object param2 = null)
        {
            if (callback[ConstLuaStateFuncName.OnEnter] != null)
            {
                callback[ConstLuaStateFuncName.OnEnter].BeginPCall();
                callback[ConstLuaStateFuncName.OnEnter].Push(this);
                callback[ConstLuaStateFuncName.OnEnter].Push(prevState);
                callback[ConstLuaStateFuncName.OnEnter].Push(param1);
                callback[ConstLuaStateFuncName.OnEnter].Push(param2);
                callback[ConstLuaStateFuncName.OnEnter].PCall();
                callback[ConstLuaStateFuncName.OnEnter].EndPCall();
            }
        }

        public void OnFixedUpdate()
        {
            if (callback[ConstLuaStateFuncName.OnFixedUpdate] != null)
            {
                callback[ConstLuaStateFuncName.OnFixedUpdate].BeginPCall();
                callback[ConstLuaStateFuncName.OnFixedUpdate].Push(this);
                callback[ConstLuaStateFuncName.OnFixedUpdate].PCall();
                callback[ConstLuaStateFuncName.OnFixedUpdate].EndPCall();
            }
        }

        public void OnLateUpdate()
        {
            if (callback[ConstLuaStateFuncName.OnLateUpdate] != null)
            {
                callback[ConstLuaStateFuncName.OnLateUpdate].BeginPCall();
                callback[ConstLuaStateFuncName.OnLateUpdate].Push(this);
                callback[ConstLuaStateFuncName.OnLateUpdate].PCall();
                callback[ConstLuaStateFuncName.OnLateUpdate].EndPCall();
            }
        }

        public void OnLeave(IState nextState, object param1 = null, object param2 = null)
        {
            if (callback[ConstLuaStateFuncName.OnLeave] != null)
            {
                callback[ConstLuaStateFuncName.OnLeave].BeginPCall();
                callback[ConstLuaStateFuncName.OnLeave].Push(this);
                callback[ConstLuaStateFuncName.OnLeave].Push(nextState);
                callback[ConstLuaStateFuncName.OnLeave].Push(param1);
                callback[ConstLuaStateFuncName.OnLeave].Push(param2);
                callback[ConstLuaStateFuncName.OnLeave].PCall();
                callback[ConstLuaStateFuncName.OnLeave].EndPCall();
            }
        }

        public void OnUpdate()
        {
            if (callback[ConstLuaStateFuncName.OnUpdate] != null)
            {
                callback[ConstLuaStateFuncName.OnUpdate].BeginPCall();
                callback[ConstLuaStateFuncName.OnUpdate].Push(this);
                callback[ConstLuaStateFuncName.OnUpdate].PCall();
                callback[ConstLuaStateFuncName.OnUpdate].EndPCall();
            }
        }

        public void OnRelease()
        {
            if (callback[ConstLuaStateFuncName.OnRelease] != null)
            {
                callback[ConstLuaStateFuncName.OnRelease].BeginPCall();
                callback[ConstLuaStateFuncName.OnRelease].Push(this);
                callback[ConstLuaStateFuncName.OnRelease].PCall();
                callback[ConstLuaStateFuncName.OnRelease].EndPCall();
            }

            if (mPeerTable != null)
            {
                mPeerTable.Dispose();
            }
            foreach (string k in callback.Keys)
            {
                if (callback[k] != null)
                {
                    callback[k].Dispose();
                }
            }
            callback.Clear();
        }
    }
    #endregion

    #region LuaUBBParser
    public class LuaUBBParser : UBBParser , ILuaNeedExtendCS
    {
        private LuaTable mPeerTable = null;
        private LuaFunction mOnTagHandlers = null;
        public void SetLuaTalbe(LuaTable peerTable)
        {
            mPeerTable = peerTable;
            mOnTagHandlers = mPeerTable.GetLuaFunction("OnTagHandlers");
        }

        public void AddTag(string key)
        {
            this.handlers[key] = OnTagHandlers;
        }

        string OnTagHandlers(string tagName, bool end, string attr)
        {
            string str = "";
            if (mOnTagHandlers != null)
            {
                mOnTagHandlers.BeginPCall();
                mOnTagHandlers.Push(this);
                mOnTagHandlers.Push(tagName);
                mOnTagHandlers.Push(end);
                mOnTagHandlers.Push(attr);
                mOnTagHandlers.PCall();
                str = mOnTagHandlers.CheckString();
                mOnTagHandlers.EndPCall();
            }
            return str;
        }
    }
    #endregion

    public class LuaMode : BaseMode , ILuaNeedExtendCS
    {
        private LuaTable    mLuatable;
        private LuaFunction _OnClear;
        private LuaFunction _OnDestroy;
        private LuaFunction _OnInitData;
        private LuaFunction _OnLogin;
        public override  void OnClear()
        {
            if (_OnClear != null)
            {
                _OnClear.BeginPCall();
                _OnClear.Push(this);
                _OnClear.PCall();
                _OnClear.EndPCall();
            }
        }

        public override void OnDestroy()
        {
            if (_OnDestroy != null)
            {
                _OnDestroy.BeginPCall();
                _OnDestroy.Push(this);
                _OnDestroy.PCall();
                _OnDestroy.EndPCall();
            }
        }

        public override void OnInitData()
        {
            if (_OnInitData != null)
            {
                _OnInitData.BeginPCall();
                _OnInitData.Push(this);
                _OnInitData.PCall();
                _OnInitData.EndPCall();
            }
        }

        public override void OnLogin()
        {
            if (_OnLogin != null)
            {
                _OnLogin.BeginPCall();
                _OnLogin.Push(this);
                _OnLogin.PCall();
                _OnLogin.EndPCall();
            }
        }
        
        public void SetLuaTalbe(LuaTable t)
        {
            mLuatable = t;
            _OnClear = mLuatable.GetLuaFunction("OnClear");
            _OnDestroy = mLuatable.GetLuaFunction("OnDestroy");
            _OnInitData = mLuatable.GetLuaFunction("OnInitData");
            _OnLogin = mLuatable.GetLuaFunction("Login");
        }
    }
}