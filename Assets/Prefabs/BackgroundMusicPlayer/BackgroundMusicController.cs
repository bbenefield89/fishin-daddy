using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    [Tooltip("Array of background music audio files to play")]
    public AudioClip[] backgroundMusicFiles;

    [Tooltip("The Audio Source component attached to this GameObject")]
    public AudioSource audioSource;

    [Tooltip("For dev purposes so I don't hear the same music over and over")]
    public bool shouldPlay = true;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying && shouldPlay)
        {
            PlayRandomSong();
        }
    }

    private void PlayRandomSong()
    {
        int idx = Random.Range(0, backgroundMusicFiles.Length);
        audioSource.clip = backgroundMusicFiles[idx];
        audioSource.Play();
    }
}
