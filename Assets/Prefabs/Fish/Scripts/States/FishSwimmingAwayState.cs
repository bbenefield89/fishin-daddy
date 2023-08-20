using System.Collections;
using UnityEngine;

public class FishSwimmingAwayState : FishState
{
    public override FishStateType State { get; } = FishStateType.SwimmingAway;

    public FishSwimmingAwayState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        _fish.StopAllCoroutines();
        _fish.StartCoroutine(BeginSwimAway());
    }

    public override void UpdateState()
    {
        //
    }

    public override void ExitState()
    {
        _fish.StopAllCoroutines();
    }

    private IEnumerator BeginSwimAway()
    {
        _fish.StartCoroutine(SwimAway());
        yield return FadeAway();
        _fish.Reset();
    }

    private IEnumerator SwimAway()
    {
        Vector3 currentPos = _fish.transform.position;
        Vector3 bobberPos = BobberController.Instance.transform.position;
        Vector3 dirToSwim = (currentPos - bobberPos).normalized;
        Vector3 swimToPos = dirToSwim * 20f;
        yield return _fish.MoveRoutine(swimToPos, true);
    }

    private IEnumerator FadeAway()
    {
        Renderer renderer = _fish.GetComponentInChildren<Renderer>();
        Color fishModelColor = renderer.material.color;

        while (fishModelColor.a > 0f)
        {
            float deltaAlpha = _fish.FadeAwaySpeed * Time.deltaTime;
            fishModelColor.a -= deltaAlpha;
            renderer.material.color = fishModelColor;

            yield return null;
        }
    }

}
