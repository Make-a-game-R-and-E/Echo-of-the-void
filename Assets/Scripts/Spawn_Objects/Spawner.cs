using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs to Spawn")]
    [Tooltip("Array of prefabs to spawn randomly")]
    [SerializeField] private GameObject[] objectPrefabs;

    [Header("Spawn Settings")]
    [Tooltip("Assign spawn point transforms in the Inspector")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Interval Settings")]
    [Tooltip("Minimum & maximum spawn interval in seconds")]
    [SerializeField] private float minSpawnInterval = 0f;
    [SerializeField] private float maxSpawnInterval = 2f;

    [Header("Hierarchy Organization")]
    [Tooltip("All spawned objects will become children of this Transform.")]
    [SerializeField] private Transform spawnParent;

    private float spawnTimer = 0f;

    private void Start()
    {
        // Initialize the spawn timer with a random interval
        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        // Count down the spawn timer
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            // Spawn a new object
            SpawnObject();

            // Reset the timer to a new random interval
            spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void SpawnObject()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned to the Spawner.");
            return;
        }

        if (objectPrefabs.Length == 0)
        {
            Debug.LogWarning("No object prefabs assigned to the Spawner.");
            return;
        }

        // Select a random spawn point
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPos = spawnPoints[spawnPointIndex].position;

        // Select a random prefab from the array
        int prefabIndex = Random.Range(0, objectPrefabs.Length);
        GameObject selectedPrefab = objectPrefabs[prefabIndex];

        // Instantiate the object, parenting it under 'spawnParent'
        GameObject spawnedObject = Instantiate(
            selectedPrefab,
            spawnPos,
            Quaternion.identity,
            spawnParent // This will keep the hierarchy organized
        );

        if (spawnedObject == null)
        {
            Debug.LogError("Failed to spawn object. Check if the prefab is correctly set.");
        }
    }
}
