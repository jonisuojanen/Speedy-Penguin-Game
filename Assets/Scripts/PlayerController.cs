using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform m_GFX;
    [SerializeField]
    private float m_RotationSpeed;
    [SerializeField]
    private float m_JumpForce;

    private float input;
    private Rigidbody m_RigidBody;
    private float m_MaxGroundRayLength = 0.8f;

    private bool m_IsSliding = true;
    private bool m_IsJumping = false;
    private bool m_IsGrounded = false;

    /*
    * -> Rotate graphics object by input
    * -> Rotate rigidbody velocity vector by input
    * -> Jumping when grounded  
    */

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 rotation = transform.position + m_RigidBody.velocity;
        m_GFX.LookAt(new Vector3(rotation.x, transform.position.y, rotation.z), Vector3.up);

        GatherInput();
    }

    private void GatherInput()
    {
        input = Input.GetAxisRaw("Horizontal");
        if (m_IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, Vector3.down, m_MaxGroundRayLength))
            {
                Debug.DrawRay(transform.position, Vector3.down, Color.red, m_MaxGroundRayLength);
                m_IsJumping = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_IsSliding = !m_IsSliding;
            //TODO: brake toggle
        }
    }

    private void FixedUpdate()
    {
        if (input != 0)
        {   
            m_RigidBody.velocity = 
            Quaternion.AngleAxis(input * m_RotationSpeed * Time.fixedDeltaTime, Vector3.up) * m_RigidBody.velocity;
        }
        if (m_IsJumping)
        {
            m_RigidBody.AddForce(Vector3.up * Mathf.Lerp(m_JumpForce, m_JumpForce*2,m_RigidBody.velocity.magnitude), ForceMode.Impulse);
            m_IsJumping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_IsGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_IsGrounded = false;
        }
    }
}