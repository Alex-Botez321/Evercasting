using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    protected GameObject player;
    public NavMeshAgent navMeshAgent;       //public because water needs access to it
    protected Health health;
    protected Animator animator;

    
    [Tooltip("At what range is the enemy too far.")]
    [SerializeField] protected float maxAttackRange = 15f;
    [Tooltip("Frequency of attacks, timer starts when attack ends.")]
    [SerializeField] protected float attackCooldown = 3f;
    [Tooltip("What percent health value is considered low health.")]
    [SerializeField] protected int lowHealthThreshold;
    [Tooltip("How important is dodging projectiles to the enemy.")]
    [SerializeField] protected float dodgeWeight;
    [Tooltip("How big of a radius should the enemy look for a random location.")]
    protected float randomRadius = 2;
    protected EnemyAudioManager audioManager;

    public virtual void TakeDamage(float damage)
    {
        //audioManager.PlayDamage();
    }

    public virtual void OnAnimationFinish()
    {
        
    }

    public virtual void AttackTrigger()
    {

    }

    public virtual void DestroyTrigger()
    {
        Destroy(this.gameObject);
    }

    public virtual void ChangeSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }
}
