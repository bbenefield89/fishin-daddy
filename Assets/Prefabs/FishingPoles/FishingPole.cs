using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPole : MonoBehaviour
{
    public static FishingPole Instance;
    
    public float StopAudioAfterSeconds = 1f;

    private AudioSource _audioSource;
    private float _audioStartedPlayingAt;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayReelingSfx()
    {
        if (!_audioSource.isPlaying)
        {
            _audioStartedPlayingAt = Time.time;
            _audioSource.time = 24f;
            _audioSource.Play();
        }
        else if (Time.time > _audioStartedPlayingAt + StopAudioAfterSeconds)
        {
            StopReelingSfx();
        }
    }

    public void StopReelingSfx()
    {

        _audioSource.Stop();
    }
}
