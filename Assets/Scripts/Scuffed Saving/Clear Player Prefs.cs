using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPlayerPrefs : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.DeleteAll();
    }
}
