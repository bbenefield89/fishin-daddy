using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMover : MonoBehaviour
{
    public static FishMover Instance { get; private set; }

    [SerializeField] private GameObject bobberModel;
    [SerializeField] private float moveSpeed = 1.0f;
    private float swimDistance = 2.0f;

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
            transform.rotation = Quaternion.LookRotation(endPos - transform.position);

            while (Vector3.Distance(transform.position, endPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            transform.rotation = Quaternion.LookRotation(startPos - transform.position);

            while (Vector3.Distance(transform.position, startPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
