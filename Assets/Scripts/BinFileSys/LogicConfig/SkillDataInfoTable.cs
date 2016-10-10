/*
 *       HeroAttributesTable.cs
 *       Copyright (c) 2014, TopCloud Company 
 *       All rights reserved.
 *       Create By Sword.
 */

// Read the file has been defined in HeroBasicAttributes.cs.

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SkillDataInfoTable : LogicFileWithKey<UInt32, wl_res.SkillDataInfo>
{
	public override UInt32 GetKey(wl_res.SkillDataInfo Value)
	{
		return Value.SkillID;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Skill/SkillDataInfo");
	}
}

public class ExclusiveEquipAddtionTable : LogicFileWithKey<UInt64, wl_res.ExclusiveEquipAddtion>
{
    public UInt64 BuildKey(UInt32 HeroID, UInt32 SkillID)
    {
        UInt64 Key = HeroID;
        Key = (Key << 32) + SkillID / 1000;
        return Key;
    }

    public override UInt64 GetKey(wl_res.ExclusiveEquipAddtion Value)
    {
        return BuildKey(Value.HeroID, Value.SkillID);
    }

	public override void Init()
	{
		ReadBinFile("LocalConfig/Skill/ExclusiveEquipAddtion");
	}
}

public class SkillLearnTable : LogicFileWithKey<UInt32, wl_res.SkillLearn>
{
    private List<uint> m_allSkill = new List<uint>();

	public override UInt32 GetKey(wl_res.SkillLearn Value)
	{
		return Value.SkillID;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Skill/SkillLearn");

        foreach (KeyValuePair<uint, wl_res.SkillLearn> value in GetTable())
        {
            m_allSkill.Add(value.Key);
        }
	}

    public List<uint> getAllSkillList() 
    {
        return m_allSkill;
    }
}

//技能槽限制
public class SkillSlotLimitTable : LogicFileWithKey<UInt32, wl_res.SkillSlotLimit>
{
    private Dictionary<uint, uint> m_skillSlotLimit = new Dictionary<uint,uint>();

	public override UInt32 GetKey(wl_res.SkillSlotLimit Value)
	{
		return Value.SlotNum;
	}

    public uint GetNeedStarLv(uint index)
    {
        // 根据技能槽index获取所需星级
        uint ret = 0;
		if (!m_skillSlotLimit.TryGetValue(index, out ret))
        {
			return 0;
        }
        return ret;
    }

	public override void Init()
	{
		ReadBinFile("LocalConfig/Skill/SkillSlotLimit");

        foreach (KeyValuePair<UInt32, wl_res.SkillSlotLimit> Pair in GetTable())
        {
            if (!m_skillSlotLimit.ContainsKey((uint)Pair.Value.SlotNum) ||
                m_skillSlotLimit[(uint)Pair.Value.SlotNum] > (uint)Pair.Key)
            {
                m_skillSlotLimit[(uint)Pair.Value.SlotNum] = (uint)Pair.Key;
            }
        }
	}
}

public class SkillUpgradeTable : LogicFileWithKey<UInt32, wl_res.SkillUpgrade>
{
	public override UInt32 GetKey(wl_res.SkillUpgrade Value)
	{
		return Value.level;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Skill/SkillUpgrade");
	}
}

public class MultiAttackInfoTabel : LogicFileWithKey<UInt32, wl_res.MultiAttackInfo>
{
    public override UInt32 GetKey(wl_res.MultiAttackInfo Value)
    {
        return Value.SkillId;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Skill/MultiAttackInfo");
    }
}

public class TalentTable : LogicFileWithKey<UInt32, wl_res.Talent>
{
	public override UInt32 GetKey(wl_res.Talent Value)
	{
		return Value.TalentID;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Skill/Talent");
	}
}

public class TalentLearnTable : LogicFileWithKey<UInt32, wl_res.TalentLearn>
{
	public override UInt32 GetKey(wl_res.TalentLearn Value)
	{
		return Value.TalentID;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Skill/TalentLearn");
	}
}

public class TalentReplaceTable : LogicFileWithKey<UInt32, wl_res.TalentReplace>
{
	public override UInt32 GetKey(wl_res.TalentReplace Value)
	{
		return Value.TalentID;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Skill/TalentReplace");
	}
}

public class TalentSlotExtendTable : LogicFileWithKey<UInt32, wl_res.TalentSlotExtend>
{
	public override UInt32 GetKey(wl_res.TalentSlotExtend Value)
	{
		return Value.SlotNum;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Skill/TalentSlotExtend");
	}
}

public class TalentSlotLimitTable : LogicFileWithKey<UInt32, wl_res.TalentSlotLimit>
{
	public override UInt32 GetKey(wl_res.TalentSlotLimit Value)
	{
		return Value.HeroStar;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Skill/TalentSlotLimit");
	}
}

public class QTEInfoTable : LogicFileWithKey<UInt32, wl_res.QTEInfo>
{
    List<uint> m_allQTEList = new List<uint>();

	public override UInt32 GetKey(wl_res.QTEInfo Value)
	{
		return Value.Id;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Skill/QTEInfo");

        m_allQTEList.Clear();

        foreach (KeyValuePair<UInt32, wl_res.QTEInfo> Pair in GetTable())
        {
            if (Pair.Value.Level == 1)
            {
                m_allQTEList.Add(Pair.Value.Id);
            }
        }
	}

    public List<uint> getAllQTEList()
    {
        // 获取所有QTE列表
        return m_allQTEList;
    }
}

public class QteBuffTable : LogicFileWithKey<UInt32, wl_res.QteBuff>
{
    public override UInt32 GetKey(wl_res.QteBuff Value)
	{
        return Value.Id;
	}

	public override void Init()
	{
        ReadBinFile("LocalConfig/Skill/QteBuff");
	}
}

public class QteUpgradeTable : LogicFileWithKey<UInt32, wl_res.QteUpgrade>
{
    public override UInt32 GetKey(wl_res.QteUpgrade Value)
	{
        return Value.ID;
	}

	public override void Init()
	{
        ReadBinFile("LocalConfig/Skill/QteUpgrade");
	}
}

public class NaturalEnemyHeroTable : LogicFileWithoutKey<wl_res.NaturalEnemyHero>
{
    Dictionary<uint, List<wl_res.NaturalEnemyHero>> m_dicLstData = new Dictionary<uint, List<wl_res.NaturalEnemyHero>>();
    Dictionary<ulong, wl_res.NaturalEnemyHero> m_dicData = new Dictionary<ulong, wl_res.NaturalEnemyHero>();

    //获取指定英雄的天敌配置列表
    public List<wl_res.NaturalEnemyHero> GetNaturalEnemyHeroList(uint uHeroId)
    {
        List<wl_res.NaturalEnemyHero> lstRet = null;
        m_dicLstData.TryGetValue(uHeroId, out lstRet);
        return lstRet;
    }

    public wl_res.NaturalEnemyHero GetConf(uint uAttackHeroId, uint uUnderAttackHeroId)
    {
        ulong uKey = MakeKey(uAttackHeroId, uUnderAttackHeroId);
        wl_res.NaturalEnemyHero ret = null;
        m_dicData.TryGetValue(uKey, out ret);
        return ret;
    }

    public ulong MakeKey(uint uAttackHeroId, uint uUnderAttackHeroId)
    {
        return (ulong)uAttackHeroId << 32 | uUnderAttackHeroId;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Skill/NaturalEnemyHero");

        List<wl_res.NaturalEnemyHero> lstData = GetTable();
        List<wl_res.NaturalEnemyHero> lstTmp = null;
        for (int i = 0; i < lstData.Count; ++i)
        {
            if (m_dicLstData.TryGetValue(lstData[i].AttackHeroId, out lstTmp))
            {
                lstTmp.Add(lstData[i]);
            }
            else
            {
                List<wl_res.NaturalEnemyHero> lst = new List<wl_res.NaturalEnemyHero>();
                lst.Add(lstData[i]);
                m_dicLstData.Add(lstData[i].AttackHeroId, lst);
            }

            if (m_dicLstData.TryGetValue(lstData[i].UnderAttackHeroId, out lstTmp))
            {
                lstTmp.Add(lstData[i]);
            }
            else
            {
                List<wl_res.NaturalEnemyHero> lst = new List<wl_res.NaturalEnemyHero>();
                lst.Add(lstData[i]);
                m_dicLstData.Add(lstData[i].UnderAttackHeroId, lst);
            }

            ulong uKey = MakeKey(lstData[i].AttackHeroId, lstData[i].UnderAttackHeroId);
            if ( !m_dicData.ContainsKey(uKey))
            {
                m_dicData.Add(uKey, lstData[i]);
            }
        }
    }
}

public class NaturalEnemyHeroDescTable : LogicFileWithoutKey<wl_res.NaturalEnemyHeroDesc>
{
    Dictionary<uint, List<wl_res.NaturalEnemyHeroDesc>> m_dicLstData = new Dictionary<uint, List<wl_res.NaturalEnemyHeroDesc>>();
    Dictionary<ulong, wl_res.NaturalEnemyHeroDesc> m_dicData = new Dictionary<ulong, wl_res.NaturalEnemyHeroDesc>();

    //获取指定英雄的天敌配置列表
    public List<wl_res.NaturalEnemyHeroDesc> GetNaturalEnemyHeroList(uint uHeroId)
    {
        List<wl_res.NaturalEnemyHeroDesc> lstRet = null;
        m_dicLstData.TryGetValue(uHeroId, out lstRet);
        return lstRet;
    }

    public wl_res.NaturalEnemyHeroDesc GetConf(uint uAttackHeroId, uint uUnderAttackHeroId)
    {
        ulong uKey = MakeKey(uAttackHeroId, uUnderAttackHeroId);
        wl_res.NaturalEnemyHeroDesc ret = null;
        m_dicData.TryGetValue(uKey, out ret);
        return ret;
    }

    public ulong MakeKey(uint uAttackHeroId, uint uUnderAttackHeroId)
    {
        return (ulong)uAttackHeroId << 32 | uUnderAttackHeroId;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Skill/NaturalEnemyHeroDesc");

        List<wl_res.NaturalEnemyHeroDesc> lstData = GetTable();
        List<wl_res.NaturalEnemyHeroDesc> lstTmp = null;
        for (int i = 0; i < lstData.Count; ++i)
        {
            if (m_dicLstData.TryGetValue(lstData[i].AttackHeroId, out lstTmp))
            {
                lstTmp.Add(lstData[i]);
            }
            else
            {
                List<wl_res.NaturalEnemyHeroDesc> lst = new List<wl_res.NaturalEnemyHeroDesc>();
                lst.Add(lstData[i]);
                m_dicLstData.Add(lstData[i].AttackHeroId, lst);
            }

            if (m_dicLstData.TryGetValue(lstData[i].UnderAttackHeroId, out lstTmp))
            {
                lstTmp.Add(lstData[i]);
            }
            else
            {
                List<wl_res.NaturalEnemyHeroDesc> lst = new List<wl_res.NaturalEnemyHeroDesc>();
                lst.Add(lstData[i]);
                m_dicLstData.Add(lstData[i].UnderAttackHeroId, lst);
            }

            ulong uKey = MakeKey(lstData[i].AttackHeroId, lstData[i].UnderAttackHeroId);
            if (!m_dicData.ContainsKey(uKey))
            {
                m_dicData.Add(uKey, lstData[i]);
            }
        }
    }
}

public class PartnerHeroTable : LogicFileWithoutKey<wl_res.PartnerHero>
{
    private Dictionary<uint, List<uint>> m_dicData = new Dictionary<uint, List<uint>>();

    //获取指定英雄的所有伙伴英雄
    public List<uint> GetPartnerHero(uint uHeroId)
    {
        List<uint> lstTmp = null;
        m_dicData.TryGetValue(uHeroId, out lstTmp);
        return lstTmp;
    }

    public wl_res.PartnerHero GetPartnerHeroConf(uint uMasterHeroId, uint uPartnerHeroId)
    {
        List<uint> lstTmp = null;
        if ( !m_dicData.TryGetValue(uMasterHeroId, out lstTmp) )
        {
            return null;
        }

        List<wl_res.PartnerHero> lstData = GetTable();
        List<wl_res.PartnerHero> LstPartnerHero = new List<wl_res.PartnerHero>();

        for (int i = 0; i < lstData.Count; ++i)
        {
            if (lstData[i].HeroId == uMasterHeroId)
            {
                LstPartnerHero.Add(lstData[i]);
            }     
        }
        for (int i = 0; i < LstPartnerHero.Count; i++)
        {
            if (LstPartnerHero[i].PartnerHeroId==uPartnerHeroId)
            {
                return LstPartnerHero[i];
            }
        }           

         return null;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Skill/PartnerHero");

        List<wl_res.PartnerHero> lstData = GetTable();
        List<uint> lstTmp = null;
        for (int i = 0; i < lstData.Count; ++i)
        {                       
            if ( m_dicData.TryGetValue(lstData[i].HeroId, out lstTmp) )
            {
                lstTmp.Add(lstData[i].PartnerHeroId);
            }
            else
            {
                List<uint> lst = new List<uint>();
                lst.Add(lstData[i].PartnerHeroId);
                m_dicData.Add(lstData[i].HeroId, lst);
            }
        }
    }
}
public class PredatorRelationshipTable : LogicFileWithKey<UInt32, wl_res.PredatorRelationship>
{
    List<wl_res.PredatorRelationship> m_list;
    Dictionary<uint,wl_res.PredatorRelationship> m_dict;
	public override UInt32 GetKey(wl_res.PredatorRelationship Value)
	{
		return Value.ID;
	}

    public override void Init()
    {
        ReadBinFile("LocalConfig/Skill/PredatorRelationship");
        m_dict = GetTable();
        m_list = new List<wl_res.PredatorRelationship>();
        List<uint> keys = new List<uint>(m_dict.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            m_list.Add(m_dict[keys[i]]);
        }
    }

    public List<wl_res.PredatorRelationship> GetList()
    {
        return m_list;
    }

    public wl_res.PredatorRelationship GetPredatorRelationship(uint casterid,uint targetid)
    {
        for(int i = 0; i < m_list.Count;i++)
        {
            if(m_list[i].AttackHeroId == casterid && m_list[i].UnderAttackHeroId == targetid)
            {
                return m_list[i];
            }
        }
        return null;
    }
}

public class PredatorUpgradeTable : LogicFileWithoutKey<wl_res.PredatorUpgrade>
{

    public Dictionary<string, wl_res.PredatorUpgrade> m_dict;
    public override void Init()
    {
        ReadBinFile("LocalConfig/Skill/PredatorUpgrade");
        m_dict = new Dictionary<string,wl_res.PredatorUpgrade>();
        List<wl_res.PredatorUpgrade> preUps = GetTable();
        if(preUps != null)
        {
            for(int i = 0;i < preUps.Count;i++)
            {
                wl_res.PredatorUpgrade curPreUp = null;
                string curKey = preUps[i].RelationId.ToString() + preUps[i].Level.ToString();
                if(!m_dict.TryGetValue(curKey,out curPreUp))
                {
                    m_dict.Add(curKey,preUps[i]);
                }
            }
        }
    }

    public bool GetPredatorUpgradeByRelationIdAndLevel(uint relationId, uint level,out wl_res.PredatorUpgrade retData)
    {
        retData = null;
        string curKey = relationId.ToString() + level.ToString();
        if (m_dict.TryGetValue(curKey, out retData))
        {
            return true;
        }
        return false;
    }
}

public class PredatorUpgradeConditionTable : LogicFileWithKey<UInt32, wl_res.PredatorUpgradeCondition>
{
	public override UInt32 GetKey(wl_res.PredatorUpgradeCondition Value)
	{
		return Value.Level;
	}

    public override void Init()
    {
        ReadBinFile("LocalConfig/Skill/PredatorUpgradeCondition");

    }
}

