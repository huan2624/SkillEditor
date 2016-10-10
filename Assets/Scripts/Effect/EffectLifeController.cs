using UnityEngine;
using System.Collections;


public class EffectLifeController : EffectBehaviour
{
    private float m_fLiveTime = 0.0f;
    private float m_fStartTime = 0.0f;

    public void SetLiveTime(float fDuration)
    {
        m_fLiveTime = fDuration;
    }

    void Start()
    {
        m_fStartTime = Time.time;
    }

    void Update()
    {
        if (m_fStartTime > 0.0f && Time.time >= m_fStartTime + m_fLiveTime)
        {
            DestroyThisEffect();
            m_fStartTime = 0.0f;
        }
    }
}