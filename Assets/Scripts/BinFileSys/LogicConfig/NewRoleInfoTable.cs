/*
 *       HeroAttributesTable.cs
 *       Copyright (c) 2014, TopCloud Company 
 *       All rights reserved.
 *       Create By Zln.
 */

// Read the file has been defined in HeroBasicAttributes.cs.

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class LevelExpTable : LogicFileWithKey<UInt32, wl_res.LevelExp>
{
    public override UInt32 GetKey(wl_res.LevelExp Value)
    {
        return Value.Level;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Role/LevelExp");
    }
}

public class FightCapacityParamTable : LogicFileWithKey<UInt32, wl_res.FightCapacityParam>
{
    List<wl_res.FightCapacityParam> m_FightCapacityParamList = new List<wl_res.FightCapacityParam>();

    public override UInt32 GetKey(wl_res.FightCapacityParam Value)
    {
        return Value.Param;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Role/FightCapacityParam");
        m_FightCapacityParamList.Clear();

        foreach (KeyValuePair<UInt32, wl_res.FightCapacityParam> Pair in GetTable())
        {
            m_FightCapacityParamList.Add(Pair.Value);
        }
    }

    public wl_res.FightCapacityParam getFightCapacityParam()
    {
        if (0 != m_FightCapacityParamList.Count)
        {
            return m_FightCapacityParamList[0];
        }
        return null;
    }
}

public class AttributeFormulaParamTable : LogicFileWithKey<UInt32, wl_res.AttributeFormulaParam>
{
    List<wl_res.AttributeFormulaParam> m_AttributeFormulaParamList = new List<wl_res.AttributeFormulaParam>();

    public override UInt32 GetKey(wl_res.AttributeFormulaParam Value)
    {
        return Value.Param;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Role/AttributeFormulaParam");
        m_AttributeFormulaParamList.Clear();

        foreach (KeyValuePair<UInt32, wl_res.AttributeFormulaParam> Pair in GetTable())
        {
            m_AttributeFormulaParamList.Add(Pair.Value);
        }
    }

    public wl_res.AttributeFormulaParam getAttributeFormulaParam()
    {
        if (0 != m_AttributeFormulaParamList.Count)
        {
            return m_AttributeFormulaParamList[0];
        }
        return null;
    }
}

public class AttributeTransferCoefficientTable : LogicFileWithKey<UInt32, wl_res.AttributeTransferCoefficient>
{
    public override UInt32 GetKey(wl_res.AttributeTransferCoefficient Value)
    {
        return Value.HeroType;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Role/AttributeTransferCoefficient");
    }
}

public class RoleChangeNameFeeTable : LogicFileWithKey<UInt32, wl_res.RoleChangeNameFee>
{
    List<wl_res.RoleChangeNameFee> m_RoleChangeNameFeeList = new List<wl_res.RoleChangeNameFee>();

    public override UInt32 GetKey(wl_res.RoleChangeNameFee Value)
    {
        return Value.seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Role/RoleChangeNameFee");
        m_RoleChangeNameFeeList.Clear();

        foreach (KeyValuePair<UInt32, wl_res.RoleChangeNameFee> Pair in GetTable())
        {
            m_RoleChangeNameFeeList.Add(Pair.Value);
        }
    }

    public wl_res.RoleChangeNameFee getRoleChangeNameFee()
    {
        if (0 != m_RoleChangeNameFeeList.Count)
        {
            return m_RoleChangeNameFeeList[0];
        }
        return null;
    }
}

public class SkillExtraAttrAddTable : LogicFileWithKey<UInt32, wl_res.SkillExtraAttrAdd>
{
    List<wl_res.SkillExtraAttrAdd> m_SkillExtraAttrAddList = new List<wl_res.SkillExtraAttrAdd>();

    public override UInt32 GetKey(wl_res.SkillExtraAttrAdd Value)
    {
        return Value.SeqNo;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Role/SkillExtraAttrAdd");
        m_SkillExtraAttrAddList.Clear();

        foreach (KeyValuePair<UInt32, wl_res.SkillExtraAttrAdd> Pair in GetTable())
        {
            m_SkillExtraAttrAddList.Add(Pair.Value);
        }
    }

    public wl_res.SkillExtraAttrAdd getSkillExtraAttrAddByNum(int num)
    {
        if (m_SkillExtraAttrAddList.Count > num)
        {
            return m_SkillExtraAttrAddList[num];
        }
        return null;
    }

    public int GetSkillExtraAttrAddCnt()
    {
        return m_SkillExtraAttrAddList.Count;
    }

    public List<wl_res.SkillExtraAttrAdd> GetSkillExtraAttrAddListByHeroID(uint heroID)
    {
        List<wl_res.SkillExtraAttrAdd> list = new List<wl_res.SkillExtraAttrAdd>();
        for (int i = 0; i < m_SkillExtraAttrAddList.Count; i++)
        {
            //if (WLGame.GameSys.Get<WLGame.HeroDataMgrSys>().IsSameHeroid(m_SkillExtraAttrAddList[i].Heroid, heroID))
            //{
            //    list.Add(m_SkillExtraAttrAddList[i]);
            //}
        }
        return list;
    }
}



