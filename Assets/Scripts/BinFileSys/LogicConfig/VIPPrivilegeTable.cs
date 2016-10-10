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

public class VIPPrivilegeTable : LogicFileWithKey<UInt32, wl_res.VIPPrivilege>
{
    public uint m_maxLevel;                     // 最高vip等级
    private uint m_minRoomSweepVipLv = 0;       // 最低vip可以扫荡密室逃脱等级
    private uint m_minGameLevelSweepVipLv = 0;       // 最低关卡可以扫荡VIP等级

    public uint GetMinRoomSweepVipLevel()
    {
        // 获取最低可扫荡密室逃脱vip等级
        return m_minRoomSweepVipLv;
    }

    public uint GetMinGameLevelSweepVipLevel()
    {
        // 获取最低可扫荡关卡vip等级
        return m_minGameLevelSweepVipLv;
    }

    public override UInt32 GetKey(wl_res.VIPPrivilege Value)
    {
        return Value.VIPLevel;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Reward/VIPPrivilege");

        foreach (KeyValuePair<UInt32, wl_res.VIPPrivilege> Pair in GetTable())
        {
            if (Pair.Value.VIPLevel > m_maxLevel)
            {
                m_maxLevel = Pair.Value.VIPLevel;
            }

            if (Pair.Value.IsRealityRoomSweepOpen == 1 &&  m_minRoomSweepVipLv == 0)
            {
                // 密室逃脱可扫荡vip等级
                m_minRoomSweepVipLv = Pair.Value.VIPLevel;
            }

            if (Pair.Value.IsOpenGameLevelMassSweep == 1 &&  m_minRoomSweepVipLv == 0)
            {
                // 关卡可扫荡vip等级
                m_minGameLevelSweepVipLv = Pair.Value.VIPLevel;
            }
        }
    }


}

