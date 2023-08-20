public class FishBitingState : FishState
{
    public override FishStateType State { get; } = FishStateType.Biting;

    public FishBitingState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        BiteHook();
    }

    public override void UpdateState()
    {
        if (_fish.CurrentStateType == FishStateType.Hooked)
        {
            _fish.SetState(new FishHookedState(_fish));
        }
        else if (_fish.CurrentStateType == FishStateType.SwimmingAway)
        {
            _fish.SetState(new FishSwimmingAwayState(_fish));
        }
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
    }

    private void BiteHook()
    {
        //BobberController.Instance.IsBeingBit = true;
        BobberController.Instance.SetState(
            new BobberIsBeingBitState(BobberController.Instance));
        FishHookedAudioController.Instance.PlayFishHookedAudio();
    }
}
