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
    public uint m_maxLevel;                     // ���vip�ȼ�
    private uint m_minRoomSweepVipLv = 0;       // ���vip����ɨ���������ѵȼ�
    private uint m_minGameLevelSweepVipLv = 0;       // ��͹ؿ�����ɨ��VIP�ȼ�

    public uint GetMinRoomSweepVipLevel()
    {
        // ��ȡ��Ϳ�ɨ����������vip�ȼ�
        return m_minRoomSweepVipLv;
    }

    public uint GetMinGameLevelSweepVipLevel()
    {
        // ��ȡ��Ϳ�ɨ���ؿ�vip�ȼ�
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
                // �������ѿ�ɨ��vip�ȼ�
                m_minRoomSweepVipLv = Pair.Value.VIPLevel;
            }

            if (Pair.Value.IsOpenGameLevelMassSweep == 1 &&  m_minRoomSweepVipLv == 0)
            {
                // �ؿ���ɨ��vip�ȼ�
                m_minGameLevelSweepVipLv = Pair.Value.VIPLevel;
            }
        }
    }


}

