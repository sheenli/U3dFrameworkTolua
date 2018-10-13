using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if usetolua
using LuaInterface;
#endif

public class UIMgr : EventDispatcherNode
{
    private enum CommandType
    {
        /// <summary>
        /// 创建
        /// </summary>
        Create,

        /// <summary>
        /// 显示
        /// </summary>
        Show,

        /// <summary>
        /// 隐藏
        /// </summary>
        Hide,

        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 隐藏一个列表
        /// </summary>
        HideList,
    }

    private class CommandData
    {

        public CommandData(CommandType _cmdType, string _uiName, Func<BaseUI> _createFun, object _param
            , Action<BaseUI> _callback = null)
        {
            cmdType = _cmdType;
            uiName = _uiName;
            param = _param;
            createFun = _createFun;
            callback = _callback;
        }

        public CommandData(List<string> list)
        {
            this.cmdType = CommandType.HideList;
            this.uinames = list;
        }
        public CommandType cmdType;
        public string uiName;
        public object param;
        public Func<BaseUI> createFun = null;
        public List<string> uinames = new List<string>();
        public Action<BaseUI> callback = null;
        public bool isCallBack = true;
        public static CommandData CreateShow(string _uiName
            , Func<BaseUI> _createFun, object _param, Action<BaseUI> callback)
        {
            CommandData data = new CommandData(CommandType.Show, _uiName, _createFun, _param, callback);
            return data;
        }

        public static CommandData CreateHide(string _uiName)
        {
            CommandData data = new CommandData(CommandType.Hide, _uiName, null, null);
            return data;
        }

        public static CommandData CreateDelete(string _uiName)
        {
            CommandData data = new CommandData(CommandType.Delete, _uiName, null, null);
            return data;
        }

        public static CommandData CreateCreate(string _uiName
          , Func<BaseUI> _createFun, object _param, Action<BaseUI> callback)
        {
            CommandData data = new CommandData(CommandType.Create, _uiName, _createFun, _param, callback);
            return data;
        }

        public static CommandData CreateHideList(List<string>_list)
        {
            return new CommandData(_list);
        }
    }

    /// <summary>
    /// 所有UI
    /// </summary>
    private Dictionary<string,BaseUI> mUIArray = new Dictionary<string, BaseUI>();

    private List<CommandData> mCmds = new List<CommandData>();

    public static UIMgr Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Init(int designResolutionX, int designResolutionY)
    {
		FairyGUI.GRoot.inst.SetContentScaleFactor(designResolutionX, designResolutionY);        
    }

    public BaseUI GetWindow(string uiName)
    {
        BaseUI wind = null;
        foreach (string name in mUIArray.Keys)
        {
            if (name == uiName)
            {
                wind = mUIArray[name];
                break;
            }
        }
        return wind;
    }

    public List<string> GetAllOpenWindows()
    {
        List<string> list = new List<string>();
        foreach(string uiName in mUIArray.Keys)
        {
            if (IsOpenWindow(uiName))
            {
                list.Add(uiName);
            }
        }
        return list;
    }

    private void _CreateWind(CommandData cmd)
    {
        BaseUI wind = GetWindow(cmd.uiName);
        if (wind == null)
        {
            wind = cmd.createFun();
            mUIArray.Add(cmd.uiName, wind);
        }
        if (cmd.isCallBack && cmd.callback != null) cmd.callback(wind);
        _ShowWindow(cmd);
    }

    private void _ShowWindow(CommandData cmd)
    {
        BaseUI wind = GetWindow(cmd.uiName);
        wind.SetData(cmd.param);
        if (wind.isShowing)
        {
            if (!wind.isTop)
            {
                wind.BringToFront();
            }
            wind.Refresh();
        }
        else
        {
            FairyGUI.GRoot.inst.ShowWindow(wind);
        }
        if (cmd.isCallBack && cmd.callback != null) cmd.callback(wind);
    }

    public void ShowWindow(string uiName, Type type, Func<BaseUI> _createFun = null,
        object param = null, List<string> hideWinds = null,
        bool hideDotDel = false,Action<BaseUI> callback = null)
    {
        if (hideWinds != null && hideWinds.Contains(uiName))
        {
            hideWinds.Remove(uiName);
        }
        if(_createFun == null)
        {
            _createFun = () =>
            {
                return Activator.CreateInstance(type) as BaseUI;
            };
        }
        CommandData cmd = CommandData.CreateShow(uiName, _createFun, param, callback);
        mCmds.Add(cmd);
        if (hideWinds != null)
        {
            for (int i = 0;i< hideWinds.Count;i++)
            {
                BaseUI wind = GetWindow(hideWinds[i]);
                if (wind != null)
                {
                    if (wind.isDotDel && !hideDotDel)
                    {
                        hideWinds.RemoveAt(i);
                        i--;
                    }
                }
            }
            CommandData hideCmd = CommandData.CreateHideList(hideWinds);
            mCmds.Add(hideCmd);
        }
    }

    public void CloseWindow(string uiName)
    {
        CommandData cmd = CommandData.CreateHide(uiName);
        mCmds.Add(cmd);
    }

    private void _HideWind(CommandData cmd)
    {
        BaseUI wind = GetWindow(cmd.uiName);
        if (wind != null && wind.isShowing)
        {
            FairyGUI.GRoot.inst.HideWindow(wind);
        }
    }

    private void _HideList(CommandData cmd)
    {
        foreach (string uiName in cmd.uinames)
        {
            BaseUI wind = GetWindow(uiName);
            if (wind != null && wind.isShowing)
            {
                FairyGUI.GRoot.inst.HideWindowImmediately(wind);
            }
        }
    }


    public void DeleteWind(string uiName)
    {
        CommandData cmd = CommandData.CreateDelete(uiName);
        mCmds.Add(cmd);
    }

    private void _DeleteWind(CommandData cmd)
    {
        BaseUI wind = GetWindow(cmd.uiName);
        if(wind != null)
        {
            FairyGUI.GRoot.inst.HideWindowImmediately(mUIArray[cmd.uiName], !wind.isDotDel);
            if (!wind.isDotDel && mUIArray.ContainsKey(cmd.uiName))
            {
                mUIArray.Remove(cmd.uiName);
            }
        }
        
    }


    public bool IsOpenWindow(string uiName)
    {
        BaseUI wind = GetWindow(uiName);
        if (wind != null)
        {
            return wind.isShowing;
        }
        return false;
    }


    /// <summary>
    /// 关闭所有打开的窗口
    /// </summary>
    /// <param name="isMode"></param>
    public void DeleteAllWindows(bool isDotDel = false)
    {
        foreach (string uiName in mUIArray.Keys)
        {
            if (!mUIArray[uiName].isDotDel || isDotDel)
                DeleteWind(uiName);
        }
    }
    
    /// <summary>
    /// 隐藏所有窗口
    /// </summary>
    public void HideAllWindows(bool isDotDel = false)
    {
        foreach (string uiName in mUIArray.Keys)
        {
            if (mUIArray[uiName].isShowing)
            {
                if (!mUIArray[uiName].isDotDel || isDotDel)
                {
                    CloseWindow(uiName);
                }
            }
        }
    }



#region 循环执行命令
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (mCmds.Count > 0)
        {
            CommandData data = mCmds[0];
            switch (data.cmdType)
            {
                case CommandType.Create:
                    _CreateWind(data);
                    break;
                case CommandType.Delete:
                    _DeleteWind(data);
                    break;
                case CommandType.Hide:
                    _HideWind(data);
                    break;
                case CommandType.Show:
                    if (GetWindow(data.uiName) == null)
                    {
                        CommandData createCmd = CommandData.CreateCreate(data.uiName,
                            data.createFun, data.param,data.callback);
                        createCmd.isCallBack = false;
                        mCmds.RemoveAt(0);
                        mCmds.Insert(0, createCmd);
                        return;
                    }
                    _ShowWindow(data);
                    break;
                case CommandType.HideList:
                    _HideList(data);
                    break;
            }
            mCmds.RemoveAt(0);
        }
    }
#endregion
}