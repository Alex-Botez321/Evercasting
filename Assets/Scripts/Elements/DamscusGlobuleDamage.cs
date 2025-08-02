using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;

public class DamscusGlobuleDamage : MonoBehaviour
{
    [SerializeField] private float damage = 2;
    [SerializeField] private float explosionRadius = 1;
    [SerializeField] private bool canDoDamage = false;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float globuleCooldown = 4f;
    [SerializeField] private float GlobulePlayRate = 2f;
    [SerializeField] private Coroutine _globuleCoroutine;
    [SerializeField] private VisualEffect globVFX;
    [SerializeField] private AudioClip[] globcollisionSFX;
    [HideInInspector] public float damageMultiplier = 1f;
    public ElementTable.ElementName myElement = ElementTable.ElementName.Fire;
    //private float elapsed;

    int _enemyContacts;
    Coroutine _damageCoroutine;
    private AudioSource audioSource;
    private void Awake()
    {
        Invoke("DestroyGlob", globuleCooldown);
        _globuleCoroutine = StartCoroutine(DamageLoop());
        audioSource = GetComponent<AudioSource>();

    }

    private void Start()
    {
        int soundClip = UnityEngine.Random.Range(0, globcollisionSFX.Length);
        //disabled sound because it was throwing errors
        //audioSource.PlayOneShot(globcollisionSFX[soundClip]);
        damage *= damageMultiplier;
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("UUUUU");
        if(canDoDamage == true)
        {
            Debug.Log("hhhh");
            if (other.gameObject.TryGetComponent<Health>(out Health damageable))
            {
                Debug.Log("fggf");
                damageable.Damage(damage, (int)myElement);
                canDoDamage = false;
            }
        }
    }

    //private void Explosion()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
    //    foreach (var c in colliders)
    //    {
    //        if(c.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth damageable))
    //        {
    //            damageable.Damage(damage, (int)myElement);
    //        }
    //    }
    //}



    private IEnumerator DamageLoop()
    {
        Debug.Log("penis");
        float DoTTimer = damageCooldown;
        float totalTimer = globuleCooldown;
        
        while (totalTimer >= 0)
        {
            DoTTimer -= Time.deltaTime;
            totalTimer -= Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
            if (DoTTimer < 0) 
            {
                Debug.Log("Balls");
                DoTTimer = damageCooldown;
                //Explosion();
                canDoDamage = true;
                yield return new WaitForEndOfFrame();
            }
        }
        DestroyGlob();
    }

    private void DestroyGlob()
    {
        Destroy(gameObject);
    }
}
