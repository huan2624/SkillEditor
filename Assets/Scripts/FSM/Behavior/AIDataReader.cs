using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml;

    /**
     *  AI动作
     */
    public enum AIAction
    {
        // 释放技能
        UseSkill = 1,
        // 血量触发
        ShowText = 2,
    }

    /**
     *  AI触发类型
     */
    public enum AITriggerType
    {
        // 时间触发
        TimeTrigger = 1,
        // 血量触发
        HpTrigger = 2,
    }

    /**
     *  受击类型
     */
    public enum HitType
    {
        None = 0,
        Normal = 1,
        HitBack = 2,
        HitRush = 3,
    }

    /**
     *  AI数据集合
     */
    public class AIDataSet
    {
        // 攻击数据列表
        public List<AIAttackData> attackDataList = new List<AIAttackData>();
        // 说话数据列表
        public List<AISayData> sayDataList = new List<AISayData>();
    }

    /**
     *  攻击AI数据
     */
    public class AIAttackData
    {
        // AI编号
        public int id;
        // 触发几率
        public int odds;
        // cd时间触发
        public int timeTrigger;
        // 血量触发
        public int hpTrigger;
        // 技能id
        public int skillId;
        // 预警时间
        public int warningTime;
        // 预警半径
        public int warningRadius;
        // 预警半径
        public int warningAngle;
        // 预警类型
        public int warningType;
        // 是否循环
        public bool loop;

        /*************** 非配置的扩展数据 ******************/
        // 是否已经触发
        public bool isTriggered = false;
        /*************** 非配置的扩展数据 ******************/

        /* 函数说明：构造函数 */
        public AIAttackData() {}
        public AIAttackData(BaseActor actor, wl_res.AIConfig cfg)
        {
            uint skillIndex = (uint)cfg.param[0] - 1;
            this.id = cfg.id;
            this.timeTrigger = cfg.timeTrigger;
            this.hpTrigger = cfg.hpTrigger;
            this.odds = cfg.odds;
            //this.skillId = (int)ActorUtils.GetSkillIdByIndex(actor, skillIndex);
            this.warningTime = cfg.param[1];
            this.warningRadius = cfg.param[2];
            this.warningAngle = cfg.param[3];
            this.warningType = cfg.param[4];
            this.loop = (cfg.loop != 0);
        }
    }

    /**
     *  说话AI数据
     */
    public class AISayData
    {
        // AI编号
        public int id;
        // cd时间触发
        public int timeTrigger;
        // 血量触发
        public int hpTrigger;
        // 触发几率
        public int odds;
        // 文本信息
        public string text;
        // 持续时间
        public int duration;
        // 是否循环
        public bool loop;

        /*************** 非配置的扩展数据 ******************/
        // 是否已经触发
        public bool isTriggered = false;
        /*************** 非配置的扩展数据 ******************/

        /* 函数说明：构造函数 */
        public AISayData() {}
        public AISayData(wl_res.AIConfig cfg)
        {
            this.id = cfg.id;
            this.timeTrigger = cfg.timeTrigger;
            this.hpTrigger = cfg.hpTrigger;
            this.odds = cfg.odds;
            this.text = AIDataReader.Instance().ReadSayText(cfg.param[0]);
            this.duration = cfg.param[1];
            this.loop = (cfg.loop != 0);
        }
    }

public class AIDataReader
{
    private static AIDataReader _instance = null;
    private Dictionary<int, wl_res.AIConfig> _aiConfigDict = new Dictionary<int, wl_res.AIConfig>();
    private Dictionary<int, string> _textDict = new Dictionary<int, string>();

    /* 函数说明：获取该类实例 */
    public static AIDataReader Instance()
    {
        if (_instance == null)
        {
            _instance = new AIDataReader();
            _instance.InitConfig();
        }
        return _instance;
    }

    /* 函数说明：读取AI数据 */
    public AIDataSet ReadData(BaseActor actor)
    {
        //List<int> aiList = actor.m_refActorAttribBehaviour.m_AIList;

        AIDataSet dataSet = new AIDataSet();
        //for (int i = 0; i < aiList.Count; i++)
        //{
        //    int id = aiList[i];
        //    wl_res.AIConfig template = null;
        //    if (_aiConfigDict.TryGetValue(id, out template))
        //    {
        //        ParserConfig(actor, dataSet, template);
        //    }
        //}
        return dataSet;
    }

    /* 函数说明：读取说话文本 */
    public string ReadSayText(int id)
    {
        string text;
        if (_textDict.TryGetValue(id, out text))
        {
            return text;
        }
        return "";
    }

    /********************************************************/
    /******************** 以下是内部函数 *********************/
    /********************************************************/

    void InitConfig()
    {
        //wl_res.AIConfig[] cfgArray = TextUtils.ReadTdrFile<wl_res.AIConfig>("LocalConfig/AI/AIConfig");
        //if (cfgArray != null)
        //{
        //    for (int i = 0; i < cfgArray.Length; i++)
        //    {
        //        _aiConfigDict[cfgArray[i].id] = cfgArray[i];
        //    }
        //}

        //XmlDocument xmlDoc = TextUtils.OpenXml("LocalConfig/AI/AIText");
        //if (xmlDoc != null)
        //{
        //    XmlNodeList xmlNodeList = xmlDoc.SelectNodes("/AIText/Text");
        //    for (int i = 0; i < xmlNodeList.Count; i++)
        //    {
        //        XmlNode xmlNode = xmlNodeList[i];
        //        int id = TextUtils.XmlReadInt(xmlNode, "id", 0);
        //        string text = TextUtils.XmlReadString(xmlNode, "text", "");
        //        _textDict[id] = text;
        //    }
        //}
    }

    void ParserConfig(BaseActor actor, AIDataSet dataSet, wl_res.AIConfig cfg)
    {
        //if (cfg.action == (int)AIAction.UseSkill)
        //{
        //    AIAttackData data = new AIAttackData(actor, cfg);
        //    dataSet.attackDataList.Add(data);
        //}
        //else if (cfg.action == (int)AIAction.ShowText)
        //{
        //    AISayData data = new AISayData(cfg);
        //    dataSet.sayDataList.Add(data);
        //}
    }
}
