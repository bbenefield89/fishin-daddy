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
    public float swimSpeed = 1f;
    public float timeUntilNextSwim = 5f;
    public float swimDistance = 5f;
    public float boundaryOffset = 1f;
    public int tier;

    private Vector3 oldPos;
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
            Vector3 oldPosition = transform.position;
            oldPos = transform.position;
            targetPosition = DetermineSwimToPos();

            RotateFish();

            // Swim towards the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                //transform.position = Vector3.MoveTowards(
                //    transform.position,
                //    targetPosition,
                //    swimSpeed * Time.deltaTime);

                transform.parent.position = Vector3.MoveTowards(
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

    private void RotateFish()
    {
        if (Mathf.Abs(oldPos.x - targetPosition.x) > Mathf.Abs(oldPos.z - targetPosition.z))
        {
            // Moving more in the X direction
            if (oldPos.x < targetPosition.x)
            {
                // Moving in positive X direction
                transform.eulerAngles = new Vector3(0, -90, 0);
            }
            else
            {
                // Moving in negative X direction
                transform.eulerAngles = new Vector3(0, 90, 0);
            }
        }
        else
        {
            // Moving more in the Z direction
            if (oldPos.z < targetPosition.z)
            {
                // Moving in positive Z direction
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                // Moving in negative Z direction
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}
