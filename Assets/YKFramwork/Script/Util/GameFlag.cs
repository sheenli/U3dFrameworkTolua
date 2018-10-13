// **********************************************************************
// 
// 文件名(File Name)：             GmagFalag.cs
// 
// 作者(Author)：                  Sheen
// 
// 创建时间(CreateTime):           2015/9/21 21:27:0
//
// **********************************************************************

using UnityEngine;
using System.Collections;

public class GameFlag
{
    /// <summary>
    /// 标志量
    /// </summary>
    private long mValue = 0;

    /// <summary>
    /// 标志量
    /// </summary>
    public long Value
    {
        get
        {
            return mValue;
        }
        set
        {
            mValue = value;
        }
    }

    public GameFlag()
    {

    }

    public GameFlag(long flag)
    {
        mValue = flag;
    }

    /// <summary>
    /// 添加一个标志量
    /// </summary>
    /// <param name="flag">要添加的标志量</param>
    /// <returns></returns>
    public long AddFlag(long flag)
    {
        mValue |= flag;
        return mValue;
    }

    /// <summary>
    /// 移除一个标志量
    /// </summary>
    /// <param name="flag">标志量</param>
    /// <returns></returns>
    public long RemoveFlag(long flag)
    {
        mValue &= ~flag;
        return mValue;
    }

    /// <summary>
    /// 添加或移除一个标志量
    /// </summary>
    /// <param name="remove">是否移除</param>
    /// <param name="flag">标志量</param>
    /// <returns></returns>
    public long ModifyFlag(bool remove,long flag)
    {
        mValue = remove ? RemoveFlag(flag) : AddFlag(flag);
        return mValue;
    }

    /// <summary>
    /// 某个标志量是否存在
    /// </summary>
    /// <param name="flag">标志量</param>
    /// <returns></returns>
    public bool HasFlag(long flag)
    {
        return ((mValue & flag) != 0);
    }
}