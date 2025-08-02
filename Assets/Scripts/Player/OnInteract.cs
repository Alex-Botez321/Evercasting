using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnInteract : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;
    /// <summary>
    /// When Interact has been pressed get objects in range and call interact method
    /// </summary>
    /// <param name="context"></param>
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Collider[] InteractableCollidersArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach(Collider Interactable in InteractableCollidersArray)
            {
                if(Interactable.TryGetComponent(out Teleport teleport))
                    teleport.Interact();
                else if(Interactable.TryGetComponent(out MagicPickup magicPickup))
                    magicPickup.Interact();
            }
        }
    }
}
