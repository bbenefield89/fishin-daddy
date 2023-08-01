using System.Collections;
using UnityEngine;

public class FishBehaviorController : MonoBehaviour
{
    public static FishBehaviorController Instance { get; private set; }

    [SerializeField] private GameObject bobberModel;
    [SerializeField] private float swimSpeed = 1.0f;
    [SerializeField] private float swimDistance = 2.0f;
    [SerializeField] private float timeUntilNextSwim = 1.0f;
    [SerializeField] private float desiredDistFromTargetPos = 0.1f;
    [SerializeField] private float distanceFromBobber = 1f;

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

    [ContextMenu("Functions/BeginMovement")]
    public void BeginMovement()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + transform.forward * swimDistance;

        while (!BobberController.Instance.isFishHooked)
        {
            yield return MoveRoutine(endPos, true);

            bool shouldFishNibble = RandomNumberGenerator.TruthyFalsyGenerator();
            if (shouldFishNibble)
            {
                yield return InteractWithBobber();
            }
            else
            {
                yield return MoveRoutine(startPos, true);
            }
        }
    }

    private IEnumerator MoveRoutine(Vector3 targetPos, bool shouldRotate)
    {
        if (shouldRotate)
        {
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
        }

        while (Vector3.Distance(transform.position, targetPos) > desiredDistFromTargetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, swimSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(timeUntilNextSwim);
    }

    private IEnumerator InteractWithBobber()
    {
        while (!BobberController.Instance.isFishHooked)
        {
            yield return MoveRoutine(bobberModel.transform.position, true);

            bool shouldFishBite = RandomNumberGenerator.TruthyFalsyGenerator();
            if (shouldFishBite)
            {
                BobberController.Instance.HookFish();
                FishHookedAudioController.Instance.PlayFishHookedAudio();
                yield return null;
            }
            else
            {
                Vector3 swimBackwardPos = transform.position + transform.forward * distanceFromBobber * -1;
                yield return MoveRoutine(swimBackwardPos, false);
            }
        }
    }
}
