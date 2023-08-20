using System;
using UnityEngine;

public class BobberIsBeingBitState : BobberState
{
    public override BobberStateType State => BobberStateType.BeingBit;
    private DateTime _timeBobberWasBit;

    public BobberIsBeingBitState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _timeBobberWasBit = DateTime.UtcNow;
        _bobber.Exclamations.SetActive(true);
        HideBobber();
    }

    public override void UpdateState()
    {
        TimeSpan timeSinceBobberWasBit = DateTime.UtcNow - _timeBobberWasBit;

        if (
            Input.GetMouseButtonDown(1) &&
            timeSinceBobberWasBit.TotalSeconds <= _bobber.TimeAllowedToHookFish)
        {
            HookFish();
        }
        else if (timeSinceBobberWasBit.TotalSeconds > _bobber.TimeAllowedToHookFish)
        {
            ShowBobber();
            BobberController.Instance.InvokeFishShouldSwimAway();
            _bobber.SetState(new BobberInWaterState(_bobber));
        }
    }

    public override void ExitState()
    {
        _bobber.Exclamations.SetActive(false);
    }

    private void HideBobber()
    {
        //Vector3 pos = _bobber.transform.position;
        //Vector3 newPos = new Vector3(pos.x, _bobber.WaterLevel - 1f, pos.z);
        //_bobber.transform.position = newPos;
    }

    private void ShowBobber()
    {
        Vector3 pos = _bobber.transform.position;
        Vector3 newPos = new Vector3(pos.x, _bobber.WaterLevel, pos.z);
        _bobber.transform.position = newPos;
    }

    public void HookFish()
    {
        //BobberController.Instance.InvokeFishShouldBeHooked();
        FishController.Instance.SetState(new FishFightingState(FishController.Instance));
        _bobber.SetState(new BobberHookedFishState(_bobber));
    }
}
