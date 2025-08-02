using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Scroll;

public class PlayerHealth : Health
{
    private PlayerAudioManager audioManager;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private UpdateCanvas canvas;
    private PlayerController playerController;

    private List<Material> loopyMats;
    private Coroutine _damageFlashCoroutine;
    [SerializeField]private Color DamageFlashColor = Color.red;
    [SerializeField] private bool DamageOccured;
    [SerializeField] private float invincibilitySeconds = 2;

    [SerializeField] private int maxFlaskCharges = 3;
    [SerializeField] private int currentFlaskCharges = 3;
    [SerializeField] private float flaskHealAmount = 50;
    private float storedSpeed;
    public bool slowActive = false;
    private float timeSlowed;

    protected override void Awake()
    {
        base.Awake();
        audioManager = GetComponent<PlayerAudioManager>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        playerController = GetComponent<PlayerController>();
        storedSpeed = playerController.baseMoveForce;
        if(skinnedMeshRenderer != null)
        {
            Debug.Log(skinnedMeshRenderer);
        }
        canvas = UnityEngine.Object.FindFirstObjectByType<Canvas>().GetComponent<UpdateCanvas>();
        var temp = GameObject.Find("Canvas");
        canvas = temp.GetComponent<UpdateCanvas>();
        currentFlaskCharges = maxFlaskCharges;
        canvas.InitializeFlasks(maxFlaskCharges);
        SetPlayerMaterials();
        PopulateStatusTable();
    }
    public override void Damage(float damage, int damageType)
    {
        damage -= damage * resistances[damageType];
        if (frozenDamageMultiplier > 1 && damageType != 1)
        {
            damage = damage * frozenDamageMultiplier;
            frozenDamageMultiplier = 1;
        }

        if (DamageOccured == false)
        {
            currentHealth -= damage;
            PostDamageCheck();
            DamageOccured = true;
            StartCoroutine(Invincibility());
        }
        else if (DamageOccured == false) 
        {
            //incase we need other things to be checked, add things here
            PostDamageCheck();
        }
    }
    /// <summary>
    /// Damage Script OVERLOAD, for basic prejectiles so that we know the element sent and 
    /// therefore can apply statuses
    /// </summary>
    public override void Damage(float damage, ElementTable.ElementName elementName, float ignoresResistance)
    {
        damage -= damage * resistances[(int)elementName];
        if (frozenDamageMultiplier > 1 && (int)elementName != 1)
        {
            damage = damage * frozenDamageMultiplier;
            frozenDamageMultiplier = 1;
        }

        if (DamageOccured == false)
        {
            DamageOccured = true;
            currentHealth -= damage;
            if ((int)elementName != 0)
            {

            }
            PostDamageCheck();
            //StatusDelegate addStatus = new StatusDelegate(BurnedCoroutine);
            if (statusTable != null)
            {
                statusDelegate = statusTable[(int)elementName];
            }
            StartCoroutine(Invincibility());
        }
        else if (DamageOccured == true)
        {
            //incase we need other things to be checked, like status table, add things here
            PostDamageCheck();
        }
    }

    protected override void PostDamageCheck()
    {
        canvas.UpdateHealth(currentHealth / maxHealth);


        if (currentHealth <= 0)
        {
            currentFlaskCharges = 0;
            GetComponentInChildren<Animator>().SetBool("IsDead", true);
            audioManager.PlayDeathSound();
            
        }
        else
        {
            audioManager.PlayPlayerDamage();
            damageBlinkTimer = damageBlinkDuration;
            _damageFlashCoroutine = StartCoroutine(DamageFlash());
        }

    }

    private IEnumerator Invincibility()
    {
        DamageOccured = true;

        yield return new WaitForSeconds(invincibilitySeconds);
        DamageOccured = false;  

    }

    public void OnFlaskUse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (currentFlaskCharges > 0)
            {
                currentHealth += maxHealth * 0.5f;
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;

                audioManager.PlayFlaskSound();
                canvas.UpdateHealth(currentHealth / maxHealth);
                currentFlaskCharges--;
                canvas.UpdateFlasks(currentFlaskCharges);
            }
        }
    }

    //old damgae flash code, should be more efficient as a coroutine

    //light intensity +1f because at 0 everything turns black, 1f maintains the base colour as intended
    //private void Update()
    //{
    //    damageBlinkTimer -= Time.deltaTime;
    //    float lightLevelLerp = Mathf.Clamp01(damageBlinkTimer / damageBlinkDuration);
    //    float lightIntensity = (lightLevelLerp * damageBlinkIntensity) + 1f;
    //    meshRenderer.material.color = Color.white * lightIntensity;
    //}

    private void SetPlayerMaterials()
    {
        PlayerController pcInstance = GetComponentInParent<PlayerController>();
        loopyMats = pcInstance.loopyElementMaterials;

        foreach( Material mat in loopyMats)
        {
            mat.SetColor("_DamageFlashColor", DamageFlashColor);
        }
    }
    private IEnumerator DamageFlash()
    {
        while(damageBlinkTimer > 0)
        {
            damageBlinkTimer -= Time.deltaTime;
            float lightLevelLerp = Mathf.Clamp01(damageBlinkTimer / damageBlinkDuration);
            float lightIntensity = (lightLevelLerp * damageBlinkIntensity); //+1
            DamageFlashColourChange(lightIntensity);
            skinnedMeshRenderer.material.color = DamageFlashColor * lightIntensity;
            yield return null;
        }
    }

    private void DamageFlashColourChange(float intensity)
    {
        skinnedMeshRenderer.material.SetFloat("_DamageFlashIntensity", intensity);
    }

    protected override IEnumerator StunnedCoroutine()
    {
        StatusVFXChange("LightningStatus");
        float currentDuration = StunApplicationDuration;
        float stunDuration = StunSlowLength;

        if (StunCounter < StunThreshhold)
        {
            //Stun Counter should increase on each hit, when able to
            StunCounter = StunCounter + 1;
        }
        else if (StunCounter >= StunThreshhold)
        {
            //after threshhold reached, make sure we can only have 1 stun running at a time
            StunInstance = StunInstance + 1;
            yield return new WaitForEndOfFrame();
            if (StunInstance == 1)
            {
                //code for slowing
                while (stunDuration >= 0)
                {
                    playerController.baseMoveForce = 0;
                    DamageFlashColor = Color.cyan;
                    stunDuration -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                playerController.baseMoveForce = storedSpeed;
                DamageFlashColor = Color.white;
                StunCounter = 0;
            }
            else if (StunInstance > 1)
            {
                //stun counter doesnt trigger
                yield return new WaitForEndOfFrame();
            }

        }

        while (currentDuration >= 0)
        {
            currentDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        DeactivateStatusRings("LightningStatus");
        yield return null;
    }

    public void SlowCoroutine(float speedMult, float seconds)
    {
        timeSlowed = seconds;
        if (slowActive == false)
        {
            StartCoroutine(MoveSpeedSlow(speedMult, seconds));
        }
    }

    public IEnumerator MoveSpeedSlow(float speedMult, float seconds) 
    {
        slowActive = true;

        float returnalMoveSpeed = playerController.baseMoveForce;
        float temporaryMoveSpeed = playerController.baseMoveForce * speedMult;
        while (timeSlowed > 0)
        {
            Debug.Log("ZZZZZZZZZZZZZ");
            playerController.baseMoveForce = temporaryMoveSpeed;
            timeSlowed -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        playerController.baseMoveForce = returnalMoveSpeed;
        yield return null;
    }
}