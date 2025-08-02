using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.VFX;
using static ElementTable;
using static Scroll;

public class EnemyHealth : Health
{
    private StateController stateController;
    [SerializeField] private GameObject healthBarPrefab;
    private Image healthBarUI;
    private GameObject cam;
    [SerializeField] private Vector3 positionOffSet;
    //[HideInInspector] public bool wet;        //TO ALEX: I temporarily commented this oout
    //because it was returning errors due to ambiguity,
    //if i shouldnt do it/should do the other wet variable  feel free to change this
    private float timeSlowed;
    private float storedSpeed;
    public bool slowActive = false;
    public bool wet = false;
    [SerializeField] private float resistanceGain = 0.2f;
    [SerializeField] private float resistanceLimit = 0.9f;
    [SerializeField] private float wetDuration = 5f;
    [SerializeField] private float raisedResistanceDuration = 5f;

    [ColorUsage(true, true)]
    List<SkinnedMeshRenderer> skinnedMeshRenderersList = new List<SkinnedMeshRenderer>();
    List<MeshRenderer> regularMeshRenderersList = new List<MeshRenderer>();
    MeshRenderer regularMeshRenderer;
    private bool usingSkinnedMesh;
    private Color DamageFlashColor = Color.white;
    private Material[] materialsArray;
    List<Material> materialsList = new List<Material>();
    private Coroutine _damageFlashCoroutine;




    protected override void Awake()
    {
        base.Awake();
        stateController = GetComponent<StateController>();
        storedSpeed = stateController.navMeshAgent.speed;
        cam = GameObject.Find("Main Camera");
        //statusVFX = GetComponentInChildren<VisualEffect>();
        SetBaseColors();
        SetStatusRings();
        PopulateStatusTable();

        damageBlinkDuration = 1;
    }

    private void Start()
    {
        healthBarPrefab = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        healthBarUI = healthBarPrefab.GetComponent<Image>();
        healthBarUI.fillAmount = currentHealth / maxHealth;
    }

    public override void Damage(float damage, int damageType)
    {
        damage -= damage * resistances[damageType];
        if (frozenDamageMultiplier > 1 && damageType != 1)
        {
            damage = damage * frozenDamageMultiplier;
            frozenDamageMultiplier = 1;
        }
        currentHealth -= damage;
        stateController.TakeDamage(damage);
        healthBarUI.fillAmount = currentHealth / maxHealth;
        
        damageBlinkTimer = damageBlinkDuration;
        _damageFlashCoroutine = StartCoroutine(DamageFlash());

        if (!wet)
            ResetResit();
    }

    public override void Damage(float damage, ElementTable.ElementName elementName, float ignoresResistance)
    {
        damage -= damage * (resistances[(int)elementName] * ignoresResistance);
        if(frozenDamageMultiplier > 1 && (int)elementName != 1)
        {
            damage = damage * frozenDamageMultiplier;
            frozenDamageMultiplier = 1;
        }
        Debug.Log(damage);
        currentHealth -= damage;
        stateController.TakeDamage(damage);


        //Resistance gain and start on resist pen, need ferro fluid and water to be separated
        if(!wet)
        {
            resistances[(int)elementName] += resistanceGain;
            if (resistances[(int)elementName] > resistanceLimit)
                resistances[(int)elementName] = resistanceLimit;

            StopAllCoroutines();
            StartCoroutine(ResistGainExpire());
        }


        healthBarUI.fillAmount = currentHealth / maxHealth;

        damageBlinkTimer = damageBlinkDuration;
        _damageFlashCoroutine = StartCoroutine(DamageFlash());

        if (statusTable != null)
        {
            statusDelegate = statusTable[(int)elementName];
            //Debug.Log((int)elementName);
            if (statusTable[(int)elementName] != null)
            {
                StartCoroutine(statusDelegate());
            }
        }
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

        float returnalMoveSpeed = storedSpeed;
        float temporaryMoveSpeed = storedSpeed * speedMult;
        while (timeSlowed > 0)
        {

            stateController.ChangeSpeed(temporaryMoveSpeed);
            timeSlowed -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        stateController.ChangeSpeed(returnalMoveSpeed);
        yield return null;
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
        else if (StunCounter >= StunThreshhold - 1)
        {
            //after threshhold reached, make sure we can only have 1 stun running at a time
            StunInstance = StunInstance + 1;
            yield return new WaitForEndOfFrame();
            if (StunInstance == 1)
            {
                //code for slowing
                while (stunDuration >= 0)
                {
                    stateController.ChangeSpeed(0);
                    DamageFlashColor = Color.cyan;
                    stunDuration -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                stateController.ChangeSpeed(storedSpeed);
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

        stateController.ChangeSpeed(storedSpeed);
        DeactivateStatusRings("LightningStatus");
        yield return null;
    }

    public IEnumerator WetExpire()
    {
        wet = true;
        for (int i = 0; i < resistances.Length; i++)
        {
            resistances[i] = 0;
        }
        yield return new WaitForSeconds(wetDuration);
        wet = false;
        ResetResit();
    }

    IEnumerator ResistGainExpire()
    {
        yield return new WaitForSeconds(raisedResistanceDuration);
        if(!wet)
            ResetResit();
    }

    private void ResetResit()
    {
        resistances = new float[] { 0, baseColdResistance, baseHeatResistance, baseElectricResistance, basePoisonResistance };
    }

    private void FixedUpdate()
    {
        healthBarPrefab.transform.position = cam.GetComponent<Camera>().WorldToScreenPoint(transform.position + positionOffSet)*0.5f;
        //StatusVFXChange("FireStatus");
    }

    //private void Update()
    //{
    //    damageBlinkTimer -= Time.deltaTime; //since this is a fixed update do we need . deltatime?

    //    foreach(SkinnedMeshRenderer smr in skinnedMeshRenderersList)
    //    {
    //        for (int i = 0; i < skinnedMeshRenderersList.Count; i++)
    //        {
    //            //float lightLevelLerp = Mathf.Clamp01(damageBlinkTimer / damageBlinkDuration);
    //            //float lightIntensity = (lightLevelLerp * damageBlinkIntensity) + 1f;
    //            //smr.material.color = Color.white * lightIntensity;
    //            ////smr.material.shader
    //            //Debug.Log(skinnedMeshRenderersList[3]);
    //            //continue;



    //        }

    //    }
    //}

    #region Damage Flash Code
    private void SetBaseColors()
    {
        Debug.Log("NotHere");
        if (this.gameObject.GetComponentInChildren<MeshRenderer>() != null)
        {
            usingSkinnedMesh = false;

            foreach (MeshRenderer mr in this.gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                regularMeshRenderersList.Add(mr);
            }

            for (int i = 0; i < regularMeshRenderersList.Count; i++)
            {
                materialsList.Add(regularMeshRenderersList[i].material);
            }
        }
        if (this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>() != null)
        {
            usingSkinnedMesh = true;
            foreach (SkinnedMeshRenderer sMR in this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                skinnedMeshRenderersList.Add(sMR);
            }

            for (int i = 0; i < skinnedMeshRenderersList.Count; i++)
            {
                materialsList.Add(skinnedMeshRenderersList[i].material);
            }
        }
    }

    private IEnumerator DamageFlash()
    {

        float count = 0;
        //if(usingSkinnedMesh == true)
        //{
        //    count = skinnedMeshRenderersList.Count;
        //}
        //else if (usingSkinnedMesh == false)
        //{
        //    count = regularMeshRenderersList.Count;
        //}
        count = regularMeshRenderersList.Count + skinnedMeshRenderersList.Count;

        for (int i = 0; i < count; i++)
        {
            //materialsArray[i].SetColor("_DamageFlashColor", DamageFlashColor);
            materialsList[i].SetColor("_DamageFlashColor", DamageFlashColor);
        }

        while (damageBlinkTimer > 0)
        {
            damageBlinkTimer -= Time.deltaTime;
            float lightLevelLerp = Mathf.Clamp01(damageBlinkTimer / damageBlinkDuration);
            float lightIntensity = (lightLevelLerp * damageBlinkIntensity);
            SetColorChange(lightIntensity, count);
            yield return null;
        }
    }


    private void SetColorChange(float amount, float count)
    {
        for (int i = 0; i < count; i++)
        {
            //materialsArray[i].SetFloat("_DamageFlashIntensity", amount);
            materialsList[i].SetFloat("_DamageFlashIntensity", amount);
        }
    }
    #endregion

    private void OnDestroy()
    {
        Destroy(healthBarUI);
        Destroy(healthBarPrefab);
    }
}
