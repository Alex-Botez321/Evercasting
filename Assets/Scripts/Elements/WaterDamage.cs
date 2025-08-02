using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaterDamage : MonoBehaviour
{
    //private int damage = 10;
    //float damageCooldown = 0.5f;
    [SerializeField]float waterCooldown = 12f;
    bool startCooldown = false;
    [SerializeField] float baseSpeed = 100f;
    [SerializeField] float slowedSpeed = 0.75f;
    int _enemyContacts;
    Coroutine _slowingCoroutine;
    public float coroutineCounter;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] watercollisionSFX;

    private void Awake()
    {
        Invoke("DestroyWater", waterCooldown);
        audioSource = GetComponent<AudioSource>();
        coroutineCounter = waterCooldown;
        startCooldown = true;
    }

    private void Start()
    {
        int soundClip = UnityEngine.Random.Range(0, watercollisionSFX.Length);
        audioSource.PlayOneShot(watercollisionSFX[soundClip]);
    }
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyAI))
        {
            _enemyContacts++;
            //starts a couroutine function for each enemy, in theory
            if (_slowingCoroutine == null)
            {
                _slowingCoroutine = StartCoroutine(SlowingLoop(enemyAI));
            }
        }

    }



    private IEnumerator SlowingLoop(EnemyHealth enemyAI)
    {

        coroutineCounter = waterCooldown;
        //there is an issue where if the water objects terminates the enemies never finish their couroutine
        //amd are therefore stuck in inWater = true. while(waterCooldown > 0.1f) should hopefully remedy this
        while (_enemyContacts > 0 && coroutineCounter > 0.2f)
        {
            //Debug.Log(coroutineCounter);
            enemyAI.WetExpire();

            //Debug.Log(_enemyContacts);
            //Debug.Log("Yup, water");
            //Debug.Log(enemyAI.inWater);
            yield return null;
        }
        //temp change now that ferrofluid slows
        //well see if this is important
        //enemyAI.WaterSlow(baseSpeed);
        _slowingCoroutine = null;
    }

    private void Update()
    {
        if (startCooldown == true)
        {
            coroutineCounter -= Time.deltaTime;
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyAI))
        {
            _enemyContacts--;
        }

    }

    private void DestroyWater()
    {
        Destroy(gameObject);
    }
}

#region overlap.Sphere solution for future referencve
//private void SlowEnemies()
//{
//    Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
//    foreach (Collider c in colliders)
//    {
//        if (c.TryGetComponent<Health>(out Health health))
//        {

//        }
//    }
//}

#endregion
