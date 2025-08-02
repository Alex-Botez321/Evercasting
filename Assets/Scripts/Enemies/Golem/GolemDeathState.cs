using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemDeathState : GolemState
{
    public override void OnEnter(GolemStateController StateController, GameObject Player, NavMeshAgent NavMeshAgent)
    {
        base.OnEnter(StateController, Player, NavMeshAgent);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
