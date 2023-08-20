using System.Collections;
using UnityEngine;

public class BobberCastingState : BobberState
{
    public override BobberStateType State => BobberStateType.Casting;

    public BobberCastingState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _bobber.StartCoroutine(CastBobber());
    }

    public override void UpdateState()
    {
        //
    }

    public override void ExitState()
    {
        _bobber.StopAllCoroutines();
    }

    private IEnumerator CastBobber()
    {
        yield return MoveBobber();
        yield return DropBobberToWater();
        _bobber.SetState(new BobberInWaterState(_bobber));
    }

    private IEnumerator MoveBobber()
    {
        Vector3 bobberStartingPos = _bobber.transform.position;
        Vector3 castDirection = _bobber.PcModel.forward; // The direction the PCModel is facing
        Vector3 bobberLandingPos = bobberStartingPos + castDirection * _bobber.CastDistance;
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(bobberStartingPos, bobberLandingPos);
        float fracJourney = 0;

        // Move the bobber to the cast distance along a curve
        while (fracJourney < 1)
        {
            float distCovered = (Time.time - startTime) * _bobber.BobberMovementSpeed;
            fracJourney = distCovered / journeyLength;

            // Calculate the current position on the arc
            Vector3 currentPos = Vector3.Lerp(bobberStartingPos, bobberLandingPos, fracJourney);

            // Add a vertical offset to create the arc
            currentPos.y += _bobber.ArcHeight * Mathf.Sin(fracJourney * Mathf.PI);

            _bobber.transform.position = currentPos;

            yield return null;
        }
    }

    private IEnumerator DropBobberToWater()
    {
        Vector3 start = _bobber.transform.position;
        Vector3 end = new Vector3(start.x, _bobber.WaterLevel, start.z);
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(start, end);
        float fracJourney = 0;

        // Move the bobber downward to the water level
        while (fracJourney < 1)
        {
            float distCovered = (Time.time - startTime) * _bobber.BobberMovementSpeed;
            fracJourney = distCovered / journeyLength;
            _bobber.transform.position = Vector3.Lerp(start, end, fracJourney);
            yield return null;
        };
    }
}
