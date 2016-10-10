/*
 *       GiftBagTable.cs
 *       Copyright (c) 2014, TopCloud Company 
 *       All rights reserved.
 *       Create By Sword.
 */

// Read the file has been defined in GameLevelConf.cs.

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;


class StoryTable : LogicFileWithoutKey<wl_res.BubbleUpInfo>
{
    Dictionary<uint, List<wl_res.BubbleUpInfo>> m_StoryTable = new Dictionary<uint, List<wl_res.BubbleUpInfo>>();

    public List<wl_res.BubbleUpInfo> GetStory(uint StoryID)
	{
        List<wl_res.BubbleUpInfo> StoryList = null;
        if (!m_StoryTable.TryGetValue(StoryID, out StoryList))
		{
            Debuger.LogError("GetStory Failed StoryID = " + StoryID);
		}
        return StoryList;
	}

	public override void Init()
	{
        ReadBinFile("LocalConfig/BubbleUp/BubbleUpInfo");

		int i = 0;

		uint curStoryID = 0;
        foreach (wl_res.BubbleUpInfo Value in GetTable())
		{
			if (Value.Id != 0)
			{
                curStoryID = Value.Id;
			}
			else
			{
                Value.Id = curStoryID;
			}

            List<wl_res.BubbleUpInfo> StoryList = null;
            if (!m_StoryTable.TryGetValue(curStoryID, out StoryList))
			{
                StoryList = new List<wl_res.BubbleUpInfo>();
                m_StoryTable.Add(curStoryID, StoryList);
			} 

            StoryList.Add(Value);
			
			++i;
		}
	}
}