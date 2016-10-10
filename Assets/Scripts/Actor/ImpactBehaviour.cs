using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ImpactType : int
{
    IMPACT_NORMAL_HIT = 0,   // 普通伤害，无动作影响，受击方保持现有动作
    IMPACT_HIT_RECOVERY = 1,   // GuaGua：硬直最标准的翻译就是DiabloII里的Fast Hit Recovery（快速打击恢复）
    IMPACT_HIT_RECOVERY_SMALL_BACKOFF = 2,  // 小击退硬直
    IMPACT_HIT_RECOVERY_BIG_BACKOFF = 3,  // 大击退硬直

    IMPACT_FLOATUP = 4,   // Delay: 这里沿用mint的浮空翻译Float up这个词表达浮空非常贴切
    IMPACT_PUTDOWN = 5,   // Delay: put sb. down， 在美剧里经常出现表示“丢翻、撂倒”某人
    IMPACT_DROPDOWN = 6,   // Delay

    IMPACT_NORMAL_DIE = 7,   // 普通死亡
    IMPACT_BACKOFF_DIE = 8,   // 击退死亡
    IMPACT_FLYAWAY_DIE = 9,   // 击飞死亡
    IMPACT_SMASH_DIE = 10,  // 粉碎死亡
}

// ImpactAnimationID is corresponding to Animator transition condition..
public enum ImpactAnimatorTransitionID : int
{
    IMPACT_ANIMATION_NONE = 0,
    IMPACT_ANIMATION_HIT_RECOVERY = 1,
    IMPACT_ANIMATION_FLOATUP = 2,
    IMPACT_ANIMATION_PUTDOWN = 3,
    IMPACT_ANIMATION_NORMAL_DIE = 4,
    IMPACT_ANIMATION_FLYAWAY_DIE = 5,
    IMPACT_ANIMATION_SMASH_DIE = 99, // reserved by Smash Die, and can not set into Animator
}

[Flags]
public enum ImpactFilterFlag : uint
{
    AnimationFlag = 0x1 << 0,
    MyHitEffect = 0x1 << 1,
    MyCriticalHitEffect = 0x1 << 2,
    MyMaterialFlash = 0x1 << 3,
    MyBloodyEffect = 0x1 << 4,
    MyDeathEffect = 0x1 << 5,
    MyDeathSound = 0x1 << 6,
    MyDeathDisappear = 0x1 << 7,
    MyMaterialHitSound = 0x1 << 8,
    MyCriticalHitSound = 0x1 << 9,
    MyHitScreamSound = 0x1 << 10,
    MySmallBackOffCurve = 0x1 << 11,
    MyBigBackOffCurve = 0x1 << 12,
    MyBackOffDieCurve = 0x1 << 13,
    MyFlyAwayDieCurve = 0x1 << 14,
}

[Flags]
public enum DyingFlag : uint
{
    FadeOutDying = 0x1 << 0,
    SoulOutOfBody = 0x1 << 1,
    Destroyed = 0x1 << 2,
}

public enum ImpactState
{
    CREATE = 0,
    READY = 1,
    RUNNING = 2,
    MASK = 3,       // The Impact which has a "Mask" can only wait for being destroyed.
    DESTROYED = 4,
}

public class ImpactInformation
{
    // Info from skill !
    public ImpactType Impact_Type = ImpactType.IMPACT_NORMAL_HIT; // can only be Impact_Type( 0 - 3 )
    public uint ImpactSkillID = 0;
    public uint SkillCasterUId = 0;
    public bool Critical = false;
    public bool Dead = false;

    // RunTime Data
    public float ImpactDuration = 0.0f;
    public float ImpactStartTime = 0.0f;
    public ImpactState TheImpactState = ImpactState.CREATE;

    public uint AllFilterFlags = 0xFFFFFFFF;

    public void EnableFlag(ImpactFilterFlag FilterFlag, bool Enable = true)
    {
        AllFilterFlags = Enable ? AllFilterFlags | (uint)FilterFlag : AllFilterFlags & ~((uint)FilterFlag);
    }

    public bool IsFlagEnable(ImpactFilterFlag FilterFlag)
    {
        return (AllFilterFlags & (uint)FilterFlag) != 0x00000000;
    }
}

/// <summary>
/// 管理受击时特效及音效的播放，还有击飞击退
/// </summary>
public class ImpactBehaviour : MonoBehaviour
{
    delegate void DisplayHandler(ImpactInformation ImpactInfo);
    Dictionary<ImpactFilterFlag, DisplayHandler> DisplayHandlers = new Dictionary<ImpactFilterFlag, DisplayHandler>();

    SkillBehaviour SkillDispComponent = null;

    BaseActor m_Actor = null;

    MecanimBehaviour _MecanimBehaviour = null;

    ImpactInformation CurImpact = null;
    Vector3 GOPosition = Vector3.one;
    bool SkillDisturbed = false;
    bool ImpactSameReplaced = false;
    //-------------------------------------------------------------------------------//
    // Impact Display from myself.
    // Effect
    ImpactEffect BloodyEffect = new ImpactEffect();
    ImpactEffect HitEffect = new ImpactEffect();
    ImpactEffect CriticalHitEffect = new ImpactEffect();
    ImpactEffect SmashDieEffect = null;
    ImpactDecal BleedDecal = new ImpactDecal();
    ImpactEffect DeadBodyEffect = null;
    // Sound
    ImpactSound CriticalHitSound = new ImpactSound();
    ImpactSound ScreamSound = new ImpactSound();
    ImpactSound DeadBodySound = new ImpactSound();
    ImpactSound DieSound = new ImpactSound();
    uint BodyMaterialID = 0;
    float NormalHitFlyAwayDieRate = 0.0f;
    // Curve
    MotionTrack BigBackOffTrack = new MotionTrack();
    MotionTrack SmallBackOffTrack = new MotionTrack();
    MotionTrack FlyAwayDieTrack = new MotionTrack();
    MotionTrack BackOffDieTrack = new MotionTrack();
    // Flash Color
    Color FlashColor = new Color();

    // how long should I lay on the ground if I died ?
    //float DeadLyingTotalTime = 0.2f;

    // Some valid Impact info
    // Impact Direction 
    Vector3 ImpactDir = new Vector3();
    const float ImpactEffectDuration = 3.0f;
    uint m_UniqueID = 0;
    ActorType m_ActorType = ActorType.UNKNOWN;
    //-------------------------------------------------------------------------------//
    int HashImpact_HitRecovery = 0;
    int HashDeathFlyAway = 0;
    int HashDeathNormal = 0;

    //-------------------------------------------------------------------------------//
    // Special State
    enum ImpactReceiverSpecialState
    {
        NORMAL = 0,
        HITRECOVERY = 1,
        DEAD = 2,
        DYINGONTHEGROUND = 3,
        // PUTDOWN Begin
        PUTDOWN_FALLING = 4,
        PUTDOWN_LYING = 5,
        PUTDOWN_STANDINGUP = 6,
        PUTDOWN_HITRECOVERY = 7,
        // PUTDOWN END

    }

    ImpactReceiverSpecialState CurSpecialState = ImpactReceiverSpecialState.NORMAL;
    //-------------------------------------------------------------------------------//
    uint AllDyingFlag = 0xFFFFFFFF;

    void EnableDyingFlag(DyingFlag Flag, bool Enable = true)
    {
        AllDyingFlag = Enable ? AllDyingFlag | (uint)Flag : AllDyingFlag & ~((uint)Flag);
    }

    bool IsDyingFlagEnable(DyingFlag Flag)
    {
        return (AllDyingFlag & (uint)Flag) != 0x00000000;
    }

    void RegisterAnimationCallBack(MecanimBehaviour _MecanimBehaviour,
                                    MecanimBehaviour.AnimationKey Key)
    {
        _MecanimBehaviour.RegisterBeginCallback(Key, MecanimBeginCallback);
        _MecanimBehaviour.RegisterHaltCallback(Key, MecanimHaltCallback);
        _MecanimBehaviour.RegisterFinishCallback(Key, MecanimFinishCallback);
    }

    void UnRegisterAnimationCallBack(MecanimBehaviour _MecanimBehaviour,
                                    MecanimBehaviour.AnimationKey Key)
    {
        _MecanimBehaviour.UnRegisterBeginCallback(Key, MecanimBeginCallback);
        _MecanimBehaviour.UnRegisterHaltCallback(Key, MecanimHaltCallback);
        _MecanimBehaviour.UnRegisterFinishCallback(Key, MecanimFinishCallback);
    }

    void Start()
    {
        m_Actor = gameObject.GetComponent<BaseActor>();

        m_UniqueID = (uint)m_Actor.GetUniqueID();//gameObject.GetAttribute<uint>( GOAttribute.ACTOR_UNIQUE_ID );

        SkillDispComponent = gameObject.GetComponent<SkillBehaviour>();
        if (null == SkillDispComponent)
        {
            Debug.LogError("Actor must have a SkillDisp Component!");
        }

        _MecanimBehaviour = gameObject.GetComponent<MecanimBehaviour>();

        if (null == _MecanimBehaviour)
        {
            Debug.LogError("Actor must have an FastMecanim!");
        }

        RegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.HIT_RECOVERY_1);
        RegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.HIT_RECOVERY_2);
        RegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.NORMAL_DIE_1);
        RegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.NORMAL_DIE_2);
        RegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.FLYAWAY_DIE_1);
        RegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.FLYAWAY_DIE_2);
        RegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.BACKOFF_DIE_1);
        RegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.BACKOFF_DIE_2);

        RegisterImpactDisplayDelegate();

        // Filter my Dead Action
        m_ActorType = ActorManager.Instance.GetActorType(m_Actor);
        if (ActorType.HERO == m_ActorType)
        {
            EnableDyingFlag(DyingFlag.SoulOutOfBody, false);
        }
    }

    void RegisterImpactDisplayDelegate()
    {
        DisplayHandlers.Add(ImpactFilterFlag.MyHitEffect, DisplayMyHitEffect);
        DisplayHandlers.Add(ImpactFilterFlag.MyCriticalHitEffect, DisplayMyCriticalHitEffect);
        DisplayHandlers.Add(ImpactFilterFlag.MyMaterialFlash, DisplayMyBodyFlash);
        DisplayHandlers.Add(ImpactFilterFlag.MyBloodyEffect, DisplayMyBloodyEffect);
        DisplayHandlers.Add(ImpactFilterFlag.MyDeathEffect, DisplayMyDeathEffect);
        DisplayHandlers.Add(ImpactFilterFlag.MyDeathSound, DisplayMyDeathSound);
        DisplayHandlers.Add(ImpactFilterFlag.MyDeathDisappear, DisplayMyDeathDisappear);
        DisplayHandlers.Add(ImpactFilterFlag.MyMaterialHitSound, DisplayMyMaterialHitSound);
        DisplayHandlers.Add(ImpactFilterFlag.MyCriticalHitSound, DisplayMyCriticalHitSound);
        DisplayHandlers.Add(ImpactFilterFlag.MyHitScreamSound, DisplayMyHitScreamSound);
    }

    #region all Display functions 所有特效音效展示
    //////////////////////////////////////////////////////////////////////////
    // all Display functions
    //播放打击特效
    void DisplayMyHitEffect(ImpactInformation ImpactInfo)
    {
        //GameSys.Get<EffectSys>().CreateWorldPosEffect(HitEffect.Name, EffectBehaviour.GetDummyPointTransform(m_Actor, HitEffect.Point).position,
        //                                                ImpactDir, HitEffect.Scale, ImpactEffectDuration, Quaternion.identity);
    }

    //播放暴击特效
    void DisplayMyCriticalHitEffect(ImpactInformation ImpactInfo)
    {
        //GameSys.Get<EffectSys>().CreateWorldPosEffect(CriticalHitEffect.Name, EffectBehaviour.GetDummyPointTransform( m_Actor, CriticalHitEffect.Point).position,
        //                                                ImpactDir, CriticalHitEffect.Scale, ImpactEffectDuration, Quaternion.identity );
    }

    //播放角色闪光特效
    void DisplayMyBodyFlash(ImpactInformation ImpactInfo)
    {
        //StopCoroutine( "MybodyIsFlashing" );
        //StartCoroutine( "MybodyIsFlashing" );
    }

    //播放流血特效
    void DisplayMyBloodyEffect(ImpactInformation ImpactInfo)
    {
        //GameSys.Get<EffectSys>().CreateWorldPosEffect( BloodyEffect.Name, EffectBehaviour.GetDummyPointTransform( m_Actor, BloodyEffect.Point ).position,
        //                                                ImpactDir, BloodyEffect.Scale, ImpactEffectDuration, Quaternion.identity );
    }

    //播放死亡特效
    void DisplayMyDeathEffect(ImpactInformation ImpactInfo)
    {
        //GameSys.Get<EffectSys>().CreateWorldPosEffect(SmashDieEffect.Name, EffectBehaviour.GetDummyPointTransform(m_Actor, SmashDieEffect.Point).position, 
        //                                                ImpactDir, SmashDieEffect.Scale, ImpactEffectDuration, Quaternion.identity );
    }

    //播放死亡音效
    void DisplayMyDeathSound(ImpactInformation ImpactInfo)
    {
//         if (string.IsNullOrEmpty(DieSound.Name))
//         {
//             //Debuger.LogWarning("DieSound missing, SkillID:" + ImpactInfo.ImpactSkillID);
//             return;
//         }
//         GameSys.Get<AudioSys>().PlaySound(DieSound.Name, GOPosition);
    }

    //死亡后隐藏
    void DisplayMyDeathDisappear(ImpactInformation ImpactInfo)
    {
        SkinnedMeshRenderer[] GORenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        for (uint ui = 0; ui < GORenderers.Length; ++ui)
        {
            GORenderers[ui].enabled = false;
        }
    }

    //播放材质音效
    void DisplayMyMaterialHitSound(ImpactInformation ImpactInfo)
    {
//         wl_res.SkillDataInfo ImpactSkillInfo = GameSys.Get<SkillSys>().GetSkillConfigData(ImpactInfo.ImpactSkillID);
// 
//         if (null != ImpactSkillInfo)
//         {
//             //GameSys.Get<AudioSys>().PlayMaterialSound( ImpactSkillInfo.SkillMaterialID, BodyMaterialID, GOPosition );
//         }
    }

    //播放暴击音效
    void DisplayMyCriticalHitSound(ImpactInformation ImpactInfo)
    {
        if (string.IsNullOrEmpty(CriticalHitSound.Name))
        {
            //Debuger.LogWarning("CriticalHitSound missing, ImpactSkillID:" + ImpactInfo.ImpactSkillID);
            return;
        }
        //GameSys.Get<AudioSys>().PlaySound(CriticalHitSound.Name, GOPosition);
    }

    //播放受击尖叫
    void DisplayMyHitScreamSound(ImpactInformation ImpactInfo)
    {
        if (string.IsNullOrEmpty(ScreamSound.Name))
        {
            //Debuger.LogWarning("HitScreamSound missing, ImpactSkillID:" + ImpactInfo.ImpactSkillID);
            return;
        }
        //GameSys.Get<AudioSys>().PlaySound(ScreamSound.Name, GOPosition);
    }

    //////////////////////////////////////////////////////////////////////////
    #endregion

    //被技能击中，开始受到影响
    public void OnSkillImpacted(BaseActor caster, CastSkillInfo attack_skill_info, SkillImpactResult skill_impact)
    {
        if (null == attack_skill_info || null == caster || null == skill_impact)
        {
            return;
        }

        ImpactInformation NewImpact = new ImpactInformation();
        NewImpact.Dead = skill_impact.bCauseDeath;
        SkillImpactResult.ImpactResult TheImpactResult = skill_impact.Result;
        switch (TheImpactResult)
        {
            case SkillImpactResult.ImpactResult.SR_SIMPLE_INJURED:
                NewImpact.Impact_Type = ImpactType.IMPACT_NORMAL_HIT;
                break;
            case SkillImpactResult.ImpactResult.SR_HIT_RECOVERY:
                NewImpact.Impact_Type = ImpactType.IMPACT_HIT_RECOVERY;
                break;
            case SkillImpactResult.ImpactResult.SR_SMALL_BACKOFF:
                NewImpact.Impact_Type = ImpactType.IMPACT_HIT_RECOVERY_SMALL_BACKOFF;
                break;
            case SkillImpactResult.ImpactResult.SR_HEAVY_BACKOFF:
                NewImpact.Impact_Type = ImpactType.IMPACT_HIT_RECOVERY_BIG_BACKOFF;
                break;
            case SkillImpactResult.ImpactResult.SR_FLOATUP:
                NewImpact.Impact_Type = ImpactType.IMPACT_FLOATUP;
                break;
            case SkillImpactResult.ImpactResult.SR_PUTDOWN:
                NewImpact.Impact_Type = ImpactType.IMPACT_PUTDOWN;
                break;
            case SkillImpactResult.ImpactResult.SR_DROPDOWN:
                NewImpact.Impact_Type = ImpactType.IMPACT_DROPDOWN;
                break;
            default:
                break;
        }

        NewImpact.Critical = skill_impact.bCritImpact;
        NewImpact.ImpactSkillID = attack_skill_info.m_uiSkillId;
        NewImpact.SkillCasterUId = attack_skill_info.CasterUId;

        ReceiveImpact(NewImpact);
    }

    public void ReceiveImpact(ImpactInformation NewImpactInfo)
    {

        if (null != NewImpactInfo)
        {
            //NewImpactInfo.Impact_Type = ImpactType.IMPACT_HIT_RECOVERY;
            if (!IngnoreThisImpact(NewImpactInfo))
            {
                ImpactInfoFilter(NewImpactInfo);

                ProcessImpact(NewImpactInfo);
            }
        }
    }

    private void ProcessImpact(ImpactInformation ImpactInfo)
    {
        if (SkillDisturbed)
        {
            SkillDispComponent.CancelSkill();

            SkillDisturbed = false;
        }

        else if (ImpactSameReplaced)
        {
        }

        BeginImpact(ImpactInfo);
    }

    //开始影响（各种特效音效）
    private void BeginImpact(ImpactInformation ImpactInfo)
    {

        /*Before change, Notify Current Special State End*/
        BeginReceiverSpecialState(ImpactInfo);

        // Begin Impact display except ACTION!
        if (gameObject.activeInHierarchy)
        {
            BeginImpactDisplay(ImpactInfo);
        }
        //////////////////////////////////////////////////////////////////////////
        // Temp for Disable GO
        //////////////////////////////////////////////////////////////////////////
        else if (ImpactInfo.Dead && IsDyingFlagEnable(DyingFlag.Destroyed))
        {
            //销毁角色
            //GameSys.Get<SceneSys>().DestroyActorInCurScene((uint)m_Actor.m_refActorAttribBehaviour.m_ACTOR_UNIQUE_ID);
        }
    }

    //遍历播放受影响时的特效音效
    void BeginImpactDisplay(ImpactInformation ImpactInfo)
    {
        // Calculate impact direction
        ImpactDir = Vector3.zero;
        BaseActor Caster = ActorManager.Instance.GetActor(ImpactInfo.SkillCasterUId);
        if (null != Caster)
        {
            ImpactDir = (gameObject.transform.position - Caster.transform.position);
            ImpactDir.Normalize();
        }

        GOPosition = gameObject.transform.position;

        uint AllFilterFlags = ImpactInfo.AllFilterFlags;

        IEnumerator<KeyValuePair<ImpactFilterFlag, DisplayHandler>> it = DisplayHandlers.GetEnumerator();
        while (it.MoveNext())
        {
            if (0x00000000 != (AllFilterFlags & (uint)(it.Current.Key)))
            {
                it.Current.Value(ImpactInfo);
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // Implement of FastMecanim Callback
    void MecanimBeginCallback(MecanimBehaviour curBehaviour)
    {
        AnimatorStateInfo curAnimatorStateInfo = curBehaviour.GetCurAnimatorStateInfo();

        if (null == CurImpact
            || ImpactState.READY == CurImpact.TheImpactState)
        {
            return;
        }

        CurImpact.ImpactStartTime = Time.time;
        CurImpact.ImpactDuration = curAnimatorStateInfo.length;
        CurImpact.TheImpactState = ImpactState.RUNNING;

        // Where am I?
        GOPosition = gameObject.transform.position;

        BeginImpactTranslate();
    }

    //受击动作播放中断
    void MecanimHaltCallback(MecanimBehaviour curBehaviour)
    {
    }

    //受击动作播放结束
    void MecanimFinishCallback(MecanimBehaviour curBehaviour)
    {
        // Where am I?
        GOPosition = gameObject.transform.position;

        if (CurImpact != null)
        {
            CurImpact.ImpactStartTime = 0.0f;
            CurImpact.TheImpactState = ImpactState.DESTROYED;
            CurImpact = null;
        }

        if (m_Actor.GetActorAttribute().Hp <= 0)
        {
            OnDyingBegin();
        }
    }
    //////////////////////////////////////////////////////////////////////////

    //大击退
    void DoBigBackOff(float duration)
    {
    }

    //小击退
    void DoSmallBackOff(float duration)
    {
    }

    //开始受影响位移
    void BeginImpactTranslate()
    {
        if (CurImpact.IsFlagEnable(ImpactFilterFlag.MySmallBackOffCurve))
        {
            //使用速度+加速度方式测试一下效果,Felix
            DoSmallBackOff(CurImpact.ImpactDuration);
        }
        else if (CurImpact.IsFlagEnable(ImpactFilterFlag.MyBigBackOffCurve))
        {
            //使用速度+加速度方式测试一下效果,Felix
            DoBigBackOff(CurImpact.ImpactDuration);
        }
    }

    private void ImpactInfoFilter(ImpactInformation ImpactInfo)
    {

        ImpactInfo.AllFilterFlags = GetStandardFlag(ImpactInfo.Impact_Type);

        if (ImpactInfo.Critical)
        {
            FilterWithCriticalHit(ImpactInfo);
        }

        FilterBloodyEffect(ImpactInfo);
        FilterWithSkillHitEffectAndSound(ImpactInfo);

        if (ImpactInfo.Dead)
        {
            ConvertImpactIntoDead(ImpactInfo);
        }

        SkillDisturbed = false;
        ImpactSameReplaced = false;

        // Take Buffer into account
        if (false /*如果霸体状态或者免疫某种状态*/)
        {
        }

        if (ImpactInfo.IsFlagEnable(ImpactFilterFlag.AnimationFlag))
        {
            // Take Skill into account
            if (SkillDispComponent.CurSkillId > 0 && ImpactInfo.ImpactSkillID > 0)
            {
                if (ImpactInfo.Dead)
                {
                    SkillDisturbed = true;
                }
                else
                {
                    
                }
            }

            // Take Current Impact into account
            if (ThisImpactIsMasked(ImpactInfo))
            {
                ImpactInfo.EnableFlag(ImpactFilterFlag.AnimationFlag, false);
            }
            else if (null != CurImpact &&
                      (GetImpactAnimatorTransitionID(CurImpact.Impact_Type) ==
                        GetImpactAnimatorTransitionID(ImpactInfo.Impact_Type)))
            {
                if (CurImpact.TheImpactState == ImpactState.RUNNING)
                {
                    ImpactSameReplaced = true;
                }
            }
        }

        if (ImpactInfo.IsFlagEnable(ImpactFilterFlag.AnimationFlag))
        {
            ImpactInfo.TheImpactState = ImpactState.READY;
        }
        else
        {
            ImpactInfo.TheImpactState = ImpactState.MASK;
        }
    }

    //-----------------------------------------------------------------------------
    // Impact base Rules begin
    //-----------------------------------------------------------------------------

    bool IngnoreThisImpact(ImpactInformation ImpactInfo)
    {
        bool Ingnored = true;
        // only Normal and Lying can be impacted right now!
        if (ImpactReceiverSpecialState.NORMAL == CurSpecialState ||
             ImpactReceiverSpecialState.PUTDOWN_LYING == CurSpecialState ||
             ImpactReceiverSpecialState.HITRECOVERY == CurSpecialState)
        {
            Ingnored = false;
        }

        return Ingnored;
    }

    void BeginReceiverSpecialState(ImpactInformation ImpactInfo)
    {
        // Pre-Change State
        if (ImpactInfo.Impact_Type != ImpactType.IMPACT_NORMAL_HIT && null != CurImpact && ImpactState.RUNNING == CurImpact.TheImpactState)
        {
        }

        if (ImpactInfo.Dead)
        {
            CurSpecialState = ImpactReceiverSpecialState.DEAD;
        }
        else if (ImpactType.IMPACT_PUTDOWN == ImpactInfo.Impact_Type && (ImpactReceiverSpecialState.NORMAL == CurSpecialState || ImpactReceiverSpecialState.HITRECOVERY == CurSpecialState))
        {
            CurSpecialState = ImpactReceiverSpecialState.PUTDOWN_FALLING;
        }
        else if (ImpactType.IMPACT_HIT_RECOVERY == ImpactInfo.Impact_Type && (ImpactReceiverSpecialState.NORMAL == CurSpecialState || ImpactReceiverSpecialState.HITRECOVERY == CurSpecialState))
        {
            CurSpecialState = ImpactReceiverSpecialState.HITRECOVERY;
        }
        else if ((ImpactType.IMPACT_HIT_RECOVERY_SMALL_BACKOFF == ImpactInfo.Impact_Type || ImpactType.IMPACT_HIT_RECOVERY_BIG_BACKOFF == ImpactInfo.Impact_Type) && (ImpactReceiverSpecialState.NORMAL == CurSpecialState || ImpactReceiverSpecialState.HITRECOVERY == CurSpecialState))
        {
            CurSpecialState = ImpactReceiverSpecialState.HITRECOVERY;
        }

        // Post Change State
        AdjustReceiverDirection(ImpactInfo);
        CurImpact = ImpactInfo;
    }

    //如果不是普通攻击，则将受击者朝向攻击者
    void AdjustReceiverDirection(ImpactInformation ImpactInfo)
    {
        if (ImpactType.IMPACT_NORMAL_HIT != ImpactInfo.Impact_Type)
        {
            BaseActor Caster = ActorManager.Instance.GetActor(ImpactInfo.SkillCasterUId);
            if (null != Caster)
            {
                Vector3 NewReceiverDir = new Vector3();
                NewReceiverDir = Caster.transform.position - gameObject.transform.position;
                NewReceiverDir.Normalize();
                gameObject.transform.forward = NewReceiverDir;
            }

        }
    }

    void ConvertImpactIntoDead(ImpactInformation NewImpact)
    {
        ImpactType DeadType = NewImpact.Impact_Type;

        switch (NewImpact.Impact_Type)
        {
            case ImpactType.IMPACT_NORMAL_HIT:
            case ImpactType.IMPACT_HIT_RECOVERY:
                DeadType = ImpactType.IMPACT_NORMAL_DIE;
                break;
            case ImpactType.IMPACT_HIT_RECOVERY_BIG_BACKOFF:
            case ImpactType.IMPACT_HIT_RECOVERY_SMALL_BACKOFF:
                NewImpact.EnableFlag(ImpactFilterFlag.MyBigBackOffCurve, false);
                NewImpact.EnableFlag(ImpactFilterFlag.MySmallBackOffCurve, false);
                NewImpact.EnableFlag(ImpactFilterFlag.MyBackOffDieCurve, true);
                DeadType = ImpactType.IMPACT_BACKOFF_DIE;
                break;
            default:
                break;
        }

        if (ImpactType.IMPACT_NORMAL_DIE == DeadType)
        {
            if (null != SmashDieEffect)
            {
                DeadType = ImpactType.IMPACT_SMASH_DIE;
                NewImpact.EnableFlag(ImpactFilterFlag.MyDeathEffect, true);
                // NewImpact.EnableFlag( ImpactFilterFlag.MyDeathEffectSound, true );
                NewImpact.EnableFlag(ImpactFilterFlag.MyDeathDisappear, true);
            }
            else if (NormalHitFlyAwayDieRate >= UnityEngine.Random.Range(0.0f, 1.0f))
            {
                DeadType = ImpactType.IMPACT_FLYAWAY_DIE;
            }
        }

        if (DeadType == ImpactType.IMPACT_FLYAWAY_DIE)
        {
            NewImpact.EnableFlag(ImpactFilterFlag.MyBigBackOffCurve, false);
            NewImpact.EnableFlag(ImpactFilterFlag.MySmallBackOffCurve, false);
            NewImpact.EnableFlag(ImpactFilterFlag.MyBackOffDieCurve, false);
            NewImpact.EnableFlag(ImpactFilterFlag.MyFlyAwayDieCurve, true);
        }

        if (!string.IsNullOrEmpty(DieSound.Name))
        {
            NewImpact.EnableFlag(ImpactFilterFlag.MyDeathSound, true);
        }

        NewImpact.EnableFlag(ImpactFilterFlag.AnimationFlag, true);
        NewImpact.Impact_Type = DeadType;
    }

    uint GetStandardFlag(ImpactType TheImpactType)
    {
        uint StandardFlag = 0x00000000;

        switch (TheImpactType)
        {
            case ImpactType.IMPACT_NORMAL_HIT:
                StandardFlag = (uint)ImpactFilterFlag.MyHitEffect | (uint)ImpactFilterFlag.MyMaterialFlash |
                               (uint)ImpactFilterFlag.MyBloodyEffect | (uint)ImpactFilterFlag.MyMaterialHitSound;
                break;
            case ImpactType.IMPACT_HIT_RECOVERY:
            case ImpactType.IMPACT_PUTDOWN:
            case ImpactType.IMPACT_FLOATUP:
                StandardFlag = (uint)ImpactFilterFlag.AnimationFlag | (uint)ImpactFilterFlag.MyHitEffect | (uint)ImpactFilterFlag.MyBloodyEffect |
                               (uint)ImpactFilterFlag.MyMaterialHitSound | (uint)ImpactFilterFlag.MyHitScreamSound;
                break;

            case ImpactType.IMPACT_HIT_RECOVERY_SMALL_BACKOFF:
                StandardFlag = (uint)ImpactFilterFlag.AnimationFlag | (uint)ImpactFilterFlag.MyHitEffect | (uint)ImpactFilterFlag.MyBloodyEffect |
                               (uint)ImpactFilterFlag.MyMaterialHitSound | (uint)ImpactFilterFlag.MySmallBackOffCurve | (uint)ImpactFilterFlag.MyHitScreamSound;
                break;

            case ImpactType.IMPACT_HIT_RECOVERY_BIG_BACKOFF:
                StandardFlag = (uint)ImpactFilterFlag.AnimationFlag | (uint)ImpactFilterFlag.MyHitEffect | (uint)ImpactFilterFlag.MyBloodyEffect |
                               (uint)ImpactFilterFlag.MyMaterialHitSound | (uint)ImpactFilterFlag.MyBigBackOffCurve | (uint)ImpactFilterFlag.MyHitScreamSound;
                break;
            default:
                break;
        }

        return StandardFlag;
    }

    void FilterWithCriticalHit(ImpactInformation NewImpact)
    {
        NewImpact.EnableFlag(ImpactFilterFlag.MyMaterialHitSound, false);
        NewImpact.EnableFlag(ImpactFilterFlag.MyCriticalHitSound, true);

        NewImpact.EnableFlag(ImpactFilterFlag.MyHitEffect, true);
        //NewImpact.EnableFlag( ImpactFilterFlag.MyCriticalHitEffect, true );
    }

    void FilterBloodyEffect(ImpactInformation ImpactInfo)
    {
        if (ActorType.HERO == m_ActorType /*|| ActorType.PET == MyActorType*/ )
        {
            BaseActor Caster = ActorManager.Instance.GetActor(ImpactInfo.SkillCasterUId);
            if (null != Caster && ActorType.MONSTER == ActorManager.Instance.GetActorType(Caster))
            {
                ImpactInfo.EnableFlag(ImpactFilterFlag.MyBloodyEffect, false);
            }
        }
    }

    void FilterWithSkillHitEffectAndSound(ImpactInformation ImpactInfo)
    {

        //wl_res.SkillDataInfo ImpactSkillInfo = GameSys.Get<SkillSys>().GetSkillConfigData(ImpactInfo.ImpactSkillID);

        //if (null != ImpactSkillInfo)
        //{
        //    if (ImpactSkillInfo.MarkHitEffect > 0)
        //    {
        //        ImpactInfo.EnableFlag(ImpactFilterFlag.MyHitEffect, false);
        //        ImpactInfo.EnableFlag(ImpactFilterFlag.MyCriticalHitEffect, false);
        //    }

        //    if (ImpactSkillInfo.MarkHitSound > 0)
        //    {
        //        ImpactInfo.EnableFlag(ImpactFilterFlag.MyMaterialHitSound, false);
        //        ImpactInfo.EnableFlag(ImpactFilterFlag.MyCriticalHitSound, false);
        //    }
        //}
    }

    //影响的优先级
    uint GetImpactPriority(ImpactType TheImpactType)
    {
        uint PriorityLv = 0;
        switch (TheImpactType)
        {
            case ImpactType.IMPACT_HIT_RECOVERY:
                PriorityLv = 1;
                break;
            case ImpactType.IMPACT_HIT_RECOVERY_SMALL_BACKOFF:
                PriorityLv = 2;
                break;
            case ImpactType.IMPACT_HIT_RECOVERY_BIG_BACKOFF:
                PriorityLv = 3;
                break;
            case ImpactType.IMPACT_FLOATUP:
            case ImpactType.IMPACT_PUTDOWN:
                PriorityLv = 4;
                break;
            default:
                break;
        }

        return PriorityLv;
    }

    MecanimBehaviour.AnimationKey GetImpactAnimatorTransitionID(ImpactType TheImpactType)
    {
        MecanimBehaviour.AnimationKey AnimKey = MecanimBehaviour.AnimationKey.NONE;

        switch (TheImpactType)
        {
            case ImpactType.IMPACT_HIT_RECOVERY:
            case ImpactType.IMPACT_HIT_RECOVERY_BIG_BACKOFF:
            case ImpactType.IMPACT_HIT_RECOVERY_SMALL_BACKOFF:
                AnimKey = MecanimBehaviour.AnimationKey.HIT_RECOVERY_1;
                break;
            case ImpactType.IMPACT_NORMAL_DIE:
            case ImpactType.IMPACT_SMASH_DIE:
                AnimKey = MecanimBehaviour.AnimationKey.NORMAL_DIE_1;
                break;

            //TransitionID = ImpactAnimatorTransitionID.IMPACT_ANIMATION_SMASH_DIE;
            //break;
            case ImpactType.IMPACT_FLYAWAY_DIE:
            case ImpactType.IMPACT_BACKOFF_DIE:

                // 暂时屏蔽 击飞死亡 需要重构
                AnimKey = MecanimBehaviour.AnimationKey.NORMAL_DIE_1;
                //AnimKey = MecanimBehaviour.AnimationKey.FLYAWAY_DIE_1;
                break;
            case ImpactType.IMPACT_FLOATUP:
                //AnimKey = ImpactAnimatorTransitionID.IMPACT_ANIMATION_FLOATUP;
                break;
            case ImpactType.IMPACT_PUTDOWN:
                //AnimKey = ImpactAnimatorTransitionID.IMPACT_ANIMATION_PUTDOWN;
                break;
            default:
                break;
        }

        return AnimKey;
    }

    bool ThisImpactIsMasked(ImpactInformation NewImpact)
    {
        bool IsMarked = false;

        if (null != CurImpact && !CurImpact.Dead)
        {
            if (NewImpact.Dead)
            {
                IsMarked = false;
            }
            else
            {
                uint CurImpactLv = GetImpactPriority(CurImpact.Impact_Type);
                IsMarked = (CurImpactLv < 4 && (CurImpactLv > GetImpactPriority(NewImpact.Impact_Type)));
            }
        }


        return IsMarked;
    }
    //-----------------------------------------------------------------------------
    // Impact base Rule end
    //-----------------------------------------------------------------------------

    //-----------------------------------------------------------------------------
    // Process Special State
    //-----------------------------------------------------------------------------
    //-----------------------------------------------------------------------------//
    // Enter Dying On the Ground



    //----------------------------------------------------------------------//
    // Enter Dying On the Ground
    public void OnDyingBegin()
    {
        StartCoroutine(SorryIAmDyingOnTheGround());
    }

    //受影响后死亡
    IEnumerator SorryIAmDyingOnTheGround()
    {
        //yield return new WaitForSeconds( bull );
        float AlphaTime = 1.0f;

        // Temp code: Although I break my rule, but this is Demand from Colin
        if (null != DeadBodyEffect && IsDyingFlagEnable(DyingFlag.SoulOutOfBody))
        {
            //播放死亡后的特效和音效
        }
        //////////////////////////////////////////////////////////////////////////

        if (IsDyingFlagEnable(DyingFlag.FadeOutDying))
        {
            Vector4 Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            while (AlphaTime >= 0.0f)
            {
                //尸体消失

                AlphaTime -= Time.deltaTime;
                yield return null;
            }
        }

        if (IsDyingFlagEnable(DyingFlag.Destroyed))
        {
            //销毁角色
            //GameSys.Get<SceneSys>().DestroyActorInCurScene(MyUniqueID);
        }
    }
    //----------------------------------------------------------------------------

    void OnDestroy()
    {
        UnRegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.HIT_RECOVERY_1);
        UnRegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.HIT_RECOVERY_2);
        UnRegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.NORMAL_DIE_1);
        UnRegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.NORMAL_DIE_2);
        UnRegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.FLYAWAY_DIE_1);
        UnRegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.FLYAWAY_DIE_2);
        UnRegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.BACKOFF_DIE_1);
        UnRegisterAnimationCallBack(_MecanimBehaviour, MecanimBehaviour.AnimationKey.BACKOFF_DIE_2);
    }
}

public class ImpactEffect
{
    public string Name = null;
    public DummyPoint Point = DummyPoint.DM_BREAST;
    public float Scale = 1.0f;
}

public class ImpactDecal
{
    public string Name = null;
    public float Scale = 1.0f;
}

public class ImpactSound
{
    public string Name = "";
}

public class MotionTrack
{
    public uint XZCurveID = 0;
    public uint YCurveID = 0;
    public float XZDistance = 0.0f;
    public float YHeight = 0.0f;
    public float StartTime = 0.0f;
    public float Duration = 0.0f;
}
