using UnityEngine;
using System.Collections;

public class EffectManager : TSingleton<EffectManager> {

    protected GameObject CreateEffect(string effect_name, Vector3 pos, Vector3 dir, float fScale, Quaternion rotation)
    {
        if (string.IsNullOrEmpty(effect_name))
        {
            return null;
        }

        string effectprefab_path = effect_name;
        GameObject prefabObj = Resources.Load(effectprefab_path) as GameObject;

        if (prefabObj == null)
        {
            return null;
        }

        GameObject objEffect = new GameObject();
        objEffect.transform.position = pos;

        if (dir.sqrMagnitude > 0.001f)
        {
            objEffect.transform.forward = dir;
        }
        else if (rotation != null)
        {
            objEffect.transform.rotation = rotation;
        }

        if (fScale <= 0.0f)
        {
            fScale = 1.0f;
        }
        else if (fScale < 0.1f)
        {
            fScale = 0.1f;
        }
        else
        {
            fScale = Mathf.Min(fScale, 10.0f);
        }


        GameObject effect = (GameObject)GameObject.Instantiate(prefabObj);
        if (effect == null)
        {
            Debuger.LogError("Fail to Instantiate Effect: " + effectprefab_path);
            return null;
        }

        // must reset effect's scale
        effect.transform.localScale = Vector3.one;
        effect.transform.parent = objEffect.transform;
        effect.transform.localPosition = Vector3.zero;
        effect.transform.localRotation = Quaternion.identity;
        objEffect.transform.localScale = new Vector3(fScale, fScale, fScale);

        objEffect.name = string.Format("{0}[{1}]", effect_name, CommHelper.GenerateGOID());
        effect.name = objEffect.name + "_Effect";

        SetParticleState(false, effect);

        return objEffect;
    }

    public GameObject CreateWorldPosEffect(string effect_name, Vector3 pos, Vector3 dir, float fScale, float fDuration, Quaternion rotation)
    {
        GameObject objEffect = CreateEffect(effect_name, pos, dir, fScale, rotation);
        if (objEffect == null)
        {
            return objEffect;
        }

        if (fDuration > 0.0f && fDuration < Mathf.Infinity)
        {
            EffectLifeController life_controller = (EffectLifeController)objEffect.AddComponent<EffectLifeController>();

            if (life_controller)
            {
                life_controller.SetLiveTime(fDuration);
            }
        }

        return objEffect;
    }

    // 遍历整个obj 设置粒子系统 开关
    public void SetParticleState(bool bIsClose, GameObject obj)
    {
        if (null == obj)
        {
            return;
        }

        if (null != obj.GetComponent<ParticleSystem>())
        {
            if (bIsClose)
            {
                obj.GetComponent<ParticleSystem>().Stop(true);
            }
            else
            {
                obj.GetComponent<ParticleSystem>().Simulate(0.0f);
                obj.GetComponent<ParticleSystem>().Play(true);
            }
        }

        Transform particleTran = obj.transform;
        int nCount = particleTran.childCount;
        for (int i = 0; i < nCount; ++i)
        {
            Transform ChildTran = particleTran.GetChild(i);
            SetParticleState(bIsClose, ChildTran.gameObject);
        }
    }

    public void DestroyEffect(GameObject effect)
    {
        if (effect != null)
        {
            GameObject.Destroy(effect);
        }
    }
}
