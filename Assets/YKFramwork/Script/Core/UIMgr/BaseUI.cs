using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public abstract class BaseUI : Window
{
    public InterchangeableEventListenerMgr eventMgr;

    protected string btnNameStartsWith = "Btn";

    protected string BlackBgName = "BlackBg";

    protected string mCloseBtnName = "BtnClose";

    protected Dictionary<string, FairyGUI.GObject> UIObjs = new Dictionary<string, FairyGUI.GObject>();

    protected Dictionary<string, FairyGUI.Controller> UICtrls = new Dictionary<string, FairyGUI.Controller>();

    public bool isDotDel = false;

    protected object mParamData;
    public void SetData(object paramData)
    {
        mParamData = paramData;
    }

    public object GetData()
    {
        return mParamData;
    }

    protected virtual bool IsNeedShowAnimation
    {
        get { return true; }
    }

    protected virtual bool IsNeedHideAnimation
    {
        get { return true; }
    }

    /// <summary>
    /// 当前窗口属于哪个包
    /// </summary>
    public abstract string PackName
    {
        get;
    }

    public virtual string ResName
    {
        get { return "Main"; }
    }    
    

    /// <summary>
    /// 首次进入这个界面
    /// </summary>
    protected override void OnInit()
    {
        eventMgr = new InterchangeableEventListenerMgr(this.OnHandler, 1);
        base.OnInit();
        GObject windObj = UIPackage.CreateObject(this.PackName, this.ResName);

        if (windObj == null)
        {
            throw new System.Exception("不存在包名："+this.PackName+ "/ResName="+ ResName);
        }

        this.contentPane = windObj.asCom;
        this.container.cachedTransform.position = Vector3.zero;
        this.container.cachedTransform.localScale = Vector3.one;

        if (this.contentPane == null)
        {
            Debug.LogError("创建物体失败");
        }
        this.contentPane.SetSize(GRoot.inst.width, GRoot.inst.height);
        

        var obj = this.contentPane.GetChild(BlackBgName);
		var closeBtn = this.contentPane.GetChild (mCloseBtnName);
		if(closeBtn != null && closeBtn.asButton != null)
		closeBtn.asButton.onClick.Add (this.OnBtnClose);
        //if (obj != null)
        //{
        //    mBlackBg = obj.asImage;
        //    mBlackBg.pivot = GamUtil.CenterPivot;
        //}
        
        for (int i = 0;i< this.contentPane.numChildren;i++)
        {
            FairyGUI.GObject gObject = this.contentPane.GetChildAt(i);
            UIObjs[gObject.name] = gObject;
            if (gObject.name.StartsWith(btnNameStartsWith))
            {
                gObject.onClick.Add(a => 
                {
                    OnBtnClick(a);
                });
            }
        }

        foreach (Controller ctrl in this.contentPane.Controllers)
        {
            this.UICtrls[ctrl.name] = ctrl;
        }
		this.pivot = GamUtil.CenterPivot;
    }

    protected override void DoShowAnimation()
    {
        if (IsNeedShowAnimation)
        {
			if(!string.IsNullOrEmpty(FairyGUI.UIConfig.globalModalWaiting))
			FairyGUI.GRoot.inst.ShowModalWait ();
			this.scale = GamUtil.UIStartScale;
			Tweener tweener = DG.Tweening.DOTween.To(() => this.scale, a => this.scale = a, Vector2.one, 0.3f)
                .SetEase(Ease.OutBounce).OnComplete(() =>
                {
						if(!string.IsNullOrEmpty(FairyGUI.UIConfig.globalModalWaiting))
						FairyGUI.GRoot.inst.CloseModalWait();
                    this.OnShown();
                })
                .SetUpdate(true)
				.SetTarget(this);
        }
        else
        {
			this.scale = Vector2.one;
            this.OnShown();
        }
    }

    protected override void DoHideAnimation()
    {
        if (IsNeedHideAnimation)
        {
			DG.Tweening.DOTween.To(() => scale, a => this.scale = a, Vector2.zero, 0.3f)
            .OnComplete(() => { base.DoHideAnimation(); });
        }
        else
        {
            this.HideImmediately();
        }
    }
    public override void Dispose()
    {
        OnDestroy();
        base.Dispose();
    }

    protected virtual void OnDestroy()
    {

    }

    protected override void OnShown()
    {
        base.OnShown();
        this.visible = true;
        OnShown(mParamData);
    }

    /// <summary>
    /// 显示界面的时候
    /// </summary>
    /// <param name="param"></param>
    protected virtual void OnShown(object param)
    {

    }

    protected override void OnHide()
    {
        base.OnHide();
        eventMgr.RemoveAll();
    }

	protected virtual void OnBtnClose()
	{
		
	}

    protected virtual void OnHandler(EventData ev)
    {

    }

    protected virtual void OnBtnClick(EventContext btn)
    {

    }

    public void Refresh()
    {
        this.OnShown();
    }

}
