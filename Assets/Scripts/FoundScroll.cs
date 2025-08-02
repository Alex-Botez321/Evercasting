using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundScroll : MonoBehaviour
{
    [SerializeField] private ScrollManager scrollManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            scrollManager.EnableChoiceScrolls();
            Destroy(this);
        }
    }
    // Start is called before the first frame update
}
