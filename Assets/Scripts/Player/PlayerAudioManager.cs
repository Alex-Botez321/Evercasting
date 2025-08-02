using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] iceSpells;
    [SerializeField] private AudioClip[] fireSpells;
    [SerializeField] private AudioClip[] lightningSpells;
    [SerializeField] private AudioClip[] mercurySpells;
    [SerializeField] private AudioClip[] playerDamage;
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip iceCharge;
    [SerializeField] private AudioClip fireCharge;
    [SerializeField] private AudioClip lightningCharge;
    [SerializeField] private AudioClip mercuryCharge;
    [SerializeField] private AudioClip[] flaskDrink;
    [SerializeField] private float damageCooldown;
    [SerializeField] private float minPitchIce = 1;
    [SerializeField] private float maxPitchIce = 1;
    [SerializeField] private float minPitchFire = 1;
    [SerializeField] private float maxPitchFire = 1;
    [SerializeField] private float minPitchLightning = 1;
    [SerializeField] private float maxPitchLightning = 1;
    [SerializeField] private float minPitchMercury = 1;
    [SerializeField] private float maxPitchMercury = 1;
    [SerializeField] private float minPitchDamage = 1;
    [SerializeField] private float maxPitchDamage = 1;
    private bool canPlayDamage = true;
    private AudioSource audioSource;

    public delegate void ElementSound();
    ElementSound elementReaction = null;
    private ElementSound[] elementAudioTable;

    ElementSound elementChargedReaction = null;
    private ElementSound[] elementChargedAudioTable;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        elementAudioTable = new ElementSound[4];
        elementAudioTable[0] = PlayIceSound;
        elementAudioTable[1] = PlayFireSound;
        elementAudioTable[2] = PlayLightningSound;
        elementAudioTable[3] = PlayMercurySound;

        elementChargedAudioTable = new ElementSound[4];
        elementChargedAudioTable[0] = PlayIceCharge;
        elementChargedAudioTable[1] = PlayFireCharge;
        elementChargedAudioTable[2] = PlayLightningCharge;
        elementChargedAudioTable[3] = PlayMercuryCharge;
    }

    public void PlayPorjectileSound(int element)
    {
        elementReaction = elementAudioTable[(int)element];
        elementReaction();
    }

    public void PlayChargedPorjectileSound(int element)
    {
        elementReaction = elementAudioTable[(int)element];
        elementReaction();
    }

    public void PlayIceSound()
    {
        int soundClip = Random.Range(0, iceSpells.Length);
        audioSource.pitch = Random.Range(minPitchIce, maxPitchIce);
        audioSource.PlayOneShot(iceSpells[soundClip]);
    }
    public void PlayFireSound() 
    {
        int soundClip = Random.Range(0, fireSpells.Length);
        audioSource.pitch = Random.Range(minPitchFire, maxPitchFire);
        audioSource.PlayOneShot(fireSpells[soundClip]);
    }

    public void PlayLightningSound()
    {
        int soundClip = Random.Range(0, lightningSpells.Length);
        audioSource.pitch = Random.Range(minPitchLightning, maxPitchLightning);
        audioSource.PlayOneShot(lightningSpells[soundClip]);
    }

    public void PlayMercurySound()
    {
        int soundClip = Random.Range(0, mercurySpells.Length);
        audioSource.pitch = Random.Range(minPitchMercury, maxPitchMercury);
        audioSource.PlayOneShot(mercurySpells[soundClip]);
    }

    public void PlayFlaskSound()
    {
        int soundClip = Random.Range(0, flaskDrink.Length);
        audioSource.PlayOneShot(flaskDrink[soundClip]);
    }

    public void PlayPlayerDamage()
    {
        if (canPlayDamage)
        {
            int soundClip = Random.Range(0, playerDamage.Length);
            audioSource.pitch = Random.Range(minPitchDamage, maxPitchDamage);
            audioSource.PlayOneShot(playerDamage[soundClip]);
            StartCoroutine(PlayerDamageTimer());
        }
    }

    public void PlayIceCharge()
    {
       audioSource.PlayOneShot(iceCharge);
    }

    public void PlayFireCharge()
    {
        audioSource.PlayOneShot(fireCharge);
    }

    public void PlayLightningCharge()
    {
        audioSource.PlayOneShot(lightningCharge);
    }

    public void PlayMercuryCharge()
    {
        audioSource.PlayOneShot(mercuryCharge);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(playerDeath);
    }

    IEnumerator PlayerDamageTimer()
    {
        canPlayDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canPlayDamage = true;
    }
}
