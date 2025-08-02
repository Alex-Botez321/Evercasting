    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class InputCheck : MonoBehaviour
{
    [SerializeField] public GameObject ControlerTrue;
    [SerializeField] public GameObject KeyboardTrue;
    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.ToLower() == "gamepad")
        {
            ControlerTrue.SetActive(true);
            KeyboardTrue.SetActive(false);
        }
        else
        {
            ControlerTrue.SetActive(false);
            KeyboardTrue.SetActive(true);
        }
    }
    void Start()
    {
        //if (input.currentControlScheme.ToLower() == "gamepad")
        {

        }
    }
}
