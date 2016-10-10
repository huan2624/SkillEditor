using UnityEngine;
using System.Collections;


public class CFanBody
{
	public Vector2 m_vForward;
	public Vector3 m_vCenter;
	public float m_fRadiusIn;
	public float m_fRadiusOut;
	public float m_fAngle;
	public float m_fHeight;
	public float m_fDistance;
	public float m_fVerticalOffset;
	public Vector2 m_vLocalForward;

	public CFanBody(Vector3 vPosition ,Vector3 vForward , float fDistance ,float fRadiusIn, float fRadiusOut, float fHeight , float fAngle , float fVerticalOffset)
	{
		m_vForward.x = vForward.x;
		m_vForward.y = vForward.z;
		m_fDistance = fDistance;
		m_vCenter = vPosition + vForward.normalized * m_fDistance;
		m_fRadiusIn = fRadiusIn;
		m_fRadiusOut = fRadiusOut;
		m_fHeight = fHeight;
		m_fAngle = fAngle;
		m_fVerticalOffset = fVerticalOffset;
	}

    public bool Pt3InFan(Vector3 vPoint, float fActorRadius, float fActorHeight)
	{
        if (vPoint.y < m_vCenter.y + m_fVerticalOffset - fActorHeight || vPoint.y > m_vCenter.y + m_fHeight + m_fVerticalOffset + fActorHeight)
		{
			return false;
		}

		Vector2 vMapPoint;
		vMapPoint.x = vPoint.x;
		vMapPoint.y = vPoint.z;
        return Pt2InSector(vMapPoint, fActorRadius);
	}

    private bool Pt2InSector(Vector2 vPoint, float fActorRadius)
	{
		Vector2 vCenter;
		vCenter.x = m_vCenter.x;
		vCenter.y = m_vCenter.z;

		Vector2 vOffset = vPoint - vCenter;
		float sqrMag = vOffset.sqrMagnitude;
        float sqrInR = m_fRadiusIn * m_fRadiusIn;
        float sqrOutR = (m_fRadiusOut + fActorRadius) * (m_fRadiusOut + fActorRadius);
        if (sqrMag < sqrInR || sqrMag > sqrOutR)
		{
			return false;
		}

		float fR = Vector2.Angle(m_vForward, vOffset);
		
		return (fR <= m_fAngle / 2);
	}
}