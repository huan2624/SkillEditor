using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*----------------------------------------------------------------
 * Animator动画控制器
 * 
 * 需要状态机中 有两个参数
 * bool ValidLock
 * int  AnimKey
 * ValidLock 为锁定动画 让状态机是否可以切换状态的锁 
 * AnimKey 为对应的动作序号
 * 
 * 
 * 
 * --------------------------------------------------------------*/
public class MecanimBehaviour : MonoBehaviour {

    public enum AnimationKey
    {
        NONE = 0,
        //--IDLE( 1 - 100 )------------------
        IDLE_1 = 1,
        IDLE_2 = 2,
        IDLE_3 = 3,
        //--OTHERS( 101 - 200 )--------------
        MOVE = 101,
        VICTORIOUS_1 = 102,
        JIUMING = 103,
        //--UI( 201 - 300 )------------------
        // Reserved
        //--Skill( 301 - 500 )---------------
        SKILL_1 = 301,
        SKILL_2 = 302,
        SKILL_3 = 303,
        SKILL_4 = 304,
        SKILL_5 = 305,
        SKILL_6 = 306,
        SKILL_7 = 307,
        SKILL_8 = 308,
        SKILL_9 = 309,
        SKILL_10 = 310,
        SKILL_11 = 311,
        SKILL_12 = 312,
        SKILL_13 = 313,
        SKILL_14 = 314,
        SKILL_15 = 315,
        SKILL_MAX = 500,
        // Impact ( 501 - 600 )---------------
        HIT_RECOVERY_1 = 501,
        HIT_RECOVERY_2 = 502,
        // Dying ( 601 - 700 )---------------
        NORMAL_DIE_1 = 601,
        NORMAL_DIE_2 = 602,
        FLYAWAY_DIE_1 = 611,
        FLYAWAY_DIE_2 = 612,
        BACKOFF_DIE_1 = 621,
        BACKOFF_DIE_2 = 622,
        // FLOAT ATTACK ( 701 - 800 )---------
        // Reserved
        // PUT DOWN ATTACK ( 801 - 900 )---------
        // Reserved
    };

    public enum MecainmState
    {
        None,               // 无状态
        Prepare,            // 准备播放
        BeginTransition,    // 开始切换动画
        Transitioning,      // 动画切换中
        Play,               // 切换动画完成 准备开始播放
        Finish,             // 播放完成
    }

    /// <summary>
    /// Animator状态机
    /// </summary>
    public Animator m_Animator = null;
    /// <summary>
    /// 角色控制器
    /// </summary>
    public BaseActor m_Actor = null;
    /// <summary>
    /// 动作状态
    /// </summary>
    public MecainmState m_MecainmState = MecainmState.None;

    // 准备播放的 动画队列
    LinkedList<AnimationKeyInfo> m_PreparePlayAnimationKeyList = new LinkedList<AnimationKeyInfo>();

    // 播放完的时候是否开始播放IDLE动画
    private bool m_FinishToIdle;
    //动作播放速度
    private float m_PlaySpeed;
    //当前播放的动作的键名
    private AnimationKey m_CurPlayAnimationKey;
    //当前动作状态
    private AnimatorStateInfo m_CurAnimatorStateInfo;
    //当前动作播放的时间
    private float m_PastNormalizedTime = 0.0f;
    //是否使用时间长度来控制动作播放是否完成（比如持续施法的时候很有用）
    private bool m_IsUseTotalTime = false;
    //动作需要持续的时间
    float m_TotalTime = 0.0f;
    //停止当前动作
    private bool m_StopAnimator = false;

    #region 动作完成事件
    public delegate void MecanimBehaviourCallback(MecanimBehaviour curBehaviour);

    // 动作开始播放的回调
    Dictionary<AnimationKey, MecanimBehaviourCallback> m_OnAnimationBeginCallBackTable
                                    = new Dictionary<AnimationKey, MecanimBehaviourCallback>();

    // 动作被打断时的回调
    Dictionary<AnimationKey, MecanimBehaviourCallback> m_OnAnimationHaltCallBackTable
                                    = new Dictionary<AnimationKey, MecanimBehaviourCallback>();

    // 动作完成的回调
    Dictionary<AnimationKey, MecanimBehaviourCallback> m_OnAnimationFinishCallBackTable
                                    = new Dictionary<AnimationKey, MecanimBehaviourCallback>();

    public void RegisterFinishCallback(AnimationKey eAnimationKey, MecanimBehaviourCallback CallbackFunction)
    {
        if (!m_OnAnimationFinishCallBackTable.ContainsKey(eAnimationKey))
        {
            m_OnAnimationFinishCallBackTable.Add(eAnimationKey, CallbackFunction);
            return;
        }
        m_OnAnimationFinishCallBackTable[eAnimationKey] += CallbackFunction;
    }

    public void RegisterHaltCallback(AnimationKey eAnimationKey, MecanimBehaviourCallback CallbackFunction)
    {
        if (!m_OnAnimationHaltCallBackTable.ContainsKey(eAnimationKey))
        {
            m_OnAnimationHaltCallBackTable.Add(eAnimationKey, CallbackFunction);
            return;
        }
        m_OnAnimationHaltCallBackTable[eAnimationKey] += CallbackFunction;
    }

    public void RegisterBeginCallback(AnimationKey eAnimationKey, MecanimBehaviourCallback CallbackFunction)
    {
        if (!m_OnAnimationBeginCallBackTable.ContainsKey(eAnimationKey))
        {
            m_OnAnimationBeginCallBackTable.Add(eAnimationKey, CallbackFunction);
            return;
        }
        m_OnAnimationBeginCallBackTable[eAnimationKey] += CallbackFunction;
    }

    public void UnRegisterFinishCallback(AnimationKey eAnimationKey, MecanimBehaviourCallback CallbackFunction)
    {
        m_OnAnimationFinishCallBackTable[eAnimationKey] -= CallbackFunction;
    }

    public void UnRegisterHaltCallback(AnimationKey eAnimationKey, MecanimBehaviourCallback CallbackFunction)
    {
        m_OnAnimationHaltCallBackTable[eAnimationKey] -= CallbackFunction;
    }

    public void UnRegisterBeginCallback(AnimationKey eAnimationKey, MecanimBehaviourCallback CallbackFunction)
    {
        m_OnAnimationBeginCallBackTable[eAnimationKey] -= CallbackFunction;
    }
    #endregion

    /// <summary>
    /// 设置当前动作播放时间
    /// </summary>
    /// <param name="TotalTime"></param>
    public void SetAnimationTotalTime(float TotalTime)
    {
        m_TotalTime = TotalTime;
        m_IsUseTotalTime = true;
    }

    public void SetAnimatorState(bool bIsPlaying)
    {
        m_StopAnimator = !bIsPlaying;
    }

    public void SetSpeed(float fSpeed)
    {
        m_PlaySpeed = fSpeed;
    }

    public float GetSpeed()
    {
        return m_PlaySpeed;
    }

    public AnimatorStateInfo GetCurAnimatorStateInfo()
    {
        return m_CurAnimatorStateInfo;
    }

    // 返回最后播放的 AnimationKey 如果处于播放状态 那么就是当前的 AnimationKey
    public AnimationKey GetLastPlayAnimationKey()
    {
        return m_CurPlayAnimationKey;
    }

    public bool IsInTransition()
    {
        return m_Animator.IsInTransition(0);
    }

    // Use this for initialization
    void Start () {
        m_Actor = gameObject.GetComponent<BaseActor>();
        m_Animator = m_Actor.Model.GetComponent<Animator>();
        if (null == m_Animator)
        {
            Destroy(this);
            return;
        }

        SetAnimatorState(true);
    }

    void OnDestroy()
    {
        SetAnimatorState(false);
    }

    // 清空当前播放动作
    public void StopCurAnimation()
    {
        //Debuger.Log(m_Actor + " StopCurAnimation: " + m_LastPlayAnimationKey + " m_MecainmState ==" + m_MecainmState);
        MecanimBehaviourCallback CallbackFunc = null;
        if (m_OnAnimationHaltCallBackTable.TryGetValue(m_CurPlayAnimationKey, out CallbackFunc))
        {
            if (CallbackFunc != null)
            {
                CallbackFunc(this);
            }
        }
        m_PreparePlayAnimationKeyList.Clear();
        m_CurPlayAnimationKey = AnimationKey.IDLE_1;
        m_Animator.SetBool("ValidLock", false);

        m_MecainmState = MecainmState.Prepare;
    }


    /// <summary>
    /// 播放动作
    /// </summary>
    /// <param name="eAnimationKey"></param>
    /// <param name="bHalt"></param>
    /// <param name="fAnimationSpeed"></param>
    /// <param name="bFinishToIdle"></param>
    /// <returns></returns>
    public bool PlayAnimation(AnimationKey eAnimationKey, bool bHalt = true, float fAnimationSpeed = 1.0f, bool bFinishToIdle = false)
    {
        if (m_CurPlayAnimationKey == eAnimationKey && eAnimationKey == AnimationKey.NONE)
        {
            return false;
        }

        if (m_CurPlayAnimationKey == AnimationKey.NORMAL_DIE_1
            || m_CurPlayAnimationKey == AnimationKey.NORMAL_DIE_2)
        {
            return false;
        }

        if (bHalt)
        {
            MecanimBehaviourCallback CallbackFunc = null;
            if (m_OnAnimationHaltCallBackTable.TryGetValue(m_CurPlayAnimationKey, out CallbackFunc))
            {
                if (CallbackFunc != null)
                {
                    CallbackFunc(this);
                }
            }
            m_PreparePlayAnimationKeyList.Clear();

            m_MecainmState = MecainmState.Prepare;
            //			m_Animator.SetBool("ValidLock", false);
        }

        //Debuger.Log(m_Actor + "Play Animation: " + eAnimationKey);
        m_PreparePlayAnimationKeyList.AddLast(new AnimationKeyInfo(eAnimationKey, fAnimationSpeed, bFinishToIdle));

        //Debuger.Log("Animation Prepare" + eAnimationKey);

        return true;
    }

    void UpdateAnimation()
    {
        if (m_MecainmState == MecainmState.Prepare || m_MecainmState == MecainmState.Finish)
        {
            if (m_PreparePlayAnimationKeyList.Count == 0)
            {
                return;
            }

            m_CurPlayAnimationKey = m_PreparePlayAnimationKeyList.First.Value.m_eKey;
            m_FinishToIdle = m_PreparePlayAnimationKeyList.First.Value.m_bFinishToIdle;
            m_PlaySpeed = m_PreparePlayAnimationKeyList.First.Value.m_fSpeed;

            // 硬编码 处理死亡后站立
            if (m_CurPlayAnimationKey >= AnimationKey.NORMAL_DIE_1)
            {
                m_PreparePlayAnimationKeyList.Clear();
            }
            else
            {
                m_PreparePlayAnimationKeyList.RemoveFirst();
            }

            if (m_CurPlayAnimationKey == AnimationKey.NONE)
            {
                m_CurPlayAnimationKey = AnimationKey.IDLE_1;
            }

            // 硬编码 切换动画为 当前动画
            m_Animator.SetBool("ValidLock", true);
            m_Animator.SetInteger("AnimKey", (int)(m_CurPlayAnimationKey));
            //Debuger.Log(m_Actor + "ValidLock: " + m_LastPlayAnimationKey);
            //Debuger.Log("Anim Set " + m_LastPlayAnimationKey);

            m_MecainmState = MecainmState.BeginTransition;
            m_PastNormalizedTime = 0.0f;
        }
        else if (m_MecainmState == MecainmState.BeginTransition)
        {
            //if (!m_Animator.IsInTransition(0) && !m_bStopAnimator)
            //if (!m_bStopAnimator)
            //{
            //    return;
            //}
            m_MecainmState = MecainmState.Transitioning;

            m_PastNormalizedTime += Time.deltaTime; //NormalStep * m_Animator.speed;/*0.0f;*/
        }
        else if (m_MecainmState == MecainmState.Transitioning)
        {
            UpdateCurAnimatorState();
            if (!m_CurAnimatorStateInfo.IsName(m_CurPlayAnimationKey.ToString()))
            {
                return;
            }

            m_Animator.SetBool("ValidLock", false);
            //Debuger.Log(m_Actor + "ValidUnLock: " + m_LastPlayAnimationKey);

            m_Animator.speed = m_PlaySpeed;

            // 切换完成
            m_MecainmState = MecainmState.Play;

            // 获取当前播放动作状态
            UpdateCurAnimatorState();

            //执行动作开始回调
            MecanimBehaviourCallback CallbackBeginFunc = null;
            if (m_OnAnimationBeginCallBackTable.TryGetValue(m_CurPlayAnimationKey, out CallbackBeginFunc))
            {
                if (CallbackBeginFunc != null)
                {
                    CallbackBeginFunc(this);
                }
            }

            //float NormalStep = Time.fixedDeltaTime / m_CurAnimatorStateInfo.length;
            m_PastNormalizedTime += Time.deltaTime;/*0.0f;*/
        }
        else if (m_MecainmState == MecainmState.Play)
        {
            UpdateCurAnimatorState();

            bool IsLoop = m_CurAnimatorStateInfo.loop;

            float NormalStep = Time.deltaTime;// m_CurAnimatorStateInfo.length;

            //float fPastNormalizedTime = 0.0f;

            if ((IsLoop || m_PastNormalizedTime < m_CurAnimatorStateInfo.length) && !m_StopAnimator)
            {
                if (m_PlaySpeed > 0)
                {
                    m_PastNormalizedTime += NormalStep;
                }

                if (m_IsUseTotalTime)
                {
                    if (m_PastNormalizedTime < m_TotalTime)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            //执行动作完成回调
            MecanimBehaviourCallback CallbackFunc = null;
            if (m_OnAnimationFinishCallBackTable.TryGetValue(m_CurPlayAnimationKey, out CallbackFunc))
            {
                if (CallbackFunc != null)
                {
                    //Debuger.Log(m_Actor + " Animation Finish" + m_LastPlayAnimationKey);
                    CallbackFunc(this);
                }
            }

            m_CurPlayAnimationKey = AnimationKey.NONE;
            m_MecainmState = MecainmState.Finish;
            m_IsUseTotalTime = false;

            // 动画播放完成 如果当前没有其他动作需要播放 并且此动作要返回IDLE 压IDLE进入状态列表
            if (m_FinishToIdle && m_PreparePlayAnimationKeyList.Count == 0)
            {
                AnimationKeyInfo NextAnimationInfo = new AnimationKeyInfo(AnimationKey.IDLE_1, 1.0f, false);
                m_PreparePlayAnimationKeyList.AddLast(NextAnimationInfo);
                m_MecainmState = MecainmState.Prepare;
            }
        }
    }

    void UpdateCurAnimatorState()
    {
        if (m_Animator.IsInTransition(0))
        {
            m_CurAnimatorStateInfo = m_Animator.GetNextAnimatorStateInfo(0);
        }
        else
        {
            m_CurAnimatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        }
    }

    // Update is called once per frame
    void Update () {
        UpdateAnimation();
        if (m_Animator != null)
        {
            m_Animator.speed = m_PlaySpeed;
        }
    }

    class AnimationKeyInfo
    {
        public AnimationKeyInfo()
        {

        }

        public AnimationKeyInfo(AnimationKey eKey, float fSpeed, bool bFinishToIdle)
        {
            m_eKey = eKey;
            m_fSpeed = fSpeed;
            m_bFinishToIdle = bFinishToIdle;
        }
        //动作键名
        public AnimationKey m_eKey = AnimationKey.NONE;
        //动作速度
        public float m_fSpeed = 1.0f;
        //动作完成后是否进入待机
        public bool m_bFinishToIdle = false;
    }
}
