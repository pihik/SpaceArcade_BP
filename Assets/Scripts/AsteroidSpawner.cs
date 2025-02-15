using System.Collections;
using UnityEngine;

public class AsteroidSpawner : Spawner
{
    [Range(0.0f, 2f)]
    [SerializeField] float asteroidSpeedMultiplier = 1f;

    Asteroid_ObjectPool[] asteroidPool;

    protected override void OnEnable()
    {
        base.OnEnable();

        GameManager.instance.AsteroidStopEvent += SpawnPauseWithResume;
    }

    protected override void Start()
    {
        asteroidPool = InGameHelper.instance.GetAsteroidPools();
        
        base.Start();
    }

    protected override IEnumerator SpawnRoutine()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, asteroidPool.Length);
            int spawnIndex = GetSpawnIndex();

            GameObject asteroid = asteroidPool[randomIndex].GetObjectFromPool();
            asteroid.transform.position = spawnPoints[spawnIndex].transform.position;

            if (asteroid.TryGetComponent<AsteroidMove>(out AsteroidMove asteroidComponent))
            {
                asteroidComponent.SetSpeedMultiplier(asteroidSpeedMultiplier);
                asteroidComponent.SetDirection(spawnPoints[spawnIndex].GetCurrentDirection());
            }

            yield return new WaitForSeconds(GetNextSpawnTime());
        }
    }

    void SpawnPauseWithResume(float timeToPause, float resumeDuration)
    {
        if (timeToPause < resumeDuration)
        {
            resumeDuration += timeToPause;
        }

        Invoke(nameof(StopSpawning), timeToPause);
        Invoke(nameof(StartSpawning), resumeDuration);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        GameManager.instance.AsteroidStopEvent -= SpawnPauseWithResume;
    }
}