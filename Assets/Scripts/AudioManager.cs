using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip tapSound;
    public AudioClip winSound;

    void Awake()
    {
        // Simple Singleton pattern for easy access
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        PlayMusic();
    }

    // Use this for looping background tracks
    public void PlayMusic()
    {
        if (backgroundMusic == null) return;
        if (!musicSource.isPlaying) {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Optimized for rapid-fire sounds (like tapping)
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        // PlayOneShot is more performant for overlapping sounds
        sfxSource.PlayOneShot(clip);
    }
}