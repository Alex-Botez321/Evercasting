using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] protected AudioClip spawnSFX;
    [SerializeField] protected AudioClip[] attack1SFX;
    [SerializeField] protected AudioClip[] attack2SFX;
    [SerializeField] protected AudioClip[] attack3SFX;
    [SerializeField] protected AudioClip[] deathSFX;
    [SerializeField] protected AudioClip[] damageSFX;
    [SerializeField] protected AudioClip[] teleportSFX;
    [SerializeField] protected float damageCooldown = 1.5f;
    [SerializeField] private float minPitchAttack1 = 1;
    [SerializeField] private float maxPitchAttack1 = 1;
    [SerializeField] private float minPitchAttack2 = 1;
    [SerializeField] private float maxPitchAttack2 = 1;
    [SerializeField] private float minPitchAttack3 = 1;
    [SerializeField] private float maxPitchAttack3 = 1;
    [SerializeField] private float minPitchDamage = 1;
    [SerializeField] private float maxPitchDamage = 1;
    [SerializeField] private float minPitchTeleport = 0.8f;
    [SerializeField] private float maxPitchTeleport = 1.3f;
    protected bool canPlayDamage = true;
    protected AudioSource audioSource;

    public virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void Start()
    {
        audioSource.PlayOneShot(spawnSFX);
    }

    public virtual void PlayAttack1()
    {
        int soundClip = Random.Range(0, attack1SFX.Length);
        audioSource.pitch = Random.Range(minPitchAttack1, maxPitchAttack1);
        audioSource.PlayOneShot(attack1SFX[soundClip]);
    }

    public virtual void PlayAttack2()
    {
        int soundClip = Random.Range(0, attack2SFX.Length);
        audioSource.pitch = Random.Range(minPitchAttack2, maxPitchAttack2);
        audioSource.PlayOneShot(attack2SFX[soundClip]);
    }

    public virtual void PlayAttack3()
    {
        int soundClip = Random.Range(0, attack3SFX.Length);
        audioSource.pitch = Random.Range(minPitchAttack3, maxPitchAttack3);
        audioSource.PlayOneShot(attack3SFX[soundClip]);
    }

    public virtual void PlayDamage()
    {
        if (canPlayDamage)
        {
            int soundClip = Random.Range(0, damageSFX.Length);
            audioSource.pitch = Random.Range(minPitchDamage, maxPitchDamage);
            audioSource.PlayOneShot(damageSFX[soundClip]);
            StartCoroutine(DamageTimer());
        }
    }

    public virtual void PlayDeath()
    {
        int soundClip = Random.Range(0, deathSFX.Length);
        audioSource.PlayOneShot(deathSFX[soundClip]);
    }

    public virtual void PlayTeleport()
    {
        int soundClip = Random.Range(0, deathSFX.Length);
        audioSource.pitch = Random.Range(minPitchTeleport, maxPitchTeleport);
        audioSource.PlayOneShot(deathSFX[soundClip]);
    }

    IEnumerator DamageTimer()
    {
        canPlayDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canPlayDamage = true;
    }
}
