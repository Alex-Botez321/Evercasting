using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToDeactivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("ENTEREEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            objectToDeactivate.SetActive(false);

        }

    }
}
