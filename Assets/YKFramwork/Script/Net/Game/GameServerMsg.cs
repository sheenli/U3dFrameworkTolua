using UnityEngine;
using System.Collections;

public class GameServerMsg 
{
    public GameServerMsg(NetPackage _pack, byte[] _result)
    {
        pack = _pack;
        resultData = _result;
    }
    /// <summary>
    /// 消息头信息 例如mesid
    /// </summary>
    public NetPackage pack;
    /// <summary>
    /// 消息的主体内容
    /// </summary>
    public byte[] resultData;
}
