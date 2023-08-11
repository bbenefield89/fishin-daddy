using System.Collections;
using UnityEngine;

public class FishInterestedState : FishState
{
    public FishInterestedState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        _fish.StartCoroutine(Move());
    }

    public override void UpdateState()
    {
        if (_fish.IsFishNibbling)
        {
            _fish.SetState(new FishNibblingState(_fish));
        }
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
        _fish.IsFishInterested = false;
    }

    private IEnumerator Move()
    {
        Vector3 startPos = _fish.transform.position;
        Vector3 endPos = _fish.transform.position + _fish.transform.forward * _fish.SwimDistance;

        while (true)
        {
            yield return _fish.MoveRoutine(endPos, true);

            bool shouldFishNibble = _fish.FishAlwaysNibble ?
                _fish.FishAlwaysNibble :
                RandomNumberGenerator.TruthyFalsyGenerator();

            if (shouldFishNibble)
            {
                _fish.IsFishNibbling = true;
                yield return null;
            }
            else
            {
                yield return _fish.MoveRoutine(startPos, true);
            }
        }
    }
}
