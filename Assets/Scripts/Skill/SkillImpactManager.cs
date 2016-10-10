using UnityEngine;
using System.Collections;
using wl_res;
using System.Collections.Generic;
using System;

public enum ImapctTargetType
{
    IMPACT_TYPE_UNKNOWN = 0,    //未知
    IMPACT_TYPE_MYSELF,         //自己
    IMPACT_TYPE_ALLY,           //友方
    IMPACT_TYPE_ENEMY,          //敌方
    IMPACT_TYPE_MAXCOUNT,       //最大数量
}

public class SkillImpactResult
{
    public enum ImpactResult
    {
        SR_SIMPLE_INJURED = 0,      // 普通伤害
        SR_HIT_RECOVERY,            // 硬直
        SR_SMALL_BACKOFF,           // 小击退
        SR_HEAVY_BACKOFF,           // 大击退
        SR_FLOATUP,                 // 浮空
        SR_PUTDOWN,                 // 击倒
        SR_DROPDOWN,                // 摔击倒地，扔出去
        SR_MISS,                    // 闪避
        SR_IMMUNO,                  // 免疫
        SR_GAIN,					// 治疗
    }

    public ImpactResult Result = ImpactResult.SR_SIMPLE_INJURED;

    public int nTotalDamage = 0;    //总伤害
    public bool bCritImpact = false;//是否是暴击
    public bool bCauseDeath = false;//是否引起死亡

    public string GetString()
    {
        string str = Result.ToString();

        str += " TotalDamage " + nTotalDamage;
        if (bCritImpact)
            str += " [CritImpact]";
        if (bCauseDeath)
            str += " [CauseDeath]";

        return str;
    }
}

// 改变对方状态

public class SkillImpactCenter
{
    public class SkillValue
    {
        public float fBasicHp = 0.0f;
        public float fBasicAttack = 0.0f;
        public float fBasicDefense = 0.0f;

        public float fBasicAttackIncRate = 0.0f;
        public float fBasicDefenseIncRate = 0.0f;
        public float fHpIncRate = 0.0f;

        public float fExtraAttack = 0.0f;
        public float fExtraDefense = 0.0f;
        public float fExtraHp = 0.0f;

        public float fCriticalAttack = 0.0f;
        public float fCriticalDefense = 0.0f;

        public float fFallDownAttack = 0.0f;
        public float fFallDownDefense = 0.0f;

        public float fFloatupAttack = 0.0f;
        public float fFloatupDefense = 0.0f;

        public float fColdDownAttack = 0.0f;
        public float fColdDownDefense = 0.0f;

        public float fDirectDamage = 0.0f;
        public float fDamageExempt = 0.0f;

        public float fFloatupAvoid = 0.0f;
        public float fFallDownAvoid = 0.0f;
        public float fDefeatAvoid = 0.0f;
        public float fColdDownAvoid = 0.0f;
    }

    /* 技能表现优先级
     * 浮空        4
     * 倒地        4
     * 大击退      3
     * 小击退      2
     * 硬直        1
     */
    //计算技能伤害结果
    public static SkillImpactResult CommitSkillImpact(BaseActor ActorCaster, BaseActor ActorTarget,
        wl_res.SkillDataInfo skill_data, SkillImpactData skill_impact_data)
    {
        if (skill_data == null)
        {
            return null;
        }

        SKILL_EFFECT SkillEffect = (SKILL_EFFECT)skill_data.SkillEffect;

        SkillImpactResult result = null;

        switch (SkillEffect)
        {
            case SKILL_EFFECT.E_SE_NO_EFFECT:
                break;
            case SKILL_EFFECT.E_SE_HURT:
                {
                    result = CommitHurtSkillImpact(ActorCaster, ActorTarget, skill_data, skill_impact_data);
                }
                break;
            case SKILL_EFFECT.E_SE_GAIN:
                {
                    CommitGainSkillImpact(ActorCaster, ActorTarget, skill_data);
                }
                break;
            case SKILL_EFFECT.E_SE_HURT_GAIN:
                {
                    if (ActorCaster.GetActorGroup() != ActorTarget.GetActorGroup())
                    {
                        result = CommitHurtSkillImpact(ActorCaster, ActorTarget, skill_data, skill_impact_data);
                    }
                    else
                    {
                        CommitGainSkillImpact(ActorCaster, ActorTarget, skill_data);
                    }
                }
                break;
        }


        if (result == null)
        {   
            // 这里触发的Buff是通过被动技能 或者 触发Buff的Buff来引发的
            #region 处理释放非伤害类型技能时触发Buff的代码

            
            #endregion
        }

        //技能 对应的Buffer 触发
        if (skill_data.ToOtherBuffID != 0 && result.Result != SkillImpactResult.ImpactResult.SR_IMMUNO)
        {
            
        }

        if (skill_data.ToSelfBuffID != 0)
        {
            
        }


        return result;
    }

    //计算加血
    static void CommitGainSkillImpact(BaseActor ActorCaster, BaseActor ActorTarget,
                                 wl_res.SkillDataInfo skill_data)
    {
        
    }

    //计算伤害
    static SkillImpactResult CommitHurtSkillImpact(BaseActor ActorCaster, BaseActor ActorTarget,
                                        wl_res.SkillDataInfo skill_data, SkillImpactData skill_impact_data)
    {
        SkillImpactResult result = new SkillImpactResult();
        result.Result = SkillImpactResult.ImpactResult.SR_SIMPLE_INJURED;

        #region 处理释放技能时触发Buff的代码



        #endregion

        return result;
    }

    protected static bool GetSkillImpactResult(float fColdDownRate, float fFallDownRate, float fFloatupRate, ref SkillImpactResult.ImpactResult Result)
    {
        float Random = UnityEngine.Random.Range(0.0f, 1.0f);

        if (Random > fColdDownRate)
        {
            return false;
        }

        Result = SkillImpactResult.ImpactResult.SR_HIT_RECOVERY;
        return true;
    }

}
