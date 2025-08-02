using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class ElementalStateController : StateController
{
    [Tooltip("Attack prefab to use.")]
    [SerializeField] private GameObject triangleAttack;
    [Tooltip("Attack prefab to use.")]
    [SerializeField] private GameObject burstAttack;
    [Tooltip("The range the enemy will try to be at.")]
    [SerializeField] protected float idealRange = 10f;
    [Tooltip("Plus/minus how wide the ideal rage is.")]
    [SerializeField] protected float idealRangeVariance = 2.0f;
    [Tooltip("At what range is the enemy too close.")]
    [SerializeField] protected float closeRange = 5f;

    #region States
    public ElementalState currentState;
    public ElementalWalkState walkState = new ElementalWalkState();
    public ElementalTriangleAttackState triangleAttackState = new ElementalTriangleAttackState();
    public ElementalBurstAttackState burstAttackState = new ElementalBurstAttackState();
    public ElementalDeathState deathState = new ElementalDeathState();
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
        ChangeState(walkState);
    }

    void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    public void ChangeState(ElementalState newState)
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

        walkState.idealAttackRange = idealRange;
        walkState.idealRangeVariance = idealRangeVariance;
        walkState.maxAttackRange = maxAttackRange;
        walkState.closeAttackRange = closeRange;
        walkState.attackCooldown = attackCooldown;
        walkState.randomRadius = randomRadius;
        triangleAttackState.triangleAttack = triangleAttack;
        burstAttackState.burstAttack = burstAttack;
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
}
