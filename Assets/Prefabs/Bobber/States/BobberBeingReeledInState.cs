using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberBeingReeledInState : BobberState
{
    public BobberBeingReeledInState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _bobber.StartCoroutine(ReelBobberIn());
    }

    public override void UpdateState()
    {
        // Nothing to do here
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
                _bobber.Exclamations.SetActive(false);

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
