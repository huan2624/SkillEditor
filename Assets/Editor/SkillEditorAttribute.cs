#region EditDetai Attribute 编辑器数据
using System;

[System.AttributeUsage(System.AttributeTargets.Class |
               System.AttributeTargets.Struct | System.AttributeTargets.Field,
               AllowMultiple = true)]
public class EditDetail : System.Attribute
{
    public EditDetail()
    {
    }

    public void SetEditor(DataEdit ed)
    {
        m_ed = ed;
    }

    public DataEdit GetEditor()
    {
        return m_ed;
    }
    private DataEdit m_ed;
}
#endregion

#region ReadOnly Attribute 是不是只读
//用于表明自定义类型是不是使用按钮作为打开/折起的控件
[System.AttributeUsage(System.AttributeTargets.Field)]
class ReadOnly : System.Attribute
{
    public ReadOnly(bool bShowTipe)
    {
        m_bShowTip = bShowTipe;
    }
    private bool m_bShowTip;
    public bool ShowTip
    {
        get { return m_bShowTip; }
    }
}
#endregion

#region CacheArray Attribute 缓存数组
[System.AttributeUsage(System.AttributeTargets.Field,
                AllowMultiple = true)]
//用于修饰数组(Array)的属性，数组的可用大小由带CacheSize属性的成员值指定
class CacheArray : System.Attribute
{
    public CacheArray(int id)
    {
        m_id = id;
    }
    private int m_id = 0;
    private int m_iSize = 0;
    public int Id
    {
        get { return m_id; }
    }
    public int Size
    {
        get { return m_iSize; }
        set { m_iSize = value; }
    }
}
#endregion

#region CacheSize Attribute 缓存大小
[System.AttributeUsage(System.AttributeTargets.Field,
            AllowMultiple = true)]
//指示这是一个指定数组大小值的成员，指定带有CacheArray属性的数组的可用大小
class CacheSize : System.Attribute
{
    public CacheSize(int id)
    {
        m_id = id;
    }
    private int m_id = 0;
    private int m_iSize = 0;
    public int Id
    {
        get { return m_id; }
    }

    public int Size
    {
        get { return m_iSize; }
        set { m_iSize = value; }
    }

}
#endregion

#region IntEnum Attribute 类型
class IntEnum : System.Attribute
{
    public IntEnum(Type emType)
    {
        m_emType = emType;
    }
    private Type m_emType;
    public Type EmType
    {
        get { return m_emType; }
    }
}
#endregion

#region ButtonFoldout Attribute 用于表明自定义类型是不是使用按钮作为打开/折起的控件
//用于表明自定义类型是不是使用按钮作为打开/折起的控件
class ButtonFoldout : System.Attribute
{
    public ButtonFoldout()
    {
    }
}
#endregion
