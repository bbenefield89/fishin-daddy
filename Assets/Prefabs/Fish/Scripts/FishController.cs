using System.Collections;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public static FishController Instance { get; private set; }

    public bool IsFishHooked = false;
    public bool IsFishInterested = false;

    [SerializeField] private GameObject _bobberModel;
    [SerializeField] private float _swimSpeed = 1.0f;
    [SerializeField] private float _swimDistance = 2.0f;
    [SerializeField] private float _timeUntilNextSwim = 1.0f;
    [SerializeField] private float _desiredDistFromTargetPos = 0.1f;
    [SerializeField] private float _distanceFromBobber = 1f;

    [Header("Exposed variables for easier debugging")]
    public bool FishAlwaysInterested = false;
    public bool FishAlwaysBite = false;

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
        Vector3 endPos = transform.position + transform.forward * _swimDistance;

        while (!IsFishHooked)
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

        while (IsFishHooked)
        {
            yield return FollowBobber();
        }
    }

    private IEnumerator FollowBobber()
    {
        float reelSpeed = BobberController.Instance.ReelSpeed;
        Vector3 bobberPos = BobberController.Instance.transform.position;
        Vector3 targetPos = new Vector3(
            bobberPos.x,
            transform.position.y,
            bobberPos.z);

        Vector3 pcPos = PCModel.Instance.transform.position;
        Vector3 lookToPos = new Vector3(
            pcPos.x,
            transform.position.y,
            pcPos.z);
            
        transform.rotation = Quaternion.LookRotation(lookToPos - transform.position);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, reelSpeed * Time.deltaTime);
        yield return null;
    }

    private IEnumerator MoveRoutine(Vector3 targetPos, bool shouldRotate)
    {
        if (shouldRotate)
        {
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
        }

        while (Vector3.Distance(transform.position, targetPos) > _desiredDistFromTargetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _swimSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(_timeUntilNextSwim);
    }

    private IEnumerator InteractWithBobber()
    {
        while (!IsFishHooked)
        {
            yield return MoveRoutine(_bobberModel.transform.position, true);

            bool shouldFishBite = FishAlwaysBite ?
                FishAlwaysBite :
                RandomNumberGenerator.TruthyFalsyGenerator();

            if (shouldFishBite)
            {
                BobberController.Instance.HookFish();
                FishHookedAudioController.Instance.PlayFishHookedAudio();
                yield return null;
            }
            else
            {
                Vector3 swimBackwardPos = transform.position + transform.forward * _distanceFromBobber * -1;
                yield return MoveRoutine(swimBackwardPos, false);
            }
        }
    }

    public void Reset()
    {
        StopAllCoroutines();
        IsFishHooked = false;
        IsFishInterested = false;
        transform.position = new Vector3(0f, -1f, 0f);
        transform.rotation = Quaternion.identity;
    }
}
