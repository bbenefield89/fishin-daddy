public class FishBitingState : FishState
{
    public FishBitingState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        BiteHook();
    }

    public override void UpdateState()
    {
        if (_fish.IsHooked)
        {
            _fish.SetState(new FishHookedState(_fish));
        }
        else if (_fish.IsSwimmingAway)
        {
            _fish.SetState(new FishSwimmingAwayState(_fish));
        }
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
        _fish.IsBiting = false;
    }

    private void BiteHook()
    {
        BobberController.Instance.IsBeingBit = true;
        FishHookedAudioController.Instance.PlayFishHookedAudio();
    }
}
