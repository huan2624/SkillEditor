using UnityEngine;
using System.Collections;

public class SkillPreview : MonoBehaviour {

    private int m_SkillId;
    public string m_strCaster;
    public string m_strTarget;
    private BaseActor m_Caster = null;
    private BaseActor m_Target = null;

    private Transform m_Point1;
    private Transform m_Point2;

    //创建施法者
    private void CreateCaster()
    {
        if (m_Caster != null)
        {
            Destroy(m_Caster.gameObject);
        }
        m_Caster = LoadActor();
        m_Caster.gameObject.transform.position = m_Point1.position;
    }
    //创建目标
    private void CreateTarget()
    {
        if (m_Target != null)
        {
            Destroy(m_Target.gameObject);
        }
        m_Target = LoadActor();
        m_Target.gameObject.transform.position = m_Point2.position;
    }

    private BaseActor LoadActor()
    {
        GameObject obj = Resources.Load("Role 1") as GameObject;
        GameObject actor = Instantiate(obj);
        BaseActor baseac = actor.GetComponent<BaseActor>();
        return baseac;
    }

    public void StartFreeMode(string strCaster, string strTarget, int iSkillId, float fAttackRange)
    {
        m_SkillId = iSkillId;
        m_strCaster = strCaster;
        m_strTarget = strTarget;

        CreateCaster();
        CreateTarget();
    }

    // Use this for initialization
    void Start()
    {
        m_Point1 = GameObject.Find("Point1").transform;
        m_Point2 = GameObject.Find("Point2").transform;

        AudioManager.Instance.Init();
    }
	
	// Update is called once per frame
	void Update () {
        AudioManager.Instance.OnUpdate();

    }

    void OnGUI()
    {
        if (m_SkillId != 0 && m_SkillId != 0 && m_Caster != null && m_Target != null)
        {
            if (GUI.Button(new Rect(150, 15, 120, 40), m_SkillId.ToString()))
            {   
                // 响应 技能编辑器 自由模式下 按下技能按钮事件
                m_Caster.AttackBySkillID((uint)m_SkillId, m_Target);
            }
        }
    }
}
