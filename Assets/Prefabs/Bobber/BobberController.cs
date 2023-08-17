using System;
using System.Collections;
using UnityEngine;

public class BobberController : MonoBehaviour
{
    #region Props
    public static BobberController Instance { get; private set; }
    private BobberState _currentState;

    [Header("State vars")]
    public bool IsIdle = true;
    public bool IsCasting = false;
    public bool IsInWater = false;
    public bool IsBeingReeledIn = false;
    public bool IsBeingBit = false;

    [Tooltip("GameObject that holds a reference to the position the bobber should be when not casted")]
    public Transform BobberReturnPosition;
    public GameObject Exclamations;
    public AudioSource FishHookedAudio;
    public Transform PcModel;

    [Tooltip("How far down on the Y axis the bobber should drop after casting")]
    public float WaterLevel = 0f;
    public float CastDistance = 5f;
    public float BobberMovementSpeed = 15f;
    public float ArcHeight = 1f;
    public float ReelSpeed = 5f;
    public float TimeAllowedToHookFish = 1f;
    public RandomNumberGenerator Rng;

    public event Action OnFishShouldSwimAway;
    public event Action OnFishShouldBeHooked;
    #endregion

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
        SetState(new BobberIdleState(this));
    }

    private void Update()
    {
        _currentState.UpdateState();
    }

    public void SetState(BobberState state)
    {
        if (_currentState != null)
        {
            _currentState.ExitState();
        }

        _currentState = state;
        _currentState.EnterState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.GROUND))
        {
            ReturnBobberToFishingPole();
            FishController.Instance.Reset();
        }
    }

    private void ReturnBobberToFishingPole()
    {
        SetState(new BobberIdleState(this));
    }

    public void InvokeFishShouldSwimAway()
    {
        OnFishShouldSwimAway?.Invoke();
    }

    public void InvokeFishShouldBeHooked()
    {
        OnFishShouldBeHooked?.Invoke();
    }
}
