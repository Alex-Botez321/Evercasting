using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OpenPause : MonoBehaviour
{
    [SerializeField] private GameObject Pause;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject selectObject;
    [SerializeField] private GameObject otherSelectObject;
    private GameObject lastButtonSelected;

    private void Awake()
    {
        eventSystem.SetSelectedGameObject(selectObject);
        lastButtonSelected = eventSystem.currentSelectedGameObject;

        StartCoroutine(FixMenus());
    }

    IEnumerator FixMenus()
    {
        OnPause();

        yield return new WaitForFixedUpdate();

        OnPause();
    }

    public void OnPause()
    {
        if(Pause.activeSelf)
        {
            Pause.SetActive(false);
            eventSystem.SetSelectedGameObject(selectObject);
            lastButtonSelected = eventSystem.currentSelectedGameObject;
        }
        else
        {
            Pause.SetActive(true);
            eventSystem.SetSelectedGameObject(otherSelectObject);
            lastButtonSelected = eventSystem.currentSelectedGameObject;
        }
    }

    public void OnControllerChanged(PlayerInput input)
    {

        if (input.currentControlScheme.ToLower() == "gamepad")
        {
            eventSystem.SetSelectedGameObject(lastButtonSelected);
        }
        else
        {
            lastButtonSelected = eventSystem.currentSelectedGameObject;
            eventSystem.SetSelectedGameObject(null);
        }
    }
}
