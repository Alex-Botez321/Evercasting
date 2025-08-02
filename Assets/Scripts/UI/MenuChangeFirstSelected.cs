using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuChangeFirstSelected : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject selectObject;
    public void onClick()
    {
        eventSystem.currentSelectedGameObject.ToString();
        eventSystem.SetSelectedGameObject(selectObject);
    }

    //private void OnEnable()
    //{
    //    eventSystem.firstSelectedGameObject = selectObject;
    //}
}
