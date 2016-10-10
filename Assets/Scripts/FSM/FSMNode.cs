using UnityEngine;
using System.Collections;

public class FSMNode {

    public const int NullStateID = -999;
    private FSMManager m_Manager = null;
    private int m_StateID = NullStateID;

    public FSMNode(int id, FSMManager system)
    {
        if (id == NullStateID)
        {
            LogManager.LogError("FSMNode ERROR : state id can not as NullStateID");
        }
        if (null == system)
        {
            LogManager.LogError("FSMNode ERROR : system is not allow null");
        }

        m_StateID = id;
        m_Manager = system;
        m_Manager.AddState(this);
    }

    public int GetStateID() {
        return m_StateID;
	}

    public void EnterState()
    {
        if (m_Manager != null)
            m_Manager.EnterState(m_StateID);
    }

    public void LeaveState()
    {
        if (m_Manager != null && m_Manager.GetCurrentStateID() == GetStateID())
            m_Manager.LeaveCurrentState();
    }

    public virtual void OnEnter() { }
    public virtual void OnEnterAgain() { }
    public virtual void OnLeave() { }
    public virtual void Update() { }
    public virtual void Transition() { }
}
