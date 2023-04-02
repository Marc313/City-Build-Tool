using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip buildSound;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayBuildSound()
    {
        source.PlayOneShot(buildSound);
    }
}