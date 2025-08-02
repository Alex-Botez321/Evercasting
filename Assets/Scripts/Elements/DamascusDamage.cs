using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamascusDamage : MonoBehaviour
{
    [SerializeField] private GameObject damascusGlobules;
    [SerializeField] private float globspawntime = 2f;
    [HideInInspector] public float damageMultiplier = 1f;
    [SerializeField] private int globCount;
    [SerializeField] private float globForce;

    //theoretical layer 10 for walls, if i want to give designers more work
    // or maybe i just go with nearest mesh
    // or maybe just use default layer 0??????
    private int layerNumber = 0; 

    private Coroutine _explosionCouroutine;

    private void Awake()
    {
        
        _explosionCouroutine = StartCoroutine(ExplosionCoroutine());
    }

    private IEnumerator ExplosionCoroutine()
    {
        LayerMask layerMask = 1 << layerNumber;
        yield return new WaitForSeconds(globspawntime);

        float incrementAngle = 360 / globCount;
        Vector3 baseVector = new Vector3(1, transform.position.y, 1);
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        for (int i = 0; i < globCount; i++)
        {
            GameObject projectile = Instantiate(damascusGlobules, spawnPosition, Quaternion.identity);
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            Vector3 shootVector = Quaternion.Euler(0, incrementAngle * i, 0) * baseVector;
            Vector3 projectileDirection = new Vector3(shootVector.x, 1, shootVector.z);
            projRB.AddForce(projectileDirection * globForce, ForceMode.Force);
        }

        Destroy(this.gameObject);

    }
}
