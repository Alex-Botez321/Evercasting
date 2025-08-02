using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WindIdleState : WindState
{
    public float attackRange;
    public float attackCooldown;
    private float attackTimeStamp = 0f;
    public float meleeRange;
    public float maxAttackRange;
    private int layerMask;
    public float randomRadius;
    private float teleportTimestamp;
    public float teleportCooldown;

    public override void OnEnter(WindStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, NavMeshAgent, Animator, AudioManager);
        navMeshAgent.isStopped = true;
        layerMask = ~(LayerMask.GetMask("Agents", "Projectile"));
        animator.SetInteger("Attack", 0);
        attackTimeStamp = Time.time;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Vector3 position = self.transform.position;
        Vector3 playerPosition = player.transform.position;
        RaycastHit hit;
        float distanceToPlayer = Vector3.Distance(position, playerPosition);
        //Face agent in direction of player or movement depending on line of sight
        stateController.gameObject.transform.LookAt(playerPosition);

        Debug.DrawRay(position, playerPosition - position, Color.green, 5);

        if (Physics.Raycast(position, playerPosition - position, out hit, Mathf.Infinity, layerMask))
        {
            if(hit.transform.gameObject != player || Time.time > teleportCooldown + teleportTimestamp)
            {
                Debug.DrawRay(position, position - hit.point, Color.red, 10);

                Debug.Log(hit.transform.gameObject.name);

                Vector3 randomPosition = Random.insideUnitSphere * maxAttackRange + playerPosition;
                NavMeshHit location;
                NavMesh.SamplePosition(randomPosition, out location, maxAttackRange, 1);
                if(Physics.Raycast(location.position, Vector3.down, Mathf.Infinity))
                {
                    self.transform.position = location.position;
                    audioManager.PlayTeleport();
                    teleportTimestamp = Time.time;
                    return;
                }
            }

            AttemptAttack(distanceToPlayer);  

            Debug.DrawRay(position, stateController.gameObject.transform.forward, Color.black, 0.2f);
        }        
    }

    private void AttemptAttack(float distanceToPlayer)
    {
        if(attackTimeStamp + attackCooldown <= Time.time)
        {
            float randomAttack = Random.Range(0f, 1f);
            if (Vector3.Distance(player.transform.position, self.transform.position) <= meleeRange)
            {
                stateController.ChangeState(stateController.windMeleeState);
            }
            else if(randomAttack <= 0.05f)
            {
                stateController.ChangeState(stateController.fancyAttackState);
            }
            else if(randomAttack<= 0.55f)
            {
                stateController.ChangeState(stateController.windSingleAttackState);
            }
            else
            {
                stateController.ChangeState(stateController.windMultipleAttackState);
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
