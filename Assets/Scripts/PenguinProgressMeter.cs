using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenguinProgressMeter : MonoBehaviour
{
    [SerializeField] private Transform m_GoalTransform;
    [SerializeField] private Transform m_TargetTransform;
    [SerializeField] private RectTransform m_ProgressImageTransform;

    [HideInInspector] public float m_Progress;
    private float m_TotalDistance;


    private void Start()
    {
        m_TotalDistance = Vector3.Distance(m_GoalTransform.position, m_TargetTransform.position);
    }



    private void Update()
    {
        float dst = Vector3.Distance(m_TargetTransform.position, m_TargetTransform.position);

        m_Progress = 1 - ((m_TotalDistance - dst) / m_TotalDistance);
        m_ProgressImageTransform.localScale = new Vector3(m_Progress, 1, 1);
    }
}
