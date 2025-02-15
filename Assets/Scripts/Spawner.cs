using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] spawningObjects;
    [SerializeField] protected SpawnPoint[] spawnPoints;

    [Tooltip("How many enemies should be spawned")]
    [SerializeField] protected int wantedNumberOfEnemies = int.MaxValue;

    [SerializeField] bool spawnRoutineOnStart = true;
    [SerializeField] bool spawnRandomObject = true;
    [SerializeField] bool spawnOnRandomPoint = true;

    [Tooltip("Time to next Spawn")]
    [SerializeField] float minSpawnTime = 5;
    [SerializeField] float maxSpawnTime = 5;

    protected Coroutine spawnRoutine;
    protected int spawnedObjects = 0;

    int spawnObjectIndex = 0;
    int spawnPointIndex = 0;

    virtual protected void OnEnable()
    {
        GameManager.instance.StartSpawning += StartSpawning;
        GameManager.instance.StopSpawning += StopSpawning;
    }

    virtual protected void Start()
    {
        if (spawnRoutineOnStart)
        {
            spawnRoutine = StartCoroutine(SpawnRoutine());
        }
    }

    virtual protected IEnumerator SpawnRoutine()
    {
        while (spawnedObjects <= wantedNumberOfEnemies)
        {
            SpawnObject(spawningObjects[GetObjectIndex()], spawnPoints[GetSpawnIndex()].transform.position);

            yield return new WaitForSeconds(GetNextSpawnTime());
        }

        OnSpawningObjectsDepleted();
    }

    protected void SpawnObject(GameObject obj, Vector3 position)
    {
        GameObject asteroid = Instantiate(obj, position, Quaternion.identity);
        spawnedObjects++;
    }

    protected int GetObjectIndex()
    {
        return GetIndex(spawnRandomObject, ref spawnObjectIndex, spawningObjects.Length);
    }

    protected int GetSpawnIndex()
    {
        return GetIndex(spawnOnRandomPoint, ref spawnPointIndex, spawnPoints.Length);
    }

    int GetIndex(bool useRandom, ref int index, int arrayLength)
    {
        if (!useRandom)
        {
            if (index >= arrayLength)
            {
                index = 0;
            }
            return index++;
        }

        if (arrayLength > 1)
        {
            return UnityEngine.Random.Range(0, arrayLength);
        }
        return 0;
    }

    protected float GetNextSpawnTime()
    {
        if (minSpawnTime < maxSpawnTime)
        {
            float time = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);

            return time;
        }
        return minSpawnTime;
    }

    protected void StopSpawning(float timeToPause)
    {
        Invoke(nameof(StopSpawning), timeToPause);
    }

    protected void StartSpawning(float timeToResume)
    {
        Invoke(nameof(StartSpawning), timeToResume);
    }

    protected void StopSpawning()
    {
        if (spawnRoutine == null)
        {
            return;
        }

        StopCoroutine(spawnRoutine);
    }

    protected void StartSpawning()
    {
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    protected virtual void OnSpawningObjectsDepleted() { }

    virtual protected void OnDisable()
    {
        GameManager.instance.StartSpawning -= StartSpawning;
        GameManager.instance.StopSpawning -= StopSpawning;
    }
}