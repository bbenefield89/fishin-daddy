using System.Collections;
using UnityEngine;

public class BobberInWaterState : BobberState
{
    public override BobberStateType State => BobberStateType.InWater;

    public BobberInWaterState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _bobber.transform.parent = null;
        _bobber.StartCoroutine(AttractFishToBobber());
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButton(1))
        {
            _bobber.SetState(new BobberBeingReeledInState(_bobber));
        }
    }

    public override void ExitState()
    {
        _bobber.StopAllCoroutines();
    }

    /**
     * I think this method needs to live inside one of the Fish classes
     * Possibly in Idle and check for if the bobber is in the water
     */
    private IEnumerator AttractFishToBobber()
    {
        while (
            FishController.Instance.CurrentStateType == FishStateType.Idle)
        {
            // Weird name because we aren't checking for a bite but rather how often to try and spawn a fish
            float nextBiteCheckIntervalRandom = _bobber.Rng.Generate();
            yield return new WaitForSeconds(nextBiteCheckIntervalRandom);
            FishController.Instance.CheckFishInterestInBobber();
        }
    }
}
