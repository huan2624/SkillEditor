using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

public class SkillDataReader
{
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

    private delegate void UIHandler(ref object obEdit);
    private Dictionary<string, UIHandler> m_UiRenderDict = new Dictionary<string, UIHandler>();

    MemoryStreamUtil out_stream;

    public SkillDataReader()
    {
        RegisterUIRenders();
    }

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

    //创建类型实例
    private object CreateInstance(Type t)
    {
        if (t.Name == "String")
        {
            return (string)"";
        }
        return Assembly.GetAssembly(t).CreateInstance(t.FullName);
    }

    private byte[] LoadFile(string strFile)
    {
        TextAsset asset = Resources.Load(strFile) as TextAsset;
        return asset.bytes;
    }

    public SkillDispData LoadSkillData(string strFile)
    {
        Debug.Log("没缓存");
        Type t = typeof(SkillDispData);
        object data = CreateInstance(t);

        byte[] bytes = LoadFile(strFile);
        using (out_stream = new MemoryStreamUtil(bytes))
        {
            ReadObject(ref data, t);
        }
        out_stream = null;
        return data as SkillDispData;
    }

    private void ReadObject(ref object ob, Type t)
    {
        if (ob == null)
        {
            if (!InitType(ref ob, t))
            {
                Debug.LogError("Cannot create instance for " + t.Name + ")");
            }
        }
        ReadDataByType(ref ob, t);
    }

    private void ReadDataByType(ref object obEdit, Type t)
    {
        if (t == null)
        {
            Debug.LogError("t Cannot be null");
            return;
        }
        string strTypeName = t.Name;

        //非数组
        if (m_UiRenderDict.ContainsKey(strTypeName))
        {
            (m_UiRenderDict[strTypeName])(ref obEdit);
            
        }
        else if (obEdit != null && t.BaseType.ToString() == "System.Enum")
        {
            (m_UiRenderDict["Enum"])(ref obEdit);
        }
        else if (IsArray(obEdit, t))
        {
            (m_UiRenderDict["Self_Array"])(ref obEdit);
        }
        else
        {
            (m_UiRenderDict["Self_Defined"])(ref obEdit);
        }
    }

    //Built-in 类型（已有类型的支持）
    private void RegisterUIRenders()
    {
        //基本类型
        RegisterUIHandler("SByte", new UIHandler(ReadSByte), true); //有符号8位数
        RegisterUIHandler("Byte", new UIHandler(ReadByte), true);//无符号8位数
        RegisterUIHandler("Int16", new UIHandler(ReadInt16), true);//有符号16位
        RegisterUIHandler("UInt16", new UIHandler(ReadUInt16), true);//无符号16位
        RegisterUIHandler("Int32", new UIHandler(ReadInt32), true);//int
        RegisterUIHandler("UInt32", new UIHandler(ReadUInt32), true);//uint
        RegisterUIHandler("Int64", new UIHandler(ReadInt64), true);//int64
        RegisterUIHandler("UInt64", new UIHandler(ReadUInt64), true);//uint64
        RegisterUIHandler("String", new UIHandler(ReadString), true);//String
        RegisterUIHandler("Enum", new UIHandler(ReadEnum), true);//枚举
        RegisterUIHandler("Boolean", new UIHandler(ReadBool), true);//bool
        RegisterUIHandler("Single", new UIHandler(ReadFloat), true);//float
        RegisterUIHandler("Double", new UIHandler(ReadDouble), true);//double
        RegisterUIHandler("Decimal", new UIHandler(ReadDecimal), true);//float

        //常见类型
        RegisterUIHandler("Vector2", new UIHandler(ReadVector2), false);//Vector2
        RegisterUIHandler("Vector3", new UIHandler(ReadVector3), false);//Vector3
        RegisterUIHandler("Vector4", new UIHandler(ReadVector4), false);//Vector4
        RegisterUIHandler("Color", new UIHandler(ReadColor), false); //Color
        RegisterUIHandler("Bounds", new UIHandler(ReadBounds), false); //Color
        RegisterUIHandler("ArrayList", new UIHandler(ReadArrayList), false); //Color
        RegisterUIHandler("AnimationCurve", new UIHandler(ReadAnimationCurve), false); //AnimationCurve

        //自定义class
        RegisterUIHandler("Self_Defined", new UIHandler(ReadUserDefined), false); //自定义的Class
        RegisterUIHandler("Self_Array", new UIHandler(ReadUserDefinedArray), false); //任意类型声明的Class或struct
    }

    //注册类型
    private void RegisterUIHandler(string strTypeName, UIHandler dMethod, bool bBaseType)
    {
        if (m_UiRenderDict.ContainsKey(strTypeName))
        {
            m_UiRenderDict[strTypeName] += dMethod;
        }
        else
        {
            m_UiRenderDict.Add(strTypeName, dMethod);
        }
    }

    #region 读取各种数据
    //获取int数据
    private int ConverNumberField()
    {
        ValidateRead(eSerialType.eInt);
        int int32Value = out_stream.ReadInt();
        return int32Value;
    }

    //获取float数据
    private float ConverFloatField()
    {
        ValidateRead(eSerialType.eFloat);
        float fValue = out_stream.ReadFloat();
        return fValue;
    }

    private void ReadSByte(ref object obEdit)
    {
        int value = ConverNumberField();
        obEdit = (SByte)value;
    }

    private void ReadByte(ref object obEdit)
    {
        int value = ConverNumberField();
        obEdit = (Byte)value;
    }

    private void ReadInt16(ref object obEdit)
    {
        int value = ConverNumberField();
        obEdit = (Int16)value;
    }

    private void ReadUInt16(ref object obEdit)
    {
        int value = ConverNumberField();
        obEdit = (UInt16)value;
    }

    private void ReadInt32(ref object obEdit)
    {
        int value = ConverNumberField();
        obEdit = (Int32)value;
    }

    private void ReadUInt32(ref object obEdit)
    {
        int value = ConverNumberField();
        obEdit = (UInt32)value;
    }

    private void ReadInt64(ref object obEdit)
    {
        int value = ConverNumberField();
        obEdit = (Int64)value;
    }

    private void ReadUInt64(ref object obEdit)
    {
        int value = ConverNumberField();
        obEdit = (UInt64)value;
    }

    private void ReadString(ref object obEdit)
    {
        ValidateRead(eSerialType.eString);
        int iSize = out_stream.ReadInt();
        string strRead = "";
        for (int i = 0; i < iSize; i++)
        {
            strRead = strRead + out_stream.ReadChar();
        }
        obEdit = strRead;
    }

    private void ReadEnum(ref object obEdit)
    {
        ValidateRead(eSerialType.eEnum);
        obEdit = out_stream.ReadInt();
    }

    private void ReadBool(ref object obEdit)
    {
        ValidateRead(eSerialType.eBoolean);
        obEdit = out_stream.ReadBool();
    }

    private void ReadFloat(ref object obEdit)
    {
        ValidateRead(eSerialType.eFloat);
        obEdit = out_stream.ReadFloat();
    }

    private void ReadDouble(ref object obEdit)
    {
        obEdit = (double)ConverFloatField();
    }

    private void ReadDecimal(ref object obEdit)
    {
        obEdit = (decimal)ConverFloatField();
    }

    private void ReadVector2(ref object obEdit)
    {
        ValidateRead(eSerialType.eVector2);
        Vector2 v2 = new Vector2(0, 0);
        v2.x = out_stream.ReadFloat();
        v2.y = out_stream.ReadFloat();
        obEdit = v2;
    }

    private void ReadVector3(ref object obEdit)
    {
        ValidateRead(eSerialType.eVector3);
        Vector3 v3 = new Vector3(0, 0, 0);
        v3.x = out_stream.ReadFloat();
        v3.y = out_stream.ReadFloat();
        v3.z = out_stream.ReadFloat();
        obEdit = v3;
    }

    private void ReadVector4(ref object obEdit)
    {
        ValidateRead(eSerialType.eVector4);
        Vector4 v4 = new Vector4(0, 0, 0, 0);
        v4.x = out_stream.ReadFloat();
        v4.y = out_stream.ReadFloat();
        v4.z = out_stream.ReadFloat();
        v4.w = out_stream.ReadFloat();
        obEdit = v4;
    }

    private void ReadColor(ref object obEdit)
    {
        ValidateRead(eSerialType.eColor);
        Color c = new Color(0, 0, 0, 0); ;
        c.a = out_stream.ReadFloat();
        c.r = out_stream.ReadFloat();
        c.g = out_stream.ReadFloat();
        c.b = out_stream.ReadFloat();
        obEdit = c;
    }

    private void ReadBounds(ref object obEdit)
    {
        Bounds v = (Bounds)obEdit;
        ValidateRead(eSerialType.eBounds);
        object center = null;
        object extents = null;
        object max = null;
        object size = null;
        object min = null;
        ReadVector3(ref center);
        ReadVector3(ref extents);
        ReadVector3(ref max);
        ReadVector3(ref size);
        ReadVector3(ref min);

        v.center = (Vector3)center;
        v.extents = (Vector3)extents;
        v.max = (Vector3)max;
        v.size = (Vector3)size;
        v.min = (Vector3)min;
    }

    private void ReadArrayList(ref object obEdit)
    {
        ArrayList ar = (ArrayList)obEdit;
        ValidateRead(eSerialType.eArrayList);
        int iSize = out_stream.ReadInt();
        for (int i = 0; i < iSize; i++)
        {
            object arElem = ar[i];
            if (arElem == null)
            {
                Debuger.LogError("Error: [" + i + "] is NULL.Stop Renderring it.");
            }
            else
            {
                ReadObject(ref arElem, arElem.GetType());
            }
            ar[i] = arElem;
        }
    }

    private void ReadAnimationCurve(ref object obEdit)
    {
        AnimationCurve v = (AnimationCurve)obEdit;
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
    }

    private void ReadUserDefined(ref object obEdit)
    {
        ValidateRead(eSerialType.eSelf_Defined);
        Type obType = obEdit.GetType();
        var f_list = obType.GetFields();

        for (int i = 0; i < f_list.GetLength(0); i++)
        {
            object ob = f_list[i].GetValue(obEdit);
            ReadObject(ref ob, f_list[i].FieldType);
            f_list[i].SetValue(obEdit, ob);
        }
    }

    private void ReadUserDefinedArray(ref object obEdit)
    {
        Array ar = obEdit as Array;
        ValidateRead(eSerialType.eSelf_Array);
        int iSize = out_stream.ReadInt();
        Resize(ref ar, ar.GetType().GetElementType(), ar.GetLength(0), iSize);
        for (int i = 0; i < iSize; i++)
        {
            object arElem = ar.GetValue(i);
            ReadObject(ref arElem, arElem.GetType());
            ar.SetValue(arElem, i);
        }
        obEdit = ar as object;
    }
    #endregion

    //设置数组尺寸
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
    
    //判断是否是数组
    private bool IsArray(object obEdit, Type t)
    {
        return GetArrayDepth(t.Name) > 0;
    }

    //判断是几维数组
    private int GetArrayDepth(string strTypeName)
    {
        string[] tmp = strTypeName.Split('[');
        return tmp.GetLength(0) - 1;
    }

    //验证读取类型是否正确，与保存时的类型相对应
    private void ValidateRead(eSerialType t)
    {
        eSerialType et = (eSerialType)(out_stream.ReadInt());
        if (et != t)
        {
            Exception e = new Exception("ValidateRead failed .It's not " + t.ToString() + ":" + et.ToString());
            throw e;
        }
    }
}
