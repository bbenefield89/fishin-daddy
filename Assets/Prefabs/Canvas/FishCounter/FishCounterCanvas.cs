using UnityEngine;
using TMPro;

public class FishCounterCanvas : MonoBehaviour
{
    public static FishCounterCanvas Instance { get; private set; }

    public TextMeshProUGUI fishCounterText;

    private int fishCaughtCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateFishCounterUI(int amount)
    {
        fishCaughtCount += amount;
        fishCounterText.text = "Fish: " + fishCaughtCount.ToString();
    }
}
