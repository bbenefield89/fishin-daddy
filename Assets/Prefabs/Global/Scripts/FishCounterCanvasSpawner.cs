using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCounterCanvasSpawner : MonoBehaviour
{
    public GameObject fishCounterCanvas;
    
    void Start()
    {
        GameObject canvas = Instantiate(fishCounterCanvas);
        canvas.name = Prefabs.FISH_COUNTER_CANVAS;
    }
}
