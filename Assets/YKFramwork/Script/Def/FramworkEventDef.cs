using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 加载资源的回调
/// </summary>
public class ResLoadGroupEvent : EventData<string>
{
    public const string LoadGroupFinish = "FramworkEventDefLoadGroupFinish";
    public ResLoadGroupEvent(string groupName) : base(LoadGroupFinish, groupName)
    {

    }
}

/// <summary>
/// 加载资源组出错
/// </summary>
public class ResGroupLoadError : EventData<string>
{
    public const string eventName = "ResGroupLoadError";
    public ResGroupLoadError(string groupName) : base(eventName, groupName)
    {

    }
}

public struct LoadGroupItemErrorInfo
{
    public string groupName;
    public string itemName;
    public LoadGroupItemErrorInfo(string groupName, string itemName)
    {
        this.groupName = groupName;
        this.itemName = itemName;
    }
}

/// <summary>
/// 加载资源组里面单个资源出错
/// </summary>
public class LoadGroupItemError : EventData<LoadGroupItemErrorInfo>
{
    public const string eventName = "LoadGroupItemError";
    public LoadGroupItemError(string groupName,string itemName) : base(eventName, new LoadGroupItemErrorInfo(groupName,itemName))
    {

    }
}

public struct GroupProgress
{
    public GroupProgress(string groupName,int allCount, int currentCout)
    {
        this.groupName = groupName;
        this.allCount = allCount;
        this.currentCout = currentCout;
    }
    public string groupName;
    public int allCount;
    public int currentCout;
    public float Progress
    {
        get
        {
            return (float)allCount / (float)currentCout;
        }
    }
}

/// <summary>
/// 资源加载进度
/// </summary>
public class LoadGroupProgress : EventData<GroupProgress>
{
    public const string eventName = "LoadGroupProgress";
    public LoadGroupProgress(string groupName,int allCount,int currentCout) : base(eventName, new GroupProgress(groupName,allCount,currentCout))
    {

    }
}


public class ServerErrorEvent : EventData<ErrorData>
{
    public ServerErrorEvent(ErrorData value) : base(EventDef.ServerError.ToString(), value)
    {

    }
}


public class GetGPSFinishEvent : EventData<GetGPSFinishEvent.GPSLocationInfo>
{
    public static string evenId = "GetGPSFinishEvent";
    public GetGPSFinishEvent(GPSLocationInfo value) : base(evenId, value)
    {

    }

    public struct GPSLocationInfo
    {
        public float longitude;
        public float latitude;
    }
}

public class ServerResponseEvent : EventData<GameServerMsg>
{
    public ServerResponseEvent(GameServerMsg value) : base(EventDef.ServerResponse.ToString(),value)
    {

    }
}

public partial class GamUtil
{
    public static Vector2 CenterPivot = new Vector2(0.5f, 0.5f);
    public static Vector2 UIStartScale = new Vector2(0.6f,0.6f);
}

