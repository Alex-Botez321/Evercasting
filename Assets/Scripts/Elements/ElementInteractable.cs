using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementInteractable : MonoBehaviour
{
    //[SerializeField] GameObject myInteractableObject;
    [SerializeField] ElementTable.ElementName objectElement;
    [SerializeField] private float damageMultiplier = 1f;
    //[SerializeField] ReactionTextPopUp reactionTextCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Projectile")
        {
            other.gameObject.TryGetComponent<ProjectileDamage>(out ProjectileDamage proj);

            if (proj.myElement == objectElement) //&& proj != null)
            {
                Debug.LogWarning("destroy proj");
                Destroy(other);
                Debug.Log("Yup, destroyd");
            }
            else
            {
                if (ElementTable.instance.firstElement == ElementTable.ElementName.Null)
                {
                    //doing the projectile collision routine, but with our object
                    ElementTable.instance.firstElement = objectElement;
                    ElementTable.instance.secondElement = proj.myElement;
                    ElementTable.instance.reactionPosition = this.transform.position;
                    ElementTable.instance.damageMultiplier = proj.GetComponent<ProjectileDamage>().multiplier;
                    ElementTable.instance.ElementChecker();
                    Destroy(other);
                    Destroy(this.gameObject);
                }
                else
                {
                    //doing the projectile collision routine, but with our object
                    ElementTable.instance.firstElement = objectElement;
                    ElementTable.instance.secondElement = proj.myElement;
                    ElementTable.instance.reactionPosition = this.transform.position;
                    ElementTable.instance.damageMultiplier = proj.GetComponent<ProjectileDamage>().multiplier;
                    ElementTable.instance.ElementChecker();
                    Destroy(other);
                    Destroy(this.gameObject);
                    
                }
            }
            
        }
    }
}
