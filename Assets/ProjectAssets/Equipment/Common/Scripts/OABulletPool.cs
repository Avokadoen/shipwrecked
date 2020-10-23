using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OABulletPool :  MonoBehaviour
{
    [SerializeField]
    [Tooltip("Size of the pool")]
    [Range(1, 200)]
    private int size = 50;

    [SerializeField]
    [Tooltip("Prefab that is meant to be pooled")]
    private GameObject prefab;

    [SerializeField]
    private List<OABallisticBullet> freePool;
    // private List<OABallisticBullet> takenPool; TODO: use this to reclaim bullets in the future

    void Awake()
    {
        OAExtentions.AssertObjectNotNull(
            prefab,
            $"OAObjectPool is missing prefab!"
        );

        OAExtentions.AssertObjectNotNull(
            prefab.GetComponent<OABallisticBullet>(),
            $"OAObjectPool prefab is no a OABallisticBullet!"
        );

        freePool = new List<OABallisticBullet>(size);
        // takenPool = new List<OABallisticBullet>(size);
        for (var i = 0; i < size; i++)
        {
            var pooledObject = Instantiate(prefab);
            pooledObject.transform.parent = transform;
            pooledObject.SetActive(false);
            var pooledBullet = pooledObject.GetComponent<OABallisticBullet>();
            pooledBullet.Pool = this;
            freePool.Insert(i, pooledBullet);
        }
    }

    /// <summary>
    /// Retrieves the next object in the pool
    /// Object will be re-collected after max duration is reached
    /// Object will be null if there is no objects left in the pool
    /// </summary>
    /// <returns>A gameobject from the free pool</returns>
    public OABallisticBullet GetNext()
    {
        var freeIndex = freePool.Count - 1;
        if (freeIndex < 0)
        {
            return null; // TODO: add to pool?
        }

        var element = freePool[freeIndex];
        freePool.SwapRemoveAt(freeIndex);
        element.transform.parent = null;
        return element;
    }

    // TODO: ensure it has the same composition as the prefab
    public void ReturnObject(OABallisticBullet obj)
    {
        obj.transform.parent = transform;
        freePool.Add(obj);
        obj.gameObject.SetActive(false);
        // TODO: check if we are over size
    }
}
