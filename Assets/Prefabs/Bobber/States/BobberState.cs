public abstract class BobberState
{
    public abstract BobberStateType State{ get; }
    protected BobberController _bobber;

    public BobberState(BobberController bobber)
    {
        _bobber = bobber;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
