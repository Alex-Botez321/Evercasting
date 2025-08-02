using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DamascusRigidGlob : MonoBehaviour
{
    [SerializeField] private GameObject globule;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            RaycastHit groundHit;
            if (Physics.Raycast(position, -Vector3.up, out groundHit, 100))
            {
                Instantiate(globule, this.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
            
        }
    }
}
