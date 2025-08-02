using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class MouseAiming : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float maxCursorDistance = 7f;
    [SerializeField] private Texture2D cursorTexture;

    private Vector3 mousePos;
    private Vector2 controllerValue;
    private CursorMode cursorMode = CursorMode.Auto;
    private bool usingController = false;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cursorPrefab;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, cursorMode);
    }

    private void Start()
    {
        cursorPrefab = Instantiate(cursorPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
    }

    void FixedUpdate()
    {
        if(!usingController)
        {
            mousePos = Input.mousePosition*2;
            Ray mouseRay = playerCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, groundLayerMask))
            {
                transform.position = raycastHit.point;
            }
        }
        else
        {
            Vector3 playerPos = player.transform.position;
            transform.position = new Vector3(
                playerPos.x + controllerValue.y * maxCursorDistance,
                playerPos.y, 
                playerPos.z + -controllerValue.x * maxCursorDistance);

            //Mouse.current.WarpCursorPosition(playerCamera.WorldToScreenPoint(transform.position)*0.5f);
            cursorPrefab.transform.position = playerCamera.GetComponent<Camera>().WorldToScreenPoint(transform.position) * 0.5f;
            Debug.Log(controllerValue);
        }

    }

    public void OnControllerAim(InputAction.CallbackContext context)
    {
        if(context.performed)
            controllerValue = context.ReadValue<Vector2>();
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.ToLower() == "gamepad")
        {
            Cursor.visible = false;
            usingController = true;
            cursorPrefab.SetActive(true);
        }
        else
        {
            cursorPrefab.SetActive(false);
            Cursor.visible = true;
            usingController = false;
        }

        Debug.Log(usingController);
    }
}
