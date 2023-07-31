using UnityEngine;

public class FishHookedAudioController : MonoBehaviour
{
    public static FishHookedAudioController Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayFishHookedAudio()
    {
        audioSource.Play();
    }
}
