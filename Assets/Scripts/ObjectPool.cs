using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class WeightedPrefab
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float weight = 1f;
        public float unlockDistance = 0f;
    }
    public WeightedPrefab[] weightedPrefabs;
    // [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int initialPoolSize = 10;
    private Queue<GameObject> pool = new();

    void Awake()
    {
        // if (prefabs.Length == 0 || prefabs == null)
        // {
        //     Debug.LogError("ObjectPool: No prefabs assigned to the pool.");
        //     return;
        // }

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject prefab = GetRandomPrefab();
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
        if (weightedPrefabs.Length == 0 || weightedPrefabs == null)
        {
            Debug.LogError("ObjectPool: No prefabs assigned to the pool.");
            return null;
        }

        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(GetRandomPrefab());
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        if (parent != null) obj.transform.SetParent(parent);

        obj.SetActive(true);
        return obj;
    }

    private GameObject GetRandomPrefab()
    {
        // int randomIndex = Random.Range(0, prefabs.Length);
        // return prefabs[randomIndex];

        if (weightedPrefabs == null || weightedPrefabs.Length == 0)
        {
            Debug.LogError($"[{name}] ObjectPool has no weighted prefabs assigned!");
            return null;
        }

        // Get player distance for unlock checks
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        float playerDistance = player != null ? player.position.z : 0f;

        // Build a list of available prefabs based on unlock distance
        List<WeightedPrefab> availablePrefabs = new();
        foreach (var wp in weightedPrefabs)
        {
            if (playerDistance >= wp.unlockDistance)
            {
                availablePrefabs.Add(wp);
            }
        }

        if (availablePrefabs.Count == 0) availablePrefabs.Add(weightedPrefabs[0]);

        // Weighted random selection
        float totalWeight = 0f;
        foreach (var wp in availablePrefabs)
        {
            totalWeight += wp.weight;
        }

        float roll = Random.value * totalWeight;
        foreach (var wp in availablePrefabs)
        {
            if (roll < wp.weight)
            {
                return wp.prefab;
            }
            roll -= wp.weight;
        }

        return availablePrefabs[^1].prefab; // Fallback
    }
    
    public void RefreshPool()
{
    Queue<GameObject> newPool = new();

    while (pool.Count > 0)
    {
        GameObject obj = pool.Dequeue();
        Destroy(obj); // or ReturnToPool() with new prefab later
    }

    for (int i = 0; i < initialPoolSize; i++)
    {
        GameObject prefab = GetRandomPrefab();
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        newPool.Enqueue(obj);
    }

    while (newPool.Count > 0)
        pool.Enqueue(newPool.Dequeue());
}


    // deactivates and returns the passed object to the pool (queue)
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
