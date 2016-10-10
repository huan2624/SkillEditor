using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectElementHandler : BaseSkillElementHandler
{
    EffectElement m_EffectElement;

    List<GameObject> m_lstEffects = new List<GameObject>();

    public bool Setup(EffectElement effect_element)
    {
        if (effect_element == null || effect_element.m_StartupEvent == null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(effect_element.m_EffectInfo.m_Name))
        {
            return false;
        }

        m_EffectElement = effect_element;

        RegisterEventHandler(m_EffectElement.m_StartupEvent, m_EffectElement.m_TerminateEvent);

        return true;
    }

    public override bool Startup(SkillDispEvent evt)
    {
        switch (m_EffectElement.m_EffectInfo.m_Mode)
        {
            case EffectMode.EFT_ON_WORLD_POS:
                if (evt != null && evt.m_EventType == SkillDispEventType.DISPEVT_PROGRAM_CUSTOM)
                {
                    CustomSkillDispEvent custom_evt = evt as CustomSkillDispEvent;
                    if (custom_evt != null)
                    {
                        CreateWorldPosEffect(custom_evt.m_Position, ref m_EffectElement.m_EffectInfo);
                    }
                }
                break;

            case EffectMode.EFT_ON_CASTER_POS:
                CreateCasterPosEffect(ref m_EffectElement.m_EffectInfo);
                break;

            case EffectMode.EFT_FOLLOW_CASTER:
                //CreateFollowCasterEffect(ref m_EffectElement.m_EffectInfo);
                break;

            case EffectMode.EFT_ON_TARGET_POS:
                CreateTargetPosEffect(ref m_EffectElement.m_EffectInfo, ref m_CurSkillInfo.HitTargets);
                break;

            case EffectMode.EFT_FOLLOW_TARGET:
                //CreateFollowTargetEffect(ref m_EffectElement.m_EffectInfo, ref m_CurSkillInfo.m_lstShootTargets);
                break;

            case EffectMode.EFT_ON_SKILLTARGET_POS:
                {
                    List<uint> CurTarget = new List<uint>(1);
                    CurTarget.Add(m_CurSkillInfo.SelectTargetId);
                    //CreateTargetPosEffect(ref m_EffectElement.m_EffectInfo, ref CurTarget);
                }
                break;

            case EffectMode.EFT_FOLLOW_SKILLTARGET:
                //CreateFollowTargetEffect(ref m_EffectElement.m_EffectInfo, ref m_CurSkillInfo.m_lstSkillTargets);
                break;

            default:
                break;
        }

        return true;
    }

    public override void Terminate(SkillDispEvent evt)
    {
        foreach (GameObject goEffect in m_lstEffects)
        {
            EffectManager.Instance.DestroyEffect(goEffect);
        }
        m_lstEffects.Clear();
        m_lstEffects = null;
    }

    bool CreateWorldPosEffect(Vector3 world_pos, ref EffectInfo effect_info)
    {
        GameObject goEffect = EffectManager.Instance.CreateWorldPosEffect(GetEffectPath(effect_info.m_Name),
            world_pos, Vector3.zero, effect_info.m_Scale, effect_info.m_Duration, Quaternion.identity);
        if (goEffect != null)
        {
            m_lstEffects.Add(goEffect);
            return true;
        }
        return false;
    }

    bool CreateCasterPosEffect(ref EffectInfo effect_info)
    {
        BaseActor Caster = m_CurSkillInfo.Caster;
        if (Caster == null)
        {
            return false;
        }

        Vector3 pos = GetEffectPos(Caster, ref effect_info);
        Vector3 dir = GetEffectDir(effect_info.m_Dir, Caster.transform, Caster.transform);
        Quaternion rotate;
        if (effect_info.m_Dir == EffectDir.EFT_DIR_ORIGINAL)
        {
            //Vector3 v = pos - Caster.transform.position;
            //Debuger.Log("EffectPos: " + v.ToString());

            Transform dmpos = EffectBehaviour.GetDummyPointTransform(Caster, effect_info.m_Pos);
            if (dmpos != null)
            {
                rotate = dmpos.transform.rotation * effect_info.Rotation;
            }
            else
            {
                rotate = Quaternion.identity;
            }
        }
        else
        {
            dir = effect_info.ReviseVector(dir);
            rotate = Quaternion.identity;
        }

        GameObject goEffect = EffectManager.Instance.CreateWorldPosEffect(GetEffectPath(effect_info.m_Name),
            pos, dir, effect_info.m_Scale, effect_info.m_Duration, rotate);
        if (goEffect != null)
        {
            m_lstEffects.Add(goEffect);
            return true;
        }
        return false;
    }

    bool CreateTargetPosEffect(ref EffectInfo effect_info, ref List<uint> lstShootTargets)
    {
        bool bSucceed = false;

        IEnumerator<uint> it = (IEnumerator<uint>)lstShootTargets.GetEnumerator();
        while (it.MoveNext())
        {
            uint target_id = it.Current;
            BaseActor ActorTarget = ActorManager.Instance.GetActor(target_id);
            if (ActorTarget)
            {
                Vector3 pos = GetEffectPos(ActorTarget, ref effect_info);
                Vector3 dir = GetEffectDir(effect_info.m_Dir, ActorTarget.gameObject.transform, m_CurSkillInfo.Caster ? m_CurSkillInfo.Caster.transform : null);
                Quaternion rotate;
                if (effect_info.m_Dir == EffectDir.EFT_DIR_ORIGINAL)
                {
                    Transform dmpos = EffectBehaviour.GetDummyPointTransform(ActorTarget, effect_info.m_Pos);
                    if (dmpos != null)
                    {
                        rotate = effect_info.Rotation * dmpos.transform.rotation;
                    }
                    else
                    {
                        rotate = Quaternion.identity;
                    }
                }
                else
                {
                    dir = effect_info.ReviseVector(dir);
                    rotate = Quaternion.identity;
                }
                GameObject goEffect = EffectManager.Instance.CreateWorldPosEffect(GetEffectPath(effect_info.m_Name),
                    pos, dir, effect_info.m_Scale, effect_info.m_Duration, rotate);
                if (goEffect != null)
                {
                    m_lstEffects.Add(goEffect);
                    bSucceed = true;
                }
            }
        }
        return bSucceed;
    }

    Vector3 GetEffectPos(BaseActor Actor, ref EffectInfo effect_info)
    {
        Transform trans = EffectBehaviour.GetDummyPointTransform(Actor, effect_info.m_Pos);
        return EffectBehaviour.GetEffectPos(trans, effect_info.Offset, effect_info.m_OffsetDistance);
    }

    Vector3 GetEffectDir(EffectDir Dir, Transform TargetTrans, Transform CasterTrans)
    {
        Vector3 dir = Vector3.zero;
        switch (Dir)
        {
            case EffectDir.EFT_DIR_BINDING_TARGET:
                {
                    if (TargetTrans != null)
                    {
                        dir = TargetTrans.forward;
                    }
                }
                break;

            case EffectDir.EFT_DIR_INV_BINDING_TARGET:
                {
                    if (TargetTrans != null)
                    {
                        dir = -TargetTrans.forward;
                    }
                }
                break;

            case EffectDir.EFT_DIR_SKILL_CASTER:
                {
                    if (null != CasterTrans)
                    {
                        dir = CasterTrans.forward;
                    }
                }
                break;

            case EffectDir.EFT_DIR_CASTER_TO_TARGET:
                {
                    if (null != CasterTrans)
                    {
                        if (m_CurSkillInfo.m_SkillParam != null)
                        {
                            dir = (m_CurSkillInfo.m_SkillParam.m_ApproachPos - CasterTrans.position).normalized;
                        }
                        else if (TargetTrans != null)
                        {
                            dir = (TargetTrans.position - CasterTrans.position).normalized;
                        }
                    }
                }
                break;

            case EffectDir.EFT_DIR_TARGET_TO_CASTER:
                {
                    if (null != CasterTrans)
                    {
                        if (m_CurSkillInfo.m_SkillParam != null)
                        {
                            dir = (CasterTrans.position - m_CurSkillInfo.m_SkillParam.m_ApproachPos).normalized;
                        }
                        else if (TargetTrans != null)
                        {
                            dir = (CasterTrans.position - TargetTrans.position).normalized;
                        }
                    }
                }
                break;

            default:
                break;
        }

        return dir;
    }
}
