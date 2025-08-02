using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FadeInButtons : MonoBehaviour
{
    [SerializeField] private List<Image> images;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private EventSystem eventSystem;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Main Camera")
        {
            mainButtons.SetActive(true);
            for(int i = 0; i < images.Count; i++)
            {
                StartCoroutine(FadeIn(images[i]));
            }
        }
    }

    IEnumerator FadeIn(Image image)
    {
        while (image.color.a < 250)
        {
            Color tempColour = image.color;
            tempColour.a += fadeSpeed * Time.deltaTime;
            image.color = tempColour;

            yield return new WaitForFixedUpdate();
        }
    }
}
