//------------------------------------------------------------------------------
// 技能编辑需要使用到的各种枚举及Attribute
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System;

#region Omit Attribute 省略的
//对界面不可用、隐藏
[System.AttributeUsage(System.AttributeTargets.Enum | System.AttributeTargets.Field,
               AllowMultiple = true)]
public class Omit : System.Attribute
{
    public Omit()
    {
    }
}
#endregion

#region StaticCaption Attribute 静态说明文字
[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Field,
               AllowMultiple = true)]
public class StaticCaption : System.Attribute
{
    public StaticCaption(string strCaption)
    {
        m_strCaption = strCaption;
    }

    public string Value
    {
        get { return m_strCaption; }
        set { m_strCaption = value; }
    }

    private string m_strCaption;
}
#endregion

#region SelectFile Attribute 选择文件类型
public enum FileType
{
    OT_NONE,
    OT_PREFABS,
    OT_MONO_SCRIPT,
    OT_AUDIO_CLIP,
    OT_PNG,
    OT_FMOD_EVENT
}


[System.AttributeUsage(System.AttributeTargets.Field,
               AllowMultiple = true)]
public class SelectFile : System.Attribute
{
    public SelectFile(FileType type)
    {
        Value = type;
    }

    public FileType Value
    {
        get { return m_Type; }
        set { m_Type = value; }
    }

    private FileType m_Type;
}
#endregion

#region TextArea Attribute 文本框
[System.AttributeUsage(System.AttributeTargets.Field,
           AllowMultiple = true)]
public class TextArea : System.Attribute
{
    public TextArea()
    {
    }

}
#endregion

#region FixedArray Attribute 固定数组
[System.AttributeUsage(System.AttributeTargets.Field,
           AllowMultiple = true)]
public class FixedArray : System.Attribute
{
    public FixedArray()
    {
    }
}
#endregion

#region Slider Attribute 滑块
//数值使用Slider来控制
[System.AttributeUsage(System.AttributeTargets.Field,
       AllowMultiple = true)]
public class Slider : System.Attribute
{
    public Slider(float fMin, float fMax)
    {
        Max = fMax;
        Min = fMin;
    }

    private float m_fMin = 0;
    private float m_fMax = 0;

    public float Min
    {
        get { return m_fMin; }
        set { m_fMin = value; }
    }

    public float Max
    {
        get { return m_fMax; }
        set { m_fMax = value; }
    }
}

#endregion



#region ShowEmptyArray Attribute 空数组
[System.AttributeUsage(System.AttributeTargets.Class |
               System.AttributeTargets.Struct | System.AttributeTargets.Field,
               AllowMultiple = true)]
public class ShowEmptyArray : System.Attribute
{
    public ShowEmptyArray()
    {
    }

}

#endregion

#region EnumMutex Attribute 互斥
[System.AttributeUsage(System.AttributeTargets.Field,
           AllowMultiple = true)]
public class EnumMutex : System.Attribute
{
    //@param id :               表示一个互斥组ID
    //@param iValue             互斥值
    //@param iDefaultValue      互斥产生时变成默认值
    public EnumMutex(int id, int iValue, int iDefaultValue)
    {
        m_id = id;
        m_iValue = iValue;
        m_iDefaultValue = iDefaultValue;
    }
    private int m_id;
    private int m_iValue;
    private int m_iDefaultValue;

    public int Id
    {
        get { return m_id; }
    }

    public int Value
    {
        get { return m_iValue; }
    }

    public int DefaultValue
    {
        get { return m_iDefaultValue; }
    }
}
#endregion
