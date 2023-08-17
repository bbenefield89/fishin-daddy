public class FishIdleState : FishState
{
    public override FishStateType State { get; } = FishStateType.Idle;

    public FishIdleState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        _fish.StopAllCoroutines();
    }

    public override void UpdateState()
    {
        if (_fish.CurrentStateType == FishStateType.Interested)
        {
            _fish.SetState(new FishInterestedState(_fish));
        }
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
    }
}
