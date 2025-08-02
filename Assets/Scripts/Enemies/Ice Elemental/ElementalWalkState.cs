using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElementalWalkState : ElementalState
{
    public float attackRange;
    public float attackCooldown;
    private float attackTimeStamp = 0f;
    public float idealAttackRange;
    public float maxAttackRange;
    public float closeAttackRange;
    public float idealRangeVariance;
    private int layerMask;
    public float randomRadius;

    public override void OnEnter(ElementalStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, NavMeshAgent, Animator, AudioManager);
        navMeshAgent.isStopped = true;
        layerMask = ~(LayerMask.GetMask("Agents", "Projectile"));
        animator.SetInteger("Attacks", 0);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Vector3 position = self.transform.position;
        Vector3 playerPosition = player.transform.position;
        RaycastHit hit;
        float distanceToPlayer = Vector3.Distance(position, playerPosition);
        //Face agent in direction of player or movement depending on line of sight
        if (Physics.Raycast(position, playerPosition - position, out hit, Mathf.Infinity, layerMask))
        {
            if(hit.transform.gameObject != player)
            {
                Debug.DrawRay(position, playerPosition - position, Color.red);
                stateController.gameObject.transform.LookAt(navMeshAgent.nextPosition);
                navMeshAgent.destination = playerPosition;
                navMeshAgent.isStopped = false;
                return;
            }

            Debug.DrawRay(position, playerPosition - position, Color.green);
            stateController.gameObject.transform.LookAt(playerPosition);
            //checking if enemy is within ideal attack range
            if (distanceToPlayer < idealAttackRange + idealRangeVariance && distanceToPlayer > idealAttackRange - idealRangeVariance)
            {
                AttemptAttack(distanceToPlayer);
                navMeshAgent.isStopped = true;
            }
            else if(distanceToPlayer < idealAttackRange - idealRangeVariance)
            {
                
                navMeshAgent.isStopped = false;
                Vector3 searchArea = -self.transform.forward * (idealAttackRange - distanceToPlayer) + position;
                navMeshAgent.destination = RandomNavmeshLocation(searchArea, randomRadius);
                AttemptAttack(distanceToPlayer);
            }
            else if(distanceToPlayer > idealAttackRange + idealRangeVariance)
            {
                navMeshAgent.isStopped = false;
                Vector3 searchArea = self.transform.forward * (distanceToPlayer - idealAttackRange) + position;
                navMeshAgent.destination = RandomNavmeshLocation(searchArea, randomRadius);
                AttemptAttack(distanceToPlayer);
            }
            else if (distanceToPlayer > maxAttackRange)
            {
                navMeshAgent.destination = playerPosition;
                navMeshAgent.isStopped = false;
                
            }

            Debug.DrawRay(position, stateController.gameObject.transform.forward, Color.black, 0.2f);
        }        
    }

    private void AttemptAttack(float distanceToPlayer)
    {
        if(attackTimeStamp + attackCooldown <= Time.time)
        {
            attackTimeStamp = Time.time;
            float normalisedRange = distanceToPlayer/maxAttackRange;
            float randomAttack = Random.Range(0f, 1f);
            if(randomAttack<=normalisedRange)
            {
                stateController.ChangeState(stateController.triangleAttackState);
            }
            else
            {
                stateController.ChangeState(stateController.burstAttackState);
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        navMeshAgent.isStopped = false;
    }
}
