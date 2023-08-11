using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberBeingReeledInState : BobberState
{
    public BobberBeingReeledInState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _bobber.Exclamations.SetActive(false);
        _bobber.StartCoroutine(ReelBobberIn());
    }

    public override void UpdateState()
    {
        //
    }

    public override void ExitState()
    {
        _bobber.IsBeingReeledIn = false;
    }

    private IEnumerator ReelBobberIn()
    {
        while (true)
        {
            if (Input.GetMouseButton(1))
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

            yield return null;
        }
    }
}
