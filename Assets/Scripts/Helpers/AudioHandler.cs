using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioHandlerHelper : MonoBehaviour
{
    public Vector3 defaultAudioPoint = new Vector3(0, 0, -10);
    public Rect hearableArea = new Rect(-3f, -2.75f, 6f, 5.5f);
    public bool autoCleanup = false;

    private IDictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private IDictionary<string, float> _audioTimes = new Dictionary<string, float>();
    private string _currentMusicName;
    private AudioSource _music;

    void Start()
    {
        //gameObject.AddComponent<AudioSource> ();
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /*
    public void OnLevelWasLoaded(int unused) {
        Cleanup ();
    }
    */

    void OnEnable()
    {
        if (autoCleanup)
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        if (autoCleanup)
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Cleanup();
    }

    public void Cleanup()
    {
        foreach (var item in _audioClips)
        {
            Resources.UnloadAsset(item.Value);
        }

        _audioClips.Clear();
    }

    public bool Load(string audioName)
    {
        if (_audioClips.ContainsKey(audioName))
            return true;

        var audioClip = Resources.Load("Audio/" + audioName) as AudioClip;
        if (audioClip == null)
            return false;

        _audioClips.Add(audioName, audioClip);

        return true;
    }

    public AudioClip GetAudioClip(string audioName)
    {
        if (!_audioClips.ContainsKey(audioName))
        {
            if (!Load(audioName))
                return null;
        }

        return _audioClips[audioName];
    }

    public bool Play(string audioName)
    {
        return Play(audioName, defaultAudioPoint);
    }

    public bool Play(string audioName, float volume)
    {
        return Play(audioName, defaultAudioPoint, volume);
    }

    public bool Play(string audioName, Vector3 point, float volume = 1.0f)
    {
        var audioClip = GetAudioClip(audioName);
        if (audioClip == null)
            return false;

        PlayAudioSource(audioClip, point, volume, audioName);

        return true;
    }

    public void PlayAudioSource(AudioClip audioClip, Vector3 point, float volume, string audioName = "")
    {
        if (!hearableArea.Contains(point))
            return;

        if (!string.IsNullOrEmpty(audioName))
        {
            var time = Time.time;

            if (_audioTimes.ContainsKey(audioName))
            {
                var lastPlayTime = _audioTimes[audioName];
                if (time - lastPlayTime < 0.1f)
                    return;
            }

            _audioTimes[audioName] = time;
        }

        //AudioSource.PlayClipAtPoint (audioClip, defaultAudioPoint, volume);

        var newGameObject = new GameObject("SoundEffect");
        newGameObject.AddComponent<AutoKillAudio>();
        newGameObject.transform.position = point;

        var audioSource = newGameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.spatialBlend = 0f;
        audioSource.Play();

        DontDestroyOnLoad(newGameObject);
    }

    private void EnsureMusicAudioSource()
    {
        if (_music != null)
            return;

        var musicGameObject = new GameObject("Music");
        musicGameObject.transform.position = defaultAudioPoint;

        _music = musicGameObject.AddComponent<AudioSource>();
        _music.spatialBlend = 0f;
        _music.volume = 0.7f;
        _music.loop = true;
        _music.playOnAwake = false;

        DontDestroyOnLoad(musicGameObject);
    }

    public bool PlayMusic(string audioName, float? volume = null)
    {
        if (_currentMusicName == audioName)
        {
            if (volume.HasValue)
                SetMusicVolume(volume.Value);
            return true;
        }

        var audioClip = GetAudioClip(audioName);
        if (audioClip == null)
            return false;

        EnsureMusicAudioSource();

        _music.Stop();
        _music.clip = audioClip;
        if (volume.HasValue)
            _music.volume = volume.Value;
        _music.Play();

        _currentMusicName = audioName;

        return true;
    }

    public void SetMusicVolume(float volume)
    {
        EnsureMusicAudioSource();
        _music.volume = volume;
    }

    public void StopMusic()
    {
        _currentMusicName = null;
        if (_music == null)
            return;
        _music.Stop();
    }
}

static internal class AudioHandler
{
    //Disable the unused variable warning
#pragma warning disable 0414
    static private AudioHandlerHelper _audioHandlerHelper =
        (new GameObject("AudioHandlerHelper")).AddComponent<AudioHandlerHelper>();
#pragma warning restore 0414

    static public AudioHandlerHelper GetHelper()
    {
        return _audioHandlerHelper;
    }

    static public void Cleanup()
    {
        _audioHandlerHelper.Cleanup();
    }

    static public bool Load(string audioName)
    {
        return _audioHandlerHelper.Load(audioName);
    }

    static public bool Play(string audioName)
    {
        return _audioHandlerHelper.Play(audioName);
    }

    static public bool Play(string audioName, float volume)
    {
        return _audioHandlerHelper.Play(audioName, volume);
    }

    static public bool Play(string audioName, Vector3 point)
    {
        return _audioHandlerHelper.Play(audioName, point);
    }

    static public bool Play(string audioName, Vector3 point, float volume)
    {
        return _audioHandlerHelper.Play(audioName, point, volume);
    }

    static public bool PlayMusic(string audioName)
    {
        return _audioHandlerHelper.PlayMusic(audioName);
    }

    static public bool PlayMusic(string audioName, float volume)
    {
        return _audioHandlerHelper.PlayMusic(audioName, volume);
    }

    static public void SetMusicVolume(float volume)
    {
        _audioHandlerHelper.SetMusicVolume(volume);
    }

    static public void StopMusic()
    {
        _audioHandlerHelper.StopMusic();
    }
}