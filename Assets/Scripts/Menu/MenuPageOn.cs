using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPageOn : MonoBehaviour
{
    [SerializeField] private Material material;

    public void OnClick()
    {
        material.SetFloat("_PageFloat", 1);
    }

    public void OnBack()
    {
        material.SetFloat("_PageFloat", 0);
    }
}
