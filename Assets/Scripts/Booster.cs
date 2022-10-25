using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private float m_BoostTime;
    [SerializeField] private float m_BoostAmount;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerController.ActivateBoost(m_BoostTime, m_BoostAmount);
            print("activated boost!");

            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Gameplay/SpeedBoost", gameObject);
        }
    }
}
