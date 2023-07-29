using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishMover : MonoBehaviour
{
    public enum BoundaryType
    {
        MIN,
        MAX,
    }

    [System.Serializable]
    public struct NibbleTiming
    {
        public float min;
        public float max;
    }

    public float swimSpeed = 1f;
    public float timeUntilNextSwim = 5f;
    public float swimDistance = 5f;
    public float boundaryOffset = 1f;
    public int tier;
    public NibbleTiming nibbleTiming;
    public GameObject bobber;

    private Vector3 targetPosition;
    private IEnumerator currentMovementCoroutine;

    private delegate Vector3 CheckAxisMethods(Dictionary<string, float> boundaries);

    private void Start()
    {
        bobber = GameObject.Find(Prefabs.BOBBER_MODEL);
        currentMovementCoroutine = MoveFish();
        StartCoroutine(currentMovementCoroutine);
    }

    private IEnumerator MoveFish()
    {
        while (true)
        {
            // Determine a new target position
            targetPosition = DetermineSwimToPos();

            RotateFish();

            // Swim towards the target position
            while (Vector3.Distance(transform.parent.position, targetPosition) > 0.01f)
            {
                transform.parent.position = Vector3.MoveTowards(
                    transform.parent.position,
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

        if (transform.parent.position.z < boundaries["prevBoundaryMinZ"])
        {
            nextZPos = Random.Range(boundaries["nextBoundaryMinZ"], boundaries["prevBoundaryMinZ"]);
            nextXPos = Random.Range(boundaries["nextBoundaryMinX"], boundaries["nextBoundaryMaxX"]);
        }
        else if (transform.parent.position.z > boundaries["prevBoundaryMaxZ"])
        {
            nextZPos = Random.Range(boundaries["prevBoundaryMaxZ"], boundaries["nextBoundaryMaxZ"]);
            nextXPos = Random.Range(boundaries["nextBoundaryMinX"], boundaries["nextBoundaryMaxX"]);
        }
        else
        {
            nextZPos = Random.Range(boundaries["nextBoundaryMinZ"], boundaries["nextBoundaryMaxZ"]);

            if (transform.parent.position.x < boundaries["prevBoundaryMinX"])
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

        if (transform.parent.position.x < boundaries["prevBoundaryMinX"])
        {
            nextXPos = Random.Range(boundaries["nextBoundaryMinX"], boundaries["prevBoundaryMinX"]);
            nextZPos = Random.Range(boundaries["nextBoundaryMinZ"], boundaries["nextBoundaryMaxZ"]);
        }
        else if (transform.parent.position.x > boundaries["prevBoundaryMaxX"])
        {
            nextXPos = Random.Range(boundaries["prevBoundaryMaxX"], boundaries["nextBoundaryMaxX"]);
            nextZPos = Random.Range(boundaries["nextBoundaryMinZ"], boundaries["nextBoundaryMaxZ"]);
        }
        else
        {
            nextXPos = Random.Range(boundaries["nextBoundaryMinZ"], boundaries["nextBoundaryMaxZ"]);

            if (transform.parent.position.z < boundaries["prevBoundaryMinZ"])
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
        Vector3 startPos = transform.parent.position;

        if (Mathf.Abs(startPos.x - targetPosition.x) > Mathf.Abs(startPos.z - targetPosition.z))
        {
            // Moving more in the X direction
            if (startPos.x < targetPosition.x)
            {
                // Moving in positive X direction
                transform.parent.eulerAngles = new Vector3(0, -90, 0);
            }
            else
            {
                // Moving in negative X direction
                transform.parent.eulerAngles = new Vector3(0, 90, 0);
            }
        }
        else
        {
            // Moving more in the Z direction
            if (startPos.z < targetPosition.z)
            {
                // Moving in positive Z direction
                transform.parent.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                // Moving in negative Z direction
                transform.parent.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.BOBBER))
        {
            bool isFishInterested = Random.Range(0, 2) == 0 ? false : true;
            if (!FishManager.Instance.IsFishHooked && isFishInterested)
            {
                StopCoroutine(currentMovementCoroutine);

                currentMovementCoroutine = MoveTowardsBobber();
                StartCoroutine(currentMovementCoroutine);
            }
        }
    }

    public IEnumerator MoveTowardsBobber()
    {
        targetPosition = bobber.transform.position;
        RotateFish();
        yield return ApproachBobber();
        yield return HookFish();
    }

    private IEnumerator ApproachBobber()
    {
        while (Vector3.Distance(transform.parent.position, targetPosition) > 0.2f)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, targetPosition, swimSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator HookFish()
    {
        if (!FishManager.Instance.IsFishHooked)
        {
            bool isThisFishHooked = Random.Range(0, 2) == 0 ? false : true;
            if (isThisFishHooked)
            {
                GetComponent<FishHooked>().HookFish();
            }
            else
            {
                currentMovementCoroutine = MoveAwayFromBobber();
                StartCoroutine(currentMovementCoroutine);
            }
        }
        else
        {
            ResumeNormalMovement();
        }

        yield return null;
    }

    public IEnumerator MoveAwayFromBobber()
    {
        Vector3 distanceFromBobber = (transform.parent.position - bobber.transform.position) * 2;
        targetPosition = transform.parent.position + distanceFromBobber;
        yield return ApproachBobber();
        yield return new WaitForSeconds(Random.Range(nibbleTiming.min, nibbleTiming.max));

        currentMovementCoroutine = MoveTowardsBobber();
        StartCoroutine(currentMovementCoroutine);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.BOBBER))
        {
            StopCoroutine(currentMovementCoroutine);

            currentMovementCoroutine = MoveFish();
            StartCoroutine(currentMovementCoroutine);
        }
    }

    public void ResumeNormalMovement()
    {
        StopCoroutine(currentMovementCoroutine);

        currentMovementCoroutine = MoveFish();
        StartCoroutine(currentMovementCoroutine);
    }
}
