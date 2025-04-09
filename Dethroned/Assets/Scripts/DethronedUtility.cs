using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DethronedUtility : MonoBehaviour
{
    /// <summary>
    /// fetches a reference to a inactive prefab in the object pool. if there is no inactive prefab (or the pool is empty) it will instantiate one and add it to the pool.
    /// </summary>
    /// <param name="pool">the object pool list</param>
    /// <param name="prefab"></param>
    /// <returns>a GameObject fetched from the pool (or created if there was nothing to fetch)</returns>
    public static GameObject FetchPooledGameObject(List<GameObject> pool, GameObject prefab)
    {
        GameObject obj = pool.Find(p => !p.activeSelf && p.name.Equals(prefab.name + "(Clone)"));
        if (!obj) { obj = Instantiate(prefab); pool.Add(obj); }
        return obj;
    }
}
