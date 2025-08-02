using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePatternCleanUp : MonoBehaviour
{
    [SerializeField] private int childCount = 0;
    private void OnTransformChildrenChanged()
    {
        if(transform.childCount <= childCount)
            Destroy(this.gameObject);
    }
}
