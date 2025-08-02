using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class DroneConeAttackState : DroneState
{
    public GameObject projectile;
    public override void OnEnter(DroneStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, NavMeshAgent, Animator, AudioManager);
        animator.SetInteger("Attack", 3);
        navMeshAgent.destination = self.transform.position;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void SpawnAttack()
    {
        GameObject attack = GameObject.Instantiate(projectile, self.transform.GetChild(0).transform.position, Quaternion.identity);
        attack.transform.LookAt(player.transform.position);

        for (int i = 0; i < attack.transform.childCount; i++)
        {
            GameObject projectile = attack.transform.GetChild(i).gameObject;
            projectile.GetComponent<Rigidbody>().AddForce((self.transform.GetChild(0).transform.position - player.transform.position) * -50, ForceMode.Force);
            projectile.GetComponent <Rigidbody>().constraints = RigidbodyConstraints.None;
            projectile.GetComponent<ProjectileDamage>().SetVariables(self, 0.75f, 0, 1);
            projectile.GetComponent<ProjectileDamage>().canCollide = true;
        }
        audioManager.PlayAttack1();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnAnimationFinish()
    {
        base.OnAnimationFinish();
        stateController.ChangeState(stateController.walkState);
    }
}
