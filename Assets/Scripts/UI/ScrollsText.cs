using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScrollsText : MonoBehaviour
{
    [SerializeField] private float disableTimer = 5f;
    [SerializeField] private string startText;
    [HideInInspector] public string attribute;
    [SerializeField] private TextMeshProUGUI tmp;
    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        tmp.text = startText + " " + attribute;
        StartCoroutine(DisableText());
    }

    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(disableTimer);
        this.gameObject.SetActive(false);
        yield return null;
    }
}
