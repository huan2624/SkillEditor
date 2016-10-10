//角色身上各种挂点
[System.Serializable]
public enum DummyPoint
{
    DM_NONE = 0,   // 无
    DM_HEAD = 1,    // 头顶
    DM_BREAST = 2,    // 胸部
    DM_L_HAND = 3,   // 左手
    DM_R_HAND = 4,   // 右手
    DM_L_FOOT = 5,   // 左脚
    DM_R_FOOT = 6,   // 右脚
    DM_ROOT = 7,   // 脚底中心
    DM_AVATAR_CLSP_A = 8,
    DM_AVATAR_CLSP_B = 9,
    DM_WEAPON_CLSP_A = 10,
    DM_WEAPON_CLSP_B = 11,
    DM_WEAPON_CLSP_C = 12,
    DM_WEAPON_CLSP_D = 13,
    DM_BIP = 14,  // 腰椎
}

public class AvatarDefine
{
    public static string GetDummyPointName(DummyPoint dm_point)
    {
        string strDummyPointName = "";
        switch (dm_point)
        {
            case DummyPoint.DM_HEAD:    // 头顶
                strDummyPointName = "DM_HEAD";
                break;

            case DummyPoint.DM_BREAST:    // 胸部
                strDummyPointName = "DM_BREAST";
                break;

            case DummyPoint.DM_L_HAND:   // 左手
                strDummyPointName = "DM_L_HAND";
                break;

            case DummyPoint.DM_R_HAND:   // 右手
                strDummyPointName = "DM_R_HAND";
                break;

            case DummyPoint.DM_L_FOOT:   // 左脚
                strDummyPointName = "DM_L_FOOT";
                break;

            case DummyPoint.DM_R_FOOT:   // 右脚
                strDummyPointName = "DM_R_FOOT";
                break;

            case DummyPoint.DM_ROOT:    // 脚底中心
                strDummyPointName = "DM_ROOT";
                break;

            case DummyPoint.DM_AVATAR_CLSP_A:
                strDummyPointName = "DM_AVATAR_CLSP_A";
                break;

            case DummyPoint.DM_AVATAR_CLSP_B:
                strDummyPointName = "DM_AVATAR_CLSP_B";
                break;

            case DummyPoint.DM_WEAPON_CLSP_A:
                strDummyPointName = "DM_WEAPON_CLSP_A";
                break;

            case DummyPoint.DM_WEAPON_CLSP_B:
                strDummyPointName = "DM_WEAPON_CLSP_B";
                break;

            case DummyPoint.DM_WEAPON_CLSP_C:
                strDummyPointName = "DM_WEAPON_CLSP_C";
                break;

            case DummyPoint.DM_WEAPON_CLSP_D:
                strDummyPointName = "DM_WEAPON_CLSP_D";
                break;
            case DummyPoint.DM_BIP:
                strDummyPointName = "DM_BIP";
                break;
            default:
                break;
        }

        return strDummyPointName;
    }

    public static DummyPoint GetDummyPointIdByName(string strDummyPointName)
    {
        if (string.IsNullOrEmpty(strDummyPointName))
        {
            return DummyPoint.DM_NONE;
        }
        else if (strDummyPointName == "DM_HEAD")
        {
            return DummyPoint.DM_HEAD;
        }
        else if (strDummyPointName == "DM_BREAST")
        {
            return DummyPoint.DM_BREAST;
        }
        else if (strDummyPointName == "DM_L_HAND")
        {
            return DummyPoint.DM_L_HAND;
        }
        else if (strDummyPointName == "DM_R_HAND")
        {
            return DummyPoint.DM_R_HAND;
        }
        else if (strDummyPointName == "DM_L_FOOT")
        {
            return DummyPoint.DM_L_FOOT;
        }
        else if (strDummyPointName == "DM_R_FOOT")
        {
            return DummyPoint.DM_R_FOOT;
        }
        else if (strDummyPointName == "DM_ROOT")
        {
            return DummyPoint.DM_ROOT;
        }
        else if (strDummyPointName == "DM_AVATAR_CLSP_A")
        {
            return DummyPoint.DM_AVATAR_CLSP_A;
        }
        else if (strDummyPointName == "DM_AVATAR_CLSP_B")
        {
            return DummyPoint.DM_AVATAR_CLSP_B;
        }
        else if (strDummyPointName == "DM_WEAPON_CLSP_A")
        {
            return DummyPoint.DM_WEAPON_CLSP_A;
        }
        else if (strDummyPointName == "DM_WEAPON_CLSP_B")
        {
            return DummyPoint.DM_WEAPON_CLSP_B;
        }
        else if (strDummyPointName == "DM_WEAPON_CLSP_C")
        {
            return DummyPoint.DM_WEAPON_CLSP_C;
        }
        else if (strDummyPointName == "DM_WEAPON_CLSP_D")
        {
            return DummyPoint.DM_WEAPON_CLSP_D;
        }
        else if (strDummyPointName == "DM_BIP")
        {
            return DummyPoint.DM_BIP;
        }

        return DummyPoint.DM_NONE;
    }
}