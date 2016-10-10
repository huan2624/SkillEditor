using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

class FamilyBasicConfTable : LogicFileWithKey<UInt32, wl_res.FamilyBasicConf>
{
    public override UInt32 GetKey(wl_res.FamilyBasicConf Value)
    {
        return (UInt32)Value.Level;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Family/FamilyBaisc");
    }
}

class FamilyBuildConfTable : LogicFileWithKey<UInt32, wl_res.FamilyBuildConf>
{
    public override UInt32 GetKey(wl_res.FamilyBuildConf Value)
    {
        return (UInt32)Value.BuildType;
    }
    public override void Init()
    {
        ReadBinFile("LocalConfig/Family/FamilyBuild");
    }
}

class FamilyLimitConfTable : LogicFileWithKey<UInt32, wl_res.FamilyLimitConf>
{
    public override UInt32 GetKey(wl_res.FamilyLimitConf Value)
    {
        return (UInt32)Value.VipLevel;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Family/FamilyLimit");
    }
}

class FamilyCreatConfTable : LogicFileWithoutKey<wl_res.CreateFamily>
{
    public wl_res.CreateFamily CreatFamilyInfo = new wl_res.CreateFamily();

    public override void Init()
    {
        ReadBinFile("LocalConfig/Family/CreateFamily");

        CreatFamilyInfo =(wl_res.CreateFamily)GetTable()[0];
    }
}
class FamilyBossConfTable : LogicFileWithKey<UInt32,wl_res.FamilyBOSS>
{
    public override UInt32 GetKey(wl_res.FamilyBOSS Value)
    {
        return (UInt32)Value.No;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Family/FamilyBOSS");
    }
}
class FamilyShopConfTable : LogicFileWithKey<UInt32,wl_res.FamilyShop>
{
    public override UInt32 GetKey(wl_res.FamilyShop Value)
    {
        return (UInt32)Value.SerialNo;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/shop/FamilyShop");
    }
}

class FamilyBattleAwardTable : LogicFileWithKey<UInt32, wl_res.FamilyBattleAward>
{
    public override UInt32 GetKey(wl_res.FamilyBattleAward Value)
    {
        return (UInt32)Value.Level;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Family/FamilyBattleAward");
    }
}

class FamilyIntervalTimeTable : LogicFileWithKey<UInt32, wl_res.FamilyIntervalTime>
{
    List<float> m_allIntervalTimeList = new List<float>();
    public override UInt32 GetKey(wl_res.FamilyIntervalTime Value)
    {
        return (UInt32)Value.Seq;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Family/FamilyIntervalTime");
        m_allIntervalTimeList.Clear();
        foreach (KeyValuePair<UInt32, wl_res.FamilyIntervalTime> Pair in GetTable())
        {
            m_allIntervalTimeList.Add(Pair.Value.FamilyTeamMaxAttackTime);
        }
    }

    public List<float> GetIntervalTime()
    {
        return m_allIntervalTimeList;
    }
}

class FamilyTeamActivityTimeTable : LogicFileWithKey<UInt32, wl_res.FamilyTeamActivityTime>
{
    public override UInt32 GetKey(wl_res.FamilyTeamActivityTime Value)
    {
        return (UInt32)Value.Weekday;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Family/FamilyTeamActivityTime");
    }
}

class FamilyWarCityTable : LogicFileWithKey<UInt32, wl_res.FamilyWarCity>
{
    public Dictionary<uint, List<uint>> CityIdPath = new Dictionary<uint, List<uint>>();
    public override UInt32 GetKey(wl_res.FamilyWarCity Value)
    {
        return (UInt32)Value.CityID;
    }

    public override void Init()
    {
        ReadBinFile("LocalConfig/Family/FamilyWarCity");
        CityIdPath.Clear();
        List<uint> idList =new List<uint>(GetTable().Keys);
        for (int i = 0; i < idList.Count;i++ )
        {
            //string[] strPath = TextUtils.Bytes2String(ref GetTable()[idList[i]].GoToCityList).Split(',');
            //List<uint> path =new List<uint>();
            //for(int j=0;j<strPath.Length;j++)
            //{
            //    path.Add(uint.Parse(strPath[j]));
            //}
            //CityIdPath[idList[i]] = path;
        }
    }
}