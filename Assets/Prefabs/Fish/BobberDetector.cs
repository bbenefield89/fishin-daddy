using System.Linq;
using TMPro;
using UnityEngine;

public class BobberDetector : MonoBehaviour
{
    public GameObject fishObject;
    public TextMeshProUGUI fishHookedTextObj;

    private GameObject fishSpawner;
    private GameObject fishCounterCanvas;
    private AudioSource fishHookedAudio;

    private void Start()
    {
        fishSpawner = GameObject.Find(Prefabs.FISH_SPAWNER);
        fishCounterCanvas = GameObject.Find(Prefabs.FISH_COUNTER_CANVAS);
        fishHookedAudio = GameObject.Find(Prefabs.FISH_HOOKED_AUDIO).GetComponent<AudioSource>();
    }

    private void Update()
    {
        bool isFishHooked = fishHookedTextObj.text != "";

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
            fishHookedTextObj.text = "!!!";
            fishHookedAudio.Play();
        }
    }
}
