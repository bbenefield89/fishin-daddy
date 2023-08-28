using UnityEngine;

public class GroundController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.BOBBER))
        {
            BobberController.Instance.Reset();
        }
        else if (other.CompareTag(Tags.FISH))
        {
            FishController.Instance.Reset();
            BobberController.Instance.Reset();
            FishCounterCanvas.Instance.UpdateFishCounterUI(1);
        }
    }
}
