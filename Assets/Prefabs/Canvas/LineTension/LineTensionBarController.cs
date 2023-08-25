using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineTensionBarController : MonoBehaviour
{
    public static LineTensionBarController Instance { get; private set; }

    private Slider _lineTensionBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _lineTensionBar = GetComponent<Slider>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLineTension(float lineTension)
    {
        _lineTensionBar.value = lineTension;
    }
}
