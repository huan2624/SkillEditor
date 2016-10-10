using UnityEngine;
using System.Collections;

public class ShootPointElementHandler : BaseSkillElementHandler
{
    ShootPointElement m_ShootPointElement;

    public bool Setup(ShootPointElement shootpoint_element)
    {
        if (shootpoint_element == null
            || shootpoint_element.m_StartupEvent == null)
        {
            return false;
        }

        m_ShootPointElement = shootpoint_element;

        RegisterEventHandler(m_ShootPointElement.m_StartupEvent, m_ShootPointElement.m_TerminateEvent);

        return true;
    }

    public override bool Startup(SkillDispEvent evt)
    {
        //Debuger.Log("ShootPoint " + (Time.time - m_CurSkillInfo.m_fCurAnimStartTime));

        if (ExecuteShootTest(m_ShootPointElement.m_ShootTestParam))
        {
           // Debuger.Log("Shoot Target Count " + m_CurSkillInfo.m_lstShootTargets.Count);
            OnHitTargets(m_ShootPointElement.m_ImpactData);
        }
        else
        {
           // Debuger.Log("Shoot No Target");
        }

        return false;   // Make clear event register now
    }

    public override bool IsAlwaysActive()
    {
        if (m_ShootPointElement == null)
            return false;

        if (m_ShootPointElement.m_bBreakable)
            return false;

        return true;
    }
}