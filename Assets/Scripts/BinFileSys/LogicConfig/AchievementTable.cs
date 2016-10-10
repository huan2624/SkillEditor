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

public class AchievementTable : LogicFileWithKey<UInt32, wl_res.Achievement>
{
    public override UInt32 GetKey(wl_res.Achievement Value)
    {
        return Value.AchievementId;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Achievement/Achievement");
    }
}

public class HandbookAchievementTable : LogicFileWithKey<UInt32, wl_res.HandbookAchievement>
{
    public override UInt32 GetKey(wl_res.HandbookAchievement Value)
    {
        return Value.AchievementId;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Achievement/HandbookAchievement");
    }
}

