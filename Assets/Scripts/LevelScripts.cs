using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScripts : MonoBehaviour
{
    public GameObject deadPanel;

    private void Start()
    {
        GameManager.instance.levelScripts = this;
    }
    public void StartLevel()
    {
        GameManager.instance.SetGameActive(true);
    }

    public void RestartLevel()
    {
        GameManager.instance.ReloadScene();
    }

}
