using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


struct TimePoint
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Vector3 playerVelocity;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform m_GFX;
    [SerializeField]
    private Animator m_Animator;
    [SerializeField]
    private float m_RotationSpeed;
    [SerializeField]
    private float m_JumpForce;
    [SerializeField]
    private float m_SlidingSpeed, m_StandingSpeed;
    private float m_TargetSpeed;

    private float input;
    private Rigidbody m_RigidBody;
    private float m_MaxGroundRayLength = 0.8f;

    private bool m_IsSliding = true;
    private bool m_IsJumping = false;
    private bool m_IsGrounded = false;
    private bool m_CanJump = true;
    [SerializeField]
    private float m_coyoteAndJumpBufferTime;
    private float m_coyoteTimer = 0f;
    private float m_jumpBuffer = 0f;

    
    private bool m_IsBoosting;
    private float m_BoostTime;
    private float m_BoostSpeed;

    private bool m_IsSlowed;
    private float m_SlowedTime;
    private float m_SlowedSpeed;

    public GameObject m_scoreText;

    [SerializeField]
    private Image m_FadeImage;

    [SerializeField]
    private int m_Lives = 3;

    private Queue<TimePoint> m_SavePoints = new Queue<TimePoint>();
    private int m_QueueLimit = 7;
    private float m_LastSaveTime = 0;
    private bool m_ShouldFade = false;
    private float m_FadeTime = 0;
    private void Awake()
    {

    }

    void Start()
    {
        GameManager.instance.playerController = this;
        GameManager.instance.GetVariables();
        m_RigidBody = GetComponent<Rigidbody>();
        m_TargetSpeed = m_SlidingSpeed;
    }

    void Update()
    {
        Vector3 rotation = transform.position - m_RigidBody.velocity;
        m_GFX.LookAt(rotation, Vector3.up);

#if UNITY_STANDALONE || UNITY_EDITOR
        GatherInput();
#endif
#if UNITY_ANDROID || UNITY_IOS
        GatherInputMobile();
#endif
        UpdateTimers();
    }

    private void FixedUpdate()
    {
        SetSpeed();

        if (input != 0)
        {   
            m_RigidBody.velocity = 
            Quaternion.AngleAxis(input * m_RotationSpeed * Time.fixedDeltaTime, Vector3.up) * m_RigidBody.velocity;
        }
        if (m_IsJumping)
        {
            m_RigidBody.AddForce(Vector3.up * Mathf.Lerp(m_JumpForce, m_JumpForce*2,m_RigidBody.velocity.magnitude), ForceMode.Impulse);
            m_IsJumping = false;


            //PLAY JUMPING SOUND

        }
        if (m_IsGrounded) m_coyoteTimer = m_coyoteAndJumpBufferTime;

        TrackObject();

        FadePanel();
    }

    void TrackObject()
    {
        if (!m_IsGrounded)
            return;

        if(m_SavePoints.Count == 0)
        {
            TimePoint t = new TimePoint();
            t.playerPosition = transform.position;
            t.playerRotation = m_GFX.rotation;
            t.playerVelocity = m_RigidBody.velocity;
            m_LastSaveTime = Time.time;
            m_SavePoints.Enqueue(t);
            if (m_SavePoints.Count > m_QueueLimit)
            {
                m_SavePoints.Dequeue();
            }
            return;
        }

        if (Time.time - m_LastSaveTime >= 1.0f)
        {
            TimePoint tp = new TimePoint();
            Debug.Log("Count: " + m_SavePoints.Count);

            tp.playerPosition = transform.position;
            tp.playerRotation = m_GFX.rotation;
            tp.playerVelocity = m_RigidBody.velocity;
            m_SavePoints.Enqueue(tp);
            m_LastSaveTime = Time.time;
            if (m_SavePoints.Count > m_QueueLimit)
            {
                m_SavePoints.Dequeue();
            }
            return;
        }
    }

    void FadePanel()
    {
        if(m_ShouldFade)
        {
            m_FadeTime += Time.deltaTime;
            Color col = new Color(0,0,0,0);
            col.a = Mathf.Clamp(Mathf.Sin(m_FadeTime * Mathf.PI)*2,0,1);
            m_FadeImage.color = col;
        }
        else
        {
            m_FadeImage.color = new Color(0, 0, 0, 0);
            m_FadeTime = 0;
        }
    }

    void Respawn()
    {
        TimePoint tp = m_SavePoints.Dequeue();

        transform.position = tp.playerPosition;
        m_GFX.rotation = tp.playerRotation;
        m_RigidBody.velocity = tp.playerVelocity;
    }

    private void GatherInput()
    {
        input = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space)) // && Physics.Raycast(transform.position, Vector3.down, m_MaxGroundRayLength))
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.red, m_MaxGroundRayLength);
            m_IsJumping = true;
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Gameplay/Jump", gameObject);
        }

        if (!m_IsGrounded)
            return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            m_IsSliding = !m_IsSliding;
            m_TargetSpeed = (m_IsSliding) ? m_SlidingSpeed : m_StandingSpeed;
            print("speed up: " + m_IsSliding);
            m_Animator.SetBool("BackSlide", !m_IsSliding);
        }
    }

    public void KillPlayer(bool isGoal)
    {
        if (isGoal)
        {
            GameManager.instance.DemoFinished();
            return;
        }

        if(m_Lives > 0)
        {
            StartCoroutine(FadeAndRespawnEvent());
            m_Lives--;
        }
        else
        {
            GameManager.instance.levelScripts.deadPanel.SetActive(true);
            GameManager.instance.SetGameActive(false);
        }
    }


    private void GatherInputMobile()
    {
        Gamepad pad = Gamepad.current;
        if (pad == null)
        {
            Debug.LogError("No Gamepad detected!");
            return;
        }

        input = pad.rightStick.ReadValue().x;

        //Jump buffer
        if (pad.rightTrigger.wasPressedThisFrame && !m_IsJumping) m_jumpBuffer = m_coyoteAndJumpBufferTime;

        //Queue jump to FixedUpdate
        if (m_jumpBuffer > 0f && m_coyoteTimer > 0f && m_CanJump) //(m_IsGrounded && pad.rightTrigger.wasPressedThisFrame) // && Physics.Raycast(transform.position, Vector3.down, m_MaxGroundRayLength))
        {
            StartCoroutine(JumpReset());
            Debug.DrawRay(transform.position, Vector3.down, Color.red, m_MaxGroundRayLength);
            m_jumpBuffer = 0f;
            m_coyoteTimer = 0f;
            m_IsJumping = true;
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Gameplay/Jump", gameObject);
        }

        //Slide toggle
        if (pad.leftTrigger.isPressed && m_IsSliding && m_IsGrounded)
        {
            m_IsSliding = false;
            m_TargetSpeed = m_StandingSpeed;
            print("belly to back");
            m_Animator.SetBool("BackSlide", true);
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Gameplay/BellyToBack", gameObject);

            ////SFX TRIGGERS HERE
            //if (m_IsSliding)
            //{
            //    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Gameplay/BackToBelly", gameObject);
            //}
            //else
            //{
            //    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Gameplay/BellyToBack", gameObject);
            //}
        }

        if (pad.leftTrigger.wasReleasedThisFrame && !m_IsSliding)
        {
            m_IsSliding = true;
            m_TargetSpeed = m_SlidingSpeed;
            print("back to belly");
            m_Animator.SetBool("BackSlide", false);
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Gameplay/BackToBelly", gameObject);
        }
    }

    private void SetSpeed()
    {
        Vector3 targetVelocityVector = m_RigidBody.velocity.normalized;
        Vector3 currentVelocityVector = m_RigidBody.velocity;
        float currentSpeed = m_RigidBody.velocity.magnitude;
        float interpRate = 1f;

        if (m_IsSlowed)
        {
            targetVelocityVector *= m_SlowedSpeed;
            interpRate = 5f;
        }
        else if (m_IsBoosting)
        {
            targetVelocityVector *= m_BoostSpeed;
            interpRate = 5f;
        }
        else if (!m_IsBoosting)
        {
            if (m_IsSliding && currentSpeed <= m_TargetSpeed)
            {
                targetVelocityVector *= m_TargetSpeed;
                interpRate = 1f;
            }
            else if (!m_IsSliding && currentSpeed != m_TargetSpeed)
            {
                targetVelocityVector *= m_TargetSpeed;
                interpRate = 2f;
            }
        }

        m_RigidBody.velocity = Vector3.Lerp(currentVelocityVector, targetVelocityVector, interpRate * Time.deltaTime);
    }

    private void UpdateTimers()
    {
        if (m_IsBoosting)
        {
            if (m_BoostTime > 0f) m_BoostTime -= Time.deltaTime;
            if (m_BoostTime <= 0f) m_IsBoosting = false;
        }
        if (m_IsSlowed)
        {
            if (m_SlowedTime > 0f) m_SlowedTime -= Time.deltaTime;
            if (m_SlowedTime <= 0f) m_IsSlowed = false;
        }
        if (m_coyoteTimer > 0f) m_coyoteTimer -= Time.deltaTime;
        if (m_jumpBuffer > 0f) m_jumpBuffer -= Time.deltaTime;

    }

    private IEnumerator JumpReset()
    {
        m_CanJump = false;
        yield return new WaitForSeconds(m_coyoteAndJumpBufferTime * 2);
        m_CanJump = true;
    }

    public void ActivateBoost(float time, float speed)
    {
        m_BoostTime = time;
        m_BoostSpeed = m_RigidBody.velocity.magnitude + speed;
        m_IsBoosting = true;
    }

    public void ActivateSlow(float time, float speed)
    {
        m_SlowedTime = time;
        m_SlowedSpeed = speed;
        m_IsSlowed = true;
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

    private IEnumerator FadeAndRespawnEvent()
    {
        m_ShouldFade = true;
        yield return new WaitForSecondsRealtime(0.2f);
        Respawn();
        yield return new WaitForSecondsRealtime(0.8f);
        m_ShouldFade = false;
    }

}

