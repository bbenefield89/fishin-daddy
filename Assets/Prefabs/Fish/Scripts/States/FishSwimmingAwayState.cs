using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSwimmingAwayState : FishState
{
    public FishSwimmingAwayState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        _fish.IsSwimmingAway = true;
        _fish.StartCoroutine(SwimAway());
    }

    public override void UpdateState()
    {
        //
    }

    public override void ExitState()
    {
        _fish.IsSwimmingAway = false;
    }

    // Move fish in opposite direction of bobber
    private IEnumerator SwimAway()
    {
        Vector3 currentPos = _fish.transform.position;
        Vector3 bobberPos = BobberController.Instance.transform.position;
        Vector3 dirToSwim = (currentPos - bobberPos).normalized;
        Vector3 swimToPos = dirToSwim * 20f;
        Func<bool> notAtSwimToPos = () => Vector3.Distance(_fish.transform.position, swimToPos) > 0.1f;

        while (notAtSwimToPos())
        {
            _fish.transform.position = Vector3.MoveTowards(
                _fish.transform.position,
                swimToPos,
                _fish.SwimSpeed * Time.deltaTime);

            yield return null;
        }

        // Fish should begin to fade away
        // Fish should then return to Idle State
        // Fish should be visible again
    }
}
