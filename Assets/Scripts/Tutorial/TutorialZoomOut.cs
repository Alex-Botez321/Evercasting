using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZoomOut : MonoBehaviour
{
    [SerializeField] private float minimumZoom;
    [SerializeField] private float maximumZoom;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 StartingPoint;
    [SerializeField] private Vector3 EndingPoint;
    private CinemachineVirtualCamera cam;
    private float distanceBetweenPoints;

    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        distanceBetweenPoints = StartingPoint.z - EndingPoint.z;
    }

    private void FixedUpdate()
    {
        float progress = StartingPoint.z - player.transform.position.z;
        float normalisedProgress = progress / distanceBetweenPoints;

        cam.m_Lens.OrthographicSize = Mathf.Lerp(minimumZoom, maximumZoom, normalisedProgress);
    }
}
