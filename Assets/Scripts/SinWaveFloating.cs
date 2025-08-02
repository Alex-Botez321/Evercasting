using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWaveFloating : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float floatDistance = 2f;
    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.y += floatSpeed * Mathf.Sin(Time.time * floatSpeed) * floatDistance;
        transform.position = pos;
    }
}
