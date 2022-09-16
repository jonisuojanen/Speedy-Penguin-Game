using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DEBUG : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmp;

    void Update()
    {
        tmp.text = "FPS: " + 1 / Time.deltaTime;
    }
}
