using System.Collections;
using UnityEngine;

public class FishInterestedState : FishState
{
    public override FishStateType State { get; } = FishStateType.Interested;

    public FishInterestedState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        _fish.StopAllCoroutines();
        _fish.StartCoroutine(Move());
    }

    public override void UpdateState()
    {
        //
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
    }

    private IEnumerator Move()
    {
        Vector3 startPos = _fish.transform.position;
        Vector3 endPos = _fish.transform.position + _fish.transform.forward * _fish.SwimDistance;

        while (true)
        {
            yield return _fish.MoveRoutine(endPos, true);

            bool shouldFishNibble = _fish.AlwaysNibble ?
                _fish.AlwaysNibble :
                RandomNumberGenerator.TruthyFalsyGenerator();

            if (shouldFishNibble)
            {
                _fish.SetState(new FishNibblingState(_fish));
                yield return null;
            }
            else
            {
                yield return _fish.MoveRoutine(startPos, true);
            }
        }
    }
}
