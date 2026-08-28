using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRaterLimiter : MonoBehaviour
{
    private int fpsLimit = 60;
    void Start()
    {
        Application.targetFrameRate = fpsLimit;   
    }
}
