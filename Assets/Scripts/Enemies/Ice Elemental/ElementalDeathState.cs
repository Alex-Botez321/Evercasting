using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElementalDeathState : ElementalState
{
    public override void OnEnter(ElementalStateController StateController, GameObject Self, GameObject Player, NavMeshAgent navMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, navMeshAgent, Animator, AudioManager);
        AudioManager.PlayDeath();
        animator.SetBool("IsDead", true);
        navMeshAgent.destination = self.transform.position;
        //stateController.GetComponentInParent<RoomData>().EnemyDead();
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
