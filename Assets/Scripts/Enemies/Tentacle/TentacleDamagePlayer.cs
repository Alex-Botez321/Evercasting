using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleDamagePlayer : MonoBehaviour
{
    [SerializeField] private float tentacleDamage = 10f;
    [SerializeField] private ElementTable.ElementName chargeElement = ElementTable.ElementName.Fire;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health>().Damage(tentacleDamage, (int)chargeElement);
        }
    }
}
