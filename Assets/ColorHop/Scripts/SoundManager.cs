using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private const string PREF_SFX_ON = "sfx_on";
    private const string PREF_MUSIC_ON = "music_on";

    private List<AudioSource> sfxSources = new List<AudioSource>();
    private AudioSource musicSource;

    private bool sfxOn = true;
    private bool musicOn = true;

    public bool SfxOn
    {
        get { return sfxOn; }
        set
        {
            sfxOn = value;
            PlayerPrefs.SetInt(PREF_SFX_ON, sfxOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool MusicOn
    {
        get { return musicOn; }
        set
        {
            musicOn = value;
            PlayerPrefs.SetInt(PREF_MUSIC_ON, musicOn ? 1 : 0);
            PlayerPrefs.Save();
            if (musicSource != null) musicSource.mute = !musicOn;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxOn = PlayerPrefs.GetInt(PREF_SFX_ON, 1) == 1;
        musicOn = PlayerPrefs.GetInt(PREF_MUSIC_ON, 1) == 1;

        for (int i = 0; i < 5; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sfxSources.Add(source);
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.mute = !musicOn;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (!sfxOn) return;
        if (clip == null)
        {
            Debug.LogWarning("PlaySFX called with null clip");
            return;
        }

        AudioSource free = GetFreeSource();
        free.clip = clip;
        free.volume = volume;
        free.pitch = 1f;
        free.Play();
    }

    public void PlaySFXPitched(AudioClip clip, float pitch, float volume = 1f)
    {
        if (!sfxOn) return;
        if (clip == null)
        {
            Debug.LogWarning("PlaySFXPitched called with null clip");
            return;
        }

        AudioSource free = GetFreeSource();
        free.clip = clip;
        free.volume = volume;
        free.pitch = pitch;
        free.Play();
    }

    private AudioSource GetFreeSource()
    {
        for (int i = 0; i < sfxSources.Count; i++)
        {
            if (!sfxSources[i].isPlaying) return sfxSources[i];
        }
        return sfxSources[0];
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.Play();
    }
}
