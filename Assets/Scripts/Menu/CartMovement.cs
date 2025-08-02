using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMovement : MonoBehaviour
{
    [SerializeField] private float cartSpeed = 5f;

    private void FixedUpdate()
    {
        if (Input.anyKey)
        {
            GetComponent<CinemachineDollyCart>().m_Speed = cartSpeed;
            Destroy(this);
        }
    }
}
