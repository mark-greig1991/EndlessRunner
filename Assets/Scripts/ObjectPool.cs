using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialPoolSize = 10;
    private Queue<GameObject> pool = new Queue<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // creates, disables, and queues objects == poolsize
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }


    // pulls an object from the pool, if none exists, creates an object for use instead
    // the pool object's transform is set so it spawns in correctly, and it is parented for organisation
    // the object is activated and returned
    public GameObject GetFromPool(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        if (parent != null) obj.transform.SetParent(parent);

        obj.SetActive(true);
        return obj;
    }

    // deactivates and returns the passed object to the pool (queue)
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
