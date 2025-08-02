using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneDownAttackState : DroneState
{
    public GameObject projectile;
    public override void OnEnter(DroneStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, NavMeshAgent, Animator, AudioManager);
        animator.SetInteger("Attack", 2);
        navMeshAgent.destination = self.transform.position;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void SpawnAttack()
    {
        GameObject attack = GameObject.Instantiate(projectile, self.transform.position,
            Quaternion.LookRotation(Vector3.down, Vector3.right));

        attack.GetComponent<Rigidbody>().AddForce(self.transform.forward * 100, ForceMode.Force);
        attack.GetComponent<DownAttackProjectile>().owner = self;
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
