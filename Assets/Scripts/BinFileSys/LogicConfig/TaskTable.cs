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

class MainTaskTable : LogicFileWithKey<UInt32, wl_res.MainTask>
{
    public override UInt32 GetKey(wl_res.MainTask Value)
    {
        return Value.TaskId;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/task/MainTask");
    }
}

class DailyTaskTable : LogicFileWithKey<UInt32, wl_res.EverydayTask>
{
    public override UInt32 GetKey(wl_res.EverydayTask Value)
    {
        return Value.TaskId;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/task/EverydayTask");
    }
}

class ActivityTaskTable : LogicFileWithKey<UInt32, wl_res.Activity>
{
    public override UInt32 GetKey(wl_res.Activity Value)
    {
        return Value.ActivityId;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/task/Activity");
    }
}

class ActiveScoreAwardTable : LogicFileWithKey<UInt32, wl_res.ActiveScoreAward>
{
    public override UInt32 GetKey(wl_res.ActiveScoreAward Value)
    {
        return Value.SeqNo;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/task/ActiveScoreAward");
    }

    public List<UInt32> GetSeqNos()
    {
        Dictionary<UInt32,wl_res.ActiveScoreAward> values = GetTable();
        List<UInt32> keys = new List<uint>(values.Keys);
        return keys;
    }
}
