using UnityEngine;

public class BobberDetector : MonoBehaviour
{
    public GameObject fishObject;

    private GameObject fishSpawner;
    private GameObject fishCounterCanvas;
    private AudioSource fishHookedAudio;
    private bool isFishHooked = false;

    private void Start()
    {
        fishSpawner = GameObject.Find(Prefabs.FISH_SPAWNER);
        fishCounterCanvas = GameObject.Find(Prefabs.FISH_COUNTER_CANVAS);
        fishHookedAudio = GameObject.Find(Prefabs.FISH_HOOKED_AUDIO).GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isFishHooked && Input.GetMouseButtonDown(1))
        {
            CatchFish();
        }
    }

    private void CatchFish()
    {
        fishCounterCanvas.GetComponent<FishCounterCanvas>().UpdateFishCounterUI();
        fishSpawner.GetComponent<FishSpawner>().fish.Remove(fishObject);
        Destroy(fishObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.BOBBER))
        {
            isFishHooked = true;
            fishHookedAudio.Play();
        }
    }
}
