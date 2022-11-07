using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikeHz : MonoBehaviour
{
    [SerializeField] private float m_ActivationDelay = 1f;
    [SerializeField] private float m_DamageRadius = 2f;
    [SerializeField] private float m_SlowTime = 1f;
    [SerializeField] private float m_SlowedSpeed = 10f;

    [SerializeField] private BoxCollider m_ActivationCollider;
    [SerializeField] private BoxCollider m_HitboxCollider;
    [SerializeField] private Transform hitScanPos;

    private bool m_Active;
    private Rigidbody m_Rb;

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!m_Active) StartCoroutine(ActivateSpike());
            else CheckForCollision(hitScanPos.position);
        }
        else if (m_Active && other.CompareTag("Ground"))
        {
            CheckForCollision(hitScanPos.position);
        }
    }

    IEnumerator ActivateSpike()
    {
        //Run some animation
        m_ActivationCollider.enabled = false;
        yield return new WaitForSeconds(m_ActivationDelay);
        m_ActivationCollider.enabled = false;
        m_HitboxCollider.enabled = true;
        m_Rb.isKinematic = false;
        m_Active = true;
    }

    void CheckForCollision(Vector3 collisionPos)
    {
        Collider[] collision = Physics.OverlapSphere(collisionPos, m_DamageRadius);
        foreach (Collider col in collision)
        {
            if (col.CompareTag("Player")) 
            {
                GameManager.instance.playerController.ActivateSlow(m_SlowTime, m_SlowedSpeed);
                print("hit & slowed player!");
            }
        }

        Destroy(gameObject);
    }
}
