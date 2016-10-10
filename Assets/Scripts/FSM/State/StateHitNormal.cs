using UnityEngine;
using System.Collections;

public class StateHitNormal : BaseState
{
    private float _runTime = 0;

    public StateHitNormal(StateID id, BaseActor actor)
            : base(id, actor)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        BaseActor attacker = GetActor();

        attacker.GetMecanim().RegisterFinishCallback(MecanimBehaviour.AnimationKey.HIT_RECOVERY_1, OnAnimOver);
        attacker.GetMecanim().PlayAnimation(MecanimBehaviour.AnimationKey.HIT_RECOVERY_1);

        _runTime = 0;
    }

    public override void OnEnterAgain()
    {
        base.OnEnterAgain();
    }

    public override void OnLeave()
    {
        base.OnLeave();

        BaseActor attacker = GetActor();

        attacker.GetMecanim().UnRegisterFinishCallback(MecanimBehaviour.AnimationKey.HIT_RECOVERY_1, OnAnimOver);
    }

    public override void Update()
    {
        base.Update();

        // 超时退出【容错处理】
        _runTime += Time.deltaTime;
        if (_runTime > 1.5f)
        {
            LeaveState();
            return;
        }
    }

    public override void Transition()
    {
        base.Transition();
    }

    void OnAnimOver(MecanimBehaviour animMgr)
    {
        LeaveState();
    }
}
