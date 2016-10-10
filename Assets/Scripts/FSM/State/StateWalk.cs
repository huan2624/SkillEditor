using UnityEngine;
using System.Collections;

// 路点信息
public class WayPoint
{
    // 到达坐标
    public Vector3 point;
    // 移动速度
    public float speed;
}

public class StateWalk : BaseState
{
    private NavMeshAgent m_NavMeshAgent;
    private WayPoint m_NextWayPoint;
    private bool m_IsJoystick;
    private float _trunSpeed = 10.0f;

    public StateWalk(StateID id, BaseActor actor)
            : base(id, actor)
    {
        InitNavMeshAgent();
    }

    //停止摇杆
    public void StopJoystick()
    {
        if (m_IsJoystick)
        {
            LeaveState();
        }
    }

    // 设置目标点（使用默认速度行走）
    public void SetPathPoint(Vector3 worldPoint, bool isJoystick = false)
    {
        m_IsJoystick = isJoystick;
        WayPoint wayPoint = new WayPoint();
        wayPoint.point = worldPoint;
        wayPoint.speed = 5;

        m_NextWayPoint = wayPoint;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        BaseActor attacker = GetActor();

        // 设置目的地
        m_NavMeshAgent.updatePosition = true;
        m_NavMeshAgent.speed = m_NextWayPoint.speed;
        m_NavMeshAgent.acceleration = m_NextWayPoint.speed * 10f;
        m_NavMeshAgent.SetDestination(m_NextWayPoint.point);

        // 播放行走动作
        MecanimBehaviour mecanimBehaviour = attacker.GetMecanim();
        if (mecanimBehaviour && mecanimBehaviour.GetLastPlayAnimationKey() != MecanimBehaviour.AnimationKey.MOVE)
        {
            attacker.GetMecanim().PlayAnimation(MecanimBehaviour.AnimationKey.MOVE);
        } 
    }

    public override void OnEnterAgain()
    {
        base.OnEnterAgain();

        if (m_NavMeshAgent.enabled)
        {
            // 设置目的地
            m_NavMeshAgent.updatePosition = true;
            m_NavMeshAgent.speed = m_NextWayPoint.speed;
            m_NavMeshAgent.acceleration = m_NextWayPoint.speed * 10f;
            m_NavMeshAgent.SetDestination(m_NextWayPoint.point);
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();

        if (m_NavMeshAgent.enabled)
        {
            m_NavMeshAgent.Stop();
            m_NavMeshAgent.ResetPath();
        }
    }

    public override void Update()
    {
        base.Update();

        if (!IsCanMove())
        {
            LeaveState();
            return;
        }

        // 转身
        Vector3 newRotation;
        if (m_IsJoystick)
        {
            newRotation = m_NextWayPoint.point - GetActor().transform.position;
        }
        else
        {
            newRotation = m_NavMeshAgent.steeringTarget - GetActor().transform.position;
        }
        if (newRotation.sqrMagnitude >= 0.05f)
        {
            newRotation = Quaternion.LookRotation(newRotation).eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;
            GetActor().transform.rotation = Quaternion.Slerp(GetActor().transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * _trunSpeed);
        }

        float moveSpeed = m_NextWayPoint.speed;
        float acceleration = moveSpeed * 10.0f;
        if (m_NavMeshAgent.pathPending)
        {
            return;
        }

        // 判断是否走到目标点
        if (m_NavMeshAgent.remainingDistance < 0.2)
        {
            if (!m_IsJoystick)
            {
                LeaveState();
            }
            return;
        }

        // 播放行走动作
        //MecanimBehaviour mecanimBehaviour = GetActor().GetMecanim();
        //if (mecanimBehaviour)
        //{
        //    if (mecanimBehaviour.GetLastPlayAnimationKey() != MecanimBehaviour.AnimationKey.MOVE)
        //    {
        //        float animSpeed = 1;
        //        mecanimBehaviour.PlayAnimation(MecanimBehaviour.AnimationKey.MOVE, true, animSpeed);
        //    }
        //    if (mecanimBehaviour.IsInTransition())
        //    {
        //        m_NavMeshAgent.Stop();
        //        return;
        //    }
        //}

        //m_NavMeshAgent.Resume();
        //m_NavMeshAgent.speed = moveSpeed;
        //m_NavMeshAgent.acceleration = acceleration;
    }

    public override void Transition()
    {
        base.Transition();
    }

    bool IsCanMove()
    {
        if (m_NavMeshAgent == null)
        {
            return false;
        }

        if (!m_NavMeshAgent.enabled)
        {
            return false;
        }

        // 中了无法移动的buf

        return true;
    }

    #region 初始化NavMeshAgent
    bool InitNavMeshAgent()
    {
        if (m_NavMeshAgent != null)
        {
            return true;
        }

        m_NavMeshAgent = GetActor().gameObject.GetComponent<NavMeshAgent>();
        if (m_NavMeshAgent == null)
        {
            m_NavMeshAgent = GetActor().gameObject.AddComponent<NavMeshAgent>();
            m_NavMeshAgent.areaMask = -1;
        }
        m_NavMeshAgent.updatePosition = false;
        m_NavMeshAgent.updateRotation = false;
        m_NavMeshAgent.autoBraking = true;
        m_NavMeshAgent.stoppingDistance = 0.1f;
        m_NavMeshAgent.autoRepath = false;
        m_NavMeshAgent.baseOffset = -0.02f;
        m_NavMeshAgent.radius = GetActor().GetBodyRadius();
        ////m_NavMeshAgent.height = GetActor().GetBodyHeight();
        m_NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        return true;
    }
    #endregion
}
