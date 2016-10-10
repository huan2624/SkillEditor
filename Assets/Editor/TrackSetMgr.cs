//------------------------------------------------------------------------------
// Section数据管理基础类
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;

//Section阶段基础数据
public class EditSectionBase
{
	public string SecName = "";
    //public int AnimationKey = 0;

	public List<int> SecItemHasList = new List<int>();
	public virtual string GetSectionName(){return SecName;}
	public virtual void   SetSectionName(string name)
	{
		SecName = name;
	}
    public virtual string GetChineseName() { return ""; }
}

//Section数据管理基类
public class BaseSectionMgr
{
    //增加一个Section
    protected int AddSection(EditSectionBase obj)
	{
		var f_list = GetType().GetFields();	
		int s=0, t = 0;
		for (int i = 0; i < f_list.GetLength(0); i++)
		{
			object ob = f_list[i].GetValue(this);
			
			Array ar = ob as Array;
			int existCount = ar == null? 0 : ar.GetLength(0);
			
			string filetypename = f_list[i].FieldType.ToString();
			if(filetypename == obj.GetType().ToString()+"[]")
			{
				object obTmp = Array.CreateInstance(obj.GetType(), existCount+1);
				
				for (int j = 0; j < existCount; j++)
				{
					EditSectionBase oldsec = (EditSectionBase)ar.GetValue(j);
                    if (oldsec.SecName == "" || oldsec.SecName == null)
                    {
                        oldsec.SetSectionName(oldsec.GetChineseName() + j.ToString());
                    }
                    
					(obTmp as Array).SetValue(oldsec, j);
					t++;
				}
                if (obj.SecName == "" || obj.SecName == null)
                {
                    obj.SetSectionName(obj.GetChineseName() + existCount.ToString());
                }
				(obTmp as Array).SetValue(obj, existCount);
				f_list[i].SetValue(this, obTmp);
				break;
			}
			s+=existCount;
		}
		return s + t;
	}

    //删除一个Section
    protected void DelSection(EditSectionBase obj)
	{
		var f_list = GetType().GetFields();	
		for (int i = 0; i < f_list.GetLength(0); i++)
		{
			object ob = f_list[i].GetValue(this);
			
			Array ar = ob as Array;
			int existCount = ar == null? 0 : ar.GetLength(0);
			
			string filetypename = f_list[i].FieldType.ToString();
			if(filetypename == obj.GetType().ToString()+"[]")
			{
				object obTmp = null;
				if(existCount-1 > 0) 
				{
					obTmp = Array.CreateInstance(obj.GetType(), existCount-1);
					
					for (int j = 0, k=0; j < existCount; j++)
					{
						EditSectionBase oldsec = (EditSectionBase)ar.GetValue(j);
						if(oldsec.GetHashCode() == obj.GetHashCode()) continue;
						oldsec.SetSectionName(obj.GetType().ToString() + k.ToString());
						(obTmp as Array).SetValue(oldsec, k);
						k++;
					}
				}
				f_list[i].SetValue(this, obTmp);
			}
		}
	}
}

