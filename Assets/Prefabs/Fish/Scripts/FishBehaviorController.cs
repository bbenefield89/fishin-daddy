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

            bool shouldFishNibble = true;// RandomNumberGenerator.TruthyFalsyGenerator();
            if (shouldFishNibble)
            {
                yield return MoveRoutine(bobberModel.transform.position, true);
                yield return Nibble();
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

    // after 1f, go for another nibble
    // repeat this until fish bites
    // private method nibble
    private IEnumerator Nibble()
    {
        // when close enough, check if fish bites
        bool shouldFishBite = true;// RandomNumberGenerator.TruthyFalsyGenerator();
        if (shouldFishBite)
        {
            // if fish bites, call BobberController.Instance.HookFish()
            BobberController.Instance.HookFish();
            FishHookedAudioController.Instance.PlayFishHookedAudio();
        }
        else
        {
            // if fish does not bite, move fish backwards from bobber but still facing bobber
            Vector3 targetPos = transform.position + transform.forward * distanceFromBobber * -1;

            while (Vector3.Distance(transform.position, targetPos) > desiredDistFromTargetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, swimSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
