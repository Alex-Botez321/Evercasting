using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class DownAttackProjectile : MonoBehaviour
{
    [SerializeField] private GameObject projectilesToSpawn;
    [SerializeField] private int projectileCount;
    [SerializeField] public GameObject owner;
    [SerializeField] private float projectileDamage;
    private Rigidbody rb;
    [SerializeField] private float fallSpeed = 10f;
    private bool hasStarted = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.velocity = Vector3.down * 10f;
        //Debug.Break();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner && !hasStarted)
        {
            hasStarted = true;
            Destroy(GetComponentInChildren<VisualEffect>());
            StartCoroutine(SpawnProjectiles());
        }
    }
    IEnumerator SpawnProjectiles()
    {
        rb.velocity = Vector3.zero;
        float incrementAngle = 360 / projectileCount;
        Vector3 baseVector = new Vector3(1, transform.position.y, 1);
        for (int i = 0; i < projectileCount; i++)
        {
            Vector3 shootVector = Quaternion.Euler(0, incrementAngle * i, 0) * baseVector;
            Vector3 projectileDirection = new Vector3(shootVector.x, 1, shootVector.z);
            GameObject projectile = Instantiate(projectilesToSpawn, transform.position,
                Quaternion.LookRotation(projectileDirection, Vector3.up), transform);
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            //projectile.transform.rotation = Quaternion.Euler(0, angle, 0);

            projRB.AddForce(projectileDirection * 600, ForceMode.Force);

            projectile.GetComponent<ProjectileDamage>().SetVariables(this.gameObject, 1, 1, 1);
            projectile.GetComponent<ProjectileDamage>().ownerObject = owner;

            yield return new WaitForSecondsRealtime(0.01f);

            projectile.GetComponent<ProjectileDamage>().canCollide = true;
        }
        
        Destroy(this);
        Destroy(transform.GetChild(0).gameObject);
        Destroy(GetComponent<SphereCollider>());
    }
}
