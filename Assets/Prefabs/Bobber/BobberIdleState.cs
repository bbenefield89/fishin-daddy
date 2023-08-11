using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberIdleState : BobberState
{
    public BobberIdleState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _bobber.StopAllCoroutines();
        _bobber.IsIdle = true;
        _bobber.IsCasting = false;
        _bobber.IsInWater = false;
        _bobber.IsBeingReeledIn = false;
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
        _bobber.IsIdle = false;
        _bobber.IsCasting = true;
    }
}
