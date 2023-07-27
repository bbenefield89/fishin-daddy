using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationMark : MonoBehaviour
{
    public float moveDistance = 0.5f;
    public float moveSpeed = 5f;
    public float timeBetweenAnimations = 0.1f;

    public IEnumerator Animate(float startDelay)
    {
        while (true)
        {
            yield return new WaitForSeconds(startDelay);

            Vector3 targetPos = transform.localPosition + new Vector3(0, moveDistance, 0);
            yield return Move(targetPos);

            yield return new WaitForSeconds(timeBetweenAnimations);

            targetPos = transform.localPosition - new Vector3(0, moveDistance, 0);
            yield return Move(targetPos);
        }
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        while (transform.localPosition != targetPos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
