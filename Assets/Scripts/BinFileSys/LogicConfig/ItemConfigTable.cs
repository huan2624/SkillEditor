/*
 *       ItemConfigTable.cs
 *       Copyright (c) 2014, TopCloud Company 
 *       All rights reserved.
 *       Create By Sword.
 */

// Read the file has been defined in item_conf_client.cs.

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EquipBaseInfoTable : LogicFileWithKey<UInt32, wl_res.EquipBaseInfo>
{
    private List<uint> m_equipList = new List<uint>();

    public override UInt32 GetKey(wl_res.EquipBaseInfo Value)
	{
		return Value.Id;
	}     

    public override void Init()
	{
		ReadBinFile("LocalConfig/Item/EquipBaseInfo");

        Dictionary<UInt32, wl_res.EquipBaseInfo> table = GetTable();
        foreach (KeyValuePair<UInt32, wl_res.EquipBaseInfo> value in table)
        {
            // 只添加白色
            if (value.Value.Color == (byte)wl_res.ItemColorType.RES_ITEM_COLOR_TYPE_WHITE)
            {
                m_equipList.Add(value.Key);
            }
        }       
	}        

    public List<uint> getAllEquip()
    {
        return m_equipList;
    }
}

public class EquipAttributeTable : LogicFileWithKey<Int64, wl_res.EquipAttribute>
{
    public override Int64 GetKey(wl_res.EquipAttribute Value)
    {
        return BuildKey(Value.Id, Value.ColorLevel);
    }

    public Int64 BuildKey(uint id, uint ColorLevel)
    {
        Int64 Key = ((Int64)id << 32) + ColorLevel;
        return Key;     
    }        

    public override void Init()
    {
        ReadBinFile("LocalConfig/Item/EquipAttribute");
    }
}

public class MaterialBaseInfoTable : LogicFileWithKey<UInt32, wl_res.MaterialBaseInfo>
{
    private List<uint> m_expList = new List<uint>();

    public override UInt32 GetKey(wl_res.MaterialBaseInfo Value)
	{
		return Value.Id;
	}

    public override void Init()
	{
		ReadBinFile("LocalConfig/Item/MaterialBaseInfo");

        foreach (KeyValuePair<UInt32, wl_res.MaterialBaseInfo> Pair in GetTable())
        {
            // 获取所有经验
            if (Pair.Value.FuncId == 1002)
            {
                m_expList.Add(Pair.Key);
            }
        }
	}

    public List<uint> getAllExp()
    {
        // 获取所有经验药水
        return m_expList;
    }
}

public class EquipUpgradeTable : LogicFileWithKey<UInt32, wl_res.EquipUpgrade>
{
    public override UInt32 GetKey(wl_res.EquipUpgrade Value)
    {
        return Value.Lv;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Item/EquipUpgrade");
    }
}

// 专属装备升级材料
public class ExclusiveEquipUpMatTable : LogicFileWithoutKey<wl_res.ExclusiveEquipUpgradeMaterial>
{
    private uint m_upMatId;     // 升级材料id

    public override void Init()
    {
        ReadBinFile("LocalConfig/Item/ExclusiveEquipUpgradeMaterial");
        List<wl_res.ExclusiveEquipUpgradeMaterial> upMatList = GetTable();
        if (upMatList.Count == 1)
        {
            m_upMatId = upMatList[0].MaterialID;
        }
    }

    public uint GetUpMatId()
    {
        return m_upMatId;
    }
}

// 专属装备升级
public class ExclusiveEquipUpgradeTable : LogicFileWithKey<UInt32, wl_res.ExclusiveEquipUpgrade>
{
    private uint m_maxLevel;        // 最高等级

    public uint GetMaxLevel()
    {
        return m_maxLevel;
    }

	public override UInt32 GetKey(wl_res.ExclusiveEquipUpgrade Value)
	{
		return Value.Lv;
	}

    public override void Init()
    {
        ReadBinFile("LocalConfig/Item/ExclusiveEquipUpgrade");

        Dictionary<UInt32, wl_res.ExclusiveEquipUpgrade> dictionary = GetTable();
        foreach (KeyValuePair<UInt32, wl_res.ExclusiveEquipUpgrade> Pair in dictionary)
        {
            // 获取所有经验
            if (Pair.Value.Lv > m_maxLevel)
            {
                m_maxLevel = Pair.Value.Lv;
            }
        }
    }
}

// 装备洗练
public class EquipWashTable : LogicFileWithoutKey<wl_res.EquipWash>
{
    private wl_res.EquipWash m_equipWash;     // 升级材料id

    public override void Init()
    {
        ReadBinFile("LocalConfig/Item/EquipWash");
        List<wl_res.EquipWash> washList = GetTable();
        if (washList.Count == 1)
        {
            m_equipWash = washList[0];
        }
    }

    public wl_res.EquipWash GetEquipWash()
    {
        return m_equipWash;
    }
}

// 专属装备分解合成
public class ExclusiveEquipComposeTable : LogicFileWithoutKey<wl_res.ExclusiveEquipCompose>
{
    private wl_res.ExclusiveEquipCompose m_equipInfo;     

    public override void Init()
    {
        ReadBinFile("LocalConfig/Item/ExclusiveEquipCompose");
        List<wl_res.ExclusiveEquipCompose> EquipList = GetTable();
        if (EquipList.Count == 1)
        {
            m_equipInfo = EquipList[0];
        }
    }

    public wl_res.ExclusiveEquipCompose GetEquipWash()
    {
        return m_equipInfo;
    }
}

//装备品阶
public class EquipColorTable : LogicFileWithKey<UInt32, wl_res.EquipColor>
{         
    public override UInt32 GetKey(wl_res.EquipColor Value)
    {
        return BuildKey(Value.Color, Value.ColorLevel);
    }
    public UInt32 BuildKey(byte Color, byte ColorLevel)
    {
        UInt32 Key = Color;
        Key = (Key << 8) + ColorLevel;
        return Key;
    }          
    public override void Init()
    {
        ReadBinFile("LocalConfig/Item/EquipColor");       
    }
}

//装备品阶数量
public class EquipColorNumTable : LogicFileWithKey<UInt32, wl_res.EquipColorNum>
{
    public override UInt32 GetKey(wl_res.EquipColorNum Value)
    {
        return Value.Color;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Item/EquipColorNum");
    }

    public uint GetEquipColorNum(byte color)
    {
        Dictionary<UInt32, wl_res.EquipColorNum> dictionary = GetTable();
        foreach (KeyValuePair<UInt32, wl_res.EquipColorNum> Pair in dictionary)
        {
            if (Pair.Key == color)
            {
                return Pair.Value.Num;
            }
        }
        return 0;
    }
        
}


// 掉落库
public class DropTable : LogicFileWithKey<UInt32, wl_res.Drop>
{                                    
    public override UInt32 GetKey(wl_res.Drop Value)
    {
        return Value.Id;
    }
    
    public override void Init()
    {
        ReadBinFile("LocalConfig/Item/Drop");     
    }      
}
