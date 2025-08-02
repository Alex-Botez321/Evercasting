using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsTimer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Credits());
    }
    IEnumerator Credits()
    {
        yield return new WaitForSeconds(35);
        SceneManager.LoadScene("MainMenu");
        //SceneManager.GetActiveScene().buildIndex - 10
    }
}
