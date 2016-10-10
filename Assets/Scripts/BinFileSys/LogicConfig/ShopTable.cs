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

public class NormalShopTable : LogicFileWithKey<UInt32, wl_res.NormalShop>
{
    public override UInt32 GetKey(wl_res.NormalShop Value)
    {
        return Value.SerialNo;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Shop/NormalShop");

        foreach (KeyValuePair<UInt32, wl_res.NormalShop> Pair in GetTable())
        {
            m_NormalShop = Pair.Value;
        }
    }

    public wl_res.NormalShop GetConf()
    {
        return m_NormalShop;
    }


    private wl_res.NormalShop m_NormalShop;
}                 

public class VIPShopTable : LogicFileWithKey<UInt32, wl_res.VIPShop>
{
    public override UInt32 GetKey(wl_res.VIPShop Value)
    {
        return Value.SerialNo;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Shop/VIPShop");
    }
}

public class RechargeTable : LogicFileWithKey<UInt32, wl_res.Recharge>
{
    public override UInt32 GetKey(wl_res.Recharge Value)
    {
        return Value.SerialNo;
    }
    List<wl_res.Recharge> m_RechargeList = new List<wl_res.Recharge>();
    public override void Init()
    {
        ReadBinFile("LocalConfig/Shop/Recharge");

        m_RechargeList.Clear();

        foreach (KeyValuePair<UInt32, wl_res.Recharge> Pair in GetTable())
        {
            m_RechargeList.Add(Pair.Value);
        }
    }

    public List<wl_res.Recharge> getAllRechargeList()
    {
        if (0 != m_RechargeList.Count)
        {
            return m_RechargeList;
        }
        return null;
    }                                                                            
}


//普通商店刷新时间
public class NormalRefreshTimeTable : LogicFileWithKey<UInt32, wl_res.NormalRefreshTime>
{
    public override UInt32 GetKey(wl_res.NormalRefreshTime Value)
    {
        return Value.SerialNo;
    }
    private List<wl_res.NormalRefreshTime> m_allNormalRefreshTimeList = new List<wl_res.NormalRefreshTime>();

    public override void Init()
    {
        ReadBinFile("LocalConfig/Shop/NormalRefreshTime");

        m_allNormalRefreshTimeList.Clear();
        foreach (KeyValuePair<uint , wl_res.NormalRefreshTime> item in GetTable())
        {
            m_allNormalRefreshTimeList.Add(item.Value);
        }
    }
    public List<wl_res.NormalRefreshTime> GetAllNormalRefreshTimeList()
    {
        if (m_allNormalRefreshTimeList.Count!=0)
        {
            return m_allNormalRefreshTimeList;
        }
        return null;
    }
}
//特殊物品
public class SpecialGoodsTable : LogicFileWithKey<UInt32, wl_res.SpecialGoods>
{
    public override UInt32 GetKey(wl_res.SpecialGoods Value)
    {
        return Value.SerialNo;
    }
    public override void Init()
    {
        ReadBinFile("LocalConfig/Shop/SpecialGoods");
    }
}

//商店刷新
public class ShopRefreshTable : LogicFileWithKey<UInt32, wl_res.ShopRefresh>
{
    wl_res.ShopRefresh m_ShopRefresh = new wl_res.ShopRefresh();
    public override UInt32 GetKey(wl_res.ShopRefresh Value)
    {
        return Value.SerialNo;
    }
    public override void Init()
    {
        ReadBinFile("LocalConfig/Shop/ShopRefresh");
        m_ShopRefresh = new wl_res.ShopRefresh();

        foreach (KeyValuePair<UInt32, wl_res.ShopRefresh> Pair in GetTable())
        {
            m_ShopRefresh = Pair.Value;
            return;
        }
    }
    public wl_res.ShopRefresh GetShopRefresh()
    {
        return m_ShopRefresh;
    } 
}


//VIP商店开启等级
public class OpenVIPShopLevelTable : LogicFileWithKey<UInt32, wl_res.OpenVIPShopLevel>
{
    wl_res.OpenVIPShopLevel m_OpenVIPShopLevel = new wl_res.OpenVIPShopLevel();
    public override UInt32 GetKey(wl_res.OpenVIPShopLevel Value)
    {
        return Value.level;
    }
    public override void Init()
    {
        ReadBinFile("LocalConfig/Shop/OpenVIPShopLevel");
        m_OpenVIPShopLevel = new wl_res.OpenVIPShopLevel();

        foreach (KeyValuePair<UInt32, wl_res.OpenVIPShopLevel> Pair in GetTable())
        {
            m_OpenVIPShopLevel = Pair.Value;
            return;
        }
    }
    public uint GetOpenVIPShopLevel()
    {
        return m_OpenVIPShopLevel.level;
    }
}
