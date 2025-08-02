using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WindMeleeAttackState : WindState
{
    public GameObject hitbox;

    public override void OnEnter(WindStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, NavMeshAgent, Animator, AudioManager);
        hitbox.SetActive(true);
        animator.SetInteger("Attack", 2);
        //audioManager.PlayAttack2();
        
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void SpawnAttack()
    {
        base.SpawnAttack();
        audioManager.PlayAttack1();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnAnimationFinish()
    {
        base.OnAnimationFinish();
        stateController.ChangeState(stateController.idleState);
        hitbox.SetActive(false);
    }
}
