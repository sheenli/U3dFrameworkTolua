using System;
using UnityEngine;

/// <summary>
/// 游戏Action接口
/// </summary>
public abstract class GameAction
{
    private readonly int _actionId;

    protected GameAction(int actionId)
    {
        Head = new PackageHead() { ActionId = actionId };
    }

    public long ActionId
    {
        get { return Head.ActionId; }
    }
    public event Action<ActionResult> Callback;
    public PackageHead Head { get; private set; }

    public byte[] Send(byte[] bs)
    {
        NetWriter writer = NetWriter.Instance;
        SetActionHead(writer);
        writer.SetBodyData(bs);
        return writer.PostData();
    }

    protected byte[] mResultDate = null;
    /// <summary>
    /// 尝试解Body包
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public bool TryDecodePackage(NetReader reader)
    {
        try
        {
            mResultDate = reader.Buffer;
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log(string.Format("Action {0} decode package error:{1}", ActionId, ex));
            return false;
        }
    }

    public void OnCallback(ActionResult result)
    {
        try
        {
            if(Callback != null)
			{
				Callback(result);	
			}
        }
        catch (Exception ex)
        {
            Debug.Log(string.Format("Action {0} callback process error:{1}", ActionId, ex));
        }
    }


    protected virtual void SetActionHead(NetWriter writer)
    {
        writer.writeInt32("actionId", (int)ActionId);
    }

    /// <summary>
    /// 获取返回的消息内容
    /// </summary>
    /// <returns></returns>
    public byte[] GetResponseData()
    {
        return mResultDate;
    }


}
