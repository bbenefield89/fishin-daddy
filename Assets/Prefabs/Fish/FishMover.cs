using System.Collections;
using UnityEngine;

public class FishMover : MonoBehaviour
{
    [Tooltip("How fast this fish can swim")]
    public float swimSpeed = 1f;

    [Tooltip("Time until the fish picks a new spot to swim towards")]
    public float timeUntilNextSwim = 5f;

    [Tooltip("The center of the area in which the fish can move")]
    public Vector3 swimAreaCenter = Vector3.zero;

    [Tooltip("The size of the area in which the fish can move")]
    public float swimDistance = 10f;

    [Tooltip("An offset to prevent fish from getting to close to the ground")]
    public float fishToGroundOffset = 1f;

    private GameObject waterObject;
    private Vector3 targetPosition;
    private Bounds swimArea;

    private void Start()
    {
        waterObject = GameObject.Find(Prefabs.WATER);
        swimArea = waterObject.GetComponent<BoxCollider>().bounds;
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
                Vector3 newPosition = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    swimSpeed * Time.deltaTime);

                // Make sure the new position is within the swim area
                if (swimArea.Contains(newPosition))
                {
                    transform.position = newPosition;
                }

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
        float[] possibleX = new float[2]
        {
                Random.Range(groundBounds.min.x - fishToGroundOffset, swimDistance * -1),
                Random.Range(groundBounds.max.x + fishToGroundOffset, swimDistance),
        };

        float[] possibleZ = new float[2]
        {
                Random.Range(groundBounds.min.z - fishToGroundOffset, swimDistance * -1),
                Random.Range(groundBounds.max.z + fishToGroundOffset, swimDistance),
        };

        float randomX = possibleX[Random.Range(0, 2)];
        float randomZ = possibleZ[Random.Range(0, 2)];
        Vector3 swimToPos = new Vector3(randomX, 0f, randomZ);

        return swimToPos;
    }

    //private Vector3 DetermineSwimToPos()
    //{
    //    Vector3 swimToPos;
    //    bool hitGround;

    //    do
    //    {
    //        float randomX = Random.Range(swimArea.min.x, swimArea.max.x);
    //        float randomZ = Random.Range(swimArea.min.z, swimArea.max.z);
    //        swimToPos = new Vector3(randomX, 0f, randomZ);

    //        // Perform a capsule cast from the fish's current position to the new position
    //        Vector3 top = transform.position + Vector3.up * 0.5f; // Top of the capsule
    //        Vector3 bottom = transform.position + Vector3.down * 0.5f; // Bottom of the capsule
    //        float radius = 0.5f; // Radius of the capsule

    //        RaycastHit hit;
    //        if (Physics.CapsuleCast(top, bottom, radius, swimToPos - transform.position, out hit))
    //        {
    //            // If the capsule hit the Ground object, we need to pick a new position
    //            hitGround = hit.collider.gameObject.CompareTag(Tags.GROUND);
    //        }
    //        else
    //        {
    //            // If the capsule didn't hit anything, the position is good
    //            hitGround = false;
    //        }
    //    }
    //    while (hitGround); // Keep picking new positions until we find one that doesn't hit the Ground

    //    return swimToPos;
    //}

}
