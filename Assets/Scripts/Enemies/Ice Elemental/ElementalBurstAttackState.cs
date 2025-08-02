using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElementalBurstAttackState : ElementalState
{
    public GameObject burstAttack;

    public override void OnEnter(ElementalStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        base.OnEnter(StateController, Self, Player, NavMeshAgent, Animator, AudioManager);
        
        animator.SetInteger("Attacks", 1);
        audioManager.PlayAttack1();
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
        GameObject attack = GameObject.Instantiate(burstAttack, self.transform.position, Quaternion.identity);
        attack.transform.LookAt(player.transform.position);

        for (int i = 0; i < attack.transform.childCount; i++)
        {
            GameObject projectile = attack.transform.GetChild(i).gameObject;
            projectile.GetComponent<Rigidbody>().AddForce(self.transform.forward * 600, ForceMode.Force);
            projectile.GetComponent<ProjectileDamage>().SetVariables(self, 1, 0, 1);
        }
    }
    public override void OnAnimationFinish()
    {
        base.OnAnimationFinish();
        stateController.ChangeState(stateController.walkState);
    }
}
