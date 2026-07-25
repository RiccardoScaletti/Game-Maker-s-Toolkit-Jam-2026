using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;   // loops
    [SerializeField] private AudioSource sfxSource;     // one-shots
    [SerializeField] private AudioSource tickingSource; // dedicated loop

    [Header("Clips")]
    public AudioClip bgMusic;
    public AudioClip goblinNoise;
    public AudioClip angryBossNoise;
    public AudioClip cashRegister;
    public AudioClip oldLady;
    public AudioClip smoking;
    public AudioClip timeTicking;
    public AudioClip genericRummage;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        PlayMusic(bgMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void StartTicking()
    {
        tickingSource.clip = timeTicking;
        tickingSource.loop = true;
        tickingSource.Play();
    }

    public void StopTicking() => tickingSource.Stop();
}