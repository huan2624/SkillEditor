using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class TSingleton<T> 
    where T:class , new()
{
    static T ms_instance = null;
    public static T Instance 
    {
        get 
        {
            if( null == ms_instance )
            {
                ms_instance = new T();
            }
            return ms_instance ;
        }
    }
}

public class TSingletonX<T>
    where T : class , new()
{
    static T ms_instace = null;
    public static void CreateSingleton()
    {
        if( null == ms_instace )
        {
            ms_instace = new T();
        }
    }

    public static void DestroySingleton()
    {
        if (ms_instace != null)
        {
            ms_instace = null;
        }
    }

    public static T Intance
    {
        get 
        {
            return ms_instace;
        }
    }
}


public class TSingletonM<T> : MonoBehaviour
	where T:class , new()
{
	static T ms_instance = null;
	public static T Instance 
	{
		set
		{
			ms_instance = value;
		}
		get 
		{
			return ms_instance ;
		}
	}
}