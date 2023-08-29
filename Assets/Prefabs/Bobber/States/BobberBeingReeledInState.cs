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
            FishingPole.Instance.PlayReelingSfx();
            ReelBobberIn();

            if (FishController.Instance.CheckIfFishShouldSwimAway())
            {
                FishController.Instance.SetState(
                    new FishSwimmingAwayState(FishController.Instance));
            }
        }
        else
        {
            FishingPole.Instance.StopReelingSfx();
            _bobber.SetState(new BobberInWaterState(_bobber));
        }
    }

    public override void ExitState()
    {
        _bobber.StopAllCoroutines();
    }

    private void ReelBobberIn()
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
