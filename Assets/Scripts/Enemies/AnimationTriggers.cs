using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    private StateController stateController;
    private void Awake()
    {
        stateController = GetComponentInParent<StateController>();
    }

    public virtual void OnAnimationFinish()
    {
        stateController.OnAnimationFinish();
    }

    public virtual void AttackTrigger()
    {
        stateController.AttackTrigger();
    }

    public virtual void DestroyTrigger()
    {
        stateController.DestroyTrigger();
    }
}
