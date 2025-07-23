using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // To make SoundManager globally accessible.

    public AudioSource audioSource;

    [Header("Sound Clips")]
    public AudioClip cardFlipSound;
    public AudioClip matchClip;
    public AudioClip mismatchClip;
    public AudioClip gameWinClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    // Plays the Card flip sound
    public void PlayFlip()
    {
        if (cardFlipSound != null)
        {
            {
                audioSource.PlayOneShot(cardFlipSound);
            }
        }
    }
    // Plays the match sound
    public void PlayMatch()
    {
        if(matchClip != null)
        {
            audioSource.PlayOneShot(matchClip);
        }
    }
    // Plays the mismatch sound
    public void PlayMisMatch()
    {
        if(mismatchClip != null)
        {
            audioSource.PlayOneShot(mismatchClip);
        }
    }
    // Plays the game over sound
    public void PlayGameOver()
    {
        if (gameWinClip != null)
        {
            audioSource.PlayOneShot(gameWinClip);
        }
    }
}
