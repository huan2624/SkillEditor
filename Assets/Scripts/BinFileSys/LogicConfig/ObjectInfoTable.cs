/*
 *       ObjectInfoTable.cs
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

public class ObjectInfoTable : LogicFileWithKey<UInt32, wl_res.ObjectInfo>
{
	public override UInt32 GetKey(wl_res.ObjectInfo Value)
	{
		return Value.ObjectID;
	}

	public override void Init()
	{
		ReadBinFile("LocalConfig/ObjectConfig/ObjectInfo");
	}
}