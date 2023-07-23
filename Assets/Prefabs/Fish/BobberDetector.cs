using TMPro;
using UnityEngine;

public class BobberDetector : MonoBehaviour
{
    public GameObject fishWrapper;
    public TextMeshProUGUI fishHookedTextObj;

    private GameObject fishCounterCanvas;
    private AudioSource fishHookedAudio;

    private void Start()
    {
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
        Destroy(fishWrapper);
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
