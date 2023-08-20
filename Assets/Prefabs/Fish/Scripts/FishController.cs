using System;
using System.Collections;
using UnityEngine;

public enum FishStateType
{
    Idle,
    Interested,
    Nibbling,
    Biting,
    Hooked,
    SwimmingAway,
    Fighting,
    Resting,
}

public class FishController : MonoBehaviour
{
    public static FishController Instance { get; private set; }

    private float _originalAlpha;
    private FishState _currentState;
    public FishStateType CurrentStateType => _currentState.State;

    [Header("Movement/distance variables")]
    public float SwimSpeed = 1.0f;
    public float FightSwimSpeed = 3.0f;
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
        //BobberController.Instance.OnFishShouldSwimAway += () => SetState(new FishSwimmingAwayState(this));
        //BobberController.Instance.OnFishShouldBeHooked += () => SetState(new FishHookedState(this));
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

    public IEnumerator MoveRoutine(Vector3 targetPos, bool shouldRotate, float speed = 0f)
    {
        if (shouldRotate)
        {
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
        }

        Func<bool> hasNotArrivedAtTargetPos = () =>
            Vector3.Distance(transform.position, targetPos) > DesiredDistFromTargetPos;

        while (hasNotArrivedAtTargetPos())
        {
            Vector3 prevPos = transform.position;
            float speedToSwim = speed == 0f ? SwimSpeed : speed;
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                speedToSwim * Time.deltaTime);
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

    public IEnumerator SwimAway(float distanceToSwim, float speedToSwim = 0f)
    {
        Vector3 currentPos = transform.position;
        Vector3 bobberPos = BobberController.Instance.transform.position;
        Vector3 dirToSwim = (currentPos - bobberPos).normalized;
        Vector3 targetPos = currentPos + new Vector3(dirToSwim.x, transform.position.y, dirToSwim.z);
        yield return MoveRoutine(targetPos * distanceToSwim, true, speedToSwim);
    }

    public bool CheckIfFishIsHooked()
    {
        return CurrentStateType == FishStateType.Hooked
            || CurrentStateType == FishStateType.Fighting
            || CurrentStateType == FishStateType.Resting
            ? true
            : false;
    }

    public bool CheckIfFishShouldSwimAway()
    {
        return CurrentStateType == FishStateType.Biting
            || CurrentStateType == FishStateType.Nibbling
            || CurrentStateType == FishStateType.Interested
            ? true
            : false;
    }
}
