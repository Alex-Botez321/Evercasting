using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTitle : MonoBehaviour
{
    //public void NextScene()
    //{
        //SceneManager.LoadScene("LevelTest");
    //}

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }
}
