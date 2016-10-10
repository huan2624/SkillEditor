using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

// public class ArenaRewardTable : LogicFileWithoutKey<wl_res.ArenaEverydayAwardConf>
// {
//     public List<wl_res.ArenaEverydayAwardConf> ArenaRewardList = new List<wl_res.ArenaEverydayAwardConf>();
//     public override void Init()
//     {
//         ReadBinFile("LocalConfig/Arena/ArenaEverydayAwardConf");
//         ArenaRewardList.Clear();
//         foreach (wl_res.ArenaEverydayAwardConf Value in GetTable())
//         {
//             ArenaRewardList.Add(Value);
//         }
//     }
// }


public class ArenaRankScoreTable : LogicFileWithKey<Int32, wl_res.ArenaRankScore>
{
    public override Int32 GetKey(wl_res.ArenaRankScore Value)
    {
        return Value.RankUpperLimit;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Arena/ArenaRankScore");
    }
}

public class ArenaShopTable : LogicFileWithKey<UInt32, wl_res.ArenaShop>
{
    public override UInt32 GetKey(wl_res.ArenaShop Value)
    {
        return Value.ID;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Arena/ArenaShop");
    }
}

public class ArenaPayCostTable : LogicFileWithKey<UInt32, wl_res.ArenaPayCost>
{
    public override UInt32 GetKey(wl_res.ArenaPayCost Value)
    {
        return Value.Times;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Arena/ArenaPayCost");
    }
}

public class ArenaFightParamTable : LogicFileWithKey<UInt32, wl_res.ArenaFightParam>
{
    public override UInt32 GetKey(wl_res.ArenaFightParam Value)
    {
        return Value.SerialNo;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Arena/ArenaFightParam");
    }
}

public class ArenaRefreshListTable : LogicFileWithKey<UInt32, wl_res.ArenaRefreshList>
{
    public override UInt32 GetKey(wl_res.ArenaRefreshList Value)
    {
        return Value.SerialNo;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Arena/ArenaRefreshList");
    }
}
