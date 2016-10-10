using UnityEngine;
using System.Collections;

public class StateIdle : BaseState
{
    public StateIdle(StateID id, BaseActor actor)
            : base(id, actor)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        BaseActor attacker = GetActor();

        attacker.GetMecanim().PlayAnimation(MecanimBehaviour.AnimationKey.IDLE_1);
    }

    public override void OnEnterAgain()
    {
        base.OnEnterAgain();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Transition()
    {
        base.Transition();
    }
}
