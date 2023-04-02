using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip buildSound;
    [SerializeField] private AudioClip removeSound;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayBuildSound()
    {
        source.PlayOneShot(buildSound);
    }

    public void PlayRemoveSound()
    {
        source.PlayOneShot(removeSound);
    }
}