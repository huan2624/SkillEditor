using UnityEngine;
using System.Collections;

public enum BehaviorID
{
    Patrol,     //巡逻
    Chase,      //追逐
    Goback,     //返回
    Attack,     //攻击

    Follow,     //跟随
    Manual,     //手动
    Runaway,    //逃跑
}

public class BaseBehavior : FSMNode {
    
    private BaseActor m_Actor = null;
    private AIDataSet _data = null;
    private BaseActor m_TargetActor = null;

    public BaseBehavior(BehaviorID id, BaseActor actor, AIDataSet data)
        : base((int)id, actor.GetBehaviourMgr())
    {
        m_Actor = actor;
        _data = data;
    }

    public BaseActor GetActor()
    {
        return m_Actor;
    }

    /* 函数说明：获取AI配置 */
    public AIDataSet GetData()
    {
        return _data;
    }

    public BaseActor TargetActor
    {
        set { m_TargetActor = value; }
        get { return m_TargetActor; }
    }
}
