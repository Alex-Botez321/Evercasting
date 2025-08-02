using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GolemState
{
    protected GolemStateController stateController;
    protected GameObject player;
    protected NavMeshAgent navMeshAgent;


    public virtual void OnEnter(GolemStateController StateController, GameObject Player, NavMeshAgent NavMeshAgent) 
    {
        stateController = StateController;
        player = Player;
        navMeshAgent = NavMeshAgent;

    }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }

}
