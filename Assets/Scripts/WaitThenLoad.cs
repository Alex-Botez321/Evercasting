using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitThenLoad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            StartCoroutine(WaitCorutine());
        }
    }
    IEnumerator WaitCorutine()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
