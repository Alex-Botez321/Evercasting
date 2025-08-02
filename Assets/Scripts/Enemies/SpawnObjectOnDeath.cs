using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnObjectOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    private void OnDestroy()
    {
        Instantiate(objectToSpawn, transform.position, quaternion.identity);
    }
}
