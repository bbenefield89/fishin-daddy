public class FishBitingState : FishState
{
    public FishBitingState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        BiteHook();
    }

    public override void UpdateState()
    {
        if (_fish.IsFishHooked)
        {
            _fish.SetState(new FishHookedState(_fish));
        }
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
        _fish.IsFishBiting = false;
    }

    private void BiteHook()
    {
        BobberController.Instance.IsBeingBit = true;
        FishHookedAudioController.Instance.PlayFishHookedAudio();
    }
}
