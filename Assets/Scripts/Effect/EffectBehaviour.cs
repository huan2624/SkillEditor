using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectPos
{
    public uint m_uiTargetId;
    public DummyPoint m_emDummyPoint;
    public Vector3 m_vOffset;
    public float m_OffsetDisance;
}

public class EffectBehaviour : MonoBehaviour
{

    public static Transform GetDummyPointTransformByName(GameObject objTarget, string strDummyPoint)
    {
        if (string.IsNullOrEmpty(strDummyPoint))
        {
            return objTarget.transform;
        }

        //return FindTransformInAllChildren(objTarget.transform, strDummyPoint);
        foreach (Transform transform in objTarget.transform.GetComponentsInChildren<Transform>())
        {
            if (transform.name == strDummyPoint)
            {
                return transform;
            }
        }

        return null;
    }

    public static Transform GetDummyPointTransform(BaseActor ActorTarget, DummyPoint emDummyPoint)
	{
		Transform effect_trans = null;
        if (DummyPoint.DM_NONE == emDummyPoint)
		{
			effect_trans = ActorTarget.gameObject.transform;
		}
		else
		{
			Dictionary<DummyPoint, Transform> mapDummyTrans = ActorTarget.GetActorAttribute().ActorDummyPointDic;
            if (mapDummyTrans != null && mapDummyTrans.Count > 0)
            {
                mapDummyTrans.TryGetValue(emDummyPoint, out effect_trans);
            }
            else
            {
				effect_trans = GetDummyPointTransformByName( ActorTarget.gameObject, AvatarDefine.GetDummyPointName(emDummyPoint));
            }

            if (effect_trans == null)
            {
				effect_trans = ActorTarget.gameObject.transform;
            }
		}

		return effect_trans;
	}

    public static bool GetEffectWorldPos(EffectPos effect_pos, ref Vector3 world_pos)
    {
        if (effect_pos.m_uiTargetId == 0)
        {
            // Specific world position
            world_pos = effect_pos.m_vOffset;
        }
        else
        {
            // Specific game object
            BaseActor ActorTarget = ActorManager.Instance.GetActor(effect_pos.m_uiTargetId);
			if (!ActorTarget)
            {
                return false;
            }

			Transform effect_trans = EffectBehaviour.GetDummyPointTransform(ActorTarget, effect_pos.m_emDummyPoint);
            if (effect_trans)
            {
                world_pos = effect_trans.position + effect_pos.m_vOffset;
            }
            else
            {
				world_pos = ActorTarget.gameObject.transform.position;
            }
        }

        return true;
    }

    public static Vector3 GetEffectPos(Transform dummy_pos_trans, Vector3 vRotateOffset, float Distance)
    {
        if (dummy_pos_trans == null)
            return Vector3.zero;

        Vector3 pos = dummy_pos_trans.position;

        if (Distance > 1e-6f)
        {
            //Vector3 dir = Quaternion.Euler(vRotateOffset) * dummy_pos_trans.forward;
            Vector3 dir = dummy_pos_trans.rotation * vRotateOffset;
            pos += dir * Distance;
        }

        return pos;
    }

    public static bool GetEffectPosRotaion(EffectPos effect_pos, ref Vector3 position, ref Quaternion rotation)
    {
        BaseActor ActorTarget = ActorManager.Instance.GetActor(effect_pos.m_uiTargetId);
		if (ActorTarget == null)
        {
            return false;
        }

		Transform dummy_point_trans = EffectBehaviour.GetDummyPointTransform( ActorTarget, effect_pos.m_emDummyPoint);
        if (dummy_point_trans == null)
	    {
            return false;
	    }

        position = EffectBehaviour.GetEffectPos(dummy_point_trans, effect_pos.m_vOffset, effect_pos.m_OffsetDisance);

        rotation = dummy_point_trans.rotation;

        return true;
    }

    protected void DestroyThisEffect()
    {
        EffectManager.Instance.DestroyEffect(gameObject);
    }
}