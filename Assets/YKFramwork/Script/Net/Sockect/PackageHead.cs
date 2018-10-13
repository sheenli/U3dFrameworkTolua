using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 
/// </summary>
public class PackageHead
{
    public long StatusCode
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }

    public long ActionId
    {
        get;
        set;
    }

    public long MsgId
    {
        get;
        set;
    }

    public string SessionId { get; set; }
    public int UserId { get; set; }

    /// <summary>
    /// st
    /// </summary>
    public string StrTime
    {
        get;
        set;
    }
}
