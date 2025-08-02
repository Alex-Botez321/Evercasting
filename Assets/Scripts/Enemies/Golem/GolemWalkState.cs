using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemWalkState : GolemState
{
    public float attackRange;
    public float attackCooldown;
    public float attackTimeStamp;

    private int layerMask;

    public override void OnEnter(GolemStateController StateController, GameObject Player, NavMeshAgent NavMeshAgent)
    {
        base.OnEnter(StateController, Player, NavMeshAgent);

        layerMask = ~(LayerMask.GetMask("Agents", "Projectile"));
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Vector3 selfPosition = stateController.transform.position;
        Vector3 playerPosition = player.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(selfPosition, playerPosition - selfPosition, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(selfPosition, playerPosition - selfPosition, Color.red);
            navMeshAgent.destination = playerPosition;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
