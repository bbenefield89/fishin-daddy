using System.Collections;
using UnityEngine;

public enum FishStateType
{
    Idle,
    Interested,
    Nibbling,
    Biting,
    Hooked,
    SwimmingAway
}

public class FishController : MonoBehaviour
{
    public static FishController Instance { get; private set; }

    private float _originalAlpha;
    private FishState _currentState;
    public FishStateType CurrentStateType => _currentState.State;

    [Header("Movement/distance variables")]
    public float SwimSpeed = 1.0f;
    public float TimeUntilNextSwim = 1.0f;
    public float SwimDistance = 2.0f;
    public float DesiredDistFromTargetPos = 0.1f;
    public float DistanceFromBobber = 1f;
    public float FadeAwaySpeed = 0.5f;

    [Header("Exposed variables for easier debugging")]
    public bool AlwaysInterested = false;
    public bool AlwaysNibble = false;
    public bool AlwaysBite = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupBobberToFishEvents();
        _originalAlpha = GetComponentInChildren<Renderer>().material.color.a;
        SetState(new FishIdleState(this));
    }

    private void SetupBobberToFishEvents()
    {
        BobberController.Instance.OnFishShouldSwimAway += () => SetState(new FishSwimmingAwayState(this));
        BobberController.Instance.OnFishShouldBeHooked += () => SetState(new FishHookedState(this));
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

    private void Update()
    {
        _currentState.UpdateState();
    }

    public IEnumerator MoveRoutine(Vector3 targetPos, bool shouldRotate)
    {
        if (shouldRotate)
        {
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
        }

        while (Vector3.Distance(transform.position, targetPos) > DesiredDistFromTargetPos)
        {
            Vector3 prevPos = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, SwimSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(TimeUntilNextSwim);
    }

    public void Reset()
    {
        if (CurrentStateType == FishStateType.Hooked)
        {
            FishCounterCanvas.Instance.UpdateFishCounterUI();
        }

        ResetPosition();
        ResetAlpha();
        SetState(new FishIdleState(this));
    }

    private void ResetPosition()
    {
        transform.position = new Vector3(0f, -1f, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void ResetAlpha()
    {
        Renderer fishRenderer = GetComponentInChildren<Renderer>();
        Color fishColor = fishRenderer.material.color;

        fishColor.a = _originalAlpha;
        fishRenderer.material.color = fishColor;
    }

    public void CheckFishInterestInBobber()
    {
        bool isFishInterested = AlwaysInterested ?
            AlwaysInterested :
            RandomNumberGenerator.TruthyFalsyGenerator();

        if (isFishInterested)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Vector3 desiredPos = BobberController.Instance.transform.position + BobberController.Instance.transform.position.normalized * DistanceFromBobber;
        Quaternion lookDir = Quaternion.LookRotation(BobberController.Instance.transform.position - desiredPos);

        transform.rotation = Quaternion.Euler(0f, lookDir.eulerAngles.y + 90, 0f);
        transform.position = desiredPos + transform.forward * -1;

        //IsIdle = false;
        //IsInterested = true;
        SetState(new FishInterestedState(this));
    }
}
