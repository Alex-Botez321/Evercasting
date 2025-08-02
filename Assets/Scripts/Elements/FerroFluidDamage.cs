using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FerroFluidDamage : MonoBehaviour
{
    //private int damage = 10;
    //float damageCooldown = 0.5f;
    float ferrofluidCooldown = 5f;
    bool startCooldown = false;
    [SerializeField] float baseSpeed = 100f;
    [SerializeField] float slowedSpeed = 0.1f;
    int _enemyContacts;
    Coroutine _slowingCoroutine;
    public float coroutineCounter;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] watercollisionSFX;
    /// <summary>
    /// PLEASE NOTE: THIS IS TEMPORARRY CODE!!!!!!!
    /// I will change it when implementing other things fully, for now they just slow like water
    /// </summary>
    private void Awake()
    {
        Invoke("DestroyWater", ferrofluidCooldown);
        audioSource = GetComponent<AudioSource>();
        coroutineCounter = ferrofluidCooldown;
        startCooldown = true;
    }

    private void Start()
    {
        int soundClip = UnityEngine.Random.Range(0, watercollisionSFX.Length);
        audioSource.PlayOneShot(watercollisionSFX[soundClip]);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enemy");
        Debug.Log(other.gameObject.name);
        if (other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyAI))
        {
            _enemyContacts++;
            Debug.Log("yo");
            //starts a couroutine function for each enemy, in theory
            if (_slowingCoroutine == null)
            {
                _slowingCoroutine = StartCoroutine(SlowingLoop(enemyAI));
            }
        }
        if (other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            _enemyContacts++;
            StartCoroutine(SlowingPlayer(player));
        }
    }



    private IEnumerator SlowingLoop(EnemyHealth enemyAI)
    {

        coroutineCounter = ferrofluidCooldown;
        //there is an issue where if the water objects terminates the enemies never finish their couroutine
        //amd are therefore stuck in inWater = true. while(waterCooldown > 0.1f) should hopefully remedy this
        while (_enemyContacts > 0 && coroutineCounter > 0.2f)
        {
            //Debug.Log(coroutineCounter);
            enemyAI.SlowCoroutine(slowedSpeed, 1f);

            //Debug.Log(_enemyContacts);
            //Debug.Log("Yup, water");
            //Debug.Log(enemyAI.inWater);
            yield return null;
        }

        enemyAI.SlowCoroutine(1,1);
        _slowingCoroutine = null;
    }

    private IEnumerator SlowingPlayer(PlayerHealth player)
    {
        coroutineCounter = ferrofluidCooldown;
        //there is an issue where if the water objects terminates the enemies never finish their couroutine
        //amd are therefore stuck in inWater = true. while(waterCooldown > 0.1f) should hopefully remedy this
        while (_enemyContacts > 0 && coroutineCounter > 0.2f)
        {
            //Debug.Log(coroutineCounter);
            player.SlowCoroutine(slowedSpeed, 1f);

            //Debug.Log(_enemyContacts);
            //Debug.Log("Yup, water");
            //Debug.Log(enemyAI.inWater);
            yield return null;
        }
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
