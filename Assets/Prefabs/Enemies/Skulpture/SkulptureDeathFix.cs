using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulptureDeathFix : MonoBehaviour
{
    private ElementalStateController stateController;

    private void Awake()
    {
        stateController = GetComponent<ElementalStateController>();
    }

    private void FixedUpdate()
    {
        if(stateController.currentState == stateController.deathState)
        {
            Destroy(this.gameObject);
        }
    }
}
