using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class NewBehaviourScript : MonoBehaviour {
    MeshFilter meshFilter;
    public GameObject cube;
    // Use this for initialization
    void Start () {
        meshFilter = GetComponent<MeshFilter>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            int Section_Num = 20;
            float angle = 180 / ((float)Section_Num);
            List<FanSection> sections = new List<FanSection>();
            float startAngle = angle * (int)(Section_Num / 2);
            float endAngle = -angle * (int)(Section_Num / 2);
            for (int i = Section_Num / 2; i >= -Section_Num / 2; i--)
            {
                Debug.Log("222");
                Vector3 dirSection = Quaternion.Euler(0, angle * i, 0) * Vector3.forward;

                Vector3 posIn = new Vector3(0, 0.01f, 0) + dirSection * 1;
                Vector3 posOut = new Vector3(0, 0.01f, 0) + dirSection * 5;

                sections.Add(new FanSection(posIn, posOut));

                //if (i == Section_Num / 2 - 1)
                //{
                //    Debug.Log("123 = " + startAngle);
                //    float tempangle = 120 / angle;
                //    Vector3 dirSection = Quaternion.Euler(0, startAngle - 2, 0) * Vector3.forward;

                //    Vector3 posIn = new Vector3(0, 0.01f, 0) + dirSection * 2;
                //    Vector3 posOut = new Vector3(0, 0.01f, 0) + dirSection * 5;

                //    sections.Add(new FanSection(posIn, posOut));
                //}
                //else if (i == 1-Section_Num / 2)
                //{
                //    Debug.Log("456 = " + endAngle);
                //    float tempangle = 120 / angle;
                //    Vector3 dirSection = Quaternion.Euler(0, endAngle + 2, 0) * Vector3.forward;

                //    Vector3 posIn = new Vector3(0, 0.01f, 0) + dirSection * 2;
                //    Vector3 posOut = new Vector3(0, 0.01f, 0) + dirSection * 5;

                //    sections.Add(new FanSection(posIn, posOut));
                //}
                //else
                //{
                //    Debug.Log("222");
                //    float tempangle = 120 / angle;
                //    Vector3 dirSection = Quaternion.Euler(0, angle * i, 0) * Vector3.forward;

                //    Vector3 posIn = new Vector3(0, 0.01f, 0) + dirSection * 2;
                //    Vector3 posOut = new Vector3(0, 0.01f, 0) + dirSection * 5;

                //    sections.Add(new FanSection(posIn, posOut));
                //}

            }
            CFanMesh m_FanMesh = new CFanMesh();
            m_FanMesh.BuildFan(cube, ref sections);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            LoadHero();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            LoadSkill();
        }
    }

    private void LoadHero()
    {
        HeroBasicAttributesTable table = new HeroBasicAttributesTable();
        table.Init();

        Dictionary<uint, wl_res.HeroBasicAttributes> dic = table.GetTable();

        sb = new StringBuilder();
        foreach (var item in dic)
        {
            wl_res.HeroBasicAttributes info = item.Value;

            sb.Append(item.Key + ",");
            sb.Append(info.heroID + ",");
            sb.Append(info.ResId + ",");
            sb.Append(CoverArrayToString(info.SkillId) + ",");
            sb.Append(CoverArrayToString(info.NormalAttackId) + ",");
            sb.Append(UTF8BytesToString(ref info.Name) + "\r\n");
        }

        string toPath = Application.dataPath + "/hero.csv";
        FileStream fs = new FileStream(toPath, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
        //开始写入
        sw.Write(sb.ToString());
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
        Debug.Log("写完了");
    }

    private void LoadSkill()
    {
        SkillDataInfoTable table = new SkillDataInfoTable();
        table.Init();

        Dictionary<uint, wl_res.SkillDataInfo> dic = table.GetTable();

        sb = new StringBuilder();
        foreach (var item in dic)
        {
            wl_res.SkillDataInfo info = item.Value;

            sb.Append(item.Key + ",");
            sb.Append(info.SkillID + ",");
            sb.Append(info.SkillDispID + ",");
            sb.Append(UTF8BytesToString(ref info.szSkillName) + "\r\n");
        }

        string toPath = Application.dataPath + "/skill.csv";
        FileStream fs = new FileStream(toPath, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
        //开始写入
        sw.Write(sb.ToString());
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
        Debug.Log("写完了");
    }

    private string CoverArrayToString(uint[] arr)
    {
        string str = "";
        foreach (var item in arr)
        {
            str += item + "|";
        }
        return str;
    }

    private StringBuilder sb;

    /// <summary>
    /// 将一个UTF8编码格式的byte数组，转换为一个String
    /// </summary>
    /// <param name="str">编码格式的数组</param>
    /// <returns>转换的string</returns>
    static public string UTF8BytesToString(ref byte[] str)
    {
        if (str == null)
            return null;

        //为了让string的长度正确，
        byte[] tempStr = new byte[strlen(str)];
        System.Buffer.BlockCopy(str, 0, tempStr, 0, tempStr.Length);
        return System.Text.Encoding.UTF8.GetString(tempStr);
    }

    /// <summary>
    /// 功能同c语言中的strlen
    /// </summary>
    /// <param name="str">输入的bytes</param>
    /// <returns>输入bytes的string长度</returns>
    static public int strlen(byte[] str)
    {
        if (str == null)
            return 0;

        byte nullChar = 0x00;
        int count = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (nullChar == str[i])
            {
                break;
            }

            count++;
        }

        return count;
    }
}
