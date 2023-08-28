using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns the Fish Counter Canvas
/// This is a workaround for some weird issues with the canvas not
/// appear in the correct place in the windows build
/// </summary>
public class FishCounterCanvasSpawner : MonoBehaviour
{
    public GameObject fishCounterCanvas;
    
    void Start()
    {
        GameObject canvas = Instantiate(fishCounterCanvas);
        canvas.name = Prefabs.FISH_COUNTER_CANVAS;
    }
}
