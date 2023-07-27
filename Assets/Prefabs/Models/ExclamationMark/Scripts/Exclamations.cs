using System.Collections;
using UnityEngine;

public class Exclamations : MonoBehaviour
{
    void Start()
    {
        Transform player = GameObject.Find(Prefabs.PC).transform;
        Vector3 targetPos = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(targetPos);
        transform.rotation = rotation;

        foreach (Transform t in transform)
        {
            ExclamationMark exclamationMarkScript = t.GetComponent<ExclamationMark>();
            StartCoroutine(exclamationMarkScript.Animate(0));
        }
    }
}
