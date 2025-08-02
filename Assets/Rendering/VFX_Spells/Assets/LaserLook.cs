using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;  // The target object to look at

    void Update()
    {
        // If the target is not null, make the object look at the target
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}