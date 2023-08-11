using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberInWaterState : BobberState
{
    public BobberInWaterState(BobberController bobber) : base(bobber)
    {
    }

    public override void EnterState()
    {
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
    }

    public override void ExitState()
    {
        _bobber.IsInWater = false;
    }

    private IEnumerator AttractFishToBobber()
    {
        while (!FishController.Instance.IsFishInterested)
        {
            float nextBiteCheckIntervalRandom = _bobber.Rng.Generate();
            yield return new WaitForSeconds(nextBiteCheckIntervalRandom);

            FishController.Instance.IsFishInterested =
                FishController.Instance.FishAlwaysInterested ?
                FishController.Instance.FishAlwaysInterested :
                RandomNumberGenerator.TruthyFalsyGenerator();

            if (FishController.Instance.IsFishInterested)  // Check some conditions again because coroutines
            {
                FishController.Instance.Spawn();
            }
        }
    }
}
