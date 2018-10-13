using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePool<T>
{
    public delegate void InitCallbackDelegate(T obj);
    public delegate void ReturnCallbackDelegate(T obj);
    public delegate T CreateCallbackDelegate(Type type);
    public InitCallbackDelegate initCallback;
    public ReturnCallbackDelegate returnCallback;
    public CreateCallbackDelegate createCallback;
    protected Queue<T> _pool;
    protected Transform _manager;
    /// <summary>
    /// 需要设置一个manager，加入池里的对象都成为这个manager的孩子
    /// </summary>
    /// <param name="manager"></param>
    public SimplePool(Transform manager)
    {
        _manager = manager;
        _pool = new Queue<T>();
    }

    public virtual void Clear()
    {
        _pool.Clear();
    }

    public virtual T Get()
    {
        T ret = default(T);
        Type t = typeof(T);
        if (_pool.Count > 0)
            ret = _pool.Dequeue();
        else
        {
            if (createCallback != null)
            {
                ret = createCallback(t);
            }
        }
        if (ret != null && initCallback != null)
        {
            initCallback(ret);
        }
        return ret;
    }

    public virtual void Pop(T obj)
    {
        if (returnCallback != null)
        {
            returnCallback(obj);
        }
        _pool.Enqueue(obj);
        
    }
}

public class SimplePoolGameobject : SimplePool<GameObject>
{
    public SimplePoolGameobject(Transform manager) : base(manager)
    {

    }

    public override void Clear()
    {
        foreach (GameObject obj in _pool)
        {
            GameObject.Destroy(obj);
        }
        base.Clear();
    }

    public override GameObject Get()
    {
        return base.Get();
    }

    public override void Pop(GameObject obj)
    {
#if (UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER)
        obj.transform.SetParent(_manager, false);
#else
			Vector3 p = obj.localPosition;
			Vector3 s = obj.localScale;
			Quaternion q = obj.localRotation;
			obj.parent = _manager;
			obj.localPosition = p;
			obj.localScale = s;
			obj.localRotation = q;
#endif
        base.Pop(obj);
    }
}

public class SimplePoolObject : SimplePool<UnityEngine.Object>
{
    public SimplePoolObject(Transform manager) : base(manager)
    {
    }
    public override void Clear()
    {
        foreach (UnityEngine.Object obj in _pool)
        {
            Component.Destroy(obj);
        }
        base.Clear();
    }

    public override UnityEngine.Object Get()
    {
        return base.Get();
    }

    public override void Pop(UnityEngine.Object obj)
    {

        base.Pop(obj);
    }
}
