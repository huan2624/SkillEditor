//------------------------------------------------------------------------------
// 技能展示数据，游戏中也是使用此数据
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[System.Serializable]
public enum DecalType
{
    DecalType_Additive = 0,
    DecalType_Alphainv = 1,
}

[System.Serializable]
public struct TrailSection
{
    [StaticCaption("位置")]
    public Vector3 point0;
    [StaticCaption("方向")]
    public Vector3 point1;
    [StaticCaption("时间")]
    public float time;
    public TrailSection(Vector3 p0, Vector3 p1, float t)
    {
        point0 = p0;
        point1 = p1;
        time = t;
    }
}

[System.Serializable]
public enum SectionType
{
    [StaticCaption("吟唱阶段")]
    SEC_TYPE_PREPARE,  // ??
    [StaticCaption("施放阶段")]
    SEC_TYPE_PERFORM,
    [StaticCaption("表现阶段")]
    SEC_TYPE_SHOW,  // ???
    [StaticCaption("影响阶段")]
    SEC_TYPE_IMPACT,
}

#region Event Definitions 各种技能触发事件定义
[System.Serializable]
public enum SkillDispEventType
{
    [StaticCaption("没有事件触发")]
    DISPEVT_NONE = 0,
    [StaticCaption("定时触发")]
    DISPEVT_TIMEPOINT_TRIGGER,
    [StaticCaption("吟唱开始时触发")]
    DISPEVT_PREPARE_BEGIN,
    [StaticCaption("吟唱结束时触发")]
    DISPEVT_PREPARE_END,
    [StaticCaption("ShootPoint触发")]
    DISPEVT_ON_SHOOT_POINT,
    [StaticCaption("命中目标时触发")]
    DISPEVT_ON_SHOOT_TARGET,
    [StaticCaption("施放阶段开始时触发")]
    DISPEVT_PERFORM_BEGIN,
    [StaticCaption("施放阶段结束时触发")]
    DISPEVT_PERFORM_END,
    [StaticCaption("影响阶段开始时触发")]
    DISPEVT_IMPACT_BEGIN,
    [StaticCaption("影响阶段结束时触发")]
    DISPEVT_IMPACT_END,
    [StaticCaption("表现阶段开始时触发")]
    DISPEVT_SHOW_BEGIN,
    [StaticCaption("表现阶段结束时触发")]
    DISPEVT_SHOW_END,
    [StaticCaption("程序触发事件")]
    DISPEVT_PROGRAM_CUSTOM,
}

[System.Serializable]
public enum CustomEventFlag
{
    [StaticCaption("非法类型")]
    Invalid = 0,
    [StaticCaption("随机攻击区域")]
    RandomImpactPos = 1,
    [StaticCaption("炸弹落地爆炸")]
    Bomb_Explosion = 2,
}

[System.Serializable]
public class SkillDispEvent
{
    [StaticCaption("事件类型")]
    public SkillDispEventType m_EventType = SkillDispEventType.DISPEVT_NONE;
    [StaticCaption("自定义事件标志")]
    public CustomEventFlag m_CustomFlag = CustomEventFlag.Invalid; // 当事件类型为PROGRAM_CUSTOM时，这个参数是标志
    [StaticCaption("浮点参数")]
    public float m_Param1 = 0.0f;
    [StaticCaption("整型参数")]
    public int m_Param2 = 0;
    [StaticCaption("字符串参数")]
    public string m_Param3 = "";

    public SkillDispEvent()
    {
    }

    public SkillDispEvent(SkillDispEventType EventType)
    {
        m_EventType = EventType;
    }

    public SkillDispEvent(SkillDispEventType EventType, float Param1)
    {
        m_EventType = EventType;
        m_Param1 = Param1;
    }

    public SkillDispEvent(SkillDispEventType EventType, float Param1, int Param2)
    {
        m_EventType = EventType;
        m_Param1 = Param1;
        m_Param2 = Param2;
    }

    public SkillDispEvent(SkillDispEventType EventType, float Param1, int Param2, string Param3)
    {
        m_EventType = EventType;
        m_Param1 = Param1;
        m_Param2 = Param2;
        m_Param3 = Param3;
    }
}

// NOTICE: This class is not used for serialize
public class CustomSkillDispEvent : SkillDispEvent
{
    public Vector3 m_Position;
}
#endregion

#region Impact Definitions 各种受击类型

[System.Serializable]
public enum SkillImpactType
{
    [StaticCaption("普通受伤")]
    SIMPLE_INJURED = 0, // 普通受伤
    [StaticCaption("小击退")]
    SMALL_BACK_OFF,     // 小击退
    [StaticCaption("大击退")]
    HEAVY_BACK_OFF,     // 大击退
    [StaticCaption("浮空击落")]
    FLOAT_DROPDOWN,     // 浮空击落
}

[System.Serializable]
public class SkillImpactData
{
    [StaticCaption("受击类型")]
    public SkillImpactType m_ImpactType;
    [StaticCaption("伤害系数")]
    public float m_ImpactFactor = 1.0f;
}

#endregion

#region Element Definitions 各种表现元素定义
[System.Serializable]
public enum EffectMode
{
    [StaticCaption("世界固定位置")]
    EFT_ON_WORLD_POS = 0,
    [StaticCaption("施法者位置")]
    EFT_ON_CASTER_POS = 1,
    [StaticCaption("受击对象位置")]
    EFT_ON_TARGET_POS = 2,
    [StaticCaption("跟随施法者")]
    EFT_FOLLOW_CASTER = 3,
    [StaticCaption("跟随受击对象")]
    EFT_FOLLOW_TARGET = 4,
    [StaticCaption("技能锁定目标位置")]
    EFT_ON_SKILLTARGET_POS = 5,
    [StaticCaption("跟随技能锁定目标")]
    EFT_FOLLOW_SKILLTARGET = 6,
}

[System.Serializable]
public enum EffectDir
{
    [StaticCaption("特效原有方向")]
    EFT_DIR_ORIGINAL = 0,
    [StaticCaption("目标角色的朝向")]
    EFT_DIR_BINDING_TARGET = 1,
    [StaticCaption("施法者的朝向")]
    EFT_DIR_SKILL_CASTER = 2,
    [StaticCaption("目标角色朝向的反方向")]
    EFT_DIR_INV_BINDING_TARGET = 3,
    [StaticCaption("施放者至目标角色朝向")]
    EFT_DIR_CASTER_TO_TARGET = 4,
    [StaticCaption("目标角色至施放者朝向")]
    EFT_DIR_TARGET_TO_CASTER = 5,
}

[System.Serializable]
public enum ChangePosType
{
    [StaticCaption("拉回")]
    CPT_PULL_BACK,
    [StaticCaption("移形换位")]
    CPT_SWITCH_POS,
    [StaticCaption("投掷")]
    CPT_THROW,
}

[System.Serializable]
public class EffectInfo
{
    [StaticCaption("资源名")]
    [SelectFile(FileType.OT_PREFABS)]
    public string m_Name = "";
    [StaticCaption("位置类型")]
    public EffectMode m_Mode;
    [StaticCaption("方向信息")]
    public EffectDir m_Dir;
    [StaticCaption("大米点")]
    public DummyPoint m_Pos;
    [StaticCaption("偏移量X")]
    public float m_OffsetX = 0.0f;
    [StaticCaption("偏移量Y")]
    public float m_OffsetY = 0.0f;
    [StaticCaption("偏移量Z")]
    public float m_OffsetZ = 0.0f;
    [StaticCaption("偏移量")]
    public float m_OffsetDistance = 0.0f;
    [StaticCaption("缩放比例")]
    public float m_Scale = 1.0f;
    [StaticCaption("持续时间")]
    public float m_Duration = 1.0f;
    [StaticCaption("旋转角度X")]
    public float m_AngleX = 0.0f;
    [StaticCaption("旋转角度Y")]
    public float m_AngleY = 0.0f;
    [StaticCaption("旋转角度Z")]
    public float m_AngleZ = 0.0f;
    [StaticCaption("是否跟随大米点旋转")]
    public bool m_SyncRotation = false;

    public Vector3 Offset
    {
        get { return new Vector3(m_OffsetX, m_OffsetY, m_OffsetZ); }
    }
    public Quaternion Rotation
    {
        get { return Quaternion.Euler(m_AngleX, m_AngleY, m_AngleZ); }
    }
    public Vector3 ReviseVector(Vector3 v)
    {
        return Rotation * v;
    }
}

[System.Serializable]
public class SimEffectInfo
{
    [StaticCaption("资源名")]
    [SelectFile(FileType.OT_PREFABS)]
    public string m_Name = "";
    [StaticCaption("大米点")]
    public DummyPoint m_Pos;
    [StaticCaption("偏移量X")]
    public float m_OffsetX = 0.0f;
    [StaticCaption("偏移量Y")]
    public float m_OffsetY = 0.0f;
    [StaticCaption("偏移量Z")]
    public float m_OffsetZ = 0.0f;
    [StaticCaption("偏移量")]
    public float m_OffsetDistance = 0.0f;
    [StaticCaption("缩放比例")]
    public float m_Scale = 1.0f;
    [StaticCaption("旋转角度X")]
    public float m_AngleX = 0.0f;
    [StaticCaption("旋转角度Y")]
    public float m_AngleY = 0.0f;
    [StaticCaption("旋转角度Z")]
    public float m_AngleZ = 0.0f;

    public Vector3 Offset
    {
        get { return new Vector3(m_OffsetX, m_OffsetY, m_OffsetZ); }
    }
    public Quaternion Rotation
    {
        get { return Quaternion.Euler(m_AngleX, m_AngleY, m_AngleZ); }
    }
    public Vector3 ReviseVector(Vector3 v)
    {
        return Rotation * v;
    }
}

[System.Serializable]
public class EffectPosInfo
{
    [StaticCaption("大米点")]
    public DummyPoint m_BeginPos;
    [StaticCaption("偏移量X")]
    public float m_OffsetX = 0.0f;
    [StaticCaption("偏移量Y")]
    public float m_OffsetY = 0.0f;
    [StaticCaption("偏移量Z")]
    public float m_OffsetZ = 0.0f;
    [StaticCaption("偏移量")]
    public float m_OffsetDistance = 0.0f;
    [StaticCaption("缩放比例")]
    public float m_Scale = 1.0f;
    [StaticCaption("旋转角度X")]
    public float m_AngleX = 0.0f;
    [StaticCaption("旋转角度Y")]
    public float m_AngleY = 0.0f;
    [StaticCaption("旋转角度Z")]
    public float m_AngleZ = 0.0f;

    public Vector3 Offset
    {
        get { return new Vector3(m_OffsetX, m_OffsetY, m_OffsetZ); }
    }
    public Quaternion Rotation
    {
        get { return Quaternion.Euler(m_AngleX, m_AngleY, m_AngleZ); }
    }
    public Vector3 ReviseVector(Vector3 v)
    {
        return Rotation * v;
    }
}

[System.Serializable]
public class RibbonEffectInfo
{
    [StaticCaption("资源名")]
    [SelectFile(FileType.OT_PREFABS)]
    public string m_Name = "";

    [StaticCaption("开始点")]
    public EffectPosInfo m_BeginPos;

    [StaticCaption("结束点")]
    public EffectPosInfo m_EndPos;

}

[System.Serializable]
public class DispElementBase
{
    [StaticCaption("触发开始的事件")]
    public SkillDispEvent m_StartupEvent = new SkillDispEvent();    // The event to activate this element
    [StaticCaption("触发结束的事件")]
    public SkillDispEvent m_TerminateEvent = new SkillDispEvent();  // The event to deactivate this element
    [StaticCaption("结束时触发的事件")]
    public SkillDispEvent m_OutputEvent = new SkillDispEvent();     // The event produced by this element
}

[System.Serializable]
[StaticCaption("特效元素")]
public class EffectElement : DispElementBase
{
    [StaticCaption("特效详细信息")]
    public EffectInfo m_EffectInfo;
}

[System.Serializable]
[StaticCaption("音效信息")]
public class AudioInfo
{
    [StaticCaption("音效位置")]
    public EffectMode m_Mode;
    [StaticCaption("资源名")]
    //[SelectFile(FileType.OT_FMOD_EVENT)]
    [SelectFile(FileType.OT_AUDIO_CLIP)]
    public string m_EventPath = "";
    //[StaticCaption("老资源名(已废除)")]
    //[SelectFile(FileType.OT_AUDIO_CLIP)]
    //public string m_Name = "";
    [StaticCaption("播放概率")]
    public float m_PlayRate = 1.0f;
}

[System.Serializable]
[StaticCaption("音效元素")]
public class AudioElement : DispElementBase
{
    [StaticCaption("音效信息")]
    public AudioInfo m_AudioInfo;
}


[System.Serializable]
public class AttackRange
{
    [StaticCaption("扇形在前方多远")]
    public float m_Distance = 0.0f;
    [StaticCaption("扇形内半径")]
    public float m_RadiusIn = 0.1f;
    [StaticCaption("扇形外半径")]
    public float m_RadiusOut = 3.0f;
    [StaticCaption("扇形高度")]
    public float m_Height = 2.0f;
    [StaticCaption("扇形角度")]
    public float m_Angle = 120.0f;
    [StaticCaption("扇形朝向偏转")]
    public float m_ForwardDelta = 0.0f;
    [StaticCaption("扇形离地距离")]
    public float m_VOffset = 0.0f;
}

[System.Serializable]
public enum ShootTestBase
{
    [StaticCaption("基于施法者")]
    BASE_CASTER,
    [StaticCaption("基于目标角色")]
    BASE_TARGET,
}

[System.Serializable]
public enum ShootTestType
{
    [StaticCaption("范围检测")]
    SHOOTTEST_RANGE,
    [StaticCaption("胶囊体碰撞检测")]
    SHOOTTEST_CAPSULE_COLLISION,
}

[System.Serializable]
public class ShootTestParam
{
    [StaticCaption("碰撞检测基础")]
    public ShootTestBase m_ShootTestBase = ShootTestBase.BASE_CASTER;
    [Omit]
    public string m_RangeId = "";
    [StaticCaption("扇形范围")]
    public AttackRange m_Range = new AttackRange();
    [StaticCaption("同一对象最大受击次数")]
    public int m_MaxImpactTimes = 1;
    [StaticCaption("最大击中次数")]
    public int m_MaxShootCount = 10;
    [StaticCaption("受击时间间隔")]
    public float m_ImpactInterval = 0.0f;
    [StaticCaption("碰撞检测类型")]
    public ShootTestType m_ShootTestType = ShootTestType.SHOOTTEST_CAPSULE_COLLISION;
}

[System.Serializable]
[StaticCaption("顿刀信息")]
public class ShootTestInfo
{
    [StaticCaption("隔多久检测一次")]
    public float m_Interval = 0.0f;
    [StaticCaption("检测总时长")]
    public float m_TotalTime = 0.0f;
    [StaticCaption("顿刀时长")]
    public float m_PauseTime = 0.0f;
    [StaticCaption("检测参数")]
    public ShootTestParam m_ShootTestParam = new ShootTestParam();
}

[System.Serializable]
[StaticCaption("顿刀元素")]
public class ShootTestElement : DispElementBase
{
    public ShootTestInfo m_ShootTestInfo;
}

[System.Serializable]
public class WeaponTrailInfo
{
    [StaticCaption("资源名")]
    [SelectFile(FileType.OT_PREFABS)]
    public string m_PrefabName = "Assets/Resources/Effect/WeaponTrail/WeaponTrail.prefab";
    [StaticCaption("绑定点")]
    public string m_BindPos = "Bip001 Prop1";
    [StaticCaption("颜色")]
    public Color m_Color = new Color(0.667f, 0.667f, 0.667f, 1);
    [StaticCaption("距离")]
    public float m_Distance = 0.4f;
    [StaticCaption("宽度")]
    public float m_Length = 1.6f;
    [StaticCaption("持续时间(秒)")]
    public float m_Time = 0.15f;
    [StaticCaption("刀光数据")]
    [FixedArray]
    [Omit]
    public TrailSection[] m_TrailData;
}

[System.Serializable]
[StaticCaption("刀光元素")]
public class WeaponTrailElement : DispElementBase
{
    [StaticCaption("刀光信息")]
    public WeaponTrailInfo m_WeaponTrailInfo = new WeaponTrailInfo();
}

[System.Serializable]
public enum TranslateMethod
{
    [StaticCaption("直线")]
    TRANS_STRAIGHT_LINE = 0, // Straight line
    [StaticCaption("抛物线")]
    TRANS_PARA_CURVE,   // Para curve
}

[System.Serializable]
public enum TranslateTarget
{
    [StaticCaption("施法者")]
    TRANS_CASTER = 0, // Impact caster
    [StaticCaption("受击者")]
    TRANS_TARGET,   // Impact target
    [StaticCaption("施法者和受击者")]
    TRANS_BOTH,   // Impact both caster and target
}

[System.Serializable]
public enum TranslateDir
{
    [StaticCaption("向前")]
    TRANS_FORWARD = 0,
    [StaticCaption("向后")]
    TRANS_BACKWARD,
    [StaticCaption("浮空")]
    TRANS_PULLUP,
    [StaticCaption("向目标接近")]
    TRANS_APPROACH,
    [StaticCaption("向目标方向")]
    TRANS_POINT_TO_TARGET,
}

[System.Serializable]
public class TranslateInfo
{
    [StaticCaption("移动目标")]
    public TranslateTarget m_Target = TranslateTarget.TRANS_CASTER;
    [StaticCaption("移动轨迹")]
    public TranslateMethod m_Method = TranslateMethod.TRANS_STRAIGHT_LINE;
    [StaticCaption("移动方向")]
    public TranslateDir m_Direction = TranslateDir.TRANS_FORWARD;
    [StaticCaption("移动距离")]
    public float m_Distance = 0.0f;
    [StaticCaption("移动高度")]
    public float m_Height = 0.0f;
    [StaticCaption("移动速度")]
    public float m_Speed = 0.0f;
    [StaticCaption("加速度")]
    public float m_Accel = 0.0f;
    [StaticCaption("加速度的加速度")]
    public float m_AccelEx = 0.0f;
}

[System.Serializable]
[StaticCaption("位移元素")]
public class TranslateElement : DispElementBase
{
    [StaticCaption("位移信息")]
    public TranslateInfo m_TranslateInfo;
}

[System.Serializable]
public class CurveTranslateInfo
{
    [StaticCaption("移动目标")]
    public TranslateTarget m_Target = TranslateTarget.TRANS_CASTER;
    [StaticCaption("移动轨迹")]
    public AnimationCurve m_Curve;
    [StaticCaption("移动方向")]
    public TranslateDir m_Direction = TranslateDir.TRANS_FORWARD;
    [StaticCaption("移动距离")]
    public float m_Distance = 0.0f;
    [StaticCaption("移动时间")]
    public float m_TranslateTime = 0.0f;
}

[System.Serializable]
[StaticCaption("曲线位移元素")]
public class CurveTranslateElement : DispElementBase
{
    [StaticCaption("曲线位移信息")]
    public CurveTranslateInfo m_CurveTranslateInfo;
}

[System.Serializable]
public class SpeedInfo
{
    [StaticCaption("初速度")]
    public float m_Speed = 0.0f;
    [StaticCaption("加速度")]
    public float m_Accel = 0.0f;
    [StaticCaption("加速度的加速度")]
    public float m_AccelEx = 0.0f;

    public float Calculate(float fTime)
    {
        float fTranslate = m_Speed * fTime;

        if (m_Accel != 0.0f)
        {
            fTranslate += (m_Accel * fTime * fTime * 0.5f);
        }

        if (m_AccelEx != 0.0f)
        {
            fTranslate += (m_AccelEx * fTime * fTime * fTime * 0.333333f);
        }

        return fTranslate;
    }
}

[System.Serializable]
public class BulletInfo
{
    [StaticCaption("子弹效果")]
    public SimEffectInfo m_BulletEffect = new SimEffectInfo();
    [StaticCaption("射出角度范围")]
    public float m_BulletAngleRange = 0.0f;
    [StaticCaption("射出子弹个数")]
    public int m_BulletCount = 1;
    [StaticCaption("长度")]
    public float m_BulletLength = 0.0f;
    [StaticCaption("半径")]
    public float m_BulletRadius = 0.0f;
    [StaticCaption("飞行距离")]
    public float m_FlyDistance = 0.0f;
    [StaticCaption("飞行速度信息")]
    public SpeedInfo m_SpeedInfo = new SpeedInfo();
    [StaticCaption("子弹碰撞检测的间隔")]
    public float m_ShootTestInterval = 0.0f;
    [StaticCaption("击中效果")]
    public SkillImpactData m_ImpactData = new SkillImpactData();
    [StaticCaption("同一对象最大受击次数")]
    public int m_MaxImpactTimes = 1;
    [StaticCaption("最大击中次数")]
    public int m_MaxShootCount = 10;
}

[System.Serializable]
[StaticCaption("子弹元素")]
public class BulletElement : DispElementBase
{
    [StaticCaption("子弹信息")]
    public BulletInfo m_BulletInfo;
}

// 鉴于原来的子弹其实都是群体伤害 而且无法锁定目标 所以做了追踪弹的元素
// 其命中方式就是只管飞到距离目标位置规定的距离 就算命中了
[System.Serializable]
[StaticCaption("追踪弹元素")]
public class TrackingBulletElement : DispElementBase
{
    [StaticCaption("子弹效果")]
    public SimEffectInfo m_BulletEffect = new SimEffectInfo();

    [StaticCaption("飞行速度信息")]
    public SpeedInfo m_SpeedInfo = new SpeedInfo();

    [StaticCaption("击中效果")]
    public SkillImpactData m_ImpactData = new SkillImpactData();

    [StaticCaption("击中目标绑定点")]
    public DummyPoint m_TargetPos;

    [StaticCaption("击中目标距离绑定点的距离")]
    public float m_fHitDistance = 0.1f;

    [StaticCaption("最大子弹数")]
    public int m_MaxBulletCount = 1;
}

[System.Serializable]
[StaticCaption("闪现元素")]
public class BlinkElement : DispElementBase
{
    [StaticCaption("原地特效")]
    public SimEffectInfo m_CurPosEffect = new SimEffectInfo();

    [StaticCaption("是否瞬间移动")]
    public bool m_IsBlink = true;

    [StaticCaption("是否锁定目标")]
    public bool m_IsLockTarget = true;

    [StaticCaption("闪现速度信息")]
    public SpeedInfo m_SpeedInfo = new SpeedInfo();

    [StaticCaption("目标位置偏移距离")]
    public float m_AimOffset = 1.0f;

    [StaticCaption("飞行击中效果")]
    public SkillImpactData m_ImpactData = new SkillImpactData();
}

[System.Serializable]
[StaticCaption("持续施法元素")]
public class ContinuousCastElement : DispElementBase
{
    [StaticCaption("伤害检测参数")]
    public ShootTestParam m_ShootTestParam = new ShootTestParam();

    [StaticCaption("伤害检测间隔 单位秒")]
    public float m_ShootTestTimeDelta = 1.0f;

    [StaticCaption("最长持续时间 单位秒")]
    public float m_MaxDurTime = 15.0f;

    [StaticCaption("伤害表现")]
    public SkillImpactData m_ImpactData = new SkillImpactData();
}

[System.Serializable]
[StaticCaption("改变角色位置元素")]
public class ChangePosElement : DispElementBase
{
    [StaticCaption("原地特效")]
    public SimEffectInfo m_CurPosEffect = new SimEffectInfo();

    [StaticCaption("改变位置")]
    ChangePosType Type = ChangePosType.CPT_SWITCH_POS;

    [StaticCaption("位移水平方向速度信息")]
    public SpeedInfo m_SpeedInfo = new SpeedInfo();
}

[System.Serializable]
[StaticCaption("带子元素")]
public class RibbonElement : DispElementBase
{
    [StaticCaption("带子信息")]
    public RibbonEffectInfo m_RibbonEffect = new RibbonEffectInfo();

    [StaticCaption("飞行速度信息")]
    public SpeedInfo m_SpeedInfo = new SpeedInfo();

    [StaticCaption("击中效果")]
    public SkillImpactData m_ImpactData = new SkillImpactData();

    [StaticCaption("最大带子数")]
    public int m_MaxRibbonCount = 1;
}

[System.Serializable]
public class CurveBulletInfo
{
    [StaticCaption("炮弹效果")]
    public SimEffectInfo m_BulletEffect = new SimEffectInfo();
    [StaticCaption("是否锁定目标")]
    public bool m_LockTarget = false;
    [StaticCaption("射出角度范围")]
    public float m_BulletAngleRange = 0.0f;
    [StaticCaption("射出炮弹个数")]
    public int m_BulletCount = 1;
    [StaticCaption("长度")]
    public float m_BulletLength = 0.0f;
    [StaticCaption("半径")]
    public float m_BulletRadius = 0.0f;
    [StaticCaption("飞行时间")]
    public float m_TotalTime = 0.0f;
    [StaticCaption("炮弹飞行最大距离")]
    public float m_FlyDistance = 0.0f;
    [StaticCaption("炮弹水平位置变化的曲线")]
    public AnimationCurve m_HorzCurve;
    [StaticCaption("炮弹飞行最大高度")]
    public float m_MaxHeight = 0.0f;
    [StaticCaption("炮弹垂直高度变化的曲线")]
    public AnimationCurve m_VertCurve;
    [StaticCaption("伤害检测参数")]
    public ShootTestParam m_ShootTestParam = new ShootTestParam();
    [StaticCaption("击中效果")]
    public SkillImpactData m_ImpactData = new SkillImpactData();
}

[System.Serializable]
[StaticCaption("曲线炮弹元素")]
public class CurveBulletElement : DispElementBase
{
    [StaticCaption("曲线炮弹信息")]
    public CurveBulletInfo m_CurveBulletInfo;
}

[System.Serializable]
public class VirtualBulletInfo
{
    [StaticCaption("子弹碰撞检测的间隔")]
    public float m_ShootTestInterval = 0.0f;
    [StaticCaption("子弹击中效果")]
    public SkillImpactData m_ImpactData = new SkillImpactData();
    [StaticCaption("同一对象最大受击次数")]
    public int m_MaxImpactTimes = 1;
    [StaticCaption("最大击中次数")]
    public int m_MaxShootCount = 10;
}

[System.Serializable]
[StaticCaption("虚拟子弹元素")]
public class VirtualBulletElement : DispElementBase
{
    [StaticCaption("虚拟子弹信息")]
    public VirtualBulletInfo m_VirtualBulletInfo;
}

[System.Serializable]
public class ShowAttackRangeConfig
{
    [StaticCaption("是否显示攻击区域")]
    public bool m_Show = false;

    [StaticCaption("开始时间【归一化】")]
    public float m_BeginTime = 0.0f;

    [StaticCaption("结束时间【归一化】")]
    public float m_EndTime = 0.0f;

    [StaticCaption("垂直方向上的偏移")]
    public float m_VertOffset = 0.1f;
}

[System.Serializable]
public class ShootPointElement : DispElementBase
{
    [StaticCaption("伤害检测参数")]
    public ShootTestParam m_ShootTestParam = new ShootTestParam();
    [StaticCaption("显示攻击区域配置")]
    public ShowAttackRangeConfig m_ShowAtkRangeCfg;
    [StaticCaption("伤害表现")]
    public SkillImpactData m_ImpactData = new SkillImpactData();
    [StaticCaption("是否可被打断")]
    public bool m_bBreakable = true;
}

[System.Serializable]
public class DirectImpactElement : DispElementBase
{
    [StaticCaption("伤害表现")]
    public SkillImpactData m_ImpactData = new SkillImpactData();
}

[System.Serializable]
public class ShowPointElement : DispElementBase
{
    [StaticCaption("是否做伤害检测")]
    public bool m_bDoShootTest = true;
    [StaticCaption("伤害检测参数")]
    public ShootTestParam m_ShootTestParam = new ShootTestParam();
    [StaticCaption("是否显示攻击区域")]
    public bool m_ShowAttackRange = false;
    [StaticCaption("表现索引")]
    public int m_ShowIndex = 0;
}

[System.Serializable]
public class RelativePos
{
    [StaticCaption("与角色正朝向夹角")]
    public float m_Angel;

    [StaticCaption("与角色之间的距离")]
    public float m_Distance;
}

[System.Serializable]
public enum SummonType
{
    [StaticCaption("怪物")]
    MONSTER,
    //     [StaticCaption("宠物")]
    //     PET,
}

[System.Serializable]
public class SummonNPCElement : DispElementBase
{
    [StaticCaption("召唤类型")]
    public SummonType m_SummonType = SummonType.MONSTER;

    [StaticCaption("角色ID")]
    public int m_RoleId;

    [StaticCaption("召唤数量")]
    public int m_Count;

    [StaticCaption("可选出生位置")]
    [ShowEmptyArray]
    public RelativePos[] m_BornPoses;

    [StaticCaption("出生动画序号")]
    public int m_nBornAnimIndex;

    [StaticCaption("出生动画时间")]
    public float m_fBornAnimTime;

    [StaticCaption("出生特效")]
    public SimEffectInfo m_BornEffect;

    [StaticCaption("特效时长")]
    public float m_EffectDuration;
}

[System.Serializable]
public class RandomPosImpactElement : DispElementBase
{
    [StaticCaption("位置数量")]
    public int m_Count;

    [StaticCaption("可选攻击位置")]
    [ShowEmptyArray]
    public AttackRange[] m_Ranges;

    [StaticCaption("是否显示攻击区域")]
    public bool m_ShowAttackRange = false;

    [StaticCaption("伤害表现")]
    public SkillImpactData m_ImpactData = new SkillImpactData();

    [StaticCaption("同一对象最大受击次数")]
    public int m_MaxImpactTimes = 1;
}

[System.Serializable]
public class TargetFlashParam
{
    [StaticCaption("颜色R分量")]
    public float Color_R = 1.0f;
    [StaticCaption("颜色G分量")]
    public float Color_G = 1.0f;
    [StaticCaption("颜色B分量")]
    public float Color_B = 1.0f;
    [StaticCaption("Alpha值")]
    public float Color_A = 1.0f;
    [StaticCaption("变色时长")]
    public float m_Duration;
}

[System.Serializable]
public class TargetFlashElement : DispElementBase
{
    public TargetFlashParam m_TargetFlashParam = new TargetFlashParam();
}

[System.Serializable]
public enum CameraShakeType
{
    [StaticCaption("Y轴方向小震动")]
    YAXES_SMALL_LOOP = 0,
    [StaticCaption("Y轴方向大震动")]
    YAXES_BIG_LOOP = 1,
    [StaticCaption("视觉方向小缩放")]
    ZOOM_SMALL_LOOP = 2,
    [StaticCaption("视觉方向大缩放")]
    ZOOM_BIG_LOOP = 3,
    [StaticCaption("视觉方向缩放一次")]
    ZOOM_ONCE = 4,
}

[System.Serializable]
public class CameraShakeParam
{
    [StaticCaption("震屏类型")]
    public CameraShakeType m_ShakeType;
    [StaticCaption("震动时长")]
    public float m_ShakeTime = 0.25f;
}

[System.Serializable]
public class CameraShakeElement : DispElementBase
{
    public CameraShakeParam m_CameraShakeParam = new CameraShakeParam();
}

[System.Serializable]
public class DecalParam
{
    [StaticCaption("贴花材质")]
    [SelectFile(FileType.OT_PNG)]
    public string m_Texture = "";
    [StaticCaption("贴花时长")]
    public float m_Duartion = 1;
    [StaticCaption("贴花透明度")]
    public float m_Transparency = 1;
    //[StaticCaption("贴花淡入时间")]
    //public float m_FadeInTime;
    [StaticCaption("贴花淡出时间")]
    public float m_FadeOutTime = 1;
    [StaticCaption("贴花大小")]
    public float m_Scale = 1;
    [StaticCaption("贴花随机旋转")]
    public bool m_RandRotate = false;
    [StaticCaption("贴花闪烁最大透明度")]
    public float m_BlinkMaxAlpha = 1;
    [StaticCaption("贴花闪烁最小透明度")]
    public float m_BlinkMinAlpha = 1;
    [StaticCaption("贴花闪烁频率")]
    public float m_BlinkRate = 0;
    [StaticCaption("贴花左右偏移")]
    public float m_LRShift = 0;
    [StaticCaption("贴花前后偏移")]
    public float m_FBShift = 0;
    [StaticCaption("贴花初始旋转")]
    public float m_Rotate = 0;
    [StaticCaption("贴花旋转速度")]
    public float m_RotateSpeed;
    [StaticCaption("贴花模式")]
    public EffectMode m_Mode;
    [StaticCaption("贴花模式")]
    public DecalType m_Type;
    [StaticCaption("贴花闪烁最大缩放")]
    public float m_ScaleBlinkmax = 1;
    [StaticCaption("贴花闪烁最小缩放")]
    public float m_ScaleBlinkmin = 1;
    [StaticCaption("贴花闪烁缩放频率")]
    public float m_ScaleBlinkRate = 0;
}

[System.Serializable]
public class DecalElement : DispElementBase
{
    public DecalParam m_DecalParam = new DecalParam();
}

[System.Serializable]
public class AppTimeScaleElement : DispElementBase
{
    [StaticCaption("持续时长")]
    public float m_Duration = 0.5f;

    [StaticCaption("游戏速度")]
    public float m_TimeScale = 1.0f;
}

[System.Serializable]
public enum ActionPointType
{
    [StaticCaption("动作后摇开始")]
    ActionClosureBegin = 0,
}

[System.Serializable]
public class ActionPointElement : DispElementBase
{
    [StaticCaption("断点类型")]
    public ActionPointType m_Type = ActionPointType.ActionClosureBegin;
}

#endregion


//所有展示元素的集合
[System.Serializable]
public class DispInfo
{
    public EffectElement[] m_EffectElements;
    public AudioElement[] m_AudioElements;
    public ShootTestElement[] m_ShootTestElements;
    public WeaponTrailElement[] m_WeaponTrailElements;
    public TranslateElement[] m_TranslateElements;
    public BulletElement[] m_BulletElements;
    public ShootPointElement[] m_ShootPointElements;
    public CurveTranslateElement[] m_CurveTranslateElements;
    public TargetFlashElement[] m_TargetFlashElements;
    public DecalElement[] m_DecalElements;
    public CameraShakeElement[] m_CameraShakeElements;
    public VirtualBulletElement[] m_VirtualBulletElements;
    public CurveBulletElement[] m_CurveBulletElements;
    public ShowPointElement[] m_ShowPointElements;
    public DirectImpactElement[] m_DirectImpactElements;
    public SummonNPCElement[] m_SummonNPCElements;
    public RandomPosImpactElement[] m_RandomPosImpactElements;
    public AppTimeScaleElement[] m_AppTimeScaleElements;
    public ActionPointElement[] m_ActionPointElements;
    public TrackingBulletElement[] m_TrackingBulletElements;
    public BlinkElement[] m_BlinkElements;
    public ChangePosElement[] m_ChangePosElements;
    public ContinuousCastElement[] m_ContinuousCastElements;
}

//[System.Serializable][StaticCaption("降落参数")]
//public class LandingParam
//{
//    [StaticCaption("降落垂直速度")]
//    public float m_SpeedVert = 0.0f;
//    [StaticCaption("降落水平速度")]
//    public float m_SpeedHorz = 0.0f;
//    [StaticCaption("加速度")]
//    public float m_Accel = 0.0f;
//    [StaticCaption("加速度的加速度")]
//    public float m_AccelEx = 0.0f;

//    public bool IsValid()
//    {
//        if (Mathf.Abs(m_SpeedVert) > 0.1f)
//            return true;

//        if (Mathf.Abs(m_Accel) > 0.1f)
//            return true;

//        if (Mathf.Abs(m_AccelEx) > 0.1f)
//            return true;

//        return false;
//    }
//}

//[System.Serializable][StaticCaption("受击坠落参数")]
//public class DropDownParam
//{
//    [StaticCaption("降落垂直速度")]
//    public float m_SpeedVert = 0.0f;
//    [StaticCaption("降落水平速度")]
//    public float m_SpeedHorz = 0.0f;
//    [StaticCaption("加速度")]
//    public float m_Accel = 0.0f;
//    [StaticCaption("加速度的加速度")]
//    public float m_AccelEx = 0.0f;
//    [StaticCaption("离地高度")]
//    public float m_OffsetHeight = 1.0f; // The height from ground

//    public bool IsValid()
//    {
//        if (Mathf.Abs(m_SpeedVert) > 0.1f)
//            return true;

//        if (Mathf.Abs(m_Accel) > 0.1f)
//            return true;

//        if (Mathf.Abs(m_AccelEx) > 0.1f)
//            return true;

//        return false;
//    }
//}

[System.Serializable]
public enum SkillShowType
{
    SKILL_SHOW_SIMPLE,  // 简单表现
}

[System.Serializable]
public class SkillPrepareData
{
    public string m_StateName;
    public int m_NameHash = 0;
    public int m_AnimationKey = 0;
    public DispInfo m_DispInfo;
}

[System.Serializable]
public class SkillPerformData
{
    public string m_StateName;
    public int m_NameHash = 0;
    public int m_AnimationKey = 0;
    public DispInfo m_DispInfo;
}

[System.Serializable]
public class SkillShowData
{
    public SkillShowType m_SkillShowType = SkillShowType.SKILL_SHOW_SIMPLE;
    public string m_StateName;
    public int m_NameHash = 0;
    public int m_AnimationKey = 0;
    public float m_fTotalTime = 1.0f;
    public DispInfo m_DispInfo;
}

//技能所有数据
[System.Serializable]
public class SkillDispData
{
    //吟唱阶段
    public SkillPrepareData[] m_PrepareDatas;
    //施放阶段
    public SkillPerformData[] m_PerformDatas;
    //表现阶段
    public SkillShowData[] m_ShowDatas;
}
