/*
 *       BigLevelInfoTable.cs
 *       Copyright (c) 2014, TopCloud Company 
 *       All rights reserved.
 *       Create By Sword.
 */

// Read the file has been defined in BigLevelInfo.cs.

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;


public class BigLevelInfoTable : LogicFileWithKey<UInt32, wl_res.BigLevelInfo>
{
	public override UInt32 GetKey(wl_res.BigLevelInfo Value)
	{
		return Value.BigLevelID;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/Levels/BigLevelInfo");
	}
}