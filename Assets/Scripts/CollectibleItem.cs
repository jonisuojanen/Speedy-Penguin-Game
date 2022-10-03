using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private int m_amount;
    [SerializeField] private float m_rotateSpeed;
    private float m_rotationY;

    void Update()
    {
        m_rotationY = m_rotationY + m_rotateSpeed * Time.deltaTime;
        if (m_rotationY > 360f) m_rotationY -= 360f;
        transform.rotation = Quaternion.Euler(transform.rotation.x, m_rotationY, transform.rotation.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddScore(m_amount);
            print("collected");
            Destroy(gameObject);
        }
    }
}
