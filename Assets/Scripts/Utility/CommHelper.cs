using UnityEngine;
using System.Collections;

public class CommHelper {

    static uint ms_uiGOID = 0x10000000;

    public static uint GenerateGOID()
    {
        ++ms_uiGOID;
        return ms_uiGOID;
    }

    public static string GetPureFileNameWithouExt(string strFileName)
    {
        int nNameLen = strFileName.Length;
        int nExtDotPos = nNameLen;
        for (int i = nNameLen - 1; i >= 0; i--)
        {
            var ch = strFileName[i];
            if (nExtDotPos == nNameLen && ch == '.')
            {
                nExtDotPos = i;
            }

            if (ch == '/')
            {
                return strFileName.Substring(i + 1, nExtDotPos - i - 1);
            }
        }

        if (nExtDotPos < nNameLen)
        {
            return strFileName.Substring(0, nExtDotPos);
        }
        else
        {
            return strFileName;
        }
    }
}
