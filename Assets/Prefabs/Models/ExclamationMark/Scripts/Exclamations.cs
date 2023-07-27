using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exclamations : MonoBehaviour
{
    void Start()
    {
        Transform player = GameObject.Find(Prefabs.PC).transform;
        Vector3 targetPos = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(targetPos);
        transform.rotation = rotation;
    }
}
