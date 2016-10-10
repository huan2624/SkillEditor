using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseActor : MonoBehaviour
{
    // 属性组件 根据 角色类型会挂载 不同类型的属性组件
    public ActorBaseAttribute m_refActorAttribute = null;
    //角色模型
    public GameObject Model { get; set; }
    //技能管理器
    protected SkillBehaviour m_SkillBehaviour;
    //受击管理器
    protected ImpactBehaviour m_SkillImpact;
    //动画管理器
    protected MecanimBehaviour m_Mecanim;
    //动画状态机
    protected FSMManager m_StateMgr;
    //行为树
    protected FSMManager m_BehaviourMgr;
    //移动组件
    protected MoveBehaviour m_MoveBehaviour;

    public uint GetUniqueID()
    {
        return m_refActorAttribute.ACTOR_UNIQUE_ID;
    }

    public SkillBehaviour GetSkillBehaviour()
    {
        return m_SkillBehaviour;
    }

    public FSMManager GetStateMgr()
    {
        return m_StateMgr;
    }

    public FSMManager GetBehaviourMgr()
    {
        return m_BehaviourMgr;
    }

    public MecanimBehaviour GetMecanim()
    {
        return m_Mecanim;
    }

    public ActorBaseAttribute GetActorAttribute()
    {
        return m_refActorAttribute;
    }

    public FSMNode GetState(int id)
    {
        return m_StateMgr.GetState(id);
    }

    public ActorGroup GetActorGroup()
    {
        return m_refActorAttribute.ActorGroup;
    }

    public bool IsDead()
    {
        return m_refActorAttribute.IsDead;
    }

    public float GetBodyRadius()
    {
        return 0.5f;
    }

    void Start()
    {
        Model = transform.FindChild("Warrior_W_I").gameObject;
        m_Mecanim = gameObject.AddComponent<MecanimBehaviour>();
        m_MoveBehaviour = gameObject.AddComponent<MoveBehaviour>();
        m_SkillImpact = gameObject.AddComponent<ImpactBehaviour>();
        m_SkillBehaviour = gameObject.AddComponent<SkillBehaviour>();
        m_refActorAttribute = new ActorBaseAttribute();
        m_StateMgr = new FSMManager();

        m_refActorAttribute.ACTOR_UNIQUE_ID = CommHelper.GenerateGOID();
        ActorManager.Instance.RegistActor(this);

        InitState();
    }

    // 设置绑定点
    private void SetupObjectDummyPointMapAttribute()
    {
        if (m_refActorAttribute.ActorDummyPointDic == null)
        {
            m_refActorAttribute.ActorDummyPointDic = new Dictionary<DummyPoint, Transform>();
        }

        m_refActorAttribute.ActorDummyPointDic.Clear();

        for (DummyPoint point = DummyPoint.DM_HEAD; point <= DummyPoint.DM_BIP; ++point)
        {
            Transform trans = EffectBehaviour.GetDummyPointTransformByName(Model, AvatarDefine.GetDummyPointName(point));
            if (trans != null)
            {
                m_refActorAttribute.ActorDummyPointDic[point] = trans;
            }
        }
    }

    public void InitState()
    {
        StateIdle idleAction = new StateIdle(StateID.Idle, this);
        StateWalk walkAction = new StateWalk(StateID.Walk, this);
        StateAttack attackAction = new StateAttack(StateID.Attack, this);
        StateHitNormal hitAction = new StateHitNormal(StateID.HitNormal, this);
        GetStateMgr().SetDefaultState((int)StateID.Idle);
        GetStateMgr().Enable = true;
    }

    public void OnSkillImpacted(BaseActor attacker, CastSkillInfo attack_skill_info, SkillImpactResult skill_impact)
    {
        m_SkillImpact.OnSkillImpacted(attack_skill_info.Caster, attack_skill_info, skill_impact);

        if (!IsDead())
        {
            OnHit(attacker, attack_skill_info);
        }
    }

    void OnHit(BaseActor attacker, CastSkillInfo attack_skill_info)
    {
        // 如果怪物处于巡逻状态， 被攻击时主动进入追击状态


        // 判断是否死亡
        if (m_refActorAttribute.Hp <= 0)
        {
            int addAnger = 0;
            Dead(addAnger);

            // 记录杀手的ID
            //m_killerId = attacker.m_refActorAttribBehaviour.m_ActorID;
            return;
        }

        // 处理受击效果

        //HitType hitType = (HitType)attack_skill_info.m_SkillDataInfo.HitType;
        Vector3 targetPoint = transform.position;
        StateHitNormal hitNormalAction = GetStateMgr().GetState((int)StateID.HitNormal) as StateHitNormal;
        hitNormalAction.EnterState();

        // 增加怒气


        // 响应被攻击事件

    }

    /* 函数说明：死亡 */
    public void Dead(int addAnger)
    {
        m_refActorAttribute.IsDead = true;

    }

    public void MoveTo(Vector3 point, bool isJoystick = false) {
        StateWalk walkAction = GetStateMgr().GetState((int)StateID.Walk) as StateWalk;
        walkAction.SetPathPoint(point, isJoystick);
        walkAction.EnterState();
    }

    public void StopJoystick() {
        StateID currActionID = (StateID)GetCurrentActionID();
        if (currActionID == StateID.Walk)
        {
            StateWalk walkAction = GetStateMgr().GetState((int)StateID.Walk) as StateWalk;
            walkAction.StopJoystick();
        }
    }

    /// <summary>
    /// 函数说明：根据技能ID释放技能
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool AttackBySkillID(uint skillID, BaseActor target)
    {
        StateAttack attackAction = GetStateMgr().GetState((int)StateID.Attack) as StateAttack;
        if (attackAction != null)
        {
            //ActorUtils.FaceToTarget(this, target);
            uint targetID = (target != null) ? (uint)target.GetUniqueID() : 0;
            attackAction.SetAttackInfo(skillID, targetID);
            attackAction.EnterState();
            return attackAction.GetSkillResult() == CheckSkillResult.OK;
        }
        return false;
    }

    public int GetCurrentActionID()
    {
        return GetStateMgr().GetCurrentStateID();
    }

    // Update is called once per frame
    void Update()
    {

        m_StateMgr.Update();
    }
}