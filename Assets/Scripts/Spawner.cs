using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float SpawnRate = 1f;
    public SpriteRenderer SpawnBoundary;
    public GameObject PrefabToSpawn;

    private float _nextTimeToSpawn = 0f;

    private float _lastSpawnedX = 0f;
    private float _lastSpawnedY = 0f;
    
    void Update()
    {
        if (!(Time.time >= _nextTimeToSpawn))
        {
            return;
        }

        if (_lastSpawnedX >= SpawnBoundary.bounds.min.x)
        {
            return;
        }

        SpawnRandomPrefab();
        _nextTimeToSpawn = Time.time + 1f / SpawnRate;
    }

    private void SpawnRandomPrefab()
    {
        var newPrefab = Instantiate(PrefabToSpawn, RandomPointInBoundary(), Quaternion.identity, this.transform);
    }

    private Vector2 RandomPointInBoundary()
    {
        var bounds = SpawnBoundary.bounds;
        const float scale = 1f;

        var randomPointInBounds = new Vector2(
            Random.Range(bounds.min.x * scale, bounds.max.x * scale),
            Random.Range(bounds.min.y * scale, bounds.max.y * scale)
        );

        var rndPointInside = new Vector2(randomPointInBounds.x, randomPointInBounds.y);

        _lastSpawnedX = bounds.min.x;
        _lastSpawnedY = bounds.min.y;

        return rndPointInside;
    }
}
