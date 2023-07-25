using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoundaryType
{
    MIN,
    MAX,
}

public class FishMover : MonoBehaviour
{
    [Tooltip("How fast this fish can swim")]
    public float swimSpeed = 1f;

    [Tooltip("Time until the fish picks a new spot to swim towards")]
    public float timeUntilNextSwim = 5f;

    [Tooltip("The center of the area in which the fish can move")]
    public Vector3 swimAreaCenter = Vector3.zero;

    [Tooltip("The size of the area in which the fish can move")]
    public float swimDistance = 5f;

    [Tooltip("An offset to prevent fish from getting to close to the ground")]
    public float boundaryOffset = 1f;

    public int tier;

    private Vector3 targetPosition;
    private delegate Vector3 CheckAxisMethods(Dictionary<string, float> boundaries);

    private void Start()
    {
        StartCoroutine(MoveFish());
    }

    private IEnumerator MoveFish()
    {
        while (true)
        {
            // Determine a new target position
            targetPosition = DetermineSwimToPos();

            // Swim towards the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    swimSpeed * Time.deltaTime);

                yield return null;
            }

            // Wait for a certain amount of time before picking a new spot to swim towards
            yield return new WaitForSeconds(timeUntilNextSwim);
        }
    }

    private Vector3 DetermineSwimToPos()
    {
        GameObject groundObject = GameObject.Find(Prefabs.GROUND);
        Bounds groundBounds = groundObject.GetComponent<BoxCollider>().bounds;

        float prevBoundaryMinZ = CalcPrevBoundary(groundBounds.center.z, groundBounds.extents.z, BoundaryType.MIN);
        float prevBoundaryMaxZ = CalcPrevBoundary(groundBounds.center.z, groundBounds.extents.z, BoundaryType.MAX);

        float prevBoundaryMinX = CalcPrevBoundary(groundBounds.center.x, groundBounds.extents.x, BoundaryType.MIN);
        float prevBoundaryMaxX = CalcPrevBoundary(groundBounds.center.x, groundBounds.extents.x, BoundaryType.MAX);

        Dictionary<string, float> boundaries = new Dictionary<string, float>
        {
            { "prevBoundaryMinZ", prevBoundaryMinZ },
            { "nextBoundaryMinZ", CalcNextBoundary(prevBoundaryMinZ, BoundaryType.MIN) },

            { "prevBoundaryMaxZ", prevBoundaryMaxZ },
            { "nextBoundaryMaxZ", CalcNextBoundary(prevBoundaryMaxZ, BoundaryType.MAX) },

            { "prevBoundaryMinX", prevBoundaryMinX },
            { "nextBoundaryMinX", CalcNextBoundary(prevBoundaryMinX, BoundaryType.MIN) },

            { "prevBoundaryMaxX", prevBoundaryMaxX },
            { "nextBoundaryMaxX", CalcNextBoundary(prevBoundaryMaxX, BoundaryType.MAX) },
        };

        CheckAxisMethods[] axisFunctions = new CheckAxisMethods[]
        {
            CheckZAxisFirst,
            CheckXAxisFirst,
        };

        return axisFunctions[Random.Range(0, 2)](boundaries);
    }

    private float CalcPrevBoundary(float centerAxis, float extentsAxis, BoundaryType boundaryType)
    {
        return boundaryType == BoundaryType.MIN ?
            (centerAxis + extentsAxis) * tier * -1 - boundaryOffset :
            (centerAxis + extentsAxis) * tier + boundaryOffset;
    }

    private float CalcNextBoundary(float prevBoundary, BoundaryType boundaryType)
    {
        return boundaryType == BoundaryType.MIN ?
            prevBoundary - swimDistance + (boundaryOffset * 2) :
            prevBoundary + swimDistance - (boundaryOffset * 2);
    }

    private Vector3 CheckZAxisFirst(Dictionary<string, float> boundaries)
    {
        float nextXPos;
        float nextZPos;

        if (transform.position.z < boundaries["prevBoundaryMinZ"])
        {
            nextZPos = Random.Range(boundaries["nextBoundaryMinZ"], boundaries["prevBoundaryMinZ"]);
            nextXPos = Random.Range(boundaries["nextBoundaryMinX"], boundaries["nextBoundaryMaxX"]);
        }
        else if (transform.position.z > boundaries["prevBoundaryMaxZ"])
        {
            nextZPos = Random.Range(boundaries["prevBoundaryMaxZ"], boundaries["nextBoundaryMaxZ"]);
            nextXPos = Random.Range(boundaries["nextBoundaryMinX"], boundaries["nextBoundaryMaxX"]);
        }
        else
        {
            nextZPos = Random.Range(boundaries["nextBoundaryMinZ"], boundaries["nextBoundaryMaxZ"]);

            if (transform.position.x < boundaries["prevBoundaryMinX"])
            {
                nextXPos = Random.Range(boundaries["prevBoundaryMinX"], boundaries["nextBoundaryMinX"]);
            }
            else
            {
                nextXPos = Random.Range(boundaries["prevBoundaryMaxX"], boundaries["nextBoundaryMaxX"]);
            }
        }

        return new Vector3(nextXPos, transform.position.y, nextZPos);
    }


    private Vector3 CheckXAxisFirst(Dictionary<string, float> boundaries)
    {
        float nextXPos;
        float nextZPos;

        if (transform.position.x < boundaries["prevBoundaryMinX"])
        {
            nextXPos = Random.Range(boundaries["nextBoundaryMinX"], boundaries["prevBoundaryMinX"]);
            nextZPos = Random.Range(boundaries["nextBoundaryMinZ"], boundaries["nextBoundaryMaxZ"]);
        }
        else if (transform.position.x > boundaries["prevBoundaryMaxX"])
        {
            nextXPos = Random.Range(boundaries["prevBoundaryMaxX"], boundaries["nextBoundaryMaxX"]);
            nextZPos = Random.Range(boundaries["nextBoundaryMinZ"], boundaries["nextBoundaryMaxZ"]);
        }
        else
        {
            nextXPos = Random.Range(boundaries["nextBoundaryMinZ"], boundaries["nextBoundaryMaxZ"]);

            if (transform.position.z < boundaries["prevBoundaryMinZ"])
            {
                nextZPos = Random.Range(boundaries["prevBoundaryMinZ"], boundaries["nextBoundaryMinZ"]);
            }
            else
            {
                nextZPos = Random.Range(boundaries["prevBoundaryMaxZ"], boundaries["nextBoundaryMaxZ"]);
            }
        }

        return new Vector3(nextXPos, transform.position.y, nextZPos);
    }

}
