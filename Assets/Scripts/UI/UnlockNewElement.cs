using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockNewElement : MonoBehaviour
{
    [SerializeField] private ElementTable.ElementName unlockElement;

    public void OnClick()
    {
        Debug.Log(unlockElement);
        PlayerPrefs.SetInt("newElement", (int)unlockElement);
        GetComponent<LoadNextScene>().LoadScene();
    }
}
