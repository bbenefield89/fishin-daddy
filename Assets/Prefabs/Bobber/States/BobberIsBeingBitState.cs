using System;
using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetMouseButtonDown(1))
        {
            TimeSpan timeSinceBobberWasBit = DateTime.UtcNow - _timeBobberWasBit;

            if (timeSinceBobberWasBit.TotalSeconds > _bobber.TimeAllowedToHookFish)
            {
                Debug.Log("Fish Got Away");
            }
            else
            {
                HookFish();
            }
        }
    }

    public override void ExitState()
    {
        _bobber.IsBeingBit = false;
    }

    private void HideBobber()
    {
        Vector3 pos = _bobber.transform.position;
        Vector3 newPos = new Vector3(pos.x, _bobber.WaterLevel - 1f, pos.z);
        _bobber.transform.position = newPos;
    }

    public void HookFish()
    {
        FishController.Instance.IsFishHooked = true;
        _bobber.SetState(new BobberInWaterState(_bobber));
    }
}
