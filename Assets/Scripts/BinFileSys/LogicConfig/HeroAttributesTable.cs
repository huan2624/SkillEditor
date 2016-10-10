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

public class HeroBasicAttributesTable : LogicFileWithKey<UInt32, wl_res.HeroBasicAttributes>
{
    public override UInt32 GetKey(wl_res.HeroBasicAttributes Value)
	{
		return Value.heroID;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Hero/HeroBasicAttributes");
	}
}

// public class HeroSkillSlotTable : LogicFileWithKey<UInt32, wl_res.HeroSkillSlot>
// {
// }

// public class HeroEquipPlaceTable : LogicFileWithKey<UInt32, wl_res.HeroEquipPlace>
// {
// 	
// 	UInt32 GetKey(wl_res.HeroEquipPlace Value)
// 	{
// 		UInt32 Key = Value.LevelMax;
// 		Key = (Key<<8) + Value.RoleStarLevel;
// 		return Key;
// 	}
// 
// 	byte GetLevelMaxFromKey(UInt32 Key)
// 	{
// 		return (byte)(Key>>8);
// 	}
// 
// 	byte GetRoleStarLevelFromKey(UInt32 Key)
// 	{
// 		return (byte)(Key);
// 	}
// 
// 	UInt32 BuildKey( byte LevelMax, byte RoleStar )
// 	{
// 
// 	}
// 
// 	public void Init()
// 	{
// 		ReadBinFile("LocalConfig/RolesInfo/HeroEquipPlace");
// 	}
// }

public class HeroUpgradeTable : LogicFileWithKey<UInt32, wl_res.HeroUpgrade>
{
    // 最大等级
    public static uint m_maxLevel = 0;

    public override UInt32 GetKey(wl_res.HeroUpgrade Value)
	{
		return Value.Level;
	}

    public override void Init()
	{
		ReadBinFile("LocalConfig/Hero/hero_upgrade");

		foreach (KeyValuePair<uint, wl_res.HeroUpgrade> pair in GetTable())
        {
            if (pair.Value.Level > m_maxLevel)
            {
                m_maxLevel = pair.Value.Level;
            }
        }
	}
}

public class RoleResourcesTable : LogicFileWithKey<UInt32, wl_res.RoleResources>
{
	public override UInt32 GetKey(wl_res.RoleResources Value)
	{
		return Value.ID;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Hero/RoleResources");
	}
}

//public class HeroColorTable : LogicFileWithKey<UInt32, wl_res.HeroColor>
//{
//    public UInt32 BuildKey(byte Color, byte ColorLevel)
//    {
//        UInt32 Key = Color;
//        Key = (Key << 8) + ColorLevel;
//        return Key;
//    }
//
//    public override UInt32 GetKey(wl_res.HeroColor Value)
//    {
//        return BuildKey(Value.Color, Value.ColorLevel);
//    }
//
//    public override void Init()
//    {
//        ReadBinFile("LocalConfig/Hero/HeroColor");
//    }
//}

public class HeroCombineTable : LogicFileWithKey<UInt32, wl_res.HeroCombine>
{
    List<uint> m_allHeroList = new List<uint>();      
    public override UInt32 GetKey(wl_res.HeroCombine Value)
    {
        return Value.HeroId;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Hero/HeroCombine");

        m_allHeroList.Clear();

        foreach (KeyValuePair<UInt32, wl_res.HeroCombine> Pair in GetTable())
        {
            if (Pair.Value.CanCombine == 1)
            {
                m_allHeroList.Add(Pair.Value.HeroId);
            }
        }
    }        

    public List<uint> getAllHeroList()
    {
        return m_allHeroList;
    }
}

public class HeroAdvanceTable : LogicFileWithKey<UInt32, wl_res.HeroRiseColor>
{
    private List<uint> m_allLevel = new List<uint>();

    public List<uint> GetAllLevel()
    {
        return m_allLevel;
    }

    public override UInt32 GetKey(wl_res.HeroRiseColor Value)
	{
		return Value.Level;
	}

	public override void Init()
	{
        ReadBinFile("LocalConfig/Hero/HeroRiseColor");

        // 获取所有等级
        Dictionary<UInt32, wl_res.HeroRiseColor> dictionary = GetTable();
        foreach (KeyValuePair<UInt32, wl_res.HeroRiseColor> Pair in dictionary)
        {
            m_allLevel.Add(Pair.Value.Level);
        }
	}
}

public class HeroWearEquipTable : LogicFileWithKey<UInt32, wl_res.HeroWearEquip>
{
    public override UInt32 GetKey(wl_res.HeroWearEquip Value)
    {
        return Value.HeroType;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Hero/HeroWearEquip");
    }
}

public class HeroUpgradeLimitTable : LogicFileWithKey<UInt32, wl_res.HeroUpgradeLimit>
{
    public override UInt32 GetKey(wl_res.HeroUpgradeLimit Value)
    {
        return Value.RoleLevel;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Hero/HeroUpgradeLimit");
    }
}

public class HeroRecommendTable : LogicFileWithKey<UInt32, wl_res.HeroRecommend>
{
    private List<UInt32>[] m_recommList = 
    {
        new List<UInt32>(),
        new List<UInt32>(),
        new List<UInt32>(),
    };

    public List<UInt32>[] GetRecommList()
    {
        return m_recommList;
    }

    public override UInt32 GetKey(wl_res.HeroRecommend Value)
    {
        return Value.Index;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Hero/HeroRecommend");

        foreach (KeyValuePair<UInt32, wl_res.HeroRecommend> Pair in GetTable())
        {
            m_recommList[Pair.Value.position - 1].Add(Pair.Value.Index);
        }
    }
}

//战斗力特效
public class HeroFightCapacityTable : LogicFileWithKey<UInt32, wl_res.HeroFightCapacity>
{
    private List<uint> m_AllFightCapacityNumber = new List<uint>();
    public override UInt32 GetKey(wl_res.HeroFightCapacity Value)
    {
        return Value.FightCapacityNumber;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Hero/HeroFightCapacity");
        foreach (KeyValuePair<UInt32, wl_res.HeroFightCapacity> Pair in GetTable())
        {
            m_AllFightCapacityNumber.Add(Pair.Value.FightCapacityNumber);
        }
    }

    public List<uint> GetAllFightCapacityNumber()
    {
        return m_AllFightCapacityNumber;
    }
}

//升星
public class HeroRiseStarTable : LogicFileWithKey<UInt32, wl_res.HeroRiseStar>
{
    private List<uint> m_AllHeroRiseStar = new List<uint>();
    public override UInt32 GetKey(wl_res.HeroRiseStar Value)
    {
        return Value.Star;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Hero/HeroRiseStar");
        foreach (KeyValuePair<UInt32, wl_res.HeroRiseStar> Pair in GetTable())
        {
            m_AllHeroRiseStar.Add(Pair.Value.Star);
        }
    }

    public List<uint> GetAllHeroRiseStar()
    {
        return m_AllHeroRiseStar;
    }
}


//升阶
public class HeroRiseColorTable : LogicFileWithKey<UInt32, wl_res.HeroRiseColor>
{
    private List<uint> m_AllHeroRiseColor = new List<uint>();
    private List<wl_res.HeroRiseColor> m_lstHeroRiseColorConfig = new List<wl_res.HeroRiseColor>();
    public override UInt32 GetKey(wl_res.HeroRiseColor Value)
    {
        return Value.Level;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Hero/HeroRiseColor");
        foreach (KeyValuePair<UInt32, wl_res.HeroRiseColor> Pair in GetTable())
        {
            m_AllHeroRiseColor.Add(Pair.Value.Level);
            m_lstHeroRiseColorConfig.Add(Pair.Value);
        }
    }

    public List<uint> GetAllHeroRiseColor()
    {
        return m_AllHeroRiseColor;
    }

    public List<wl_res.HeroRiseColor> GetHeroRiseColorConfig()
    {
        return m_lstHeroRiseColorConfig;
    }
}

//英雄星座图
public class HeroStarFigureTable : LogicFileWithKey<UInt32, wl_res.HeroStarFigure>
{
    public override UInt32 GetKey(wl_res.HeroStarFigure Value)
    {
        return Value.HeroId;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Hero/HeroStarFigure");
    }
}