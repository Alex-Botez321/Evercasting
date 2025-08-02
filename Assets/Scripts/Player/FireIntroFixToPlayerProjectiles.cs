using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireIntroFixToPlayerProjectiles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdatePlayerStats());
    }

    IEnumerator UpdatePlayerStats()
    {
        yield return new WaitForSeconds(1);
        GetComponent<ScrollManager>().UpdateAtributes();
    }
}
