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

public class BuyStaminaTable : LogicFileWithKey<UInt32, wl_res.BuyStamina>
{
    public override UInt32 GetKey(wl_res.BuyStamina Value)
    {
        return Value.time;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Role/BuyStamina");
    }
}

public class StaminaConfigTable : LogicFileWithKey<UInt32, wl_res.StaminaConfig>
{
    public override UInt32 GetKey(wl_res.StaminaConfig Value)
    {
        return Value.type;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Role/StaminaConfig");
    }
}

class StaminaAndRoleLvTable : LogicFileWithKey<UInt32, wl_res.StaminaAndRoleLv>
{
    public override UInt32 GetKey(wl_res.StaminaAndRoleLv Value)
    {
        return (UInt32)Value.Lv;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Role/StaminaAndRoleLv");
    }
}
