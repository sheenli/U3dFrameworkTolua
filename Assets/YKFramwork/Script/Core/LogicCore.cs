using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicCore : EventDispatcherNode
{
    public static LogicCore Instance
    {
        get;
        private set;
    }

    public void Awake()
    {
        Instance = this;
        //全局对象
        DontDestroyOnLoad(gameObject);
    }

}
