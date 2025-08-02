using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class DroneChargingState : DroneState
{
    public float chargeSpeed;
    LayerMask layerMask;

    public override void OnEnter(DroneStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, NavMeshAgent, Animator, AudioManager);

        animator.SetBool("AttackFinished", false);
        layerMask = ~(LayerMask.GetMask("Agents", "Player"));

        RaycastHit hit;
        if (Physics.Raycast(self.transform.position, player.transform.position - self.transform.position, out hit, Mathf.Infinity, layerMask))
        {
            navMeshAgent.destination = hit.point;
            navMeshAgent.speed = chargeSpeed;
        }
        audioManager.PlayAttack2();
    }

    public override void OnExit()
    {
        base.OnExit();
        animator.SetBool("AttackFinished", true);
        stateController.walkState.attackTimeStamp = Time.time;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        if (Vector3.Distance(self.transform.position, navMeshAgent.destination) < 2f)
            stateController.ChangeState(stateController.walkState);
    }

    public override void OnAnimationFinish()
    {
        base.OnAnimationFinish();
    }
}
