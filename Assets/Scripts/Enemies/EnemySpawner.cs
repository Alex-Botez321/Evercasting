using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private AudioClip[] spawnSFX;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SpawnEnemy()
    {
        enemy = Instantiate(enemy, transform.position, Quaternion.identity, GetComponentInParent<EnemyWaveManager>().transform);
        enemy.transform.localScale = new Vector3(1f, 1f, 1f);
        int soundClip = Random.Range(0, spawnSFX.Length);
        audioSource.PlayOneShot(spawnSFX[soundClip]);
        Destroy(this.gameObject);
    }
}
