using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleAI : MonoBehaviour
{
    //no I cba to do a state machine, it's just 2 attacks and a cooldown

    private Animator animator;
    [SerializeField] private float meleeRange;
    [SerializeField] private EnemyAudioManager audioManager;
    [SerializeField] private float attackCooldow = 0f;
    private float attackTimeStamp;
    private GameObject player;
    [SerializeField] private GameObject projectileSpawnPoint;
    [SerializeField] private GameObject projectile;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        audioManager = GetComponent<EnemyAudioManager>();
        player = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        transform.LookAt(player.transform.position);
        if (Time.time > attackTimeStamp+attackCooldow)
        {
            attackTimeStamp = Time.time;
            if (Vector3.Distance(transform.position, player.transform.position) <= meleeRange)
            {
                animator.SetInteger("Attack", 1);
            }
            else 
            {
                animator.SetInteger("Attack", 2);
            }
        }
    }

    public void SpawnProjectile()
    {
        GameObject attack = GameObject.Instantiate(projectile, projectileSpawnPoint.transform.position, Quaternion.identity);
        attack.transform.LookAt(player.transform.position);

        for (int i = 0; i < attack.transform.childCount; i++)
        {
            GameObject projectile = attack.transform.GetChild(i).gameObject;
            projectile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            projectile.GetComponent<Rigidbody>().AddForce((projectileSpawnPoint.transform.position - player.transform.position) * -70, ForceMode.Force);
            projectile.GetComponent<ProjectileDamage>().SetVariables(this.gameObject, 0.75f, 0, 1);
        }

        audioManager.PlayAttack2();
    }

    public void OnAnimationFinised()
    {
        animator.SetInteger("Attack", 0);
    }

    public void DeleteSelf()
    {
        //i'm tiered bad code ik
        Destroy(this.gameObject);
    }
}
