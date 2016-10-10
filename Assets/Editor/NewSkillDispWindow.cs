using UnityEngine;
//------------------------------------------------------------------------------
// 创建新技能窗口
//------------------------------------------------------------------------------
using System.Collections;
using UnityEditor;
using System.Text.RegularExpressions;

public class NewSkillDispWindow : EditorWindow
{
    private static NewSkillDispWindow window = null;
    private string m_strNewSkillDispId = "";
    private string m_strTipsMessage = "";
    public string NewSkillDispId
    {
        get { return m_strNewSkillDispId; }
    }

    public static void AddWindow()
    {
        Rect wr = new Rect(0, 0, 300, 150);
        window = (NewSkillDispWindow)EditorWindow.GetWindowWithRect(typeof(NewSkillDispWindow), wr, true, "创建一个新的技能");
        window.Show();
    }

    void OnGUI()
    {

        // backup editor styles
        GUIStyle styleEditorTextField = new GUIStyle(EditorStyles.textField);
        GUIStyle styleEditorLabel = new GUIStyle(EditorStyles.label);
        // modify editor styles
        EditorStyles.textField.normal = GUI.skin.textField.normal;
        EditorStyles.textField.focused = GUI.skin.textField.focused;
        EditorStyles.label.normal = GUI.skin.label.normal;

        GUILayout.BeginVertical();

        GUILayout.Label("输入技能ID，不可和原来的重复:");

        m_strNewSkillDispId = GUILayout.TextField(m_strNewSkillDispId, 10);

        if (!string.IsNullOrEmpty(m_strNewSkillDispId))
        {
            m_strNewSkillDispId = Regex.Replace(m_strNewSkillDispId, "[^0-9]", "");
        }

        Rect rectBtnCreate = new Rect(position.width / 2 - 100, 100, 60f, 30);
        if (GUI.Button(rectBtnCreate, "创建") && !string.IsNullOrEmpty(m_strNewSkillDispId))
        {
            string strDispFile = SDE_Options.GetSkillDispFilePath() + m_strNewSkillDispId + ".bytes";
            if (System.IO.File.Exists(strDispFile))
            {
                m_strTipsMessage = "SkillDisp File " + strDispFile + " Exists!";
            }
            else
            {
                SkillWindowEditor.CurrentEditor.CreateSkill(int.Parse(m_strNewSkillDispId));
                window.Close();
            }
        }

        Rect rectBtnCancel = new Rect(position.width / 2 + 40, rectBtnCreate.y, rectBtnCreate.width, rectBtnCreate.height);
        if (GUI.Button(rectBtnCancel, "取消"))
        {
            m_strNewSkillDispId = "";
            window.Close();
        }

        if (!string.IsNullOrEmpty(m_strTipsMessage))
        {
            EditorGUILayout.HelpBox(m_strTipsMessage, MessageType.Error);
        }

        GUILayout.EndVertical();

        // reset editor styles
        EditorStyles.textField.normal = styleEditorTextField.normal;
        EditorStyles.textField.focused = styleEditorTextField.focused;
        EditorStyles.label.normal = styleEditorLabel.normal;
    }
}
