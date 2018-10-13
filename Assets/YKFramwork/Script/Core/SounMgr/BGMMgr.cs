using UnityEngine;
using System.Collections;

public class BGMMgr : MonoBehaviour
{
    private bool mIsOn = true;
    private float mVolume = 1;

    public float Volume
    {
        get
        {
            return mVolume;
        }
        set
        {
            mVolume = value > 1 ? 1 : value;
            mVolume = value < 0 ? 0 : value;
            SetVolume();
            PlayerPrefs.SetFloat("BGMVolume", mVolume);
            PlayerPrefs.Save();
        }
    }
    public bool IsOn
    {
        get
        {
            return mIsOn;
        }
        set
        {
            if (mIsOn && !value)
            {
                mIsOn = value;
                PauseBGM();
            }
            else if (!mIsOn && value)
            {
                mIsOn = value;
                Resume();
            }
            PlayerPrefs.SetString("BGMMgrIsON", mIsOn.ToString());
            PlayerPrefs.Save();
        }
    }

    public static BGMMgr Instance
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
        mIsOn = bool.Parse(PlayerPrefs.GetString("BGMMgrIsON", "true"));
        Volume = PlayerPrefs.GetFloat("BGMVolume",0.3f);
        Instance = this;
        scourceCtrls[0] = new SoundSourceCtrl(0);
        scourceCtrls[1] = new SoundSourceCtrl(1);
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    public SoundSourceCtrl [] scourceCtrls = new SoundSourceCtrl [2];

    private string currentMusicName = "";
    public void PlayBGMAsync(string strBGMName)
    {
        currentMusicName = strBGMName;
        if (!IsOn || string.IsNullOrEmpty(strBGMName))
        {
            return;
        }

        int index = FetchASourceIndex();
        SoundSourceCtrl srcCtrl = scourceCtrls[index];
        srcCtrl.curMusicName = strBGMName;

        ResMgr.Intstance.LoadAsset(strBGMName, typeof(AudioClip), a => 
        {
            if (Instance.scourceCtrls[index].curMusicName.Equals(strBGMName))
            {
                Instance.scourceCtrls[index].InitFadeIn(a as AudioClip);
                Instance.scourceCtrls[1 - index].InitFadeOut();
            }
        });

    }

    /// <summary>
    /// 停止背景音乐播放
    /// </summary>
    public void StopBGM()
    {
        for (int i = 0; i < scourceCtrls.Length;i++ )
        {
            scourceCtrls[i].InitFadeOut();
        }
    }

    public void PauseBGM()
    {
        StopBGM();
    }

    public void Resume()
    {
        PlayBGMAsync(currentMusicName);
    }

    private void SetVolume()
    {
        for (int i = 0; i < scourceCtrls.Length; i++)
        {
            if (scourceCtrls[i] != null && (scourceCtrls[i].status == SoundSourceCtrl.BGMSourceStatus.Play))
            {
                scourceCtrls[i].audioSoure.volume = this.Volume;
            }
        }
    }

    /// <summary>
    /// 资源释放
    /// </summary>
    public void Release()
    {
        return;
        for (int i = 0; i < scourceCtrls.Length; i++)
        {
            if (scourceCtrls[i] != null)
            {
                scourceCtrls[i].Release();
            }
        }
    }

    /// <summary>
    /// 寻找一个正在淡入的声音组件
    /// </summary>
    /// <returns></returns>
    private int FetchASourceIndex()
    {
        int ret = 0;
        for (int i = 0; i < scourceCtrls.Length;i++ )
        {
            SoundSourceCtrl ctrl = scourceCtrls[i];
            if (ctrl.status == SoundSourceCtrl.BGMSourceStatus.Slient || ctrl.status == SoundSourceCtrl.BGMSourceStatus.FadeIn)
            {
                ret = i;
                break;
            }
        }
        return ret;
    }

    private float updateGapAccum = 0;
    public void FixedUpdate()
    {
        updateGapAccum += Time.fixedDeltaTime;
        if (updateGapAccum > 0.1f)
        {
            updateGapAccum = 0;
            for (int i = 0; i < scourceCtrls.Length;i++ )
            {
                UpdateSoundSource(i);
            }
            
        }
    }

    private void UpdateSoundSource(int index)
    {
        SoundSourceCtrl ctrl = scourceCtrls[index];
        if (ctrl != null 
            && ctrl.status != SoundSourceCtrl.BGMSourceStatus.Slient
            && ctrl.status != SoundSourceCtrl.BGMSourceStatus.Play)
        {
            if (ctrl.status == SoundSourceCtrl.BGMSourceStatus.FadeIn)
            {
                ctrl.audioSoure.volume += 0.1f;
                if (ctrl.audioSoure.volume >= Volume)
                {
                    ctrl.audioSoure.volume = Volume;
                    ctrl.status = SoundSourceCtrl.BGMSourceStatus.Play;
                }
            }

            if (ctrl.status == SoundSourceCtrl.BGMSourceStatus.FadeOut)
            {
                ctrl.audioSoure.volume -= 0.1f;
                if (ctrl.audioSoure.volume <= 0f)
                {
                    ctrl.audioSoure.volume = 0f;
                    ctrl.status = SoundSourceCtrl.BGMSourceStatus.Slient;
                    ctrl.audioSoure.Stop();
                }
            }
        }
    }

    #region 声音播放控制
    public class SoundSourceCtrl
    {
        /// <summary>
        /// 音效播放组件
        /// </summary>
        public AudioSource audioSoure = null;

        /// <summary>
        /// 播放的状态
        /// </summary>
        public BGMSourceStatus status = BGMSourceStatus.Slient;

        /// <summary>
        /// 时间
        /// </summary>
        public float updateTimeAccum = 0;

        /// <summary>
        /// 当前正在播放的声音
        /// </summary>
        public string curMusicName = null;

        /// <summary>
        /// 序号
        /// </summary>
        public int index = 0;
        public enum BGMSourceStatus
        {
            /// <summary>
            /// 正在淡入
            /// </summary>
            FadeIn,

            /// <summary>
            /// 正在淡出
            /// </summary>
            FadeOut,

            /// <summary>
            /// 停止
            /// </summary>
            Slient,

            /// <summary>
            /// 正在播放
            /// </summary>
            Play,
        }

        public SoundSourceCtrl(int _index)
        {
            audioSoure = Instance.gameObject.AddComponent<AudioSource>();
            audioSoure.loop = true;
            audioSoure.volume = 0;
            /*audioSoure.volume = 1.0f;*/
            //声音大小//
            index = _index;
        }

        public void InitFadeIn(AudioClip clip)
        {
           
            audioSoure.clip = clip;
            status = BGMSourceStatus.FadeIn;
            if (audioSoure.clip != null)
            {
                audioSoure.Play();
            }
        }

        public void InitFadeOut()
        {
            if(audioSoure.clip != null)
            status = BGMSourceStatus.FadeOut;
        }

        public void Release()
        {
            if (!audioSoure.isPlaying && audioSoure != null)
            {
                audioSoure.clip = null;
                status = BGMSourceStatus.Slient;
            }
            
        }
    }
    #endregion
}
