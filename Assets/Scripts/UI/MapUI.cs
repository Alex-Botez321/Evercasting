using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MapUI : MonoBehaviour
{
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private float bufferSize;
    [SerializeField] private GameObject mapTile;
    [SerializeField] private float tileSize;
    [SerializeField] private GameObject canvas;

    public void BuildMap(int xPosition, int yPosition)
    {
        Instantiate(mapTile, 
            new Vector3(startPosition.x + ((tileSize+bufferSize) * xPosition), 
            startPosition.y + ((tileSize + bufferSize) * yPosition), 0), 
            Quaternion.identity, canvas.transform);

    }
}
