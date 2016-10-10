using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillParam
{
    public Vector3 m_ApproachPos = Vector3.zero;
}

public class ImpactInfo
{
    //作用次数
    public int m_nImpactTimes = 0;
    //检测时间
    public float m_fLatestImpactTime = 0.0f;
}

public class SkillTargetInfo
{
    public Vector3 m_vPosition = Vector3.zero;
    public Vector3 m_vForward = Vector3.zero;
}

public class SkillDispInfo
{
    public float m_fCreateTime = 0.0f;
    public CastSkillInfo m_CurSkillInfo;
}

/// <summary>
/// 当前施放的技能信息
/// </summary>
public class CastSkillInfo {
    //客户端生成的唯一id
    public uint UniqueId { get; private set; }

    public uint m_uiSkillId = 0;

    public int m_nSkillDispId = 0;

    //技能展示数据（技能编辑器中配置的）
    public SkillDispData m_DispData = null;

    public wl_res.SkillDataInfo m_SkillDataInfo = null;

    public SkillParam m_SkillParam = null;

    private BaseActor m_Caster = null;

    public uint CasterUId { private set; get; }
    
    /// <summary>
    /// 施法者
    /// </summary>
    public BaseActor Caster
    {
        set
        {
            m_Caster = value;
            CasterUId = (uint)m_Caster.GetActorAttribute().ACTOR_UNIQUE_ID;
        }
        get
        {
            if (m_Caster != null)
                return m_Caster;

            if (CasterUId != 0)
            {
                return ActorManager.Instance.GetActor(CasterUId);
            }

            return null;
        }
    }

    //施法者原位置
    public Vector3 CasterOriginalPos { get; set;}
    //施法事件管理器
    SkillEventManager m_DispEventManager = null;
    public SkillEventManager DispEventManager
    {
        get { return m_DispEventManager; }
    }

    public uint SelectTargetId = 0;    //玩家或者AI选择的指定目标
    //视野范围内的目标列表
    public List<uint> SkillTargets = new List<uint>();
    public void SetupTargets(ref List<uint> lstTargets)
    {
        SkillTargets.Clear();
        if (lstTargets == null)
        {
            return;
        }

        IEnumerator<uint> it = (IEnumerator<uint>)lstTargets.GetEnumerator();
        while (it.MoveNext())
        {
            uint TargetID = it.Current;
            SkillTargets.Add(TargetID);
        }
    }

    //存储检测目标的位置和朝向
    public SkillTargetInfo SkillTargetInfo = null;

    //存储碰撞或范围检测到的目标()
    public List<uint> HitTargets = new List<uint>();
    //存储作用检测
    public Dictionary<uint, ImpactInfo> ImpactInfoDic = new Dictionary<uint, ImpactInfo>();

    public float StartupTime = 0.0f;    //技能开始时间
    public float TimeScale = 1.0f;	// 匹配动作播放速度 两者要保持一致 才能够让技能在准确的关键帧触发对应的事件
    public float CurAnimTotalTime = 0f; //当前动作的动画时长
    public float CurAnimStartTime = 0f;//当前动作开始的时间点

    public CastSkillInfo()
    {
        UniqueId = CommHelper.GenerateGOID();
        m_DispEventManager = new SkillEventManager(this);
    }
}
