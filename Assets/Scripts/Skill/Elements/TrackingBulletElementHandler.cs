using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackingBulletElementHandler : BaseSkillElementHandler
{
	TrackingBulletElement m_BulletElement = null;

	public class BulletInstance
	{
		public GameObject m_goBulletEffect = null;
		public Vector3 m_curPos = new Vector3();
		public Vector3 m_AimPos = new Vector3();
		public float m_fTotalTranslate = 0.0f; // 上一帧的飞行距离
		public BaseActor m_Target;

        public int              m_EjectionNum = 0;								// 弹射次数
        public float            m_EjectionDistance = 0.0f;						// 弹射距离
        public HashSet<uint>    m_EjectionedUIDSet = new HashSet<uint>();       // 被弹射过的单位的列表 
		public bool				m_IsOverEjectioned = true;						// 是否可以回弹

		public bool InitBulletInstance(TrackingBulletElement elem, CastSkillInfo SkillInfo, BaseActor Caster, BaseActor Target)
		{

			Transform trans = EffectBehaviour.GetDummyPointTransform(Caster, elem.m_BulletEffect.m_Pos);
			m_curPos = EffectBehaviour.GetEffectPos(trans, elem.m_BulletEffect.Offset, elem.m_BulletEffect.m_OffsetDistance);

			if (Target == null)
			{
				return false;
			}

			m_Target = Target;

			if ( SkillInfo.m_SkillDataInfo != null )
			{
				m_EjectionNum = SkillInfo.m_SkillDataInfo.EjectionedNum;
				m_EjectionDistance = SkillInfo.m_SkillDataInfo.EjectionedDistance;
				m_IsOverEjectioned = SkillInfo.m_SkillDataInfo.IsOverEjectioned != 0;

				// 如果是普攻的话 弹射效果受Buff影响加成
				//if (Caster.IsNormalAttackSkill(SkillInfo.m_SkillDataInfo.SkillID))
				//{
				//	EjectionInfo refEjectionInfo = Caster.m_refActorAttribBehaviour.m_EjectionInfo;

				//	m_EjectionNum += refEjectionInfo.m_NewEjectionedNum;
				//	m_EjectionDistance += refEjectionInfo.m_EjectionDistance;
				//	m_IsOverEjectioned |= refEjectionInfo.m_IsOverEjectioned;
				//}
			}

			if (!string.IsNullOrEmpty(elem.m_BulletEffect.m_Name))
			{
				string strEffectPath = TrackingBulletElementHandler.GetEffectPath(elem.m_BulletEffect.m_Name);

				Transform AimTrans = EffectBehaviour.GetDummyPointTransform(m_Target,
																	elem.m_TargetPos);

				m_AimPos = AimTrans.position;
				Vector3 vDir = m_curPos - m_AimPos;

				m_goBulletEffect = EffectManager.Instance.CreateWorldPosEffect(strEffectPath,
					m_curPos, vDir, elem.m_BulletEffect.m_Scale,
					Mathf.Infinity, elem.m_BulletEffect.Rotation);

#if UNITY_EDITOR
				if (m_goBulletEffect == null)
				{
                    Debug.LogError("Fail to create bullet effect: " + strEffectPath);
				}
#endif

				m_goBulletEffect.transform.forward = vDir;
			}

			return true;
		}
	}

	List<BulletInstance> m_lstBulletInstances = null;


	public bool Setup(TrackingBulletElement bullet_element)
	{
		if (bullet_element == null || bullet_element.m_StartupEvent == null)
		{
			return false;
		}

		if ( bullet_element.m_SpeedInfo.m_Speed <= 0.0f )
		{
			return false;
		}

		m_BulletElement = bullet_element;

		RegisterEventHandler(m_BulletElement.m_StartupEvent, /*m_BulletElement.m_TerminateEvent*/
			new SkillDispEvent(SkillDispEventType.DISPEVT_NONE));

		return true;
	}

	public override bool Startup(SkillDispEvent evt)
	{

		BaseActor Caster = m_CurSkillInfo.Caster;
		if (Caster == null)
		{
			return false;
		}

		if (m_lstBulletInstances == null)
		{
			m_lstBulletInstances = new List<BulletInstance>();
		}

        //攻击范围
        float fMaxRange = 100000.0f;
        if (m_CurSkillInfo.m_SkillDataInfo != null)
        {
            fMaxRange = m_CurSkillInfo.m_SkillDataInfo.MaxRange + Caster.GetBodyRadius();
        }

        //目标数
		int TargetNum = 0;
		if (m_CurSkillInfo.SelectTargetId != 0)
		{
			BaseActor Target = ActorManager.Instance.GetActor(m_CurSkillInfo.SelectTargetId);
			if (Target != null)
			{
                fMaxRange += Target.GetBodyRadius();
                //如果目标在攻击范围内，则实例化一个子弹
				if ((Target.transform.position - Caster.transform.position).sqrMagnitude < fMaxRange*fMaxRange)
				{
					BulletInstance bullet_inst = new BulletInstance();
					bullet_inst.InitBulletInstance(m_BulletElement, m_CurSkillInfo, Caster, Target);
					m_lstBulletInstances.Add(bullet_inst);
					++TargetNum;
				}
			}
		}

        //最大子弹数
		int MaxBulletCount = m_BulletElement.m_MaxBulletCount;
		if (m_CurSkillInfo.m_SkillDataInfo != null)
		{
			if (m_CurSkillInfo.m_SkillDataInfo.MaxTargetNum != 0)
			{
				MaxBulletCount = m_CurSkillInfo.m_SkillDataInfo.MaxTargetNum;
			}
		}

		//if (Caster.IsNormalAttackSkill(m_CurSkillInfo.m_uiSkillId))
		//{
		//	MaxBulletCount += Caster.GetBuffStateNum(eActorBuffEffectState.ABE_MAX_TARGET_NUM);
		//}
		
		for (int i = 0; TargetNum < MaxBulletCount && i < m_CurSkillInfo.SkillTargets.Count; ++i)
		{
			if (m_CurSkillInfo.SelectTargetId == m_CurSkillInfo.SkillTargets[i])
			{
				continue;
			}

			BaseActor Target = ActorManager.Instance.GetActor(m_CurSkillInfo.SkillTargets[i]);
			if (Target == null)
			{
				continue;
			}

			if ((Target.transform.position - Caster.transform.position).sqrMagnitude > fMaxRange * fMaxRange)
			{
				continue;
			}

			BulletInstance bullet_inst = new BulletInstance();

			Transform trans = EffectBehaviour.GetDummyPointTransform(Caster, m_BulletElement.m_BulletEffect.m_Pos);
			bullet_inst.m_curPos = EffectBehaviour.GetEffectPos(trans, m_BulletElement.m_BulletEffect.Offset, m_BulletElement.m_BulletEffect.m_OffsetDistance);

			bullet_inst.InitBulletInstance(m_BulletElement, m_CurSkillInfo, Caster, Target);

			m_lstBulletInstances.Add(bullet_inst);

			++TargetNum;

		}

	//	m_fLastShootTestTime = 0.0f;

		//Debuger.Log ("Start Bullet Time: " + m_BulletStartFlyTime);
		return base.Startup(evt);
	}

	public override void Terminate(SkillDispEvent evt)
	{
		KillBullet();
		base.Terminate(evt);
	}

	protected void KillBullet()
	{
		if (m_lstBulletInstances == null)
		{
			return;
		}

		IEnumerator<BulletInstance> it = (IEnumerator<BulletInstance>)m_lstBulletInstances.GetEnumerator();
		while (it.MoveNext())
		{
			KillBullet(it.Current);
		}

		m_lstBulletInstances.Clear();
	}

	protected void KillBullet(BulletInstance bullet_inst)
	{
		if (bullet_inst == null)
		{
			return;
		}

		if (null != bullet_inst.m_goBulletEffect)
		{
			EffectManager.Instance.DestroyEffect(bullet_inst.m_goBulletEffect);
		}
	}

	public override bool Update(float fDeltaTime)
	{
		if (StartupTime <= 0.0f)
		{
			return true; // Not startup yet!
		}

		base.Update(fDeltaTime);

		fDeltaTime = ElemEclipsedTime;
		
		for (int i = 0; i < m_lstBulletInstances.Count; )
		{
			if (UpdateBullet(m_lstBulletInstances[i], fDeltaTime))
			{
				++i;
			}
			else
			{
				KillBullet(m_lstBulletInstances[i]);
				m_lstBulletInstances[i] = null;
				m_lstBulletInstances.RemoveAt(i);
			}
		}

		return (m_lstBulletInstances.Count > 0);
	}

	public List<BulletInstance> GetBulletInstance()
	{
		return m_lstBulletInstances;
	}

    bool ResetAim(BulletInstance bullet_inst)
    {
        --bullet_inst.m_EjectionNum;

		float fNearestDistanceSqrMagnitude = Mathf.Infinity;
		bool IsFind = false;

		BaseActor OldTarget = bullet_inst.m_Target;

		List<BaseActor> m_EjectionedNearList = new List<BaseActor>(bullet_inst.m_EjectionedUIDSet.Count);

		// 第一次搜寻没有被弹射过 并且在合理范围内的目标
// 		Dictionary<ActorGroup, HashSet<ActorBehaviour>>.Enumerator ActorGroupTableEnumerator = GameSys.Get<ActorManager>().m_ActorGroupTable.GetEnumerator();
// 
// 		while (ActorGroupTableEnumerator.MoveNext())
// 		{
// 
// 		}

		List<uint> EnemyList = ActorManager.Instance.BuildEnemyListInScene(m_CurSkillInfo.Caster);

		for (int i = 0; i < EnemyList.Count; ++i)
        {
			uint UID = EnemyList[i];

			BaseActor Target = ActorManager.Instance.GetActor(UID);
            if (Target == null)
            {
                continue;
            }

			if (OldTarget == Target)
			{
				continue;
			}

			if (Target.IsDead())
			{
				continue;
			}

			if (bullet_inst.m_EjectionedUIDSet.Contains(UID) && bullet_inst.m_IsOverEjectioned)
			{
				m_EjectionedNearList.Add(Target);
				continue;
			}

			float DistanceSqrMagnitude = (bullet_inst.m_curPos - Target.transform.position).sqrMagnitude;
			if (DistanceSqrMagnitude > bullet_inst.m_EjectionDistance * bullet_inst.m_EjectionDistance)
            {
                continue;
            }

			if (DistanceSqrMagnitude < fNearestDistanceSqrMagnitude)
			{
				bullet_inst.m_Target = Target;
				fNearestDistanceSqrMagnitude = DistanceSqrMagnitude;
				IsFind = true;
			}
        }

		// 弹射目标没有找到 那么寻找在范围内弹射过的 但是在弹射范围内的单位
		if (!IsFind && bullet_inst.m_IsOverEjectioned)
		{
			for ( int i = 0; i < m_EjectionedNearList.Count; ++i)
			{
				BaseActor Target = m_EjectionedNearList[i];

				float DistanceSqrMagnitude = (bullet_inst.m_curPos - Target.transform.position).sqrMagnitude;
				if (DistanceSqrMagnitude > bullet_inst.m_EjectionDistance * bullet_inst.m_EjectionDistance)
				{
					continue;
				}

				if (DistanceSqrMagnitude < fNearestDistanceSqrMagnitude)
				{
					bullet_inst.m_Target = Target;
					fNearestDistanceSqrMagnitude = DistanceSqrMagnitude;
					IsFind = true;
				}
			}
		}

		return IsFind;
    }

	protected bool UpdateBullet(BulletInstance bullet_inst, float fDeltaTime)
	{
		if (bullet_inst == null)
		{
			return false;
		}

		// 放在这里更新位置 如果目标被销毁 那么子弹依旧会飞到目标所在的位置 否则更新子弹的目标位置
		if (bullet_inst.m_Target != null)
		{
			Transform AimTrans = bullet_inst.m_Target.transform;
			AimTrans = EffectBehaviour.GetDummyPointTransform(bullet_inst.m_Target, 
																	m_BulletElement.m_TargetPos);

			bullet_inst.m_AimPos = EffectBehaviour.GetEffectPos(AimTrans, 
												m_BulletElement.m_BulletEffect.Offset, 
												m_BulletElement.m_BulletEffect.m_OffsetDistance);
		}

		if (null != bullet_inst.m_goBulletEffect)
		{
			float fTotalTranslate = m_BulletElement.m_SpeedInfo.Calculate(fDeltaTime);

			float CurTranslate = fTotalTranslate - bullet_inst.m_fTotalTranslate;

			Vector3 Forward = bullet_inst.m_AimPos - bullet_inst.m_curPos;
			bullet_inst.m_goBulletEffect.transform.forward = Forward;

			float ForwardMagnitude = Forward.magnitude;

			Vector3 vec3Distance = Vector3.zero;
			vec3Distance.x = CurTranslate * Forward.x / ForwardMagnitude;
			vec3Distance.y = CurTranslate * Forward.y / ForwardMagnitude;
			vec3Distance.z = CurTranslate * Forward.z / ForwardMagnitude;

			bullet_inst.m_goBulletEffect.transform.position += vec3Distance;

			bullet_inst.m_curPos = bullet_inst.m_goBulletEffect.transform.position;

			bullet_inst.m_fTotalTranslate = fTotalTranslate;
		}

		if ((bullet_inst.m_curPos - bullet_inst.m_AimPos).sqrMagnitude <= m_BulletElement.m_fHitDistance)
		{
			// 如果目标已经销毁 那么直接结束删除子弹就行了
			if (bullet_inst.m_Target == null)
			{
				return false;
			}

			m_CurSkillInfo.HitTargets.Add( (uint)bullet_inst.m_Target.GetUniqueID());

            if (bullet_inst.m_EjectionedUIDSet != null)
            {
                bullet_inst.m_EjectionedUIDSet.Add( (uint)bullet_inst.m_Target.GetUniqueID() );
            }

			OnHitTargets(m_BulletElement.m_ImpactData);

            if (bullet_inst.m_EjectionNum == 0)
            {
                return false; // OK, enough
            }

            return ResetAim(bullet_inst);
		}

		return true;
	}
}