
public static class StringHelper
{
    public static int ToInt(this string str)
    {
        return int.Parse(str);
    }
    public static long ToLong(this string str)
    {
        return long.Parse(str);
    }
    public static float ToFloat(this string str)
    {
        return float.Parse(str);
    }
    public static bool ToBool(this string str)
    {
        if (str == "0")
        {
            return false;
        }
        return true;
    }
}
