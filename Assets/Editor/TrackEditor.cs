//------------------------------------------------------------------------------
// 主要负责中间技能阶段部分展示，包括Section外框，滑块
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
public class TrackEditor
{
    //递归渲染
	void RenderRecursively(ref object obEdit, Type t ,object obParent, ref DataEdit editor, bool ischeck)
	{
		int hashcode = obEdit.GetHashCode();
		Type type = obEdit.GetType();
		var attrs = System.Attribute.GetCustomAttributes(type);
		int iAttrCount = attrs.GetLength(0);
		bool bEditInSubWindow = false;

		for (int i = 0; i < iAttrCount; i++)
		{
			if (attrs[i] is EditDetail)
			{
				bEditInSubWindow = true;
			}
		}
		if(bEditInSubWindow == true)
		{	
			if(ischeck == false)
			{
				AppendTrack(false, 19.0f);
			}
            MethodInfo info = type.GetMethod("GetSceID");
            MethodInfo info1 = type.GetMethod("GetStart");
            MethodInfo info2 = type.GetMethod("GetEnd");

            int secid = (int)info.Invoke(obEdit, null);
            int trackbegin = (1 + 0) * 200;
            float start = (float)info1.Invoke(obEdit, null) * 200;
            float end = (float)info2.Invoke(obEdit, null) * 200;



            m_MovableRectMgr.SetRectPos(IncTackCount * 3, trackbegin + start);
            m_MovableRectMgr.SetRectPos(IncTackCount * 3 + 1, trackbegin + end);
			

			IncTackCount++;
			return;
		}
		else
		{
			if(type.BaseType.Name == "Array")
			{
				Array ar = (obEdit as Array);
				if(ar.GetLength(0)!=0)
				{
					if(ischeck == false) AppendTrack(true, 20.0f);
					IncTackCount++;
				}
			}
			else
			{
				if(ischeck == false) AppendTrack(true, 20.0f);
				IncTackCount++;
			}

		}


		bool IsFoldOut = editor.GetFoldState(hashcode);
		if(IsFoldOut == true)
		{
			Type obType = obEdit.GetType();
			var f_list = obType.GetFields();
			Array ar = null;

			if(obType.BaseType.Name == "Array")
			{
				ar = (obEdit as Array);
			}
		
			for (int i = 0; i < f_list.GetLength(0); i++)
			{
				object ob = f_list[i].GetValue(obEdit);
				RenderRecursively(ref ob, f_list[i].FieldType, obEdit, ref editor, ischeck);

			}
			if(ar != null)
			{
				for(int i=0; i<ar.GetLength(0); i++)
				{
					object ob = ar.GetValue(i);
					RenderRecursively(ref ob, ob.GetType(), obEdit, ref editor, ischeck);
				}
			}

		}
//         else
//         {
//             int hjk = 0;
//         }


	}

    //将滑块所指示的数值赋值到EditData中
    void EditRec(ref object obEdit, object obParent, ref DataEdit editor, ref int trackcount, ref int hash, ref List<int> deletehash, int deletesec)
	{
		int hashcode = obEdit.GetHashCode();
		Type type = obEdit.GetType();
		var attrs = System.Attribute.GetCustomAttributes(type);
		int iAttrCount = attrs.GetLength(0);
		bool bEditInSubWindow = false;
		for (int i = 0; i < iAttrCount; i++)
		{
			if (attrs[i] is EditDetail)
			{
				bEditInSubWindow = true;
			}
		}
		if(m_MovableRectMgr.CurrentTrackIdx == trackcount)
		{
			hash = hashcode;
		}
		trackcount++;
		if(type.BaseType.Name == "Array")
		{
			Array ar = (obEdit as Array);
			if(ar.GetLength(0)==0)
			{
				trackcount--;
			}
		}

		if(bEditInSubWindow == true)
		{	

			int fff = editor.GetSelectHash();
			MethodInfo info3 = type.GetMethod("SetSceID");
			MethodInfo info4 = type.GetMethod("SetStart");
			MethodInfo info5 = type.GetMethod("SetEnd");
			if(hashcode == fff)
			{
                if (editor.IsObjectDirty(fff) == false)
                {
                    info3.Invoke(obEdit, new object[] { m_MovableRectMgr.CurrentTrackSecid });
                    info4.Invoke(obEdit, new object[] { m_MovableRectMgr.CurrentStart });
                    info5.Invoke(obEdit, new object[] { m_MovableRectMgr.CurrentEnd });
                }
			}
			MethodInfo info6 = type.GetMethod("GetSceID");;
			int secid = (int)info6.Invoke(obEdit,null);
			if(deletesec == secid)
			{
				deletehash.Add(hashcode);
			}
			
			if(m_ShiftFlag != 10000)
			{
				MethodInfo info7 = type.GetMethod("GetStart");
				MethodInfo info8 = type.GetMethod("GetEnd");
				float start = (float)info7.Invoke(obEdit,null);
				float end = (float)info8.Invoke(obEdit,null);

				if(m_ShiftFlag >0)
				{
					if(secid+1 >= m_ShiftFlag)
					{
						secid++;
						start += 1;
						end += 1;
					}

				}
				else 
				{
					if(secid+1 >= -m_ShiftFlag)
					{
						secid--;
						start -= 1;
						end -= 1;
					}

				}
				info3.Invoke(obEdit, new object[]{secid});
				info4.Invoke(obEdit, new object[]{start});
				info5.Invoke(obEdit, new object[]{end});


			}
			return;
		}

		bool IsFoldOut = editor.GetFoldState(hashcode);
		if(m_ShiftFlag != 10000 || IsFoldOut == true)
		{
			Type obType = obEdit.GetType();
			var f_list = obType.GetFields();
			Array ar = null;
			
			if(obType.BaseType.Name == "Array")
			{
				ar = (obEdit as Array);
			}
			
			for (int i = 0; i < f_list.GetLength(0); i++)
			{
				object ob = f_list[i].GetValue(obEdit);
				EditRec(ref ob, obEdit, ref editor, ref trackcount, ref hash,  ref deletehash, deletesec);
				
			}
			if(ar != null)
			{
				for(int i=0; i<ar.GetLength(0); i++)
				{
					object ob = ar.GetValue(i);
					EditRec(ref ob, obEdit, ref editor, ref trackcount, ref hash, ref deletehash, deletesec);
				}
			}
			
		}
	}

    //编辑Section，设置其信息
    void EditSec(ref object obsec, ref List<int> SeccInfo)
	{
		Type obType = obsec.GetType();
		var f_list = obType.GetFields();

		for (int i = 0; i < f_list.GetLength(0); i++)
		{
			object ob = f_list[i].GetValue(obsec);
			if(ob != null)
			{
				Array ar = null;
				if(ob.GetType().BaseType.Name == "Array")
				{
					ar = (ob as Array);
				}
				if(ar != null)
				{
					for(int j=0; j<ar.GetLength(0); j++)
					{
						/*for(int k=0; k<m_Hashcodes[inc].Count; k++)
						{
							m_SecTrackMap[m_Hashcodes[inc][k]] = inc;
						}

						inc++;*/
                        object ob1 = ar.GetValue(j);
                        MethodInfo info = ob1.GetType().GetMethod("SetSectionName");
                        string secname = GetSectionNameByIdx(SeccInfo.Count);
                        info.Invoke(ob1, new object[] { secname });

						SeccInfo.Add(i);
					}
				}
			}
		}   
	}

    //更新滑块数据信息
	public void EditTrack(object ob, object obsec, ref DataEdit editor)
	{
		/*for(int i=0; i<10; i++)
		{
			m_Hashcodes[i].Clear();
		}*/
		int deletesec = -1;
		if(m_SectionMgr.BeingDeleteTrack != -1)
		{
			deletesec = m_SectionMgr.BeingDeleteTrack;
			MethodInfo info6 = obsec.GetType().GetMethod("DeleteSectionFromEditor");
			info6.Invoke(obsec, new object[]{m_SectionMgr.BeingDeleteTrack});
			m_SectionMgr.BeingDeleteTrack = -1;

		}
		m_ShiftFlag = 10000;

		List<int> SeccInfo = new List<int>();
		EditSec(ref obsec, ref SeccInfo);
        if (SeccInfo.Count != m_SeccInfo.Count && m_SeccInfo.Count != 0)
		{
			if(SeccInfo.Count - 1 == m_SeccInfo.Count)
			{
				int i=0;
				for(; i<m_SeccInfo.Count; i++)
				{
					if(SeccInfo[i] != m_SeccInfo[i])
					{
						break;
					}
				}
				m_ShiftFlag = i+1;
				
			}
			else if(SeccInfo.Count + 1 ==  m_SeccInfo.Count)
			{
				int i=0;
				for(; i<SeccInfo.Count; i++)
				{
					if(SeccInfo[i] != m_SeccInfo[i])
					{
						break;
					}
				}

				m_ShiftFlag = -(i+1);

			}
		}
		m_SeccInfo = SeccInfo;

		int trc = 0;
		int hash = -1;
		List<int> deletehash = new List<int>();
		EditRec (ref ob, null, ref editor, ref trc, ref hash,  ref  deletehash, deletesec);
		if(hash != -1)
		{
			editor.SelectItem(hash);
			m_MovableRectMgr.CurrentTrackIdx = -1;
		}

		MethodInfo info7 = ob.GetType().GetMethod("DeleteItem");
		for(int i=0; i<deletehash.Count; i++)
		{
			info7.Invoke(ob, new object[]{deletehash[i]});
		}
	}

    //将所有Section初始化进来
	void RenderSectionRec(ref object secob, object parent)
	{
		Type obType = secob.GetType();
		var f_list = obType.GetFields();

		for (int i = 0; i < f_list.GetLength(0); i++)
		{
			object ob = f_list[i].GetValue(secob);
			if(ob != null)
			{
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
						MethodInfo info = ob1.GetType().GetMethod("GetSectionName");
						string secName = (string)info.Invoke(ob1, null);
                        if (secName != null)
                        {
                            AppendSection(secName);
                        }
                        else
                        {
                            Debug.LogWarning("GetSectionName for: " + ob1 + " error!");
                        }
					}
				}
			}
		}
	}

    //随着左边技能元素的折叠展开而变化
    public void ChangeTrack(object ob, object obSec, Type typeOfOb , FieldInfo fi, ref DataEdit editor)
	{

		m_SectionMgr.Dispose();
		RenderSectionRec(ref obSec, null);


		if(editor.EditObject != null)
		{
			Type type = editor.EditObject.GetType();
			MethodInfo info = type.GetMethod("GetSceID");
			MethodInfo info1 = type.GetMethod("GetStart");
			MethodInfo info2 = type.GetMethod("GetEnd");
			int secid = (int)info.Invoke(editor.EditObject, null);
			//int trackbegin = secid;
			float start = (float)info1.Invoke(editor.EditObject, null);
			float end = (float)info2.Invoke(editor.EditObject, null);
			
			m_MovableRectMgr.CurrentTrackSecid = secid;
			m_MovableRectMgr.CurrentStart = start;
			m_MovableRectMgr.CurrentEnd = end;

			     
		}

		IncTackCount = 0;
		RenderRecursively (ref ob, typeOfOb, null, ref editor, true);
		if(IncTackCount != LastrackCount)
		{
			ClearTrack ();
			IncTackCount = 0;
			RenderRecursively (ref ob, typeOfOb, null, ref editor, false);
			LastrackCount = IncTackCount;
		}

	}
	
    //销毁TrackEditor
	public void DestroyTrackEditor()
	{
		m_SectionMgr.Dispose();
		m_MovableRectMgr.Dispose();
		m_TrackMgr.Dispose();
	}

    //获取一条滑块数据
	public void GetOneTrackData( int TrackIndex, ref TrackDataElement Element )
	{
		m_MovableRectMgr.GetOneTrackData (TrackIndex, ref Element); 
	}

	public void OnGUI( int FirstDrawSectionLogicIndex, int MaxDrawSectionNum, float XOffset, float PosY, float Height,
	                   int FirstDrawTrackLogicIndex, int MaxDrawTrackNum,
	                  SkillWindowEditor Wnd, Event e, int FirstDrawRectLogicIndex, int MaxDrawRectNum , ref SkillSectionData sec, float hhh)
	{
        m_TrackMgr.ScrollHeight = -hhh;
        m_MovableRectMgr.ScrollHeight = -hhh;
        // Begin Draw Section外框
        m_SectionMgr.OnGUI(Wnd, e, FirstDrawSectionLogicIndex, (int)MaxDrawSectionNum, XOffset, PosY, Height, ref sec);
        // Begin Draw Track线条
        m_TrackMgr.OnGUI( FirstDrawTrackLogicIndex, MaxDrawTrackNum );
        // Begin Draw MovableRect滑块
        m_MovableRectMgr.OnGUI( Wnd, e, FirstDrawRectLogicIndex, ( int )MaxDrawRectNum, FirstDrawSectionLogicIndex);
		if(m_MovableRectMgr.GetCurMovingRectID() != -1 )
		{
			//int HashCOde = EditDataContainer.GetHashCodeFromTrack(m_MovableRectMgr.GetCurMovingRectID()/3-1);
			//m_Editor.SelectItem(HashCOde);
		}
		
		m_MovableRectMgr.CollectPositionData();
		//CollectTrackData( ref m_TrackData );
        isFramebyFrame = Wnd.m_IsinFramebyFrameMode;
        if (isFramebyFrame == true)
        {
            m_Slider.Draw(Wnd, e);
        }
        else
        {
            m_Slider.Reset();
        }
        
	}

    //获取Section名
    public string GetSectionNameByIdx(int idx)
    {
        return m_SectionMgr.GetSectionNameByIdx(idx);
    }

    //获取Section个数
    public int GetSectionCOunt()
    {
        return m_SectionMgr.GetSectionCount();
    }
	
	
	
	float m_TrackNum = 0.0f;
	float m_SectionNum = 0.0f;

	public static int EditorWindow_W = 1320;
	public static int EditorWindow_H = 600;
	public static float ms_TrackViewWidth = 200.0f;
	public static float ms_InspectorViewWidth = 300.0f;
	public static float ms_VerticalScrollBarWidth = 20.0f;
	public static float ms_ControlBarHeight = 20.0f;
	public static float ms_MenuBarHeight = 20.0f;
	public static float ms_IndicatorFootHeight = 20.0f;
	public static float ms_SectionWidth = 200.0f;
    public static int ms_CurSelectSectionIndex = -1;

	private int IncTackCount = 0;
	private int LastrackCount = 0;

    //Section外框整体控制绘图
    SectionMgr m_SectionMgr = new SectionMgr();
    //滑动条控制器
    MovableRectMgr m_MovableRectMgr = new MovableRectMgr();
    //线条控制器
    TrackMgr m_TrackMgr = new TrackMgr();
    //逐帧模式下的滑块控制器
    Slider m_Slider = new Slider();
	//Dictionary<int , int> m_SecTrackMap = new Dictionary<int, int>();
	List<List<int>> m_Hashcodes = new List<List<int>>();
	List<int> m_SeccInfo = new List<int> ();
	int m_ShiftFlag;
    bool isFramebyFrame = false;
	public TrackEditor()
	{
		for(int i=0; i<10; i++)
		{
			m_Hashcodes.Add(new List<int>());
		}

	}
    //获取逐帧模式下滑块所在的时间比例
    public float GetSliderNorTime()
    {
        return m_Slider.GetNorTime();
    }
    //获取逐帧模式下滑块所在的帧id
    public int GetSliderSecIdx()
    {
        return m_Slider.GetSliderSecIdx();
    }

    //点击删除Section按钮
    public void OnDeleteSectionButton( int iSectionIndex )
	{
		DeleteSection (iSectionIndex);
	}

    //追加一条Section
    public void AppendSection( string Name )
	{
		m_SectionMgr.AppendSection( Name, ms_SectionWidth );
		m_SectionNum += 1.0f;
	}

    //删除一条Section
    public void DeleteSection( int DelIndex )
	{
		if ( m_SectionMgr.DelSection( DelIndex ) )
		{
			// To do, delete all tracks
			m_SectionNum -= 1.0f;
		}
		
		/*int DeletTrackidx = EditDataContainer.DeleteSectionTrack(DelIndex);
		while(DeletTrackidx!=-1)
		{
			DeleteTrack(DeletTrackidx);
			DeletTrackidx = EditDataContainer.DeleteSectionTrack(DelIndex);
		}
		EditDataContainer.DeleteSection(DelIndex);
		
		for(int i=0; i<m_TrackMgr.GetTrackCount(); i++)
		{
			if(m_TrackMgr.IsTrackEmpty(i) == false)
			{
				TrackDataElement data = new TrackDataElement();
				GetOneTrackData(i, ref data);
				if(data.m_SectionIndex >  DelIndex)
				{
					float CurPosx = m_MovableRectMgr.GetCurentPosX(i*3);
					m_MovableRectMgr.SetRectPos(i*3, CurPosx-200);
					CurPosx = m_MovableRectMgr.GetCurentPosX(i*3+1);
					m_MovableRectMgr.SetRectPos(i*3+1, CurPosx-200);
				}
				
			}
		}*/
	}
    //插入一条Section
    public void InsertSection( string Name, int InsertIndex )
	{
		m_SectionMgr.InsertSection( Name, InsertIndex, ms_SectionWidth );
		m_SectionNum += 1.0f;
	}
    //追加一条元素数据
    public void AppendTrack( bool Empty, float h)
	{
		m_TrackMgr.AppendTrack( Empty, h );
		m_MovableRectMgr.AppendRectArray( Empty,  h);
		m_TrackNum += 1.0f;
		m_MovableRectMgr.CollectPositionData();
	}

    //插入一条元素数据
    public void InsertTrack( int Index, bool Empty , float h)
	{
		m_TrackMgr.InsertTrackAt( Index, Empty );
		m_MovableRectMgr.InsertRectArrayAt( Index, Empty , h);
		m_TrackNum += 1.0f;
		m_MovableRectMgr.CollectPositionData( );
	}
    //删除一条元素数据
    public void DeleteTrack( int Index )
	{
		if ( m_TrackMgr.DeleteTrack( Index ) && m_MovableRectMgr.DeleteRectArray( Index ) )
		{
			m_TrackNum -= 1.0f;
			m_MovableRectMgr.CollectPositionData();
		}
	}
    //清除所有元素数据
    public void ClearTrack()
	{
        m_Hashcodes.Clear();
		m_TrackMgr.Dispose();
		m_MovableRectMgr.Dispose ();
	}

    //线条控制器
    public class TrackMgr
	{
		List<Track> m_TrackPool = new List<Track>();
		public TrackMgr()
		{
		}
        public float ScrollHeight;
		public void OnGUI( int FirstDrawTrackLogicIndex, int MaxDrawTrackNum )
		{
			int TrackLogicIndex = 0;
			int MaxDrawLogicIndex = FirstDrawTrackLogicIndex + MaxDrawTrackNum;
            float acch = ScrollHeight;
			foreach ( var Element in m_TrackPool )
			{
				if ( TrackLogicIndex >= FirstDrawTrackLogicIndex && TrackLogicIndex < MaxDrawLogicIndex )
				{
					Track.ms_DrawLineIndex = ( TrackLogicIndex - FirstDrawTrackLogicIndex );
					Element.Draw(acch, Vector2.zero);
				}
				else if ( TrackLogicIndex >= MaxDrawLogicIndex )
				{
					break;
				}
				acch+=Element.m_height;
				
				++TrackLogicIndex;
			}
		}
		
		public void Dispose()
		{
			foreach ( var Element in m_TrackPool )
			{
				Element.Dispose();
			}
			
			m_TrackPool.Clear();
		}
		
		public int GetTrackCount()
		{
			return m_TrackPool.Count;
		}
		
		public bool IsTrackEmpty(int idx)
		{
			return m_TrackPool[idx].IsEmpty ();
		}
		
		public void AppendTrack( bool Empty, float h )
		{
			Track NewTrack = new Track( Empty );
			NewTrack.Init();
			NewTrack.m_height = h;
			m_TrackPool.Add( NewTrack );
		}
		
		public void InsertTrackAt( int Index, bool Empty )
		{
			Track NewTrack = new Track( Empty );
			NewTrack.Init();
			m_TrackPool.Insert( Index, NewTrack );
		}
		
		public bool DeleteTrack( int Index )
		{
			bool Result = false;
			if ( Index >= 0 && Index < m_TrackPool.Count )
			{
				m_TrackPool[ Index ].Dispose();
				m_TrackPool.RemoveAt( Index );
				Result = true;
			}
			
			return Result;
		}
		
        //画滑块底下的线条
		class Track : CustomUI
		{
			//private static float ms_LineHeight = 20.0f;
			private static Rect ms_LineRect = new Rect( ms_TrackViewWidth, 0.0f, EditorWindow_W - ms_InspectorViewWidth - ms_VerticalScrollBarWidth - ms_TrackViewWidth, 1.0f );
			public static int ms_DrawLineIndex = 0;
			public float m_height;
			// public int m_BelongSectionIndex = 0;
			bool m_EmptyTrack = false;
			
			public Track( bool Empty )
			{
				m_EmptyTrack = Empty;
			}
			
			public override void Init()
			{
				CreateTextures( new List<string> { "am_box_lightblue" } );
			}
			
			public override void Draw(float currentheight, Vector2 mousepos)
			{
				if ( !m_EmptyTrack )
				{
					ms_LineRect.y = currentheight + ms_MenuBarHeight + ms_ControlBarHeight + m_height*0.25f;
					GUI.DrawTexture( ms_LineRect, RetrieveTex( 0 ) );
				}
			}
			
			public bool IsEmpty()
			{
				return m_EmptyTrack;
			}
		}
	}

    //逐帧模式下用到
    public class Slider
    {
        Rect m_DrawRC;
        Rect m_Bar;
        Texture m_Tex = null;
        Texture m_Tex1 = null;
        float lastposx = 0;
        float m_NorTime = 0;
        bool DragBegin = false;
        public Slider()
        {
            m_Tex = (Texture)Resources.Load("am_box_yellow");
            m_Tex1 = (Texture)Resources.Load("am_box_red");
            m_DrawRC.x = 200;
            m_DrawRC.y = 40;
            m_DrawRC.width = 10;
            m_DrawRC.height = 20;

            m_Bar.x = m_DrawRC.x;
            m_Bar.y = m_DrawRC.y + m_DrawRC.height;
            m_Bar.width = 2;
            m_Bar.height = 600;
            
        }

        public void Reset()
        {
            m_DrawRC.x = 200;
            m_DrawRC.y = 40;
            m_DrawRC.width = 10;
            m_DrawRC.height = 20;

            m_Bar.x = m_DrawRC.x;
            m_Bar.y = m_DrawRC.y + m_DrawRC.height;
            m_Bar.width = 2;
            m_Bar.height = 600;
            m_NorTime = 0;
        }

        public float GetNorTime()
        {
            return m_NorTime;
        }
        public int GetSliderSecIdx()
        {
            return (int)((m_DrawRC.x - 200) / 200);
        }
        public void Draw(SkillWindowEditor Wnd, Event e)
        {
            
            if (e.type == EventType.mouseDown && EditorWindow.mouseOverWindow == Wnd)
            {
                Vector2 pos = e.mousePosition;
                lastposx = e.mousePosition.x;
                if (pos.x > m_DrawRC.x && pos.y < m_DrawRC.y + m_DrawRC.height && pos.x < m_DrawRC.x + m_DrawRC.width && pos.y > m_DrawRC.y)
                {
                    DragBegin = true;
                }
            }
            if (e.type == EventType.mouseDrag && EditorWindow.mouseOverWindow == Wnd)
            {
                Vector2 pos = e.mousePosition;
                if(DragBegin == true)
                {
                    float deltax = pos.x - lastposx;
                    m_DrawRC.x += deltax;
                    if (m_DrawRC.x < 200) m_DrawRC.x = 200;
                   
                    m_Bar.x += deltax;
                    if (m_Bar.x < 200) m_Bar.x = 200;
                    lastposx = pos.x;
                    m_NorTime = (m_DrawRC.x - 200) / 200;
                }
                
            }
            if (e.type == EventType.mouseUp && EditorWindow.mouseOverWindow == Wnd)
            {
                DragBegin = false;
            }


            GUI.DrawTexture(m_DrawRC, m_Tex);
            GUI.DrawTexture(m_Bar, m_Tex1);
        }
    }

    //Section外框整体控制绘图
    public class SectionMgr
	{
		// Dictionary<int, Section> m_SectionPool = new Dictionary<int, Section>();
		List<Section> m_SectionPool = new List<Section>();
        Vector2 mpos = Vector2.zero;
		public int BeingDeleteTrack = -1;

        public string GetSectionNameByIdx(int idx)
        {
            return m_SectionPool[idx].m_Name;
        }

        public int GetSectionCount()
        {
            return m_SectionPool.Count;
        }


        public void OnGUI(SkillWindowEditor Wnd, Event e, int FirstDrawSectionLogicIndex, int MaxDrawSectionNum, float XOffset, float PosY, float Height , ref SkillSectionData sec)
		{
            /*if (ms_CurSelectSectionIndex >= 0 && sec != null)
            {
                object ob = sec.GetSectionData(ms_CurSelectSectionIndex);
                EditSectionBase editBase = sec.GetSection(ms_CurSelectSectionIndex) as EditSectionBase;
                if (EditSectionWindow.Edit(ref ob, editBase.GetChineseName()))
                {
                    sec.SetSectionData(ms_CurSelectSectionIndex, ob);
                    ms_CurSelectSectionIndex = -1;
                }
            }*/
			// Draw Rect
           
            if (e.type == EventType.mouseDown && EditorWindow.mouseOverWindow)
            {
                mpos = e.mousePosition;
            }
			if ( FirstDrawSectionLogicIndex >= 0 )
			{

				int SectionLogicIndex = 0;
				int MaxDrawSectionLogicIndex = FirstDrawSectionLogicIndex + MaxDrawSectionNum;
				int DeleteIdx = -1;
				for ( int i=0; i<m_SectionPool.Count; i++ )
				{
					if ( SectionLogicIndex >= FirstDrawSectionLogicIndex && SectionLogicIndex < MaxDrawSectionLogicIndex )
					{
						Section.ms_SectionRect.x = XOffset;
						Section.ms_SectionRect.y = PosY;
						Section.ms_SectionRect.width = m_SectionPool[i].m_Width;
						Section.ms_SectionRect.height = Height;
                        m_SectionPool[i].Draw(0, mpos);
						if(m_SectionPool[i].IsBeingDelete() == true)
						{
							DeleteIdx = i;
						}
                        if (m_SectionPool[i].IsBeingEdit() == true)
                        {
                            object ob = sec.GetSectionData(i);
                            EditSectionBase editBase = sec.GetSection(i) as EditSectionBase;
                            if (EditSectionWindow.Edit(ref ob, editBase.GetChineseName()))
                            {
                                sec.SetSectionData(i, ob);
                            }
                        }
						XOffset += m_SectionPool[i].m_Width;
					}
					else if ( SectionLogicIndex >= MaxDrawSectionLogicIndex )
					{
						break;
					}
					
					++SectionLogicIndex;
				}

                if (DeleteIdx != -1)
                {
                    if (Wnd.m_IsinFramebyFrameMode == true)
                    {
                        Wnd.ShowNotification(new GUIContent("逐帧模式不允许删除阶段"));
                    }
                    else
                    {
                        BeingDeleteTrack = DeleteIdx;
                    }
                    
                }			
			}
		}
		
		public void AppendSection( string Name, float Width )
		{
			Section NewSection = new Section( Name, Width );
			NewSection.Init();
			m_SectionPool.Add( NewSection );
		}
		
		public void InsertSection(string Name, int index, float Width)
		{
			Section NewSection = new Section( Name, Width );
			NewSection.Init();
			m_SectionPool.Insert( index, NewSection );
		}
		
		public bool DelSection( int Index )
		{
			bool Result = false;
			if ( Index >= 0 && Index < m_SectionPool.Count )
			{
				m_SectionPool[ Index ].Dispose();
				m_SectionPool.RemoveAt( Index );
				Result = true;
			}
			
			return Result;
		}
		
		public void Dispose()
		{
			foreach ( var Element in m_SectionPool )
			{
				Element.Dispose();
			}
			
			m_SectionPool.Clear();
		}
		
		public class Section : CustomUI
		{
            bool ShowName = false;
			public string m_Name;
			public float m_Width = 0.0f;
			//public int gg = 0;
			bool bIsBeingDelete = false;
            bool bIsBeingEdit = false;
			public static Rect ms_SectionRect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
			static Rect  ms_LabelRect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
			static Rect  ms_LineRect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
			public static float ms_SectionCaptionHeight = 20.0f;
			static float ms_ButtonWidth = 15.0f;
            Texture m_DeleteTex = null;
            Texture m_EditTex = null;
			public Section( string SectionName, float Width )
			{
				m_Name  = SectionName;
				m_Width = Width;
			}

			public bool IsBeingDelete()
			{
				return bIsBeingDelete;
			}

            public bool IsBeingEdit()
            {
                return bIsBeingEdit;
            }
			
			public override void Init()
			{
				CreateTextures( new List<string> { "am_box_yellow" } );
                m_DeleteTex = (Texture)Resources.Load("tex_delete_hover");
                m_EditTex = (Texture)Resources.Load("am_icon_cameraswitcher"); 
			}
			
			public override void Draw(float currentheight, Vector2 mousepos)
			{
				// Draw
				ms_LabelRect.x = ms_SectionRect.x;
				ms_LabelRect.y = ms_SectionRect.y;
				ms_LabelRect.width = ms_SectionRect.width*0.5f;
				ms_LabelRect.height = ms_SectionCaptionHeight - 2.0f;
                if (mousepos.x > ms_LabelRect.x && mousepos.y < ms_LabelRect.y + ms_LabelRect.height && mousepos.x < ms_LabelRect.x + ms_LabelRect.width && mousepos.y > ms_LabelRect.y)
                {
                    ShowName = true;
                    
                }
                else
                {
                    ShowName = false;
                    GUI.Label(ms_LabelRect, m_Name);
                }

                if (ShowName == true)
                {
                    m_Name = GUI.TextField(ms_LabelRect, m_Name);
                }
				
				ms_LineRect.x = ms_SectionRect.x;
				ms_LineRect.y = ms_SectionRect.y;
				ms_LineRect.width = 1;
				ms_LineRect.height = ms_SectionRect.height;
				GUI.DrawTexture( ms_LineRect, RetrieveTex( 0 ) );
				
				ms_LineRect.x = ms_SectionRect.x;
				ms_LineRect.y = ms_SectionRect.y;
				ms_LineRect.width = ms_SectionRect.width;
				ms_LineRect.height = 1;
				GUI.DrawTexture( ms_LineRect, RetrieveTex( 0 ) );
				
				ms_LineRect.x = ms_SectionRect.x;
				ms_LineRect.y = ms_SectionRect.y + ms_SectionCaptionHeight;
				ms_LineRect.width = ms_SectionRect.width;
				ms_LineRect.height = 1;
				GUI.DrawTexture( ms_LineRect, RetrieveTex( 0 ) );
				
				ms_LineRect.x = ms_SectionRect.x + ms_SectionRect.width;
				ms_LineRect.y = ms_SectionRect.y;
				ms_LineRect.width = 1;
				ms_LineRect.height = ms_SectionRect.height;
				GUI.DrawTexture( ms_LineRect, RetrieveTex( 0 ) );
				
				// Draw Degree scale
				float ScaleStep = ms_SectionRect.width / 10.0f;
				ms_LineRect.x = ms_SectionRect.x;
				ms_LineRect.y = ms_SectionRect.y + 15.0f;
				ms_LineRect.width = 1.0f;
				ms_LineRect.height = 3.0f;
				for ( int i = 0; i < 11; ++i )
				{
					if ( 5 == i )
					{
						ms_LineRect.y = ms_SectionRect.y + 10.0f;
						ms_LineRect.height = 10.0f;
					}
					else
					{
						ms_LineRect.y = ms_SectionRect.y + 15.0f;
						ms_LineRect.height = 5.0f;
					}
					
					GUI.DrawTexture( ms_LineRect, RetrieveTex( 0 ) );
					ms_LineRect.x += ScaleStep;
				}
				
				// Draw Delete Button
				ms_LineRect.x = ms_SectionRect.x + ms_SectionRect.width - ms_ButtonWidth - 6.0f;
				ms_LineRect.y = ms_SectionRect.y + 1.0f;
				ms_LineRect.height = 16 /*- ms_ButtonWidth*/;
				ms_LineRect.width = 20;
                bIsBeingDelete = false;
                if (GUI.Button(ms_LineRect, m_DeleteTex, GUI.skin.GetStyle("ButtonImage")))
				{
					bIsBeingDelete = true;	
				}
                bIsBeingEdit = false;
                ms_LineRect.x -= 22;
                if (GUI.Button(ms_LineRect, m_EditTex, GUI.skin.GetStyle("ButtonImage")))
                {
                    bIsBeingEdit = true;
                }
			}
		}
		
	}


    //自定义UI
    public class CustomUI
	{
		List<Texture> m_TexArray = new List<Texture>();
		List<string>  m_TexName = new List<string>();
		
		public CustomUI()
		{
            
		}
		
		public virtual void Init()
		{
		}
		
		public Texture RetrieveTex( int TexIndex )
		{
			Texture Tex = null;
			if ( TexIndex >= 0 && TexIndex < m_TexArray.Count )
			{
				Tex = m_TexArray[ TexIndex ];
			}
			
			return Tex;
		}
		
		public void CreateTextures( List<string> TexName )
		{
			foreach ( var Name in TexName )
			{
				m_TexArray.Add( TexturePoolMgr.GetTexture( Name ) );
			}
			
			m_TexName = TexName;
		}
		
		public virtual void Dispose()
		{
			foreach ( var Tex in m_TexName )
			{
				TexturePoolMgr.ReleaseTexture( Tex );
			}
			m_TexArray.Clear();
		}
		
		public virtual void Draw(float currentheight,  Vector2 mousepos)
		{
		}
	}

    //滑动块视图控制器
    public class MovableRectMgr
	{
		List<RectArray> m_RectArrayPool = new List<RectArray>();
		int m_CurMovingRectID = -1;
		float m_DragXoffset = 0.0f;
		static float m_DragInTor = 2.0f;
		static float m_DragOutTor = 40.0f;
		static TrackDataElement ms_RetrieveData = new TrackDataElement();
		TrackData m_TrackData = new TrackData();

		public  int CurrentTrackSecid = 0;
		public  float CurrentStart= 0.0f;
		public  float CurrentEnd = 0.4f;
		public  int CurrentTrackIdx = -1;
        public float ScrollHeight;
		public MovableRectMgr()
		{
		}
		
		public int GetCurMovingRectID()
		{
			return m_CurMovingRectID;
		}
		
		public void AppendRectArray( bool Empty, float h)
		{
			RectArray NewRectArray = new RectArray( Empty  );
			NewRectArray.Init();
			NewRectArray.SetHeight (h);
			m_RectArrayPool.Add( NewRectArray );
		}
		
		public void InsertRectArrayAt( int Index )
		{
			InsertRectArrayAt (Index, false, 20);
		}
		
		public void InsertRectArrayAt( int Index, bool Empty, float h)
		{
			RectArray NewRectArray = new RectArray( Empty );
			NewRectArray.Init();
			NewRectArray.SetHeight (h);
			m_RectArrayPool.Insert( Index, NewRectArray );
		}
		
		public bool DeleteRectArray( int Index )
		{
			bool Result = false;
			if ( Index >= 0 && Index < m_RectArrayPool.Count )
			{
				m_RectArrayPool[ Index ].Dispose();
				m_RectArrayPool.RemoveAt( Index );
				Result = true;
			}
			
			return Result;
		}
		
		public void Dispose()
		{
			foreach ( var Element in m_RectArrayPool )
			{
				Element.Dispose();
			}
			m_RectArrayPool.Clear();
			m_TrackData.Dispose();
		}
		
		public void SetRectPos(int rectId, float pos)
		{
			RenderableRect TheRect = LocateRect( rectId );
			if ( null != TheRect )
			{
				TheRect.m_PosX = pos;
				TheRect.m_LogicPosX = TheRect.m_PosX-200;
			}
		}
		
		public float GetCurentPosX(int rectId)
		{
			RenderableRect TheRect = LocateRect( rectId );
			if ( null != TheRect )
			{
				return TheRect.m_PosX;
			}
			return -1;
		}

		public void GetOneTrackData( int TrackIndex, ref TrackDataElement Element )
		{
			if ( TrackIndex < m_TrackData.m_ValidDataNum )
			{
				Element.m_PosHead = m_TrackData.m_TrackData[ TrackIndex ].m_PosHead;
				Element.m_PosTail = m_TrackData.m_TrackData[ TrackIndex ].m_PosTail;
				Element.m_SectionIndex = m_TrackData.m_TrackData[ TrackIndex ].m_SectionIndex;
			}
		}
		
		public void OnGUI( SkillWindowEditor Wnd, Event e, int FirstDrawRectLogicIndex, int MaxDrawRectNum, int FirstDrawSectionLogicIndex )
		{

			// Draw Rect
			if ( FirstDrawSectionLogicIndex != RectArray.ms_CurFirstDrawSection )
			{
				RectArray.ms_CurFirstDrawSection = FirstDrawSectionLogicIndex;
				RectArray.ms_FirstDrawSectionIndexChange = true;
			}
			else
			{
				RectArray.ms_FirstDrawSectionIndexChange = false;
			}
			
			int RectArrayLogicIndex = 0;
			int MaxDrawRectLogicIndex = FirstDrawRectLogicIndex + MaxDrawRectNum;
			float totalheight = ScrollHeight;
			foreach ( var ItRectArray in m_RectArrayPool )
			{
				if ( RectArrayLogicIndex >= FirstDrawRectLogicIndex && RectArrayLogicIndex < MaxDrawRectLogicIndex )
				{
					RectArray.ms_DrawLineIndex = RectArrayLogicIndex - FirstDrawRectLogicIndex;
					
					GetOneTrackData( RectArrayLogicIndex, ref ms_RetrieveData );
					RectArray.ms_MySectionIndex = ms_RetrieveData.m_SectionIndex;

                    ItRectArray.Draw(totalheight, Vector2.zero);
					totalheight += ItRectArray.m_LineHeight;
				}
				else if ( RectArrayLogicIndex >= MaxDrawRectLogicIndex )
				{
					break;
				}
				++RectArrayLogicIndex;
			}

			if ( -1 == m_CurMovingRectID  && e.type == EventType.mouseDown && EditorWindow.mouseOverWindow == Wnd && TryToFindMovingRect( e.mousePosition ) )
			{
                // find 
				CurrentTrackIdx = (int)m_CurMovingRectID/3;
				m_CurMovingRectID = -1;

			}

			if ( -1 == m_CurMovingRectID && e.type == EventType.mouseDrag && EditorWindow.mouseOverWindow == Wnd && TryToFindMovingRect( e.mousePosition ) )
			{
			}
			else if ( (  m_CurMovingRectID  >= 0 && ( EditorWindow.mouseOverWindow != Wnd || e.type == EventType.mouseUp || MouseLeaveCurRect( e.mousePosition ) ) ) )
			{
				// Stop moving
				m_CurMovingRectID = -1;
				m_DragXoffset = 0.0f;
			}
			//int m = m_CurMovingRectID;
			if (  m_CurMovingRectID >= 0 )
			{
                RenderableRect TheRect = LocateRect(m_CurMovingRectID);
                if (null != TheRect)
                {
                    float LastPos = TheRect.m_PosX;
                    TheRect.m_PosX = e.mousePosition.x - m_DragXoffset;
                    if (TheRect.m_PosX > LastPos)
                    {
                        TheRect.m_LogicPosX += (TheRect.m_PosX - LastPos);
                    }
                    else
                    {
                        TheRect.m_LogicPosX -= (LastPos - TheRect.m_PosX);
                    }


                    if (m_CurMovingRectID % 3 == 0)
                    {
                        CurrentStart = (TheRect.m_LogicPosX) / ms_SectionWidth;
                        if (CurrentStart <= 0) CurrentStart = 0;
                    }
                    else if (m_CurMovingRectID % 3 == 1)
                    {
                        CurrentEnd = (TheRect.m_LogicPosX) / ms_SectionWidth;
                    }
                    CurrentTrackSecid = (int)Math.Floor(CurrentStart);

                }

            }
		}
		
        //鼠标是否处于滑块上
		bool IsMouseInRect( Vector2 MousePos, RenderableRect rc, float offset )
		{
			bool Result = false;
			if ( null != rc )
			{
				Result =  MousePos.x >= ( rc.m_PosX - offset ) && MousePos.x <= ( rc.m_PosX + rc.m_Width + offset ) &&
					MousePos.y >= ( rc.m_PosY - offset ) && MousePos.y <= ( rc.m_PosY + rc.m_Height + offset );
			}
			
			return Result;
		}

        //找出移动的滑块，并记录滑动距离
        bool TryToFindMovingRect(Vector2 MousePos)
        {
            int RectIndex = 0;
            foreach (var ItRectArray in m_RectArrayPool)
            {
                int ArrayRectCount = ItRectArray.GetRectArrayCount();
                for (int i = 0; i < ArrayRectCount; ++i)
                {
                    if (2 != RectIndex % 3)
                    {
                        RenderableRect TheRect = LocateRect(RectIndex);

                        if (null != TheRect && IsMouseInRect(MousePos, TheRect, m_DragInTor))
                        {
                            m_CurMovingRectID = RectIndex;
                            m_DragXoffset = MousePos.x - TheRect.m_PosX;
                            return true;
                        }
                    }
                    ++RectIndex;
                }
            }

            return false;
        }

        public void CollectPositionData(  )
		{
			m_TrackData.m_ValidDataNum = 0;
			int RectArrayIndex = 0;
			foreach ( var ItRectArray in m_RectArrayPool )
			{
				TrackDataElement TheElement = null;
				if ( RectArrayIndex < m_TrackData.m_TrackData.Count )
				{
					TheElement = m_TrackData.m_TrackData[RectArrayIndex];
				}
				else
				{
					TheElement = new TrackDataElement();
					m_TrackData.m_TrackData.Add( TheElement );
				}
				
				if ( null != TheElement )
				{
					if ( ItRectArray.IsEmpty() )
					{
						TheElement.m_PosHead = -1.0f;
						TheElement.m_PosTail = -1.0f;
						TheElement.m_SectionIndex = -1;
					}
					else
					{
						TheElement.m_PosHead = ( ItRectArray.GetRenderableRect( 0 ).m_LogicPosX ) / ms_SectionWidth;
						TheElement.m_PosTail = ( ItRectArray.GetRenderableRect( 1 ).m_LogicPosX ) / ms_SectionWidth;
						TheElement.m_SectionIndex = ( int )Math.Floor( TheElement.m_PosHead );
						TheElement.m_PosHead = TheElement.m_PosHead - (int)(TheElement.m_PosHead);
						TheElement.m_PosTail -= (float)TheElement.m_SectionIndex; 
					}
					++m_TrackData.m_ValidDataNum;
				}
				++RectArrayIndex;
			}
		}

        //根据index来获取RenderableRect
        RenderableRect LocateRect( int Index )
		{
			RenderableRect TheRect = null;
			if ( Index >= 0 )
			{
				int RectArrayIndex = Index / 3;
				int RectIndex = Index % 3;
				if ( RectArrayIndex < m_RectArrayPool.Count )
				{
					if ( !m_RectArrayPool[ RectArrayIndex ].IsEmpty() && RectIndex < m_RectArrayPool[ RectArrayIndex ].GetRectArrayCount() )
					{
						TheRect = m_RectArrayPool[ RectArrayIndex ].GetRenderableRect( RectIndex );
					}
				}
			}
			
			return TheRect;
		}
		
		bool MouseLeaveCurRect( Vector2 MousePos )
		{
			bool Result = false;
			
			RenderableRect Rect = LocateRect( m_CurMovingRectID );
			if ( null != Rect && !IsMouseInRect( MousePos, Rect, m_DragOutTor ) )
			{
				Result = true;
			}
			
			return Result;
		}

        //滑块头尾及中间段UI组件
        public class RectArray : CustomUI
		{
			private List<RenderableRect> m_RectArray = new List<RenderableRect>();
			public static int ms_DrawLineIndex = 0;
			public static int ms_CurFirstDrawSection = 0;
			public static bool ms_FirstDrawSectionIndexChange = false;
			public static int ms_MySectionIndex = 0;
			public float m_LineHeight = 20.0f;
			static float ms_RectHeight = 13.0f;
			static float ms_RectWidth  = 8.0f;
			bool m_Empty = false;
			public RectArray( bool Empty )
			{
				m_Empty = Empty;
			}

			public void SetHeight(float h)
			{
				m_LineHeight = h;
			}
			
			public bool IsEmpty()
			{
				return m_Empty;
			}
			
			public override void Init()
			{
				if ( !m_Empty )
				{
					CreateTextures( new List<string> { "am_box_red", "am_box_pink", "am_box_darkblue" } );
					if ( 0 == m_RectArray.Count )
					{
						float MinLeft = ms_TrackViewWidth;
						float MaxRight = EditorWindow_W - ms_InspectorViewWidth - ms_VerticalScrollBarWidth;
						m_RectArray.Add( new RenderableRect( MinLeft, 0.0f, ms_RectWidth, ms_RectHeight, RetrieveTex( 0 ), MinLeft, MaxRight - ms_RectWidth, 0.0f ) );
						m_RectArray.Add( new RenderableRect( MinLeft + 50.0f, 0.0f, ms_RectWidth, ms_RectHeight, RetrieveTex( 1 ), MinLeft, MaxRight, 50.0f ) );
						m_RectArray.Add( new RenderableRect( 0.0f, 0.0f, 0.0f, ms_RectHeight, RetrieveTex( 2 ), MinLeft, MaxRight, 0.0f ) );
					}
				}
			}
			
			public int GetRectArrayCount()
			{
				return 3;
			}
			
			public RenderableRect GetRenderableRect( int Index )
			{
				RenderableRect TheRect = null;
				if ( Index >= 0 && Index < m_RectArray.Count )
				{
					TheRect = m_RectArray[ Index ];
				}
				
				return TheRect;
			}

            public override void Draw(float currentheight, Vector2 mousepos)
			{
				if ( ms_MySectionIndex >= ms_CurFirstDrawSection && !m_Empty && 3 == m_RectArray.Count )
				{
					RenderableRect RectHead = m_RectArray[ 0 ];
					RectHead.m_PosY = currentheight + ms_ControlBarHeight + ms_MenuBarHeight /*+ ( m_LineHeight - ms_RectHeight ) * 0.5f*/  ;
					
					RenderableRect RectTail = m_RectArray[ 1 ];
					RectTail.m_PosY = RectHead.m_PosY;
					
					AdjustRectPos( RectHead, RectTail );
					
					// Fool!
					if ( ms_FirstDrawSectionIndexChange )
					{
						float ExpectMin = ( ms_MySectionIndex - ms_CurFirstDrawSection ) * ms_SectionWidth + ms_TrackViewWidth;
						float ExpectMax = ExpectMin + ms_SectionWidth;
						if ( RectHead.m_PosX > ExpectMax )
						{
							float Delta = ( ( int )( ( RectHead.m_PosX - ExpectMax ) / ms_SectionWidth ) + 1 ) * ms_SectionWidth;
							RectHead.m_PosX -= Delta;
							RectTail.m_PosX -= Delta;
						}
						else if ( RectHead.m_PosX < ExpectMin )
						{
							float Delta = ( ( int )( ( ExpectMin - RectHead.m_PosX ) / ms_SectionWidth ) + 1 ) * ms_SectionWidth;
							RectHead.m_PosX += Delta;
							RectTail.m_PosX += Delta;
						}
					}
					
					RenderableRect RectLink = m_RectArray[ 2 ];
					RectLink.m_PosX = RectHead.m_PosX + RectHead.m_Width;
					RectLink.m_PosY = RectHead.m_PosY;
					RectLink.m_Width = RectTail.m_PosX - RectLink.m_PosX;
					
					RectHead.Draw();
					RectTail.Draw();
					RectLink.Draw();
				}
			}
			
			void AdjustRectPos( RenderableRect R0, RenderableRect R1 )
			{
				float Delta = 0.0f;
				if ( R0.m_PosX < R0.m_MinLeft )
				{
					Delta = R0.m_MinLeft - R0.m_PosX;
					R0.m_PosX = R0.m_MinLeft;
					R0.m_LogicPosX += Delta;
				}
				
				if ( R0.m_PosX > ( R0.m_MaxRight - R0.m_Width ) )
				{
					Delta = R0.m_PosX - ( R0.m_MaxRight - R0.m_Width );
					R0.m_PosX = R0.m_MaxRight - R0.m_Width;
					R0.m_LogicPosX -= Delta;
				}
				
				// R0 push R1
				R1.m_MinLeft = R0.m_PosX + R0.m_Width;
				
				if ( R1.m_PosX < R1.m_MinLeft )
				{
					Delta = R1.m_MinLeft - R1.m_PosX;
					R1.m_PosX = R1.m_MinLeft;
					R1.m_LogicPosX += Delta;
				}
				
				if ( R1.m_PosX > ( R1.m_MaxRight - R0.m_Width ) )
				{
					Delta = R1.m_PosX - ( R1.m_MaxRight - R0.m_Width );
					R1.m_PosX = R1.m_MaxRight - R1.m_Width;
					R1.m_LogicPosX -= Delta;
				}
			}
		}

        //单个滑块
        public class RenderableRect
		{
			public float m_PosX = 0.0f;
			public float m_PosY = 0.0f;
			public float m_Width = 0.0f;
			public float m_Height = 0.0f;
			public float m_MinLeft = 0.0f;
			public float m_MaxRight = 0.0f;
			
			public float m_LogicPosX = 0.0f;
			public float m_LogicPosY = 0.0f;
			
			Rect m_DrawRC = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
			Texture m_Tex = null;
			
			void ChangeTexture( Texture Tex )
			{
				m_Tex = Tex;
			}
			
			public RenderableRect( float PosX, float PosY, float Width, float Height, Texture Tex, float MinLeft, float MaxRight, float LogicStartX )
			{
				m_LogicPosX = LogicStartX;
				m_LogicPosY = 0.0f;
				
				m_PosX = PosX;
				m_PosY = PosY;
				m_Width = Width;
				m_Height = Height;
				m_MinLeft = MinLeft;
				m_MaxRight = MaxRight;
				
				m_DrawRC.y = m_PosY;
				m_DrawRC.width = m_Width;
				m_DrawRC.height = m_Height;
				
				if ( !( m_MaxRight > m_MinLeft ) )
				{
					m_MinLeft = 0.0f;
					m_MaxRight = m_Width;
				}
				
				if ( m_MinLeft < 0.0f )
				{
					m_MinLeft = 0.0f;
				}
				
				if ( m_MaxRight < m_Width )
				{
					m_MaxRight = m_Width;
				}
				
				m_Tex = Tex;
			}
			
			public void Draw()
			{
				m_DrawRC.x = m_PosX;
				m_DrawRC.y = m_PosY;
				m_DrawRC.width = m_Width;
				m_DrawRC.height = m_Height;
				GUI.DrawTexture( m_DrawRC, m_Tex );
			}
		}
	}

    //滑块数据管理
    public class TrackData
	{
		public List<TrackDataElement> m_TrackData = new List<TrackDataElement>();
		public int m_ValidDataNum = 0;
		
		public TrackData()
		{
			
		}
		
		public void Dispose()
		{
			m_TrackData.Clear();
			m_ValidDataNum = 0;
		}
	}

    //滑块数据（开始点，结束点，以及SectionIndex）
    public class TrackDataElement
	{
		public float m_PosHead = -1.0f;
		public float m_PosTail = -1.0f;
		public int   m_SectionIndex = 0;
	}


}

