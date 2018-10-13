using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class BuildCfgAttribute : System.Attribute
{
    /// <summary>
    /// 打包平台
    /// </summary>
    public BuildTarget target;
    /// <summary>
    /// 打包合集
    /// </summary>
    public int collectionID;
    /// <summary>
    /// 渠道id
    /// </summary>
    public int channel;
    /// <summary>
    /// 优先级别
    /// </summary>
    public int priority;
    public BuildCfgAttribute(BuildTarget t,
        int _collectionID,int _channel,int _priority = 0)
    {
        target = t;
        collectionID = _collectionID;
        channel = _channel;
        priority = _priority;
    }

    public static List<BuildCfgAttributeInfo> GetAllAttribute(BuildTarget target,
        int _channel, int _collectionID,out BuildCfgAttributeInfo buildPlayersetting)
    {
        buildPlayersetting = null;
        List<Type> types = CustomSettings.GetcustomTypesList<BuildCfgAttribute, Type>();
        List<BuildCfgAttributeInfo> sortList = new List<BuildCfgAttributeInfo>();
        foreach (Type t in types)
        {
            object[] objs = t.GetCustomAttributes(typeof(BuildCfgAttribute), false);
            
            if (objs != null && objs.Length > 0)
            {
                BuildCfgAttributeInfo ba = new BuildCfgAttributeInfo();
                ba.att = objs[0] as BuildCfgAttribute;
                ba.type = t;
                ba.obj = System.Activator.CreateInstance(t) as IBuildCfg;
                if (ba.att.target != target
                    || (ba.att.collectionID != -1 &&ba.att.collectionID != _collectionID)
                    || (ba.att.channel != -1 && ba.att.channel != _channel)
                    )
                {
                    continue;
                }

                int pos = 0;
                for (int i = 0; i < sortList.Count; i++)
                {
                    if (sortList[i].att.priority < ba.att.priority)
                    {
                        break;
                    }
                    pos++;
                }
                if (t.IsDefined(typeof(BuildSetBuildOptionAndScenesAttribute),false))
                {
                    buildPlayersetting = ba;
                }
                
                sortList.Insert(pos, ba);
            }
        }
        return sortList;
    }

    public override string ToString()
    {
        return "target="+ target+ "/collectionID="+ collectionID+ "/channel="+ channel+ "/priority="+ priority;
    }
}

public class BuildSetBuildOptionAndScenesAttribute : System.Attribute
{

}

public class BuildCfgAttributeInfo
{
    public Type type;
    public IBuildCfg obj;
    public BuildCfgAttribute att;
    public override string ToString()
    {
        return "type=" + type + "/obj =" + obj + "att=" + att;
    }
}

public abstract class IBuildCfg
{
    public virtual string[] BuildScenes { set; get; }
    public virtual BuildOptions BuildOptions { set; get; }
    public abstract void BuildBefore();

    public abstract void BuildAfter(string buildPath);
}
