using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsUnclock : MonoBehaviour
{
    [SerializeField] private bool fps60;

    private void Start()
    {
        if (fps60)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}
