using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlasmaDamage : MonoBehaviour
{
    [SerializeField] private float damage = 30;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float expansionRate = 1f;
    [SerializeField] private float bufferTime = 1f;
    [SerializeField] private float minimumHitboxRadius = 0f;
    [SerializeField] private float maximumHitboxRadius = 5f;
    [SerializeField] private float plasmaCooldown = 5f;
    [SerializeField] private float plasmaPlayRate = 2f;
    [SerializeField] private VisualEffect plasmaVFX;
    [SerializeField] private AudioClip[] plasmacollisionSFX;
    [SerializeField] public float damageMultiplier = 1f;
    public ElementTable.ElementName myElement = ElementTable.ElementName.Fire;
    //private float elapsed;

    int _enemyContacts;
    EnemyHealth[] currentEnemies = new EnemyHealth[5]; //theoretical amount of enemies
    Coroutine _damageCoroutine;
    private AudioSource audioSource;
    private SphereCollider sphereCollider;

    private void Awake()
    {
        Invoke("DestroyPlasma", plasmaCooldown);
        audioSource = GetComponent<AudioSource>();
        plasmaVFX = GetComponentInChildren<VisualEffect>();
        plasmaVFX.playRate = plasmaPlayRate;
        sphereCollider = GetComponent<SphereCollider>();


    }


    private void Start()
    {
        int soundClip = UnityEngine.Random.Range(0, plasmacollisionSFX.Length);
        audioSource.PlayOneShot(plasmacollisionSFX[soundClip]);

        damage *= damageMultiplier;

        Debug.Log(damage);

        StartCoroutine(ExpandColliderSize());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
        {
            Debug.Log("PlasmaDamage");
            Debug.Log(damage);
            enemy.Damage(damage, (int)myElement);
        }
    }

    //had to separate into 2 coroutines due to memory leaks and limited time to investigate them
    private IEnumerator ExpandColliderSize()
    {
        while(sphereCollider.radius < maximumHitboxRadius)
        {
            sphereCollider.radius += expansionRate*Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(ShrinkColliderSize());
        yield return null;
    }

    private IEnumerator ShrinkColliderSize()
    {
        while (sphereCollider.radius > minimumHitboxRadius)
        {
            sphereCollider.radius -= expansionRate*Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(ExpandColliderSize());
        yield return new WaitForSeconds(bufferTime);
        yield return null;
    }

    private void DestroyPlasma()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopCoroutine(ShrinkColliderSize());
        StopCoroutine(ExpandColliderSize());
    }
}
