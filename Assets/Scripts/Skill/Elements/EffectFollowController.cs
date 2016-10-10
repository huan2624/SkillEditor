using UnityEngine;
using System.Collections;

public class EffectFollowController : EffectBehaviour
{
    private EffectPos m_TargetPos = new EffectPos();

    private Quaternion m_Rotation;

	private bool m_bSyncRotation = true;

    public bool SetFollowingParameter(EffectPos target_pos, bool bSyncRotation, Quaternion Rotation)
    {
        if (target_pos.m_uiTargetId == 0)
        {
            return false;
        }

        m_TargetPos.m_uiTargetId = target_pos.m_uiTargetId;
        m_TargetPos.m_emDummyPoint = target_pos.m_emDummyPoint;
        m_TargetPos.m_vOffset = target_pos.m_vOffset;
        m_TargetPos.m_OffsetDisance = target_pos.m_OffsetDisance;

        m_Rotation = Rotation;

        m_bSyncRotation = bSyncRotation;
        Update();

        return true;
    }

    void Update()
    {
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        if (EffectBehaviour.GetEffectPosRotaion(m_TargetPos, ref position, ref rotation))
        {
            transform.position = position;

            if (m_bSyncRotation)
            {
                transform.rotation = rotation * m_Rotation;
            }
         }
        else // target does not exist, destroy current effect
        {
            DestroyThisEffect();
        }
    }
}