using System.Collections;
using UnityEngine;

public class BobberInWaterState : BobberState
{
    public BobberInWaterState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _bobber.IsInWater = true;
        _bobber.transform.parent = null;
        _bobber.StartCoroutine(AttractFishToBobber());
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButton(1))
        {
            _bobber.IsBeingReeledIn = true;
            _bobber.SetState(new BobberBeingReeledInState(_bobber));
        }
        else if (_bobber.IsBeingBit)
        {
            _bobber.SetState(new BobberIsBeingBitState(_bobber));
        }
    }

    public override void ExitState()
    {
        _bobber.IsInWater = false;
    }

    private IEnumerator AttractFishToBobber()
    {
        while (FishController.Instance.IsIdle)
        {
            float nextBiteCheckIntervalRandom = _bobber.Rng.Generate();
            yield return new WaitForSeconds(nextBiteCheckIntervalRandom);

            FishController.Instance.IsInterested =
                FishController.Instance.AlwaysInterested ?
                FishController.Instance.AlwaysInterested :
                RandomNumberGenerator.TruthyFalsyGenerator();

            if (FishController.Instance.IsInterested)  // Check some conditions again because coroutines
            {

                FishController.Instance.Spawn();
            }
        }
    }
}
