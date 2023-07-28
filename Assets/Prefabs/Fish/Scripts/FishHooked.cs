using UnityEngine;

public class FishHooked : MonoBehaviour
{
    public GameObject fishObject;
    public GameObject exclamationMarks;

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
            bool isFishInterested = Random.Range(0, 2) == 0 ? false : true;

            if (isFishInterested)
            {
                FishMover fishMover = GetComponent<FishMover>();
                StartCoroutine(fishMover.MoveTowardsBobber());
            }
        }
    }

    public void HookFish()
    {
        isFishHooked = true;
        fishHookedAudio.Play();
        GameObject exclamationMarksInstatiated = Instantiate(exclamationMarks);
        exclamationMarksInstatiated.transform.SetParent(fishObject.transform, false);
    }
}
