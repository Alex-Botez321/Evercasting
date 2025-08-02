using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.UI.GridLayoutGroup;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField] public float damage = 10;
    [SerializeField] public ElementTable.ElementName myElement;
    //[SerializeField] private ElementReactionLibrary ElementReactionLibrary;
    [SerializeField] private float projectileLifeTime = 1f;
    [SerializeField] private float power = 1f;
    public float multiplier = 1f;
    public const float resistanceBase = 2f;
    public float resistanceBypass = 2;
    private Rigidbody rb;

    public bool canCollide = false;
    public GameObject ownerObject; 

    /// <summary>
    /// Seting up the projectile in acordance to stats of the creator
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="_multiplier"></param>
    /// <param name="lifeTime"></param>
    public void SetVariables(GameObject owner, float _multiplier, float lifeTime, float resistPen)
    {
        ownerObject = owner;
        damage *= _multiplier;
        multiplier = _multiplier;
        projectileLifeTime += lifeTime;
        resistanceBypass = resistanceBase - resistPen;

        StartCoroutine(ToggleCollision());
        StartCoroutine(Countdown());
    }

    IEnumerator ToggleCollision()
    {
        yield return new WaitForFixedUpdate();
        canCollide = true;
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(projectileLifeTime);
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //--------if not owner try health and try projectile else destroy--------//

        //Ensuring no collision with object that instantiated projectile
        //if (!ownerObject & ownerObject != null) <---- to be changed into later!!!!!!

        if (canCollide)
        {
            //
            if(other.transform.parent != transform.parent)
            {
                if (other.tag == "Projectile")
                {
                    //Destroy(GetComponent<SphereCollider>()); //moving this into size comparison below, commenting to remember its initial position incase its integral for it to be here
                    //setting elements for element table
                    if (ElementTable.instance.firstElement == ElementTable.ElementName.Null)
                    {
                        ElementTable.instance.firstElement = myElement;
                        ElementTable.instance.damageMultiplier += multiplier;
                        //for larger projectiles going through smaller one
                        //if statement comparison only happens in one direction becasue both projectiles will call it,
                        //its better for only the loser to destroy themselves and make the other bigger
                        //if (other.gameObject.transform.localScale.x > this.gameObject.transform.localScale.x)
                        //{
                        float scaleDifference = other.gameObject.transform.localScale.x - this.transform.localScale.x;
                        if (scaleDifference > 0.1f) //0.5 is the minimum to avoid size 0.00001 projectiles and such
                        {
                            Destroy(this.gameObject);
                            other.gameObject.transform.localScale = new Vector3(scaleDifference, scaleDifference, scaleDifference);
                        }
                        else if (scaleDifference <= 0.1) //again, only destroying this object because both projectiles will call their own code
                        {
                            //change this
                            scaleDifference = this.gameObject.transform.localScale.x + scaleDifference;
                            this.gameObject.transform.localScale = new Vector3(scaleDifference, scaleDifference, scaleDifference);
                            Destroy(other.gameObject);
                        }

                    }
                    else
                    {
                        ElementTable.instance.secondElement = myElement;
                        ElementTable.instance.reactionPosition = other.transform.position;
                        ElementTable.instance.damageMultiplier += multiplier;
                        //Call element table logic for the collision
                        ElementTable.instance.ElementChecker();
                    }
                }
                else if (other.tag == "Wall")
                    Destroy(this.gameObject);
                else if (other.gameObject.TryGetComponent<Health>(out Health health) && other.gameObject != ownerObject)
                {
                    other.gameObject.GetComponent<Health>().Damage(damage, myElement, resistanceBypass);
                    Destroy(this.gameObject);
                }
                else if (other.gameObject.GetComponentInParent<Health>() && other.gameObject != ownerObject)
                {
                    //just for the tentacle to take damage
                    other.gameObject.GetComponentInParent<Health>().Damage(damage, myElement, resistanceBypass);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Once projectile leaves object it got instantiated by allow it to collide with others
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Health>(out Health health))
            canCollide = true;
        else if(other.gameObject == ownerObject)
            canCollide = true;
    }

    
}
#region Dan's TriggerEnter Code
////& other.gameObject.tag != "Player" is a REAAAAAALLLY temp fix
////just so that projectile doesnt hit the player when it fires
//if (other.gameObject.TryGetComponent<Health>(out Health health) & other.gameObject.tag != "Player")
//{
//    health.Damage(damage);
//   //potentially remove this ne in fvour of the other block
//    if (health.ElementName == ElementReactionLibrary.ElementName.Fire & myElement == ElementReactionLibrary.ElementName.Ice)
//    {
//        ElementReactionLibrary.FireIceReaction(transform.position);
//        Debug.Log(Time.time);
//    }
//    else if(health.ElementName == ElementReactionLibrary.ElementName.Ice & myElement == ElementReactionLibrary.ElementName.Fire)
//    {
//        ElementReactionLibrary.FireIceReaction(transform.position);
//        Debug.Log(Time.time);
//    }
//}
//else if (other.gameObject.TryGetComponent<ProjectileDamage>(out ProjectileDamage projectile))
//{
//    if (projectile.myElement == ElementReactionLibrary.ElementName.Fire & myElement == ElementReactionLibrary.ElementName.Ice)
//    {
//        ElementReactionLibrary.FireIceReaction(transform.position);
//        Debug.Log(Time.time);
//    }
//    else if (projectile.myElement == ElementReactionLibrary.ElementName.Ice & myElement == ElementReactionLibrary.ElementName.Fire)
//    {
//        ElementReactionLibrary.FireIceReaction(transform.position);
//        Debug.Log(Time.time);
//    }
//    Destroy(projectile);
//    Destroy(this.gameObject);
//}
#endregion