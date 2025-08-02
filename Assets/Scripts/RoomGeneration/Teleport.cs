using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling.LowLevel;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Animations;

public class Teleport : MonoBehaviour, IInteractable
{
    private BoxCollider m_BoxCollider;
    [SerializeField] public Transform teleportDestination;
    [SerializeField] private AudioClip[] teleportSFX;
    private GameObject player;
    private GameObject barrier;
    public bool canTeleport = true;
    public bool locked = false;
    private AudioSource audioSource;

    private void Awake()
    {
        player = GameObject.Find("Player");
        barrier = GameObject.Find("Barrier");
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        player.transform.position = teleportDestination.position;
    }
    

    private void FixedUpdate()
    {
        if (teleportDestination == null)
        {
            Quaternion currentRotation = this.transform.rotation;
            currentRotation.eulerAngles = new Vector3 (currentRotation.eulerAngles.x, currentRotation.eulerAngles.y+90, currentRotation.eulerAngles.z);
            Instantiate(barrier, this.transform.position, currentRotation); 
            Destroy(this.gameObject);
        }
        else
            Debug.DrawRay(transform.position, teleportDestination.position - transform.position, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(canTeleport && !locked)
            {
                teleportDestination.gameObject.GetComponent<Teleport>().canTeleport = false;
                teleportDestination.gameObject.GetComponentInParent<RoomData>().PlayerEntered();
                player.transform.position = teleportDestination.position;
                int soundClip = UnityEngine.Random.Range(0, teleportSFX.Length);
                audioSource.PlayOneShot(teleportSFX[soundClip]);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            canTeleport = true;
    }
}