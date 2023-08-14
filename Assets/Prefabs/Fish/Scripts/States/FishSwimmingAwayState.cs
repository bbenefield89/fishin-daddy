using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSwimmingAwayState : FishState
{
    public FishSwimmingAwayState(FishController fish) : base(fish) { }

    public override void EnterState()
    {
        _fish.StopAllCoroutines();
        _fish.IsSwimmingAway = true;
        _fish.StartCoroutine(BeginSwimAway());
    }

    public override void UpdateState()
    {
        //
    }

    public override void ExitState()
    {
        _fish.IsSwimmingAway = false;
        _fish.StopAllCoroutines();
    }

    private IEnumerator BeginSwimAway()
    {
        _fish.StartCoroutine(SwimAway());
        yield return FadeAway();
        _fish.Reset();
        // Fish should rotate to face correct swimming direction
        // Found bug, fish "stutters" when begins swimming around bobber
    }

    private IEnumerator SwimAway()
    {
        Vector3 currentPos = _fish.transform.position;
        Vector3 bobberPos = BobberController.Instance.transform.position;
        Vector3 dirToSwim = (currentPos - bobberPos).normalized;
        Vector3 swimToPos = dirToSwim * 20f;
        Func<bool> notAtSwimToPos = () =>
        {
            Vector3 fishPos = _fish.transform.position;
            return Vector3.Distance(fishPos, swimToPos) > 0.1f;
        };

        while (notAtSwimToPos())
        {
            _fish.transform.position = Vector3.MoveTowards(
                _fish.transform.position,
                swimToPos,
                _fish.SwimSpeed * Time.deltaTime);

            yield return null;
        }
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
