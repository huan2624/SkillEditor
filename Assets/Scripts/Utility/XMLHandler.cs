#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class XMLHandler
{
    // An example where the encoding can be found is at
    // http://www.eggheadcafe.com/articles/system.xml.xmlserialization.asp
    // NOTIFY: UFT8 format is important!

    string UTF8ByteArrayToString(byte[]characters)
    {
        UTF8Encoding encoding  = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }

    byte[]StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding  = new UTF8Encoding();
        byte[]byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    // Here we serialize our UserData object of myData
    string SerializeObject<T>(T data)
    {
        MemoryStream memoryStream  = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(T));
        XmlTextWriter xmlTextWriter  = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, data);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        string XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    // Here we deserialize it back into its original form
    T DeSerializeObject<T>(string pXmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(T));
        MemoryStream memoryStream  = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        return (T)xs.Deserialize(memoryStream);
    }

    // Finally our save and load methods for the file itself
    public bool CreateXML<T>(string strFilePathName, T data)
    {
        if (string.IsNullOrEmpty(strFilePathName))
        {
            return false;
        }

        StreamWriter writer = null;
        FileInfo file = new FileInfo(strFilePathName);
        if (!file.Exists)
        {
            writer = file.CreateText();
        }
        else
        {
            //file.Delete();
            writer = file.CreateText();
        }

        if (writer == null)
        {
            LogManager.LogError("Fail to Create XML: " + strFilePathName);
            return false;
        }

        string content = SerializeObject<T>(data);
        writer.Write(content);
        writer.Close();
        LogManager.LogError("Create XML OK. " + strFilePathName);

        return true;
    }

    public bool LoadXML<T>(string strFilePathName, ref T data)
    {
        if (string.IsNullOrEmpty(strFilePathName))
        {
            return false;
        }

        StreamReader reader = File.OpenText(strFilePathName);
        if (reader == null)
        {
            LogManager.LogError("Fail to Open XML: " + strFilePathName);
            return false;
        }

        string content = reader.ReadToEnd();
        reader.Close();

        data = DeSerializeObject<T>(content);

        return true;
    }
}
#endif