//------------------------------------------------------------------------------
// 技能编辑器主窗口
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class SkillWindowEditor : EditorWindow
{
    SkillPreview preview;

    //当前编辑器主窗口
    private static SkillWindowEditor ms_window = null;
    //当前技能数据文件物理完整路径
    public string m_strSkillFile = "";
    //当前技能数据文件Resource路径
    public string m_strResourcePath = "";
    //当前使用的路径及皮肤设置
    private SDE_Options sde_options = null;
    //编辑器宽度
    public static int EditorWindow_W = 1320;
    //编辑器高度
    public static int EditorWindow_H = 600;
    //编辑器名字
    private static string m_EditorName = "技能编辑器1.0";
    // 技能编辑器专用场景
    private static string ms_SceneFile = "Assets/Scenes/SkillEditor.unity"; 
    //当前使用的皮肤
    private GUISkin skin = null;
    //当前使用的皮肤名称
    private string skin_name = null;
    //是否从外部选中了技能数据文件
    private static bool m_bSelectFlag = false;
    //当前技能id
    private static int m_iCurSkillId = 0;
    //是否是逐帧模式
    public bool m_IsinFramebyFrameMode = false;
    
    
    //施法者id
    private string m_strCasterID = "110002";
    //目标怪id
    private string m_strMonsterID = "200002";
    //攻击半径
    private string m_strAttackRange = "3.0";

    private static int m_iMainEditor = 0;
    private static int m_iInspectorEditor = 1;
    //专门绘制左边技能元素
    private DataEdit m_Editor = new DataEdit(m_iMainEditor);
    //专门绘制右边Inspector
    private DataEdit m_SubEditor = new DataEdit(m_iInspectorEditor);
    //专门绘制中间技能阶段视图
    private TrackEditor m_TrackEditor = new TrackEditor();
    //技能在游戏中使用的总数据
    private static SkillDispData m_DispData = null;
    //技能在编辑器中使用的总数据
    private SkillEditData m_EditDataContainer = null;
    //存储Section阶段信息
    private SkillSectionData m_SkillSectionData = null;

    /// GuaGua Owner Begin
    /// [2/25/2014]
    #region GuaGuaControl
    public static float ms_TrackViewWidth = 200.0f;
    public static float ms_InspectorViewWidth = 300.0f;
    public static float ms_VerticalScrollBarWidth = 20.0f;
    public static float ms_ControlBarHeight = 20.0f;
    public static float ms_MenuBarHeight = 20.0f;
    public static float ms_IndicatorFootHeight = 20.0f;
    // Inspector UI
    Rect m_InspectorRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

    // Track UI
    Rect m_TrackRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

    // View UI
    Rect m_ScrollViewRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

    float m_HorizontalBarScrollPos = 0.0f;
    Rect m_HorizontalScrollBarRect = new Rect(0.0f, 0.0f, 0.0f, 20.0f);
    float m_VerticalBarScrollPos = 0.0f;
    Rect m_VerticalScrollBarRect = new Rect(0.0f, 0.0f, 20.0f, 0.0f);

    // Track Calculation
    float m_MoveStep = 1.0f;
    float m_OneTrackAreaHeight = 20.0f;
    float m_TrackNum = 0.0f;
    float m_SectionNum = 0.0f;
    public static float ms_SectionWidth = 200.0f;
    public static int ms_CurFirstDrawLogicSectionIndex = 0;
    public static int ms_CurFirstDrawLogicTrackIndex = 0;
    #endregion

    #region painting dimensions
    //按钮高度
    private float height_button = 15f;
    //按钮间距
    private float button_margin = 2f;
    private float width_playback_controls = 245f;
    private float height_playback_controls = 22f;
    #endregion

    #region Property inspector declaration
    //属性开始点坐标
    private Vector2 property_view_pos = new Vector2(0, 0);
    //元素开始点坐标
    private Vector2 element_view_pos = new Vector2(0, 0);
    #endregion

    #region textures
    //添加新元素的按钮贴图
    private Texture tex_icon_track = (Texture)Resources.Load("am_icon_track");
    //添加新元素的按钮悬停贴图
    private Texture tex_icon_track_hover = (Texture)Resources.Load("am_icon_track_hover");
    //添加新阶段的按钮贴图
    private Texture tex_icon_group_closed = (Texture)Resources.Load("am_icon_group_closed");
    //添加新阶段的按钮悬停贴图
    private Texture tex_icon_group_hover = (Texture)Resources.Load("am_icon_group_hover");
    //播放键纹理
    Texture StartFrameByFrameToggleTexture = null;
    //停止播放键纹理
    Texture StopFrameByFrameToggleTexture = null;
    #endregion

    #region Menu
    //点击添加新元素的菜单
    private GenericMenu skilldisp_track_menu = new GenericMenu();           // add track menu
    //点击添加新阶段的菜单
    private GenericMenu skilldisp_section_menu = new GenericMenu(); 			// add track menu
    #endregion

    

    SkillWindowEditor()
    {
        StartFrameByFrameToggleTexture = (Texture)Resources.Load("am_icon_event");
        StopFrameByFrameToggleTexture = (Texture)Resources.Load("am_icon_event");
    }

    [MenuItem("Window/SkillDisp Editor")]
    static void AddWindow()
    {
        Rect wr = new Rect(0, 0, EditorWindow_W, EditorWindow_H);
        ms_window = (SkillWindowEditor)EditorWindow.GetWindowWithRect(typeof(SkillWindowEditor), wr, true, m_EditorName);
        ms_window.minSize = new Vector2(EditorWindow_W, EditorWindow_H);
        ms_window.OnInit();
        ms_window.Show();
    }

    //初始化
    public void OnInit()
    {
        if (null == sde_options)
        {
            sde_options = SDE_Options.loadFile();
        }
        EditorApplication.projectWindowItemOnGUI += OnProjectItemOnGUI;
        //EditorApplication.playmodeStateChanged += SaveHistory;
        //EditorApplication.playmodeStateChanged += OnplaymodeStateChanged;


        bool haveOne = false;
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] objects = scene.GetRootGameObjects();
        foreach (var item in objects)
        {
            Debug.Log(item.name);
            if (item.name == "preview")
            {
                if (haveOne)
                {
                    Destroy(item);
                }
                else
                {
                    preview = item.GetComponent<SkillPreview>();
                }
                haveOne = true;
            }
        }

        if (!haveOne)
        {
            GameObject stupidobj = new GameObject("preview");
            preview = stupidobj.AddComponent<SkillPreview>();
        }
    }

    void Update()
    {
        if (ms_window == null)
        {
            ms_window = GetWindow<SkillWindowEditor>() as SkillWindowEditor;
        }
        Repaint();
    }

    void OnGUI()
    {
        if (sde_options == null)
        {
            OnInit();
        }

        SkillWindowEditor.loadSkin(sde_options, ref skin, ref skin_name, position);
        
        Event e = Event.current;

        Rect rectBtnNew = new Rect(button_margin, 0f, 50f, height_button);
        if (GUI.Button(rectBtnNew, "新建", EditorStyles.toolbarButton))
        {
            NewSkillDispWindow.AddWindow();
        }

        Rect rectBtnOpen = new Rect(rectBtnNew.x + rectBtnNew.width + button_margin, rectBtnNew.y, 50f, rectBtnNew.height);
        if (GUI.Button(rectBtnOpen, "打开", EditorStyles.toolbarButton))
        {
            bool bOpenFlag = true;
            var filePath = EditorUtility.OpenFilePanel("Open SkillDisp File", SDE_Options.GetSkillDispFilePath(), "bytes");
            if (filePath.Length != 0)
            {
                m_strSkillFile = filePath;
                m_strResourcePath = SDE_Options.FormatFileName(filePath, FileNameType.RESOURCE_NAME);
                m_iCurSkillId = GetSkillId(m_strResourcePath);
            }
            else
            {
                bOpenFlag = false;
            }
            if (bOpenFlag && ms_window != null)
            {
                ms_window.titleContent = new GUIContent(m_EditorName + "-" + m_strSkillFile);
                if (IsXmlFormat(m_strSkillFile))
                {
                    m_Editor.LoadXml<SkillDispData>(m_strSkillFile, ref m_DispData);
                }
                else
                {
                    m_DispData = (SkillDispData)m_Editor.Load<SkillDispData>(m_strResourcePath);
                }
                m_iCurSkillId = GetSkillId(m_strResourcePath);
                m_EditDataContainer = new SkillEditData();
                m_SkillSectionData = new SkillSectionData();
                m_EditDataContainer.OpenDispInfo(ref m_DispData, ref m_SkillSectionData);
            }
        }
        Rect rectBtnSave = new Rect(rectBtnOpen.x + rectBtnOpen.width + button_margin, rectBtnOpen.y, 50f, rectBtnOpen.height);
        if (GUI.Button(rectBtnSave, "保存", EditorStyles.toolbarButton))
        {
            m_EditDataContainer.SaveData(m_SkillSectionData, out m_DispData);

            
            if (!IsXmlFormat(m_strSkillFile))
            {
                m_Editor.Save<SkillDispData>(m_DispData, m_strSkillFile);
            }
            else
            {
                m_Editor.SaveXml<SkillDispData>(m_DispData, m_strSkillFile);
            }
        }

        Rect rectBtnApply = new Rect(rectBtnSave.x + rectBtnSave.width + button_margin, rectBtnOpen.y, 50f, rectBtnOpen.height);
        if (GUI.Button(rectBtnApply, "应用", EditorStyles.toolbarButton))
        {
            m_EditDataContainer.SaveData(m_SkillSectionData, out m_DispData);
            SkillManager.Instance.AddCacheDispData(m_iCurSkillId, m_DispData);
        }
        Rect rectBtnAutoConvert = new Rect(rectBtnApply.x + rectBtnApply.width + button_margin, rectBtnOpen.y, 70f, rectBtnOpen.height);
        if (GUI.Button(rectBtnAutoConvert, "一键转换", EditorStyles.toolbarButton))
        {
            string SelectedPath = "";
            int ret = EditorUtility.DisplayDialogComplex("一键转换", "将要转换所有技能数据，请选择技能备份数据的位置。", "默认位置", "指定位置", "取消");
        }

        Rect rectPre = rectBtnAutoConvert;
        rectPre.x = rectBtnSave.x + rectPre.width + 110;
        rectPre.width = 80;
        if (GUI.Button(rectPre, "创建施法者"))
        {

            Scene scene = SceneManager.GetActiveScene();
            string strCurScene = scene.path;

            if (ms_SceneFile != strCurScene)
            {
                EditorSceneManager.SaveOpenScenes();
                EditorSceneManager.OpenScene(ms_SceneFile);
                ShowNotification(new GUIContent("已经切换到编辑器场景，请运行场景后再尝试此操作！"));
            }
            else
            {
                if (!EditorApplication.isPlaying)
                {
                    ShowNotification(new GUIContent("场景没有在运行，请运行场景后再尝试此操作！"));
                }
                else
                {
                    //if (preview != null)
                    //{
                    //    // preview.CreateCaster(DataEdit.FormatFileName(m_strCasterID, FileNameType.RESOURCE_NAME), m_iCurSkillId);
                    ShowNotification(new GUIContent("已经创建放法者,请切换到技能编辑场景"));
                    //}

                }

            }
        }
        Rect rectGo = rectPre;
        rectGo.x = rectPre.x + rectPre.width + button_margin;
        rectGo.width = 120;
        m_strCasterID = EditorGUI.TextField(rectGo, m_strCasterID);

        rectPre.x = rectGo.x + rectGo.width + 10;
        rectPre.width = 80;
        if (GUI.Button(rectPre, "创建一个怪"))
        {
            if (m_iCurSkillId != 0)
            {
                if (preview != null)
                {
                    preview.StartFreeMode(m_strCasterID, m_strMonsterID, m_iCurSkillId, 1);
                }

            }
            else
            {
                ShowNotification(new GUIContent("请先打开技能或新建技能"));
            }
        }

        rectGo.x = rectPre.x + rectPre.width;
        m_strMonsterID = EditorGUI.TextField(rectGo, m_strMonsterID);

        rectGo.x += rectGo.width + 10;
        rectGo.width = 50;
        GUI.Label(rectGo, "攻击范围");
        rectGo.x += rectGo.width;
        rectGo.width = 50;
        m_strAttackRange = GUI.TextField(rectGo, m_strAttackRange);
        float fAttackRange = Str2Flt(m_strAttackRange);
        if (fAttackRange <= 0)
        {
            fAttackRange = 1.0f;
        }

        Texture playToggleTexture;
        if (false)
        {
            playToggleTexture = getSkinTextureStyleState("nav_stop").background;
        }
        else
        {
            playToggleTexture = getSkinTextureStyleState("nav_play").background;
        }
        Rect rectBtnTogglePlay = new Rect(rectGo.x + rectGo.width + 10, rectBtnSave.y, 32, 18);

        if (GUI.Button(rectBtnTogglePlay, playToggleTexture, GUI.skin.GetStyle("ButtonImage")))
        {
        }

        rectGo.x += 100;
        rectGo.width = 85;
        rectGo.height = 18;
        string labelString = "播放速度" + Time.timeScale.ToString();
        if (labelString.Length > 8)
        {
            labelString = labelString.Substring(0, 8);
        }
        if (GUI.Button(rectGo, labelString))
        {
            Time.timeScale = 1.0f;
        }
        rectGo.x += (rectGo.width + 2);
        rectGo.width = 70;
        float timeScale = GUI.HorizontalSlider(rectGo, Time.timeScale, 0.05f, 2.0f);
        Time.timeScale = timeScale;

        rectGo.x += rectGo.width + 20;
        rectGo.width = 90;
        if (GUI.Button(rectGo, "记录特效偏移"))
        {
            
        }

        rectGo.x += 100;
        rectGo.width = 20;
        bool oldMode = true;
        GUI.Toggle(rectGo, true, "");
        rectGo.x += rectGo.width;
        rectGo.width = 80;
        GUI.Label(rectGo, "使用预存刀光");

        Rect rectBtnToggleEdit = new Rect(rectGo.x + rectGo.width + 10 + rectBtnTogglePlay.width + 2, rectBtnSave.y, 32, 18);
        Texture ToggleeditTex = true ? StopFrameByFrameToggleTexture : StartFrameByFrameToggleTexture;
        if (GUI.Button(rectBtnToggleEdit, ToggleeditTex, GUI.skin.GetStyle("ButtonImage")))
        {
        }

        Rect rectAreaPlaybackControls = new Rect(0f, position.height - ms_IndicatorFootHeight, ms_TrackViewWidth + width_playback_controls, height_playback_controls);
        GUI.BeginGroup(rectAreaPlaybackControls);
        Rect rectNewTrack = new Rect(5f, ms_IndicatorFootHeight / 2f - 15f / 2f, 15f, 15f);
        Rect rectBtnNewTrack = new Rect(rectNewTrack.x, 0f, rectNewTrack.width, ms_IndicatorFootHeight);
        if (GUI.Button(rectBtnNewTrack, new GUIContent("", "New Element"), "label"))
        {
            if (false)
            {
                ShowNotification(new GUIContent("逐帧模式下不允许添加元素！"));
            }
            else if (skilldisp_track_menu.GetItemCount() <= 0)
            {
                buildAddTrackMenu();
            }

            skilldisp_track_menu.ShowAsContext();
        }
        GUI.DrawTexture(rectNewTrack, (rectBtnNewTrack.Contains(e.mousePosition) ? tex_icon_track_hover : tex_icon_track));


        Rect rectNewGroup = new Rect(rectNewTrack.x + rectNewTrack.width + 5f, ms_IndicatorFootHeight / 2f - 15f / 2f, 15f, 15f);
        Rect rectBtnNewGroup = new Rect(rectNewGroup.x, 0f, rectNewGroup.width, ms_IndicatorFootHeight);
        if (GUI.Button(rectBtnNewGroup, new GUIContent("", "New Group"), "label"))
        {
            if (false)
            {
                ShowNotification(new GUIContent("逐帧模式下不允许添加阶段！"));
            }
            //setScrollViewValue(maxScrollView());skilldisp_section_menu
            else if (skilldisp_section_menu.GetItemCount() <= 0)
            {
                buildAddSectionMenu();
            }

            skilldisp_section_menu.ShowAsContext();
        }
        GUI.DrawTexture(rectNewGroup, (rectBtnNewGroup.Contains(e.mousePosition) ? tex_icon_group_hover : tex_icon_group_closed));
        Rect rectDeleteElement = new Rect(rectNewGroup.x + rectNewGroup.width + 5f + 1f, ms_IndicatorFootHeight / 2f - 11f / 2f, 11f, 11f);
        Rect rectBtnDeleteElement = new Rect(rectDeleteElement.x, 0f, rectDeleteElement.width, ms_IndicatorFootHeight);

        GUIContent gcDeleteButton;
        string strTitleDeleteTrack = "Track";
        if (!GUI.enabled) gcDeleteButton = new GUIContent("");
        else gcDeleteButton = new GUIContent("", "Delete Track");
        if (GUI.Button(rectBtnDeleteElement, gcDeleteButton, "label"))
        {
            if (m_IsinFramebyFrameMode == true)
            {
                ShowNotification(new GUIContent("逐帧模式下不允许删除元素！"));
            }
            else
            {
                string strMsgDeleteTrack = "Effect_01";

                if ((EditorUtility.DisplayDialog("Delete " + strTitleDeleteTrack, "Are you sure you want to delete " + strMsgDeleteTrack + "?", "Delete", "Cancel")))
                {
                    int CurrentObj = m_Editor.GetSelectHash();
                    m_EditDataContainer.DeleteItem(CurrentObj);
                }
                else
                {

                }

            }
        }
        GUI.DrawTexture(rectDeleteElement, (getSkinTextureStyleState((GUI.enabled && rectBtnDeleteElement.Contains(e.mousePosition) ? "delete_hover" : "delete")).background));
        if (GUI.color.a < 1f) GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);

        Rect rectCopy = new Rect(rectDeleteElement.x + rectDeleteElement.width + 5f, ms_IndicatorFootHeight / 2f - 15f / 2f, 15f, 15f);
        Rect rectBtnCopy = new Rect(rectCopy.x, 0f, rectCopy.width, ms_IndicatorFootHeight);
        if (GUI.Button(rectBtnCopy, "Copy"))
        {
            m_EditDataContainer.CopyItem(m_Editor.GetSelectHash());
        }
        GUI.EndGroup();

        m_TrackRect.x = 0.0f;
        m_TrackRect.y = ms_ControlBarHeight + ms_MenuBarHeight;
        m_TrackRect.width = ms_TrackViewWidth;
        m_TrackRect.height = position.height - ms_ControlBarHeight - ms_MenuBarHeight - ms_IndicatorFootHeight;
        GUI.Box(m_TrackRect, "", GUI.skin.GetStyle("GroupElementBG"));
        GUILayout.BeginArea(m_TrackRect);
        Rect ElmementViewRt1 = new Rect();
        ElmementViewRt1.x = 0;
        ElmementViewRt1.y = 0;
        ElmementViewRt1.width = m_TrackRect.width;
        ElmementViewRt1.height = m_TrackRect.height;
        Rect ElementViewRt2 = ElmementViewRt1;
        ElementViewRt2.width = 1024;
        ElementViewRt2.height = 1024;
        element_view_pos = GUI.BeginScrollView(ElmementViewRt1, element_view_pos, ElementViewRt2);
        if (m_EditDataContainer != null)
        {
            //绘制左边技能元素
            m_Editor.EditInUI(m_EditDataContainer, typeof(SkillEditData), null, "技能", 0, m_SubEditor);
            //中间视图随着左边技能元素的折叠展开而变化
            m_TrackEditor.ChangeTrack(m_EditDataContainer, m_SkillSectionData, typeof(SkillEditData), null, ref m_Editor);

        }

        GUI.EndScrollView();
        GUILayout.EndArea();

        m_InspectorRect.x = position.width - ms_InspectorViewWidth;
        m_InspectorRect.y = ms_ControlBarHeight + ms_MenuBarHeight;
        m_InspectorRect.width = ms_InspectorViewWidth;
        m_InspectorRect.height = position.height - ms_ControlBarHeight - ms_MenuBarHeight;
        GUI.Box(m_InspectorRect, "", GUI.skin.GetStyle("GroupElementBG"));
        GUILayout.BeginArea(m_InspectorRect);
        Rect PropertyViewRt1 = new Rect();
        PropertyViewRt1.x = 0;
        PropertyViewRt1.y = 0;
        PropertyViewRt1.width = m_InspectorRect.width;
        PropertyViewRt1.height = m_InspectorRect.height;
        Rect PropertyViewRt2 = PropertyViewRt1;
        PropertyViewRt2.width = 1024;
        PropertyViewRt2.height = 1024;
        property_view_pos = GUI.BeginScrollView(PropertyViewRt1, property_view_pos, PropertyViewRt2);
        //绘制右边Inspector
        m_SubEditor.Edit(null, 0, null);
        //m_SubEditor1.Edit(1);
        GUI.EndScrollView();
        GUILayout.EndArea();

        m_ScrollViewRect.height = position.height - (ms_ControlBarHeight + ms_MenuBarHeight) - ms_IndicatorFootHeight;
        m_ScrollViewRect.width = position.width - ms_InspectorViewWidth - ms_TrackViewWidth - m_VerticalScrollBarRect.width;
        m_ScrollViewRect.x = ms_TrackViewWidth;
        m_ScrollViewRect.y = ms_ControlBarHeight + ms_MenuBarHeight;
        // Draw the Horizontal and vertical Scroll Bar
        float OneScreenSectionNum = m_ScrollViewRect.width / ms_SectionWidth;
        float OneScreenTrackNum = 50;
        m_HorizontalScrollBarRect.x = ms_TrackViewWidth + width_playback_controls;
        m_HorizontalScrollBarRect.y = position.height - ms_IndicatorFootHeight;
        m_HorizontalScrollBarRect.width = position.width - (ms_TrackViewWidth + width_playback_controls) - ms_InspectorViewWidth - m_VerticalScrollBarRect.width;
        float LastValue = m_HorizontalBarScrollPos;
        float CurValue = GUI.HorizontalScrollbar(m_HorizontalScrollBarRect, m_HorizontalBarScrollPos, OneScreenSectionNum, 0.0f, m_SectionNum < OneScreenSectionNum ? OneScreenSectionNum : m_SectionNum);
        if (CurValue > LastValue || CurValue < LastValue)
        {
            m_HorizontalBarScrollPos = CurValue > LastValue ? (m_HorizontalBarScrollPos + m_MoveStep) : (m_HorizontalBarScrollPos - m_MoveStep);
        }
        m_HorizontalBarScrollPos = GUI.HorizontalScrollbar(m_HorizontalScrollBarRect, m_HorizontalBarScrollPos, OneScreenSectionNum, 0.0f, m_SectionNum < OneScreenSectionNum ? OneScreenSectionNum : m_SectionNum);

        m_VerticalScrollBarRect.x = position.width - ms_InspectorViewWidth - m_VerticalScrollBarRect.width;
        m_VerticalScrollBarRect.y = ms_ControlBarHeight + ms_MenuBarHeight;
        m_VerticalScrollBarRect.height = m_ScrollViewRect.height;
        LastValue = m_VerticalBarScrollPos;
        CurValue = GUI.VerticalScrollbar(m_VerticalScrollBarRect, m_VerticalBarScrollPos, OneScreenTrackNum, 0.0f, m_TrackNum < OneScreenTrackNum ? OneScreenTrackNum : m_TrackNum);
        if (CurValue > LastValue || CurValue < LastValue)
        {
            m_VerticalBarScrollPos = CurValue > LastValue ? (m_VerticalBarScrollPos + m_MoveStep) : (m_VerticalBarScrollPos - m_MoveStep);
        }
        m_VerticalBarScrollPos = GUI.VerticalScrollbar(m_VerticalScrollBarRect, m_VerticalBarScrollPos, OneScreenTrackNum, 0.0f, m_TrackNum < OneScreenTrackNum ? OneScreenTrackNum : m_TrackNum);
        ms_CurFirstDrawLogicSectionIndex = (int)m_HorizontalBarScrollPos;
        ms_CurFirstDrawLogicTrackIndex = (int)m_VerticalBarScrollPos;
        //绘制中间技能块
        m_TrackEditor.OnGUI(ms_CurFirstDrawLogicSectionIndex, (int)OneScreenSectionNum, ms_TrackViewWidth, ms_MenuBarHeight, m_VerticalScrollBarRect.height + ms_ControlBarHeight,
                            ms_CurFirstDrawLogicTrackIndex, (int)OneScreenTrackNum,
                            this, e, ms_CurFirstDrawLogicTrackIndex, (int)OneScreenTrackNum, ref m_SkillSectionData, element_view_pos.y);

        
        if (m_EditDataContainer != null)
        {
            
            //更新滑块移动
            m_TrackEditor.EditTrack(m_EditDataContainer, m_SkillSectionData, ref m_Editor);
            if (m_SkillSectionData != null)
            {
                //随着滑块的移动更新属性
                m_EditDataContainer.UpdateWithSection(m_SkillSectionData);
            }
        }

    }

    void addTrackFromMenu(object type)
    {
        if (m_EditDataContainer != null)
        {
            int Etype = (int)type;
            m_EditDataContainer.AddEditData(Etype, null, 0);
        }
    }

    void addSectionFromMenu(object type)
    {
        int sectype = (int)type;
        m_SkillSectionData.AddSectionFromEditor(sectype, null);
    }

    //创建法术元素menu
    void buildAddTrackMenu()
    {
        skilldisp_track_menu.AddItem(new GUIContent("特效"), false, addTrackFromMenu, 0);
        skilldisp_track_menu.AddItem(new GUIContent("音效"), false, addTrackFromMenu, 1);
        skilldisp_track_menu.AddItem(new GUIContent("顿刀"), false, addTrackFromMenu, 2);
        skilldisp_track_menu.AddItem(new GUIContent("刀光"), false, addTrackFromMenu, 3);
        skilldisp_track_menu.AddItem(new GUIContent("位移"), false, addTrackFromMenu, 4);
        skilldisp_track_menu.AddItem(new GUIContent("子弹"), false, addTrackFromMenu, 5);
        skilldisp_track_menu.AddItem(new GUIContent("SHOOT点"), false, addTrackFromMenu, 6);
        skilldisp_track_menu.AddItem(new GUIContent("曲线位移"), false, addTrackFromMenu, 7);
        skilldisp_track_menu.AddItem(new GUIContent("角色闪光"), false, addTrackFromMenu, 8);
        skilldisp_track_menu.AddItem(new GUIContent("地表贴花"), false, addTrackFromMenu, 9);
        skilldisp_track_menu.AddItem(new GUIContent("镜头震动"), false, addTrackFromMenu, 10);
        skilldisp_track_menu.AddItem(new GUIContent("虚拟子弹"), false, addTrackFromMenu, 11);
        skilldisp_track_menu.AddItem(new GUIContent("曲线炮弹"), false, addTrackFromMenu, 12);
        skilldisp_track_menu.AddItem(new GUIContent("表现点"), false, addTrackFromMenu, 13);
        skilldisp_track_menu.AddItem(new GUIContent("直接伤害"), false, addTrackFromMenu, 14);
        skilldisp_track_menu.AddItem(new GUIContent("召唤怪物"), false, addTrackFromMenu, 15);
        skilldisp_track_menu.AddItem(new GUIContent("随机范围攻击"), false, addTrackFromMenu, 16);
        skilldisp_track_menu.AddItem(new GUIContent("游戏变速"), false, addTrackFromMenu, 17);
        skilldisp_track_menu.AddItem(new GUIContent("动作断点"), false, addTrackFromMenu, 18);
        skilldisp_track_menu.AddItem(new GUIContent("追踪弹"), false, addTrackFromMenu, 19);
        skilldisp_track_menu.AddItem(new GUIContent("闪现"), false, addTrackFromMenu, 20);
        skilldisp_track_menu.AddItem(new GUIContent("持续施法"), false, addTrackFromMenu, 21);
    }

    //创建法术阶段menu
    void buildAddSectionMenu()
    {
        skilldisp_section_menu.AddItem(new GUIContent("吟唱阶段"), false, addSectionFromMenu, 0);
        skilldisp_section_menu.AddItem(new GUIContent("施放阶段"), false, addSectionFromMenu, 1);
        skilldisp_section_menu.AddItem(new GUIContent("表现阶段"), false, addSectionFromMenu, 2);
        //skilldisp_section_menu.AddItem(new GUIContent(SkillEditData.GetSectionName(SectionType.SEC_TYPE_IMPACT, "Impact")), false, addSectionFromMenu, 3);
    }

    //获取技能编辑窗口
    public static SkillWindowEditor CurrentEditor
    {
        get { return (SkillWindowEditor)ms_window; }
    }

    //创建技能数据文件
    public void CreateSkill(int iSkillId)
    {
        m_strSkillFile = SDE_Options.GetSkillDispFilePath() + iSkillId + ".bytes";
        m_strResourcePath = SDE_Options.FormatFileName(m_strSkillFile, FileNameType.RESOURCE_NAME);
        m_iCurSkillId = GetSkillId(m_strResourcePath);
        ms_window.titleContent = new GUIContent(m_EditorName + "-" + m_strSkillFile);

        m_EditDataContainer = new SkillEditData();
        m_SkillSectionData = new SkillSectionData();
        PerformSection.PerformData data2 = new PerformSection.PerformData();
        m_SkillSectionData.AddSectionFromEditor(1, data2);
    }

    //保存当前技能数据
    public void SaveIfDirty()
    {
        m_Editor.Save<SkillDispData>(m_DispData, m_strSkillFile);
    }

    //重置技能预览中的技能数据
    private static bool ResetSkillData(int iCurSkillId)
    {

        //if (preview != null)
        //{
        //    preview.ResetSkillData(iCurSkillId);
        //    return true;
        //}


        return false;
    }

    //在project视图中选中一个技能数据文件时，将它加载进来
    private static void OnProjectItemOnGUI(string item, Rect selectionRect)
    {
        if (string.IsNullOrEmpty(item))
        {
            return;
        }

        if (!Event.current.isMouse)
        {
            return;
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {

            if (selectionRect.Contains(Event.current.mousePosition))
            {

                string path = AssetDatabase.GUIDToAssetPath(item);
                Debug.Log("OnProjectItemOnGUI------------------" + path);
                if (path.Contains("Config/Skill") && path.Contains(".bytes"))
                {
                    if (ms_window != null)
                    {
                        ms_window.SaveIfDirty();
                        ms_window.m_strSkillFile = path;
                        ms_window.m_strResourcePath = SDE_Options.FormatFileName(ms_window.m_strSkillFile, FileNameType.RESOURCE_NAME);
                        m_iCurSkillId = GetSkillId(ms_window.m_strResourcePath);
                    }

                    if (!ResetSkillData(m_iCurSkillId))
                    {
                        Debug.Log("Failed to reset skill data " + m_iCurSkillId);
                    }
                    m_bSelectFlag = true;
                }
            }
        }
    }


    #region 皮肤Style
    public static void loadSkin(SDE_Options sde_options, ref GUISkin _skin, ref string skinName, Rect position)
    {
        if (_skin == null || skinName == null || skinName != sde_options.skin)
        {
            _skin = (GUISkin)Resources.Load(sde_options.skin);
            skinName = sde_options.skin;
        }

        GUI.skin = _skin;
        GUI.color = GUI.skin.window.normal.textColor;
        GUI.DrawTexture(new Rect(0f, 0f, position.width, position.height), EditorGUIUtility.whiteTexture);
        GUI.color = Color.white;
    }

    public static GUIStyleState getSkinTextureStyleState(string name)
    {
        if (name == "properties_bg") return GUI.skin.GetStyle("Textures_1").normal;
        if (name == "delete") return GUI.skin.GetStyle("Textures_1").hover;
        if (name == "rename") return GUI.skin.GetStyle("Textures_1").active;
        if (name == "zoom") return GUI.skin.GetStyle("Textures_1").focused;
        if (name == "nav_skip_back") return GUI.skin.GetStyle("Textures_1").onNormal;
        if (name == "nav_play") return GUI.skin.GetStyle("Textures_1").onHover;
        if (name == "nav_skip_forward") return GUI.skin.GetStyle("Textures_1").onActive;
        if (name == "next_key") return GUI.skin.GetStyle("Textures_1").onFocused;
        if (name == "prev_key") return GUI.skin.GetStyle("Textures_2").normal;
        if (name == "nav_stop") return GUI.skin.GetStyle("Textures_2").hover;
        if (name == "accept") return GUI.skin.GetStyle("Textures_2").focused;
        if (name == "delete_hover") return GUI.skin.GetStyle("Textures_2").active;
        if (name == "popup") return GUI.skin.GetStyle("Textures_2").onNormal;
        if (name == "playonstart") return GUI.skin.GetStyle("Textures_2").onHover;
        if (name == "nav_stop_white") return GUI.skin.GetStyle("Textures_2").onActive;
        if (name == "select_all") return GUI.skin.GetStyle("Textures_2").onFocused;
        if (name == "x") return GUI.skin.GetStyle("Textures_3").normal;
        if (name == "select_this") return GUI.skin.GetStyle("Textures_3").hover;
        //if(name == "select_exclusive") return GUI.skin.GetStyle("Textures_3").active;
        Debug.LogWarning("Skin texture " + name + " not found.");

        return GUI.skin.label.normal;
    }
    #endregion

    private bool IsXmlFormat(string strPath)
    {
        return strPath.IndexOf(".xml") > 0;
    }

    private static int GetSkillId(string strSkillPath)
    {
        try
        {
            string strSkillId = strSkillPath.Substring(strSkillPath.LastIndexOf('/') + 1);
            int iDot = strSkillId.LastIndexOf('.');
            if (iDot >= 0)
            {
                strSkillId = strSkillId.Substring(0, iDot);
            }
            return int.Parse(strSkillId);
        }
        catch (Exception)
        {
            Debug.Log("Error skill file :" + strSkillPath);
            return 0;
        }

    }

    float Str2Flt(string str)
    {
        if (string.IsNullOrEmpty(str))
            return 0.0f;

        bool bDotExists = false;

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] >= '0' && str[i] <= '9')
            {
                continue;
            }

            if (str[i] == '.')
            {
                if (bDotExists)
                    return 0.0f;
                bDotExists = true;
            }
            else
            {
                return 0.0f;
            }
        }

        return float.Parse(str);
    }
}

#region 纹理资源池管理
public class TexturePoolMgr
{
    class TextureInfo
    {
        public Texture Tex = null;
        public uint RefCount = 0;
    }

    static Dictionary<string, TextureInfo> ms_TexturePool = new Dictionary<string, TextureInfo>();
    public static Texture GetTexture(string TexName)
    {
        TextureInfo TexInfo;
        if (ms_TexturePool.TryGetValue(TexName, out TexInfo))
        {
            ++TexInfo.RefCount;

            return TexInfo.Tex;
        }
        else
        {
            TexInfo = new TextureInfo();
            TexInfo.Tex = (Texture)Resources.Load(TexName);

            if (null != TexInfo.Tex)
            {
                TexInfo.RefCount = 1;
                ms_TexturePool.Add(TexName, TexInfo);
            }

            return TexInfo.Tex;
        }
    }

    public static void ReleaseTexture(string TexName)
    {
        TextureInfo TexInfo;
        if (ms_TexturePool.TryGetValue(TexName, out TexInfo))
        {
            --TexInfo.RefCount;
            if (0 == TexInfo.RefCount)
            {
                Resources.UnloadAsset(TexInfo.Tex);
                ms_TexturePool.Remove(TexName);
            }
        }
    }
}
public class TrackDataElement
{
    public float m_PosHead = -1.0f;
    public float m_PosTail = -1.0f;
    public int m_SectionIndex = 0;
}
#endregion
