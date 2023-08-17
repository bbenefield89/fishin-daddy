using System.Collections;
using UnityEngine;

public class FishHookedState : FishState
{
    public override FishStateType State { get; } = FishStateType.Hooked;

    public FishHookedState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        _fish.StartCoroutine(FollowBobber());
    }

    public override void UpdateState()
    {
        if (_fish.CurrentStateType == FishStateType.Idle)
        {
            _fish.SetState(new FishIdleState(_fish));
        }
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
    }

    private IEnumerator FollowBobber()
    {
        while (true)
        {
            float reelSpeed = BobberController.Instance.ReelSpeed;
            Vector3 bobberPos = BobberController.Instance.transform.position;
            Vector3 targetPos = new Vector3(
                bobberPos.x,
                _fish.transform.position.y,
                bobberPos.z);

            Vector3 pcPos = PCModel.Instance.transform.position;
            Vector3 lookToPos = new Vector3(
                pcPos.x,
                _fish.transform.position.y,
                pcPos.z);

            _fish.transform.rotation = Quaternion.LookRotation(
                lookToPos - _fish.transform.position);

            _fish.transform.position = Vector3.MoveTowards(
                _fish.transform.position,
                targetPos,
                reelSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
