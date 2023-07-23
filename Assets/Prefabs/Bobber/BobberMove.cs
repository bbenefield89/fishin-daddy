using System.Collections;
using UnityEngine;

public class BobberMove : MonoBehaviour
{
    [Tooltip("The PCModel game object")]
    public Transform pcModel;

    [Tooltip("GameObject that holds a reference to the position the bobber should be when not casted")]
    public Transform bobberReturnPosition;

    [Tooltip("The maximum distance the bobber can be cast from the fishing pole")]
    public float castDistance = 1f;

    [Tooltip("The y-coordinate of the water level")]
    public float waterLevel = 0f;

    [Tooltip("How fast the bobber moves towards its destination after casting")]
    public float bobberMovementSpeed = 5f;

    [Tooltip("The maximum height of the arc")]
    public float arcHeight = 1f;

    private bool isCasting = false;
    private bool hasBeenCasted = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isCasting && !hasBeenCasted)
        {
            StartCoroutine(CastBobber());
        }
        else if (Input.GetMouseButtonDown(1) && !isCasting && hasBeenCasted)
        {
            ReturnBobberToFishingPole();
        }
    }

    private IEnumerator CastBobber()
    {
        isCasting = true;
        hasBeenCasted = true;

        yield return MoveBobber();
        yield return DropBobberToWater();

        isCasting = false;
        transform.parent = null;  // Remove the bobber from its parent to prevent it from moving/rotating when the PC moves/rotates
    }

    private IEnumerator MoveBobber()
    {
        Vector3 bobberStartingPos = transform.position;
        Vector3 castDirection = pcModel.forward; // The direction the PCModel is facing
        Vector3 bobberLandingPos = bobberStartingPos + castDirection * castDistance;
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(bobberStartingPos, bobberLandingPos);
        float fracJourney = 0;

        // Move the bobber to the cast distance along a curve
        while (fracJourney < 1)
        {
            float distCovered = (Time.time - startTime) * bobberMovementSpeed;
            fracJourney = distCovered / journeyLength;

            // Calculate the current position on the arc
            Vector3 currentPos = Vector3.Lerp(bobberStartingPos, bobberLandingPos, fracJourney);

            // Add a vertical offset to create the arc
            currentPos.y += arcHeight * Mathf.Sin(fracJourney * Mathf.PI);

            transform.position = currentPos;

            yield return null;
        }
    }

    private IEnumerator DropBobberToWater()
    {
        Vector3 start = transform.position;
        Vector3 end = new Vector3(start.x, waterLevel, start.z);
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(start, end);
        float fracJourney = 0;

        // Move the bobber downward to the water level
        while (fracJourney < 1)
        {
            float distCovered = (Time.time - startTime) * bobberMovementSpeed;
            fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(start, end, fracJourney);
            yield return null;
        }
    }


    private void ReturnBobberToFishingPole()
    {
        hasBeenCasted = false;
        transform.position = bobberReturnPosition.position;
        transform.parent = bobberReturnPosition;
    }
}
