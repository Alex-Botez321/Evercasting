using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTest : MonoBehaviour
{
    [SerializeField] private float endPosition;
    private Vector3 startPosition;
    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+moveSpeed*Time.deltaTime);
        if(transform.position.z >= endPosition)
            transform.position = startPosition;
    }
}
