/*
 *       StaminaTable.cs
 *       Copyright (c) 2014, TopCloud Company 
 *       All rights reserved.
 *       Create By zln.
 */

// Read the file has been defined in HeroBasicAttributes.cs.

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class MoneyTreeCostTable : LogicFileWithKey<UInt32, wl_res.MoneyTreeCost>
{
    public override UInt32 GetKey(wl_res.MoneyTreeCost Value)
    {
        return Value.Times;
    }

	uint m_MaxKey = 0;

	public uint GetMaxKey()
	{
		return m_MaxKey;
	}

    public bool GetRealData(UInt32 key, out wl_res.MoneyTreeCost Value)
	{
		if (key > m_MaxKey)
		{
			key = m_MaxKey;
		}
		return m_LogicDataTable.TryGetValue(key, out Value);
	}

    public override void Init()
    {
        ReadBinFile("LocalConfig/MoneyTree/MoneyTreeCost");

        Dictionary<UInt32, wl_res.MoneyTreeCost>.Enumerator Enumerator = m_LogicDataTable.GetEnumerator();
		for (; Enumerator.MoveNext(); )
		{
			if (Enumerator.Current.Key > m_MaxKey)
			{
				m_MaxKey = Enumerator.Current.Key;
			}
		}
    }
}

public class MoneyTreeTable : LogicFileWithKey<UInt32, wl_res.MoneyTree>
{
    public override UInt32 GetKey(wl_res.MoneyTree Value)
    {
        return Value.RoleLevel;
    }

    uint m_MaxKey = 0;

    public uint GetMaxKey()
    {
        return m_MaxKey;
    }

    public uint GetGold(uint Level)
    {
        wl_res.MoneyTree Value = null;
        if (!GetRealData(Level, out Value))
        {
            Debuger.LogError("当前等级没有配置！ Level:" + Level);
            return 0;
        }
        else
        {
            return Value.Gold;
        }
    }

    public bool GetRealData(UInt32 key, out wl_res.MoneyTree Value)
    {
        if (key > m_MaxKey)
        {
            key = m_MaxKey;
        }
        return m_LogicDataTable.TryGetValue(key, out Value);
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/MoneyTree/MoneyTree");

        Dictionary<UInt32, wl_res.MoneyTree>.Enumerator Enumerator = m_LogicDataTable.GetEnumerator();
        for (; Enumerator.MoveNext(); )
        {
            if (Enumerator.Current.Key > m_MaxKey)
            {
                m_MaxKey = Enumerator.Current.Key;
            }
        }
    }
}


// VIP暴击倍数
public class VIPStrikeProbabilityTable : LogicFileWithKey<UInt32, wl_res.VIPStrikeProbability>
{
    public override UInt32 GetKey(wl_res.VIPStrikeProbability Value)
    {
        return Value.VipLevel;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/MoneyTree/VIPStrikeProbability");
    }
}

