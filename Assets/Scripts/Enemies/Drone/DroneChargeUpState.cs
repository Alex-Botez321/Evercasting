using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneChargeUpState : DroneState
{
    public float chargeUpTime;
    private float startTime;
    public override void OnEnter(DroneStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, NavMeshAgent, Animator, AudioManager);
        animator.SetBool("AttackFinished", false);
        animator.SetBool("ChargeFinished", false);
        animator.SetInteger("Attack", 1);
        navMeshAgent.speed = 0;
        startTime = Time.time;
        //drone doesn't move need to heck destination
        //speed also doesn't update
    }


    public override void OnExit()
    {
        base.OnExit();
        animator.SetBool("AttackFinished", false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        if (Time.time > startTime + chargeUpTime) 
        { 
            stateController.ChangeState(stateController.chargingState);
            animator.SetBool("ChargeFinished", true);
        }
    }

    public override void OnAnimationFinish()
    {
        base.OnAnimationFinish();
    }
}