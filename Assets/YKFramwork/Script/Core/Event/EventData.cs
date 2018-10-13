using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 发送注册事件
/// </summary>
public class EventData
{
    public bool isBreak = false;
    /// <summary>
    /// 时间名称或者ID
    /// </summary>
    public string name;

    /// <summary>
    /// 事件发送的类型
    /// </summary>
    public virtual object value { get; set; }
    public EventData(string name)
    {
        this.name = name;
    }

    public void Break()
    {
        isBreak = true;
    }
}

/// <summary>
/// 泛型的事件类型
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventData<T> : EventData
{
    public EventData(string name, T value) : base(name)
    {
        this.value = value;
    }
}

public class EventDataLua: EventData
{
    public object obj = null;
    public override object value
    {
        get
        {
            return obj;
        }

        set
        {
            obj = value;
        }
    }
    public EventDataLua(string name) : base(name)
    {
        
    }
}
