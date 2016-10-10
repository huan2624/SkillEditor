//------------------------------------------------------------------------------
// 路径及界面style风格管理
//------------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class SDE_Options : ScriptableObject {
	
	// options
	public string skin = "am_skin_dark";
	
	public bool setSkin(string _skin) {
		if(skin != _skin) {
			skin = _skin;
			return true;
		}
		return false;
	}

    public static string GetSkillDispFilePath()
    {
        return UnityEngine.Application.dataPath + "/Resources/";
    }

    //获取文件路径
    public static string FormatFileName(string strFileName, FileNameType t)
    {
        if (t == FileNameType.NONE)
        {
            return strFileName;
        }

        if (t == FileNameType.RESOURCE_NAME)
        {
            bool bOk = false;
            string strResourcePath = "Resources/";
            int index = strFileName.IndexOf(strResourcePath);
            if (index >= 0)
            {
                strFileName = strFileName.Substring(strResourcePath.Length + index);
                index = strFileName.LastIndexOf('.');
                if (index >= 0)
                {
                    strFileName = strFileName.Substring(0, index);
                    bOk = true;
                }
            }

            if (!bOk)
            {
                strResourcePath = "Resources\\";
                index = strFileName.IndexOf(strResourcePath);
                if (index >= 0)
                {
                    strFileName = strFileName.Substring(strResourcePath.Length + index);
                    index = strFileName.LastIndexOf('.');
                    if (index >= 0)
                    {
                        strFileName = strFileName.Substring(0, index);
                        bOk = true;
                    }
                }
            }
        }

        strFileName = strFileName.Replace('\\', '/');

        return strFileName;

    }



    // load file
    public static string fileName = "sde_options.asset";
	
	public static string getFilePath() {
		// search for directory
		string[] directories = Directory.GetDirectories(Application.dataPath, "SkillDispEditor", SearchOption.AllDirectories);
		string valid_directory = null;
		foreach(string directory in directories) {
            // validate as SkillDispEditor directory by checking if directory contains Files/Editor folder
			if(!Directory.Exists(directory+"/Files/Editor")) continue;
			
			valid_directory = "Assets"+directory.Substring(Application.dataPath.Length).Replace("\\","/")+"/Resources";
			if(!Directory.Exists(directory+"/Resources")) {
				Directory.CreateDirectory(directory+"/Resources");
			}
			break;
		}		
		if(valid_directory == null) {
			return null;
		}
		return valid_directory +"/"+fileName;
	}
	
	public static SDE_Options loadFile() {
		SDE_Options load_file = (SDE_Options) Resources.Load("sde_options");
		if(load_file) {
			return load_file;
		}

        return ScriptableObject.CreateInstance<SDE_Options>();
	}
	
	public static void export() {
		string newPath = EditorUtility.SaveFilePanel("Export Options", Application.dataPath, "sde_options_export", "unitypackage");
		if(newPath.Length == 0) return;
		string filePath = getFilePath();
		if(filePath == null) {
			Debug.LogWarning("SkillDisp Editor: Export failed. Options file not found.");
			return;
		}
		AssetDatabase.ExportPackage(filePath, newPath,ExportPackageOptions.Interactive);
	}
}
 