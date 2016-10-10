using UnityEngine;
using System.Collections;

public class PathBuilder
{

    public static string StreamingAssetsPath
    {
        get
        {
#if UNITY_EDITOR
            return Application.streamingAssetsPath;
#elif UNITY_ANDROID
            return Application.dataPath + "!assets/";
#else
            return Application.streamingAssetsPath;
#endif
        }
    }

    public static string WWW_Local_File_Path(string url)
    {
        if (url.StartsWith("file://"))
            return url;
        return "file://" + url;
    }


}
