using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishNibblingState : FishState
{
    public FishNibblingState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        _fish.StartCoroutine(InteractWithBobber());
    }

    public override void UpdateState()
    {
        if (_fish.IsFishBiting)
        {
            _fish.SetState(new FishBitingState(_fish));
        }
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
        _fish.IsFishNibbling = false;
    }

    private IEnumerator InteractWithBobber()
    {
        while (true)
        {
            yield return _fish.MoveRoutine(
                BobberController.Instance.transform.position,
                true);

            bool shouldFishBite = _fish.FishAlwaysBite ?
                _fish.FishAlwaysBite :
                RandomNumberGenerator.TruthyFalsyGenerator();

            if (shouldFishBite)
            {
                _fish.IsFishBiting = true;
                yield return null;
            }
            else
            {
                Vector3 swimBackwardPos = _fish.transform.position + _fish.transform.forward * _fish.DistanceFromBobber * -1;
                yield return _fish.MoveRoutine(swimBackwardPos, false);
            }
        }
    }
}
