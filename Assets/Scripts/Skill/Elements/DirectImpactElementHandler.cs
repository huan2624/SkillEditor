using UnityEngine;
using System.Collections;

public class DirectImpactElementHandler : BaseSkillElementHandler
{
    DirectImpactElement m_DirectImpactElement;

    public bool Setup(DirectImpactElement impact_element)
    {
        Debug.Log("DirectImpactElement setup");
        if (impact_element == null
            || impact_element.m_StartupEvent == null)
        {
            return false;
        }

        m_DirectImpactElement = impact_element;

        RegisterEventHandler(m_DirectImpactElement.m_StartupEvent, m_DirectImpactElement.m_TerminateEvent);

        return true;
    }

    public override bool Startup(SkillDispEvent evt)
    {
        Debug.Log("DirectImpactElementHandler Startup");
        if (ExecuteHitTest())
        {
            OnHitTargets(m_DirectImpactElement.m_ImpactData);
        }

        return false;   // Make clear event register now
    }
}