using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ElementTable.ElementName magicType;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    public void Interact()
    {
        //player.GetComponent<PlayerController>().Element = Element;

        //setting the player's spell
        //player.GetComponent<PlayerController>().element = magicType;
    }
}
