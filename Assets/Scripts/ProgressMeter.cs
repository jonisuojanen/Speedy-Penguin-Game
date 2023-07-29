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

    private void Awake()
    {
        if (m_GoalTransform == null || m_TargetTransform == null || m_ProgressImageTransform == null)
        {
            Debug.LogWarning("Progress meter variables aren't set for this scene!");
        }
    }

    private void Start()
    {
        if (m_GoalTransform == null || m_TargetTransform == null || m_ProgressImageTransform == null)
            return;

        m_TotalDistance = Vector3.Distance(m_GoalTransform.position, m_TargetTransform.position);
        m_progressStartPosX = m_ProgressImageTransform.localPosition.x;
    }

    private void FixedUpdate()
    {
        if (m_GoalTransform == null || m_TargetTransform == null || m_ProgressImageTransform == null)
            return;

        float dst = Vector3.Distance(m_GoalTransform.position, m_TargetTransform.position);

        var progressPosition = m_ProgressImageTransform.localPosition;
        progressPosition.x = dst / m_TotalDistance * m_progressStartPosX;
        m_ProgressImageTransform.localPosition = progressPosition;
    }
}
