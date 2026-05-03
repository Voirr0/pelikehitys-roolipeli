using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("SFX")]
    [SerializeField] private AudioClip[] hitWall;
    [SerializeField] private AudioClip[] openDoor;
    [SerializeField] private AudioClip[] nearMerchant;
    [SerializeField] private AudioClip[] buy;

    public enum SoundEffect
    {
        HitWall,
        OpenDoor,
        NearMerchant,
        Buy
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(SoundEffect sound)
    {
        AudioClip clip = GetClip(sound);
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    private AudioClip GetClip(SoundEffect sound)
    {
        AudioClip[] list = null;

        switch (sound)
        {
            case SoundEffect.HitWall:
                list = hitWall;
                break;
            case SoundEffect.OpenDoor:
                list = openDoor;
                break;
            case SoundEffect.NearMerchant:
                list = nearMerchant;
                break;
            case SoundEffect.Buy:
                list = buy;
                break;
        }

        if (list != null && list.Length > 0)
            return list[Random.Range(0, list.Length)];

        return null;
    }

    // --- MUSIC (optional) ---
    public void PlayMusic(AudioClip music)
    {
        if (musicSource.clip == music) return;

        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}