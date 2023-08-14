using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishIdleState : FishState
{
    public FishIdleState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        _fish.StopAllCoroutines();
        _fish.IsIdle = true;
    }

    public override void UpdateState()
    {
        if (_fish.IsInterested)
        {
            _fish.SetState(new FishInterestedState(_fish));
        }
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
        _fish.IsIdle = false;
    }
}
