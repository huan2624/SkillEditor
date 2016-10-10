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


//每日签到表
public class PerdaySignInTable : LogicFileWithKey<UInt32, wl_res.PerdaySignIn>
{
    private List<wl_res.PerdaySignIn> m_AllPerdaySignInList = new List<wl_res.PerdaySignIn>();
    public override UInt32 GetKey(wl_res.PerdaySignIn Value)
    {
        return Value.LoggedInDays;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Reward/PerdaySignIn");
        m_AllPerdaySignInList.Clear();
        foreach (KeyValuePair<UInt32, wl_res.PerdaySignIn> Pair in GetTable())
        {
            m_AllPerdaySignInList.Add(Pair.Value);
        }
    }
    public List<wl_res.PerdaySignIn> GetAllPerdaySignIn()
    {
        if (m_AllPerdaySignInList.Count>0)
        {
            return m_AllPerdaySignInList;
        }
        return null;
    }
}
public class OnlineAwardConfTable : LogicFileWithKey<UInt32, wl_res.OnlineAwardConf>
{
    List<uint> m_allTimes = new List<uint>();

    public override UInt32 GetKey(wl_res.OnlineAwardConf Value)
    {
        return Value.Times;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Reward/OnlineAwardConf");

        m_allTimes.Clear();
        foreach (KeyValuePair<UInt32, wl_res.OnlineAwardConf> Pair in GetTable())
        {
            m_allTimes.Add(Pair.Value.Times);
        }
    }

    public List<uint> getAllTimes()
    {
        return m_allTimes;
    }
}

public class TestLoginAwardTable : LogicFileWithKey<UInt32, wl_res.TestLoginAward>
{
    public override UInt32 GetKey(wl_res.TestLoginAward Value)
    {
        return Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Reward/TestLoginAward");
    }
}
public class VIPRechargeTable : LogicFileWithKey<UInt32, wl_res.VIPRecharge>
{
    List<uint> m_allRechargeList = new List<uint>();

    public override UInt32 GetKey(wl_res.VIPRecharge Value)
    {
        return Value.VIPLevel;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Reward/VIPRecharge");

        m_allRechargeList.Clear();

        foreach (KeyValuePair<UInt32, wl_res.VIPRecharge> Pair in GetTable())
        {
            m_allRechargeList.Add(Pair.Value.VIPLevel);
        }
    }

    public List<uint> getAllRechargeList()
    {
        return m_allRechargeList;
    }
}

