using SprotoType;
using System;

/// <summary>
/// 自定结构Action代理基类
/// </summary>
public class BaseAction : GameAction
{
    public BaseAction(int actionId)
        : base(actionId)
    {
        
    }

    protected override void SetActionHead(NetWriter writer)
    {
        //todo 启用自定的结构
        package headPack = new package()
        {
            session = Head.MsgId,
            protoId = ActionId,
            errorcode = 0,
        };
        byte[] data = headPack.encode();
        writer.SetHeadBuffer(data);
        writer.SetBodyData(null);
    }
}
