using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeDelay : MonoBehaviour
{
    public static TimeDelay Instance
    {
        private set;
        get;
    }

    public void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 所有计时器
    /// </summary>
    private List<TimeDelayData> timeDelayDatas = new List<TimeDelayData>();
    public void AttachTimeDelay(TimeDelayData data)
    {
        if (!timeDelayDatas.Contains(data))
        {
            timeDelayDatas.Add(data);
        }
    }

    public void DetachTimeDelay(TimeDelayData data)
    {
        if (timeDelayDatas.Contains(data))
        {
            timeDelayDatas.Remove(data);
        }
    }

    public void Update()
    {
        for (int i = 0; i < timeDelayDatas.Count;i++ )
        {
            timeDelayDatas[i].OnUpdate();
        }
    }

    public void FixedUpdate()
    {
        for (int i = 0; i < timeDelayDatas.Count; i++)
        {
            timeDelayDatas[i].OnFixedUpdate();
        }
    }

    public delegate void DelayCallback(System.Object obj);

    public class TimeDelayData
    {
        public TimeDelayData(float timeToDelay, DelayCallback delayCallback,
            bool loop,bool unTimeScale, System.Object newObj)
        {
            mLoop = loop;
            mUnTimeScale = unTimeScale;
            obj = newObj;
            StartCounting(timeToDelay, delayCallback);
        }

        /// <summary>
        /// 是否循环
        /// </summary>
        private bool mLoop = false;

        /// <summary>
        /// 不受TimeScale 影响
        /// </summary>
        private bool mUnTimeScale = false;

        /// <summary>
        /// 多久的时间执行
        /// </summary>
        private float mTimeToDelay = 0;

        /// <summary>
        /// 当前已经运行多久了
        /// </summary>
        private float mTimeAccum = 0;
        
        /// <summary>
        /// 是否开始计时了
        /// </summary>
        private bool isAccuming = false;

        /// <summary>
        /// 回调的一个对象
        /// </summary>
        private System.Object obj = null;

        /// <summary>
        /// 计时回调
        /// </summary>
        private DelayCallback callback = null;

        private void StartCounting(float time, DelayCallback delayCallback)
        {
            mTimeAccum = 0;
            mTimeToDelay = time;
            isAccuming = true;
            callback = delayCallback;
        }


        public void OnUpdate()
        {
            if (isAccuming)
            {
                if (mUnTimeScale)
                {
                    mTimeAccum += Time.unscaledDeltaTime;
                }
                else
                {
                    mTimeAccum += Time.deltaTime;
                }
                
                isAccuming = !(mTimeAccum > mTimeToDelay);
                if (!isAccuming)
                {
                    if (callback != null) callback(obj);
                    if (Instance != null) Instance.DetachTimeDelay(this);

                    if (mLoop)
                    {
                        if (Instance != null)
                        {
                            StartCounting(mTimeToDelay, callback);
                            Instance.AttachTimeDelay(this);
                        }
                    }
                }
            }
        }

        public void OnFixedUpdate()
        {

        }
    }

    /// <summary>
    /// 添加一个计时
    /// </summary>
    /// <param name="timeToDelay">多久的时间</param>
    /// <param name="delayCallback">回调</param>
    /// <param name="unTimeScale">是否忽略TimeScale</param>
    /// <param name="obj">对象</param>
    /// <returns></returns>
    public static TimeDelayData Delay(float timeToDelay, DelayCallback delayCallback,bool loop = false, bool unTimeScale = false, System.Object obj = null)
    {
        TimeDelayData timeData = null;
        if (delayCallback != null)
        {
            timeData = new TimeDelayData(timeToDelay, delayCallback, loop, unTimeScale, obj);
            Instance.AttachTimeDelay(timeData);
        }
        return timeData;
    }

    public void Destroy()
    {
        if (Instance != null)
        {
            Instance = null;
            Object.Destroy(this);
        }
    }


}
