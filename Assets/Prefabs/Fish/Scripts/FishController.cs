using System.Collections;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public static FishController Instance { get; private set; }
    private FishState _currentState;

    [Header("State variables")]
    public bool IsIdle = true;
    public bool IsInterested = false;
    public bool IsNibbling = false;
    public bool IsBiting = false;
    public bool IsHooked = false;
    public bool IsSwimmingAway = false;

    [Header("Movement/distance variables")]
    public float SwimSpeed = 1.0f;
    public float TimeUntilNextSwim = 1.0f;
    public float SwimDistance = 2.0f;
    public float DesiredDistFromTargetPos = 0.1f;
    public float DistanceFromBobber = 1f;
    public float FadeAwaySpeed = 0.5f;

    private float _originalAlpha;

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
        BobberBeingReeledInState.OnFishShouldSwimAway += () => SetState(new FishSwimmingAwayState(this));
        _originalAlpha = GetComponentInChildren<Renderer>().material.color.a;
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

    public void Spawn()
    {
        Vector3 desiredPos = BobberController.Instance.transform.position + BobberController.Instance.transform.position.normalized * DistanceFromBobber;

        Quaternion lookDir = Quaternion.LookRotation(BobberController.Instance.transform.position - desiredPos);
        transform.rotation = Quaternion.Euler(0f, lookDir.eulerAngles.y + 90, 0f);

        transform.position = desiredPos + transform.forward * -1;

        IsIdle = false;
        IsInterested = true;
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
        if (IsHooked)
        {
            FishCounterCanvas.Instance.UpdateFishCounterUI();
        }

        transform.position = new Vector3(0f, -1f, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        ResetAlpha();
        SetState(new FishIdleState(this));
    }

    private void ResetAlpha()
    {
        Renderer fishRenderer = GetComponentInChildren<Renderer>();
        Color fishColor = fishRenderer.material.color;

        fishColor.a = _originalAlpha;
        fishRenderer.material.color = fishColor;
    }
}
