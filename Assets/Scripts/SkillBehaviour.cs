using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SkillBehaviour : MonoBehaviour {
    //技能最长播放时间
    private const float MAX_SKILLDISP_TIME = 15.0f;
    //当前依附的角色
    private BaseActor m_Actor = null;
    //当前角色的动作控制器
    private MecanimBehaviour m_MecanimBehaviour = null;
    //当前角色的Transform
    private Transform m_Transform;
    //当前播放的技能
    private CastSkillInfo m_CurSkillInfo = null;   // Current Skill Info
    public uint CurSkillId
    {
        get { return m_CurSkillInfo != null ? m_CurSkillInfo.UniqueId : 0; }
    }
    //技能动作播放完毕后的回调
    private System.Action<uint, bool> _finishCallback = null;
    //技能动作播放完毕后技能的缓存
    private List<SkillDispInfo> m_lstSkillDispInfos = new List<SkillDispInfo>();   // The skills casted, now they are processing

    // Use this for initialization
    void Start () {
        m_MecanimBehaviour = gameObject.GetComponent<MecanimBehaviour>();
        m_Actor = gameObject.GetComponent<BaseActor>();
        m_Transform = transform;

        if (m_MecanimBehaviour != null)
        {
            MecanimBehaviour.AnimationKey Key = MecanimBehaviour.AnimationKey.SKILL_1;
            for (; Key <= MecanimBehaviour.AnimationKey.SKILL_15; ++Key)
            {
                m_MecanimBehaviour.RegisterBeginCallback(Key, SkillDispBeginMecanimCallback);
                m_MecanimBehaviour.RegisterHaltCallback(Key, SkillDispHaltMecanimCallback);
                m_MecanimBehaviour.RegisterFinishCallback(Key, SkillDispFinishMecanimCallback);
            }

            m_MecanimBehaviour.RegisterBeginCallback(MecanimBehaviour.AnimationKey.HIT_RECOVERY_1, OnRecoverBeginCallback);
            m_MecanimBehaviour.RegisterFinishCallback(MecanimBehaviour.AnimationKey.HIT_RECOVERY_1, OnRecoverFinishCallback);
        }
        else
        {
           Debug.LogError("SkillDisp.cs Start() can't register animaion callback _FastMecanim == null!");
        }
    }

    //技能开始
    void SkillDispBeginMecanimCallback(MecanimBehaviour curBehaviour)
    {
        Debug.Log("技能开始");
        if (m_CurSkillInfo == null)
        {
            return;
        }
        //获取当前播放的动画
        AnimatorStateInfo curInfo = curBehaviour.GetCurAnimatorStateInfo();
        //当前动画时长
        float fSkillDispTime = curInfo.length;
        //是否循环
        bool bIsUseAnimationTotalTime = curInfo.loop;
#if UNITY_EDITOR
        if (SceneManager.GetActiveScene().name == "SkillEditor")
        {
            if (bIsUseAnimationTotalTime)
            {
                fSkillDispTime = 5.0f;
            }
        }
#endif

        if (m_CurSkillInfo.m_SkillDataInfo != null)
        {
            if (m_CurSkillInfo.m_SkillDataInfo.DurTime != 0)
            {
                fSkillDispTime = m_CurSkillInfo.m_SkillDataInfo.DurTime * 0.1f;
                bIsUseAnimationTotalTime = true;
            }
        }

        if (bIsUseAnimationTotalTime)
        {
            //设置当前动画播放时长
            curBehaviour.SetAnimationTotalTime(fSkillDispTime);
        }
        if (m_CurSkillInfo.m_DispData.m_PerformDatas != null)
        {
            int nPerformPhaseCount = m_CurSkillInfo.m_DispData.m_PerformDatas.GetLength(0);
            for (int i = 0; i < nPerformPhaseCount; i++)
            {
                if (m_CurSkillInfo.m_DispData.m_PerformDatas[i].m_AnimationKey == (int)(curBehaviour.GetLastPlayAnimationKey()))
                {
                    OnPerformBeginInternal(fSkillDispTime, i);
                    return;
                }
            }
        }
    }

    //技能中断
    void SkillDispHaltMecanimCallback(MecanimBehaviour curBehaviour)
    {
        OnPerformEndInternal();
    }

    //技能结束
    void SkillDispFinishMecanimCallback(MecanimBehaviour curBehaviour)
    {
        Debug.Log("技能结束");
        if (_finishCallback != null)
        {
            uint skillId = (m_CurSkillInfo != null ? m_CurSkillInfo.m_uiSkillId : 0);
            _finishCallback(skillId, false);
            _finishCallback = null;
        }

        OnPerformEndInternal();
    }

    //开始打击恢复
    void OnRecoverBeginCallback(MecanimBehaviour curBehaviour)
    {
        if (curBehaviour || curBehaviour.m_Actor)
        {
            return;
        }

        //int DisableMove = curBehaviour.m_Actor.GetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_MOVE);
        //int DisableAttack = curBehaviour.m_Actor.GetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_ATTACK);
        //int DisableSkill = curBehaviour.m_Actor.GetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_SKILL);

        //curBehaviour.m_Actor.SetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_MOVE, DisableMove + 1);
        //curBehaviour.m_Actor.SetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_ATTACK, DisableAttack + 1);
        //curBehaviour.m_Actor.SetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_SKILL, DisableSkill + 1);
    }

    //打击恢复完成
    void OnRecoverFinishCallback(MecanimBehaviour curBehaviour)
    {
        if (curBehaviour || curBehaviour.m_Actor)
        {
            return;
        }

        //int DisableMove = curBehaviour.m_Actor.GetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_MOVE);
        //int DisableAttack = curBehaviour.m_Actor.GetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_ATTACK);
        //int DisableSkill = curBehaviour.m_Actor.GetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_SKILL);

        //curBehaviour.m_Actor.SetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_MOVE, DisableMove > 0 ? DisableMove - 1 : 0);
        //curBehaviour.m_Actor.SetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_ATTACK, DisableAttack > 0 ? DisableAttack - 1 : 0);
        //curBehaviour.m_Actor.SetBuffStateNum(eActorBuffEffectState.ABE_DISABLE_SKILL, DisableSkill > 0 ? DisableSkill - 1 : 0);
    }
    
    //当前动作开始
    void OnPerformBeginInternal(float fAnimTime, int nAnimIndex)
    {
        m_CurSkillInfo.CurAnimTotalTime = fAnimTime;
        m_CurSkillInfo.CurAnimStartTime = Time.time;

        if (m_CurSkillInfo.m_DispData.m_PerformDatas != null)
        {
            SkillPerformData perform_data = m_CurSkillInfo.m_DispData.m_PerformDatas[nAnimIndex];

            SetupSkillDispInfo(m_CurSkillInfo, perform_data.m_DispInfo, fAnimTime);
        }
        //触发施放阶段开始事件
        m_CurSkillInfo.DispEventManager.TriggerEvent(new SkillDispEvent(SkillDispEventType.DISPEVT_PERFORM_BEGIN, 0.0f, nAnimIndex));
    }

    /// <summary>
    /// 依次设置施放阶段中的所有元素信息
    /// </summary>
    /// <param name="cur_skill_info">当前技能信息</param>
    /// <param name="disp_info">展示技能信息</param>
    /// <param name="fPhaseTotalTime">动画总时长</param>
    void SetupSkillDispInfo(CastSkillInfo cur_skill_info, DispInfo disp_info, float fPhaseTotalTime)
    {
        if (disp_info == null || cur_skill_info == null)
        {
            return;
        }
        if (disp_info.m_EffectElements != null)
        {
            for (int i = 0; i < disp_info.m_EffectElements.GetLength(0); i++)
            {
                EffectElementHandler handler = new EffectElementHandler();
                handler.SetupCurSkillInfo(cur_skill_info, fPhaseTotalTime);
                if (!handler.Setup(disp_info.m_EffectElements[i]))
                {
                    Debug.LogError("Fail to setup effect element: " + i);
                }
            }
        }
        if (disp_info.m_DirectImpactElements != null)
        {
            for (int i = 0; i < disp_info.m_DirectImpactElements.GetLength(0); i++)
            {
                DirectImpactElementHandler handler = new DirectImpactElementHandler();
                handler.SetupCurSkillInfo(cur_skill_info, fPhaseTotalTime);
                if (!handler.Setup(disp_info.m_DirectImpactElements[i]))
                {
                    Debug.LogError("Fail to setup effect element: " + i);
                }
            }
        }
        if (disp_info.m_TrackingBulletElements != null)
        {
            for (int i = 0; i < disp_info.m_TrackingBulletElements.GetLength(0); i++)
            {
                TrackingBulletElementHandler handler = new TrackingBulletElementHandler();
                handler.SetupCurSkillInfo(cur_skill_info, fPhaseTotalTime);
                if (!handler.Setup(disp_info.m_TrackingBulletElements[i]))
                {
                    Debug.LogError("Fail to setup effect element: " + i);
                }
            }
        }
        if (disp_info.m_AudioElements != null)
        {
            for (int i = 0; i < disp_info.m_AudioElements.GetLength(0); i++)
            {
                AudioElementHandler handler = new AudioElementHandler();
                handler.SetupCurSkillInfo(cur_skill_info, fPhaseTotalTime);
                if (!handler.Setup(disp_info.m_AudioElements[i]))
                {
                    Debug.LogError("Fail to setup effect element: " + i);
                }
            }
        }
        if (disp_info.m_ShootPointElements != null)
        {
            for (int i = 0; i < disp_info.m_ShootPointElements.GetLength(0); i++)
            {
                ShootPointElementHandler handler = new ShootPointElementHandler();
                handler.SetupCurSkillInfo(cur_skill_info, fPhaseTotalTime);

                ShootPointElement shootpoint_element = disp_info.m_ShootPointElements[i];
                if (!handler.Setup(shootpoint_element))
                {
                    ////print ("Fail to setup shoot point element: " + i);
                }

                if (shootpoint_element.m_ShowAtkRangeCfg != null && shootpoint_element.m_ShowAtkRangeCfg.m_Show)
                {
                    FanDispElementHandler fandisp_handler = new FanDispElementHandler();
                    fandisp_handler.SetupCurSkillInfo(cur_skill_info, fPhaseTotalTime);

                    if (!fandisp_handler.Setup(shootpoint_element.m_ShootTestParam.m_Range,
                        shootpoint_element.m_ShowAtkRangeCfg.m_BeginTime,
                        shootpoint_element.m_ShowAtkRangeCfg.m_EndTime,
                        shootpoint_element.m_ShowAtkRangeCfg.m_VertOffset,
                        shootpoint_element.m_ShootTestParam.m_ShootTestBase))
                    {
                        ////print ("Fail to setup fan disp element: " + i);
                    }
                }
            }
        }
    }

    //当前动作结束
    void OnPerformEndInternal()
    {
        if (m_CurSkillInfo == null)
        {
            return;
        }
        //触发施放阶段结束事件
        m_CurSkillInfo.DispEventManager.TriggerEvent(new SkillDispEvent(SkillDispEventType.DISPEVT_PERFORM_END));

        m_CurSkillInfo.CurAnimStartTime = 0.0f;

        SkillDispInfo skilldisp_info = new SkillDispInfo();
        skilldisp_info.m_fCreateTime = Time.time;
        skilldisp_info.m_CurSkillInfo = m_CurSkillInfo;
        m_lstSkillDispInfos.Add(skilldisp_info);

        //Debuger.Log(m_Actor + " add skill_dispinfo " + m_CurSkillInfo.m_uiSkillId);

        m_CurSkillInfo = null;
    }

    void FixedUpdate()
    {
        if (m_CurSkillInfo != null)
        {
            m_CurSkillInfo.DispEventManager.Update(Time.fixedDeltaTime);
        }
        CheckUpdatePlayingSkillDisp();
    }

    void CheckUpdatePlayingSkillDisp()
    {
        float fCurTime = Time.time;

        // Check update playing skill disp
        for (int i = 0; i < m_lstSkillDispInfos.Count;)
        {
            if (fCurTime - m_lstSkillDispInfos[i].m_fCreateTime >= MAX_SKILLDISP_TIME)
            {
                m_lstSkillDispInfos[i].m_CurSkillInfo.DispEventManager.Clearup(false);
                m_lstSkillDispInfos.RemoveAt(i);
                continue;
            }

            if (m_lstSkillDispInfos[i].m_CurSkillInfo.DispEventManager.Update(Time.fixedDeltaTime))
            {
                ++i;
            }
            else
            {
                m_lstSkillDispInfos.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 播放技能
    /// </summary>
    /// <param name="uiSkillId"></param>
    /// <param name="nSkillDispId"></param>
    /// <param name="uSelectTargetId"></param>
    /// <param name="lstTargets"></param>
    /// <param name="param"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public bool ShowSkill(uint uiSkillId, int nSkillDispId, uint uSelectTargetId = 0, List<uint> lstTargets = null, SkillParam param = null, System.Action<uint, bool> callback = null)
    {
        if (uiSkillId == 0)
        {
            return false;
        }

        if (m_CurSkillInfo != null)
        {
            CancelSkill();
        }

        _finishCallback = callback;

        // 播放技能动作
        return StartNewSkillDisp(uiSkillId, nSkillDispId, uSelectTargetId, lstTargets, param);
    }

    bool StartNewSkillDisp(uint uiSkillId, int nSkillDispId, uint uSelectTargetId = 0, List<uint> lstTargets = null, SkillParam param = null)
    {
        SkillDispData disp_data = SkillManager.Instance.LoadSkillDispData(10086);
        if (disp_data == null)
        {
            Debug.LogError("[" + name + "]Fail to Get SkillDispData " + nSkillDispId);
            return false;
        }

        if (m_CurSkillInfo == null)
        {
            m_CurSkillInfo = new CastSkillInfo();
        }

        //m_CurSkillInfo.m_SkillDataInfo = 技能配置
        m_CurSkillInfo.m_uiSkillId = uiSkillId;
        m_CurSkillInfo.m_nSkillDispId = nSkillDispId;
        m_CurSkillInfo.m_DispData = disp_data;
        m_CurSkillInfo.m_SkillParam = param;
        m_CurSkillInfo.Caster = m_Actor;
        m_CurSkillInfo.CasterOriginalPos = m_Transform.position;
        m_CurSkillInfo.SelectTargetId = uSelectTargetId;
        m_CurSkillInfo.SetupTargets(ref lstTargets);
        m_CurSkillInfo.StartupTime = Time.time;

        int nAnimKey = 0;
        if (disp_data.m_PrepareDatas != null)
        {
            if (disp_data.m_PrepareDatas.GetLength(0) > 0)
                nAnimKey = disp_data.m_PerformDatas[0].m_AnimationKey;
        }

        if (nAnimKey == 0 && disp_data.m_PerformDatas != null)
        {
            if (disp_data.m_PerformDatas.GetLength(0) > 0)
                nAnimKey = disp_data.m_PerformDatas[0].m_AnimationKey;
        }

        nAnimKey = (int)MecanimBehaviour.AnimationKey.SKILL_1;

        if (nAnimKey != 0)
        {
            //计算buff增速
            //int nSpeed = m_Actor.GetBuffStateNum(eActorBuffEffectState.ABE_ATTACK_SPEED);
            int nSpeed = 0;
            float fSpeedScale = 1.0f + (float)nSpeed / 100.0f;

            // 最低攻速为1/10 暂时硬编码写死
            if (fSpeedScale < 0.1)
            {
                fSpeedScale = 0.1f;
            }

            m_CurSkillInfo.TimeScale = fSpeedScale;
            m_MecanimBehaviour.PlayAnimation((MecanimBehaviour.AnimationKey)nAnimKey, true, fSpeedScale, true);
        }

        return true;
    }

    //取消施法
    public void CancelSkill()
    {

    }


    void OnDestroy()
    {
        if (m_CurSkillInfo != null)
        {
            m_CurSkillInfo.DispEventManager.Clearup(false);
        }

        foreach (SkillDispInfo info in m_lstSkillDispInfos)
        {
            info.m_CurSkillInfo.DispEventManager.Clearup(false);
        }
        m_lstSkillDispInfos.Clear();
    }
}
