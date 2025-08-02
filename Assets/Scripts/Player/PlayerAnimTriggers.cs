using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimTriggers : MonoBehaviour
{
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    public void DeathTrigger() { SceneManager.LoadScene("Game Over"); }
    public void AttackRelease() 
    {
        controller.ReleaseAttack();
    }

    public void StartCharge()
    {
        controller.StartChargeCoroutine();
    }
    public void Footstep() { }
}