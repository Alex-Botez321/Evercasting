using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CameraSwapper : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera1;
    [SerializeField] private CinemachineVirtualCamera camera2;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (camera1.Priority >= camera2.Priority)
                camera2.Priority = 20;
            else
                camera2.Priority = 0;
        }
    }
}
