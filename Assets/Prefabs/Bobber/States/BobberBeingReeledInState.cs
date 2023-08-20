using UnityEngine;

public class BobberBeingReeledInState : BobberState
{
    public override BobberStateType State => BobberStateType.BeingReeledIn;

    public BobberBeingReeledInState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        //
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButton(1))
        {
            _bobber.Exclamations.SetActive(false);
            ReelBobberIn();

            if (FishController.Instance.CheckIfFishShouldSwimAway())
            {
                FishController.Instance.SetState(
                    new FishSwimmingAwayState(FishController.Instance));
            }
        }
        else
        {
            if (FishController.Instance.CheckIfFishIsHooked())
            {
                _bobber.SetState(new BobberHookedFishState(_bobber));
            }
            else
            {
                _bobber.SetState(new BobberInWaterState(_bobber));
            }
        }
    }

    public override void ExitState()
    {
        _bobber.StopAllCoroutines();
    }

    private void ReelBobberIn()
    {
        if (FishController.Instance.CheckIfFishIsHooked())
        {
            // Bobber should not reel in at full speed
            // Fish should not swim away at full speed
            // Tension should start to build up in fishing line
            // If tension gets too high line should snap
        }
        else
        {
            Vector3 targetPos = new Vector3(
                _bobber.BobberReturnPosition.position.x,
                _bobber.WaterLevel,
                _bobber.BobberReturnPosition.position.z);

            _bobber.transform.position = Vector3.MoveTowards(
                _bobber.transform.position,
                targetPos,
                _bobber.ReelSpeed * Time.deltaTime);
        }
    }
}
