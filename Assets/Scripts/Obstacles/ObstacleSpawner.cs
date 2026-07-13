using UnityEngine;


public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private GameConfig config;
    [SerializeField] private float spawnX = 12f;
    [SerializeField] private float spawnY = 0.5f;

    private ObjectPool<Obstacle> pool;
    private float timer;
    private float nextInterval;

    void Awake()
    {
        pool = new ObjectPool<Obstacle>(obstaclePrefab, config.obstaclePoolSize, transform);
        SetNextInterval();
    }

    void OnEnable() => GameEvents.OnGameStart += ResetSpawner;
    void OnDisable() => GameEvents.OnGameStart -= ResetSpawner;

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        timer += Time.deltaTime;
        if (timer >= nextInterval)
        {
            SpawnObstacle();
            timer = 0f;
            SetNextInterval();
        }
    }

    private void SpawnObstacle()
    {
        Obstacle obs = pool.Get();
        obs.transform.position = new Vector3(spawnX, spawnY, 0f);
        obs.Init(pool);
    }

    private void SetNextInterval()
    {
        nextInterval = Random.Range(config.minSpawnInterval, config.maxSpawnInterval);
    }

    private void ResetSpawner()
    {
        timer = 0f;
        SetNextInterval();
    }
}
