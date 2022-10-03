using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private float refreshRate;

    private float timer = 1f;
    private void Update()
    {
        timer -= Time.unscaledDeltaTime;
        if (timer <= 0f)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = fps.ToString();
            timer = refreshRate;
        }
    }
}
