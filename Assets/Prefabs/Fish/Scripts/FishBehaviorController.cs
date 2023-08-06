using System.Collections;
using UnityEngine;

public class FishBehaviorController : MonoBehaviour
{
    public static FishBehaviorController Instance { get; private set; }

    [SerializeField] private GameObject _bobberModel;
    [SerializeField] private float _swimSpeed = 1.0f;
    [SerializeField] private float _swimDistance = 2.0f;
    [SerializeField] private float _timeUntilNextSwim = 1.0f;
    [SerializeField] private float _desiredDistFromTargetPos = 0.1f;
    [SerializeField] private float _distanceFromBobber = 1f;
    [SerializeField] private bool _isFishHooked = false;

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

        while (!_isFishHooked)
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

        while (Vector3.Distance(transform.position, targetPos) > _desiredDistFromTargetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _swimSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(_timeUntilNextSwim);
    }

    private IEnumerator InteractWithBobber()
    {
        while (!_isFishHooked)
        {
            yield return MoveRoutine(_bobberModel.transform.position, true);

            bool shouldFishBite = RandomNumberGenerator.TruthyFalsyGenerator();
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
}
