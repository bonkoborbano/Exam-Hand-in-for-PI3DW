using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager instance;
    
    // Background music source
    [SerializeField] public AudioSource musicSource;
    
    // Sound effect sources
    [SerializeField] private AudioSource sfxSource1;
    [SerializeField] private AudioSource sfxSource2;
    [SerializeField] private AudioSource sfxSource3;
    [SerializeField] private AudioSource sfxSource4;

    // Background music clip
    public AudioClip musicClip;

    // Array of sound effect clips
    public AudioClip[] sfxClips = new AudioClip[4];

    private void Awake()
    {
        // Standard singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    public void PlayMusic()
    {
        // Place music in the source, enable loop and play
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }
    
    public void PlaySFX1(int index)
    {
        if (sfxClips[index] != null)
        {
            sfxSource1.PlayOneShot(sfxClips[index]);
        }
    }
    public void PlaySFX2(int index)
    {
        if (sfxClips[index] != null)
        {
            sfxSource2.PlayOneShot(sfxClips[index]);
        }
    }
    public void PlaySFX3(int index)
    {
        if (sfxClips[index] != null)
        {
            sfxSource3.PlayOneShot(sfxClips[index]);
        }
    }
    
    // Special case for looping sound effect
    public void PlaySFX4(int index)
    {
        if (sfxClips[index] != null)
        {
            sfxSource4.clip = sfxClips[index];
            sfxSource4.loop = true;
            sfxSource4.Play();
        }
    }
    
    public void PauseSFX4()
    {
        if (sfxSource4 != null && sfxSource4.isPlaying)
        {
            sfxSource4.Pause();
        }
        
    }
    public void StopSFX4()
    {
        if (sfxSource4 != null && sfxSource4.isPlaying)
        {
            sfxSource4.Stop();
        }
    }
    
    
}