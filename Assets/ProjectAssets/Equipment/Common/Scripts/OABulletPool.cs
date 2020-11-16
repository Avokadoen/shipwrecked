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
    [Tooltip("Max allowed size of the pool")]
    private int maxSize = 200;

    // total objects that that are bound to this pool
    private int pooledCount;

    [SerializeField]
    [Tooltip("Prefab that is meant to be pooled")]
    private GameObject prefab;

    [SerializeField]
    private OABallisticBullet[] freePool;

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

        freePool = new OABallisticBullet[maxSize];

        for (var i = 0; i < size; i++)
        {
            InstantiateBullet(i);
        }

        pooledCount = size;
    }


    /// <summary>
    /// Retrieves the next object in the pool
    /// Object will be re-collected after max duration is reached
    /// Object will be null if there is no objects left in the pool
    /// </summary>
    /// <returns>A gameobject from the free pool</returns>
    public OABallisticBullet GetNext()
    {
        if (size <= 0)
        {
            if (pooledCount < maxSize)
            {
                InstantiateBullet();
                pooledCount += 1;
                size = 1;
            } else
            {
                return null; // TODO: return a fake OABallisticBullet?
            }
        }

        var element = freePool[size - 1];
        size -= 1;

        element.transform.parent = null;
        return element;
    }

    // TODO: ensure it has the same composition as the prefab
    public void ReturnObject(OABallisticBullet obj)
    {
        obj.transform.parent = transform;
        freePool[size] = obj;
        size += 1;
        obj.gameObject.SetActive(false);
    }

    private void InstantiateBullet(int index = -1)
    {
        index = (index > -1) ? index : size;

        if (index < 0 || index >= maxSize)
            return;

        var pooledObject = Instantiate(prefab);
        pooledObject.transform.parent = transform;
        pooledObject.SetActive(false);
        var pooledBullet = pooledObject.GetComponent<OABallisticBullet>();
        pooledBullet.Pool = this;

       
        freePool[index] = pooledBullet;
    }
}
