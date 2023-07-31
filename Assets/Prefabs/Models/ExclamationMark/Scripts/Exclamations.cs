using System.Collections;
using UnityEngine;

public class Exclamations : MonoBehaviour
{
    private void OnEnable()
    {
        foreach (Transform exclamationMark in transform)
        {
            ExclamationMark exclamationMarkScript = exclamationMark.GetComponent<ExclamationMark>();
            StartCoroutine(exclamationMarkScript.Animate(0));
        }
    }
}
