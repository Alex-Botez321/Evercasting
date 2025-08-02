using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

//Had to make abstract to work with both enemies and player with the current damage code
//See Player Health and Enemy Health classes for their respectively
public abstract class Health : MonoBehaviour
{
    //setting health thresholds
    [SerializeField] public float baseMaxHealth = 100;
    [HideInInspector] public float bonusHealth = 1;
    [HideInInspector] public float maxHealth = 100;
    [SerializeField] public float currentHealth = 100;
    [SerializeField] public float baseColdResistance = 0;
    [SerializeField] public float baseHeatResistance = 0;
    [SerializeField] public float baseElectricResistance = 0;
    [SerializeField] public float basePoisonResistance = 0;
    [SerializeField] public float[] resistances;


    //"duration" always means for how long the status is applied.
    // be aware that for some of Stun + frozen, this hould happen moer than once
    [SerializeField] protected float burnDoT = 5f;
    [SerializeField] protected float burnDoTCooldown = 1;
    [SerializeField] protected float burnDuration = 5;
    [SerializeField] protected float burnInstance = 0;
    [SerializeField] protected float frozenDamageMultiplier = 1;
    [SerializeField] protected float frozenDamageIncrease = 0.1f;
    [SerializeField] protected float frozenDuration = 5;
    [SerializeField] protected float mercuryDoT = 2;
    [SerializeField] protected float mercuryDoTCooldown = 0.5f;
    [SerializeField] protected float mercuryDuration = 10;
    [SerializeField] protected float mercuryInstance = 0;
    [SerializeField] protected float StunSlowLength = 5;
    [SerializeField] protected float StunThreshhold = 5f;
    [SerializeField] protected float StunCounter = 0;
    [SerializeField] protected float StunApplicationDuration = 5;
    [SerializeField] protected float StunInstance = 0;
    [SerializeField] protected bool isFrozen = false;

    //used for the ststus ringVFX
    private List<VisualEffect> statusRings = new List<VisualEffect>();
    private VisualEffect statusVFXOrigin;
    private VisualEffect statusVFX0;
    private VisualEffect statusVFX1;
    private VisualEffect statusVFX2;
    private VisualEffect statusVFX3;
    private VisualEffect[] statusRingsArray;
    private string[] statusNames = new string[4] { "IceStatus", "LightningStatus", "FireStatus", "MercuryStatus" };


    public delegate IEnumerator StatusDelegate();
    protected StatusDelegate statusDelegate = null;
    protected StatusDelegate[] statusTable = new StatusDelegate[(int)ElementTable.ElementName.Length];

    protected float damageBlinkDuration = 0.5f;
    protected float damageBlinkIntensity = 20;
    protected float damageBlinkTimer = 0;

    //public StateController stateController;
    float storedSpeed;
    protected virtual void Awake()
    {
        maxHealth = baseMaxHealth + bonusHealth;
        currentHealth = maxHealth;
        resistances = new float[] { 0, baseColdResistance, baseHeatResistance, baseElectricResistance, basePoisonResistance };

        //this doesn't apply to the player and is throwing errors
        //pls move to enemy health when you can as the player and enemies need different implementations
        //since we didn't do a state machine for the player
        //stateController = GetComponent<StateController>();
        //float storedSpeed = stateController.navMeshAgent.speed;
    }
    /// <summary>
    /// Reduce health by "damage" amount & check for death
    /// Has an overload for projectiles, where we check the element name 
    /// to apply status effects
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Damage(float damage, int damageType)
    {

    }

    public virtual void Damage(float damage, ElementTable.ElementName elementName, float ignoresResistance)
    {

    }

    protected virtual void PostDamageCheck() { }

    protected virtual void PopulateStatusTable()
    {
        statusTable[(int)ElementTable.ElementName.Ice] = FrozenCoroutine;
        statusTable[(int)ElementTable.ElementName.Fire] = BurnedCoroutine;
        statusTable[(int)ElementTable.ElementName.Lightning] = StunnedCoroutine;
        statusTable[(int)ElementTable.ElementName.Mercury] = PoisonedCoroutine;
    }

    protected virtual IEnumerator BurnedCoroutine()
    {
        StatusVFXChange("FireStatus");
        float burnTimer = burnDuration;
        float DoTTimer = burnDoTCooldown;
        //Debug.Log("there is a call");
        while(burnTimer >= 0)
        {
            burnTimer -= 1 * Time.deltaTime;
            DoTTimer -= 1 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (DoTTimer <= 0)
            {
                DoTTimer = burnDoTCooldown;
                //Debug.Log("Should happen");
                Damage(burnDoT, 0);
                yield return new WaitForEndOfFrame();
            }
            //Debug.Log(burnTimer);
        }
        DeactivateStatusRings("FireStatus");
        yield return null;
    }
    protected virtual IEnumerator FrozenCoroutine()
    {
        StatusVFXChange("IceStatus");
        float currentDuration = frozenDuration;
        frozenDamageMultiplier = frozenDamageMultiplier + frozenDamageIncrease;
        while (frozenDamageMultiplier > 1)
        {
            while (currentDuration >= 0)
            {
                currentDuration -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            frozenDamageMultiplier = frozenDamageMultiplier - frozenDamageIncrease;
            currentDuration = frozenDuration;
            yield return new WaitForEndOfFrame();
        }
        DeactivateStatusRings("IceStatus");
        yield return null;
    }
    protected virtual IEnumerator StunnedCoroutine()
    {
        //stun is different depending on Health Scripts
        // the code will be in PlayerHelth/EnemyHealth
        yield return null;
    }
    protected virtual IEnumerator PoisonedCoroutine()
    {
        StatusVFXChange("MercuryStatus");
        float mercTimer = mercuryDuration;
        float DoTTimer = mercuryDoTCooldown;
        Debug.Log("there is a call");
        while (mercTimer >= 0)
        {
            mercTimer -= 1 * Time.deltaTime;
            DoTTimer -= 1 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (DoTTimer <= 0)
            {
                DoTTimer = mercuryDoTCooldown;
                Debug.Log("Should happen");
                Damage(mercuryDoT, 0);
                yield return new WaitForEndOfFrame();
            }
            Debug.Log(mercTimer);
        }
        DeactivateStatusRings("MercuryStatus");
        yield return null;
    }


#region Status Ring Code
    /// <summary>
    /// This code chunk is for setting the Status rings
    /// </summary>
    protected virtual void SetStatusRings()
    {
        foreach (VisualEffect sRing in GetComponentsInChildren<VisualEffect>())
        {
            if (sRing.gameObject.name == "StatusInner_VFX")
            {
                statusVFXOrigin = sRing;
                statusVFXOrigin.gameObject.SetActive(false);
            }
            else if (sRing.gameObject.name == "Status_VFX")
            {
                statusVFX0 = sRing;
                statusVFX0.gameObject.SetActive(false);
            }
            else if (sRing.gameObject.name == "Status_VFX (1)")
            {
                statusVFX1 = sRing;
                statusVFX1.gameObject.SetActive(false);
            }
            else if (sRing.gameObject.name == "Status_VFX (2)")
            {
                statusVFX2 = sRing;
                statusVFX2.gameObject.SetActive(false);
            }
            else if (sRing.gameObject.name == "Status_VFX (3)")
            {
                statusVFX3 = sRing;
                statusVFX3.gameObject.SetActive(false);
            }
        }
        statusRingsArray = new VisualEffect[4] { statusVFX0, statusVFX1, statusVFX2, statusVFX3 };
    }

    protected virtual void StatusVFXChange(string statusName)
    {
        //statusVFX0.SetBool("FireStatus", true);

        foreach (VisualEffect ring in statusRingsArray)
        {

            if (!ring.gameObject.activeSelf)
            {
                if (CheckExistingRings(statusName) == true)
                {
                    break;
                }
                else
                {
                    foreach (string name in statusNames)
                    {
                        if (name == statusName)
                        {
                            ring.SetBool(statusName, true);
                            if (ring == statusRingsArray[0])
                            {
                                statusVFXOrigin.gameObject.SetActive(true);
                                statusVFXOrigin.SetBool(statusName, true);
                            }
                        }
                        else
                        {
                            ring.SetBool(name, false);
                        }
                    }
                    ring.gameObject.SetActive(true);
                    break;

                }
            }
            else
            {
                continue;
            }
        }
    }

    protected virtual bool CheckExistingRings(string statusName)
    {
        foreach (VisualEffect ring in statusRingsArray)
        {
            if (ring.GetBool(statusName) == false)
            {
                continue;
            }
            else if (ring.GetBool(statusName) == true)
            {
                Debug.Log(statusName);
                return true;
            }
        }
        return false;
    }

    protected virtual void DeactivateStatusRings(string statusName)
    {
        foreach (VisualEffect ring in statusRingsArray)
        {
            if (ring.GetBool(statusName) == true)
            {
                ring.SetBool(statusName, false);
                ring.gameObject.SetActive(false);
                if(ring == statusRingsArray[0])
                {
                    statusVFXOrigin.gameObject.SetActive(false);
                    statusVFXOrigin.SetBool(statusName, false);
                }
                break;
            }
            else if (ring.GetBool(statusName) == false)
            {
                continue;
            }
        }

    }

#endregion
}
