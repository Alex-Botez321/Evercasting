using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ReactionTextPopUp : MonoBehaviour
{
    private Text displayText;
    [SerializeField] public GameObject reactionTextPrefab;
    [SerializeField] private Camera mainCamera;

    [SerializeField] float flySpeed = 0.1f;
    [SerializeField]float textLifetime = 2f;
    bool textActive = false;


    //private void Update()
    //{
    //    //displayText.rectTransform.localPosition += new Vector3(0, flySpeed, 0);
    //    displayText.transform.position += new Vector3(0, flySpeed, 0);
    //}

    private void Start()
    {
        Screen.SetResolution(3840, 2160, true);
    }
    public void SetText(string reactionText, Vector3 position)
    {
        //Vector3 TextPosition = new Vector3(position.x, 2f, position.z);
        position = new Vector3(position.x, position.y - 10, position.z);
        GameObject newTextPopup = Instantiate(reactionTextPrefab, position, Quaternion.identity, GameObject.Find("DisplayTextCanvas").transform);
        newTextPopup.transform.SetParent(this.gameObject.transform, true);
        Debug.Log(position);

        displayText = newTextPopup.GetComponent<Text>();
        displayText.text = reactionText;
        displayText.rectTransform.LookAt(transform.position - Camera.main.transform.position, Camera.main.transform.up);
        displayText.rectTransform.rotation = Quaternion.Euler(5f, 90f, 0f);
        textActive = true;
        StartCoroutine(TextRise(displayText, position));

        Destroy(newTextPopup, textLifetime);
    }

    private IEnumerator TextRise(Text displayText, Vector3 vector3)
    {
        while (displayText)
        {
            displayText.transform.position += new Vector3(0, flySpeed, 0) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    
}
