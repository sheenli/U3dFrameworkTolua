using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using YKFramework;

enum ErrorCode
{
    Success = 0,
    ConnectError = -1,
    TimeOutError = -2,
}

/// <summary>
/// 
/// </summary>
/// <param name="package"></param>
public delegate void NetPushCallback(SocketPackage package);
/// <summary>
/// 
/// </summary>
public class SocketConnect
{
    /// <summary>
    /// push Action的请求
    /// </summary>
    private static readonly List<SocketPackage> ActionPools = new List<SocketPackage>();
    private Socket _socket;
    private readonly string _host;
    private readonly int _port;
    private readonly IHeadFormater _formater;
    private bool _isDisposed;
    private readonly List<SocketPackage> _sendList;
    private readonly Queue<SocketPackage> _receiveQueue;
    private readonly Queue<SocketPackage> _pushQueue;
    private int TimeOut = 10;//30秒的超时时间
    private Thread _thread = null;
    private int HearInterval = 3000;
    private Timer _heartbeatThread = null;
    private byte[] _hearbeatPackage;

    private const int headFlagLenth = 4;

    private bool isConnect = false;

    public SocketConnect(string host, int port, IHeadFormater formater,int timeOut,int hearInterval)
    {
        this.TimeOut = timeOut;
        this.HearInterval = hearInterval;

        this._host = host;
        this._port = port;
        _formater = formater;
        _sendList = new List<SocketPackage>();
        _receiveQueue = new Queue<SocketPackage>();
        _pushQueue = new Queue<SocketPackage>();
    }

    static public void PushActionPool(int actionId, GameAction action)
    {
        RemoveActionPool(actionId);
        SocketPackage package = new SocketPackage();
        package.ActionId = actionId;
        package.Action = action;
        ActionPools.Add(package);
    }

    static public void RemoveActionPool(int actionId)
    {
        foreach (SocketPackage pack in ActionPools)
        {
            if (pack.ActionId == actionId)
            {
                ActionPools.Remove(pack);
                break;
            }
        }
    }
    /// <summary>
    /// 取出回返消息包
    /// </summary>
    /// <returns></returns>
    public SocketPackage Dequeue()
    {
        lock (_receiveQueue)
        {
            if (_receiveQueue.Count == 0)
            {
                return null;
            }
            else
            {
                return _receiveQueue.Dequeue();
            }
        }
    }

    public SocketPackage DequeuePush()
    {
        lock (_pushQueue)
        {
            if (_pushQueue.Count == 0)
            {
                return null;
            }
            else
            {
                return _pushQueue.Dequeue();
            }
        }
    }
    private void CheckReceive()
    {
        while (true)
        {
            if (_socket == null) return;
            try
            {
                if (_socket.Poll(5, SelectMode.SelectRead))
                {
                    if (_socket.Available == 0)
                    {
                        QueueEventClose();
                        Close();
                        return;
                    }
                    byte[] prefix = new byte[headFlagLenth];
                    int recnum = _socket.Receive(prefix);

                    if (recnum == headFlagLenth)
                    {
                        
                        UInt32 val = 0;
                        if (BitConverter.IsLittleEndian)
                        {
                            val = BitConverter.ToUInt32(new byte[4] { prefix[3], prefix[2], prefix[1], prefix[0] },0);
                        }
                        else
                        {
                            val = BitConverter.ToUInt32(new byte[4] { prefix[0], prefix[1], prefix[2], prefix[3] },0);
                        }
                        byte[] data = new byte[val];
                        int startIndex = 0;
                        recnum = 0;
                        do
                        {
                            int rev = _socket.Receive(data, startIndex, (int)val - recnum, SocketFlags.None);
                            recnum += rev;
                            startIndex += rev;
                        } while (recnum != val);
                        //判断流是否有Gzip压缩
                        //if (data[0] == 0x1f && data[1] == 0x8b && data[2] == 0x08 && data[3] == 0x00)
                        //{
                        //    data = NetReader.Decompression(data);
                        //}

                        NetReader reader = new NetReader(_formater);
                        reader.pushNetStream(data, NetworkType.Socket, NetWriter.ResponseContentType);
                        SocketPackage findPackage = null;

                        //Debug.Log("Socket receive ok, revLen:" + recnum
                        //    + ", actionId:" + reader.ActionId
                        //    + ", msgId:" + reader.RmId
                        //    + ", error:" + reader.StatusCode + reader.Description
                        //    + ", packLen:" + reader.Buffer.Length);
                        lock (_sendList)
                        {
                            //find pack in send queue.
                            foreach (SocketPackage package in _sendList)
                            {
                                if (package.MsgId == reader.RmId)
                                {
                                    package.Reader = reader;
                                    package.ErrorCode = reader.StatusCode;
                                    package.ErrorMsg = reader.Description;
                                    findPackage = package;
                                    //Debug.LogError("服务器返回：" + package.ActionId);
                                    break;
                                }

                            }
                        }
                        if (findPackage == null)
                        {
                            lock (_receiveQueue)
                            {
                                //find pack in receive queue.
                                foreach (SocketPackage package in ActionPools)
                                {
                                    if (package.ActionId == reader.ActionId)
                                    {
                                        package.Reader = reader;
                                        package.ErrorCode = reader.StatusCode;
                                        package.ErrorMsg = reader.Description;
                                        findPackage = package;
                                        break;
                                    }
                                }
                            }
                        }
                        if (findPackage != null)
                        {
                            lock (_receiveQueue)
                            {
                                _receiveQueue.Enqueue(findPackage);
                            }
                            lock (_sendList)
                            {
                                _sendList.Remove(findPackage);
                            }
                        }
                        else
                        {
                            //server push pack.
                            SocketPackage package = new SocketPackage();
                            package.MsgId = reader.RmId;
                            package.ActionId = reader.ActionId;
                            package.ErrorCode = reader.StatusCode;
                            package.ErrorMsg = reader.Description;
                            package.Reader = reader;

                            lock (_pushQueue)
                            {
                                _pushQueue.Enqueue(package);
                            }
                        }

                    }

                }
                else if (_socket.Poll(5, SelectMode.SelectError))
                {
                    QueueEventClose();
                    Close();
                    break;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("catch" + ex.ToString());
//                 if (connect)
//                 {
//                     Net.Instance.QueueEvent(EventDef.ServerColsed);
//                 }

                //Close();

            }

            Thread.Sleep(5);

        }

    }

    //public void CheckNetState()
    //{
    //    if (socket == null)
    //    {
    //        return;
    //    }
    //    //DateTime start = DateTime.Now;
    //    UnityEngine.NetworkReachability state = UnityEngine.Application.internetReachability;
    //    if (state == UnityEngine.NetworkReachability.NotReachable)
    //    {
    //        IsNetStateChange = true;
    //        UnityEngine.Debug.Log("IsNetStateChange = true" + state.ToString());
    //    }
    //    else if (NetState != state)//处理3G 2G的情况
    //    {
    //        UnityEngine.Debug.Log("IsNetStateChange = true" + state.ToString());
    //        IsNetStateChange = true;
    //    }
    //    //UnityEngine.Debug.Log("CheckTime" + DateTime.Now.Subtract(start).TotalMilliseconds );
    //}

    /// <summary>
    /// 打开连接
    /// </summary>
    public void Open()
    {
        isConnect = false;
        UnityEngine.NetworkReachability state = UnityEngine.Application.internetReachability;
        if (state != UnityEngine.NetworkReachability.NotReachable)
        {
            String newServerIp = "";
            AddressFamily newAddressFamily = AddressFamily.InterNetwork;
            IPv6SupportMidleware.getIPType(_host, _port.ToString(), out newServerIp, out newAddressFamily);
            if (string.IsNullOrEmpty(newServerIp))
            {
                newServerIp = _host;
            }
            //Debug.LogError("444444444444444444OpenOpenOpenOpenOpenOpenOpenOpen");
            _socket = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {

                _socket.BeginConnect(newServerIp, _port, asyncResult =>
                 {
                     
                     try
                     {
                         _socket.EndConnect(asyncResult);
                         if (_heartbeatThread == null)
                         {
                             _heartbeatThread = new Timer(SendHeartbeatPackage, null, HearInterval, HearInterval);
                            
                         }

                         _thread = new Thread(new ThreadStart(CheckReceive));
                         _thread.Start();
                         isConnect = true;
                         Net.Instance.QueueEvent(EventDef.ServerConnectFinish);
                     }
                     catch (SocketException ex)
                     {
                         isConnect = false;
                         Net.Instance.QueueEvent(EventDef.ServerConnectFailure, ex.ToString());
                     }
                        //_socket.Connect(newServerIp, _port);
                    },null);
            }
            catch
            {
                //socket.Dispose();
                _socket = null;
                throw;
            }
           
        }
    }

    /// <summary>
    /// rebuild socket send hearbeat package 
    /// </summary>
    public SocketPackage ReBuildHearbeat()
    {
        var writer = NetWriter.Instance;
        SprotoType.package headPack = new SprotoType.package()
        {
            protoId = 2,
            session = NetWriter.MsgId,
            errorcode = 0,
        };
        Sproto.SprotoPack pack = new Sproto.SprotoPack();
        byte[] headBytes = headPack.encode();
        writer.SetHeadBuffer(headBytes);
        writer.SetBodyData(new byte[0]);
        var data = writer.PostData();
        NetWriter.resetData();
        _hearbeatPackage = data;
        SocketPackage package = new SocketPackage();
        package.MsgId = (int)headPack.session;
        package.ActionId = 2;
        package.Action = ActionFactory.Create(package.ActionId);
        package.SendTime = DateTime.Now;
        package.HasLoading = false;
        return package;
    }

    private void SendHeartbeatPackage(object state)
    {
        try
        {
            SocketPackage pack = ReBuildHearbeat();
            Send(_hearbeatPackage, pack);
        }
        catch (Exception ex)
        {
            Debug.LogError("发送心跳出现异常");
            Debug.LogException(ex);
        }

    }

    public void EnsureConnected()
    {
        if (_socket == null)
        {
            Open();
        }
        else
        {
            Net.Instance.QueueEvent(EventDef.ServerConnectFinish);
        }
    }
    private readonly string LockString;
    /// <summary>
    /// 关闭连接
    /// </summary>
    public void Close()
    {
        isConnect = false;
        if (_socket == null) return;
        if (_heartbeatThread != null)
        {
            _heartbeatThread.Dispose();
            _heartbeatThread = null;
        }
        try
        {
            lock (this)
            {
                //
                _thread.Abort();
                _thread = null;
                _sendList.Clear();
                _receiveQueue.Clear();
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket = null;
            }
            
        }
        catch (Exception)
        {
            _socket = null;
        }
    }

    void QueueEventClose()
    {
        if (isConnect)
        {
            Net.Instance.QueueEvent(EventDef.ServerColsed);
        }
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="data"></param>
    private bool PostSend(byte[] data)
    {
        //EnsureConnected();
        if (_socket != null)
        {
            //socket.Send(data);
            IAsyncResult asyncSend = _socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(sendCallback), _socket);
            bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
                QueueEventClose();
                Close();
                return false;
            }
            else
            {
                //Debug.LogError("发送数据："+DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss：ffff"));
            }
            //Debug.LogError("发送完成"+ DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss：ffff"));
            return true;
        }
        return false;

    }
    private void sendCallback(IAsyncResult asyncSend)
    {
        
    }
    public void Send(byte[] data, SocketPackage package)
    {
        if (data == null)
        {
            return;
        }
        lock (_sendList)
        {
            _sendList.Add(package);
        }

        try
        {
            PostSend(data);
            //UnityEngine.Debug.Log("Socket send actionId:" + package.ActionId + ", msgId:" + package.MsgId + ", send result:" + bRet);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log("Socket send actionId: " + package.ActionId + " error" + ex);
            package.ErrorCode = (int)ErrorCode.ConnectError;
            package.ErrorMsg = ex.ToString();
            lock (_receiveQueue)
            {
                _receiveQueue.Enqueue(package);
            }
            lock (_sendList)
            {
                _sendList.Remove(package);
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        try
        {
            if (!this._isDisposed)
            {
                if (isDisposing)
                {
                    //if (socket != null) socket.Dispose(true);
                }
            }
        }
        finally
        {
            this._isDisposed = true;
        }
    }

    public void ProcessTimeOut()
    {
        SocketPackage findPackage = null;
        lock (_sendList)
        {
            
            for (int i = 0;i< _sendList.Count;i++)
            {
                if (DateTime.Now.Subtract(_sendList[i].SendTime).TotalSeconds > TimeOut)
                {
                    _sendList[i].ErrorCode = (int)ErrorCode.TimeOutError;
                    _sendList[i].ErrorMsg = "TimeOut";
                    Debug.LogError("手动处理了一个超时的消息 消息id="+ _sendList[i].ActionId);
                    findPackage = _sendList[i];
                    break;
                }
            }
        }
        if (findPackage != null)
        {
            lock (_receiveQueue)
            {
                _receiveQueue.Enqueue(findPackage);
            }
            lock (_sendList)
            {
                _sendList.Remove(findPackage);
            }
        }
    }


}

