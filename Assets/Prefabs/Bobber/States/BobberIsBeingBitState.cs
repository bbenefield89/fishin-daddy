using System;
using UnityEngine;

public class BobberIsBeingBitState : BobberState
{
    private DateTime _timeBobberWasBit;

    public BobberIsBeingBitState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _bobber.IsBeingBit = true;
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
            FishController.Instance.IsSwimmingAway = true;
            _bobber.SetState(new BobberInWaterState(_bobber));
        }
    }

    public override void ExitState()
    {
        _bobber.IsBeingBit = false;
        _bobber.Exclamations.SetActive(false);
    }

    private void HideBobber()
    {
        Vector3 pos = _bobber.transform.position;
        Vector3 newPos = new Vector3(pos.x, _bobber.WaterLevel - 1f, pos.z);
        _bobber.transform.position = newPos;
    }

    private void ShowBobber()
    {
        Vector3 pos = _bobber.transform.position;
        Vector3 newPos = new Vector3(pos.x, _bobber.WaterLevel, pos.z);
        _bobber.transform.position = newPos;
    }

    public void HookFish()
    {
        FishController.Instance.IsHooked = true;
        _bobber.SetState(new BobberInWaterState(_bobber));
    }
}
