using System.Collections;
using UnityEngine;

public class BobberController : MonoBehaviour
{
    #region Props
    public static BobberController Instance { get; private set; }

    public Transform pcModel;

    [Tooltip("GameObject that holds a reference to the position the bobber should be when not casted")]
    public Transform bobberReturnPosition;
    public GameObject exclamations;
    public AudioSource fishHookedAudio;
    public bool isFishHooked { get; private set; } = false;

    [Tooltip("How far down on the Y axis the bobber should drop after casting")]
    public float waterLevel = 0f;
    public float castDistance = 1f;
    public float bobberMovementSpeed = 5f;
    public float arcHeight = 1f;
    public float reelSpeed = 5f;
    public RandomNumberGenerator rng;

    private bool isCasting = false;
    private bool hasBeenCasted = false;
    private bool isBeingReeledIn = false;
    #endregion

    #region Awake
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    #region Update
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isCasting && !hasBeenCasted)
        {
            StartCoroutine(CastBobber());
        }
        else if (Input.GetMouseButton(1) && !isCasting && hasBeenCasted)
        {
            ReelBobberIn();
        }
        else
        {
            isBeingReeledIn = false;
        }
    }

    private IEnumerator CastBobber()
    {
        isCasting = true;
        hasBeenCasted = true;

        yield return MoveBobber();
        yield return DropBobberToWater();
        StartCoroutine(AttractFishToBobber());

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
        };
    }

    private IEnumerator AttractFishToBobber()
    {
        bool isFishInterested = false;

        while (hasBeenCasted && !isFishHooked && !isBeingReeledIn && !isFishInterested)
        {
            float nextBiteCheckIntervalRandom = rng.Generate();
            yield return new WaitForSeconds(nextBiteCheckIntervalRandom);

            isFishInterested = RandomNumberGenerator.TruthyFalsyGenerator();
            if (!isBeingReeledIn && isFishInterested)  // Check isBeingReeledIn again in case the player reels in the bobber while the coroutine is waiting
            {
                FishSpawner.Instance.Spawn();
            }
        }
    }

    public void HookFish()
    {
        Vector3 pos = transform.position;
        Vector3 newPos = new Vector3(pos.x, waterLevel - 1f, pos.z);
        transform.position = newPos;

        isFishHooked = true;
    }

    private void ReelBobberIn()
    {
        isBeingReeledIn = true;
        Vector3 targetPos = new Vector3(bobberReturnPosition.position.x, waterLevel, bobberReturnPosition.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, reelSpeed * Time.deltaTime);
        exclamations.SetActive(false);
    }
    #endregion

    #region OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.GROUND))
        {
            ReturnBobberToFishingPole();

            if (isFishHooked)
            {
                isFishHooked = false;
                FishCounterCanvas.Instance.UpdateFishCounterUI();
            }
        }
    }

    private void ReturnBobberToFishingPole()
    {
        hasBeenCasted = false;
        transform.position = bobberReturnPosition.position;
        transform.parent = bobberReturnPosition;
    }
    #endregion
}
