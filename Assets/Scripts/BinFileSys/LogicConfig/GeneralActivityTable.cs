using System;
using System.Collections.Generic;
using System.Text;

class GeneralActivityTable : LogicFileWithKey<UInt32, wl_res.GeneralActivity>
{
    public override UInt32 GetKey(wl_res.GeneralActivity Value)
    {
        return (UInt32)Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Activity/GeneralActivity");
    }
}

class WithSubTaskActivityTable : LogicFileWithKey<UInt32, wl_res.WithSubTaskActivity>
{
    public override UInt32 GetKey(wl_res.WithSubTaskActivity Value)
    {
        return (UInt32)Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Activity/WithSubTaskActivity");
    }
}


class LevelUpAwardTable : LogicFileWithKey<UInt32, wl_res.LevelUpAward>
{
    public override UInt32 GetKey(wl_res.LevelUpAward Value)
    {
        return (UInt32)Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Activity/LevelUpAward");
    }
}

class FamilyGrowTable : LogicFileWithKey<UInt32, wl_res.FamilyGrow>
{
    public override UInt32 GetKey(wl_res.FamilyGrow Value)
    {
        return (UInt32)Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Activity/FamilyGrow");
    }
}

class GetStaminaTable : LogicFileWithKey<UInt32, wl_res.GetStamina>
{
    public override UInt32 GetKey(wl_res.GetStamina Value)
    {
        return (UInt32)Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Activity/GetStamina");
    }
}

class ActivityNewPlayerAwardTable : LogicFileWithKey<UInt32, wl_res.ActivityNewPlayerAward>
{
    public override UInt32 GetKey(wl_res.ActivityNewPlayerAward Value)
    {
        return (UInt32)Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Activity/ActivityNewPlayerAward");
    }
}

//每日充值
class EverydayRechargeTable : LogicFileWithKey<UInt32, wl_res.EverydayRecharge>
{
    public override UInt32 GetKey(wl_res.EverydayRecharge Value)
    {
        return (UInt32)Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Activity/EverydayRecharge");
    }
}

//累积充值
class AccumulatedRechargeTable : LogicFileWithKey<UInt32, wl_res.AccumulatedRecharge>
{
    public override UInt32 GetKey(wl_res.AccumulatedRecharge Value)
    {
        return (UInt32)Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Activity/AccumulatedRecharge");
    }
}

//累积消费
class AccumulatedConsumeTable : LogicFileWithKey<UInt32, wl_res.AccumulatedConsume>
{
    public override UInt32 GetKey(wl_res.AccumulatedConsume Value)
    {
        return (UInt32)Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Activity/AccumulatedConsume");
    }
}

//老虎机
class TigerAndLotteryTable : LogicFileWithoutKey<wl_res.TigerAndLottery>
{         
    // 老虎机 key是 序号 value是需要钻石
    Dictionary<uint, uint> m_NeedDiamondNum = new Dictionary<uint, uint>();
   
    public override void Init()
    {
        ReadBinFile("LocalConfig/Activity/TigerAndLottery");

        m_NeedDiamondNum.Clear();
        foreach (wl_res.TigerAndLottery item in GetTable())
        {
            if (m_NeedDiamondNum.ContainsKey(item.Seq)==false)
            {
                m_NeedDiamondNum.Add(item.Seq, item.NeedDiamondNum);
            }
        }
    }
    public Dictionary<uint, uint> GetAllTigerTab()
    {
        if (m_NeedDiamondNum.Keys.Count > 0)
        {
            return m_NeedDiamondNum; 
        }
        return null;
    }              

    public uint GetNeedDiamondNum(uint seq)
    {
        if (m_NeedDiamondNum.ContainsKey(seq))
        {
            return m_NeedDiamondNum[seq];
        }
        return 0;
    }
}


//充值返利
class RechargeRebateTable : LogicFileWithKey<UInt32, wl_res.RechargeRebate>
{
    public override UInt32 GetKey(wl_res.RechargeRebate Value)
    {
        return (UInt32)Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Activity/RechargeRebate");
    }
}
