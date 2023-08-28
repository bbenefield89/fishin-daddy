using System.Collections;
using UnityEngine;

public class FishFightingState : FishState
{
    public override FishStateType State { get; } = FishStateType.Fighting;
    private Coroutine _fightingCoroutine;

    public FishFightingState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        Debug.Log("Fish Fighting");
        _fish.StartCoroutine(BeginFighting());
    }

    public override void UpdateState()
    {
        //
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
    }

    private IEnumerator BeginFighting()
    {
        float startTime = Time.time;
        float timeUntilRest = _fish.Rng.Generate();
        float endTime = startTime + timeUntilRest;

        while (Time.time < endTime)
        {
            if (_fightingCoroutine != null)
            {
                _fish.StopCoroutine(_fightingCoroutine);
            }

            if (Input.GetMouseButton(1))
            {
                Vector3 pcModelPos = PCModel.Instance.transform.position;
                Vector3 fishPos = _fish.transform.position;
                Vector3 targetPos = new Vector3(
                    pcModelPos.x,
                    fishPos.y,
                    pcModelPos.z);

                _fightingCoroutine = _fish.StartCoroutine(
                    _fish.MoveRoutine(targetPos, false, 0.5f));
            }
            else
            {
                _fightingCoroutine = _fish.StartCoroutine(
                    _fish.SwimAway(2f, _fish.FightSwimSpeed));
            }

            yield return null;
        }

        _fish.SetState(new FishRestingState(_fish));
    }
}
