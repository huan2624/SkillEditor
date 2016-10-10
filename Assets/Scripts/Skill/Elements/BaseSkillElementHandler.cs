using UnityEngine;
using System.Collections;

public class BaseSkillElementHandler {

    // Data Members
    protected CastSkillInfo m_CurSkillInfo = null;

    public uint UniqueId
    {
        get { return m_CurSkillInfo != null ? m_CurSkillInfo.UniqueId : 0; }
    }

    private float m_fTimeScale = 1.0f;  //时间缩放
    public float StartupTime { get; private set; }
    public float AnimationEclipsedTime { get; private set; }// 技能动作时间 受技能动作播放速度影响
    public float ElemEclipsedTime { get; private set; }// 元素更新时间 不受技能动作变化影响

    private float m_fPhaseTotalTime = 0.0f;//动作动画时长

    public void SetupCurSkillInfo(CastSkillInfo cur_skill_info, float fPhaseTotalTime)
    {
        m_CurSkillInfo = cur_skill_info;
        m_fPhaseTotalTime = fPhaseTotalTime;
    }

    public virtual bool Startup(SkillDispEvent evt)
    {
        StartupTime = Time.time;
        return true;
    }

    public virtual void Terminate(SkillDispEvent evt)
    {
        StartupTime = 0.0f;
        m_fTimeScale = 1.0f;
        AnimationEclipsedTime = 0.0f;
        ElemEclipsedTime = 0.0f;
    }

    public virtual bool Update(float fDeltaTime)
    {
        float Scale = 1.0f;
        if (m_CurSkillInfo != null)
        {
            Scale = m_CurSkillInfo.TimeScale;
        }

        if (StartupTime > 0.0f)
        {
            AnimationEclipsedTime += fDeltaTime * Scale;
            ElemEclipsedTime += fDeltaTime;
        }

        return true;
    }

    protected void TriggerOutputEvent(SkillDispEvent evt)
    {
        m_CurSkillInfo.DispEventManager.TriggerEvent(evt);
    }

    protected bool ExecuteHitTest()
    {
        if (m_CurSkillInfo.SelectTargetId > 0)
        {
            m_CurSkillInfo.HitTargets.Clear();
            m_CurSkillInfo.HitTargets.Add(m_CurSkillInfo.SelectTargetId);
            return true;
        }
        return true;
    }

    protected bool ExecuteShootTest(ShootTestParam shoot_test_param)
    {
        if (m_CurSkillInfo == null || shoot_test_param == null)
        {
            return false;
        }

        BaseActor SkillCaster = m_CurSkillInfo.Caster;
        if (null == SkillCaster)
        {
            return false;
        }

        Vector3 position;
        Vector3 forward;

        BaseActor ActorTester = null;
        switch (shoot_test_param.m_ShootTestBase)
        {
            case ShootTestBase.BASE_CASTER:
                {
                    ActorTester = SkillCaster;
                    position = ActorTester.gameObject.transform.position;
                    forward = ActorTester.gameObject.transform.forward;
                }
                break;

            case ShootTestBase.BASE_TARGET:
                {
                    if (m_CurSkillInfo.SkillTargets.Count > 0)
                    {
                        // Modify By Sword [2014_10_17] 检测目标不应该以第一个受技能伤害的人为检测目标 而应该以选中的目标为第一目标
                        ActorTester = ActorManager.Instance.GetActor(m_CurSkillInfo.SelectTargetId);
                    }

                    if (ActorTester != null)
                    {
                        if (m_CurSkillInfo.SkillTargetInfo == null)
                        {
                            m_CurSkillInfo.SkillTargetInfo = new SkillTargetInfo();
                        }

                        position = ActorTester.transform.position;
                        forward = SkillCaster.transform.position - ActorTester.transform.position;

                        m_CurSkillInfo.SkillTargetInfo.m_vPosition = position;
                        m_CurSkillInfo.SkillTargetInfo.m_vForward = forward;
                    }
                    else
                    {
                        if (m_CurSkillInfo.SkillTargetInfo == null)
                        {
                            return false;
                        }

                        position = m_CurSkillInfo.SkillTargetInfo.m_vPosition;
                        forward = m_CurSkillInfo.SkillTargetInfo.m_vForward;
                    }
                }
                break;

            default:
                return false;
        }

        return ExecuteShootTest(shoot_test_param, ActorTester, position, forward);
    }

    protected bool ExecuteShootTest(ShootTestParam shoot_test_param, BaseActor ActorTester, Vector3 position, Vector3 forward)
    {
        if (m_CurSkillInfo == null || shoot_test_param == null)
        {
            return false;
        }

        BaseActor SkillCaster = m_CurSkillInfo.Caster;
        if (null == SkillCaster)
        {
            return false;
        }

        if (ActorTester == null)
        {
            switch (shoot_test_param.m_ShootTestBase)
            {
                case ShootTestBase.BASE_CASTER:
                    {
                        ActorTester = SkillCaster;
                    }
                    break;

                case ShootTestBase.BASE_TARGET:
                    {
                        if (m_CurSkillInfo.SkillTargets.Count > 0)
                        {
                            ActorTester = ActorManager.Instance.GetActor(m_CurSkillInfo.SkillTargets[0]);
                        }
                    }
                    break;

                default:
                    return false;
            }
        }

        wl_res.SKILL_TARGET_TYPE SkillTargetType = wl_res.SKILL_TARGET_TYPE.E_STT_ENEMY;

        //wl_res.SkillDataInfo refSkillDataInfo = refSkillSys.GetSkillConfigData(m_CurSkillInfo.m_uiSkillId);
        wl_res.SkillDataInfo refSkillDataInfo = null;
        if (refSkillDataInfo != null)
        {
            SkillTargetType = (wl_res.SKILL_TARGET_TYPE)refSkillDataInfo.SkillTarget;
        }

        ImapctTargetType eImapctTargetType = SkillTargetType == wl_res.SKILL_TARGET_TYPE.E_STT_ENEMY || refSkillDataInfo.SkillTarget == 0
                                            ? ImapctTargetType.IMPACT_TYPE_ENEMY : ImapctTargetType.IMPACT_TYPE_ALLY;

        if (SkillManager.Instance.ExecuteShootTest(ref m_CurSkillInfo.HitTargets,
                        SkillCaster,
                        ActorTester,
                        position,
                        Quaternion.Euler(0, shoot_test_param.m_Range.m_ForwardDelta, 0) * forward,
                        shoot_test_param.m_Range,
                        ref m_CurSkillInfo.ImpactInfoDic,
                        shoot_test_param.m_ImpactInterval,
                        shoot_test_param.m_MaxImpactTimes,
                        shoot_test_param.m_MaxShootCount,
                        shoot_test_param.m_ShootTestType,
                        eImapctTargetType)
                        )
        {
            return true;
        }

        return false;
    }

    //击中目标
    protected void OnHitTargets(SkillImpactData skill_impact_data)
    {
        if (m_CurSkillInfo.HitTargets.Count <= 0)
        {
            
            return;
        }

        //触发命中目标时
        TriggerOutputEvent(new SkillDispEvent(SkillDispEventType.DISPEVT_ON_SHOOT_TARGET));

        ImpactInfo impact_info;

        var it = m_CurSkillInfo.HitTargets.GetEnumerator();
        //遍历攻击目标
        while (it.MoveNext())
        {
            uint target_id = (uint)it.Current;
            if (m_CurSkillInfo.ImpactInfoDic.TryGetValue(target_id, out impact_info))
            {
                ++impact_info.m_nImpactTimes;
                impact_info.m_fLatestImpactTime = Time.time;
            }
            else
            {
                impact_info = new ImpactInfo();
                impact_info.m_nImpactTimes = 1;
                impact_info.m_fLatestImpactTime = Time.time;

                m_CurSkillInfo.ImpactInfoDic[target_id] = impact_info;
            }

            // Notify impact info
            SkillManager.Instance.ImpactTarget(m_CurSkillInfo, target_id, skill_impact_data);
        }

        m_CurSkillInfo.HitTargets.Clear();
    }

    #region event regist
    protected void RegisterEventHandler(SkillDispEvent startup_event, SkillDispEvent terminate_event)
    {
        m_CurSkillInfo.DispEventManager.RegisterEventHandler(FormatEvent(startup_event), FormatEvent(terminate_event), this);
    }

    /// <summary>
    /// 如果是定时触发就根据比例计算出真实触发时间
    /// </summary>
    /// <param name="evt"></param>
    /// <returns></returns>
    protected virtual SkillDispEvent FormatEvent(SkillDispEvent evt)
    {
        SkillDispEvent fmt_evt;
        switch (evt.m_EventType)
        {
            case SkillDispEventType.DISPEVT_TIMEPOINT_TRIGGER:
                {
                    if (evt.m_Param1 == 0.0f)
                    {
                        fmt_evt = evt;
                    }
                    else
                    {
                        float fRealTime = m_fPhaseTotalTime * evt.m_Param1;
#if !UNITY_EDITOR
                        //if (fRealTime > TriggerPointAdvancedTime)
                        //    fRealTime -= TriggerPointAdvancedTime;
                        //else
                        //    fRealTime = 0.0f;
#endif
                        fmt_evt = new SkillDispEvent(evt.m_EventType, fRealTime, evt.m_Param2, evt.m_Param3);
                    }
                }
                break;
            default:
                {
                    fmt_evt = evt;
                }
                break;
        }

        return fmt_evt;
    }
    #endregion

    public static string GetEffectPath(string name)
    {
        string strPath = "";
        strPath += CommHelper.GetPureFileNameWithouExt(name);
        return strPath;
    }

    public virtual bool IsAlwaysActive()
    {
        return false;
    }
}
