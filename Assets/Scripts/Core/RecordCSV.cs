using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;
using System.Text.RegularExpressions;


// type support
// int , uint , string , float , bool , short , ushort ,double , byte sbyte
// 不支持 char ,如果是字符请用 string , 如果是 int8 请用 sbyte
public static class RecordCSV
{

    public static bool SetField(object obj, FieldInfo fi, string data)
    {
        if (null == fi || null == data)
        {
            return false;
        }
        if( "" == data )
        {
            return true; // 默认值可不填
        }
        bool ret = false;
        if (fi.FieldType == typeof(int))
        {
            int val = 0;
            ret = int.TryParse(data, out val);
            fi.SetValue(obj, val );
        }
        else if (fi.FieldType == typeof(uint))
        {
            uint val = 0;
            ret = uint.TryParse(data, out val);
            fi.SetValue(obj, val);
        }
        else if (fi.FieldType == typeof(string))
        {
            fi.SetValue(obj, data);
            ret = true;
        }
        else if (fi.FieldType == typeof(float))
        {
            float val = 0;
            ret = float.TryParse(data, out val);
            fi.SetValue(obj, val);
        }
        else if (fi.FieldType == typeof(bool))
        {
            bool val = false;
            if (!bool.TryParse(data, out val))
            {
                int val2 = 0;
                ret = int.TryParse(data, out val2);
                val = (0 != val2);
                fi.SetValue(obj, val);
            }
            else
            {
                ret = true;
                fi.SetValue(obj, val);
            }
        }
        else if (fi.FieldType == typeof(short))
        {
            short val = 0;
            ret = short.TryParse(data, out val);
            fi.SetValue(obj, val);
        }
        else if (fi.FieldType == typeof(ushort))
        {
            ushort val = 0;
            ret = ushort.TryParse(data, out val);
            fi.SetValue(obj, val);
        }
        else if (fi.FieldType == typeof(double))
        {
            double val = 0;
            ret = double.TryParse(data, out val);
            fi.SetValue(obj, val);
        }
        else if (fi.FieldType == typeof(byte))
        {
            byte val = 0;
            ret = byte.TryParse(data, out val);
            fi.SetValue(obj, val);
        }
        else if (fi.FieldType == typeof(sbyte))
        {
            sbyte val = 0;
            ret = sbyte.TryParse(data, out val);
            fi.SetValue(obj, val);
        }
        return ret;
    }

    public static string GetField(object obj, FieldInfo fi)
    {
        if (null == fi)
        {
            return "";
        }

        if (fi.FieldType == typeof(int))
        {
            int val = (int)fi.GetValue(obj);
            return val.ToString();
        }
        else if (fi.FieldType == typeof(uint))
        {
            uint val = (uint)fi.GetValue(obj);
            return val.ToString();
        }
        else if (fi.FieldType == typeof(string))
        {
            string val = (string)fi.GetValue(obj);
            return val;
        }
        else if (fi.FieldType == typeof(float))
        {
            float val = (float)fi.GetValue(obj);
            return val.ToString();
        }
        else if (fi.FieldType == typeof(bool))
        {
            bool val = (bool)fi.GetValue(obj);
            return (val ? "1" : "0");
        }
        else if (fi.FieldType == typeof(short))
        {
            short val = (short)fi.GetValue(obj);
            return val.ToString();
        }
        else if (fi.FieldType == typeof(ushort))
        {
            ushort val = (ushort)fi.GetValue(obj);
            return val.ToString();
        }
        else if (fi.FieldType == typeof(double))
        {
            double val = (double)fi.GetValue(obj);
            return val.ToString();
        }
        else if (fi.FieldType == typeof(byte))
        {
            byte val = (byte)fi.GetValue(obj);
            return val.ToString();
        }
        else if (fi.FieldType == typeof(sbyte))
        {
            sbyte val = (sbyte)fi.GetValue(obj);
            return val.ToString();
        }
        else
            return "";
    }

    public static string ReadFromCsvFile0<TRECORD>(string path, List<TRECORD> listRecord , bool fieldNumMatch ) where TRECORD : new()
    {
        string ret = "";
        FileStream fs = null;
        try
        {
            fs = new FileStream(path, FileMode.Open);
            if (null == fs)
            {
                ret = " file open failed , path := " + path;
                return ret;
            }
            StreamReader reader = new StreamReader(fs);
            string header = reader.ReadLine();
            char[] sp = { ',' };
            string[] fields = header.Split(sp, System.StringSplitOptions.RemoveEmptyEntries);
            if (0 == fields.Length)
            {
                ret = " no field , path := " + path;
                reader.Close();
                fs.Close();
                return ret;
            }
            System.Type tp_record = typeof(TRECORD);
            int fieldCount = tp_record.GetFields().Length;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] records = line.Split(sp);
                if (records.Length != fields.Length)
                {
                    if (0 != records.Length)
                    {
                        Debug.Log("data format error , line := " + line);
                    }
                    if (fieldNumMatch && (records.Length < fields.Length / 2))
                    {
                        continue;  //csv 进行版本兼容,不强制字段数量匹配
                    }
                }
                if (records[0].Contains("//"))
                {
                    continue;
                }
                try
                {
                    TRECORD record = new TRECORD();
                    uint ok_count = 0;
                    for (int i = 0; i < fields.Length; ++i)
                    {
                        if (i >= records.Length) break;
                        if (i >= records.Length) break;
                        string str = Regex.Replace(fields[i], @"\t", "").Trim();
                        str = Regex.Replace(str, "\"", "");
                        string val = Regex.Replace(records[i], "\t", "");
                        val = Regex.Replace(val, "\"", "");
                        //if (SetField(record, tp_record.GetField(fields[i]), records[i]))
                        if (SetField(record, tp_record.GetField(str), val))
                        {
                            ok_count += 1;
                        }
                    }
                    if (fieldNumMatch && (ok_count < fieldCount * 0.75f))  // 数量匹配有误
                    {
                        continue;
                    }
                    listRecord.Add(record);
                }
                catch (System.Exception ex)
                {
                    Debug.Log("exception := " + ex.Message);
                }
            }
            reader.Close();
            fs.Close();
            return ret;
        }
        catch (System.Exception ex)
        {
            ret = "exception := " + ex.Message;
        }
        finally
        {
            if (null != fs)
            {
                fs.Close();
                fs = null;
            }
        }
        return ret;
    }

    public static string ReadFromCsvFile0_gb2312<TRECORD>(string path, List<TRECORD> listRecord, bool fieldNumMatch) where TRECORD : new()
    {
        string ret = "";
        FileStream fs = null;
        try
        {
            fs = new FileStream(path, FileMode.Open);
            if (null == fs)
            {
                ret = " file open failed , path := " + path;
                return ret;
            }
            byte[] fileContent = new byte[fs.Length];
            if (fs.Length != fs.Read(fileContent, 0, (int)fs.Length))
            {

                ret = " file read failed , path := " + path;
                return ret;
            }
            fs.Close();
            fs = null;
            System.Text.Encoding gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf8Content = System.Text.Encoding.Convert(gb2312, System.Text.Encoding.UTF8, fileContent);
            MemoryStream ms = new MemoryStream(utf8Content, false);
            StreamReader reader = new StreamReader(ms);
            string header = reader.ReadLine();
            char[] sp = { ',' };
            string[] fields = header.Split(sp, System.StringSplitOptions.RemoveEmptyEntries);
            if (0 == fields.Length)
            {
                ret = " no field , path := " + path;
                reader.Close();
                fs.Close();
                return ret;
            }
            System.Type tp_record = typeof(TRECORD);
            int fieldCount = tp_record.GetFields().Length;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] records = line.Split(sp);
                if (records.Length != fields.Length)
                {
                    if (0 != records.Length)
                    {
                        Debug.Log("data format error , line := " + line);
                    }
                    if (fieldNumMatch && (records.Length < fields.Length / 2))
                    {
                        continue;  //csv 进行版本兼容,不强制字段数量匹配
                    }
                }
                if (records[0].Contains("//"))
                {
                    continue;
                }
                try
                {
                    TRECORD record = new TRECORD();
                    uint ok_count = 0;
                    for (int i = 0; i < fields.Length; ++i)
                    {
                        if (i >= records.Length) break;
                        string str = Regex.Replace(fields[i], @"\t", "").Trim();
                        str = Regex.Replace(str, "\"", "");
                        string val = Regex.Replace(records[i], "\t", "");
                        val = Regex.Replace(val, "\"", "");
                        //if (SetField(record, tp_record.GetField(fields[i]), records[i]))
                        if (SetField(record, tp_record.GetField(str), val))
                        {
                            ok_count += 1;
                        }
                    }
                    if (0 == ok_count) continue;
                    if (fieldNumMatch && (ok_count < fieldCount * 0.75f))  // 数量匹配有误
                    {
                        continue;
                    }
                    listRecord.Add(record);
                }
                catch (System.Exception ex)
                {
                    Debug.Log("exception := " + ex.Message);
                }
            }
            reader.Close();
        }
        catch (System.Exception ex)
        {
            ret = "exception := " + ex.Message;
        }
        finally
        {
            if (null != fs)
            {
                fs.Close();
                fs = null;
            }
        }
        return ret;
    }

    public static string ReadFromCsvFileFromResources<TKEY, TRECORD>(string path, Dictionary<TKEY, TRECORD> dictRecord, string keyName, bool fieldNumMatch) where TRECORD : new()
    {
        string ret = "";
        TextAsset asset = null;
        try
        {
            asset = Resources.Load<TextAsset>(path);
            string[] SplitLine = new string[] { "\n", "\r" };
            string[] Lines = asset.text.Split(SplitLine, StringSplitOptions.RemoveEmptyEntries);

            char[] sp = { ',' };
            string[] fields = Lines[0].Split(sp, System.StringSplitOptions.RemoveEmptyEntries);//数据表列名
            if (0 == fields.Length)
            {
                ret = " no field , path := " + path;
                return ret;
            }
            System.Type tp_record = typeof(TRECORD);
            int fieldCount = tp_record.GetFields().Length;
            FieldInfo fieldKey = tp_record.GetField(keyName);
            for(int k = 1; k < Lines.Length; k++)
            {
                string[] records = Lines[k].Split(sp);//数据表中一行数据
                if (records.Length != fields.Length)
                {
                    if (0 != records.Length)
                    {
                        //Debug.Log("data format error , line := " + Lines[k]);
                    }
                    if (fieldNumMatch && (records.Length < fields.Length / 2))
                    {
                        continue;  //csv 进行版本兼容,不强制字段数量匹配
                    }
                }
                //if (records[0].Contains("//"))
                if (String.Compare(records[0].Substring(0, 1), "0") == 0)  //  排除注释行
                {
                    continue;
                }
                try
                {
                    TRECORD record = new TRECORD();
                    uint ok_count = 0;
                    for (int i = 0; i < fields.Length; ++i)
                    {
                        if (i >= records.Length) break;
                        string str = Regex.Replace(fields[i], @"\t", "").Trim();
                        str = Regex.Replace(str, "\"", "");
                        string val = Regex.Replace(records[i], "\t", "");
                        val = Regex.Replace(val, "\"", "");
                        if (SetField(record, tp_record.GetField(str), val))
                        {
                            ok_count += 1;
                        }
                        else
                        {
                            if (fields[i] == keyName)
                            {
                                ok_count = 0;
                                break;
                            }
                        }
                    }
                    if (0 == ok_count) continue;
                    if (fieldNumMatch && (ok_count < fieldCount * 0.75f))  // 数量匹配有误
                    {
                        continue;
                    }
                    dictRecord.Add((TKEY)fieldKey.GetValue(record), record);
                }
                catch (System.Exception ex)
                {
                    Debug.Log("exception := " + ex.Message);
                }
            }
        }
        catch (System.Exception ex)
        {
            ret = "exception := " + ex.Message;
        }
        return ret;
    }

    public static string ReadFromCsvFile<TKEY, TRECORD>(string path, Dictionary<TKEY, TRECORD> dictRecord, string keyName, bool fieldNumMatch) where TRECORD : new()
    {
        string ret = "";
        FileStream fs = null;
        try
        {
            fs = new FileStream(path, FileMode.Open);
            if (null == fs)
            {
                ret = " file open failed , path := " + path;
                return ret;
            }
            StreamReader reader = new StreamReader(fs);
            string header = reader.ReadLine();
            char[] sp = { ',' };
            string[] fields = header.Split(sp, System.StringSplitOptions.RemoveEmptyEntries);
            if (0 == fields.Length)
            {
                ret = " no field , path := " + path;
                reader.Close();
                fs.Close();
                return ret;
            }
            System.Type tp_record = typeof(TRECORD);
            int fieldCount = tp_record.GetFields().Length;
            FieldInfo fieldKey = tp_record.GetField(keyName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] records = line.Split(sp);
                if (records.Length != fields.Length)
                {
                    if (0 != records.Length)
                    {
                        Debug.Log("data format error , line := " + line);
                    }
                    if (fieldNumMatch && (records.Length < fields.Length / 2))
                    {
                        continue;  //csv 进行版本兼容,不强制字段数量匹配
                    }
                }
                if( records[0].Contains( "//" ) )
                {
                    continue;
                }
                try
                {
                    TRECORD record = new TRECORD();
                    uint ok_count = 0;
                    for (int i = 0; i < fields.Length; ++i)
                    {
                        if (i >= records.Length) break;
                        if( SetField(record, tp_record.GetField(fields[i]), records[i]) )
                        {
                            ok_count += 1;
                        }
                        else
                        {
                            if( fields[i] == keyName )
                            {
                                ok_count = 0;
                                break;
                            }
                        }
                    }
                    if (0 == ok_count) continue;
                    if (fieldNumMatch && (ok_count < fieldCount * 0.75f))  // 数量匹配有误
                    {
                        continue;
                    }
                    dictRecord.Add((TKEY)fieldKey.GetValue(record), record);
                }
                catch (System.Exception ex)
                {
                    Debug.Log("exception := " + ex.Message);
                }
            }
            reader.Close();
            fs.Close();
            return ret;
        }
        catch (System.Exception ex)
        {
            ret = "exception := " + ex.Message;
        }
        finally
        {
            if (null != fs)
            {
                fs.Close();
                fs = null;
            }
        }
        return ret;
    }

    // read gb2312
    public static string ReadFromCsvFile_gb2312<TKEY,TRECORD>(string path, Dictionary<TKEY,TRECORD> dictRecord, string keyName, bool fieldNumMatch) where TRECORD : new()
    {
        string ret = "";
        FileStream fs = null;
        try
        {
            fs = new FileStream(path, FileMode.Open);
            if (null == fs)
            {
                ret = " file open failed , path := " + path;
                return ret;
            }
            byte[] fileContent = new byte[fs.Length];
            if( fs.Length != fs.Read(fileContent, 0, (int)fs.Length) )
            {

                ret = " file read failed , path := " + path;
                return ret;
            }
            fs.Close();
            fs = null;
            System.Text.Encoding gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf8Content = System.Text.Encoding.Convert(gb2312, System.Text.Encoding.UTF8, fileContent);
            MemoryStream ms = new MemoryStream(utf8Content, false);
            StreamReader reader = new StreamReader(ms);
            string header = reader.ReadLine();
            char[] sp = { ',' };
            string[] fields = header.Split(sp, System.StringSplitOptions.RemoveEmptyEntries);
            if (0 == fields.Length)
            {
                ret = " no field , path := " + path;
                reader.Close();
                fs.Close();
                return ret;
            }
            System.Type tp_record = typeof(TRECORD);
            int fieldCount = tp_record.GetFields().Length;
            FieldInfo fieldKey = tp_record.GetField(keyName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] records = line.Split(sp);
                if (records.Length != fields.Length)
                {
                    if (0 != records.Length)
                    {
                        Debug.Log("data format error , line := " + line);
                    }
                    if (fieldNumMatch && (records.Length < fields.Length / 2))
                    {
                        continue;  //csv 进行版本兼容,不强制字段数量匹配
                    }
                }
                if (records[0].Contains("//"))
                {
                    continue;
                }
                try
                {
                    TRECORD record = new TRECORD();
                    uint ok_count = 0;
                    for (int i = 0; i < fields.Length; ++i)
                    {
                        if (i >= records.Length) break;
                        if( SetField(record, tp_record.GetField(fields[i]), records[i]) )
                        {
                            ok_count += 1;
                        }
                        else
                        {
                            if( fields[i] == keyName )
                            {
                                ok_count = 0;
                                break;
                            }
                        }
                    }
                    if ( 0 == ok_count ) continue ;
                    if( fieldNumMatch && (ok_count < fieldCount * 0.75f) )  // 数量匹配有误
                    {
                        continue;
                    }
                    dictRecord.Add( (TKEY)fieldKey.GetValue(record) , record);
                }
                catch (System.Exception ex)
                {
                    Debug.Log("exception := " + ex.Message);
                }
            }
            reader.Close();
        }
        catch (System.Exception ex)
        {
            ret = "exception := " + ex.Message;
        }
        finally
        {
            if (null != fs)
            {
                fs.Close();
                fs = null;
            }
        }
        return ret;
    }

    public static void WriteToCsvFile<TRECORD>(string path, ICollection<TRECORD> listRecord)
    {
        FileStream fs = null;
        try
        {
            fs = IOUtil.TruncateOpen(path, FileMode.Truncate);
            if (null == fs)
            {
                Debug.Log(" file open failed , path := " + path);
                return;
            }
            StreamWriter wrtier = new StreamWriter(fs);
            FieldInfo[] fis = typeof(TRECORD).GetFields();
            string header = fis[0].Name;
            for (int i = 1; i < fis.Length; ++i)
            {
                header += "," + fis[i].Name;
            }
            wrtier.WriteLine(header);
            foreach (TRECORD record in listRecord)
            {
                string line = GetField(record, fis[0]);
                for (int i = 1; i < fis.Length; ++i)
                {
                    line += "," + GetField(record, fis[i]);
                }
                wrtier.WriteLine(line);
            }
            wrtier.WriteLine();

            wrtier.Flush();
            wrtier.Close();
            fs.Close();
            fs = null;
        }
        catch (System.Exception ex)
        {
            Debug.Log(" exception := " + ex.Message);
            throw ex;
        }
        finally
        {
            if (null != fs)
            {
                fs.Close();
                fs = null;
            }
        }
    }
}
