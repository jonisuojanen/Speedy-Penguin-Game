using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_slideParticles, m_landingParticles;

    private const float s_landingParticlePlayImpulseY = 1000f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_slideParticles.Play();

            if (collision.impulse.y > s_landingParticlePlayImpulseY)
            {
                StartCoroutine(LandingParticlesPlay(collision.contacts[0].normal));
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_slideParticles.Pause();

            F_AudioManager.instance.StopSlideSFX();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            if(!F_AudioManager.instance.isPlaying())
            {
                F_AudioManager.instance.StartSlideSFX();
            }
        }
    }

    private IEnumerator LandingParticlesPlay(Vector3 normal)
    {
        yield return new WaitForFixedUpdate();
        m_landingParticles.transform.rotation = Quaternion.LookRotation(normal, normal);
        m_landingParticles.Play();

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Gameplay/Landing", gameObject);
    }

}
