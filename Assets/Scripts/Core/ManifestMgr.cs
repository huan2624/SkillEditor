/* **************************************************
 * 工程名:#PROJECTNAME#
 * 文件名:#FILENAME#
 * 作  者:#AuthorName#
 * 日  期:#CreateTime#
 * 描  述:
 * **************************************************/

using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class ManifestMgr : TSingleton<ManifestMgr>
{
    /// <summary>
    /// apk或ipa包中的AssetBundleManifest文件（Application.streamingAssetsPath文件夹中）
    /// </summary>
    private AssetBundleManifest m_OldManifest;
    /// <summary>
    /// 更新过去的AssetBundleManifest文件（如果没有更新过，则不存在，Application.persistentDataPath文件夹中）
    /// </summary>
    private AssetBundleManifest m_NewManifest;


    public void Init()
    {
        bool useAssetBundle = true;
#if UNITY_EDITOR
        //useAssetBundle = ResManager.USE_ASSETBUNDLE;
#endif
        if (useAssetBundle)
        {
            string oldFilePath = Path.Combine(PathBuilder.StreamingAssetsPath, "StreamingAssets");
            AssetBundle oldManifestBundle = AssetBundle.LoadFromFile(oldFilePath);
            if (oldManifestBundle != null)
            {
                m_OldManifest = oldManifestBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                oldManifestBundle.Unload(false);
            }

            string newFilePath = Path.Combine(Application.persistentDataPath, "StreamingAssets");
            if (File.Exists(newFilePath))
            {
                AssetBundle newManifestBundle = AssetBundle.LoadFromFile(newFilePath);
                if (oldManifestBundle != null)
                {
                    m_NewManifest = oldManifestBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                    oldManifestBundle.Unload(false);
                }
            }
        }
    }

    /// <summary>
    /// 此文件是否有更新过
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool IsFileUpdated(string path)
    {
        bool updated = false;
        if (m_NewManifest != null)
        {
            Hash128 newHash = m_NewManifest.GetAssetBundleHash(path);
            if (newHash.isValid)
            {
                Hash128 oldHash = m_OldManifest.GetAssetBundleHash(path);
                if (oldHash.isValid)
                {
                    updated = !newHash.Equals(oldHash);
                }
                else
                {
                    updated = true;
                }
            }
        }
        return updated;
    }
    
    /// <summary>
    /// 获取所有依赖文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string[] GetAllDependencies(string path)
    {
        path = path.Replace(Path.GetExtension(path), ".unity3d");
        path = path.ToLower();
        if (IsFileUpdated(path))
        {
            string[] arr = m_NewManifest.GetAllDependencies(path);
            return arr;
        }
        else
        {
            if (m_OldManifest != null)
            {
                string[] arr = m_OldManifest.GetAllDependencies(path);
                return arr;
            }
            else
            {
                return new string[0];
            }
        }
    }
}
