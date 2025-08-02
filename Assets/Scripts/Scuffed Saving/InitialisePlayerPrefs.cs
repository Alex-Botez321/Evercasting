using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialisePlayerPrefs : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt("count", 0);
    }
}
