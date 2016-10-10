using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//放到需要画扇形的gameobject上
[RequireComponent(typeof(MeshFilter))]
public class FanDisp : MonoBehaviour
{
    private CFanMesh m_FanMesh;
    
    private const int Section_Num = 5*2;

    // Use this for initialization
	void Start ()
    {
        m_FanMesh = new CFanMesh();
	}

    public void BuildFan(CFanBody fanBody)
    {  
        float angle = fanBody.m_fAngle / ((float)Section_Num);

        Vector3 dir = new Vector3(fanBody.m_vForward.x, 0.0f, fanBody.m_vForward.y);
        dir.Normalize();

        Vector3 pos = transform.position;

        List<FanSection> sections = new List<FanSection>();

        for (int i = -Section_Num / 2; i <= Section_Num / 2; i++)
        {
            Vector3 dirSection = Quaternion.Euler(0, angle * i, 0) * dir;
            dirSection.Normalize();
            Vector3 posIn = pos + dirSection * fanBody.m_fRadiusIn;
            Vector3 posOut = pos + dirSection * fanBody.m_fRadiusOut;

            sections.Add(new FanSection(posIn, posOut));
        }

        m_FanMesh.BuildFan(gameObject, ref sections);      
    }
}
