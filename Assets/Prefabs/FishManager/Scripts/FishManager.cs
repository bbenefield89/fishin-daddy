using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager Instance { get; private set; }

    public bool IsFishHooked { get; set; }
    public GameObject waterObject;
    public GameObject groundObject;
    public GameObject fishPrefab;
    public List<GameObject> fish = new List<GameObject>();
    public float tier1SpawnDistance = 10f;
    public float tier2SpawnDistance = 20f;
    public int fishSpawnAmount = 1;
    public float timeBetweenSpawn = 5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnFish());
    }

    private IEnumerator SpawnFish()
    {
        while (true)
        {
            if (fish.Count < fishSpawnAmount)
            {
                GameObject newFish = Instantiate(fishPrefab);
                newFish.name = "Fish_" + fish.Count.ToString();
                newFish.GetComponentInChildren<FishMover>().tier = 1;
                newFish.transform.position = DetermineSpawnPos();
                fish.Add(newFish);
            }

            yield return new WaitForSeconds(Random.Range(1f, timeBetweenSpawn));
        }
    }

    private Vector3 DetermineSpawnPos()
    {
        Bounds groundBounds = groundObject.GetComponent<BoxCollider>().bounds;
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
        Vector3 spawnPos = new Vector3(randomX, waterObject.transform.position.y, randomZ);

        return spawnPos;
    }
}
