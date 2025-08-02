using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class DroneDeathState : DroneState
{
    public override void OnEnter(DroneStateController StateController, GameObject Self, GameObject Player, NavMeshAgent navMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, navMeshAgent, Animator, AudioManager);
        //AudioManager.PlayDeath();
        animator.SetBool("IsDead", true);
        navMeshAgent.destination = self.transform.position;
        //stateController.GetComponentInParent<RoomData>().EnemyDead();
        audioManager.PlayDeath();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnAnimationFinish()
    {
        base.OnAnimationFinish();
    }
}
