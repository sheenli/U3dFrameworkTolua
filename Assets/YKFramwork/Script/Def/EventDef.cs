using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EventDef
{
    #region 网络相关
    /// <summary>
    /// 服务器断开连接
    /// </summary>
    public const int ServerColsed = 2000;
    /// <summary>
    /// 服务器连接成功
    /// </summary>
    public const int ServerConnectFinish = ServerColsed + 1;

    /// <summary>
    /// 与服务连接失败
    /// </summary>
    public const int ServerConnectFailure = ServerConnectFinish + 1;

    /// <summary>
    /// 服务器请求超时
    /// </summary>
    public const int ServerRequestTimeout = ServerConnectFailure + 1;

    /// <summary>
    /// 服务器错误
    /// </summary>
    public const int ServerError = ServerRequestTimeout + 1;
    
    /// <summary>
    /// 登录完成
    /// </summary>
    public const int SendLoginCompleted = ServerError + 1;
    /// <summary>
    /// 服务器消息返回
    /// </summary>
    public const int ServerResponse = SendLoginCompleted + 1;
    #endregion
}