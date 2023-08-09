public abstract class FishState
{
    protected FishController _fish;

    public FishState(FishController fish)
    {
        _fish = fish;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}