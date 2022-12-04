using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField]
    private bool m_IsGoal = false;

    private PlayerController m_PlayerControllerInstance;

    private void Start()
    {
        m_PlayerControllerInstance = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_PlayerControllerInstance.KillPlayer(m_IsGoal);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_PlayerControllerInstance.KillPlayer(m_IsGoal);
        }
    }


}
