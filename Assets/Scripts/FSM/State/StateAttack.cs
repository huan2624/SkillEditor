using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class StateAttack : BaseState {

    private const uint NULL_SKILL_ID = 0;
    private uint m_CurrentSkillId = NULL_SKILL_ID;
    private uint m_PrepareSkillId = NULL_SKILL_ID; 
    private uint m_NextStageSkillId = NULL_SKILL_ID; 
    private uint m_TargetID = 0;
    private float m_RunTime;
    private CheckSkillResult m_CheckSkillResult = CheckSkillResult.Exception;

    public StateAttack(StateID id, BaseActor actor)
            : base(id, actor)
    {
        
    }

    // 设置要释放的技能信息
    public void SetAttackInfo(uint skillID, uint targetID)
    {
        m_PrepareSkillId = skillID;
        m_TargetID = targetID;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        m_RunTime = 0;
        m_CurrentSkillId = m_PrepareSkillId;
        m_NextStageSkillId = NULL_SKILL_ID;

        m_CheckSkillResult = DoSkill(m_CurrentSkillId, m_TargetID, OnAttackOver);
    }

    //开始技能
    CheckSkillResult DoSkill(uint skillID, uint TargetId, System.Action<uint, bool> callback)
    {
        BaseActor attacker = GetActor();

        CheckSkillResult skillResult = CheckSkillResult.Exception;
        List<uint> Myself = new List<uint>();
        Myself.Add(m_TargetID);
        skillResult = SkillManager.Instance.DoSkill(attacker, skillID, TargetId, Myself, null, callback);

        return skillResult;
    }

    //攻击结束
    void OnAttackOver(uint skillId, bool bHat)
    {
        if (skillId != m_CurrentSkillId)
        {
            return;
        }

        LeaveState();
    }

    public override void OnEnterAgain()
    {
        base.OnEnterAgain();
    }

    public override void OnLeave()
    {
        base.OnLeave();

        m_TargetID = 0;
        m_CurrentSkillId = NULL_SKILL_ID;
        m_PrepareSkillId = NULL_SKILL_ID;
        m_NextStageSkillId = NULL_SKILL_ID;
    }

    public override void Update()
    {
        base.Update();

        // 超时3秒退出【容错处理】
        if ((m_RunTime += Time.deltaTime) > 4.0f)
        {
            LeaveState();
            return;
        }
    }

    public override void Transition()
    {
        base.Transition();
    }

    public CheckSkillResult GetSkillResult()
    {
        return m_CheckSkillResult;
    }
}
