using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorBaseAttribute {

    // UID 客户端本地生成的ID
    public uint ACTOR_UNIQUE_ID = 0;

    public bool IsDead = false;

    public ActorGroup ActorGroup = ActorGroup.UnKnown;

    public ActorType ActorType = ActorType.UNKNOWN;

    public int Hp = 10;

    public Dictionary<DummyPoint, Transform> ActorDummyPointDic;
}
