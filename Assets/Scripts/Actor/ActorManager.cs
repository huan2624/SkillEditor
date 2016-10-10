using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ActorType
{
    UNKNOWN = 0,
    HERO,
    //TEAM_MEMBER,
    MONSTER,
    SCENE_OBJ,                  // 场景内可破坏物件（可破坏可点选）
    SCENE_SKILLDESTROY_OBJ,     // 场景内技能可破坏物件(可破坏不可点选)
    VIRSUAL_OBJ,
    NONATTACK_OBJ,              // 不可攻击的中立物体
    SHOW_HERO,
    //SHOW_TEAM_MEMBER,
    SHOW_MONSTER,
    ENEMY_PLAYER,
    MAXTYPECOUNT,
}

// 角色阵营
public enum ActorGroup
{
    UnKnown,
    Friend,             //本人的英雄或者怪物
    Enemy,              //敌方的英雄或者怪物
    Neutral,            //中立怪物，本人或者敌方都会攻击它
    ActorGroupCount,
}

public enum ActorAttribu
{
    AA_HP,              // 当前生命值
    AA_MP,              // 当前魔法值
    AA_HPMAX,           // 生命最大值
    AA_MPMAX,           // 魔法最大值
    AA_RUNSPEED,        // 移动速度
    AA_STRENGTH,        // 力量
    AA_AGILITY,         // 敏捷
    AA_INTELLECT,       // 智力
    AA_COUNT,
};

public enum ActorScaleMode
{
    UNSCALE,                // 不缩放
    PROPERTY_SCALE,         // 属性表内缩放
    RESOURCE_SCALE,         // 资源表内缩放
}

public class ActorManager : TSingleton<ActorManager> {

    private Dictionary<uint, BaseActor> m_ActorPool = new Dictionary<uint, BaseActor>();

    public void RegistActor(BaseActor actor)
    {
        m_ActorPool[actor.GetActorAttribute().ACTOR_UNIQUE_ID] = actor;
    }

    public BaseActor GetActor(uint UID)
    {
        BaseActor refActorBehaviour = null;

        if (!m_ActorPool.TryGetValue(UID, out refActorBehaviour))
        {
            return null;
        }

        return refActorBehaviour;
    }

    public ActorType GetActorType(BaseActor actor)
    {
        if (null == actor)
        {
            return ActorType.UNKNOWN;
        }

        return actor.GetActorAttribute().ActorType;
    }

    public List<uint> BuildEnemyListInScene(BaseActor Actor)
    {
        List<uint> enemys = new List<uint>();
        return enemys;
    }
}
