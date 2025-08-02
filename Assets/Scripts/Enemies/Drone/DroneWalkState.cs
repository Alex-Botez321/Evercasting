using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneWalkState : DroneState
{
    public float attackRange;
    public float attackCooldown;
    public float attackTimeStamp = -3f;
    public float maxAttackRange;
    public float coneAttackRange;
    private int layerMask;
    public float randomRadius;
    public float normalSpeed;

    public override void OnEnter(DroneStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, NavMeshAgent, Animator, AudioManager);
        navMeshAgent.isStopped = true;
        layerMask = ~(LayerMask.GetMask("Agents", "Projectile"));
        animator.SetInteger("Attack", 0);
        navMeshAgent.speed = normalSpeed;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Vector3 position = self.transform.position;
        Vector3 playerPosition = player.transform.position;
        RaycastHit hit;
        stateController.transform.LookAt(navMeshAgent.nextPosition);
        float distanceToPlayer = Vector3.Distance(position, playerPosition);
        //Face agent in direction of player or movement depending on line of sight
        if (Physics.Raycast(position, playerPosition - position, out hit, Mathf.Infinity, layerMask))
        {
            stateController.gameObject.transform.LookAt(playerPosition);
            if (hit.transform.gameObject != player)
            {
                Debug.DrawRay(position, playerPosition - position, Color.red);
                stateController.gameObject.transform.LookAt(navMeshAgent.nextPosition);
                navMeshAgent.destination = playerPosition;
                navMeshAgent.isStopped = false;
                return;
            }

            Debug.DrawRay(position, playerPosition - position, Color.green);
            stateController.gameObject.transform.LookAt(playerPosition);

            if(Time.time > attackTimeStamp + attackCooldown)
            {
                if (distanceToPlayer > maxAttackRange)
                    stateController.ChangeState(stateController.chargeState);
                else if (distanceToPlayer > coneAttackRange)
                    stateController.ChangeState(stateController.coneAttackState);
                else
                    stateController.ChangeState(stateController.downAttackState);
            }
            else
            {
                navMeshAgent.destination = playerPosition;
            }
            Debug.DrawRay(position, stateController.gameObject.transform.forward, Color.black, 0.4f);
            Debug.DrawRay(position, stateController.gameObject.transform.GetChild(0).transform.forward, Color.magenta, 0.4f);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        navMeshAgent.isStopped = false;
    }
}
