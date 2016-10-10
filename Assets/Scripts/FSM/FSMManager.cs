using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FSMManager
{
    public event Action<int> StateEnterEvent;
    public event Action<int> StateEnterAgainEvent;
    public event Action<int> StateLeaveEvent;

    public bool Enable { set; get; }
    //状态列表
    private List<FSMNode> m_StateList = new List<FSMNode>();
    //当前状态
    private FSMNode m_CurState;
    //默认状态
    private FSMNode m_DefaultState;
    //前一个状态id
    private int m_PrevStateID;

    /* 函数说明： 获取的当前状态 */
    public FSMNode GetCurrentState()
    {
        return m_CurState;
    }

    /* 函数说明： 获取的当前状态ID */
    public int GetCurrentStateID()
    {
        if (m_CurState == null)
            return FSMNode.NullStateID;
        return m_CurState.GetStateID();
    }

    /* 函数说明： 获取前一个状态的ID */
    public int GetPrevStateID()
    {
        return m_PrevStateID;
    }

    /* 函数说明： 获取默认状态 */
    public FSMNode GetDefaultState()
    {
        return m_DefaultState;
    }

    /* 函数说明： 根据id获取状态基类（使用时转成真实目标类） */
    public FSMNode GetState(int stateId)
    {
        for (int i = 0; i < m_StateList.Count; i++)
        {
            if (stateId == m_StateList[i].GetStateID())
                return m_StateList[i];
        }
        return null;
    }

    /* 函数说明： 添加新状态（相同状态不可重复添加） */
    public void AddState(FSMNode state)
    {
        if (state == null)
        {
            LogManager.LogError("StateSystem ERROR : unable to add a null state");
            return;
        }
        for (int i = 0; i < m_StateList.Count; i++)
        {
            if (m_StateList[i].GetStateID() == state.GetStateID())
            {
                LogManager.LogError("StateSystem ERROR : unable to add state by the same id : " + state.GetStateID().ToString());
                return;
            }
        }
        m_StateList.Add(state);
    }

    /* 函数说明： 删除指定状态 */
    public void DeleteState(int stateId)
    {
        for (int i = 0; i < m_StateList.Count; i++)
        {
            if (m_StateList[i].GetStateID() == stateId)
            {
                m_StateList.Remove(m_StateList[i]);
                return;
            }
        }
    }

    /* 函数说明： 设置默认状态（启动状态机时自动进入默认状态） */
    public void SetDefaultState(int stateId)
    {
        for (int i = 0; i < m_StateList.Count; i++)
        {
            FSMNode state = m_StateList[i];
            if (state.GetStateID() == stateId)
            {
                m_DefaultState = state;

                //如果当前状态为空，则将默认状态设为当前状态
                if (m_CurState == null)
                {
                    m_CurState = m_DefaultState;
                    m_CurState.OnEnter();
                }
                return;
            }
        }
    }

    /* 函数说明： 设置当前状态 */
    public bool EnterState(int stateId)
    {
        // 如果当前状态等于将设定的目标状态
        // 则调用该状态的OnEnterAgain函数，而非OnEnter函数，并且不会调用OnExit函数
        if (m_CurState != null && m_CurState.GetStateID() == stateId)
        {
            m_CurState.OnEnterAgain();
            if (StateEnterAgainEvent != null)
                StateEnterAgainEvent(stateId);
            return true;
        }

        // 设置新状态为当前状态时
        // 先调用旧状态的OnLeave函数，然后调用新状态的OnEnter函数
        for (int i = 0; i < m_StateList.Count; i++)
        {
            FSMNode state = m_StateList[i];
            if (state.GetStateID() == stateId)
            {
                if (m_CurState != null)
                {
                    m_CurState.OnLeave();
                    m_PrevStateID = m_CurState.GetStateID();
                    m_CurState = null;

                    if (StateLeaveEvent != null)
                        StateLeaveEvent(m_PrevStateID);
                }
                m_CurState = state;
                m_CurState.OnEnter();
                if (StateEnterEvent != null)
                    StateEnterEvent(stateId);
                return true;
            }
        }

        return false;
    }

    /* 函数说明： 离开当前状态，转换到默认状态 */
    public void LeaveCurrentState()
    {
        if (m_CurState != null)
        {
            m_CurState.OnLeave();
            m_PrevStateID = m_CurState.GetStateID();
            m_CurState = null;

            if (StateLeaveEvent != null)
                StateLeaveEvent(m_PrevStateID);
        }

        /* 函数说明： 转换到默认状态 */
        if (m_DefaultState != null)
        {
            m_CurState = m_DefaultState;
            m_CurState.OnEnter();
            if (StateEnterEvent != null)
                StateEnterEvent.Invoke(m_DefaultState.GetStateID());
        }
    }

    public void Update()
    {
        if (Enable == false)
            return;

        //if (_globalState != null)
        //{
        //    _globalState.lastUpdateTime += Time.deltaTime;
        //    if (_globalState.lastUpdateTime >= _globalState.updateInterval)
        //    {
        //        _globalState.Update();
        //    }
        //}

        if (m_CurState != null)
        {
            m_CurState.Update();
        }

        int currentStateId = GetCurrentStateID();
        for (int i = 0; i < m_StateList.Count; i++)
        {
            FSMNode state = m_StateList[i];
            if (state.GetStateID() != currentStateId)
            {
                state.Transition();
                //if (GetCurrentStateID() != currentStateId)
                //{
                //    break;
                //}
            }
        }
    }

}
