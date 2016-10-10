//------------------------------------------------------------------------------
// *数据解析到界面**界面数据序列化到外部*
// 左边技能元素视图和右边Inspector视图的绘制
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

public enum FileNameType
{
    NONE,    /*不进行转换*/
    RESOURCE_NAME, /*获取相对Resources目录的相对路径名*/
                   //ASSET_NAME, /*获取相对Asset目录的相对路径名*/
                   //APP_NAME,  /*获取当前运行目录的相对路径*/
                   //FULL_NAME, /*获取全路径*/
}

public class DataEdit {
    public enum eWorkMode
    {
        eSerialToUI,  /*界面编辑*/
        eSerialToDisk, /*序列化到外设（硬盘等）*/
        eSerialFromDisk /*从外设读入*/
    }

    public enum eSerialType
    {
        //基本类型
        eInt, //Int32,UInt32,Int64,UInt64,SByte,Byte,Int16,UInt16
        eFloat,//Single,Double,Decimal
        eString,
        eEnum,
        eBoolean,
        //常见类型
        eVector2,
        eVector3,
        eVector4,
        eColor,
        eBounds,
        eArrayList,
        //

        eSelf_Defined,
        eSelf_Array,
        eAnimationCurve,
    }

    #region property 属性
    private int m_iSelectHashCode = 0; //用户手中选中编辑某一项
    private int m_iUserDefinedSelectedId = 0; //用户手动设置某一自定义项为选中状态
    private Dictionary<string, UIHandler> m_UiRenderDict = new Dictionary<string, UIHandler>();  //用于渲染UI
    private Dictionary<string, bool> m_UIBaseTypeName = new Dictionary<string, bool>(); //是否是基础类型
    private UIStateCenter m_uiState = new UIStateCenter();
    private bool m_bDirty = false;
    DataEdit m_subEditor = null;
    private object m_subOb; //以子项进行编辑的时候的对象
    private string m_obName;
    private int m_iEditorId = 0;
    private int m_iEditSelectedId = 0;
    private object m_EditObject;

    private static ObjectSelectCenter m_obGlobalState = new ObjectSelectCenter();
    CacheDictionary m_ArraySize = new CacheDictionary();

    TextAsset m_asset;
    MemoryStreamUtil out_stream;
    Stream m_stream;

    private Color m_restoreText; //文字着色备份
    private Color m_restoreBg;   //背景颜色备份

    private eWorkMode m_eWorkMode = eWorkMode.eSerialToUI;

    private Dictionary<int, bool> m_DirtyHash = new Dictionary<int, bool>();
    private Dictionary<int, bool> m_CollapseDict = new Dictionary<int, bool>();
    EnumIntDictionary m_EnumIntDict = new EnumIntDictionary();
    #endregion

    private DOnDataEditChanged m_changed;

    private delegate void UIHandler(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel);
    public delegate void DOnDataEditChanged();

    public DataEdit(int iEditorid)
    {
        m_iEditorId = iEditorid;
        m_iEditSelectedId = m_iEditorId;
        m_iUserDefinedSelectedId = m_iEditorId + 1;
        SetWorkMode(eWorkMode.eSerialToUI);
        RegisterUIRenders();
    }

#if UNITY_EDITOR
    UnityEditor.EditorWindow m_EditorWindow = null;
    //设置提示消息的窗口，如果不设的话，不会有信息提示
    public void SetNotificationWindow(UnityEditor.EditorWindow window)
    {
        m_EditorWindow = window;
    }
#endif
    public bool GetFoldState(int iObjHash)
    {
        return !IsCollapse(iObjHash);
    }

    public object EditObject
    {
        get { return m_EditObject; }
    }

    
#if UNITY_EDITOR
    private void ShowNotification(string strInfo)
    {
        if (m_EditorWindow)
        {
            m_EditorWindow.ShowNotification(new GUIContent(strInfo));
        }
        else
        {
            UnityEditor.EditorUtility.DisplayDialog("提示", strInfo, "OK");
        }
    }
#endif
    //创建类型实例
    private object CreateInstance(Type t)
    {
        if (t.Name == "String")
        {
            return (string)"";
        }
        return Assembly.GetAssembly(t).CreateInstance(t.FullName);
    }

    private void BackupStyle()
    {
        m_restoreText = GUI.color;
        m_restoreBg = GUI.backgroundColor;
    }

    private void RestoreStyle()
    {
        GUI.color = m_restoreText;
        GUI.backgroundColor = m_restoreBg;
    }

    private void SetEditStyle()
    {
        GUI.color = Color.white;
        GUI.backgroundColor = Color.green;
    }

    //获取以子项进行编辑的时候的对象
    private object EditSubObject(object ob, string obName)
    {
        object newOb = m_subOb;

        if (m_subOb == null || ob.GetHashCode() != m_subOb.GetHashCode())
        {
            m_subOb = ob;
        }

        m_obName = obName;

        return m_subOb;
    }

    public bool IsObjectDirty(int iObjHas)
    {
        return m_DirtyHash.ContainsKey(iObjHas);
    }


    public void ClearDirtyFlag(int iObjHas)
    {
        if (m_DirtyHash.ContainsKey(iObjHas))
        {
            m_DirtyHash.Remove(iObjHas);
        }
    }

    public bool IsObjectDirty(object ob)
    {
        return IsObjectDirty(ob.GetHashCode());
    }


    public void ClearDirtyFlag(object ob)
    {
        if (ob == null)
        {
            return;
        }

        ClearDirtyFlag(ob.GetHashCode());
        Type t = ob.GetType();
        if (!IsBaseType(t.Name))
        {
            FieldInfo[] ar_f = t.GetFields();
            foreach (var f in ar_f)
            {
                ClearDirtyFlag(f.GetValue(ob));
            }
        }
    }


    //设置改变
    private void SetDirty(bool bDirty)
    {
        m_bDirty = bDirty;
    }

    //是否改变
    public bool IsDirty()
    {
        return m_bDirty;
    }

    //获取被选择用来编辑的子项,子项用于在子编辑器中编辑
    public int GetSelectHash()
    {
        return m_uiState.GetInt(m_iEditSelectedId);
    }

    //设置被编辑的子项，子项用于在子编辑器中编辑
    public void SelectItem(int iHashCode)
    {
        m_iSelectHashCode = iHashCode;
    }

    //获取被选中的自定义子项，仅设置选中状态,子项要有ButtonFoldout属性
    public int GetFoucusItem()
    {
        return m_iUserDefinedSelectedId;
    }

    //设置被选中的自定义子项，仅设置选中状态,子项要有ButtonFoldout属性
    public void SelectFocusItem(int iHashCode)
    {
        m_iUserDefinedSelectedId = iHashCode;
    }

    //@brief设置某一项是收起
    //@param iHashCode 被设置项的哈希
    public void Collapse(int iHashCode)
    {
        if (!m_CollapseDict.ContainsKey(iHashCode))
        {
            m_CollapseDict.Add(iHashCode, true);
        }
    }


    //@brief设置某一项展开
    //@param iHashCode 被设置项的哈希
    public void Expand(int iHashCode)
    {
        if (m_CollapseDict.ContainsKey(iHashCode))
        {
            m_CollapseDict.Remove(iHashCode);
        }
    }

    //返回某一项是否是收起状态
    private bool IsCollapse(int iHashCode)
    {
        return m_CollapseDict.ContainsKey(iHashCode);
    }

    //返回某一项是否是收起状态
    private bool IsExpand(int iHashCode)
    {
        return !IsCollapse(iHashCode);
    }

    //加入改变列表
    private void AddDirtyObject(int iObjHas)
    {
        if (!m_DirtyHash.ContainsKey(iObjHas))
        {
            m_DirtyHash.Add(iObjHas, true);
        }
    }

    //获取成员的展示名，可以重定义可阅读的名字
    private string GetFieldCaption(FieldInfo fi)
    {
        var attrs = System.Attribute.GetCustomAttributes(fi);
        string strCaption = null;
        int iAttrCount = attrs.GetLength(0);
        for (int i = 0; i < iAttrCount; i++)
        {
            if (attrs[i] is StaticCaption)
            {
                strCaption = ((StaticCaption)attrs[i]).Value;
                break;
            }
        }

        if (strCaption != null)
        {
            return strCaption;
        }

        attrs = System.Attribute.GetCustomAttributes(fi.FieldType);
        iAttrCount = attrs.GetLength(0);
        for (int i = 0; i < iAttrCount; i++)
        {
            if (attrs[i] is StaticCaption)
            {
                strCaption = ((StaticCaption)attrs[i]).Value;
                break;
            }
        }

        if (strCaption != null)
        {
            return strCaption;
        }

        return fi.Name;
    }

    //是不是只读
    ReadOnly GetReadOnlyAttr(FieldInfo fi)
    {
        if (fi == null)
        {
            return null;
        }

        var attrs = System.Attribute.GetCustomAttributes(fi);
        int iAttrCount = attrs.GetLength(0);
        for (int i = 0; i < iAttrCount; i++)
        {
            if (attrs[i] is ReadOnly)
            {
                return attrs[i] as ReadOnly;
            }
        }

        return null;

    }
    
    // 是不是可折叠属性
    bool IsButtonFoldout(object ob)
    {
        bool bFlag = false;
        Type t = ob.GetType();
        var attrs = System.Attribute.GetCustomAttributes(t);
        int iAttrCount = attrs.GetLength(0);
        for (int i = 0; i < iAttrCount; i++)
        {
            if (attrs[i] is ButtonFoldout)
            {
                bFlag = true;
                break;
            }
        }

        return bFlag;
    }

    // 获取显示视图的样式
    GUIStyle GetSelectFoldoutStyle(bool bSelect, int iIndentLevel)
    {
        GUIStyle selectStyle = new GUIStyle();
        GUIStyleState st = new GUIStyleState();
        st.background = null;
        if (bSelect)
        {
            st.textColor = Color.green;
        }
        else
        {
            st.textColor = Color.white;
        }
        selectStyle.normal = st;
        selectStyle.margin = new RectOffset(12 * (iIndentLevel + 1), 0, 0, 0);
        return selectStyle;

    }

    //获取互斥属性
    private List<EnumMutex> GetEnumMutex(FieldInfo fi)
    {

        if (fi == null)
        {
            return null;
        }
        List<EnumMutex> ls = new List<EnumMutex>();
        var attrs = System.Attribute.GetCustomAttributes(fi);
        for (int i = 0; i < attrs.GetLength(0); i++)
        {
            if (attrs[i] is EnumMutex)
            {
                ls.Add(attrs[i] as EnumMutex);
            }
        }

        if (ls.Count == 0)
        {
            return null;
        }

        return ls;
    }

    //初始化数据类型
    private bool InitType(ref object ob, Type t)
    {
        if (t.BaseType.Name == "Array")
        {
            ob = Array.CreateInstance(t.GetElementType(), 0);
        }
        else
        {
            ob = CreateInstance(t);
        }
        return ob != null;
    }

    //设置工作模式
    private eWorkMode SetWorkMode(eWorkMode mode)
    {
        eWorkMode oldMode = m_eWorkMode;
        m_eWorkMode = mode;
        return oldMode;
    }

    public void SetListener(DOnDataEditChanged change)
    {
        m_changed = change;
    }

    //将数据序列化到视图上
    public void EditInUI(object ob, Type typeOfOb, FieldInfo fi, string obName, int iIndentLevel, DataEdit subEditor)
    {

        eWorkMode eOldMode = SetWorkMode(eWorkMode.eSerialToUI);
        m_subEditor = subEditor;
        SerialInUI(ref ob, typeOfOb, null, fi, obName, iIndentLevel);
        if (IsDirty())
        {
            if (m_changed != null)
            {
                m_changed();
            }
            SetDirty(false);
        }
        SetWorkMode(eOldMode);
    }

    //绘制右边Inspector视图
    public void Edit(FieldInfo fi, int iIndentLevel, DataEdit subEditor)
    {
        if (m_subOb != null)
        {
            EditInUI(m_subOb, m_subOb.GetType(), fi, m_obName, iIndentLevel, subEditor);
        }
    }

    #region 从文件加载技能数据
    public void SaveXml<T>(T data, string strXmlPath)
    {
#if UNITY_EDITOR
        XMLHandler xml = new XMLHandler();
        if (xml.CreateXML<T>(strXmlPath, data))
        {
            LogManager.Log("技能数据同时已经备份为:" + strXmlPath);
        }
        else
        {
            LogManager.LogError("技能数据备份失败:" + strXmlPath);
        }
#endif
    }

    public void Save<T>(object ob, string strFileName)
    {
        if (ob == null || !(ob is T))
        {
            return;
        }
#if UNITY_EDITOR

        eWorkMode eOldMode = SetWorkMode(eWorkMode.eSerialToDisk);

        try
        {
            m_stream = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            SerialInUI(ref ob, typeof(T), null, null, ob.GetType().ToString(), 0);
            m_stream.Flush();
            m_stream.Close();
            UnityEditor.AssetDatabase.Refresh();
        }
        catch (Exception e)
        {
            LogManager.LogError(e.Message);
        }

        SetWorkMode(eOldMode);
#endif
    }

    //加载二进制文件
    private bool LoadBin(string strFile)
    {
        m_asset = Resources.Load(strFile) as TextAsset;
        if (m_asset != null)
        {
            if (out_stream != null)
            {
                out_stream.Dispose();
                out_stream = null;
            }
            out_stream = new MemoryStreamUtil(m_asset.bytes);
        }
        return m_asset != null;
    }

    //加载xml文件
    public bool LoadXml<T>(string strFile, ref T data)
    {
#if UNITY_EDITOR
        XMLHandler xml = new XMLHandler();
        return xml.LoadXML<T>(strFile, ref data);
#else
            return false;
#endif
    }

    //加载数据文件
    public T Load<T>(string strFileName) where T : class
    {
        object ob = CreateInstance(typeof(T));
        eWorkMode eOldMode = SetWorkMode(eWorkMode.eSerialFromDisk);
        try
        {
            if (!LoadBin(strFileName))
            {
                LogManager.LogError("Load error.Does it exist? ->" + strFileName);
                return null;
            }
            SerialInUI(ref ob, typeof(T), null, null, "Skill Element", 0);
        }
        catch (Exception e)
        {
            LogManager.LogError("Load error:" + e.Message + "(while loading " + strFileName + ")");
        }
        SetWorkMode(eOldMode);
        return (T)ob;
    }
    #endregion

    //@param ob         当前要编辑的对象
    //@param fi         ob对应的FieldInfo
    //@param  obName    ob的名字
    //@brief  如果 ob为空，说明这是个值类型，无法自动为其生成一个对象
    //        如果 fi为空，则说明ob并不是某个类的成员，直接编辑ob。
    private void SerialInUI(ref object ob, Type t, object obParent, FieldInfo fi, string obName, int iIndentLevel)
    {
        if (m_eWorkMode != eWorkMode.eSerialFromDisk)
        {
            if (ob == null && fi == null)
            {
                //空类型
                LogManager.LogError("Fatal Error! ob and fi cannot be null at the same time!");
                return;
            }
        }

        if (ob == null)
        {
            if (!InitType(ref ob, t))
            {
                // Debuger.Log("Cannot create instance for " + fi.Name + "(" + fi.FieldType.Name + ")");
            }
        }
        RenderBuildInType(ref ob, t, obParent, fi, obName, iIndentLevel);

    }


    #region 渲染数据
    //Built-in 类型（注册已有类型的支持）
    private void RegisterUIRenders()
    {
        //基本类型
        RegisterUIHandler("Int32", new UIHandler(RenderInt32), true);//int
        RegisterUIHandler("UInt32", new UIHandler(RenderUInt32), true);//uint
        RegisterUIHandler("Int64", new UIHandler(RenderInt64), true);//int64
        RegisterUIHandler("UInt64", new UIHandler(RenderUInt64), true);//uint64
        RegisterUIHandler("String", new UIHandler(RenderString), true);//String
        RegisterUIHandler("Enum", new UIHandler(RenderEnum), true);//枚举
        RegisterUIHandler("Boolean", new UIHandler(RenderBool), true);//bool
        RegisterUIHandler("SByte", new UIHandler(RenderSByte), true); //有符号8位数
        RegisterUIHandler("Byte", new UIHandler(RenderByte), true);//无符号8位数
        RegisterUIHandler("Int16", new UIHandler(RenderInt16), true);//有符号16位
        RegisterUIHandler("UInt16", new UIHandler(RenderUInt16), true);//无符号16位
        RegisterUIHandler("Single", new UIHandler(RenderFloat), true);//float
        RegisterUIHandler("Double", new UIHandler(RenderDouble), true);//double
        RegisterUIHandler("Decimal", new UIHandler(RenderDecimal), true);//float

        //常见类型
        RegisterUIHandler("Vector2", new UIHandler(RenderVector2), false);//Vector2
        RegisterUIHandler("Vector3", new UIHandler(RenderVector3), false);//Vector3
        RegisterUIHandler("Vector4", new UIHandler(RenderVector4), false);//Vector4
        RegisterUIHandler("Color", new UIHandler(RenderColor), false); //Color
        RegisterUIHandler("Bounds", new UIHandler(RenderBounds), false); //Color
        RegisterUIHandler("ArrayList", new UIHandler(RenderArrayList), false); //Color
        RegisterUIHandler("AnimationCurve", new UIHandler(RenderAnimationCurve), false); //AnimationCurve

        //

        //自定义class
        RegisterUIHandler("Self_Defined", new UIHandler(RenderUserDefined), false); //自定义的Class
        RegisterUIHandler("Self_Array", new UIHandler(RenderUserDefinedArray), false); //任意类型声明的Class或struct
    }

    //注册类型
    private void RegisterUIHandler(string strTypeName, UIHandler dMethod, bool bBaseType)
    {
        m_UIBaseTypeName.Add(strTypeName, bBaseType);
        if (m_UiRenderDict.ContainsKey(strTypeName))
        {
            m_UiRenderDict[strTypeName] += dMethod;
        }
        else
        {
            m_UiRenderDict.Add(strTypeName, dMethod);
        }
    }

    //处理用户自定义数据类型
    private void RenderUserDefined(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    bool bDirty = false;
                    if (obEdit == null)
                    {
                        if (fi == null)
                        {
                            LogManager.LogError("Error: fi should not be null:" + strUICaption);
                            return;
                        }

                        obEdit = CreateInstance(fi.FieldType);
                    }

                    if (obEdit == null)
                    {
                        LogManager.LogError("Error: obEdit should not be null:" + strUICaption);
                        return;
                    }

                    bool bEditInSubWindow = false;

                    if (m_subOb != obEdit)
                    {
                        bEditInSubWindow = IsSubEditItem(obEdit);

                        if (!bEditInSubWindow && fi != null)
                        {
                            bEditInSubWindow = IsSubEditItem(fi);
                        }
                    }
                    string strCap = strUICaption;
                    if (fi != null && strCap == null)
                    {
                        strCap = GetFieldCaption(fi);
                    }
                    EditorGUI.indentLevel = iIndentLevel;
                    if (bEditInSubWindow)
                    {
                        int iHashCode = obEdit.GetHashCode();
                        bool bEditNow = false;

                        if (iHashCode == m_iSelectHashCode)
                        {
                            m_uiState.Set(m_iEditSelectedId, iHashCode);
                            m_EditObject = obEdit;
                            bEditNow = true;
                            m_iSelectHashCode = 0;
                        }
                        else
                        {
                            bEditNow = m_uiState.GetInt(m_iEditSelectedId) == iHashCode;
                        }

                        if (bEditNow)
                        {
                            BackupStyle();
                            SetEditStyle();
                        }

                        if (GUILayout.Button(strCap))
                        {
                            m_uiState.Set(m_iEditSelectedId, obEdit.GetHashCode());
                            m_EditObject = obEdit;
                            bEditNow = true;

                        }


                        if (bEditNow)
                        {
                            if (m_subEditor == null)
                            {
                                LogManager.LogError("There is no sub editor for " + strCap + "(" + strCap + ").You should set an sub editor in EditInUI by the subEditor parameter.Stop renderring");
                                return;
                            }
                            obEdit = m_subEditor.EditSubObject(obEdit, strCap);
                        }

                        if (bEditNow)
                        {
                            RestoreStyle();
                        }
                        return;
                    }

                    int iHasCode = obEdit.GetHashCode();
                    bool bValue = IsExpand(iHasCode);
                    Event e = Event.current;

                    bool bFoldState = bValue;

                    if (!IsButtonFoldout(obEdit))
                    {
                        string strFoldCap = strCap;
                        if (bFoldState)
                        {
                            strFoldCap = "▼" + strCap;
                        }
                        else
                        {
                            strFoldCap = "►" + strCap;
                        }

                        int iLastSelect = m_iUserDefinedSelectedId;
                        if (GUILayout.Button(strFoldCap, GetSelectFoldoutStyle(iHasCode == m_iUserDefinedSelectedId, iIndentLevel)))
                        {
                            bFoldState = !bValue;
                            m_iUserDefinedSelectedId = iHasCode;
                        }
                    }
                    else
                    {
                        bFoldState = EditorGUILayout.Foldout(bValue, strCap);
                    }

                    if (bFoldState)
                    {
                        Type obType = obEdit.GetType();
                        var f_list = obType.GetFields();
                        CacheDictionary cacheSize = new CacheDictionary(); //用于存储带有CacheSize属性的值
                        List<int> arryOb = new List<int>();

                        for (int i = 0; i < f_list.GetLength(0); i++)
                        {

                            object ob = f_list[i].GetValue(obEdit);
                            //如果该成员为整数，但以枚举值来进行编辑，则记下来，后面会使用到。
                            IntEnum ie = IsIntEnum(f_list[i], ob);
                            if (ie != null)
                            {
                                m_EnumIntDict.Set(ob.GetHashCode(), ie.EmType);
                                arryOb.Add(ob.GetHashCode());
                            }

                            //数组大小是否由某个成员来指定？获取属性来判断
                            CacheArray ca = GetCacheArrayProperty(f_list[i]);

                            if (!IsOmit(f_list[i]) || m_eWorkMode != eWorkMode.eSerialToUI)
                            {
                                if (ca != null)
                                {
                                    m_ArraySize.Set(ob.GetHashCode(), cacheSize.Get(ca.Id));
                                    arryOb.Add(ob.GetHashCode());
                                }

                                SerialInUI(ref ob, f_list[i].FieldType, obEdit, f_list[i], null, iIndentLevel + 1);
                                f_list[i].SetValue(obEdit, ob);
                                if (IsObjectDirty(ob.GetHashCode()))
                                {
                                    bDirty = true;
                                }
                            }


                            //如果该成员用来指定某个数组的大小，则标记下来后面用。
                            CacheSize cs = GetCacheSizeProperty(f_list[i], ob);
                            if (cs != null)
                            {
                                cacheSize.Set(cs.Id, cs.Size);
                            }

                            if (IsEnum(f_list[i]))
                            {
                                //如果是枚举，判断是否需要进行互斥判断
                                List<EnumMutex> em_ls = GetEnumMutex(f_list[i]);
                                if (em_ls != null)
                                {
                                    foreach (var em in em_ls)
                                    {
                                        //如果是互斥组，则要检查前面属于互斥组内的值
                                        for (int j = 0; j < i; j++)
                                        {
                                            List<EnumMutex> em_pre_ls = GetEnumMutex(f_list[j]);
                                            bool bBreak = false;
                                            foreach (var em_pre in em_pre_ls)
                                            {
                                                if (em_pre.Id == em.Id)
                                                {
                                                    object ob_pre = f_list[j].GetValue(obEdit);
                                                    object ob_cur = f_list[i].GetValue(obEdit);
                                                    if ((int)ob_pre == em_pre.Value && (int)ob_cur == em.Value)
                                                    {
                                                        //发生互斥,设当前的值为默认值
                                                        f_list[i].SetValue(obEdit, em.DefaultValue);
                                                        bBreak = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (bBreak)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var ob in arryOb)
                        {
                            m_ArraySize.Del(ob);
                            m_EnumIntDict.Del(ob);
                        }
                    }

                    Collapse(iHasCode); //先把旧的删除了，有可能新的希哈值不一样

                    if (!bFoldState)
                    {
                        Collapse(obEdit.GetHashCode());
                    }
                    else
                    {
                        Expand(obEdit.GetHashCode());
                    }

                    if (bDirty)
                    {
                        AddDirtyObject(obEdit.GetHashCode());
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eSelf_Defined), 0, sizeof(int));
                    if (obEdit == null)
                    {
                        if (fi == null)
                        {
                            LogManager.LogError("Error: fi should not be null:" + strUICaption);
                            return;
                        }

                        obEdit = CreateInstance(fi.FieldType);
                    }

                    if (obEdit == null)
                    {
                        LogManager.LogError("Error: obEdit should not be null:" + strUICaption);
                        return;
                    }

                    string strCap = strUICaption;
                    if (fi != null && strCap == null)
                    {
                        strCap = GetFieldCaption(fi);
                    }


                    int iHasCode = obEdit.GetHashCode();
                    bool bValue = m_uiState.Get(iHasCode);
                    Event e = Event.current;

                    Type obType = obEdit.GetType();
                    var f_list = obType.GetFields();

                    for (int i = 0; i < f_list.GetLength(0); i++)
                    {
                        object ob = f_list[i].GetValue(obEdit);
                        SerialInUI(ref ob, f_list[i].FieldType, obEdit, f_list[i], null, iIndentLevel + 1);
                    }

                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eSelf_Defined);
                    if (obEdit == null)
                    {
                        if (fi == null)
                        {
                            LogManager.LogError("Error: fi should not be null:" + strUICaption);
                            return;
                        }

                        obEdit = CreateInstance(fi.FieldType);
                    }

                    if (obEdit == null)
                    {
                        LogManager.LogError("Error: obEdit should not be null:" + strUICaption);
                        return;
                    }

                    string strCap = strUICaption;
                    if (fi != null && strCap == null)
                    {
                        strCap = GetFieldCaption(fi);
                    }

                    int iHasCode = obEdit.GetHashCode();
                    bool bValue = m_uiState.Get(iHasCode);
                    Event e = Event.current;

                    Type obType = obEdit.GetType();
                    var f_list = obType.GetFields();

                    for (int i = 0; i < f_list.GetLength(0); i++)
                    {
                        object ob = f_list[i].GetValue(obEdit);
                        SerialInUI(ref ob, f_list[i].FieldType, obEdit, f_list[i], null, iIndentLevel + 1);
                        f_list[i].SetValue(obEdit, ob);
                    }

                    break;
                }
        }
    }

    //重设数组size大小
    private object Resize(ref Array obElement, Type elemType, int iSize, int iNewSize)
    {
        object obTmp = Array.CreateInstance(elemType, iNewSize);

        for (int i = 0; i < iNewSize; i++)
        {
            object obNew = CreateInstance(elemType);
            (obTmp as Array).SetValue(obNew, i);
        }

        int iNewLength = iNewSize;
        if (iNewSize > iSize)
        {
            iNewLength = iSize;
        }
        for (int i = 0; i < iNewLength; i++)
        {
            (obTmp as Array).SetValue((obElement as Array).GetValue(i), i);
        }
        obElement = obTmp as Array;
        return obElement;
    }

    //重设数组size大小
    private int ResizeArray(ref object obElement, Type t, int iIndentlevel, int iSize)
    {
#if UNITY_EDITOR
        int iNewSize = 0;

        if (obElement == null)
        {
            //数组未初始化首先初始化个空的
            obElement = CreateInstance(t);
        }

        Type obType = obElement.GetType();
        int iHash = obElement.GetHashCode();
        //如果是一维数组，并且是界面可变的，则添加size使之可以动态变化
        EditorGUI.indentLevel = iIndentlevel;
        iNewSize = EditorGUILayout.IntField("Size", iSize);
        if (iSize != iNewSize)
        {
            Array ar = obElement as Array;
            object obTmp = Resize(ref ar, obType.GetElementType(), iSize, iNewSize);
            //m_uiState.Remove(iHash);
            obElement = obTmp;
            iHash = obElement.GetHashCode();
            //m_uiState.Set(iHash, iNewSize);
        }
        return iNewSize;
#else
            return 0;
#endif
    }

    //渲染自定义的数据
    private void RenderUserDefinedArray(ref object obArrObject, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        Array ar = obArrObject as Array;
        int iSize = ar.GetLength(0);
        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    Type elemType;
                    int iHasCode = obArrObject.GetHashCode();
                    bool bValue = IsExpand(iHasCode);

                    if (obArrObject == null)
                    {
                        elemType = fi.FieldType.GetElementType();
                    }
                    else
                    {
                        elemType = obArrObject.GetType().GetElementType();
                    }

                    Event e = Event.current;
                    EditorGUI.indentLevel = iIndentLevel;

                    bool bFixedArray = false;

                    if (obArrObject != null)
                    {
                        iSize = (obArrObject as Array).GetLength(0);
                        bFixedArray = IsFixedArray(fi);
                    }

                    string arCaption = strUICaption;

                    if (fi != null)
                    {
                        arCaption = GetFieldCaption(fi);
                    }

                    bool bFoldState = bValue;

                    iSize = ar.GetLength(0);
                    if (m_ArraySize.Exit(iHasCode))
                    {
                        iSize = m_ArraySize.Get(iHasCode);
                    }

                    if (iSize <= 0 && IsShowEmptyArray(fi) || iSize > 0)
                    {

                        bFoldState = EditorGUILayout.Foldout(bValue, arCaption + "[" + iSize + "]"); //这个size下帖再更新吧:)

                        if (!bFixedArray)
                        {
                            iSize = ResizeArray(ref obArrObject, fi.FieldType, iIndentLevel + 1, iSize);
                        }
                    }

                    ar = (obArrObject as Array);
                    int iNewHasCode = ar.GetHashCode();
                    if (bFoldState)
                    {
                        for (int i = 0; i < iSize; i++)
                        {
                            object arElem = ar.GetValue(i);
                            if (arElem == null)
                            {
                                //又碰到数组元素是值类型
                                LogManager.LogError("Error: " + arCaption + "[" + i + "] is NULL.Stop Renderring it.");
                            }
                            else
                            {
                                RenderBuildInType(ref arElem, arElem.GetType(), obParent, fi, arCaption + "[" + i + "]", iIndentLevel + 1);
                            }
                            ar.SetValue(arElem, i);
                        }
                    }

                    m_uiState.Remove(iHasCode);
                    m_uiState.Set(iNewHasCode, iSize);

                    Collapse(iHasCode); //先把旧的删除了，有可能新的希哈值不一样

                    if (!bFoldState)
                    {
                        Collapse(obArrObject.GetHashCode());
                    }
                    else
                    {
                        Expand(obArrObject.GetHashCode());
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eSelf_Array), 0, sizeof(int));
                    for (int i = 0; i < iSize; i++)
                    {
                        object arElem = ar.GetValue(i);
                        if (arElem == null)
                        {
                            //The size of the array may be set by annother member,so it should stop while it meets null value.
                            iSize = i;
                            break;
                        }
                    }

                    m_stream.Write(BitConverter.GetBytes(iSize), 0, sizeof(int));
                    string arCaption = strUICaption;

                    if (fi != null)
                    {
                        arCaption = GetFieldCaption(fi);
                    }
                    for (int i = 0; i < iSize; i++)
                    {
                        object arElem = ar.GetValue(i);
                        RenderBuildInType(ref arElem, arElem.GetType(), obParent, fi, arCaption + "[" + i + "]", iIndentLevel + 1);
                        ar.SetValue(arElem, i);
                    }
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eSelf_Array);
                    string arCaption = strUICaption;

                    if (fi != null)
                    {
                        arCaption = GetFieldCaption(fi);
                    }
                    iSize = out_stream.ReadInt();
                    Resize(ref ar, ar.GetType().GetElementType(), ar.GetLength(0), iSize);
                    for (int i = 0; i < iSize; i++)
                    {
                        object arElem = ar.GetValue(i);
                        RenderBuildInType(ref arElem, arElem.GetType(), obParent, fi, arCaption + "[" + i + "]", iIndentLevel + 1);
                        ar.SetValue(arElem, i);
                    }
                    obArrObject = ar as object;
                    break;
                }
        }
    }

    //渲染bool类型
    private void RenderBool(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        bool bValue = (bool)obEdit;
        string strCap = strUICaption;
        bool bValueNew = bValue;
        if (fi != null)
        {
            strCap = GetFieldCaption(fi);
        }
        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    UnityEditor.EditorGUI.indentLevel = iIndentLevel;
                    bValueNew = UnityEditor.EditorGUILayout.Toggle(strCap, bValue);
                    obEdit = bValueNew;
                    if (bValueNew != bValue)
                    {
                        SetDirty(true);
                        AddDirtyObject(obEdit.GetHashCode());
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eBoolean), 0, sizeof(int));
                    m_stream.Write(BitConverter.GetBytes(bValue), 0, sizeof(bool));
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eBoolean);
                    bValueNew = out_stream.ReadBool();
                    obEdit = bValueNew;
                    break;
                }
        }
    }

    //渲染float类型
    private void RenderFloat(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        float fValue = (float)obEdit;
        if (fValue != ConverFloatField(ref fValue, fi, strUICaption, iIndentLevel))
        {
            SetDirty(true);
            AddDirtyObject(obEdit.GetHashCode());
        }
        obEdit = fValue;
    }

    //渲染double类型
    private void RenderDouble(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        float fValue = (float)(double)obEdit;
        if (fValue != ConverFloatField(ref fValue, fi, strUICaption, iIndentLevel))
        {
            SetDirty(true);
            AddDirtyObject(obEdit.GetHashCode());
        }
        obEdit = (double)fValue;
    }

    //渲染Decimal类型
    private void RenderDecimal(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        float fValue = (float)(Decimal)obEdit;
        if (fValue != ConverFloatField(ref fValue, fi, strUICaption, iIndentLevel))
        {
            SetDirty(true);
            AddDirtyObject(obEdit.GetHashCode());
        }
        obEdit = (decimal)fValue;
    }

    //渲染Int16类型
    private void RenderInt16(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        if (RenderIntAsEnum(ref obEdit, obParent, fi, strUICaption, iIndentLevel))
        {
            return;
        }

        int intValue = (int)(Int16)obEdit;
        if (intValue != ConverNumberField(ref intValue, obParent, fi, strUICaption, iIndentLevel))
        {
            SetDirty(true);
            obEdit = (Int16)intValue;
            AddDirtyObject(obEdit.GetHashCode());
        }
        else
        {
            obEdit = (Int16)intValue;
        }
    }

    //渲染UInt16类型
    private void RenderUInt16(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        if (RenderIntAsEnum(ref obEdit, obParent, fi, strUICaption, iIndentLevel))
        {
            return;
        }

        int intValue = (int)(UInt16)obEdit;
        if (intValue != ConverNumberField(ref intValue, obParent, fi, strUICaption, iIndentLevel))
        {
            SetDirty(true);
            obEdit = (UInt16)intValue;
            AddDirtyObject(obEdit.GetHashCode());
        }
        else
        {
            obEdit = (UInt16)intValue;
        }
    }

    //渲染Enum类型
    private void RenderEnumInternal(ref object obEdit, Type t, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        FieldInfo[] efi = t.GetFields();
        int iCount = efi.GetLength(0);
        int iValue = 0;

        if (IsInt16(obEdit))
        {
            iValue = (int)(Int16)obEdit;
        }
        else if (IsUInt16(obEdit))
        {
            iValue = (int)(UInt16)obEdit;
        }
        else if (IsInt32(obEdit))
        {
            iValue = (int)(Int32)obEdit;
        }
        else if (IsUInt32(obEdit))
        {
            iValue = (int)(UInt32)obEdit;
        }
        else if (IsInt64(obEdit))
        {
            iValue = (int)(Int64)obEdit;
        }
        else if (IsUInt64(obEdit))
        {
            iValue = (int)(UInt64)obEdit;
        }
        else
        {
            iValue = (int)obEdit;
        }


        int iValueNew = iValue;
        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    UnityEditor.EditorGUI.indentLevel = iIndentLevel;
                    ArrayList arMenu = new ArrayList();
                    ArrayList arValue = new ArrayList();
                    for (int i = 1; i < iCount; i++)
                    {
                        if (!IsOmit(efi[i]))
                        {
                            arMenu.Add(GetFieldCaption(efi[i]));
                            arValue.Add(i - 1);
                        }
                    }
                    string strCap = strUICaption;
                    if (fi != null)
                    {
                        strCap = GetFieldCaption(fi);
                    }

                    string[] strMenu = new string[arMenu.Count];

                    for (int i = 0; i < arMenu.Count; i++)
                    {
                        strMenu[i] = (string)arMenu[i];
                    }
                    int index = -1;

                    for (int i = 0; i < arMenu.Count; i++)
                    {
                        if ((int)arValue[i] == iValue)
                        {
                            index = i;
                        }
                    }

                    if (index < 0)
                    {
                        // LogManager.LogError("Error,Cannot found enumerate value for " + strUICaption);
                        return;
                    }

                    iValueNew = EditorGUILayout.Popup(strCap, index, strMenu);

                    //如果是只读的，不允许修改
                    if (iValueNew != index)
                    {

                        ReadOnly readOnlyAttr = GetReadOnlyAttr(fi);
                        if (readOnlyAttr != null)
                        {
                            if (readOnlyAttr.ShowTip)
                            {
                                ShowNotification("属性只读！");
                            }
                            iValueNew = index;
                        }
                    }

                    if (IsInt16(obEdit))
                    {
                        obEdit = (Int16)(int)arValue[iValueNew];
                    }
                    else if (IsUInt16(obEdit))
                    {
                        obEdit = (UInt16)(int)arValue[iValueNew];
                    }
                    else if (IsInt32(obEdit))
                    {
                        obEdit = (Int32)(int)arValue[iValueNew];
                    }
                    else if (IsUInt32(obEdit))
                    {
                        obEdit = (UInt32)(int)arValue[iValueNew];
                    }
                    else if (IsInt64(obEdit))
                    {
                        obEdit = (Int64)(int)arValue[iValueNew];
                    }
                    else if (IsUInt64(obEdit))
                    {
                        obEdit = (UInt64)(int)arValue[iValueNew];
                    }
                    else
                    {
                        obEdit = (int)arValue[iValueNew];
                    }

                    if (iValueNew != index)
                    {
                        SetDirty(true);
                        AddDirtyObject(obEdit.GetHashCode());
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eEnum), 0, sizeof(int));
                    m_stream.Write(BitConverter.GetBytes(iValue), 0, sizeof(int));
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eEnum);
                    iValueNew = out_stream.ReadInt();
                    obEdit = iValueNew;
                    break;
                }
        }
    }

    //渲染枚举类型
    private void RenderEnum(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        Type t = obEdit.GetType();
        RenderEnumInternal(ref obEdit, t, obParent, fi, strUICaption, iIndentLevel);
    }

    //int型当做枚举类型渲染
    private bool RenderIntAsEnum(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        Type t = m_EnumIntDict.Get(obEdit.GetHashCode());
        if (t != null)
        {
            RenderEnumInternal(ref obEdit, t, obParent, fi, strUICaption, iIndentLevel);

            return true;
        }

        return false;
    }

    //渲染Int32类型
    private void RenderInt32(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        if (RenderIntAsEnum(ref obEdit, obParent, fi, strUICaption, iIndentLevel))
        {
            return;
        }

        int intValue = (int)(Int32)obEdit;
        if ((int)obEdit != ConverNumberField(ref intValue, obParent, fi, strUICaption, iIndentLevel))
        {
            SetDirty(true);
            obEdit = (Int32)intValue;
            AddDirtyObject(obEdit.GetHashCode());
        }
        else
        {
            obEdit = (Int32)intValue;
        }
    }

    //渲染UInt32类型
    private void RenderUInt32(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        if (RenderIntAsEnum(ref obEdit, obParent, fi, strUICaption, iIndentLevel))
        {
            return;
        }

        int intValue = (int)(UInt32)obEdit;
        if ((UInt32)obEdit != ConverNumberField(ref intValue, obParent, fi, strUICaption, iIndentLevel))
        {
            SetDirty(true);
            obEdit = (UInt32)intValue;
            AddDirtyObject(obEdit.GetHashCode());
        }
        else
        {
            obEdit = (UInt32)intValue;
        }
    }

    //渲染Int64类型
    private void RenderInt64(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        if (RenderIntAsEnum(ref obEdit, obParent, fi, strUICaption, iIndentLevel))
        {
            return;
        }

        int intValue = (int)(Int64)obEdit;
        if (intValue != ConverNumberField(ref intValue, obParent, fi, strUICaption, iIndentLevel))
        {
            SetDirty(true);
            obEdit = (Int64)intValue;
            AddDirtyObject(obEdit.GetHashCode());
        }
        else
        {
            obEdit = (Int64)intValue;
        }
    }

    //渲染UInt64类型
    private void RenderUInt64(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        if (RenderIntAsEnum(ref obEdit, obParent, fi, strUICaption, iIndentLevel))
        {
            return;
        }

        int intValue = (int)(UInt64)obEdit;
        if (intValue != ConverNumberField(ref intValue, obParent, fi, strUICaption, iIndentLevel))
        {
            SetDirty(true);
            obEdit = (UInt64)intValue;
            AddDirtyObject(obEdit.GetHashCode());
        }
        else
        {
            obEdit = (UInt64)intValue;
        }
    }

    //渲染SByte类型
    private void RenderSByte(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        int intValue = (int)(SByte)obEdit;
        if (intValue != ConverNumberField(ref intValue, obParent, fi, strUICaption, iIndentLevel))
        {
            SetDirty(true);
            obEdit = (SByte)intValue;
            AddDirtyObject(obEdit.GetHashCode());
        }
        else
        {
            obEdit = (SByte)intValue;
        }
    }

    //渲染Byte类型
    private void RenderByte(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        int intValue = (int)(Byte)obEdit;
        if (intValue != ConverNumberField(ref intValue, obParent, fi, strUICaption, iIndentLevel))
        {
            SetDirty(true);
            obEdit = (Byte)intValue;
            AddDirtyObject(obEdit.GetHashCode());
        }
        else
        {
            obEdit = (Byte)intValue;
        }
    }

    //渲染String类型
    private void RenderString(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        string strCap = strUICaption;
        string strValue = (string)obEdit;
        if (strValue == null)
        {
            strValue = "";
        }

        if (fi == null)
        {
            LogManager.LogError("String type must be member of class!");
        }
        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    EditorGUI.indentLevel = iIndentLevel;
                    if (fi != null)
                    {
                        strCap = GetFieldCaption(fi);
                    }
                    FileType eFileType = GetRelativeFileType(fi);
                    int iHashCode = obParent.GetHashCode() + fi.GetHashCode();

                    if (strValue != null && eFileType != FileType.OT_NONE)
                    {
                        //处理默认值，这种情况要先加载prefab
                        UnityEngine.Object testExistsOb = m_obGlobalState.Get(iHashCode);
                        if (testExistsOb == null)
                        {
                            testExistsOb = AssetDatabase.LoadMainAssetAtPath(strValue);
                            if (testExistsOb != null)
                            {
                                m_obGlobalState.Set(iHashCode, testExistsOb);
                            }
                            else
                            {
                                //LogManager.LogError("Error default resource not found:" + strValue);
                            }
                        }
                    }

                    ReadOnly readOnlyAttr = GetReadOnlyAttr(fi);

                    if (eFileType == FileType.OT_PREFABS)
                    {
                        #region FileType.OT_PREFABS
                        EditorGUILayout.PrefixLabel(strCap);
                        UnityEngine.Object ob = m_obGlobalState.Get(iHashCode);
                        UnityEngine.Object newOb = EditorGUILayout.ObjectField(ob, typeof(GameObject), false);
                        if (ob != newOb)
                        {
                            if (readOnlyAttr != null)
                            {
                                ob = newOb;
                                if (readOnlyAttr.ShowTip)
                                {
                                    ShowNotification("属性只读!");
                                }
                            }
                            else
                            {
                                m_obGlobalState.Set(iHashCode, newOb);
                                // Debuger.Log("Length of" + newOb.name + ":" + (newOb as GameObject).+ "s");
                                obEdit = AssetDatabase.GetAssetOrScenePath(newOb);
                                SetDirty(true);
                                AddDirtyObject(obEdit.GetHashCode());
                            }
                        }
                        #endregion
                    }
                    else if (eFileType == FileType.OT_AUDIO_CLIP)
                    {
                        #region FileType.OT_AUDIO_CLIP
                        EditorGUILayout.PrefixLabel(strCap);
                        UnityEngine.Object ob = m_obGlobalState.Get(iHashCode);
                        UnityEngine.Object newOb = EditorGUILayout.ObjectField(ob, typeof(AudioClip), false);
                        if (ob != newOb)
                        {
                            if (readOnlyAttr != null)
                            {
                                if (readOnlyAttr.ShowTip)
                                {
                                    ShowNotification("属性只读!");
                                }
                            }
                            else
                            {
                                m_obGlobalState.Set(iHashCode, newOb);
                                obEdit = AssetDatabase.GetAssetOrScenePath(newOb); ;
                                SetDirty(true);
                                AddDirtyObject(obEdit.GetHashCode());
                            }
                        }
                        #endregion
                    }
                    else if (eFileType == FileType.OT_PNG)
                    {
                        #region eFileType == FileType.OT_PNG
                        EditorGUILayout.PrefixLabel(strCap);
                        UnityEngine.Object ob = m_obGlobalState.Get(iHashCode);
                        UnityEngine.Object newOb = EditorGUILayout.ObjectField(ob, typeof(Texture), false);
                        if (ob != newOb)
                        {
                            if (readOnlyAttr != null)
                            {
                                if (readOnlyAttr.ShowTip)
                                {
                                    ShowNotification("属性只读!");
                                }
                            }
                            else
                            {
                                m_obGlobalState.Set(iHashCode, newOb);
                                obEdit = AssetDatabase.GetAssetOrScenePath(newOb); ;
                                SetDirty(true);
                                AddDirtyObject(obEdit.GetHashCode());
                            }
                        }
                        #endregion
                    }
                    else if (eFileType == FileType.OT_FMOD_EVENT)
                    {
                        //#region FileType.OT_FMOD_EVENT
                        //                            EditorGUILayout.PrefixLabel(strCap);
                        //                            UnityEngine.Object ob = m_obGlobalState.Get(iHashCode);
                        //                            UnityEngine.Object newOb = EditorGUILayout.ObjectField(ob, typeof(FMODAsset), false);
                        //                            if (ob != newOb)
                        //                            {
                        //                                if (readOnlyAttr != null)
                        //                                {
                        //                                    if (readOnlyAttr.ShowTip)
                        //                                    {
                        //                                        ShowNotification("属性只读!");
                        //                                    }
                        //                                }
                        //                                else
                        //                                {
                        //                                    m_obGlobalState.Set(iHashCode, newOb);
                        //                                    obEdit = AssetDatabase.GetAssetOrScenePath(newOb); ;
                        //                                    SetDirty(true);
                        //                                    AddDirtyObject(obEdit.GetHashCode());
                        //                                }
                        //                            }
                        //#endregion
                    }
                    else
                    {
                        #region Just Text
                        bool bUseTextArea = IsUseTextArea(fi);
                        string strNewValue;

                        if (bUseTextArea)
                        {
                            EditorGUILayout.PrefixLabel(strCap);
                            strNewValue = EditorGUILayout.TextArea(strValue);
                        }
                        else
                        {
                            strNewValue = EditorGUILayout.TextField(strCap, strValue);
                        }

                        if (strValue != strNewValue)
                        {
                            if (readOnlyAttr != null)
                            {
                                if (readOnlyAttr.ShowTip)
                                {
                                    ShowNotification("属性只读!");
                                }
                            }
                            else
                            {
                                SetDirty(true);
                                AddDirtyObject(obEdit.GetHashCode());
                                obEdit = strNewValue;
                            }
                        }
                        #endregion
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eString), 0, sizeof(eSerialType));
                    m_stream.Write(BitConverter.GetBytes(strValue.Length), 0, sizeof(int));
                    foreach (var c in strValue)
                    {
                        m_stream.Write(BitConverter.GetBytes(c), 0, sizeof(char));
                    }
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eString);
                    int iSize = out_stream.ReadInt();
                    string strRead = "";
                    for (int i = 0; i < iSize; i++)
                    {
                        strRead = strRead + out_stream.ReadChar();
                    }
                    obEdit = strRead;
#if UNITY_EDITOR
                    ////
                    FileType eFileType = GetRelativeFileType(fi);
                    int iHashCode = obParent.GetHashCode() + fi.GetHashCode();
                    if (eFileType == FileType.OT_PREFABS || eFileType == FileType.OT_AUDIO_CLIP || eFileType == FileType.OT_PNG || eFileType == FileType.OT_FMOD_EVENT)
                    {

                        UnityEngine.Object ob = GameObject.Find(strRead);

                        if (ob == null)
                        {
                            ob = AssetDatabase.LoadMainAssetAtPath(strRead);
                        }

                        ///兼容代码，这是为了转化老格式，后面去掉

                        if (ob == null)
                        {
                            ob = AssetDatabase.LoadMainAssetAtPath("Assets/Resources/Effect/Skill/" + strRead + ".prefab");
                            if (ob != null)
                            {
                                strRead = "Assets/Resources/Effect/Skill/" + strRead + ".prefab";
                                obEdit = strRead;
                            }
                            else
                            {
                                ob = AssetDatabase.LoadMainAssetAtPath("Assets/Resources/Media/Sound/" + strRead + ".ogg");
                                if (ob != null)
                                {
                                    strRead = "Assets/Resources/Media/Sound/" + strRead + ".ogg";
                                    obEdit = strRead;
                                }
                                else
                                {
                                    ob = AssetDatabase.LoadMainAssetAtPath("Assets/Resources/Media/Sound/" + strRead + ".wav");
                                    if (ob != null)
                                    {
                                        strRead = "Assets/Resources/Media/Sound/" + strRead + ".wav";
                                        obEdit = strRead;
                                    }
                                }
                            }
                        }

                        ///兼容代码end

                        if (ob == null)
                        {
                            //LogManager.LogError("Error:Cannot find resource " + strRead);
                        }
                        else
                        {
                            m_obGlobalState.Set(iHashCode, ob);
                        }
                    }
                    ///
#endif

                    break;
                }
        }
    }

    //渲染Vector2类型
    private void RenderVector2(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        Vector2 v = (Vector2)obEdit;
        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    EditorGUI.indentLevel = iIndentLevel;
                    string strCap = strUICaption;
                    if (fi != null)
                    {
                        strCap = GetFieldCaption(fi);
                    }
                    Vector2 vNew = EditorGUILayout.Vector2Field(strCap, v);
                    if (vNew != v)
                    {
                        ReadOnly readOnlyAttr = GetReadOnlyAttr(fi);
                        if (readOnlyAttr != null)
                        {
                            if (readOnlyAttr.ShowTip)
                            {
                                ShowNotification("属性只读！");
                            }
                        }
                        else
                        {
                            obEdit = vNew;
                            SetDirty(true);
                            AddDirtyObject(obEdit.GetHashCode());
                        }
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eVector2), 0, sizeof(int));
                    m_stream.Write(BitConverter.GetBytes(v.x), 0, sizeof(float));
                    m_stream.Write(BitConverter.GetBytes(v.y), 0, sizeof(float));
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eVector2);
                    Vector2 v2 = new Vector2(0, 0);
                    v2.x = out_stream.ReadFloat();
                    v2.y = out_stream.ReadFloat();
                    obEdit = v2;
                    break;
                }
        }
    }

    //渲染Vector3类型
    private void RenderVector3(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        Vector3 v = (Vector3)obEdit;
        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    EditorGUI.indentLevel = iIndentLevel;
                    string strCap = strUICaption;
                    if (fi != null)
                    {
                        strCap = GetFieldCaption(fi);
                    }
                    Vector3 vNew = EditorGUILayout.Vector3Field(strCap, v);
                    if (vNew != v)
                    {
                        ReadOnly readOnlyAttr = GetReadOnlyAttr(fi);
                        if (readOnlyAttr != null)
                        {
                            if (readOnlyAttr.ShowTip)
                            {
                                ShowNotification("属性只读！");
                            }
                        }
                        else
                        {

                            obEdit = vNew;
                            SetDirty(true);
                            AddDirtyObject(obEdit.GetHashCode());
                        }
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eVector3), 0, sizeof(int));
                    m_stream.Write(BitConverter.GetBytes(v.x), 0, sizeof(float));
                    m_stream.Write(BitConverter.GetBytes(v.y), 0, sizeof(float));
                    m_stream.Write(BitConverter.GetBytes(v.z), 0, sizeof(float));
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eVector3);
                    Vector3 v3 = new Vector3(0, 0, 0);
                    v3.x = out_stream.ReadFloat();
                    v3.y = out_stream.ReadFloat();
                    v3.z = out_stream.ReadFloat();
                    obEdit = v3;
                    break;
                }
        }
    }

    //渲染Vector4类型
    private void RenderVector4(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        Vector4 v = (Vector4)obEdit;
        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    EditorGUI.indentLevel = iIndentLevel;
                    string strCap = strUICaption;
                    if (fi != null)
                    {
                        strCap = GetFieldCaption(fi);
                    }
                    Vector4 vNew = EditorGUILayout.Vector4Field(strCap, v);
                    if (vNew != v)
                    {
                        ReadOnly readOnlyAttr = GetReadOnlyAttr(fi);
                        if (readOnlyAttr != null)
                        {
                            if (readOnlyAttr.ShowTip)
                            {
                                ShowNotification("属性只读！");
                            }
                        }
                        else
                        {
                            obEdit = vNew;
                            SetDirty(true);
                            AddDirtyObject(obEdit.GetHashCode());
                        }
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eVector4), 0, sizeof(int));
                    m_stream.Write(BitConverter.GetBytes(v.x), 0, sizeof(float));
                    m_stream.Write(BitConverter.GetBytes(v.y), 0, sizeof(float));
                    m_stream.Write(BitConverter.GetBytes(v.z), 0, sizeof(float));
                    m_stream.Write(BitConverter.GetBytes(v.w), 0, sizeof(float));
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eVector4);
                    Vector4 v4 = new Vector4(0, 0, 0, 0);
                    v4.x = out_stream.ReadFloat();
                    v4.y = out_stream.ReadFloat();
                    v4.z = out_stream.ReadFloat();
                    v4.w = out_stream.ReadFloat();
                    obEdit = v4;
                    break;
                }
        }
    }

    //渲染Color类型
    private void RenderColor(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        Color v = (Color)obEdit;
        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    EditorGUI.indentLevel = iIndentLevel;
                    string strCap = strUICaption;
                    if (fi != null)
                    {
                        strCap = GetFieldCaption(fi);
                    }
                    Color vNew = EditorGUILayout.ColorField(strCap, v);
                    if (vNew != v)
                    {
                        ReadOnly readOnlyAttr = GetReadOnlyAttr(fi);
                        if (readOnlyAttr != null)
                        {
                            if (readOnlyAttr.ShowTip)
                            {
                                ShowNotification("属性只读！");
                            }
                        }
                        else
                        {
                            obEdit = vNew;
                            SetDirty(true);
                            AddDirtyObject(obEdit.GetHashCode());
                        }
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eColor), 0, sizeof(int));
                    m_stream.Write(BitConverter.GetBytes(v.a), 0, sizeof(float));
                    m_stream.Write(BitConverter.GetBytes(v.r), 0, sizeof(float));
                    m_stream.Write(BitConverter.GetBytes(v.g), 0, sizeof(float));
                    m_stream.Write(BitConverter.GetBytes(v.b), 0, sizeof(float));
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eColor);
                    Color c = new Color(0, 0, 0, 0); ;
                    c.a = out_stream.ReadFloat();
                    c.r = out_stream.ReadFloat();
                    c.g = out_stream.ReadFloat();
                    c.b = out_stream.ReadFloat();
                    obEdit = c;
                    break;
                }
        }

    }

    //渲染ArrayList类型
    private void RenderArrayList(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        ArrayList ar = (ArrayList)obEdit;
        int iSize = ar.Count;

        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    int iHasCode = obEdit.GetHashCode();
                    bool bValue = IsExpand(iHasCode);

                    bool bFoldState = EditorGUILayout.Foldout(bValue, strUICaption + "[" + iSize + "]"); //这个size下帖再更新吧:)
                    bool bFixedArray = IsFieldArray(fi);

                    if (bFoldState)
                    {
                        for (int i = 0; i < iSize; i++)
                        {
                            object arElem = ar[i];
                            if (arElem == null)
                            {
                                LogManager.LogError("Error: " + strUICaption + "[" + i + "] is NULL.Stop Renderring it.");
                            }
                            else
                            {
                                RenderBuildInType(ref arElem, arElem.GetType(), obParent, fi, strUICaption + "[" + i + "]", iIndentLevel + 1);
                            }
                            ar[i] = arElem;
                        }
                    }

                    Collapse(iHasCode); //先把旧的删除了，有可能新的希哈值不一样

                    if (!bFoldState)
                    {
                        Collapse(obEdit.GetHashCode());
                    }
                    else
                    {
                        Expand(obEdit.GetHashCode());
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eArrayList), 0, sizeof(int));
                    m_stream.Write(BitConverter.GetBytes(iSize), 0, sizeof(int));
                    for (int i = 0; i < iSize; i++)
                    {
                        object arElem = ar[i];
                        if (arElem == null)
                        {
                            LogManager.LogError("Error: " + strUICaption + "[" + i + "] is NULL.Stop Renderring it.");
                        }
                        else
                        {
                            RenderBuildInType(ref arElem, arElem.GetType(), obParent, fi, strUICaption + "[" + i + "]", iIndentLevel + 1);
                        }
                        ar[i] = arElem;
                    }
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eArrayList);
                    iSize = out_stream.ReadInt();
                    for (int i = 0; i < iSize; i++)
                    {
                        object arElem = ar[i];
                        if (arElem == null)
                        {
                            LogManager.LogError("Error: " + strUICaption + "[" + i + "] is NULL.Stop Renderring it.");
                        }
                        else
                        {
                            RenderBuildInType(ref arElem, arElem.GetType(), obParent, fi, strUICaption + "[" + i + "]", iIndentLevel + 1);
                        }
                        ar[i] = arElem;
                    }

                    break;
                }
        }

    }

    //渲染AnimationCurve类型
    private void RenderAnimationCurve(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        AnimationCurve v = (AnimationCurve)obEdit;
        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    EditorGUI.indentLevel = iIndentLevel;
                    string strCap = strUICaption;
                    if (fi != null)
                    {
                        strCap = GetFieldCaption(fi);
                    }
                    AnimationCurve vNew = EditorGUILayout.CurveField(strCap, v);
                    if (vNew != v)
                    {
                        ReadOnly readOnlyAttr = GetReadOnlyAttr(fi);
                        if (readOnlyAttr != null)
                        {
                            if (readOnlyAttr.ShowTip)
                            {
                                ShowNotification("属性只读！");
                            }
                        }
                        else
                        {
                            obEdit = vNew;
                            SetDirty(true);
                            AddDirtyObject(obEdit.GetHashCode());
                        }
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eAnimationCurve), 0, sizeof(int));
                    //保存长度/属性
                    m_stream.Write(BitConverter.GetBytes(v.length), 0, sizeof(int));
                    m_stream.Write(BitConverter.GetBytes((int)v.postWrapMode), 0, sizeof(int));
                    m_stream.Write(BitConverter.GetBytes((int)v.preWrapMode), 0, sizeof(int));
                    //保存关键帧
                    for (int i = 0; i < v.length; i++)
                    {
                        Keyframe kf = v[i];
                        m_stream.Write(BitConverter.GetBytes((float)kf.inTangent), 0, sizeof(float));
                        m_stream.Write(BitConverter.GetBytes((float)kf.outTangent), 0, sizeof(float));
                        m_stream.Write(BitConverter.GetBytes((int)kf.tangentMode), 0, sizeof(int));
                        m_stream.Write(BitConverter.GetBytes((float)kf.time), 0, sizeof(float));
                        m_stream.Write(BitConverter.GetBytes((float)kf.value), 0, sizeof(float));

                    }
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eAnimationCurve);
                    //保存长度/属性
                    int iLen = out_stream.ReadInt();
                    v.postWrapMode = (WrapMode)out_stream.ReadInt();
                    v.preWrapMode = (WrapMode)out_stream.ReadInt();
                    //保存关键帧
                    for (int i = 0; i < iLen; i++)
                    {
                        Keyframe kf = new Keyframe();
                        kf.inTangent = out_stream.ReadFloat();
                        kf.outTangent = out_stream.ReadFloat();
                        kf.tangentMode = out_stream.ReadInt();
                        kf.time = out_stream.ReadFloat();
                        kf.value = out_stream.ReadFloat();
                        v.AddKey(kf);
                    }

                    break;
                }
        }
    }

    //渲染Bounds类型
    private void RenderBounds(ref object obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        Bounds v = (Bounds)obEdit;
        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    EditorGUI.indentLevel = iIndentLevel;
                    string strCap = strUICaption;
                    if (fi != null)
                    {
                        strCap = GetFieldCaption(fi);
                    }
                    Bounds vNew = EditorGUILayout.BoundsField(strCap, v);
                    if (vNew != v)
                    {
                        ReadOnly readOnlyAttr = GetReadOnlyAttr(fi);
                        if (readOnlyAttr != null)
                        {
                            if (readOnlyAttr.ShowTip)
                            {
                                ShowNotification("属性只读！");
                            }
                        }
                        else
                        {
                            obEdit = vNew;
                            SetDirty(true);
                            AddDirtyObject(obEdit.GetHashCode());
                        }
                    }
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eBounds), 0, sizeof(int));
                    object center = v.center;
                    object extents = v.extents;
                    object max = v.max;
                    object size = v.size;
                    object min = v.min;
                    RenderVector3(ref center, obEdit, null, null, iIndentLevel);
                    RenderVector3(ref extents, obEdit, null, null, iIndentLevel);
                    RenderVector3(ref max, obEdit, null, null, iIndentLevel);
                    RenderVector3(ref size, obEdit, null, null, iIndentLevel);
                    RenderVector3(ref min, obEdit, null, null, iIndentLevel);
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eBounds);
                    object center = null;
                    object extents = null;
                    object max = null;
                    object size = null;
                    object min = null;
                    RenderVector3(ref center, obEdit, null, null, iIndentLevel);
                    RenderVector3(ref extents, obEdit, null, null, iIndentLevel);
                    RenderVector3(ref max, obEdit, null, null, iIndentLevel);
                    RenderVector3(ref size, obEdit, null, null, iIndentLevel);
                    RenderVector3(ref min, obEdit, null, null, iIndentLevel);

                    v.center = (Vector3)center;
                    v.extents = (Vector3)extents;
                    v.max = (Vector3)max;
                    v.size = (Vector3)size;
                    v.min = (Vector3)min;

                    break;
                }
        }
    }
    

    /// <summary>
    /// 数据驱动界面
    /// </summary>
    /// <param name="obEdit">数据对象</param>
    /// <param name="t">数据类型</param>
    /// <param name="obParent">数据父</param>
    /// <param name="fi">文件信息</param>
    /// <param name="strUICaption">标题名</param>
    /// <param name="iIndentLevel">压缩率</param>
    private void RenderBuildInType(ref object obEdit, Type t, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        string strTypeName = null;
        if (obEdit == null)
        {
            //值类型
            strTypeName = t.Name;
        }
        else
        {
            strTypeName = obEdit.GetType().Name;
        }

        //非数组
        if (m_UiRenderDict.ContainsKey(strTypeName))
        {
            (m_UiRenderDict[strTypeName])(ref obEdit, obParent, fi, strUICaption, iIndentLevel);
        }
        else if (obEdit != null && obEdit.GetType().BaseType.ToString() == "System.Enum" || fi != null && fi.FieldType.BaseType.ToString() == "System.Enum")
        {
            (m_UiRenderDict["Enum"])(ref obEdit, obParent, fi, strUICaption, iIndentLevel);
        }
        else if (IsArray(obEdit, t))
        {
            (m_UiRenderDict["Self_Array"])(ref obEdit, obParent, fi, strUICaption, iIndentLevel);
        }
        else
        {
            (m_UiRenderDict["Self_Defined"])(ref obEdit, obParent, fi, strUICaption, iIndentLevel);
        }
    }

    //验证读取
    private void ValidateRead(eSerialType t)
    {

        eSerialType et = (eSerialType)(out_stream.ReadInt());

        if (et != t)
        {
            Exception e = new Exception("ValidateRead failed .It's not " + t.ToString() + ":" + et.ToString());
            throw e;
        }
    }

    //填充Number数据
    private int ConverNumberField(ref int obEdit, object obParent, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        Attribute attr = null;
        int int32Value = (int)obEdit;
        int int32ValueNew = int32Value;


        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    EditorGUI.indentLevel = iIndentLevel;
                    if (fi != null)
                    {
                        var attrs = System.Attribute.GetCustomAttributes(fi);

                        for (int i = 0; i < attrs.GetLength(0); i++)
                        {
                            if (attrs[i] is Slider)
                            {
                                attr = attrs[i];
                                break;
                            }
                        }
                    }
                    string strCap = strUICaption;
                    if (fi != null)
                    {
                        strCap = GetFieldCaption(fi);
                    }

                    if (attr != null)
                    {
                        EditorGUILayout.TagField(strCap);
                        int32ValueNew = (int)EditorGUILayout.Slider((float)int32Value, (attr as Slider).Min, (attr as Slider).Max);
                    }
                    else
                    {
                        int32ValueNew = EditorGUILayout.IntField(strCap, int32Value);
                    }

                    if (int32ValueNew != int32Value)
                    {
                        ReadOnly readOnlyAttr = GetReadOnlyAttr(fi);
                        if (readOnlyAttr != null)
                        {
                            int32ValueNew = int32Value;

                            if (readOnlyAttr.ShowTip)
                            {
                                ShowNotification("属性只读");
                            }
                        }
                    }

                    obEdit = int32ValueNew;
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eInt), 0, sizeof(eSerialType));
                    m_stream.Write(BitConverter.GetBytes(int32Value), 0, sizeof(int));
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eInt);
                    int32ValueNew = out_stream.ReadInt();
                    obEdit = int32ValueNew;
                }
                break;
        }


        return int32ValueNew;
    }

    //填充Float数据
    private float ConverFloatField(ref float obEdit, FieldInfo fi, string strUICaption, int iIndentLevel)
    {
        Attribute attr = null;
        float fValue = (float)obEdit;
        float fValueNew = fValue;

        switch (m_eWorkMode)
        {
            case eWorkMode.eSerialToUI:
                {
#if UNITY_EDITOR
                    EditorGUI.indentLevel = iIndentLevel;
                    if (fi != null)
                    {
                        var attrs = System.Attribute.GetCustomAttributes(fi);

                        for (int i = 0; i < attrs.GetLength(0); i++)
                        {
                            if (attrs[i] is Slider)
                            {
                                attr = attrs[i];
                                break;
                            }
                        }
                    }

                    string strCap = strUICaption;
                    if (fi != null)
                    {
                        strCap = GetFieldCaption(fi);
                    }

                    if (attr != null)
                    {
                        EditorGUILayout.LabelField(strCap);
                        fValueNew = EditorGUILayout.Slider(fValue, (attr as Slider).Min, (attr as Slider).Max);
                    }
                    else
                    {
                        fValueNew = EditorGUILayout.FloatField(strCap, fValue);
                    }

                    if (fValueNew != fValue)
                    {

                        ReadOnly readOnlyAttr = GetReadOnlyAttr(fi);
                        if (readOnlyAttr != null)
                        {
                            fValueNew = fValue;

                            if (readOnlyAttr.ShowTip)
                            {
                                ShowNotification("属性只读");
                            }
                        }
                    }

                    obEdit = fValueNew;
#endif
                    break;
                }
            case eWorkMode.eSerialToDisk:
                {
                    m_stream.Write(BitConverter.GetBytes((int)eSerialType.eFloat), 0, sizeof(eSerialType));
                    m_stream.Write(BitConverter.GetBytes(fValue), 0, sizeof(float));
                    break;
                }
            case eWorkMode.eSerialFromDisk:
                {
                    ValidateRead(eSerialType.eFloat);
                    fValueNew = out_stream.ReadFloat();
                    obEdit = fValueNew;
                    break;
                }
        }

        return fValueNew;
    }

    //获取缓存数组属性
    private CacheArray GetCacheArrayProperty(FieldInfo fi)
    {
        if (fi == null)
        {
            return null;
        }

        var attrs = System.Attribute.GetCustomAttributes(fi);
        CacheArray cs = null;
        for (int i = 0; i < attrs.GetLength(0); i++)
        {
            if (attrs[i] is CacheArray)
            {
                cs = (attrs[i] as CacheArray);
                return cs;
            }
        }

        return null;
    }

    private CacheSize GetCacheSizeProperty(FieldInfo fi, object ob)
    {
        if (fi == null)
        {
            return null;
        }

        var attrs = System.Attribute.GetCustomAttributes(fi);
        CacheSize cs = null;
        for (int i = 0; i < attrs.GetLength(0); i++)
        {
            if (attrs[i] is CacheSize)
            {
                cs = (attrs[i] as CacheSize);
                cs.Size = GetIntValue(ob);
                return cs;
            }
        }
        return null;
    }

    //ob 可能是16位，32位，或64位有符号和无符号整数,转化成int
    private int GetIntValue(object ob)
    {
        if (IsInt16(ob))
        {
            return (int)(Int16)ob;
        }

        if (IsUInt16(ob))
        {
            return (int)(UInt16)ob;
        }

        if (IsInt32(ob))
        {
            return (int)(Int32)ob;
        }

        if (IsUInt32(ob))
        {
            return (int)(UInt32)ob;
        }

        if (IsInt64(ob))
        {
            return (int)(Int64)ob;
        }

        if (IsUInt64(ob))
        {
            return (int)(UInt64)ob;
        }
        return 0;
    }

    private bool IsUseTextArea(FieldInfo fi)
    {

        if (fi == null)
        {
            return false;
        }

        var attrs = System.Attribute.GetCustomAttributes(fi);
        for (int i = 0; i < attrs.GetLength(0); i++)
        {
            if (attrs[i] is TextArea)
            {
                return true;
            }
        }

        return false;
    }
    #endregion


    #region 判断是否为某个类型
    //是否为基础类型
    private bool IsBaseType(string strTypeName)
    {
        if (!m_UIBaseTypeName.ContainsKey(strTypeName))
        {
            return false;
        }

        return m_UIBaseTypeName[strTypeName];
    }

    //是否为内置类型
    private bool IsBuiltInType(string strTypeName)
    {
        return m_UIBaseTypeName.ContainsKey(strTypeName);
    }

    //是否为数组
    private bool IsArray(object obEdit, Type t)
    {
        if (obEdit != null)
        {
            return GetArrayDepth(obEdit.GetType().Name) > 0;
        }
        else
        {
            return GetArrayDepth(t.Name) > 0;
        }
    }

    //是否为固定数组
    private bool IsFixedArray(FieldInfo f)
    {
        var attrs = System.Attribute.GetCustomAttributes(f);
        foreach (var attr in attrs)
        {
            if (attr is FixedArray)
            {
                return true;
            }
        }

        return false;
    }

    //是否为枚举
    private bool IsEnum(FieldInfo fi)
    {
        return fi.FieldType.BaseType.ToString() == "System.Enum";
    }

    //是否为Int32
    private bool IsInt32(object ob)
    {
        if (ob == null)
        {
            return false;
        }
        return ob.GetType().Name == "Int32";
    }

    //是否为UInt32
    private bool IsUInt32(object ob)
    {
        if (ob == null)
        {
            return false;
        }
        return ob.GetType().Name == "UInt32";
    }

    //是否为Int64
    private bool IsInt64(object ob)
    {
        if (ob == null)
        {
            return false;
        }
        return ob.GetType().Name == "Int64";
    }

    //是否为UInt64
    private bool IsUInt64(object ob)
    {
        if (ob == null)
        {
            return false;
        }
        return ob.GetType().Name == "UInt64";
    }

    //是否为Int16
    private bool IsInt16(object ob)
    {
        if (ob == null)
        {
            return false;
        }
        return ob.GetType().Name == "Int16";
    }

    //是否为UInt16
    private bool IsUInt16(object ob)
    {
        if (ob == null)
        {
            return false;
        }
        return ob.GetType().Name == "UInt16";
    }

    //是否为Int
    private bool IsInt(object ob)
    {
        return IsInt16(ob) || IsUInt16(ob) || IsUInt32(ob) || IsInt32(ob) || IsInt64(ob) || IsUInt64(ob);
    }

    //整数值是不是作为Enum值来显示？
    private IntEnum IsIntEnum(FieldInfo fi, object ob)
    {
        var attrs = System.Attribute.GetCustomAttributes(fi);
        foreach (var attr in attrs)
        {
            if (attr is IntEnum)
            {
                if (IsInt(ob))
                {
                    return attr as IntEnum;
                }
                else
                {
                    return null;
                }
            }
        }

        return null;
    }

    //是否显示为空数组
    private bool IsShowEmptyArray(FieldInfo f)
    {
        if (!IsFieldArray(f))
        {
            return true;
        }

        var attrs = System.Attribute.GetCustomAttributes(f);
        foreach (var attr in attrs)
        {
            if (attr is ShowEmptyArray)
            {
                return true;
            }
        }

        return false;
    }

    //是否为编辑器子数据
    private bool IsSubEditItem(object ob)
    {
        bool bEditInSubWindow = false;
        Type t = ob.GetType();
        var attrs = System.Attribute.GetCustomAttributes(t);
        int iAttrCount = attrs.GetLength(0);
        for (int i = 0; i < iAttrCount; i++)
        {
            if (attrs[i] is EditDetail)
            {
                bEditInSubWindow = true;
                break;
            }
        }

        return bEditInSubWindow;
    }

    //是否为编辑器子数据
    private bool IsSubEditItem(FieldInfo fi)
    {
        var attrs = System.Attribute.GetCustomAttributes(fi);
        bool bEditInSubWindow = false;
        int iAttrCount = attrs.GetLength(0);
        for (int i = 0; i < iAttrCount; i++)
        {
            if (attrs[i] is EditDetail)
            {
                bEditInSubWindow = true;
                break;
            }
        }

        if (!bEditInSubWindow)
        {
            Type t = fi.FieldType;
            attrs = System.Attribute.GetCustomAttributes(t);
            iAttrCount = attrs.GetLength(0);
            for (int i = 0; i < iAttrCount; i++)
            {
                if (attrs[i] is EditDetail)
                {
                    bEditInSubWindow = true;
                    break;
                }
            }
        }


        return bEditInSubWindow;
    }

    //是否为数组
    private bool IsFieldArray(FieldInfo fi)
    {

        if (fi.FieldType.BaseType.Name == "Array")
        {
            return true;
        }

        return false;
    }

    //是否为省略的
    private bool IsOmit(FieldInfo fi)
    {
        var attrs = System.Attribute.GetCustomAttributes(fi);
        int iAttrCount = attrs.GetLength(0);
        for (int i = 0; i < iAttrCount; i++)
        {
            if (attrs[i] is Omit)
            {
                return true;
            }
        }

        return false;
    }

    //是否为省略的
    private bool IsEditable(FieldInfo fi)
    {
        var attrs = System.Attribute.GetCustomAttributes(fi);
        int iAttrCount = attrs.GetLength(0);
        for (int i = 0; i < iAttrCount; i++)
        {
            if (attrs[i] is Omit)
            {
                return false;
            }
        }

        return true;
    }

    //判断是几维数组
    private int GetArrayDepth(string strTypeName)
    {
        string[] tmp = strTypeName.Split('[');
        return tmp.GetLength(0) - 1;
    }

    //获取关联的文件类型
    private FileType GetRelativeFileType(FieldInfo fi)
    {

        if (fi == null)
        {
            return FileType.OT_NONE;
        }

        var attrs = System.Attribute.GetCustomAttributes(fi);
        for (int i = 0; i < attrs.GetLength(0); i++)
        {
            if (attrs[i] is SelectFile)
            {
                return (attrs[i] as SelectFile).Value;
            }
        }

        return FileType.OT_NONE;
    }

    #endregion

    #region Built-in Class Defined 存储数据
    //记录需要使用枚举来编辑的整数
    class EnumIntDictionary
    {
        public void Set(int id, Type t)
        {
            if (m_enumType.ContainsKey(id))
            {
                m_enumType[id] = t;
            }
            else
            {
                m_enumType.Add(id, t);
            }
        }

        public Type Get(int id)
        {
            if (m_enumType.ContainsKey(id))
            {
                return m_enumType[id];

            }
            else
            {
                return null;
            }

        }

        public bool Del(int id)
        {
            if (m_enumType.ContainsKey(id))
            {
                m_enumType.Remove(id);
                return true;
            }

            return false;
        }

        public bool Exit(int id)
        {
            return m_enumType.ContainsKey(id);
        }

        private Dictionary<int, Type> m_enumType = new Dictionary<int, Type>();
    }

    //用于存储带有CacheSize属性的值
    class CacheDictionary
    {
        public void Set(int id, int size)
        {
            if (m_CacheSize.ContainsKey(id))
            {
                m_CacheSize[id] = size;
            }
            else
            {
                m_CacheSize.Add(id, size);
            }
        }

        public int Get(int id)
        {
            if (m_CacheSize.ContainsKey(id))
            {
                return m_CacheSize[id];

            }
            else
            {
                return 0;
            }

        }

        public bool Del(int id)
        {
            if (m_CacheSize.ContainsKey(id))
            {
                m_CacheSize.Remove(id);
                return true;
            }

            return false;
        }

        public bool Exit(int id)
        {
            return m_CacheSize.ContainsKey(id);
        }

        private Dictionary<int, int> m_CacheSize = new Dictionary<int, int>();
    }

    //UI状态存储
    class UIStateCenter
    {
        public bool Get(int iHasCode)
        {
            if (m_Dict.ContainsKey(iHasCode))
            {
                if (m_Dict[iHasCode] == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                m_Dict.Add(iHasCode, 1);
                return true;
            }

        }

        public void Remove(int iHasKey)
        {
            m_Dict.Remove(iHasKey);
        }

        public int GetInt(int iHasCode)
        {
            if (m_Dict.ContainsKey(iHasCode))
            {
                return m_Dict[iHasCode];
            }
            else
            {
                m_Dict.Add(iHasCode, 0);
                return 0;
            }

        }

        public void Set(int iHasCode, int iState)
        {
            if (m_Dict.ContainsKey(iHasCode))
            {
                m_Dict[iHasCode] = iState;
            }
            else
            {
                m_Dict.Add(iHasCode, iState);
            }
        }

        public bool Revert(int iHasCode)
        {
            if (m_Dict.ContainsKey(iHasCode))
            {
                if (m_Dict[iHasCode] == 0)
                {
                    m_Dict[iHasCode] = 1;
                    return true;
                }
                else
                {
                    m_Dict[iHasCode] = 0;
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        private Dictionary<int, int> m_Dict = new Dictionary<int, int>();
    }

    //选择状态记录
    class ObjectSelectCenter
    {
        public UnityEngine.Object Get(int iHasCode)
        {
            if (m_Dict.ContainsKey(iHasCode))
            {
                return m_Dict[iHasCode];

            }
            else
            {
                return null;
            }

        }

        public void Remove(int iHasKey)
        {
            m_Dict.Remove(iHasKey);
        }


        public void Set(int iHasCode, UnityEngine.Object ob)
        {
            if (m_Dict.ContainsKey(iHasCode))
            {
                m_Dict[iHasCode] = ob;
            }
            else
            {
                m_Dict.Add(iHasCode, ob);
            }
        }


        private Dictionary<int, UnityEngine.Object> m_Dict = new Dictionary<int, UnityEngine.Object>();
    }
    #endregion
}
