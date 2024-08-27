using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] float _volume = 0.5f;
    [SerializeField] Sound[] sounds;

    public static AudioManager Instance { get; private set; }

    public float Volume
    {
        get { return _volume; }
        set
        {
            _volume = Mathf.Clamp01(value);
            UpdateSoundVolumes();
        }
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeSounds();
        UpdateSoundVolumes();
    }

    void OnValidate() => Instance = this;

    private void InitializeSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    private void UpdateSoundVolumes()
    {
        foreach (Sound s in sounds)
        {
            if (s.source != null)
            {
                s.source.volume = s.volume * _volume;
            }
        }
    }

    public void Play(Sounds name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null && s.source != null)
        {
            s.source.Play();
            //Debug.Log("Sound Played!");
        }
        else
        {
            Debug.LogWarning("Sound was found: " + (s != null));
            Debug.LogWarning("AudioSource is assigned: " + (s.source != null));
        }
    }

    public void Loop(Sounds name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null && s.source != null && !s.source.isPlaying)
        {
            s.source.loop = true;
            s.source.Play();
            //Debug.Log("Sound Looped!");
        }
        else
        {
            Debug.LogWarning("Sound was found: " + (s != null));
            Debug.LogWarning("AudioSource is assigned: " + (s.source != null));
            Debug.LogWarning("Sound already playing: " + s.source.isPlaying);
        }
    }

    public void Stop(Sounds name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null && s.source != null && s.source.isPlaying)
        {
            s.source.loop = false;
            s.source.Stop();
            Debug.Log("Sound Stopped!");
        }
    }
}