using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageTurn : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void OnTeleport()
    {
        animator.SetBool("canTurn", true);
    }

    public void OnAnimFinished()
    {
        animator.SetBool("canTurn", false);
    }
}
