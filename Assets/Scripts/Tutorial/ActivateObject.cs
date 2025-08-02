using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("ENTEREEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            objectToActivate.SetActive(true);

        }

    }
}
