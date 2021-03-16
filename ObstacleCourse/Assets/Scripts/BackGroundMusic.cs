using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    public AudioClip gameplayMusic = null;

    AudioSource source = null;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void ChangeSong()
    {
        source.clip = gameplayMusic;
        source.Play();
    }
}
