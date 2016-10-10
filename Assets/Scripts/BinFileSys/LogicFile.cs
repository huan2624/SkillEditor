/*
 *       LogicFile.cs
 *       Copyright (c) 2014, TopCloud Company 
 *       All rights reserved.
 *       Create By Sword.
 */


using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

// LogicFile for the bin file.
// Bin file is a list with ValueType.

// Base class.
public class LogicFile
{
	// To do in derived class, loading the data from bin file.
    public virtual void Init()
	{

	}
}

// We need to get a function to find a ValueType quickly, so we used dictionary.
// the key of dictionary must have a function to build it from the Value.
public class LogicFileWithKey<TKeyType, ValueType> : LogicFile
		where ValueType : wl_res.tsf4g_csharp_interface, new()
{
	protected Dictionary<TKeyType, ValueType> m_LogicDataTable = new Dictionary<TKeyType, ValueType>();

	public virtual TKeyType GetKey(ValueType Value)
	{
		return default(TKeyType);
	}

	public bool ReadBinFile(string strPath)
	{
		TextAsset textAsset = Resources.Load(strPath) as TextAsset;
		if (textAsset == null)
		{
			Debug.LogError("Bin File Read Failed! Path:" + strPath);
			return false;
		}

		byte[] rawBytes = textAsset.bytes;
		tsf4g_tdr_csharp.TdrReadBuf tdrBuff = new tsf4g_tdr_csharp.TdrReadBuf(ref rawBytes, rawBytes.Length);
		tdrBuff.disableEndian();

		//Parse Head
		tsf4g_tdr_csharp.TResHeadAll resHead = new tsf4g_tdr_csharp.TResHeadAll();
		resHead.load(ref tdrBuff);

		Debug.Log("Load TDR :" + strPath + "start");
		Debug.Log(resHead.mHead.ToString());
		Debug.Log("Load TDR :" + strPath + "end");

		int count = resHead.mHead.iCount;

		for (int i = 0; i < count; ++i)
		{
			ValueType value = new ValueType();					
			
			value.load(ref tdrBuff,0);
						 
			AddData(value);
		}

		return true;
	}

	void AddData(ValueType Value)
	{
		TKeyType Key = GetKey(Value);

        if ( m_LogicDataTable.ContainsKey(Key) )
        {
            string message;
            message = String.Format(" LogicFile add data has same key, key: {0}\n{1}", Key.ToString(), Environment.StackTrace);
            Debug.LogWarning(message);
            return;
        }
        
		m_LogicDataTable.Add(Key, Value);
	}

	public bool GetData(TKeyType key,out ValueType Value)
	{
		return m_LogicDataTable.TryGetValue(key, out Value);
	}

	// using for traversal
	public Dictionary<TKeyType, ValueType> GetTable()
	{
		return m_LogicDataTable;
	}
}


// Some logic file do not have any key, we use list to save the data.
public class LogicFileWithoutKey<ValueType> : LogicFile
		where ValueType : wl_res.tsf4g_csharp_interface, new()
{
	List<ValueType> m_LogicDataTable = new List<ValueType>();

	public virtual bool ReadBinFile(string strPath)
	{
		TextAsset textAsset = Resources.Load(strPath) as TextAsset;
		if (textAsset == null)
		{
			return false;
		}

		byte[] rawBytes = textAsset.bytes;
		tsf4g_tdr_csharp.TdrReadBuf tdrBuff = new tsf4g_tdr_csharp.TdrReadBuf(ref rawBytes, rawBytes.Length);
		tdrBuff.disableEndian();

		//Parse Head
		tsf4g_tdr_csharp.TResHeadAll resHead = new tsf4g_tdr_csharp.TResHeadAll();
		resHead.load(ref tdrBuff);

		int count = resHead.mHead.iCount;

		for (int i = 0; i < count; ++i)
		{
			ValueType value = new ValueType();

			value.load(ref tdrBuff, 0);

			AddData(value);
		}

		return true;
	}

	public void AddData(ValueType Value)
	{
		m_LogicDataTable.Add(Value);
	}

	public ValueType GetData(int index)
	{
		if (index >= m_LogicDataTable.Count)
		{
			Debug.LogError("ILogicFileWithoutKey GetData index out of range.");

			return default(ValueType);
		}

		return m_LogicDataTable[index];
	}

	// using for traversal
	public List<ValueType> GetTable()
	{
		return m_LogicDataTable;
	}
}

