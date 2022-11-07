using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressMeter : MonoBehaviour
{
    [SerializeField] private Transform m_GoalTransform;
    [SerializeField] private Transform m_TargetTransform;
    [SerializeField] private RectTransform m_ProgressImageTransform;
    
    [HideInInspector] public float m_Progress;
    private float m_TotalDistance;
    private float m_progressStartPosX;

    private void Start()
    {
        m_TotalDistance = Vector3.Distance(m_GoalTransform.position, m_TargetTransform.position);
        m_progressStartPosX = m_ProgressImageTransform.localPosition.x;
        print(m_progressStartPosX);
    }

    private void FixedUpdate()
    {
        float dst = Vector3.Distance(m_GoalTransform.position, m_TargetTransform.position);

        var progressPosition = m_ProgressImageTransform.localPosition;
        progressPosition.x = dst / m_TotalDistance * m_progressStartPosX;
        m_ProgressImageTransform.localPosition = progressPosition;
    }
}
