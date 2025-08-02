using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemStateController : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 3f;
    private float attackTimeStamp;

    [SerializeField] private GameObject projectile;

    #region States
    private GolemState currentState;
    private GolemWalkState GolemWalkState = new GolemWalkState();
    private GolemLaserState GolemLaserState = new GolemLaserState();
    private GolemWindmillState GolemWindmillState = new GolemWindmillState();
    private GolemDeathState GolemDeathState = new GolemDeathState();
    #endregion

    private void Awake()
    {
        player = GameObject.Find("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        InitializeStateVariables();
    }
    void Start()
    {
        ChangeState(GolemWalkState);
    }

    void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    public void ChangeState(GolemState newState)
    {
        if(currentState != null)
        {
            currentState.OnExit();
        }
        currentState = newState;
        currentState.OnEnter(this, player, navMeshAgent);
    }

    /// <summary>
    /// Initializing state specific variables
    /// </summary>
    private void InitializeStateVariables()
    {
        GolemWalkState.attackRange = attackRange;
        GolemWalkState.attackCooldown = attackCooldown;
        GolemWalkState.attackTimeStamp = attackTimeStamp;

        GolemLaserState.projectile = projectile;
    }
}