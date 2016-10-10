//------------------------------------------------------------------------------
// 各种在编辑器中使用的技能数据
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

using System.Reflection;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
/*enum E1
{
    EV1,
    EV2,
        EV3,
}

enum E2
{
    EV1,
    EV2,
    EV3
}
class Test
{
    [EnumMutex(1, E1.EV1, E2.EV3)]
    public EV1 e1;
    [EnumMutex(1, E2.EV1, E2.EV3)]
    [EnumMutex(1, E2.EV2, E2.EV3)]
    public EV2 e2;
}*/

public class EditDataBase
{
    public DispElementBase m_DispElementBase;
	protected int secid = 0;
	protected float start = 0;
	protected float end = 0.0f;

    public EditDataBase()
    {

    }

	public int GetSceID()
	{
		return secid;
	}
	public float GetStart()
	{
		return start;
	}
	public float GetEnd()
	{
		return end;
	}

	public  virtual void SetSceID(int id)
	{
		secid = id;
	}
	public virtual void SetStart(float s)
	{
		start = s;
        m_DispElementBase.m_StartupEvent.m_Param1 = start - (int)start;
	}
	public virtual void SetEnd(float e)
	{
		end = e;
        m_DispElementBase.m_TerminateEvent.m_Param1 = end - secid;
	}

	public virtual  void SetElement (object obj)
    {
        m_DispElementBase = null;
        m_DispElementBase = (DispElementBase)obj;
        end = m_DispElementBase.m_TerminateEvent.m_Param1 + secid;
        start = m_DispElementBase.m_StartupEvent.m_Param1 + secid;
    }
	public virtual  object GetElement()
	{
        return m_DispElementBase;
	}

    public virtual void Update() { }


}

[EditDetail]
public class EffectEditData : EditDataBase
{
    public EffectEditData()
    {
        m_DispElementBase = new EffectElement();
    }
}

[EditDetail]
public class AudioEditData : EditDataBase
{
    public AudioEditData()
    {
        m_DispElementBase = new AudioElement();
    }
}

[EditDetail]
public class ShootTestEditData : EditDataBase
{
    public ShootTestEditData()
    {
        m_DispElementBase = new ShootTestElement();
    }
}

[EditDetail]
public class TranslateEditData : EditDataBase
{
    public TranslateEditData()
    {
        m_DispElementBase = new TranslateElement();
    }
}

[EditDetail]
public class WeaponTrailEditData : EditDataBase
{
    public WeaponTrailEditData()
    {
        m_DispElementBase = new WeaponTrailElement();
    }
}

[EditDetail]
public class BulletEditData : EditDataBase
{
    public BulletEditData()
    {
        m_DispElementBase = new BulletElement();
    }
}

[EditDetail]
public class ShootPointEditData : EditDataBase
{
    public ShootPointEditData()
    {
        m_DispElementBase = new ShootPointElement();
    }
}

[EditDetail]
public class CurveTranslateEditData : EditDataBase
{
    public CurveTranslateEditData()
    {
        m_DispElementBase = new CurveTranslateElement();
    }
}

[EditDetail]
public class TargetFlashEditData : EditDataBase
{
    public TargetFlashEditData()
    {
        m_DispElementBase = new TargetFlashElement();
    }
}

[EditDetail]
public class VirtualBulletEditData : EditDataBase
{
    public VirtualBulletEditData()
    {
        m_DispElementBase = new VirtualBulletElement();
    }
}

[EditDetail]
public class CurveBulletEditData : EditDataBase
{
    public CurveBulletEditData()
    {
        m_DispElementBase = new CurveBulletElement();
    }
}

[EditDetail]
public class ShowPointEditData : EditDataBase
{
    public ShowPointEditData()
    {
        m_DispElementBase = new ShowPointElement();
    }
}

[EditDetail]
public class DirectImpactEditData : EditDataBase
{
    public DirectImpactEditData()
    {
        m_DispElementBase = new DirectImpactElement();
    }
}

[EditDetail]
public class SummonNPCEditData : EditDataBase
{
    public SummonNPCEditData()
    {
        m_DispElementBase = new SummonNPCElement();
    }
}

[EditDetail]
public class RandomPosImpactEditData : EditDataBase
{
    public RandomPosImpactEditData()
    {
        m_DispElementBase = new RandomPosImpactElement();
    }
}


[EditDetail]
public class DecalEditData : EditDataBase
{
    public DecalEditData()
    {
        m_DispElementBase = new DecalElement();
    }
}

[EditDetail]
public class CameraShakeEditData : EditDataBase
{
    public CameraShakeEditData()
    {
        m_DispElementBase = new CameraShakeElement();
    }
}

[EditDetail]
public class AppTimeScaleEditData : EditDataBase
{
    public AppTimeScaleEditData()
    {
        m_DispElementBase = new AppTimeScaleElement();
    }
}

[EditDetail]
public class ActionPointEditData : EditDataBase
{
    public ActionPointEditData()
    {
        m_DispElementBase = new ActionPointElement();
    }
}

[EditDetail]
public class TrackingBulletEditData : EditDataBase
{
	public TrackingBulletEditData()
	{
		m_DispElementBase = new TrackingBulletElement();
	}
}

[EditDetail]
public class BlinkElementEditData : EditDataBase
{
	public BlinkElementEditData()
	{
		m_DispElementBase = new BlinkElement();
	}
}

[EditDetail]
public class ChangePosElementEditData : EditDataBase
{
	public ChangePosElementEditData()
	{
		m_DispElementBase = new ChangePosElement();
	}
}


[EditDetail]
public class ContinuousCastElementEditData : EditDataBase
{
	public ContinuousCastElementEditData()
	{
		m_DispElementBase = new ContinuousCastElement();
	}
}


//Section元素数据
public class SectionElementInfo
{
	public SectionType m_type;
	public int count;
}

//一个阶段里包含所有元素的数据类（Edit模式专用）
public class SkillEditData
{
    [FixedArray]
    [StaticCaption("特效")]
    public EffectEditData[] m_EffectInfo = null;
    [FixedArray]
    [StaticCaption("音效")]
    public AudioEditData[] m_AudioInfo = null;
    [FixedArray]
    [StaticCaption("顿刀")]
    public ShootTestEditData[] m_ShootTestInfo = null;
    [FixedArray]
    [StaticCaption("刀光")]
    public WeaponTrailEditData[] m_WeaponTrailInfo = null;
    [FixedArray]
    [StaticCaption("位移")]
    public TranslateEditData[] m_TranslateInfo = null;
    [FixedArray]
    [StaticCaption("子弹")]
    public BulletEditData[] m_BulletInfo = null;
    [FixedArray]
    [StaticCaption("SHOOT点")]
    public ShootPointEditData[] m_ShootPointInfo = null;
    [FixedArray]
    [StaticCaption("曲线位移")]
    public CurveTranslateEditData[] m_CurveTranslateInfo = null;
    [FixedArray]
    [StaticCaption("角色闪光")]
    public TargetFlashEditData[] m_TargetFlashInfo = null;
    [FixedArray]
    [StaticCaption("地表贴花")]
    public DecalEditData[] m_DecalInfo = null;
    [FixedArray]
    [StaticCaption("镜头震动")]
    public CameraShakeEditData[] m_CameraShakeInfo = null;
    [FixedArray]
    [StaticCaption("虚拟子弹")]
    public VirtualBulletEditData[] m_VirtualBulletInfo = null;
    [FixedArray]
    [StaticCaption("曲线炮弹")]
    public CurveBulletEditData[] m_CurveBulletInfo = null;
    [FixedArray]
    [StaticCaption("表现点")]
    public ShowPointEditData[] m_ShowPointInfo = null;
    [FixedArray]
    [StaticCaption("直接伤害")]
    public DirectImpactEditData[] m_DirectImpactInfo = null;
    [FixedArray]
    [StaticCaption("召唤怪物")]
    public SummonNPCEditData[] m_SummonNPCInfo = null;
    [FixedArray]
    [StaticCaption("随机范围攻击")]
    public RandomPosImpactEditData[] m_RandomPosImpactInfo = null;
    [FixedArray]
    [StaticCaption("游戏变速")]
    public AppTimeScaleEditData[] m_AppTimeScaleInfo = null;
    
	[FixedArray]
    [StaticCaption("动作断点")]
    public ActionPointEditData[] m_ActionPointInfo = null;
	
	[FixedArray]
    [StaticCaption("追踪弹")]
	public TrackingBulletEditData[] m_TrackingBulletInfo = null;

	[FixedArray]
    [StaticCaption("闪现元素")]
	public BlinkElementEditData[] m_BlinkElementEditData = null;

	[FixedArray]
	[StaticCaption("改变位置元素 未完成")]
	public ChangePosElementEditData[] m_ChangePosElementEditData = null;
	
	[FixedArray]
	[StaticCaption("持续施法元素")]
	public ContinuousCastElementEditData[] m_ContinuousCastElementEditData = null;


    List<object> m_DispElementBaseVec = new List<object>();
    List<int> m_AllSecid = new List<int>();

    List<Type> m_ElemTypeName = new List<Type>();
    List<Type> m_ElemTypeNameDisp = new List<Type>();
    Dictionary<int, int> m_ElemItemEditDataHash = new Dictionary<int, int>();
    Dictionary<int, int> m_EditDataElemItemHash = new Dictionary<int, int>();
	public SkillEditData()
	{
        Type obType = this.GetType();
        var f_list = obType.GetFields();
        for (int i = 0; i < f_list.GetLength(0); i++)
        {
            Type filetype = f_list[i].FieldType;
            m_ElemTypeName.Add(filetype);
        }

        DispInfo dispinfo = new DispInfo();
        obType = dispinfo.GetType();
        f_list = obType.GetFields();
        for (int i = 0; i < f_list.GetLength(0); i++)
        {
            Type filetype = f_list[i].FieldType;
            m_ElemTypeNameDisp.Add(filetype);
        }
	}


	void SetDispInfor(ref DispInfo info, object obj)
	{
		Type obType = info.GetType();
		var f_list = obType.GetFields();
		
		for (int i = 0; i < f_list.GetLength(0); i++)
		{
			object ob = f_list[i].GetValue(info);
			Type filetype = f_list[i].FieldType;
			string filetypename = filetype.ToString();
			Array ar = null;
			
			if(filetype.BaseType.Name == "Array")
			{
				ar = (ob as Array);
			}

			int existCount = ar == null? 0 : ar.GetLength(0);
			object obTmp = null;
			//string objname = obj.GetType().ToString();
			if(filetypename == obj.GetType().ToString()+"[]")
			{
				obTmp = Array.CreateInstance(obj.GetType(), existCount+1);
				
				for (int j = 0; j < existCount; j++)
				{
					(obTmp as Array).SetValue(ar.GetValue(j), j);
				}
				(obTmp as Array).SetValue(obj, existCount);
			}
			if(obTmp != null)
			{
				f_list[i].SetValue(info, obTmp);
			}

		}
	}

    public void UpdateWithSection(SkillSectionData secdata)
    {
        Type obType = this.GetType();
        var f_list = obType.GetFields();
        for (int i = 0; i < f_list.GetLength(0); i++)
        {
            object ob = f_list[i].GetValue(this);
            if (ob != null && ob.GetType().BaseType.Name == "Array")
            {
                Array ar = ob as Array;
                int existCount = ar == null ? 0 : ar.GetLength(0);

                for (int j = 0; j < existCount; j++)
                {
                    object ob1 = ar.GetValue(j);
                    var attrs = System.Attribute.GetCustomAttributes(ob1.GetType());
		            int iAttrCount = attrs.GetLength(0);
		            bool bEditInSubWindow = false;

		            for (int k = 0; k < iAttrCount; k++)
		            {
			            if (attrs[k] is EditDetail)
			            {
				            bEditInSubWindow = true;
			            }
		            }
                    if (bEditInSubWindow == true)
                    {
                        var f_list1 = ob1.GetType().GetFields();
                        for (int k = 0; k < f_list1.GetLength(0); k++)
                        {
                            object ob2 = f_list1[k].GetValue(ob1);
                            if (ob2.GetType().BaseType.Name == "DispElementBase")
                            {
                                DispElementBase ob3 = (DispElementBase)ob2;
                                SkillDispEventType eventtype = ob3.m_StartupEvent.m_EventType;
                            }
                        }
                    }
                }
            }
        }
    }

    private void SavePrepareSectionData(ref SkillPrepareData data, PrepareSection sec)
    {
        data.m_StateName = sec.SecName;
        data.m_NameHash = Animator.StringToHash("Base Layer." + sec.SecName);
        data.m_AnimationKey = sec.data.m_AnimationKey;
    }

    private void SavePerformSectionData(ref SkillPerformData data, PerformSection sec)
    {
        //data.m_LandingParam = sec.data.m_LandingParam;

        data.m_StateName = sec.SecName;
        data.m_NameHash = Animator.StringToHash("Base Layer." + sec.SecName);
        data.m_AnimationKey = sec.data.m_AnimationKey;
    }

    private void SaveShowSectionData(ref SkillShowData data, ShowSection sec)
    {
        data.m_SkillShowType = sec.data.m_SkillShowType;
        data.m_fTotalTime = sec.data.m_fTotalTime;
    }

    //解析吟唱阶段中的状态名和状态key
    private void LoadPrepareSectionData(SkillPrepareData data, ref PrepareSection.PrepareData sec)
    {
        sec.m_StateName = data.m_StateName;
        sec.m_AnimationKey = data.m_AnimationKey;
    }


    private void LoadPerformSectionData(SkillPerformData data, ref  PerformSection.PerformData sec)
    {
        //sec.m_LandingParam = data.m_LandingParam;
        sec.m_StateName = data.m_StateName;
        sec.m_AnimationKey = data.m_AnimationKey;
    }

    private void LoadShowSectionData(SkillShowData data, ref ShowSection.ShowData sec)
    {
        sec.m_SkillShowType = data.m_SkillShowType;
        sec.m_fTotalTime = data.m_fTotalTime;
    }

    private void SetSectionData(ref SkillDispData cacheData, SkillSectionData secdata)
    {
        if (secdata.m_PerformSection != null)
        {
            for (int i = 0; i < secdata.m_PerformSection.GetLength(0); i++)
            {
                SavePerformSectionData(ref cacheData.m_PerformDatas[i], secdata.m_PerformSection[i]);
            }
        }

        if (secdata.m_PrepareSection != null)
        {
            for (int i = 0; i < secdata.m_PrepareSection.GetLength(0); i++)
            {
                SavePrepareSectionData(ref cacheData.m_PrepareDatas[i], secdata.m_PrepareSection[i]);
            }
        }

        if (secdata.m_ShowSection != null)
        {
            for (int i = 0; i < secdata.m_ShowSection.GetLength(0); i++)
            {
                SaveShowSectionData(ref cacheData.m_ShowDatas[i], secdata.m_ShowSection[i]);
            }
        }
    }

    //把数据保存到SaveData
    public void SaveData(SkillSectionData secdata, out SkillDispData SaveData)
	{
		SkillDispData m_CacheData = new SkillDispData ();
		int m_PrepareSecCount = secdata.GetPrepareSectionCount();
		int m_PerformSecCount = secdata.GetPerformSectionCount();
		int m_ShowSecCount = secdata.GetShowSectionCount();
		//int m_ImpactSecCount = secdata.GetImpactSectionCount();
		List<SectionElementInfo> sectionInfo = secdata.GetSectionInfo ();
		m_CacheData.m_PrepareDatas = new SkillPrepareData[m_PrepareSecCount];



		for(int i=0; i<m_PrepareSecCount; i++)
		{
			m_CacheData.m_PrepareDatas[i] = new SkillPrepareData();
			m_CacheData.m_PrepareDatas[i].m_DispInfo = new DispInfo();
		}
		m_CacheData.m_PerformDatas = new SkillPerformData[m_PerformSecCount];
		for(int i=0; i<m_PerformSecCount; i++)
		{
			m_CacheData.m_PerformDatas[i] = new SkillPerformData();
			m_CacheData.m_PerformDatas[i].m_DispInfo = new DispInfo();
		}
        m_CacheData.m_ShowDatas = new SkillShowData[m_ShowSecCount];
        for (int i = 0; i < m_ShowSecCount; i++)
        {
            m_CacheData.m_ShowDatas[i] = new SkillShowData();
            m_CacheData.m_ShowDatas[i].m_DispInfo = new DispInfo();
        }

        SetSectionData(ref m_CacheData, secdata);

		Type obType = GetType();
		var f_list = obType.GetFields();
		
		for (int i = 0; i < f_list.GetLength(0); i++)
		{
			object ob = f_list[i].GetValue(this);
			Array ar = null;
			if(ob.GetType().BaseType.Name == "Array")
			{
				ar = (ob as Array);
			} 
			
			if(ar != null)
			{
				for(int j=0; j<ar.GetLength(0); j++)
				{
					object ob1 = ar.GetValue(j);
					MethodInfo info = ob1.GetType().GetMethod("GetSceID");
					int secid = (int)info.Invoke(ob1, null);

					SectionType Sectype = sectionInfo[secid].m_type;
					int SecIdx = sectionInfo[secid].count;
					MethodInfo info1 = ob1.GetType().GetMethod("GetElement");
					switch (Sectype)
                    {
                        case SectionType.SEC_TYPE_PREPARE:
						    SetDispInfor(ref m_CacheData.m_PrepareDatas[SecIdx].m_DispInfo, info1.Invoke(ob1, null));
                            break;

                        case SectionType.SEC_TYPE_PERFORM:
    						SetDispInfor(ref m_CacheData.m_PerformDatas[SecIdx].m_DispInfo, info1.Invoke(ob1, null));
                            break;

                        case SectionType.SEC_TYPE_SHOW:
                            SetDispInfor(ref m_CacheData.m_ShowDatas[SecIdx].m_DispInfo, info1.Invoke(ob1, null));
                            break;

                        default:
                            break;
					}
				}
			}
			
		}
		SaveData = m_CacheData;
	}

    public List<object> GetAllElems()
    {
        return m_DispElementBaseVec;
    }

    public List<int> GetAllSecID()
    {
        m_AllSecid.Clear();
        Type obType = GetType();
		var f_list = obType.GetFields();
        for (int i = 0; i < f_list.GetLength(0); i++)
        {
            object ob = f_list[i].GetValue(this);
            Array ar = null;
            if (ob.GetType().BaseType.Name == "Array")
            {
                ar = (ob as Array);
            }

            if (ar != null)
            {
                for (int j = 0; j < ar.GetLength(0); j++)
                {
                    object ob1 = ar.GetValue(j);
                    MethodInfo info = ob1.GetType().GetMethod("GetSceID");
                    int secid = (int)info.Invoke(ob1, null);
                    m_AllSecid.Add(secid);
                }
            }
        }
        return m_AllSecid;
    }

    public static string GetSectionName(SectionType type, string strName)
    {
        Type t = typeof(SectionType);
        FieldInfo[] f_list = t.GetFields();
        int iFieldCount = f_list.GetLength(0);
        for (int i = 0; i < iFieldCount; i++)
        {
            FieldInfo f = f_list[i];
            object ob = f.GetValue(type);

            if ((int)ob == (int)type)
            {
                var attrs = System.Attribute.GetCustomAttributes(f);
                int iAttrCount = attrs.GetLength(0);

                for (int j = 0; j < iAttrCount; j++)
                {
                    if (attrs[j] is StaticCaption)
                    {
                        return (attrs[j] as StaticCaption).Value;
                    }
                }
            }
        }
        return strName;
    }

    //往对应的元素数组里加入一个新元素
    int AddElemnt(ref object OldData, object obj, int secidx, int itemidx)
    {
        Array ar = OldData as Array;
        int count = ar == null ? 0 : ar.GetLength(0);
        Type tt = m_ElemTypeName[itemidx].GetElementType();
        object obTmp = Array.CreateInstance(tt, count + 1);

        for (int i = 0; i < count; i++)
        {
            var oldedit = ar.GetValue(i);
            (obTmp as Array).SetValue(oldedit, i);
        }
        Array NewData = obTmp as Array;
        NewData.SetValue(Assembly.GetAssembly(tt).CreateInstance(tt.FullName), count);

        if (obj != null)
        {
            MethodInfo info = NewData.GetValue(count).GetType().GetMethod("SetElement");
            MethodInfo info1 = NewData.GetValue(count).GetType().GetMethod("SetSceID");
            info1.Invoke(NewData.GetValue(count), new object[] { secidx });
            info.Invoke(NewData.GetValue(count), new object[] { obj });
        }
        count++;
        OldData = NewData;

        return NewData.GetValue(count - 1).GetHashCode();
    }

    //将所有元素和hashcode存储起来，存储到list中
    void UpdateElementArray()
    {
        Type obType = this.GetType();
        var f_list = obType.GetFields();
        m_DispElementBaseVec.Clear();
        m_ElemItemEditDataHash.Clear();
        m_EditDataElemItemHash.Clear();
        for (int i = 0; i < f_list.GetLength(0); i++)
        {
            //Type filetype = f_list[i].FieldType;
            object ob = f_list[i].GetValue(this);
            if (ob != null)
            {
                if (ob.GetType().BaseType.Name == "Array")
                {
                    Array ar = ob as Array;
                    for (int j = 0; j < ar.GetLength(0); j++)
                    {
                        object Editdatadesc = ar.GetValue(j);
                        Type elemType = Editdatadesc.GetType();
                        var f_listelem = elemType.GetFields();
                        for (int k = 0; k < f_listelem.GetLength(0); k++)
                        {
                            object elem = f_listelem[k].GetValue(Editdatadesc);
                            if (elem.GetType().BaseType.Name == "DispElementBase")
                            {
                                //存储所有元素
                                m_DispElementBaseVec.Add(elem);
                                int sh  = elem.GetHashCode();
                                //存储元素数据的hashcode和元素编辑器数据的hashcode
                                m_ElemItemEditDataHash.Add(sh, Editdatadesc.GetHashCode());
                                //与上面相反，存储元素编辑器数据的hashcode和元素数据的hashcode
                                m_EditDataElemItemHash.Add(Editdatadesc.GetHashCode(), elem.GetHashCode());
                                //int hjk = 0;
                            }
                        }

                    }
                }
            }

        }
    }

    public int GetEditDataHash(int elemhash)
    {
        int hashcode = 0;
        m_ElemItemEditDataHash.TryGetValue(elemhash, out hashcode);
        return hashcode;
    }

    public int GetElemhash(int editdatahash)
    {
        int hashcode = 0;
        m_EditDataElemItemHash.TryGetValue(editdatahash, out hashcode);
        return hashcode;
    }

    //往对应的元素数组里加入一个新元素
    public int AddEditData(int Etype, object data, int secidx)
    {
        int rtsh = -1;

        Type obType = this.GetType();
        var f_list = obType.GetFields();
        for (int i = 0; i < f_list.GetLength(0); i++)
        {
            Type filetype = f_list[i].FieldType;
            if (filetype == m_ElemTypeName[Etype])
            {
                //获取this里存储的该元素的数组，然后将新元素加进来
                object ob = f_list[i].GetValue(this);
                rtsh = AddElemnt(ref ob, data, secidx, Etype);    
                f_list[i].SetValue(this, ob);
                break;
            }
        }
        UpdateElementArray();
        return rtsh;
    }

    public static T Clone<T>(T source)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException("The type must be serializable.", "source");
        }

        // Don't serialize a null object, simply return the default for that object
        if (System.Object.ReferenceEquals(source, null))
        {
            return default(T);
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new MemoryStream();
        using (stream)
        {
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }


    public void CopyItem(int hashCode)
    {
        Type obType = this.GetType();
        var f_list = obType.GetFields();
        for (int i = 0; i < f_list.GetLength(0); i++)
        {
            //Type filetype = f_list[i].FieldType;
            object ob = f_list[i].GetValue(this);
            if (ob != null)
            {
                if (ob.GetType().BaseType.Name == "Array")
                {
                    Array ar = ob as Array;
                    int j = 0;
                    for (; j < ar.GetLength(0); j++)
                    {
                        object Editdatadesc = ar.GetValue(j);
                        if (Editdatadesc.GetHashCode() == hashCode)
                        {
                            MethodInfo info1 = Editdatadesc.GetType().GetMethod("GetElement");      
                            DispElementBase oldbase = (DispElementBase)info1.Invoke(Editdatadesc, null);
                            DispElementBase newbase = Clone<DispElementBase>(oldbase);
                            AddEditData(i, newbase, 0);
                        }
                    }
                }
            }
        }
    }

	public void DeleteItem(int hashCode)
	{
        Type obType = this.GetType();
        var f_list = obType.GetFields();
        for (int i = 0; i < f_list.GetLength(0); i++)
        {
            //Type filetype = f_list[i].FieldType;
            object ob = f_list[i].GetValue(this);
            if (ob != null)
            {
                if (ob.GetType().BaseType.Name == "Array")
                {
                    Array ar = ob as Array;
                    int j = 0;
                    for (; j < ar.GetLength(0); j++)
                    {
                        object Editdatadesc = ar.GetValue(j);
                        if (Editdatadesc.GetHashCode() == hashCode)
                        {
                            break;
                        }
                    }
                    if (j < ar.GetLength(0))
                    {
                        Type tt = m_ElemTypeName[i].GetElementType();
                        int count = ar.GetLength(0) - 1;
                        object obTmp = count == 0 ? null : Array.CreateInstance(tt, count);
                        if (obTmp != null)
                        {
                            for (int k = 0, t = 0; k < ar.GetLength(0); k++)
                            {
                                if (k == j) continue;
                                var oldedit = ar.GetValue(k);
                                (obTmp as Array).SetValue(oldedit, t);
                                t++;
                            }
                        }
                        ob = null;
                        f_list[i].SetValue(this, obTmp);
                        break;
                    }
                }
            }
  
        }

        UpdateElementArray();
	}

    //解析阶段Section数据中的每一条元素
    public void LoadTrackData(ref DispInfo info, int SectionIdx)
	{
        Type obType = info.GetType();
        var f_list = obType.GetFields();
        for (int i = 0; i < f_list.GetLength(0); i++)
        {
            //循环取出DispInfo中的每一个Element数组，f_list[i]就是一个元素数组
            Type filetype = f_list[i].FieldType;
            int j = 0;
            for (; j < m_ElemTypeNameDisp.Count; j++)
            {
                if (filetype == m_ElemTypeNameDisp[j])
                {
                    break;
                }
            }
            if (j < m_ElemTypeNameDisp.Count)
            {
                //获取info中的对应m_ElemTypeNameDisp[j]的元素数组ob，然后遍历ob将每个元素存储
                object ob = f_list[i].GetValue(info);
                Array ar = ob as Array;
                if (ar != null)
                {
                    for (int k = 0; k < ar.GetLength(0); k++)
                    {
                        AddEditData(j, ar.GetValue(k), SectionIdx);
                    }
                }
            }
        }
	}

    //解析加载进来的技能数据
	public void OpenDispInfo(ref SkillDispData _DispData, ref SkillSectionData _SkillSectionData)
	{
        //吟唱阶段数据条数
        int PrepareSecCount = _DispData.m_PrepareDatas == null ? 0 : _DispData.m_PrepareDatas.GetLength(0);
		for(int i=0; i<PrepareSecCount; i++)
		{
			LoadTrackData(ref _DispData.m_PrepareDatas[i].m_DispInfo, i);
            PrepareSection.PrepareData data = new PrepareSection.PrepareData();
            LoadPrepareSectionData(_DispData.m_PrepareDatas[i], ref data);
            _SkillSectionData.AddSectionFromEditor(0, data);
		}
        //释放阶段数据条数
        int PerformSecCount = _DispData.m_PerformDatas == null ? 0 : _DispData.m_PerformDatas.GetLength(0);
		for(int i=0; i<PerformSecCount; i++)
		{
            LoadTrackData(ref _DispData.m_PerformDatas[i].m_DispInfo, i + PrepareSecCount);
            PerformSection.PerformData data = new PerformSection.PerformData();
            LoadPerformSectionData(_DispData.m_PerformDatas[i], ref data);
            _SkillSectionData.AddSectionFromEditor(1, data);
		}
        //表现阶段数据条数
        int ShowSecCount = _DispData.m_ShowDatas == null ? 0 : _DispData.m_ShowDatas.GetLength(0);
		for(int i=0; i<ShowSecCount; i++)
		{
            LoadTrackData(ref _DispData.m_ShowDatas[i].m_DispInfo, i + PrepareSecCount + PerformSecCount);
            ShowSection.ShowData data = new ShowSection.ShowData();
            LoadShowSectionData(_DispData.m_ShowDatas[i], ref data);
            _SkillSectionData.AddSectionFromEditor(2, data);
		}
	}
	
}

//吟唱阶段
public class PrepareSection : EditSectionBase
{
    public class PrepareData
    {
        public string m_StateName;
        public int m_AnimationKey = 0;
    }
    public override string GetChineseName()
    {
        return "吟唱阶段";
    }

    public PrepareSection(object ob)
    {
        if (ob != null)
        {
            data = ob as PrepareData;
        }
        SecName = this.data.m_StateName;
        //AnimationKey = this.data.m_AnimationKey;
    }

    private PrepareData m_data = new PrepareData();
    public PrepareData data
    {
        get { return m_data; }
        set { m_data = value; }

    }

    public override void SetSectionName(string name)
    {
        base.SetSectionName(name);
        if (data != null)
        {
            data.m_StateName = name;
        }
    }
}

//释放阶段
public class PerformSection : EditSectionBase
{
    public PerformSection(object ob)
    {
        if (ob != null)
        {
            this.data = ob as PerformData;
        }
        SecName = this.data.m_StateName;
        //AnimationKey = this.data.m_AnimationKey;
    }
    public override string GetChineseName()
    {
        return "施放阶段";
    }
    [StaticCaption("翻放阶段参数")]
    public class PerformData
    {
        //public LandingParam m_LandingParam;
        public string m_StateName;
        public int m_AnimationKey = 0;
    }
    private PerformData m_data = new PerformData();
    
    public PerformData data
    {
        get{return m_data;}
        set{m_data = value;}
    }

    public override void SetSectionName(string name)
    {
        base.SetSectionName(name);
        if (data != null)
        {
            data.m_StateName = name;
        }
    }
}

//表现阶段
public class ShowSection : EditSectionBase
{
    public override string GetChineseName()
    {
        return "表现阶段";
    }

    public ShowSection(object ob)
    {
        if (ob != null)
        {
            data = ob as ShowData;
        }
    }

    public class ShowData
    {
        public SkillShowType m_SkillShowType = SkillShowType.SKILL_SHOW_SIMPLE;
        public float m_fTotalTime = 0.0f;
    }

    private ShowData m_data = new ShowData();
    public ShowData data
    {
        get { return m_data; }
        set { m_data = value; }
    }
}

//影响阶段
public class ImpactSection : EditSectionBase
{
    public class ImpactData
    {
        [StaticCaption("受击类型")]
        public SkillImpactType m_ImpactType;
        //public DropDownParam m_DropDownParam; 
    }

    public ImpactSection(object ob)
    {
        if (ob != null)
        {
            data = ob as ImpactData;
        }
    }
    public override string GetChineseName()
    {
        return "影响阶段";
    }

    private ImpactData m_data = new ImpactData();

    public ImpactData data
    {
        get { return m_data; }
        set { m_data = value; }
    }
}

//Section数据管理类
public class SkillSectionData :  BaseSectionMgr
{
	public PrepareSection[] m_PrepareSection;
	public PerformSection[] m_PerformSection;
	public ShowSection[]    m_ShowSection;
	public ImpactSection[]  m_ImpactSection;

	List<SectionElementInfo> m_secinfo = new List<SectionElementInfo>();

	public List<SectionElementInfo> GetSectionInfo()
	{
		return m_secinfo;
	}

	public int GetPrepareSectionCount()
	{
		return m_PrepareSection == null? 0 : m_PrepareSection.GetLength(0);
	}

	public int GetPerformSectionCount()
	{
		return m_PerformSection == null? 0 :m_PerformSection.GetLength(0);
	}

	public int GetShowSectionCount()
	{
		return m_ShowSection == null? 0 :m_ShowSection.GetLength(0);
	}

	public int GetImpactSectionCount()
	{
		return m_ImpactSection == null? 0 :m_ImpactSection.GetLength(0);
	}

	//存储section信息
	public void AddSectionFromEditor(int idx , object ob)
	{
		SectionElementInfo newsec = new SectionElementInfo();
		int rtsecid = -1;
        switch (idx)
        {
            case 0:
                {
                    newsec.m_type = SectionType.SEC_TYPE_PREPARE;
                    rtsecid = AddSection(new PrepareSection(ob));
                    newsec.count = m_PrepareSection.GetLength(0) - 1;
                    break;
                }
            case 1:
                {
                    newsec.m_type = SectionType.SEC_TYPE_PERFORM;
                    rtsecid = AddSection(new PerformSection(ob));
                    newsec.count = m_PerformSection.GetLength(0) - 1;
                    break;
                }
            case 2:
                {
                    newsec.m_type = SectionType.SEC_TYPE_SHOW;
                    rtsecid = AddSection(new ShowSection(ob));
                    newsec.count = m_ShowSection.GetLength(0) - 1;
                    break;
                }
            case 3:
                {
                    newsec.m_type = SectionType.SEC_TYPE_IMPACT;
                    rtsecid = AddSection(new ImpactSection(ob));
                    newsec.count = m_ImpactSection.GetLength(0) - 1;
                    break;
                }
            default:
                break;
        }

		m_secinfo.Insert(rtsecid, newsec);
	}

    public object GetSectionData(int idx)
    {
        SectionType sectype = m_secinfo[idx].m_type;
        int index = m_secinfo[idx].count;

        switch (sectype)
        {
            case SectionType.SEC_TYPE_PREPARE:
                {
                    return m_PrepareSection[index].data;
                }
            case SectionType.SEC_TYPE_PERFORM:
                {
                    return m_PerformSection[index].data;
                }
            case SectionType.SEC_TYPE_SHOW:
                {
                    return m_ShowSection[index].data;
                }
            case SectionType.SEC_TYPE_IMPACT:
                {
                    return m_ImpactSection[index].data;
                }
            default:
                throw (new Exception("Unknow section type" + sectype.ToString()));
        }
    }

    public object GetSection(int idx)
    {
        SectionType sectype = m_secinfo[idx].m_type;
        int index = m_secinfo[idx].count;

        switch (sectype)
        {
            case SectionType.SEC_TYPE_PREPARE:
                {
                    return m_PrepareSection[index];
                }
            case SectionType.SEC_TYPE_PERFORM:
                {
                    return m_PerformSection[index];
                }
            case SectionType.SEC_TYPE_SHOW:
                {
                    return m_ShowSection[index];
                }
            case SectionType.SEC_TYPE_IMPACT:
                {
                    return m_ImpactSection[index];
                }
            default:
                throw (new Exception("Unknow section type" + sectype.ToString()));
        }
    }


    public void SetSectionData(int idx, object ob)
    {
        SectionType sectype = m_secinfo[idx].m_type;
        int index = m_secinfo[idx].count;

        switch (sectype)
        {
            case SectionType.SEC_TYPE_PREPARE:
                {
                    m_PrepareSection[index].data = (PrepareSection.PrepareData)ob;
                    break;
                }
            case SectionType.SEC_TYPE_PERFORM:
                {
                    m_PerformSection[index].data = (PerformSection.PerformData)ob;
                    break;
                }
            case SectionType.SEC_TYPE_SHOW:
                {
                    m_ShowSection[index].data = (ShowSection.ShowData)ob;
                    break;
                }
            case SectionType.SEC_TYPE_IMPACT:
                {
                    m_ImpactSection[index].data = (ImpactSection.ImpactData)ob;
                    break;
                }
            default:
                throw (new Exception("Unknow section type" + sectype.ToString()));
        }
    }

	public void DeleteSectionFromEditor(int idx)
	{
		SectionType sectype = m_secinfo[idx].m_type;
		int count = m_secinfo [idx].count;
		int PrepareSecCount = m_PrepareSection == null ? 0 : m_PrepareSection.GetLength (0);
		int PerformSecCount = m_PerformSection == null ? 0 : m_PerformSection.GetLength (0);
		int ShowSecCount = m_ShowSection == null ? 0 : m_ShowSection.GetLength (0);
		int ImpactSecCount = m_ImpactSection == null ? 0 : m_ImpactSection.GetLength (0);

		switch (sectype) 
		{
		case SectionType.SEC_TYPE_PREPARE:
		{
			for(int i=idx+1; i<PrepareSecCount; i++)  m_secinfo[i].count--;
			DelSection(m_PrepareSection[count]);
			break;
		}
		case SectionType.SEC_TYPE_PERFORM:
		{
			for(int i=idx+1; i<PrepareSecCount+PerformSecCount; i++) m_secinfo[i].count--;
			DelSection(m_PerformSection[count]);
			break;
		}
		case SectionType.SEC_TYPE_SHOW:
		{
			for(int i=idx+1; i<PrepareSecCount+PerformSecCount+ShowSecCount; i++) m_secinfo[i].count--;
			DelSection(m_ShowSection[count]);
			break;
		}
		case SectionType.SEC_TYPE_IMPACT:
		{
			for(int i=idx+1; i<PrepareSecCount+PerformSecCount+ShowSecCount+ImpactSecCount; i++) m_secinfo[i].count--;
			DelSection(m_ImpactSection[count]);
			break;
		}	
			
		}
		m_secinfo.RemoveAt(idx);
	}

	
	public SkillSectionData()
	{
	
	}

}


