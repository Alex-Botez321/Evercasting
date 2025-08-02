using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void EndAll()
    {
        Application.Quit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.name == "Player")
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
