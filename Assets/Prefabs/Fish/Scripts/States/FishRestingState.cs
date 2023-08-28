using System.Collections;
using UnityEngine;

public class FishRestingState : FishState
{
    public override FishStateType State { get; } = FishStateType.Resting;
    private Coroutine _restingCoroutine;

    public FishRestingState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        Debug.Log("Fish Resting");
        _fish.StartCoroutine(BeginResting());
    }

    public override void UpdateState()
    {
        //
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
    }

    private IEnumerator BeginResting()
    {
        float startTime = Time.time;
        float timeUntilFight = _fish.Rng.Generate() * 2;
        float endTime = startTime + timeUntilFight;

        while (Time.time < endTime)
        {
            if (_restingCoroutine != null)
            {
                _fish.StopCoroutine(_restingCoroutine);
            }

            if (Input.GetMouseButton(1))
            {
                Vector3 pcModelPos = PCModel.Instance.transform.position;
                Vector3 fishPos = _fish.transform.position;
                Vector3 targetPos = new Vector3(
                    pcModelPos.x,
                    fishPos.y,
                    pcModelPos.z);

                _restingCoroutine = _fish.StartCoroutine(
                    _fish.MoveRoutine(targetPos, true));
            }

            yield return null;
        }

        _fish.SetState(new FishFightingState(_fish));
    }
}
