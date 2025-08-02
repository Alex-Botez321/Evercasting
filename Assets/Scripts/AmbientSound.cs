using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    public Collider Area;
    public Collider Player;

    // Update is called once per frame
    void Update()
    {
        Vector3 closestPoint = Area.ClosestPoint(Player.transform.position);
        transform.position = closestPoint;
    }
}
