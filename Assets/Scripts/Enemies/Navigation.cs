using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 3f;
    private float attackTimeStamp;
    [SerializeField] private GameObject projectile;
    private float baseSpeed = 3.5f;
    [SerializeField]private float currentSpeed;
    public bool inWater; // probably a bad idea in the long term, but might work in the short term
    public bool WaterStatus; // for use with electricity in the future
    [SerializeField] private GameObject[] model;

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        int modelUsed = UnityEngine.Random.Range(0, 3);
        model[modelUsed].SetActive(true);
        
    }
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = attackRange;
    }


    private void FixedUpdate()
    {
        int layerMask = ~(LayerMask.GetMask("Agents", "Projectile"));
        
        //layerMask = ~layerMask;
        RaycastHit hit;


        if (Physics.Raycast(transform.position, playerTransform.position-transform.position, out hit, Mathf.Infinity, layerMask))
        {
            //Debug.DrawRay(transform.position, playerTransform.position - transform.position, Color.red);
            navMeshAgent.destination = playerTransform.position;
            if(Vector3.Distance(transform.position, playerTransform.position) <= attackRange && Time.time > attackTimeStamp + attackCooldown)
            {
                attackTimeStamp = Time.time;
                GameObject currentProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                Vector3 projectileDirection = (playerTransform.position - transform.position).normalized;
                //projectileDirection = new Vector3(projectileDirection.x, this.transform.position.y, projectileDirection.z);
                currentProjectile.GetComponent<Rigidbody>().AddForce(projectileDirection * 600, ForceMode.Force);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.yellow);
        }

        if (Vector3.Distance(transform.position, playerTransform.position) > 50)
            navMeshAgent.speed = 0;
        else
            navMeshAgent.speed = CheckForSpeedInWater();
    }

    public float CheckForSpeedInWater()
    {
        if (inWater == true)
        {
            currentSpeed = baseSpeed - 2.5f;
            return currentSpeed;
        }
        else
        {
            currentSpeed = baseSpeed;
            return currentSpeed;
        }
    }
}
