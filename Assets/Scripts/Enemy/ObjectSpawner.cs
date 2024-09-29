using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private Transform spawnPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // Example: Press space to spawn
        {
            GameObject pooledObject = objectPool.GetObjectFromPool();
            pooledObject.transform.position = spawnPoint.position;  // Set position
        }
    }
}
