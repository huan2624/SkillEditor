using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FanDispElementHandler : BaseSkillElementHandler
{
    private AttackRange m_AttackRange;

    private GameObject m_FanDispObject;

    private CFanMesh m_FanMesh;

	ShootTestBase m_eShootTestBase;

    private float m_fVertOffset = 0.1f;

    private const int Section_Num = 8 * 2;

	public bool Setup(AttackRange attack_range, float fBeginTime, float fEndTime, float fVertOffset, ShootTestBase eShootTestBase)
    {
        if (attack_range == null || attack_range.m_Angle < 0.1f
            || attack_range.m_RadiusIn < 0.0f
            || attack_range.m_RadiusIn >= attack_range.m_RadiusOut
            || fBeginTime < 0.0f || fBeginTime >= fEndTime)
        {
            return false;
        }

        m_AttackRange = attack_range;

        m_fVertOffset = fVertOffset;

		m_eShootTestBase = eShootTestBase;

        RegisterEventHandler(
            new SkillDispEvent(SkillDispEventType.DISPEVT_TIMEPOINT_TRIGGER, fBeginTime),
            new SkillDispEvent(SkillDispEventType.DISPEVT_TIMEPOINT_TRIGGER, fEndTime));

        return true;
    }

    private GameObject CreateFanDispObject(BaseActor owner)
    {
        if (owner == null)
        {
            return null;
        }

        //GameObject FanDispObj =SkillManager.Instance.CreateNewFanDispObject();
        GameObject FanDispObj1 = Resources.Load("Prefab") as GameObject;
        GameObject FanDispObj = GameObject.Instantiate(FanDispObj1);
        if (FanDispObj == null)
        {
            return null;
        }

        return FanDispObj;
    }

    public override bool Startup(SkillDispEvent evt)
    {
        if (m_AttackRange == null)
		{
            return false;
		}

        BaseActor Tester = null;
		switch (m_eShootTestBase)
		{
		case ShootTestBase.BASE_CASTER:
				Tester = m_CurSkillInfo.Caster;
			break;
		case ShootTestBase.BASE_TARGET:
			{
				Tester = ActorManager.Instance.GetActor((uint)m_CurSkillInfo.SelectTargetId);
			}
			break;
		}

		if (Tester == null)
        {
            return false;
        }

		m_FanDispObject = CreateFanDispObject(Tester);
        if (m_FanDispObject == null)
        {
            return false;
        }

        float angle = m_AttackRange.m_Angle / ((float)Section_Num);

		Vector3 dir = Tester.transform.forward;

		if (m_eShootTestBase == ShootTestBase.BASE_TARGET)
		{
			dir = m_CurSkillInfo.Caster.transform.position - Tester.transform.position;
		}

        dir.y = 0.0f;
        dir = Quaternion.Euler(0, m_AttackRange.m_ForwardDelta, 0) * dir;
        dir.Normalize();

		m_FanDispObject.transform.position = Tester.transform.position + dir * m_AttackRange.m_Distance + Vector3.up * m_fVertOffset;
        Vector3 pos = m_FanDispObject.transform.position;

        List<FanSection> sections = new List<FanSection>();

        for (int i = Section_Num / 2; i >= -Section_Num / 2; i--)
        {
            Vector3 dirSection = Quaternion.Euler(0, angle * i, 0) * dir;

            Vector3 posIn = pos + dirSection * m_AttackRange.m_RadiusIn;
            Vector3 posOut = pos + dirSection * m_AttackRange.m_RadiusOut;

            sections.Add(new FanSection(posIn, posOut));
        }

        m_FanMesh = new CFanMesh();
        m_FanMesh.BuildFan(m_FanDispObject, ref sections);   

        return true;
    }

    public override void Terminate(SkillDispEvent evt)
    {
        if (m_FanDispObject != null)
        {
            GameObject.Destroy(m_FanDispObject);
            m_FanDispObject = null;
        }
	}
}