///*
// *       GameLevelConfTable.cs
// *       Copyright (c) 2014, TopCloud Company 
// *       All rights reserved.
// *       Create By Sword.
// */

//// Read the file has been defined in GameLevelConf.cs.

//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Linq;
//using UnityEngine;

//// 关卡表需要特殊处理 因为关卡是需要顺序的 所以生成数组来确定关卡的排序
//// 然后又需要支持查找关卡 所以需要做还要再做一层封装
//public class GameLevelConfTable : LogicFileWithoutKey<wl_res.GameLevelConf>
//{
//    private Dictionary<UIGetWays.GetWayItem, List<UIGetWays.GetWayGameLevelValue>> m_getWays = new Dictionary<UIGetWays.GetWayItem, List<UIGetWays.GetWayGameLevelValue>>(new UIGetWays.GetWayItem());
//    // 关卡 场景ID 对应的 场景 索引表
//    Dictionary<uint, int> m_SceneIDIndexTable = new Dictionary<uint, int>();

//    public int GetDataIndex(uint LevelID)
//    {
//        int nIndex = -1;
//		if (!m_SceneIDIndexTable.TryGetValue(LevelID, out nIndex))
//        {
//			return -1;
//        }
//        return nIndex;
//    }

//    public wl_res.GameLevelConf GetDataByLevelID(uint LevelID)
//    {
//        int nIndex = GetDataIndex(LevelID);
//        if (nIndex == -1)
//        {
//            return null;
//        }

//        return GetData(nIndex);
//    }
    
//    public wl_res.GameLevelConf GetNextDataByLevelID(uint LevelID)
//    {
//        int iIdx = GetDataIndex(LevelID);
//        if (iIdx < 0)
//        {
//            return null;
//        }

//        wl_res.GameLevelConf conf = GetData(iIdx + 1);
//        if (conf == null)
//        {
//            return null;
//        }
//        return conf;
//    }

//    public int GetSize()
//    {
//        return m_SceneIDIndexTable.Count;
//    }

//    public List<UIGetWays.GetWayGameLevelValue> getItemGetWays(UIGetWays.GetWayItem item)
//    {
//        List<UIGetWays.GetWayGameLevelValue> getWayGameLevelList = null;
//        m_getWays.TryGetValue(item, out getWayGameLevelList);
//        return getWayGameLevelList;
//    }
    
//    public override void Init()
//    {
//        ReadBinFile("LocalConfig/Levels/GameLevelConf");

//        m_SceneIDIndexTable.Clear();

//        int i = 0;
//        List<wl_res.GameLevelConf> conTab = GetTable();
//        foreach (wl_res.GameLevelConf Value in conTab)
//        {
//            if (m_SceneIDIndexTable.ContainsKey(Value.LevelID))
//            {
//                Debuger.LogWarning("GameLevelConfTable already have a key == " + Value.LevelID);
//                continue;
//            }

//            m_SceneIDIndexTable.Add(Value.LevelID, i);
//            ++i;
//        }

//        WLGame.ConfigCenterMgrSys configMgrSys = WLGame.GameSys.Get<WLGame.ConfigCenterMgrSys>();
//        GiftBagTable giftTab = configMgrSys.GetLogicFile(WLGame.ConfigCenterMgrSys.ConfigName.GiftBag) as GiftBagTable;
//        GameLevelConfTable gameLevelConfTab = configMgrSys.GetLogicFile(WLGame.ConfigCenterMgrSys.ConfigName.GameLevelConf) as GameLevelConfTable;
//        wl_res.GameLevelConf gameLevelValue;

//        // 解析获取途径，保存起来,从2开始，去掉体验关
//        for (i = 2; i < m_SceneIDIndexTable.Count; ++i)
//        {
//            gameLevelValue = gameLevelConfTab.GetData(i);
//            for (int j = 0; j < gameLevelValue.DifficultData.Count(); ++j)
//            {
//                uint CertainGiftBagId = gameLevelValue.DifficultData[j].CertainGiftBagId;
//                List<wl_res.RandomItem> itemsList = giftTab.getGiftBag(CertainGiftBagId);
//                if (itemsList.Count == 0 && CertainGiftBagId != 0)
//                {
//                    uint levelId = gameLevelValue.LevelID;
//                    uint difficult = (uint)j;
//                    Debuger.LogError(string.Format("can't get gift {0} form level {1} difficult {2}", CertainGiftBagId, levelId, difficult));
//                }

//                for (int k = 0; k < itemsList.Count; ++k)
//                {
//                    UIGetWays.GetWayItem item = new UIGetWays.GetWayItem((wl_res.ResItemType)itemsList[k].Type, itemsList[k].Id);
//                    if (!m_getWays.ContainsKey(item))
//                    {
//                        List<UIGetWays.GetWayGameLevelValue> itemGetWayList = new List<UIGetWays.GetWayGameLevelValue>();
//                        m_getWays.Add(item, itemGetWayList);
//                    }
//                    UIGetWays.GetWayGameLevelValue getWayGameLevelValue = new UIGetWays.GetWayGameLevelValue(gameLevelValue.LevelID, gameLevelValue.DifficultData[j].Difficult);

//                    int index = -1;
//                    index = m_getWays[item].FindIndex(delegate(UIGetWays.GetWayGameLevelValue findValue)
//                    {
//                        return (findValue.m_gameLevelId == getWayGameLevelValue.m_gameLevelId && findValue.m_difficult == getWayGameLevelValue.m_difficult);
//                    });

//                    if (index == -1)
//                    {
//                        m_getWays[item].Add(getWayGameLevelValue);
//                    }
//                }

///*                itemsList = giftTab.getGiftBag(gameLevelValue.DifficultData[j].RandomGiftBagId);
//                for (int k = 0; k < itemsList.Count; ++k)
//                {
//                    UIGetWays.GetWayItem item = new UIGetWays.GetWayItem((wl_res.ResItemType)itemsList[k].Type, itemsList[k].Id);
//                    if (!m_getWays.ContainsKey(item))
//                    {
//                        List<UIGetWays.GetWayGameLevelValue> itemGetWayList = new List<UIGetWays.GetWayGameLevelValue>();
//                        m_getWays.Add(item, itemGetWayList);
//                    }
//                    UIGetWays.GetWayGameLevelValue getWayGameLevelValue = new UIGetWays.GetWayGameLevelValue(gameLevelValue.LevelID, gameLevelValue.DifficultData[j].Difficult);

//                    int index = -1;
//                    index = m_getWays[item].FindIndex(delegate(UIGetWays.GetWayGameLevelValue findValue)
//                    {
//                        return (findValue.m_gameLevelId == getWayGameLevelValue.m_gameLevelId && findValue.m_difficult == getWayGameLevelValue.m_difficult);
//                    });

//                    if (index == -1)
//                    {
//                        m_getWays[item].Add(getWayGameLevelValue);
//                    }
//                }
// */
//            }
//        }
//    }
//}

//public class LevelScenarioTriggerTable : LogicFileWithKey<UInt32, wl_res.LevelScenarioTriggerConf>
//{
//    public override UInt32 GetKey(wl_res.LevelScenarioTriggerConf Value)
//    {
//        return Value.Id;
//    }

//    public override void Init()
//    {
//        ReadBinFile("LocalConfig/Levels/LevelScenarioTriggerConf");
//    }
//}

//public class AthenaRefreshMonsterTable : LogicFileWithoutKey<wl_res.AthenaRefreshMonster>
//{
//    Dictionary<int, List<wl_res.AthenaRefreshMonster>> m_dicRefreshMonster = new Dictionary<int, List<wl_res.AthenaRefreshMonster>>();
    
//    public override void Init()
//    {
//        ReadBinFile("LocalConfig/Levels/AthenaRefreshMonster");
//        List<wl_res.AthenaRefreshMonster> lstRefreshMonster = GetTable();
//        for (int i = 0; i < lstRefreshMonster.Count; ++i )
//        {
//            List<wl_res.AthenaRefreshMonster> lstTmp;
//            if (!m_dicRefreshMonster.TryGetValue(lstRefreshMonster[i].Seq, out lstTmp))
//            {
//                lstTmp = new List<wl_res.AthenaRefreshMonster>();
//                m_dicRefreshMonster.Add(lstRefreshMonster[i].Seq, lstTmp);
//            }
//            lstTmp.Add(lstRefreshMonster[i]);
//        }
//    }
//    public wl_res.AthenaRefreshMonster GetConf(int iSeq, uint uRoleLevel)
//    {
//        List<wl_res.AthenaRefreshMonster> lstTmp;
//        if (m_dicRefreshMonster.TryGetValue(iSeq, out lstTmp))
//        {
//            for (int i = 0; i < lstTmp.Count; ++i)
//            {
//                if (uRoleLevel >= lstTmp[i].LevelLowerLimit && uRoleLevel <= lstTmp[i].LevelUpperLimit)
//                {
//                    return lstTmp[i];
//                }
//            }
//        }
//        return null;
//    }
//}

////守护雅典娜复活英雄配置
//public class AthenaReliveHeroTable : LogicFileWithKey<UInt32, wl_res.AthenaReviveHero>
//{
//    // 保存所有的index

//    public override UInt32 GetKey(wl_res.AthenaReviveHero Value)
//    {
//        return (uint)Value.ReviveNum;
//    }

//    public override void Init()
//    {
//        ReadBinFile("LocalConfig/Levels/AthenaReviveHero");
//    }
//}

////密室逃脱限制表
//public class RoomEscapeLimitTable : LogicFileWithKey<UInt32, wl_res.RoomEscapeLimit>
//{
//    public override UInt32 GetKey(wl_res.RoomEscapeLimit Value)
//    {
//        return (uint)Value.VipLevel;
//    }

//    public override void Init()
//    {
//        ReadBinFile("LocalConfig/Levels/RoomEscapeLimit");
//    }
//}

////密室重置消耗
//public class RoomEscapeResetCostTable : LogicFileWithKey<UInt32, wl_res.RoomEscapeResetCost>
//{
//    private uint m_maxNum = 0;
//    public override UInt32 GetKey(wl_res.RoomEscapeResetCost Value)
//    {
//        return (uint)Value.ResetNum;
//    }

//    public uint GetMaxResetNum()
//    {
//        return m_maxNum;
//    }

//    public override void Init()
//    {
//        ReadBinFile("LocalConfig/Levels/RoomEscapeResetCost");
//        Dictionary<UInt32, wl_res.RoomEscapeResetCost> diction = GetTable();
//        foreach (KeyValuePair<UInt32, wl_res.RoomEscapeResetCost> Pair in diction)
//        {
//            if (Pair.Value.ResetNum > m_maxNum)
//            {
//                m_maxNum = (uint)Pair.Value.ResetNum;
//            }
//        }
//    }
//}

////密室逃脱刷怪配置
//public class RealityroomRefreshMonsterTable : LogicFileWithKey<UInt32, wl_res.RealityroomMonster>
//{
//    // 保存所有的index
//    private List<int> m_monsterList = new List<int>();

//    public override UInt32 GetKey(wl_res.RealityroomMonster Value)
//    {
//        return (uint)Value.Index;
//    }

//    public override void Init()
//    {
//        ReadBinFile("LocalConfig/Levels/RealityroomMonster");
//        Dictionary<UInt32, wl_res.RealityroomMonster> lstRefreshMonster = GetTable();
//        foreach (KeyValuePair<UInt32, wl_res.RealityroomMonster> pair in lstRefreshMonster)
//        {
//            m_monsterList.Add(pair.Value.Index);
//        }
//    }

//    public List<wl_res.RealityroomMonster> GetMonster(int level)
//    {
//        List<wl_res.RealityroomMonster> lst = new List<wl_res.RealityroomMonster>();
//        for (int i = 0; i < m_monsterList.Count; ++i)
//        {
//            wl_res.RealityroomMonster monster;
//            GetData((uint)m_monsterList[i], out monster);
//            if (monster != null && monster.Level == level)
//            {
//                lst.Add(monster);
//            }
//        }

//        return lst;
//    }
//}

////溪谷对决刷怪表
//public class ValleysDuelMonsterTable : LogicFileWithoutKey<wl_res.ValleysDuelMonster>
//{
//    private List<wl_res.ValleysDuelMonster> m_dicRefreshMonster = new List<wl_res.ValleysDuelMonster>();

//    public wl_res.ValleysDuelMonster GetMonsterInfo(uint level)
//    {
//        for (int i = 0; i < m_dicRefreshMonster.Count;i++ )
//        {
//            if (m_dicRefreshMonster[i].Level == level)
//            {
//                return m_dicRefreshMonster[i];
//            }
//        }
//        return null;
//    }
//    public override void Init()
//    {
//        ReadBinFile("LocalConfig/Levels/ValleysDuelMonster");
//        m_dicRefreshMonster = GetTable();
       
//    }

//}

////溪谷对决限制表
//public class ValleysDuelLimitTable : LogicFileWithoutKey<wl_res.ValleysDuelLimit>
//{
//    public wl_res.ValleysDuelLimit m_ValleysDuelLimit = new wl_res.ValleysDuelLimit();

//    public override void Init()
//    {
//        ReadBinFile("LocalConfig/Levels/ValleysDuelLimit");
//        m_ValleysDuelLimit = GetTable()[0];
//    }
//}

//public class LevelResetTable : LogicFileWithKey<UInt32, wl_res.LevelReset>
//{
//    public UInt32 BuildKey(byte Difficult, byte ResetTimes)
//    {
//        UInt32 Key = Difficult;
//        Key = (Key << 8) + ResetTimes;
//        return Key;
//    }

//    public override UInt32 GetKey(wl_res.LevelReset Value)
//    {
//        return BuildKey((byte)Value.Difficult, (byte)Value.ResetTimes);
//    }

//    public override void Init()
//    {
//        ReadBinFile("LocalConfig/Levels/LevelReset");
//    }
//}