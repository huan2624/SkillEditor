/*
 *       HeroAttributesTable.cs
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

public class DrawCardFeeTable : LogicFileWithKey<UInt32, wl_res.DrawCardFee>
{
    public override UInt32 GetKey(wl_res.DrawCardFee Value)
    {
        return Value.DrawCardType;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Lottery/DrawCardFee");
    }
}

public class DrawCardFreeTimesTable : LogicFileWithKey<UInt32, wl_res.DrawCardFreeTimes>
{
    public override UInt32 GetKey(wl_res.DrawCardFreeTimes Value)
    {
        return Value.DrawCardType;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Lottery/DrawCardFreeTimes");
    }
}

public class DrawCardIntervalTable : LogicFileWithKey<UInt32, wl_res.DrawCardInterval>
{
    public override UInt32 GetKey(wl_res.DrawCardInterval Value)
    {
        return Value.DrawCardType;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Lottery/DrawCardInterval");
    }
}

public class LotteryShowHeroLimitTable : LogicFileWithKey<UInt32,wl_res.HeroShowLimit>
{
    public uint LimitStar = 0;
    public override UInt32 GetKey(wl_res.HeroShowLimit Value)
    {
        LimitStar = Value.HeroStar;
        return Value.HeroStar;
    }
    public override void Init()
    {
        ReadBinFile("LocalConfig/Lottery/HeroShowLimit");
    }
}

