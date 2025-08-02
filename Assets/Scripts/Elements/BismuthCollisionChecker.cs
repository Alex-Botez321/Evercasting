using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BismuthCollisionChecker : MonoBehaviour
{
    public Vector3 initProjPosition = Vector3.zero;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ProjectileDamage>(out ProjectileDamage proj))
        {
            initProjPosition = proj.gameObject.transform.position;

            BismuthDamage bismuth = GetComponentInParent<BismuthDamage>();
            bismuth.prehitProjPosition = initProjPosition;
            Debug.Log(initProjPosition);
            Debug.Log("ghghgh");
        }
    }
}
