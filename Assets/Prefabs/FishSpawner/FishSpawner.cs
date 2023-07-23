using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject waterObject;
    public GameObject groundObject;
    public GameObject fishObject;
    public float tier1SpawnDistance = 10f;
    public float tier2SpawnDistance = 20f;

    private List<GameObject> fish = new List<GameObject> { };

    [Min(1)]
    public float timeBetweenSpawn = 1f;

    private void Start()
    {
        StartCoroutine(SpawnFish());
    }

    private IEnumerator SpawnFish()
    {
        while (true)
        {
            GameObject newFish = Instantiate(fishObject);
            newFish.name = "Fish_" + fish.Count.ToString();
            newFish.transform.position = DetermineSpawnPos();
            fish.Add(newFish);

            yield return new WaitForSeconds(Random.Range(1f, timeBetweenSpawn));
        }
    }

    private Vector3 DetermineSpawnPos()
    {
        Vector3 spawnPos;
        bool hitGround;
        Bounds waterBounds = waterObject.GetComponent<BoxCollider>().bounds;
        Bounds groundBounds = groundObject.GetComponent<BoxCollider>().bounds;

        do
        {
            float[] possibleX = new float[2]
            {
                Random.Range(groundBounds.min.x, tier1SpawnDistance * -1),
                Random.Range(groundBounds.max.x, tier1SpawnDistance),
            };

            float[] possibleZ = new float[2]
            {
                Random.Range(groundBounds.min.z, tier1SpawnDistance * -1),
                Random.Range(groundBounds.max.z, tier1SpawnDistance),
            };

            float randomX = possibleX[Random.Range(0, 2)];
            float randomZ = possibleZ[Random.Range(0, 2)];

            spawnPos = new Vector3(randomX, 0f, randomZ);

            // Perform a capsule cast from the fish's current position to the new position
            Vector3 top = transform.position + Vector3.up * 0.5f; // Top of the capsule
            Vector3 bottom = transform.position + Vector3.down * 0.5f; // Bottom of the capsule
            float radius = 0.5f; // Radius of the capsule

            RaycastHit hit;
            if (Physics.CapsuleCast(top, bottom, radius, spawnPos - transform.position, out hit))
            {
                // If the capsule hit the Ground object, we need to pick a new position
                hitGround = hit.collider.gameObject.CompareTag(Tags.GROUND);
            }
            else
            {
                // If the capsule didn't hit anything, the position is good
                hitGround = false;
            }
        }
        while (hitGround); // Keep picking new positions until we find one that doesn't hit the Ground

        return spawnPos;
    }
}
