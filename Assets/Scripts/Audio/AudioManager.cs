using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundData
{
    public AudioSource audio_source = null;
    public string sFile;
}

public class AudioManager : TSingleton<AudioManager> {

    const int MAX_AUDIO_SOURCE_CNT = 10;         //最多存在的音效的音频源个数，不包括音乐的音频源
    int m_iSoundUniqueId = 1;          //正在播放的音效的唯一标识，PlaySound时生成该值，可用于StopSound
    int SoundUniqueId
    {
        get
        {
            if (m_iSoundUniqueId == 0)
            {//保证一定不能为0
                m_iSoundUniqueId++;
            }
            return m_iSoundUniqueId++;
        }
    }

    bool m_SoundOn = true;
    bool m_MusicOn = true;

    //音效是否开启
    public bool SoundOn
    {
        get
        {
            return m_SoundOn;
        }
        set
        {
            m_SoundOn = value;
            if (!value)
            {
                StopSounds();
            }
        }
    }

    //音乐是否开启
    public bool MusicOn
    {
        get
        {
            return m_MusicOn;
        }
        set
        {
            m_MusicOn = value;
            if (value)
            {
                if (!m_oMusicAudioSource.isPlaying && m_sMusicFile != null)
                {
                    AudioClip clip = GetAudioClip(m_sMusicFile);
                    if (clip != null)
                    {
                        m_oMusicAudioSource.clip = clip;
                        m_oMusicAudioSource.loop = true;
                        m_oMusicAudioSource.Play();
                    }
                }
                m_oMusicAudioSource.volume = m_fMusicVolume;
            }
            else
            {
                m_oMusicAudioSource.volume = 0.0f;
            }
        }
    }

    float m_fLastCheckTime = 0.0f;
    float m_fMusicVolume = 0.5f;        //音乐音量
    float m_fSoundVolume = 0.6f;        //音效音量
    float m_fSkillSoundVolume = 0.5f;   //技能音效音量

    AudioSource m_oMusicAudioSource = null;      //专门用于循环播放音乐的音频源
    string m_sMusicFile;

    GameObject m_AudioObject = null;
    AudioListener m_AudioListener = null;
    AudioSource m_MusicAudioSource = null;      //专门用于循环播放音乐的音频源

    Dictionary<int, SoundData> m_dicPlayingAudioSource = new Dictionary<int, SoundData>();  //正在播放的音效的音频源
    Dictionary<string, bool> m_dicPlayingSoundFile = new Dictionary<string, bool>();
    Queue<SoundData> m_queIdleAudioSource = new Queue<SoundData>();    //空闲的音效的音频源
    List<int> m_lstForDel = new List<int>();  //辅助删除正在播放的音效的音频源的list

    public void Init()
    {
        m_AudioObject = new GameObject("AudioSysGameObject");
        m_AudioListener = m_AudioObject.AddComponent<AudioListener>();
        m_MusicAudioSource = m_AudioObject.AddComponent<AudioSource>();
        m_MusicAudioSource.volume = m_fMusicVolume;
        m_MusicAudioSource.rolloffMode = AudioRolloffMode.Custom;

        for (int i = 0; i < MAX_AUDIO_SOURCE_CNT; ++i)
        {
            SoundData soundData = new SoundData();
            soundData.audio_source = m_AudioObject.AddComponent<AudioSource>();
            soundData.audio_source.volume = m_fSoundVolume;
            soundData.audio_source.rolloffMode = AudioRolloffMode.Custom;
            m_queIdleAudioSource.Enqueue(soundData);
        }
        Object.DontDestroyOnLoad(m_AudioObject);
    }

    public int PlaySound(string sFile)
    {
        return PlaySound(sFile, false, m_fSoundVolume);
    }
    public int PlaySound(string sFile, bool bIsLoop)
    {
        return PlaySound(sFile, bIsLoop, m_fSoundVolume);
    }
    public int PlaySound(string sFile, Vector3 position)
    {
        return PlaySound(sFile, false, m_fSkillSoundVolume);
    }
    public int PlaySound(AudioClip audioClip, string sFile)
    {
        if (audioClip == null)
        {
            Debuger.LogError("audioClip  null");
            return 0;
        }

        if (m_queIdleAudioSource.Count == 0)
        {
            return 0;
        }

        SoundData soundData = m_queIdleAudioSource.Dequeue();
        AudioSource audio_source = soundData.audio_source;
        if (audioClip == null)
        {
            m_queIdleAudioSource.Enqueue(soundData);
            return 0;
        }

        soundData.sFile = sFile;
        audio_source.clip = audioClip;
        audio_source.volume = m_fSoundVolume;
        audio_source.loop = false;
        audio_source.Play();
        int id = SoundUniqueId;
        m_dicPlayingAudioSource.Add(id, soundData);
        m_dicPlayingSoundFile.Add(sFile, true);
        return id;
    }

    //播放音效，一次性播放
    private int PlaySound(string sFile, bool bIsLoop, float fVolume)
    {
        if (!SoundOn)
        {//没有开启音效，直接返回
            return 0;
        }

        if (m_dicPlayingSoundFile.ContainsKey(sFile))
        {
            return 0;
        }

        if (m_queIdleAudioSource.Count == 0)
        {
            return 0;
        }
        SoundData soundData = m_queIdleAudioSource.Dequeue();
        AudioSource audio_source = soundData.audio_source;
        AudioClip clip = GetAudioClip(sFile);
        if (clip == null)
        {
            m_queIdleAudioSource.Enqueue(soundData);
            return 0;
        }
        soundData.sFile = sFile;
        audio_source.clip = clip;
        audio_source.volume = fVolume;
        audio_source.loop = bIsLoop;
        audio_source.Play();
        int id = SoundUniqueId;
        m_dicPlayingAudioSource.Add(id, soundData);
        m_dicPlayingSoundFile.Add(sFile, true);
        return id;
    }

    public int PlayFollowingSound(BaseActor Actor, string sFile)
    {
        return PlaySound(sFile, false, m_fSkillSoundVolume);
    }

    public bool PlayBGMusic(string sFile)
    {
        return PlayMusic(sFile);
    }

    //播放音乐，该接口循环播放同一个音乐
    public bool PlayMusic(string sFile)
    {
        if (m_sMusicFile == sFile)
        {
            return true;
        }
        m_sMusicFile = sFile;
        if (!MusicOn)
        {
            return true;
        }
        AudioClip clip = GetAudioClip(sFile);
        if (clip == null)
        {
            return false;
        }

        if (m_oMusicAudioSource.isPlaying)
        {
            m_oMusicAudioSource.Stop();
        }

        m_oMusicAudioSource.clip = clip;
        m_oMusicAudioSource.loop = true;
        m_oMusicAudioSource.Play();

        return true;
    }

    public void StopSound(int iSoundUniqueId)
    {
        SoundData soundData;
        if (m_dicPlayingAudioSource.TryGetValue(iSoundUniqueId, out soundData))
        {
            AudioSource audio_source = soundData.audio_source;
            if (audio_source.isPlaying)
            {
                audio_source.Stop();
            }
            m_queIdleAudioSource.Enqueue(soundData);
            m_dicPlayingAudioSource.Remove(iSoundUniqueId);
            m_dicPlayingSoundFile.Remove(soundData.sFile);
        }
    }

    public void PauseMusic()
    {
        if (m_oMusicAudioSource.isPlaying)
        {
            //m_oMusicAudioSource.Pause();
            m_oMusicAudioSource.volume = 0.0f;
        }
    }

    private void StopSounds()
    {
        foreach (KeyValuePair<int, SoundData> pair in m_dicPlayingAudioSource)
        {
            if (!pair.Value.audio_source.isPlaying)
            {
                m_queIdleAudioSource.Enqueue(pair.Value);
            }
        }
        m_dicPlayingAudioSource.Clear();
        m_dicPlayingSoundFile.Clear();
    }

    private AudioClip GetAudioClip(string sFile)
    {
        AudioClip clip = Resources.Load(sFile) as AudioClip;
        return clip;
    }

    public void OnUpdate()
    {
        CheckPlayingSounds();
        //             if ( !m_oMusicAudioSource.isPlaying && MusicOn )
        //             {
        //                 m_oMusicAudioSource.Play();
        //             }
    }

    private void CheckPlayingSounds()
    {
        if (UnityEngine.Time.time < m_fLastCheckTime + 0.1f)
        {
            return;
        }
        m_fLastCheckTime = UnityEngine.Time.time;
        m_lstForDel.Clear();
        foreach (KeyValuePair<int, SoundData> pair in m_dicPlayingAudioSource)
        {
            if (!pair.Value.audio_source.isPlaying)
            {
                m_lstForDel.Add(pair.Key);
                m_queIdleAudioSource.Enqueue(pair.Value);
                m_dicPlayingSoundFile.Remove(pair.Value.sFile);
            }
        }

        foreach (int id in m_lstForDel)
        {
            m_dicPlayingAudioSource.Remove(id);
        }
    }
}
