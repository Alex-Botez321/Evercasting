using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager nextWave;
    [SerializeField] private float spawnDelay = 5;
    [SerializeField] private bool spawnOnStart = false;

    private void Start()
    {
        if (spawnOnStart)
        {
            StartCoroutine(DelayedSpawn());
        }      
    }

    public void SpawnEnemies()
    {
        foreach(EnemySpawner spawner in GetComponentsInChildren<EnemySpawner>(true))
        {
            spawner.gameObject.SetActive(true);
        }
    }

    public void StartDelayedSpawn()
    {
        StartCoroutine(DelayedSpawn());
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnEnemies();
    }

    private void OnTransformChildrenChanged()
    {
        if (GetComponentsInChildren<StateController>(true).Length <= 0)
        {
            if (nextWave != null)
                nextWave.StartDelayedSpawn();
            else
                GetComponentInParent<RoomData>().UnlockExits();
        }
    }
}
