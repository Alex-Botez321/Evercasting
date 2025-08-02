using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToggleMap : MonoBehaviour
{
    private Mask mask;
    private void Awake()
    {
        mask = GetComponent<Mask>();
    }

    public void OnMapToggle(InputAction.CallbackContext context)
    {
        if (context.started)
            mask.enabled = !mask.enabled;
    }
}
