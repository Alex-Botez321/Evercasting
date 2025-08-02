using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WindSingleAttackState : WindState
{
    public GameObject projectile;
    public Transform projectileSpawnPoint;
    public float projectileSpeed;

    public override void OnEnter(WindStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, NavMeshAgent, Animator, AudioManager);
        
        animator.SetInteger("Attack", 1);
        //audioManager.PlayAttack1();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void SpawnAttack()
    {
        GameObject attack = GameObject.Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
        attack.transform.LookAt(player.transform.position);

        for (int i = 0; i < attack.transform.childCount; i++)
        {
            GameObject projectile = attack.transform.GetChild(i).gameObject;
            projectile.GetComponent<Rigidbody>().AddForce((projectileSpawnPoint.position - player.transform.position) * projectileSpeed, ForceMode.Force);
            projectile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            projectile.GetComponent<ProjectileDamage>().SetVariables(self, 0.75f, 0, 1);
            projectile.GetComponent<ProjectileDamage>().canCollide = true;
            audioManager.PlayAttack1();
        }
    }
    public override void OnAnimationFinish()
    {
        base.OnAnimationFinish();
        stateController.ChangeState(stateController.idleState);
    }
}
