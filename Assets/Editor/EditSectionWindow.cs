//------------------------------------------------------------------------------
// Section编辑窗口
//------------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;



public class EditSectionWindow : EditorWindow
{
    private static EditSectionWindow window = null;
    private DataEdit m_DataEdit = new DataEdit(0);
    private object m_Data = null;
    private bool m_bConfirm = false;
    private bool m_bCancel = false;

    public object SecData
    {
        get { return m_Data; }
        set { m_Data = value; }
    }


    public static bool Edit(ref object editOb , string strCaption)
    {
        if (window == null)
        {
            Rect wr = new Rect(0, 0, 400, 300);
            window = (EditSectionWindow)EditorWindow.GetWindowWithRect(typeof(EditSectionWindow), wr, true, strCaption);
            window.Show(true);
        }
        window.SecData = editOb;

        if (window.UpdateIfChanged(ref  editOb))
        {
            window.Close();
            return true;
        }

        return false;
    }

    public bool UpdateIfChanged(ref object data)
    {
        if (m_bCancel)
        {
            return true;
        }

        if (m_bConfirm)
        {
            data = m_Data;
            return true;
        }

        return false;
    }

    private int EDIT_AREA_WIDTH = 300;
    private int EDIT_AREA_HEIGHT = 200;

    void OnDestroy()
    {
        m_bCancel = true;
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
        Rect rtEdit = new Rect((position.width - EDIT_AREA_WIDTH) / 2, (position.height - EDIT_AREA_HEIGHT - 50) / 2, EDIT_AREA_WIDTH, EDIT_AREA_HEIGHT);
        GUILayout.BeginArea(rtEdit);
        GUILayout.BeginVertical();
        if (m_Data != null)
        {
            m_DataEdit.EditInUI(m_Data, m_Data.GetType(), null, "阶段详细信息", 0, null);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
        Rect rectBtnCreate = new Rect(position.width / 2 - 100, EDIT_AREA_HEIGHT, 60f, 30);
        if (GUI.Button(rectBtnCreate, "修改"))
        {
            m_bConfirm = true;
        }

        Rect rectBtnCancel = new Rect(position.width / 2 + 40, rectBtnCreate.y, rectBtnCreate.width, rectBtnCreate.height);
        if (GUI.Button(rectBtnCancel, "取消"))
        {
            m_bCancel = true;
        }

        // reset editor styles
        EditorStyles.textField.normal = styleEditorTextField.normal;
        EditorStyles.textField.focused = styleEditorTextField.focused;
        EditorStyles.label.normal = styleEditorLabel.normal;
    }
}