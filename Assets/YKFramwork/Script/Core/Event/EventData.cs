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
    public T value;
    public EventData(string name, T v = default(T)) : base(name)
    {
        this.value = v;
    }
}

public class EventDataLua: EventData<object>
{
    public object obj
    {
        get
        {
            return this.value;
        }

        set
        {
            this.value = value;
        }
    }
    public EventDataLua(string name) : base(name)
    {
        
    }
}
