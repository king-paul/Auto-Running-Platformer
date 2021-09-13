using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    // audio objects
    [Header("Audio Clips")]
    public AudioClip jumpSound;
    public AudioClip highJumpSound;
    public AudioClip landSound;
    public AudioClip hazardSound;
    public AudioClip collideSound;
    public AudioClip fallSound;
    public AudioClip collectSound;

    private AudioSource playerAudio;
    private AudioSource footsteps;

    // Start is called before the first frame update
    void Start()
    {
        // get audio sources
        var audioSources = GetComponents<AudioSource>();
        footsteps = audioSources[0];
        playerAudio = audioSources[1];
    }

    public void PlaySound(AudioClip clip)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(clip);
    }

    public void PlayRunningSound(float time)
    {
        if(!footsteps.isPlaying)
            footsteps.PlayScheduled(time);
    }

    public void PlayJumpSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(jumpSound, volumeScale);
    }

    public void PlayHighJumpSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(highJumpSound, volumeScale);
    }

    public void PlayLandSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(landSound, volumeScale);
    }

    public void PlayCollideSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(collideSound, volumeScale);
    }

    public void PlayHazardSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(hazardSound, volumeScale);
    }

    public void PlayFallSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(fallSound, volumeScale);
    }

    public void PlayCoinCollectSound(float volumeScale = 1.0f)
    {
        footsteps.Stop();
        playerAudio.PlayOneShot(collectSound, volumeScale);
    }

}