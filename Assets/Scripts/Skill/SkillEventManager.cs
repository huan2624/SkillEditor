/*------------------------------------------------------------------------------
* 技能事件管理类
*------------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillEventManager
{
    //技能事件数据
    public class DispEventInfo
    {
        public SkillDispEvent startup_event;        //开始事件
        public SkillDispEvent terminate_event;      //结束事件  
        public BaseSkillElementHandler sde_handler; //技能元素基类
        public bool bStartup = false;   //是否执行过Startup
        public float fRegisterTime = 0.0f;  // 注册时的系统时间
        public float fPastTime = 0.0f;		// 注册后经过的时间 这个时间当动作变快的时候 每帧增幅也变大
    }

    CastSkillInfo m_refCurSkillInfo;

    List<DispEventInfo> m_lstDispEventInfos = new List<DispEventInfo>();
    float m_fLastUpdateTime = 0.0f;

    public SkillEventManager(CastSkillInfo refCurSkillInfo)
    {
        m_refCurSkillInfo = refCurSkillInfo;
    }

    //注册事件
    public bool RegisterEventHandler(SkillDispEvent startup_event, SkillDispEvent terminate_event, BaseSkillElementHandler sde_handler)
    {
        if (startup_event == null || sde_handler == null)
        {
            return false;
        }

        DispEventInfo new_evt_info = new DispEventInfo();
        new_evt_info.startup_event = startup_event;
        new_evt_info.terminate_event = terminate_event;
        new_evt_info.sde_handler = sde_handler;
        new_evt_info.fRegisterTime = Time.time + 0.0001f;
        new_evt_info.fPastTime = 0.0f;
        m_lstDispEventInfos.Add(new_evt_info);

        return true;
    }

    //检测事件类型是否一致
    protected bool CheckEventType(BaseSkillElementHandler handler, SkillDispEvent evt1, SkillDispEvent evt2)
    {
        if (evt1.m_EventType != evt2.m_EventType)
        {
            return false;
        }

        switch (evt1.m_EventType)
        {
            case SkillDispEventType.DISPEVT_PROGRAM_CUSTOM:
                if (evt1.m_CustomFlag != evt2.m_CustomFlag)
                {
                    return false;
                }
                break;

            case SkillDispEventType.DISPEVT_SHOW_BEGIN:
            case SkillDispEventType.DISPEVT_SHOW_END:
                {
                    return (handler.UniqueId == (uint)evt2.m_Param2);
                }

            default:
                break;
        }

        return true;
    }

    //是否是持久事件
    protected bool IsPersistentEvent(SkillDispEvent evt)
    {
        switch (evt.m_EventType)
        {
            case SkillDispEventType.DISPEVT_ON_SHOOT_POINT:
            case SkillDispEventType.DISPEVT_ON_SHOOT_TARGET:
            case SkillDispEventType.DISPEVT_PROGRAM_CUSTOM:
                return true;

            default:
                break;
        }

        return false;
    }

    //触发事件
    public void TriggerEvent(SkillDispEvent evt)
    {
        if (evt == null)
        {
            return;
        }

        for (int i = 0; i < m_lstDispEventInfos.Count;)
        {
            bool bTerminated = false;

            DispEventInfo evt_info = m_lstDispEventInfos[i];

            if (evt_info.startup_event != null && CheckEventType(evt_info.sde_handler, evt_info.startup_event, evt))
            {
                evt_info.bStartup = evt_info.sde_handler.Startup(evt);
                if (!evt_info.bStartup)
                {
                    if (!IsPersistentEvent(evt_info.startup_event))
                    {
                        bTerminated = true;
                    }
                }
            }

            if (evt_info.terminate_event != null && CheckEventType(evt_info.sde_handler, evt_info.terminate_event, evt))
            {
                evt_info.sde_handler.Terminate(evt);
                bTerminated = true;
            }

            if (bTerminated)
            {
                m_lstDispEventInfos.RemoveAt(i);
            }
            else
            {
                ++i;
            }
        }
    }

    // return is need to terminated
    //检测事件是否执行完毕，需要移除，只有定时触发的事件才需要在这里检测
    bool UpdateDispEventInfo(DispEventInfo evt_info, float fDeltaTime, float TimeScale)
    {
        evt_info.fPastTime += fDeltaTime * TimeScale;

        if (evt_info.bStartup)
        {
            if (evt_info.terminate_event == null)
            {
                return false;
            }

            switch (evt_info.terminate_event.m_EventType)
            {
                case SkillDispEventType.DISPEVT_TIMEPOINT_TRIGGER:
                    {
                        //float fRealTime = evt_info.fRegisterTime + evt_info.terminate_event.m_Param1;
                        if (evt_info.fPastTime >= evt_info.startup_event.m_Param1 + evt_info.terminate_event.m_Param1)
                        {
                            evt_info.sde_handler.Terminate(evt_info.terminate_event);
                            return true;
                        }
                    }
                    break;

                default:
                    break;
            }
            return false;
        }

        if (evt_info.startup_event == null)
        {
            return false;
        }

        switch (evt_info.startup_event.m_EventType)
        {
            case SkillDispEventType.DISPEVT_TIMEPOINT_TRIGGER:
                {
                    if (m_refCurSkillInfo.Caster == null)
                    {
                        return true;
                    }

                    if (m_refCurSkillInfo.Caster.GetMecanim() == null)
                    {
                        return true;
                    }

                    if (m_refCurSkillInfo.Caster.GetMecanim().GetLastPlayAnimationKey() < MecanimBehaviour.AnimationKey.SKILL_1
                        || m_refCurSkillInfo.Caster.GetMecanim().GetLastPlayAnimationKey() > MecanimBehaviour.AnimationKey.SKILL_15)
                    {
                        return true;
                    }

                    //float fRealTime = evt_info.fRegisterTime + evt_info.startup_event.m_Param1;
                    if (evt_info.fPastTime >= evt_info.startup_event.m_Param1)
                    {
                        evt_info.bStartup = evt_info.sde_handler.Startup(evt_info.startup_event);
                        if (!evt_info.bStartup)
                        {
                            return true;
                        }
                    }
                }
                break;
            default:
                break;
        }

        return false;
    }

    List<DispEventInfo> tmpRemoveList = new List<DispEventInfo>();
    public bool Update(float fDeltaTime)
    {
        float fCurTime = Time.time;

        float fTimeScale = 1.0f;
        if (m_refCurSkillInfo.Caster != null)
        {
            //施法加速
            //int nSpeed = m_refCurSkillInfo.Caster.GetBuffStateNum(WLGame.eActorBuffEffectState.ABE_ATTACK_SPEED);
            int nSpeed = 0;
            fTimeScale = 1.0f + (float)nSpeed / 100.0f;

            // 最低攻速为1/10 暂时硬编码写死
            if (fTimeScale < 0.1)
            {
                fTimeScale = 0.1f;
            }
        }

        for (int i = 0; i < m_lstDispEventInfos.Count; i++)
        {
            bool bTerminated = false;
            DispEventInfo evt_info = m_lstDispEventInfos[i];
            bTerminated = UpdateDispEventInfo(evt_info, fDeltaTime, fTimeScale);

            if (bTerminated)
            {
                tmpRemoveList.Add(evt_info);
            }
            else if (!evt_info.sde_handler.Update(fDeltaTime))
            {
                if (!IsPersistentEvent(evt_info.startup_event))
                {
                    tmpRemoveList.Add(evt_info);
                }
            }
        }
        for (int i = 0; i < tmpRemoveList.Count; i++)
        {
            m_lstDispEventInfos.Remove(tmpRemoveList[i]);
        }
        tmpRemoveList.Clear();

        m_fLastUpdateTime = fCurTime;

        return (m_lstDispEventInfos.Count > 0);
    }

    //清除
    public void Clearup(bool bKeepActiveElements)
    {
        if (bKeepActiveElements)
        {
            for (int i = 0; i < m_lstDispEventInfos.Count;)
            {
                DispEventInfo evt_info = m_lstDispEventInfos[i];
                if (evt_info.bStartup || evt_info.sde_handler.IsAlwaysActive())
                {
                    ++i;
                }
                else
                {
                    m_lstDispEventInfos.RemoveAt(i);
                }
            }
        }
        else
        {
            for (int i = 0; i < m_lstDispEventInfos.Count; i++)
            {
                DispEventInfo evt_info = m_lstDispEventInfos[i];
                if (evt_info.bStartup)
                {
                    evt_info.sde_handler.Terminate(evt_info.terminate_event);
                }
            }

            m_lstDispEventInfos.Clear();
        }
    }

}
