using UnityEngine;

public class BobberIdleState : BobberState
{
    public override BobberStateType State => BobberStateType.Idle;

    public BobberIdleState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _bobber.StopAllCoroutines();
        _bobber.transform.position = _bobber.BobberReturnPosition.position;
        _bobber.transform.parent = _bobber.BobberReturnPosition;
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _bobber.SetState(new BobberCastingState(_bobber));
        }
    }

    public override void ExitState()
    {
        //
    }
}
