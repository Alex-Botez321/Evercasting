using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class BismuthDamage : Health
{
    //private int damage = 10;
    //float damageCooldown = 0.5f;
    [SerializeField]float BismuthCooldown = 10f;
    bool startCooldown = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] watercollisionSFX;
    private Coroutine _reflectionCoroutine;

    //projectile variables
    [SerializeField] private float projectileSpeed = 600;
    Vector3 newProjDirection = Vector3.one;
    Vector3 newProjScale = new Vector3(2, 2, 2);
    Ray ray;
    RaycastHit hit;
    private float maxHitDistance = 500;
    public Vector3 prehitProjPosition;


    [SerializeField] GameObject IcePrefab;
    [SerializeField] GameObject FirePrefab;
    [SerializeField] GameObject LightningPrefab;
    [SerializeField] GameObject MercuryPrefab;
    [SerializeField] GameObject[] elementProjectileArray;
    [SerializeField] ElementTable.ElementName[] elementReferenceArray;
    int elementIndex;

    /// <summary>
    /// PLEASE NOTE: THIS IS TEMPORARRY CODE!!!!!!!
    /// I will change it when implementing other things fully, for now they just slow like water
    /// </summary>

    protected override void Awake()
    {
        elementProjectileArray = new GameObject[] {null, IcePrefab, FirePrefab, LightningPrefab, MercuryPrefab};
        elementReferenceArray = Enum.GetValues(typeof(ElementTable.ElementName)).Cast<ElementTable.ElementName>().ToArray();
        Invoke("DestroyBismuth", BismuthCooldown);
    }

    public override void Damage(float damage, ElementTable.ElementName elementName, float ignoresResistance)
    {
        ElementTable.ElementName prefabElement = elementName;
        foreach (ElementTable.ElementName element in elementReferenceArray)
        {
            if (element == prefabElement)
            {
                elementIndex = System.Array.IndexOf(elementReferenceArray, element);
            }
        }

        float randomAngle = UnityEngine.Random.Range(0, 360);
        newProjDirection = Quaternion.Euler(1, randomAngle, 1) * newProjDirection;


        //_reflectionCoroutine = StartCoroutine(BismuthRefect());
        GameObject reflectProj = Instantiate(elementProjectileArray[elementIndex], this.transform.position, Quaternion.identity);
        reflectProj.transform.localScale = newProjScale;
        newProjDirection = new Vector3(newProjDirection.x, newProjDirection.y, newProjDirection.z).normalized;
        Debug.Log(newProjDirection);
        reflectProj.GetComponent<Rigidbody>().AddForce(newProjDirection * projectileSpeed, ForceMode.Force);
        reflectProj.transform.forward = -transform.GetChild(0).right;

    }

    //private void OnTriggerEnter(Collider other)
    //{

    //    float randomAngle = UnityEngine.Random.Range(0, 360);
    //    Vector3 projScale = proj.gameObject.transform.localScale;
    //    newProjDirection = Quaternion.Euler(1, randomAngle, 1 ) * proj.gameObject.transform.position;
    //    Destroy(proj);


    //    //_reflectionCoroutine = StartCoroutine(BismuthRefect());
    //    GameObject reflectProj = Instantiate(elementProjectileArray[elementIndex], this.transform.position, Quaternion.identity);
    //    reflectProj.transform.localScale = projScale;
    //    newProjDirection = new Vector3(newProjDirection.x, newProjDirection.y, newProjDirection.z).normalized;
    //    Debug.Log(newProjDirection);
    //    reflectProj.GetComponent<Rigidbody>().AddForce(newProjDirection * projectileSpeed, ForceMode.Force);
    //    reflectProj.transform.forward = -transform.GetChild(0).right;

    //}

    //private IEnumerator BismuthRefect()
    //{
    //    yield return null;
    //}

    private void DestroyBismuth()
    {
        Destroy(this.gameObject);
    }

#region COME BACK to redo actual reflection
    //Debug.Log("here");
    //float xDif = prehitProjPosition.x - proj.gameObject.transform.position.x;
    //float zDif = prehitProjPosition.z - proj.gameObject.transform.position.z;
    //Vector3 newNormal = Vector3.up;
    //Debug.Log(prehitProjPosition);
    //Debug.Log(proj.gameObject.transform.position);
    //Debug.Log(xDif);
    //Debug.Log(zDif);

    //if (xDif >= 0 && zDif >= 0)
    //{
    //    newNormal = Vector3.left; //new Vector3(-0.5f, 0, 0.5f)
    //}
    //else if (xDif < 0 && zDif >= 0)
    //{
    //    newNormal = Vector3.up;
    //}
    //else if (xDif >= 0 && zDif < 0)
    //{
    //    newNormal = new Vector3(-0.7f, 0, 0.7f);
    //}
    //else if (xDif < 0 && zDif < 0)
    //{
    //    newNormal = Vector3.back;
    //}
    //Debug.Log(newNormal);

    ////ray = new Ray(prehitProjPosition, this.gameObject.transform.position);
    ////if(Physics.Raycast(ray, out hit, maxHitDistance))
    ////{
    ////    newProjDirection = Vector3.Reflect(prehitProjPosition, newNormal);
    ////}
    //newProjDirection = Vector3.Reflect(proj.gameObject.transform.position, newNormal);
    //Vector3 newProjScale = proj.gameObject.transform.localScale;


    //ElementTable.ElementName prefabElement = proj.myElement;
    //foreach (ElementTable.ElementName element in elementReferenceArray)
    //{
    //    if(element == prefabElement)
    //    {
    //        elementIndex = System.Array.IndexOf(elementReferenceArray, element);
    //    }
    //}
#endregion
}
