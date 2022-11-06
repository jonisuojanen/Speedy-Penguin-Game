using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField]
    private bool m_IsGoal = false;
 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        if(m_IsGoal)
        {
            GameManager.instance.levelScripts.winPanel.SetActive(true);
            GameManager.instance.SetGameActive(false);
            return;
        }
        GameManager.instance.levelScripts.deadPanel.SetActive(true);
        GameManager.instance.SetGameActive(false);
    }
}
