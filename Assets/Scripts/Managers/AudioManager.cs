using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public bool SFxEnabled { get { return GameManager.Instance.playerSettings.SFxEnabled; } }
    public float SFxVolume { get { return GameManager.Instance.playerSettings.SFxVolume; } }
    public bool MusicEnabled { get { return GameManager.Instance.playerSettings.MusicEnabled; } }
    public float MusicVolume { get { return GameManager.Instance.playerSettings.MusicVolume; } }

    [SerializeField]
    private AudioClip MusicClip;
    [SerializeField]
    private AudioSource MusicSource;
    [SerializeField]
    private AudioSource SFxSource;

    protected void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    protected void Update()
    {
        PlayMusic();
    }

    public void PlaySound(AudioClip clip)
    {
        if (!SFxEnabled)
            return;

        if (clip != null)
            SFxSource.PlayOneShot(clip, SFxVolume);
    }

    public void PlayMusic()
    {
        MusicSource.gameObject.SetActive(MusicEnabled);
        if (!MusicEnabled)
            return;

        MusicSource.volume = MusicVolume;
        
        if(MusicSource.clip == MusicClip)
            return;
        MusicSource.clip = MusicClip;
        MusicSource.loop = true;
        MusicSource.Play();
        
    }
}
