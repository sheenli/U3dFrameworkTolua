using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SoundMgr : MonoBehaviour 
{

    /// <summary>
    /// 所有播放音效的组件
    /// </summary>
    private List<AudioSource> mAudioSources = new List<AudioSource>();

    private bool mIsOn = false;
    public float Volume
    {
        get
        {
            return PlayerPrefs.GetFloat("SoundVolume",1.0f);
        }
        set
        {
            FairyGUI.UIConfig.buttonSoundVolumeScale = value;
            FairyGUI.Stage.inst.soundVolume = value;
            PlayerPrefs.SetFloat("SoundVolume", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 音乐总开关
    /// </summary>
    public bool IsOn
    {
        get
        {
            return mIsOn;
        }
        set
        {
            mIsOn = value;

            if (!mIsOn) FairyGUI.Stage.inst.soundVolume = 0;
            else FairyGUI.Stage.inst.soundVolume = Volume;
            PlayerPrefs.SetString("SoundMgrIsON", mIsOn.ToString());
            PlayerPrefs.Save();
        }
    }

    public static SoundMgr Instance
    {
        private set;
        get;
    }


    public static bool IsValid
    {
        get
        {
            return Instance != null;
        }
    }

    public void Awake()
    {
        mIsOn = bool.Parse(PlayerPrefs.GetString("SoundMgrIsON", "true"));
        Instance = this;
        Volume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        FairyGUI.UIConfig.buttonSoundVolumeScale = Volume;
        if (!mIsOn) FairyGUI.Stage.inst.soundVolume = 0;
        for (int i = 0; i < 10;i++ )
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            mAudioSources.Add(source);
        }
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    /// <summary>
    /// 播放一段音效
    /// </summary>
    /// <param name="audioClipName">音效名称</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="listener">加载完成后的回调</param>
    public void PlayAudioClipAsync(string audioClipName,bool isLoop = false,Action<UnityEngine.Object> listener = null)
    {
        if (string.IsNullOrEmpty(audioClipName))
        {
            return;
        }
        AudioClip clip = TryToFindAudioClip(audioClipName);
        if (clip != null)
        {
            AudioSource source = PlayAudioClip(clip,isLoop);
            if (listener != null)
            {
                listener(source);
            }
        }
        else
        {
            ResMgr.Intstance.LoadAsset(audioClipName, typeof(AudioClip), a => 
            {
                clip = a as AudioClip;
                AudioSource source = PlayAudioClip(clip, isLoop);
                if (listener != null)
                {
                    listener(source);
                }
            });
        }
    }

    /// <summary>
    /// 播放一个音效
    /// </summary>
    /// <param name="clip">片段</param>
    /// <param name="isLoop">是否循环</param>
    /// <returns></returns>
    public AudioSource PlayAudioClip(AudioClip clip,bool isLoop)
    {
        if (IsOn)
        {
            if (clip == null)
            {
                Debug.Log("你要播放的音效片段不存在无法播放");
                return null;
            }
            else
            {
                AudioSource source = GetNotPlayAudioSource();
                source.clip = clip;
                source.loop = isLoop;
                source.volume = this.Volume;
                source.Play();
                return source;
            }
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 获取一个没有在播放的音效组件
    /// </summary>
    /// <returns></returns>
    private AudioSource GetNotPlayAudioSource()
    {
        if (mAudioSources != null)
        {
            for (int i = 0; i < mAudioSources.Count;i++ )
            {
                if (!mAudioSources[i].isPlaying)
                {
                    return mAudioSources[i];
                }
            }
        }

        AudioSource audio = gameObject.AddComponent<AudioSource>();
        mAudioSources.Add(audio);
        return audio;
    }

    /// <summary>
    /// 尝试从已经有的音效组件上去获取一个音效
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    private AudioClip TryToFindAudioClip(string clipName)
    {
        if (mAudioSources != null)
        {
            for (int i = 0; i < mAudioSources.Count; i++)
            {
                if (mAudioSources[i].clip != null && mAudioSources[i].clip.name.Equals(clipName))
                {
                    return mAudioSources[i].clip;
                }
            }
        }
        return null;
    }

    public void Release()
    {
        for (int i = 0; i < mAudioSources.Count;i++ )
        {
            if (mAudioSources[i] != null && !mAudioSources[i].isPlaying)
            {
                if (mAudioSources[i].clip != null)
                {
                    mAudioSources[i].clip = null;
                }
            }
        }
    }

}
