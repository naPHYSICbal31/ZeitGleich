using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerL2 : MonoBehaviour
{
    [Header("---Audio Sources---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("---Audio Clip---")]
    public AudioClip background;
    public AudioClip acquire;
    public AudioClip jump;
    public AudioClip transition;
    public AudioClip place;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
