using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class WindStateController : StateController
{
    [Tooltip("Attack prefab to use.")]
    [SerializeField] private GameObject multipleAttack;
    [Tooltip("Attack prefab to use.")]
    [SerializeField] private GameObject singleAttack;
    [Tooltip("The range the enemy will try to be at.")]
    [SerializeField] protected float meleeRange = 5f;
    [SerializeField] protected float teleportCooldown = 6f;
    [SerializeField] protected GameObject meleeHitbox;
    [SerializeField] protected Transform projectileSpawnPoint;
    [SerializeField] protected float projectileSpeed = 600f;

    #region States
    public WindState currentState;
    public WindIdleState idleState = new WindIdleState();
    public WindMeleeAttackState windMeleeState = new WindMeleeAttackState();
    public WindSingleAttackState windSingleAttackState = new WindSingleAttackState();
    public WindMultipleAttackState windMultipleAttackState = new WindMultipleAttackState();
    public WindDeathState deathState = new WindDeathState();
    public FancyAttackState fancyAttackState = new FancyAttackState();
    #endregion

    private void Awake()
    {
        player = GameObject.Find("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        InitializeStateVariables();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        audioManager = GetComponent<EnemyAudioManager>();
    }

    public void Start()
    {
        ChangeState(idleState);
    }

    void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    public void ChangeState(WindState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = newState;
        currentState.OnEnter(this, this.gameObject, player, navMeshAgent, animator, audioManager);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if(health.currentHealth <= 0)
        {
            ChangeState(deathState);
        }
    }

    private void InitializeStateVariables()
    {
        //to do initialize the on enter variables

        idleState.meleeRange = meleeRange;
        idleState.maxAttackRange = maxAttackRange;
        idleState.attackCooldown = attackCooldown;
        idleState.randomRadius = randomRadius;
        idleState.teleportCooldown = teleportCooldown;
        windMeleeState.hitbox = meleeHitbox;
        windMultipleAttackState.projectile = multipleAttack;
        windSingleAttackState.projectile = singleAttack;
        windMultipleAttackState.projectileSpawnPoint = projectileSpawnPoint;
        windSingleAttackState.projectileSpawnPoint = projectileSpawnPoint;
        windMultipleAttackState.projectileSpeed = projectileSpeed;
        windSingleAttackState.projectileSpeed = projectileSpeed;

        fancyAttackState.projectile = singleAttack;
        fancyAttackState.projectileSpawnPoint = projectileSpawnPoint;
        fancyAttackState.projectileSpeed = projectileSpeed;
    }

    public override void OnAnimationFinish()
    {
        base.OnAnimationFinish();
        currentState.OnAnimationFinish();
    }

    public override void AttackTrigger()
    {
        base.AttackTrigger();
        currentState.SpawnAttack();
    }

    public void StartFancyAttack()
    {
        StartCoroutine(FancyAttackWait());
    }

    IEnumerator FancyAttackWait()
    {
        for(int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(0.1f);
            fancyAttackState.StartAttack(i);
        }
        yield return new WaitForSeconds(5f);
        ChangeState(idleState);
    }
}
