using UnityEngine;
using System.Collections;

public enum StateID
{
    Idle,       //待机
    Walk,       //行走
    Attack,     //原有的技能攻击
    Qte,        //组合技

    HitNormal,  //普通受击
    HitBack,    //击退
    HitFly,     //击飞
    HitRush,    //冲撞受击
    Dead,       //死亡
}

public class BaseState : FSMNode {
    private BaseActor m_Actor = null;

    public BaseState(int id, BaseActor actor)
            : base(id, actor.GetStateMgr())
        {
        m_Actor = actor;
    }

    public BaseState(StateID id, BaseActor actor)
            : base((int)id, actor.GetStateMgr())
        {
        m_Actor = actor;
    }

    public BaseActor GetActor()
    {
        return m_Actor;
    }
}
