using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public static FishController Instance { get; private set; }
    private FishState _currentState;

    [Header("State variables")]
    public bool IsFishIdle = true;
    public bool IsFishInterested = false;
    public bool IsFishNibbling = false;
    public bool IsFishBiting = false;
    public bool IsFishHooked = false;

    [Header("Movement/distance variables")]
    public float SwimSpeed = 1.0f;
    public float TimeUntilNextSwim = 1.0f;
    public float SwimDistance = 2.0f;
    public float DesiredDistFromTargetPos = 0.1f;
    public float DistanceFromBobber = 1f;

    [Header("Exposed variables for easier debugging")]
    public bool FishAlwaysInterested = false;
    public bool FishAlwaysNibble = false;
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

    private void Start()
    {
        SetState(new FishIdleState(this));
    }

    private void Update()
    {
        _currentState.UpdateState();
    }

    public void SetState(FishState state)
    {
        if (_currentState != null)
        {
            _currentState.ExitState();
        }

        _currentState = state;
        _currentState.EnterState();
    }

    public IEnumerator MoveRoutine(Vector3 targetPos, bool shouldRotate)
    {
        if (shouldRotate)
        {
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
        }

        while (Vector3.Distance(transform.position, targetPos) > DesiredDistFromTargetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, SwimSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(TimeUntilNextSwim);
    }

    public void Spawn()
    {
        Vector3 desiredPos = BobberController.Instance.transform.position + BobberController.Instance.transform.position.normalized * DistanceFromBobber;

        Quaternion lookDir = Quaternion.LookRotation(BobberController.Instance.transform.position - desiredPos);
        transform.rotation = Quaternion.Euler(0f, lookDir.eulerAngles.y + 90, 0f);

        transform.position = desiredPos + transform.forward * -1;
        IsFishInterested = true;
    }

    public void Reset()
    {
        if (IsFishHooked)
        {
            FishCounterCanvas.Instance.UpdateFishCounterUI();
        }

        IsFishIdle = true;
    }
}
