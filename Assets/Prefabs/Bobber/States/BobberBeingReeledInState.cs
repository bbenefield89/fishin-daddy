using UnityEngine;

public class BobberBeingReeledInState : BobberState
{
    public BobberBeingReeledInState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _bobber.IsBeingReeledIn = true;
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButton(1))
        {
            _bobber.Exclamations.SetActive(false);
            ReelBobberIn();

            if (
                FishController.Instance.CurrentStateType != FishStateType.Idle &&
                FishController.Instance.CurrentStateType != FishStateType.Hooked &&
                FishController.Instance.CurrentStateType != FishStateType.SwimmingAway)
            {
                _bobber.InvokeFishShouldSwimAway();
            }
        }
    }

    public override void ExitState()
    {
        _bobber.IsBeingReeledIn = false;
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
