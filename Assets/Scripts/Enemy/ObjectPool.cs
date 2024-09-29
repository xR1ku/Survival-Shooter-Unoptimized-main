using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;  // The prefab to be pooled
    [SerializeField] private int initialPoolSize = 10; // Initial size of the pool

    private List<GameObject> pool; // List to hold the pooled objects

    private void Awake()
    {
        // Initialize the pool
        pool = new List<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);  // Deactivate the object
            pool.Add(obj);  // Add it to the pool
        }
    }

    // Function to get an object from the pool
    public GameObject GetObjectFromPool()
    {
        // Find an inactive object in the pool
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // If all objects are in use, instantiate a new one, add it to the pool, and return it
        GameObject newObj = Instantiate(objectPrefab);
        pool.Add(newObj);
        return newObj;
    }

    // Optional: Return an object to the pool (deactivate and reuse later)
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);  // Deactivate the object
    }
}
