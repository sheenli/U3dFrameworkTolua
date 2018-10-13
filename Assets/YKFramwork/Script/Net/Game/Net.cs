using System;
using System.Collections;
using System.Text;
using UnityEngine;

public class Net : EventDispatcherNode
{
    public double pingTime = 0;

    /// <summary>
    /// hhtp服务器的url
    /// </summary>
    public string HttpServerUrl = "";

    /// <summary>
    /// 消息超时
    /// </summary>
    public int timeOut = 10;

    /// <summary>
    /// 心跳包发送
    /// </summary>
    public int hearInterval = 3000;

    public static Net Instance
    {
        get;
        private set;
    }

    


    public delegate bool CanRequestDelegate(int actionId, ActionParam actionParam);
    public delegate void RequestNotifyDelegate(Status eStatus);
    /// <summary>
    /// 网络请求回调统一处理方法
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public delegate bool CommonDataCallback(NetReader reader);

    /// <summary>
    /// 网络请求出错回调方法
    /// </summary>
    /// <param name="nType"></param>
    /// <param name="actionId"></param>
    /// <param name="strMsg"></param>
    public delegate void NetError(eNetError nType, int actionId, string strMsg);


    public enum Status
    {
        eStartRequest = 0,
        eEndRequest = 1,
    }

    protected static readonly int OVER_TIME = 30;
    private const int NETSUCCESS = 0;
    private string strUrl;
    private SocketConnect mSocket = null;

    public enum eNetError
    {
        eConnectFailed = 0,
        eTimeOut = 1,
        /// <summary> 帐号在其他地方登录 </summary>
        DuplicateCode = 10004,
        /// <summary> 登录超时或操作超时 </summary>
        ServerTimeoutCode = 10001,
    }

    /// <summary>
    /// 请求代理通知
    /// </summary>
    public RequestNotifyDelegate RequestNotify { set; get; }

    /// <summary>
    /// 注册网络请求出错回调方法
    /// </summary>
    public NetError NetErrorCallback { get; set; }

    /// <summary>
    /// 注册网络请求回调统一处理方法
    /// </summary>
    public CommonDataCallback CommonCallback { get; set; }

    public IHeadFormater HeadFormater { get; set; }

    public int NetSuccess
    {
        get { return NETSUCCESS; }
    }

    public int EventProority
    {
        get
        {
            return 999;
        }
    }

    public void OnPushCallback(SocketPackage package)
    {
        try
        {
            if (package == null) return;
            //do Heartbeat package
            if (package.ActionId == 1) return;

            GameAction gameAction = ActionFactory.Create(package.ActionId);
            if (gameAction == null)
            {
                throw new ArgumentException(string.Format("Not found {0} of GameAction object.", package.ActionId));
            }
            NetReader reader = package.Reader;
            bool result = true;
            if (CommonCallback != null)
            {
                result = CommonCallback(reader);
            }

            if (result && gameAction.TryDecodePackage(reader))
            {
                OnServerResponse(package,gameAction.GetResponseData());
                //gameAction.OnCallback(actionResult);
            } 
            else
            {
                Debug.Log("Decode package fail.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    
    public void OnServerResponse(NetPackage package,byte [] bytes)
    {
        if (package.ActionId == 2)
        {
        }
        else
        {
            GameServerMsg msg = new GameServerMsg(package, bytes);
            QueueEvent(new ServerResponseEvent(msg));
            if(package.ActionId == 0x05a1)
            {
                Net.Instance.Close();
                Debug.LogWarning("账号被挤");
            }
        }
    }

    public void RequestDelegate(Net.Status eState)
    {
        //todo user implement loading method
        if (eState == Net.Status.eStartRequest)
        {
        }
        else
        {
            //Net.Status.eEndRequest

        }
    }

    void Start()
    {
    }
   
    void Awake()
    {
        Instance = this;
        threadSafe = true;
        //AppConst.AppExternalDataPath
        AttachListener(EventDef.ServerError,this.HandleEvent,99);
        AttachListener(EventDef.ServerColsed, this.HandleEvent,99);
        Init();
    }

    void Init()
    {
        RequestNotify = RequestDelegate;
        HeadFormater = new CustomHeadFormater();
        //NetErrorCallback = (type, id, msg) => Debug.LogError(string.Format("Net error:{0}-{1}", id, msg));
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (mSocket != null)
        {
            mSocket.ProcessTimeOut();
            if (mSocket != null)
            {
                SocketPackage data = mSocket.Dequeue();
                if (data != null)
                {
                    OnSocketRespond(data);
                }
            }
            if (mSocket != null)
            {
                SocketPackage data = mSocket.DequeuePush();
                if (data != null)
                {
                    OnPushCallback(data);
                }
            }
        }
    }

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        Close();
    }


    public void Close()
    {
        if (mSocket != null)
        {
            mSocket.Close();
            mSocket = null;
        }
    }

    private string serverIp;
    private int port;
    public void ConnectedServer()
    {
        UnityEngine.NetworkReachability state = UnityEngine.Application.internetReachability;
        if (state != UnityEngine.NetworkReachability.NotReachable)
        {
            NetWriter.SetUrl(serverIp + ":" + port);
            //if (mSocket == null)
            {
                string strUrl = NetWriter.GetUrl();
                string[] arr = strUrl.Split(new char[] { ':' });
                int nPort = int.Parse(arr[1]);
                mSocket = new SocketConnect(arr[0], nPort, HeadFormater,timeOut,hearInterval);
            }
            mSocket.EnsureConnected();
        }
        else
        {
            QueueEvent(EventDef.ServerConnectFailure, "当前网络没有连接");
        }
    }

    /// <summary>
    /// Send
    /// </summary>
    /// <param name="actionId"></param>
    /// <param name="callback"></param>
    /// <param name="actionParam"></param>
    /// <param name="bShowLoading"></param>
    public void Send(int actionId, byte[] actionParam,bool bShowLoading = true)
    {
        byte[] boby = actionParam;

        GameAction gameAction = ActionFactory.Create(actionId);
        if (gameAction == null)
        {
            throw new ArgumentException(string.Format("Not found {0} of GameAction object.", actionId));
        }
        //if (SceneChangeManager.mInstance != null)
        //    SceneChangeManager.mInstance.ShowLoading(true);
        if (NetWriter.IsSocket())
        {
            SocketRequest(gameAction, boby, HeadFormater, bShowLoading);
        }
        else
        {
            //HttpRequest(gameAction, boby, HeadFormater, bShowLoading);
        }
    }

    /// <summary>
    /// Socket消息发送
    /// </summary>
    /// <param name="gameAction">消息id</param>
    /// <param name="boby">内容</param>
    /// <param name="formater">解析协议对象</param>
    /// <param name="bShowLoading">是否显示loading界面</param>
    private void SocketRequest(GameAction gameAction,  byte [] boby, IHeadFormater formater, bool bShowLoading)
    {
        if (mSocket == null)
        {
            Debug.LogError("与服务器断开连接，由于服务器无响应");
            Close();
            QueueEvent(EventDef.ServerColsed);
            return;
            //string strUrl = NetWriter.GetUrl();
            //string[] arr = strUrl.Split(new char[] { ':' });
            //int nPort = int.Parse(arr[1]);
            //mSocket = new SocketConnect(arr[0], nPort, formater);

        }
        gameAction.Head.MsgId = NetWriter.MsgId;

        SocketPackage package = new SocketPackage();
        package.MsgId = (int)gameAction.Head.MsgId;
        package.ActionId = (int)gameAction.ActionId;
        package.Action = gameAction;
        package.HasLoading = bShowLoading;
        package.SendTime = DateTime.Now;
        byte[] data = gameAction.Send(boby);
        NetWriter.resetData();
        if (bShowLoading)
        {
            RequestDelegate(Status.eStartRequest);
        }
        mSocket.Send(data, package);
    }

    /// <summary>
    /// socket respond
    /// </summary>
    /// <param name="package"></param>
    private void OnSocketRespond(SocketPackage package)
    {
        if (package.ActionId == 2)
        {
            pingTime = DateTime.Now.Subtract(package.SendTime).Milliseconds;
        }
       
        if (package.HasLoading)
        {
            RequestDelegate(Status.eEndRequest);
        }
        if (package.ErrorCode == -2)
        {
            if (package.ActionId == 2)
            {
                Close();
                QueueEvent(EventDef.ServerColsed);
            }
            else
            {
                OnNetTimeOut(package.ActionId);
            }
        }
        else if (package.ErrorCode >= 10000)
        {
            ErrorData data = new ErrorData();
            data.actionId = package.ActionId;
            data.code = package.ErrorCode;
            data.errorMsg = package.ErrorMsg;
            Debug.LogError("-=============package.ErrorCode >= 10000======-----");
            OnNetError(data);
        }
        else
        {
            OnRespond(package);
        }
    }

  

    private void OnRespond(NetPackage package)
    {
        NetReader reader = package.Reader;
        
        bool result = true;
        if (CommonCallback != null)
        {
            result = CommonCallback(reader);
        }

        if (result && package.Action != null && package.Action.TryDecodePackage(reader))
        {
            OnServerResponse(package, package.Action.GetResponseData());
        }
        else
        {
            Debug.Log("Decode package fail.");
        }
    }

    
    /// <summary>
    /// 发送Get请求
    /// </summary>
    /// <param name="url">链接</param>
    /// <param name="msgId">消息id</param>
    public void SendHttpGet(string url,int msgId)
    {
        StartCoroutine(SendGet(url, msgId,(m,www)=> 
        {
            GameServerMsg msg = new GameServerMsg(new SocketPackage(), null);
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                msg.pack.ErrorCode = 1;
                msg.pack.ErrorMsg = www.error;
                msg.pack.ActionId = msgId;
                QueueEvent(new ServerResponseEvent(msg));
            }
            else
            {
                msg.pack.ErrorCode = 0;
                msg.pack.ErrorMsg = string.Empty;
                msg.pack.ActionId = msgId;
                msg.resultData = www.bytes;

                QueueEvent(new ServerResponseEvent(msg));
            }
            www.Dispose();
        }));
    }

    /// <summary>
    /// 发送Post请求
    /// </summary>
    /// <param name="url">链接</param>
    /// <param name="msgId">消息id</param>
    /// <param name="obj">数据</param>
    public void SendHttpPost(string url,int msgId,byte[] bytes)
    {
        StartCoroutine(SendPost(url, msgId, bytes, (m, www) =>
        {
            GameServerMsg msg = new GameServerMsg(new SocketPackage(), null);
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                msg.pack.ErrorCode = 1;
                msg.pack.ErrorMsg = www.error;
                msg.pack.ActionId = msgId;
                QueueEvent(new ServerResponseEvent(msg));
            }
            else
            {
                msg.pack.ErrorCode = 0;
                msg.pack.ErrorMsg = string.Empty;
                msg.pack.ActionId = msgId;
                msg.resultData = www.bytes;
                QueueEvent(new ServerResponseEvent(msg));
            }
            www.Dispose();
        }));
    }

    /// <summary>
    /// 发送Get请求
    /// </summary>
    /// <param name="_url"></param>
    /// <returns></returns>
    IEnumerator SendGet(string _url, int msgId, Action<int, WWW> action)
    {
        WWW getData = new WWW(_url);
        yield return getData;
        if (getData != null && action != null)
        {
            action(msgId, getData);
        }
    }

    /// <summary>
    /// 发送POST
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_wForm"></param>
    /// <returns></returns>
    IEnumerator SendPost(string _url, int msgId,byte[] bytes, Action<int, WWW> action)
    {
        WWW postData = new WWW(_url, bytes);
        yield return postData;
        if (postData != null && action != null)
        {
            action(msgId, postData);
        }
    }


    private void OnNetError(ErrorData data)
    {
        
        QueueEvent(new ServerErrorEvent(data));
        Debug.LogError("发送服务器错误消息"+data.code+"/"+ data.errorMsg);
        if (NetErrorCallback != null)
        {
            NetErrorCallback(eNetError.eConnectFailed, data.actionId, data.errorMsg);
        }

    }
    private void OnNetTimeOut(int nActionId)
    {
        ErrorData data = new ErrorData();
        data.actionId = nActionId;
        data.code = (int)eNetError.eTimeOut;
        data.errorMsg = "请求超时";
        Debug.LogError("======请求超时=======");
        OnNetError(data);
    }

    public void HandleEvent(EventData eventData)
    {
        if (eventData.name ==  EventDef.ServerColsed.ToString())
        {
            ErrorData data = new ErrorData();
            data.actionId = 0;
            data.code = 500000;
            data.errorMsg = "与服务器主动断开连接";
            //Debug.LogError("======与服务器主动断开连接=======");
            OnNetError(data);
        }
    }

    
}
public class ErrorData
{
    public int actionId;
    public int code;
    public string errorMsg;
}