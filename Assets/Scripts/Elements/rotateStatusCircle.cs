using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateStatusCircle : MonoBehaviour
{
    [SerializeField] private Transform positiveRotate;
    [SerializeField] private Transform negativeRotate;
    [SerializeField] private float rotateSpeed;

    private void Start()
    {
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            positiveRotate.eulerAngles += new Vector3(0, rotateSpeed, 0);
            negativeRotate.eulerAngles -= new Vector3(0, rotateSpeed, 0);
            yield return new WaitForFixedUpdate();
        }
    }
}