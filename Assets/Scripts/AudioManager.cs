using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    public AudioClip bgmusic;
    private void Start()
    {
        musicSource.clip = bgmusic;
        musicSource.Play();
    }
}
