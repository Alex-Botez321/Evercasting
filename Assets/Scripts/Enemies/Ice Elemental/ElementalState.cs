using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class ElementalState
{
    protected ElementalStateController stateController;
    protected GameObject self;
    protected GameObject player;
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;
    protected EnemyAudioManager audioManager;

    public virtual void OnEnter(ElementalStateController StateController, GameObject Self, GameObject Player, NavMeshAgent NavMeshAgent, Animator Animator, EnemyAudioManager AudioManager)
    {
        stateController = StateController;
        self = Self;
        player = Player;
        navMeshAgent = NavMeshAgent;
        animator = Animator;
        audioManager = AudioManager;
    }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
    public virtual void OnAnimationFinish() { }

    public virtual void SpawnAttack() 
    {
        
    }


    /// <summary>
    /// Returns a random vector 3 within a the a radius of the given position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public virtual Vector3 RandomNavmeshLocation(Vector3 position, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

}
