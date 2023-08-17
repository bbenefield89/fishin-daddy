using System.Collections;
using UnityEngine;

public class FishNibblingState : FishState
{
    public override FishStateType State { get; } = FishStateType.Nibbling;

    public FishNibblingState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        _fish.StartCoroutine(InteractWithBobber());
    }

    public override void UpdateState()
    {
        //
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
    }

    private IEnumerator InteractWithBobber()
    {
        while (true)
        {
            yield return _fish.MoveRoutine(
                BobberController.Instance.transform.position,
                true);

            bool shouldFishBite = _fish.AlwaysBite ?
                _fish.AlwaysBite :
                RandomNumberGenerator.TruthyFalsyGenerator();

            if (shouldFishBite)
            {
                _fish.SetState(new FishBitingState(_fish));
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
