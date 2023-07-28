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
        FishManager.Instance.fish.Remove(transform.parent.gameObject);
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.BOBBER))
        {
            bool isFishInterested = Random.Range(0, 2) == 0 ? false : true;
            if (!FishManager.Instance.IsFishHooked && isFishInterested)
            {
                FishMover fishMover = GetComponent<FishMover>();
                StartCoroutine(fishMover.MoveTowardsBobber());
            }
        }
    }

    public void HookFish()
    {
        if (!FishManager.Instance.IsFishHooked)
        {
            isFishHooked = true;
            FishManager.Instance.IsFishHooked = true;
            fishHookedAudio.Play();
            RenderExlcamations();
        }
    }


    private void RenderExlcamations()
    {
        GameObject exclamationMarksInstatiated = Instantiate(exclamationMarks);
        exclamationMarksInstatiated.transform.SetParent(transform.parent.transform, false);
    }
}
