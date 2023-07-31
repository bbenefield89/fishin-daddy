using System.Collections;
using UnityEngine;

public class FishBehaviorController : MonoBehaviour
{
    public static FishBehaviorController Instance { get; private set; }

    [SerializeField] private GameObject bobberModel;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float swimDistance = 2.0f;
    [SerializeField] private float timeUntilNextSwim = 1.0f;
    [SerializeField] private float desiredDistFromTargetPos = 0.1f;

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

    [ContextMenu("Functions/BeginMovement")]
    public void BeginMovement()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + transform.forward * swimDistance;

        while (true)
        {
            yield return MoveRoutine(endPos);
            yield return MoveRoutine(startPos);
        }
    }

    private IEnumerator MoveRoutine(Vector3 targetPos)
    {
        transform.rotation = Quaternion.LookRotation(targetPos - transform.position);

        while (Vector3.Distance(transform.position, targetPos) > desiredDistFromTargetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(timeUntilNextSwim);
    }

    public void CheckFishNibble()
    {
        bool shouldFishNibble = RandomNumberGenerator.TruthyFalsyGenerator();
        if (shouldFishNibble)
        {
            CheckFishBite();
        }
        else
        {
        }
    }

    private void CheckFishBite()
    {
        //bool shouldFishBite = RandomNumberGenerator.TruthyFalsyGenerator();
        //if (shouldFishBite)
        //{
        //    HookFish();
        //    exclamations.SetActive(true);
        //    fishHookedAudio.Play();
        //}
        //else
        //{
        //}
    }
}
