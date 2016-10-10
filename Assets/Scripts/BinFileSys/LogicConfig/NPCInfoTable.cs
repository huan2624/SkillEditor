/*
 *       NPCInfoTable.cs
 *       Copyright (c) 2014, TopCloud Company 
 *       All rights reserved.
 *       Create By Sword.
 */

// Read the file has been defined in ResConfig.cs.


using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class NPCBaseInfoTable : LogicFileWithKey<UInt32, wl_res.NPCBaseInfo>
{
    public override UInt32 GetKey(wl_res.NPCBaseInfo Value)
	{
		return Value.Index;
	}

    public override void Init()
	{
		ReadBinFile("LocalConfig/NPC/NPCBaseInfo");
	}
}

public class BossSkillControlTable : LogicFileWithKey<UInt32, wl_res.BossSkillControl>
{
    public override UInt32 GetKey(wl_res.BossSkillControl Value)
    {
        return Value.Index;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/NPC/BossSkillControl");
    }
}