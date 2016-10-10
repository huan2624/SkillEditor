using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LocalTransformHolder
{
    public Vector3 localPosition = Vector3.zero;
    public Quaternion localRotate = Quaternion.identity;
    public Vector3 localScale = Vector3.one;
}

public static class CoreUtility
{
    public static Vector3 Vector3Multiply(Vector3 lhs, Vector3 rhs)
    {
        return new Vector3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
    }

    public static void clearLocalTransform(Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public static void clearLocalTransform_noscale(Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public static void clearTransform(Transform transform)
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    // 把一个节点挂接到一个节点下,保持 localXXX 
    public static void changeTransformParent(RectTransform trans, Transform parent)
    {
        Vector3 lcaPos = trans.localPosition;
        Quaternion lcaRotate = trans.localRotation;
        Vector3 lcaScale = trans.localScale;
        Vector2 offsetMin = trans.offsetMin;
        Vector2 offsetMax = trans.offsetMax;
        Vector2 sizeDelta = trans.sizeDelta;
        trans.SetParent(parent); // 这个操作会把 trans 的localXXX 全部替换,所以之后要重新设置
        trans.localScale = lcaScale;
        trans.localRotation = lcaRotate;
        trans.localPosition = lcaPos;
        trans.offsetMin = offsetMin;
        trans.offsetMax = offsetMax;
        trans.sizeDelta = sizeDelta;
    }

    // 把一个节点挂接到一个节点下,保持 localXXX 
    public static void changeTransformParent(Transform trans, Transform parent)
    {
        Vector3 lcaPos = trans.localPosition;
        Quaternion lcaRotate = trans.localRotation;
        Vector3 lcaScale = trans.localScale;
        trans.parent = parent; // 这个操作会把 trans 的localXXX 全部替换,所以之后要重新设置
        trans.localScale = lcaScale;
        trans.localRotation = lcaRotate;
        trans.localPosition = lcaPos;
    }

    // 参见 changeTransformParent
    public static void changeTransformParent1(Transform trans, Transform parent, LocalTransformHolder localHolder)
    {
        trans.parent = parent; // 这个操作会把 trans 的localXXX 全部替换,所以之后要重新设置
        trans.localScale = localHolder.localScale;
        trans.localRotation = localHolder.localRotate;
        trans.localPosition = localHolder.localPosition;
    }

    // 此函数用与 trans 没有parent的情况,在完成后 trans 的 localXXX 会全部被替换
    // 如果 localXXX 需要被记录,请使用 changeTransformPosition1
    public static void changeTransformPosition(Transform trans, Transform pos)
    {
        Vector3 lcaPos = trans.localPosition;
        Quaternion lcaRotate = trans.localRotation;
        Vector3 lcaScale = trans.localScale;

        trans.rotation = Quaternion.Euler(pos.rotation.eulerAngles + trans.localRotation.eulerAngles);
        trans.position = pos.position + trans.rotation * Vector3Multiply(lcaPos, pos.lossyScale);
    }

    public static void changeTransformPosition_noScale(Transform trans, Transform pos)
    {
        Vector3 lcaPos = trans.localPosition;
        Quaternion lcaRotate = trans.localRotation;
        Vector3 lcaScale = trans.localScale;

        trans.rotation = Quaternion.Euler(pos.rotation.eulerAngles + trans.localRotation.eulerAngles);
        trans.position = pos.position + trans.rotation * lcaPos;
    }

    public static void changeTransformPosition_noScale(Transform trans, Transform pos, LocalTransformHolder local)
    {
        trans.rotation = Quaternion.Euler(pos.rotation.eulerAngles + local.localRotate.eulerAngles);
        trans.position = pos.position + trans.rotation * local.localPosition;
    }

    // 参见 changeTransformPosition 说明
    public static void changeTransformPosition1(Transform trans, Transform pos, LocalTransformHolder localHolder)
    {
        trans.rotation = Quaternion.Euler(pos.rotation.eulerAngles + localHolder.localRotate.eulerAngles);
        trans.position = pos.position + trans.rotation * Vector3Multiply(localHolder.localPosition, localHolder.localScale);
    }

    public static void assignTransform(Transform dest, Transform src)
    {
        dest.position = src.position;
        dest.rotation = src.rotation;
    }

    public static GameObject findGameObject(GameObject root, string name)
    {
        Transform find = findTransform(root.transform, name);
        return (null == find ? null : find.gameObject);
    }

    public static T findGameObjectComponent<T>(GameObject root, string name) where T : Component
    {
        GameObject go = findGameObject(root, name);
        return (null == go ? null : go.GetComponent<T>());
    }


    public static Transform findTransform(Transform root, string name)
    {
        Transform dt = root.Find(name);
        if (null != dt)
        {
            return dt;
        }
        else
        {
            foreach (Transform child in root)
            {
                dt = findTransform(child, name);
                if (dt)
                {
                    return dt;
                }
            }
        }
        return null;
    }

    public static Transform findTransformX(Transform root, string path)
    {
        Transform node = null;
        if (-1 == path.IndexOf('.'))
        {
            return CoreUtility.findTransform(root, path);
        }
        else
        {
            char[] sp = { '.' };
            string[] nodes = path.Split(sp);
            node = CoreUtility.findTransform(root, nodes[0]);
            for (int i = 1; i < nodes.Length; ++i)
            {
                if (null == node) break;
                node = CoreUtility.findTransform(node, nodes[i]);
            }
            if (null != node && nodes[nodes.Length - 1] == node.name)
            {
                return node;
            }
        }
        return null;
    }

    public static void DestroyAllChildrenImmediate(GameObject root)
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (Transform trans in root.transform)
        {
            temp.Add(trans.gameObject);
        }
        for (int i = 0; i < temp.Count; ++i)
        {
            temp[i].transform.parent = null;
            UnityEngine.Object.DestroyImmediate(temp[i]);
            temp[i] = null;
        }
        temp.Clear();
    }
    /// <summary>
    /// 获取该游戏对象下的子节点
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public static List<Transform> GetAllChildren(GameObject root)
    {
        if (root == null)
            return null;
        List<Transform> children = new List<Transform>();
        foreach (Transform child in root.transform)
        {
            if (child != null)
                children.Add(child);
        }
        return children;
    }

    public static Component CreateComponent(GameObject gameObject, System.Type componentType)
    {
        Component comp = gameObject.GetComponent(componentType);
        if (null == comp)
        {
            comp = gameObject.AddComponent(componentType);
        }
        return comp;
    }

    public static void setTransformsCastShadow(Transform root, bool castShadows)
    {
        Renderer ren = root.GetComponent<Renderer>();
        if (ren)
        {
            ren.castShadows = castShadows;
        }
        Renderer[] rens = root.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rens)
        {
            r.castShadows = castShadows;
        }
    }

    public static void setTransformsReceiveShadow(Transform root, bool receiveShadows)
    {
        Renderer ren = root.GetComponent<Renderer>();
        if (ren)
        {
            ren.receiveShadows = receiveShadows;
        }
        Renderer[] rens = root.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rens)
        {
            r.receiveShadows = receiveShadows;
        }
    }

    public static void setTransformsShadow(Transform root, bool castShadows, bool receiveShadows)
    {
        Renderer ren = root.GetComponent<Renderer>();
        if (ren)
        {
            ren.castShadows = castShadows;
            ren.receiveShadows = receiveShadows;
        }
        Renderer[] rens = root.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rens)
        {
            r.castShadows = castShadows;
            r.receiveShadows = receiveShadows;
        }
    }

    //改变所有子对象的层
    public static void setGameObject_layer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
        {
            setGameObject_layer(child, layer);
        }
    }

    public static void setGameObject_tag(Transform root, string tag)
    {
        root.gameObject.tag = tag;
        foreach (Transform child in root)
        {
            setGameObject_tag(child, tag);
        }

    }

   /// <summary>
   /// 把 以"splitCharArray"分割的字符串"parseString"解析成，整型List,占位符'?' 以-1替换
   /// </summary>
   /// <param name="parseString">字符串</param>
   /// <param name="splitCharArray">分割字符串字符数组</param>
   /// <returns></returns>
    public static List<int> ParseStringToIntList(string parseString,char []splitCharArray)
    {
        if (string.IsNullOrEmpty(parseString))
            return null;
        List<int> splitList = new List<int>();
        string[] splitArray = (parseString.Trim()).Split(splitCharArray);
        if (splitArray != null)
        {
            foreach (string tempString in splitArray)
            {
                int tempInt = 0;
                try
                {
                    //如果是“？”表示占位符，默认以-1表示
                    if (string.IsNullOrEmpty(tempString))
                    {
                        continue;
                    }

                    string pString = tempString.Trim();
                    if (pString == "?")
                        tempInt = -1;
                    else
                        int.TryParse(pString, out tempInt);
                }catch(System.Exception e)
                {
                    Log.Error(e.ToString());
                }
                splitList.Add(tempInt);
            }
        }
        return splitList;
    }


    public static Vector3 ParseStringToVector3(string parseString,char []splitCharArray)
    {
        Vector3 pos = Vector3.zero;
        if (!string.IsNullOrEmpty(parseString))
        {
            string[] pArray = parseString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (pArray != null && pArray.Length == 3)
            {
                pos.x = float.Parse(pArray[0]);
                pos.y = float.Parse(pArray[1]);
                pos.z = float.Parse(pArray[2]);
            }
        }
        return pos;
    }
    /// <summary>
    /// 判断一个世界坐标点是否在摄像机的视野内
    /// </summary>
    /// <param name="tCamera"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static bool IsPointInCamera(Camera tCamera,Vector3 point)
    {
        if (tCamera == null)
            return false;
        Vector3 viewPortPoint = tCamera.WorldToViewportPoint(point);
        if (viewPortPoint.x >= 0 && viewPortPoint.x <= 1 && viewPortPoint.y >= 0 && viewPortPoint.y <= 1)
            return true;
        return false;
    }

    /// <summary>
    /// 判断一个GameObject 是否在相机的视野内（该方法未完成不可用）
    /// </summary>
    /// <param name="tCamera"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsGameObjectInCamera(Camera tCamera, GameObject obj)
    {
        if (obj == null || tCamera == null)
            return false;
        int layerMask =  1 << obj.layer;
        if ((tCamera.cullingMask & layerMask) != 0)
        {
            bool centerInCamera = IsPointInCamera(tCamera, obj.transform.position);
            if (centerInCamera)
            {
                return true;
            }
            else
            {
                //tCamera.
            }
        }
        return false;
    }

    /// <summary>
    /// 获取两个点在direction方向上的距离
    /// </summary>
    /// <param name="firstPoint">起始点</param>
    /// <param name="secondPoint">终点</param>
    /// <param name="direction">方向向量</param>
    /// <returns>放回，两个点在direction 上的距离，如果distance>0，则起始点和终点与direction方向相同 ，反之 方向不同</returns>
    public static float DistanceInTwoPointsWithDirection(Vector3 firstPoint, Vector3 secondPoint,Vector3 direction)
    {
        float distance = 0f;
        Vector3 currentDirection = secondPoint - firstPoint;
        distance = Vector3.Dot(currentDirection, direction.normalized);
        return distance;
    }
    /// <summary>
    /// 随机四舍五入
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static int Random(float start, float end)
    {
        float randomData = UnityEngine.Random.Range(start, end);
        int randomInt = (int)randomData;
        return (randomData - randomInt > 0.5f) ? randomInt + 1 : randomInt;
    }

   /// <summary>
    /// 根据权重列表随机获取其中一个
   /// </summary>
   /// <param name="weight">权重列表</param>
   /// <param name="lastIndex">默认-1，</param>
   /// <param name="IsCanSamePreOne">是否可与前一个随机索引相同</param>
   /// <returns></returns>
    public static int Random(List<int> weightList,int lastIndex = -1,bool IsCanSamePreOne = false)
    {
        if (weightList == null || weightList.Count == 0)
            return -1;
        if (weightList.Count == 1)
            return 0;
        int randomWeight = 0;
        for (int i = 0; i < weightList.Count;i++ )
        {
            randomWeight += weightList[i];
        }

        int randomNum = 0;
        float freq = 0;
        int randomIndex = 0;
        do
        {
            randomNum = UnityEngine.Random.Range(0, randomWeight);
            freq = 0;
            for (int j = 0;j < weightList.Count;j++)
            {
                freq += weightList[j];
                if (randomNum < (int) freq)
                {
                    randomIndex = j;
                    break;
                }
            }
        } while ((!IsCanSamePreOne && randomIndex == lastIndex));
        return randomIndex;
    }

    /// <summary>
    /// 转换世界坐标到NGUI屏幕坐标
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public static Vector3 ChangeUnityWorldPosToNGUIPos(Vector3 worldPos,Camera worldCamera)
    {
        //使用主摄像机世界坐标到屏幕坐标
        Vector3 pos1 = worldCamera.WorldToScreenPoint(worldPos);
        pos1.z = 0;
        //使用UI摄像机转换到NGUI的世界坐标
        Vector3 pos2 = Vector3.one; //UICamera.currentCamera.ScreenToWorldPoint(pos1);
        return pos2;
    }

    /// <summary>
    /// 把NGUI坐标转换为世界坐标
    /// </summary>
    /// <param name="nguiPos"></param>
    /// <param name="worldCamera"></param>
    /// <returns></returns>
    public static Vector3 ChangeNGUIPosToUnityWorld(Vector3 nguiPos, Camera worldCamera)
    {
        //使用UICamera把NGUI坐标转换到屏幕坐标
        Vector3 pos1 = Vector3.one; //UICamera.currentCamera.WorldToScreenPoint(nguiPos);
        pos1.z = 1;
        //使用worldCamera把屏幕坐标转换为世界坐标
        Vector3 pos2 = worldCamera.ScreenToWorldPoint(pos1);
        return pos2;
    }

    /// <summary>
    /// 是否两个矩形相交
    /// </summary>
    /// <param name="rect1"></param>
    /// <param name="rect2"></param>
    /// <returns></returns>
    public static bool IsRectIntersect(Rect rect1, Rect rect2)
    {
        float minX = Mathf.Max(rect1.x,rect2.x);
        float minY = Mathf.Max(rect1.y,rect2.y);
        float maxX = Mathf.Min(rect1.xMax,rect2.xMax);
        float maxY = Mathf.Min(rect1.yMax,rect2.yMax);
        return (minX <= maxX && minY <= maxY);
    }

    /// <summary>
    /// 设置例子RenderQuene
    /// </summary>
    /// <param name="t"></param>
    /// <param name="renderQuene"></param>
    /// <param name="coverChild"></param>
    public static void SetParticleRenderQ(Transform t,int renderQuene,bool coverChild = true)
    {
        if (t.GetComponent<Renderer>() != null && t.GetComponent<Renderer>().sharedMaterial != null)
        {
            t.GetComponent<Renderer>().sharedMaterial.renderQueue = renderQuene;
        }
        if (t.childCount != 0 && coverChild)
        {
            foreach (Transform item in t)
            {
                CoreUtility.SetParticleRenderQ(item,renderQuene,coverChild);
            }
        }
    }

    /// <summary>
    /// 添加对象
    /// </summary>
    /// <param name="root">根节点</param>
    /// <param name="prefab">预制</param>
    /// <param name="name">对象名称</param>
    /// <returns></returns>
    public static GameObject AddChild(GameObject root,GameObject prefab,string name)
    {
        if (root == null)
            return null;
        GameObject obj = null;
        if (prefab != null)
        {
            obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        }else
        {
            obj = new GameObject(!string.IsNullOrEmpty(name) ? name : "GameObject");
        }
        if (obj != null)
        {
            obj.transform.parent = root.transform;
            obj.transform.localEulerAngles = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
        }
        return obj;
    }

    public static void AddChild(Transform parent, GameObject go)
    {
        if (parent == null || go == null) return;

        go.transform.parent = parent.transform;

        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.transform.localEulerAngles = Vector3.zero;
    }

    public static T GetComponent<T>(GameObject go) where T : Component
    {
        if (null == go) return null;
        T t = go.GetComponent<T>();

        if (null == t) {
            t = go.AddComponent<T>();
        }

        return t;
    }

    public static void DestroyComponent<T>(GameObject go) where T : Component
    {
        if (null == go) return ;
        T t = go.GetComponent<T>();

        if (null != t)
        {
            GameObject.DestroyImmediate(t);
        }
    }

    public static Vector3 StringToVector3(string position)
    {
        Vector3 vec = Vector3.zero;

        if (!string.IsNullOrEmpty(position))
        {
            string[] str = position.Split('|');

            vec.x = int.Parse(str[0]);
            vec.y = int.Parse(str[1]);
            vec.z = int.Parse(str[2]);
        }

        return vec;
    }
}
