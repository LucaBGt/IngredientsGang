using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSHelper : MonoBehaviour
{
    public int target = 60;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }
}
