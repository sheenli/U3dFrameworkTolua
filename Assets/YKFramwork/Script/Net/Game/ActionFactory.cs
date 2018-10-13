using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 游戏Action处理工厂
/// </summary>
public abstract class ActionFactory
{
    private static Hashtable lookupType = new Hashtable();
    private static string ActionFormat = "Action{0}";

    public static GameAction Create(object actionType)
    {
        return Create((int)actionType);
    }

    public static GameAction Create(int actionId)
    {
        GameAction gameAction = new BaseAction(actionId);
        return gameAction;
    }
}
