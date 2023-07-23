using UnityEngine;
using TMPro;

public class FishCounterCanvas : MonoBehaviour
{
    [Tooltip("Reference to the text field that displays the amount of fish caught")]
    public TextMeshProUGUI fishCounterText;

    private int fishCaughtCount = 0;

    public void UpdateFishCounterUI()
    {
        fishCaughtCount += 1;
        fishCounterText.text = "Fish: " + fishCaughtCount.ToString();
    }
}
