using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum SKILL_TARGET_TYPE
{
    E_STT_CASTER = 1, // 对施法者
    E_STT_ENEMY = 2, // 对敌方
    E_STT_FRIEND = 3, // 对友方
    E_STT_ALL = 4, // 对全体单位
}
public enum SKILL_EFFECT
{
    E_SE_NO_EFFECT = 0, // 没有任何效果
    E_SE_HURT = 1, // 伤害
    E_SE_GAIN = 2, // 加血
    E_SE_HURT_GAIN = 3, // 对敌伤害对友方回血
}
public enum SKILL_HALT_TYPE
{
    E_ST_HURT = 1, // 伤害
    E_ST_GAIN = 2, // 增益
    E_ST_SPECIAL = 4, // 特殊效果
}

public enum CheckSkillResult
{
    OK = 0,
    InvalidParam,
    InvalidCaster,
    //InCoolDown,
    InvalidTarget,
    OutofRange,
    //TooCloseRange,      //距离太近
    Disable,           // 被buff禁止使用了 Add By Sword [2014_07_12]
    Exception,
}

public class TargetImpactInfo
{
    public uint m_uiTargetId = 0;
    public SkillImpactResult m_SkillImpact;
    public CastSkillInfo m_SkillInfo;
}

public class SkillManager : TSingleton<SkillManager> {
    
    private SkillDataReader m_SkillReader = new SkillDataReader();
    private Dictionary<int, SkillDispData> m_SkillEditorDataStore = new Dictionary<int, SkillDispData>();
    private Queue<TargetImpactInfo> m_queTargetImpactInfos = new Queue<TargetImpactInfo>();

    GameObject m_FanDispObjPrefab;

    public void AddCacheDispData(int skillIdk, SkillDispData data)
    {
        m_SkillEditorDataStore[skillIdk] = data;
    }

    /// <summary>
    /// 加载编辑器配置的技能数据
    /// </summary>
    /// <param name="iSkillDispId"></param>
    /// <returns></returns>
    public SkillDispData LoadSkillDispData(int iSkillDispId)
    {
        SkillDispData data;
        if (!m_SkillEditorDataStore.TryGetValue(iSkillDispId, out data))
        {
            data = m_SkillReader.LoadSkillData(iSkillDispId.ToString());
            if (data != null)
            {
                m_SkillEditorDataStore.Add(iSkillDispId, data);
            }
        }
        else
        {
            Debug.Log("缓存加载");
        }
        
        return data;
    }

    /// <summary>
    /// 角色开始施法
    /// </summary>
    /// <param name="Caster"></param>
    /// <param name="uiSkillID">技能配置id</param>
    /// <param name="uSelectTargetId"></param>
    /// <param name="lstTargets"></param>
    /// <param name="param"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public CheckSkillResult DoSkill(BaseActor Caster, uint uiSkillID, uint uSelectTargetId, List<uint> lstTargets, SkillParam param, System.Action<uint, bool> callback = null)
    {
        SkillBehaviour skillBehav = Caster.GetSkillBehaviour();
        if (skillBehav == null)
        {
            return CheckSkillResult.Exception;
        }
        //技能编辑器文件id
        int SkillDispID = 0;
        skillBehav.ShowSkill(uiSkillID, SkillDispID, uSelectTargetId, lstTargets, param, callback);
        return CheckSkillResult.OK;
    }

    /// <summary>
    /// 角色开始受击
    /// </summary>
    /// <param name="cur_skill_info"></param>
    /// <param name="target_id"></param>
    /// <param name="skill_impact_data"></param>
    /// <returns></returns>
    public bool ImpactTarget(CastSkillInfo cur_skill_info, uint target_id, SkillImpactData skill_impact_data)
    {
        BaseActor actorTarget = ActorManager.Instance.GetActor(target_id);
        if (actorTarget == null || actorTarget.IsDead())
        {
            return false;
        }

        wl_res.SkillDataInfo skill_data = null;
        //if (skill_data == null)
        //{
        //    return false;
        //}

        // 计算伤害在这个函数里头
        SkillImpactResult skill_impact_result = SkillImpactCenter.CommitSkillImpact(cur_skill_info.Caster, actorTarget, skill_data, skill_impact_data);
        //         if (skill_impact_result != null)
        //         {
        //             TargetImpactInfo target_impact_info = new TargetImpactInfo();
        //             target_impact_info.m_uiTargetId = target_id;
        //             target_impact_info.m_SkillImpact = skill_impact_result;
        //             target_impact_info.m_SkillInfo = cur_skill_info;
        //             m_queTargetImpactInfos.Enqueue(target_impact_info);
        //             actorTarget.OnSkillImpacted(cur_skill_info.Caster, cur_skill_info, skill_impact_result);
        //         }

        TargetImpactInfo target_impact_info = new TargetImpactInfo();
        target_impact_info.m_uiTargetId = target_id;
        target_impact_info.m_SkillImpact = skill_impact_result;
        target_impact_info.m_SkillInfo = cur_skill_info;
        m_queTargetImpactInfos.Enqueue(target_impact_info);
        actorTarget.OnSkillImpacted(cur_skill_info.Caster, cur_skill_info, skill_impact_result);

        return true;
    }

    /// <summary>
    /// 检测扇形范围内的受击目标
    /// </summary>
    /// <param name="lstShootTargets">受击目标列表</param>
    /// <param name="ActorCaster">施法者</param>
    /// <param name="ActorTester">检测目标</param>
    /// <param name="position">检测位置点</param>
    /// <param name="forward">朝向</param>
    /// <param name="range">扇形数据</param>
    /// <param name="mapImpactInfos">效果列表</param>
    /// <param name="fImpactInterval">受击时间间隔</param>
    /// <param name="nMaxImpactTimes">同一对象最大受击次数</param>
    /// <param name="nMaxShootCount">最大受击次数</param>
    /// <param name="eShootTestType">检测类型</param>
    /// <param name="eImapctTargetType">阵营</param>
    /// <returns></returns>
    public bool ExecuteShootTest(ref List<uint> lstShootTargets,
            BaseActor ActorCaster,
            BaseActor ActorTester,
            Vector3 position,
            Vector3 forward,
            AttackRange range,
            ref Dictionary<uint, ImpactInfo> mapImpactInfos,
            float fImpactInterval,
            int nMaxImpactTimes,
            int nMaxShootCount,
            ShootTestType eShootTestType,
            ImapctTargetType eImapctTargetType = ImapctTargetType.IMPACT_TYPE_ENEMY)
    {
        lstShootTargets.Clear();

        WLCollision.Capsule WeaponCapsuleR = WLCollision.Capsule.GetZeroCapsule();
        WLCollision.Capsule WeaponCapsuleL = WLCollision.Capsule.GetZeroCapsule();

        // fix : 将内半径设为0，外半径 = 技能半径 + 角色半径
        float attackerBodyRadius = ActorCaster.GetBodyRadius();
        CFanBody fanBody = new CFanBody(position, forward, range.m_Distance,
            0, range.m_RadiusOut + attackerBodyRadius, range.m_Height, range.m_Angle, range.m_VOffset);

#region 优先检测 ActorTest是否是攻击目标 然后再检测其他目标 保证当ActorTest是合法的情况下 会被放到列表中
        //如果阵营正确，没有死，还可以受击，那么就继续检测
        if ((ActorTester.GetActorGroup() == ActorCaster.GetActorGroup()) != (eImapctTargetType == ImapctTargetType.IMPACT_TYPE_ENEMY)
            && !ActorTester.IsDead()
            && TargetCanBeImpacted((uint)ActorTester.GetUniqueID(), ref mapImpactInfos, fImpactInterval, nMaxImpactTimes)
            )
        {
            if (SingleShootTest(ActorTester, ActorCaster,
                            ActorTester,
                            eShootTestType,
                            position,
                            fanBody,
                            WeaponCapsuleR, WeaponCapsuleL, eImapctTargetType))
            {
                lstShootTargets.Add((uint)ActorTester.GetUniqueID());
            }
        }
#endregion

        lstShootTargets.Add(ActorTester.GetActorAttribute().ACTOR_UNIQUE_ID);

        return true;
    }

    public bool SingleShootTest(BaseActor ShootTarget,
                                    BaseActor ActorCaster,
                                    BaseActor ActorTester,
                                    ShootTestType eShootTestType,
                                    Vector3 Position,
                                    CFanBody fanBody,
                                    WLCollision.Capsule WeaponCapsuleR,
                                    WLCollision.Capsule WeaponCapsuleL,
                                    ImapctTargetType eImapctTargetType = ImapctTargetType.IMPACT_TYPE_ENEMY)
    {
        if (ActorCaster == null)
        {
            return false;
        }

        Vector3 vPos = ShootTarget.gameObject.transform.localPosition;

        uint id = (uint)ShootTarget.GetUniqueID();

        if (Vector3.SqrMagnitude(ShootTarget.gameObject.transform.position - Position) <= 0.1f)
        {
            return true;
        }
        else
        {
            float fObjRadius = 0.5f;
            float fObjHeight = 1.0f;


            if (null != ShootTarget.GetActorAttribute())
            {
                //fObjRadius = ShootTarget.GetActorAttribute().m_CapsuleRadius;
                //fObjHeight = ShootTarget.GetActorAttribute().m_CapsuleHeight;
            }

            //目标位置是否在扇形中
            if (!fanBody.Pt3InFan(ShootTarget.gameObject.transform.position, fObjRadius, fObjHeight))
            {
                return false;
            }

            switch (eShootTestType)
            {
                case ShootTestType.SHOOTTEST_RANGE:
                    {
                        return true;
                    }
                case ShootTestType.SHOOTTEST_CAPSULE_COLLISION:
                    {
                        SkillBehaviour target_skill_disp = ShootTarget.GetSkillBehaviour();
                        if (target_skill_disp == null)
                        {
                            return false;
                        }

                        //WLCollision.Capsule target_capsule = target_skill_disp.AvatarCapsule;
                        //if (WLCollision.Capsule.Intersects(WeaponCapsuleR, target_capsule)
                        //    || WLCollision.Capsule.Intersects(WeaponCapsuleL, target_capsule))
                        //{
                        //    return true;
                        //}
                    }
                    break;
                default:
                    break;
            }
        }

        return false;
    }

    bool TargetCanBeImpacted(uint target_id, ref Dictionary<uint, ImpactInfo> mapImpactInfos, float fImpactInterval, int nMaxImpactTimes)
    {
        BaseActor Target = ActorManager.Instance.GetActor(target_id);
        if (!Target.gameObject.activeInHierarchy)
        {
            return false;
        }

        ImpactInfo impact_info;
        if (!mapImpactInfos.TryGetValue(target_id, out impact_info))
        {
            return true;
        }

        //如果受击次数已经超过限制，则不能继续受击
        if (impact_info.m_nImpactTimes >= nMaxImpactTimes)
            return false;
        //如果还没达到受击时间，则不能受击
        else if (Time.time < (impact_info.m_fLatestImpactTime + fImpactInterval))
            return false;
        else
            return true;
    }

    public GameObject CreateNewFanDispObject()
    {
        if (m_FanDispObjPrefab == null)
        {
            m_FanDispObjPrefab = Resources.Load("Other/FanDispObject") as GameObject;

            if (m_FanDispObjPrefab == null)
            {
                return null;
            }
        }

        GameObject FanDispObj = GameObject.Instantiate(m_FanDispObjPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        if (FanDispObj == null)
        {
            return null;
        }

        FanDispObj.name = string.Format("FanDispObject[{0}]", CommHelper.GenerateGOID());

        return FanDispObj;
    }

    protected void ProcessTargetImpacts()
    {
        int iTipsCnt = 0;
        while (m_queTargetImpactInfos.Count > 0 && iTipsCnt < 5)
        {
            TargetImpactInfo target_impact_info = m_queTargetImpactInfos.Dequeue();
            iTipsCnt++;

            BaseActor objTarget = ActorManager.Instance.GetActor(target_impact_info.m_uiTargetId);
            if (objTarget == null)
            {
                continue;
            }
        }
    }

    protected void OnLateUpdate()
    {
        ProcessTargetImpacts();
    }
}
