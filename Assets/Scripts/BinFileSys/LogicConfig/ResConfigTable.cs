/*
 *       ResConfigTable.cs
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

public class ResConfigTable : LogicFileWithKey<UInt32, wl_res.ResConfig>
{
    public override UInt32 GetKey(wl_res.ResConfig Value)
	{
		return Value.ResID;
	}

    public override void Init()
	{
		ReadBinFile("LocalConfig/ResConfig/ResConfig");
	}
}

public class MaterialSoundTable : LogicFileWithKey<UInt32, wl_res.MaterialSound>
{
    public override UInt32 GetKey(wl_res.MaterialSound Value)
	{
		return Value.MaterialSoundID;
	}

    public override void Init()
	{
		ReadBinFile("LocalConfig/ResConfig/MaterialSound");
	}
}