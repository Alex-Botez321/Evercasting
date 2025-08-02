using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleAnimationTriggers : MonoBehaviour
{
    private TentacleAI tentacle;
    private EnemyAudioManager audioManager;

    private void Awake()
    {
        tentacle = GetComponentInParent<TentacleAI>();
        audioManager = GetComponentInParent<EnemyAudioManager>();
    }

    private void SpawnAttack()
    {
        tentacle.SpawnProjectile();
    }

    private void OnAnimFinished()
    {
        tentacle.OnAnimationFinised();
    }

    private void OnDeath()
    {
        tentacle.DeleteSelf();
    }

    private void OnSlam()
    {
        audioManager.PlayAttack1();
    }

}
