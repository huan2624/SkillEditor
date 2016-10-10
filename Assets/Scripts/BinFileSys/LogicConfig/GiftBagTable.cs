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

// 关卡表需要特殊处理 因为关卡是需要顺序的 所以生成数组来确定关卡的排序
// 然后又需要支持查找关卡 所以需要做还要再做一层封装
class GiftBagTable : LogicFileWithoutKey<wl_res.GiftBag>
{
	// 礼包表 key是 礼包ID value是 礼包里头所有物品在 table中的索引
	Dictionary<uint, List<wl_res.GiftBag>> m_GiftTable = new Dictionary<uint, List<wl_res.GiftBag>>();

	public List<wl_res.GiftBag> GetGiftBag(uint GiftBagID)
	{
        if (GiftBagID == 0)
        {
            return null;
        }

		List<wl_res.GiftBag> GiftBagList = null;
		if (!m_GiftTable.TryGetValue( GiftBagID, out GiftBagList))
		{
			Debug.LogError("GetGiftBag Failed GiftBagID = " + GiftBagID);
		}
		return GiftBagList;
	}

	public override void Init()
	{
        string[] BinFiles = {"LocalConfig/Item/Level_GiftBag", "LocalConfig/Item/Basic_GiftBag"
						, "LocalConfig/Item/Lottery_GiftBag", "LocalConfig/Item/Defend_Athena_GiftBag"
						, "LocalConfig/Item/Vip_GiftBag", "LocalConfig/Item/Family_GiftBag"};

		//ReadBinFile("LocalConfig/Item/GiftBag");
        for (int i = 0; i < BinFiles.Length; ++i )
        {
            ReadBinFile(BinFiles[i]);

            List<wl_res.GiftBag> lst = GetTable();
		    uint curGiftID = 0;
            foreach (wl_res.GiftBag Value in lst)
		    {
			    // 礼包配置文件里头 排序只有第一个会填对应礼包ID 为了后续查找方便 直接在这把空的数据补齐
			    if (Value.Id != 0)
			    {
				    curGiftID = Value.Id;
			    }
			    else
			    {
				    Value.Id = curGiftID;
			    }

			    List<wl_res.GiftBag> GiftList = null;
			    if (!m_GiftTable.TryGetValue( curGiftID, out GiftList))
			    {
				    GiftList = new List<wl_res.GiftBag>();
				    m_GiftTable.Add( curGiftID, GiftList);
			    }

			    GiftList.Add(Value);
		    }
            lst.Clear();
        }
	}

    //处理礼包套礼包
    public List<wl_res.RandomItem> getGiftBag(uint GiftBagId)
    {
        List<wl_res.RandomItem> lstItem = new List<wl_res.RandomItem>();
        GetGiftBagRandomItem(GiftBagId, lstItem, 0);
        return lstItem;
    }

    void GetGiftBagRandomItem(uint uGiftBagId, List<wl_res.RandomItem> lstItem, int iMaxCallLevel)
    {
        if (iMaxCallLevel >= 10)
        {
            return;
        }
        List<wl_res.GiftBag> tempGiftbagList = GetGiftBag(uGiftBagId);
        if (tempGiftbagList == null)
        {
            //Debug.LogError("error current gift id :" + uGiftBagId);
            return;
        }
        if (tempGiftbagList.Count == 0)
        {
            return;
        }
        for (int i = 0; i < tempGiftbagList.Count; ++i)
        {
            if (tempGiftbagList[i].Item.Rate == 0 && tempGiftbagList[i].Item.IsMustAppear != 1)
                continue;

            if (tempGiftbagList[i].Item.Type != (byte)wl_res.ResItemType.RES_ITEM_TYPE_GIFT_BAG)
            {
                bool iscontun = false;
                for (int m = 0; m < lstItem.Count; m++)
                {
                    if (lstItem[m].Id == tempGiftbagList[i].Item.Id && lstItem[m].Type == tempGiftbagList[i].Item.Type)
                    {
                        iscontun = true;
                        break;
                    }
                }
                if (iscontun)
                {
                    iscontun = false;
                    continue;
                }
                else
                {
                    lstItem.Add(tempGiftbagList[i].Item);
                }
            }
            else
            {
                GetGiftBagRandomItem(tempGiftbagList[i].Item.Id, lstItem, iMaxCallLevel + 1);
            }
        }
    }

}