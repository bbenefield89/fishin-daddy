public abstract class FishState
{
    public abstract FishStateType State { get; }
    protected FishController _fish;

    public FishState(FishController fish)
    {
        _fish = fish;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}