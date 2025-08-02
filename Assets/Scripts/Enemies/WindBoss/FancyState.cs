using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FancyAttackState : WindState
{
    public GameObject projectile;
    public Transform projectileSpawnPoint;
    public float projectileSpeed;
    public List<Vector3> targets;

    public override void OnEnter(WindStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, NavMeshAgent, Animator, AudioManager);

        targets = new List<Vector3>();

        animator.SetInteger("Attack", 0);
        //audioManager.PlayAttack1();
        LayerMask layerMask = ~(LayerMask.GetMask("Agents", "Projectile", "Wall"));
        Vector3 position = self.transform.position;
        Vector3 playerPosition = player.transform.position;
        RaycastHit hit;
        float distanceToPlayer = Vector3.Distance(position, playerPosition);
        //Face agent in direction of player or movement depending on line of sight
        stateController.gameObject.transform.LookAt(playerPosition);

        for(int i = 0; i < 8; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * 15 + playerPosition;
            NavMeshHit location;
            NavMesh.SamplePosition(randomPosition, out location, 15, 1);
            targets.Add(location.position);
        }
        stateController.StartFancyAttack();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public void StartAttack(int i)
    {
        self.transform.position = targets[i];

        GameObject attack = GameObject.Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
        attack.transform.LookAt(player.transform.position);

        for (int j = 0; j < attack.transform.childCount; j++)
        {
            GameObject projectile = attack.transform.GetChild(j).gameObject;
            projectile.GetComponent<Rigidbody>().AddForce((projectileSpawnPoint.position - player.transform.position) * (projectileSpeed + 35), ForceMode.Force);
            projectile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            projectile.GetComponent<ProjectileDamage>().SetVariables(self, 0.75f, 0, 1);
            projectile.GetComponent<ProjectileDamage>().canCollide = true;
        }

    }
    public override void OnAnimationFinish()
    {
        base.OnAnimationFinish();
        stateController.ChangeState(stateController.idleState);
    }
}
