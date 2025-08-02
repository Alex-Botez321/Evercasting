using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class DroneStateController : StateController
{
    [Tooltip("Attack prefab to use.")]
    [SerializeField] private GameObject downAttackProjectile;
    [Tooltip("Attack prefab to use.")]
    [SerializeField] private GameObject coneAttackProjectile;
    [SerializeField] private float coneAttackRange = 6f;
    [SerializeField] private float chargeUpTime = 1.5f;
    [SerializeField] private float chargeSpeed = 2;
    [SerializeField] private float chargeDamage = 10f;
    [SerializeField] private ElementTable.ElementName chargeElement = ElementTable.ElementName.Lightning;
    [SerializeField] private float normalSpeed = 3.5f;
    private SphereCollider hitbox;
    [SerializeField] private Transform modelTransform;

    #region States
    public DroneState currentState;
    public DroneWalkState walkState = new DroneWalkState();
    public DroneDeathState deathState = new DroneDeathState();
    public DroneChargeUpState chargeState = new DroneChargeUpState();
    public DroneChargingState chargingState = new DroneChargingState();
    public DroneDownAttackState downAttackState = new DroneDownAttackState();
    public DroneConeAttackState coneAttackState = new DroneConeAttackState();
    #endregion

    private void Awake()
    {
        player = GameObject.Find("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        InitializeStateVariables();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        audioManager = GetComponent<EnemyAudioManager>();
        hitbox = GetComponent<SphereCollider>();
        //modelTransform = transform.GetChild(0).transform;
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

        hitbox.center = modelTransform.localPosition;
    }

    public void ChangeState(DroneState newState)
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

        walkState.coneAttackRange = coneAttackRange;
        walkState.maxAttackRange = maxAttackRange;
        walkState.attackCooldown = attackCooldown;
        walkState.randomRadius = randomRadius;
        chargingState.chargeSpeed = chargeSpeed;
        downAttackState.projectile = downAttackProjectile;
        coneAttackState.projectile = coneAttackProjectile;
        walkState.normalSpeed = normalSpeed;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.name == "Player" && currentState == chargingState)
        {
            collision.transform.GetComponent<Health>().Damage(chargeDamage, (int)chargeElement);
        }
    }
}
