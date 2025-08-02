using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    [SerializeField] private Animator animator;
    private PlayerHealth health;
    private ScrollManager scrollManager;
    private GameObject loopyModel;
    [SerializeField] private Animation loopyLegs;
    [SerializeField] private Animation loopyUpper;
    [SerializeField] private Transform projectileSpawnPosition;


    [Header("Movement/Controls")]
    public PlayerControls playerControls;
    private Vector2 moveInput = Vector2.zero;
    [SerializeField] private Transform cursorPosition;
    [HideInInspector] private float moveForce = 5f;
    [SerializeField] private float moveDrag = 1f;
    [HideInInspector] public float maxMoveSpeed = 10f;
    [SerializeField] public float baseMoveForce = 5f;
    [SerializeField] public float baseMoveSpeed = 10f;
    private bool buttonIsPressed;
    [SerializeField] float stoppingDrag = 15f;

    //Attacks
    [Header("Attack variables")]
    //Projectiles
    [SerializeField] private GameObject iceProjectile;
    [SerializeField] private GameObject fireProjectile;
    [SerializeField] private GameObject lightningProjectile;
    [SerializeField] private GameObject mercuryProjectile;
    [HideInInspector] public GameObject[] elementProjectileArray;
    private ElementTable.ElementName[] elementsReferenceArray;
    [SerializeField] public List<ElementTable.ElementName> playerSpells;
    public int currentProjectile = 1;
    public float baseSize = 1f;
    [SerializeField] public int element = 0;
    [SerializeField] private float attackCooldown = 0.5f;
    private float attackTimeStamp;
    [SerializeField] public float baseCritChance = 0f;
    [HideInInspector] public float critChance = 0f;
    [SerializeField] public float baseCritDamageMultiplier = 2f;
    [HideInInspector] public float critDamageMultiplier = 2f;
    [SerializeField] public float baseProjectileSpeed = 600f;
    [HideInInspector] public float projectileSpeed;
    [HideInInspector] public float projectileLifeTime = 0f;

    //Charge
    [SerializeField] public float baseChargeRate = 0.02f;
    [HideInInspector] public float chargeRate;
    [SerializeField] private int maxCharge = 2;
    [SerializeField] private float currentCharge = 0;
    [SerializeField] private float chargeSlow = 60f;
    [SerializeField] private float shootKnockback = 5f;

    [HideInInspector] public float[] elementalDamage;

    [Header("Animation & VFX")]
    private PlayerAudioManager audioManager;
    [SerializeField] private GameObject chargeObject;
    [SerializeField] private Sprite[] chargeSprites;
    [SerializeField] private Animator chargeAnimator;
    private UpdateCanvas canvas;
    [SerializeField] public List<Material> loopyElementMaterials;
    private SkinnedMeshRenderer loopyMesh;
    [SerializeField] private GameObject loopyChangeVFX;
    

    private void Awake()
    {         
        rb = GetComponent<Rigidbody>();
        playerControls = new PlayerControls();
        audioManager = GetComponent<PlayerAudioManager>();
        health = GetComponent<PlayerHealth>();
        canvas = UnityEngine.Object.FindFirstObjectByType<Canvas>().GetComponent<UpdateCanvas>();
        var temp = GameObject.Find("Canvas");
        canvas = temp.GetComponent<UpdateCanvas>();
        loopyModel = transform.GetChild(0).gameObject;
        loopyMesh = loopyModel.GetComponentInChildren<SkinnedMeshRenderer>();
        scrollManager = GetComponent<ScrollManager>();
        elementalDamage = new float[] { 1, 1, 1, 1 };//set to 1 as they multiply damage based on element type
        animator = loopyModel.GetComponent<Animator>();

    }

    private void Start()
    {
        elementsReferenceArray = Enum.GetValues(typeof(ElementTable.ElementName)).Cast<ElementTable.ElementName>().ToArray();
        elementProjectileArray = new GameObject[] { null, iceProjectile, fireProjectile, lightningProjectile, mercuryProjectile };

        Physics.autoSyncTransforms = true;

        chargeRate = baseChargeRate;

        
        //animator = GetComponentInChildren<Animator>();

        if (PlayerPrefs.GetInt("newElement") != 0)
        {
            playerSpells.Add((ElementTable.ElementName)PlayerPrefs.GetInt("newElement"));
        }

        canvas.UpdateSpellList(playerSpells);
        canvas.UpdateSpellUI(element);

        loopyMesh.material = loopyElementMaterials[(int)playerSpells[element] - 1];

        //scrollManager.UpdateAtributes();
    }

    private void FixedUpdate()
    {
        //calculating movespeed including after slow effects are applied and setting player velocity
        float moveForceAfterSlow = baseMoveForce - (currentCharge * chargeSlow);
        if (moveForceAfterSlow < 0) moveForceAfterSlow = 0;

        Vector3 inputDir = new Vector3(moveInput.y, 0, -moveInput.x).normalized; /* Idk why the vector need to come out this way???????? */

        if (buttonIsPressed == true)
        {
            //player movement in regards to it's velocity to have a constant speed
            rb.AddForce((moveForceAfterSlow - (rb.velocity.magnitude * moveDrag)) * inputDir);
        }
        
        //If input direction is opposing current velocity
        if (buttonIsPressed == false || Vector3.Dot(rb.velocity.normalized, inputDir) <= 0f)
        {
            //Releases a counterforce when the player stops so that the player stops smoother

            rb.AddForce(-rb.velocity * stoppingDrag);
        }
        
        //Capping rb.velocity to never exceed maxMoveSpeed. (Using unity Rigidbody.AddForce for this sucks so just set the value)
        if(rb.velocity.magnitude > maxMoveSpeed)
        {
            rb.velocity = maxMoveSpeed * rb.velocity.normalized;
        }

        //Debug.Log(rb.velocity.magnitude);

        //Animation test
        //animator.SetFloat("Speed", rb.velocity.x);

    }


    private void Update()
    {

        //Capping rb.velocity to never exceed maxMoveSpeed. (Using unity Rigidbody.AddForce for this sucks so just set the value)
        if (rb.velocity.magnitude > maxMoveSpeed)
        {
            rb.velocity = maxMoveSpeed * rb.velocity.normalized;
        }

        //Rotates around player every frame
        //Vector3 relativePos = new Vector3(cursorPosition.position.x, 0, cursorPosition.position.z);
        Vector3 relativePos = cursorPosition.transform.position - transform.position;
        relativePos.y = 0;
        loopyModel.transform.rotation = Quaternion.LookRotation(relativePos);

        Vector3 relativeVelocity = (Quaternion.Inverse(loopyModel.transform.rotation) * rb.velocity).normalized;

        animator.SetFloat("RunDirX", relativeVelocity.x);
        animator.SetFloat("RunDirY", relativeVelocity.z);
    }

    /// <summary>
    /// Reading Movement inputs
    /// </summary>
    /// <param name="context"></param>
    public void OnMoveInput (InputAction.CallbackContext context)
    {
        buttonIsPressed = true;
        moveInput = context.ReadValue<Vector2>();
        //rb.AddForce(transform.forward * moveDirection);
        animator.SetBool("IsRunning", true);

        /*var temp = moveDirection.x;
        moveDirection.x = -moveDirection.y;
        moveDirection.y = temp;*/

        if(context.canceled)
        {
            //----- might be able to use moveDirection.magnitude instead of the extra variable and if statement not 100% sure ----//
            animator.SetBool("IsRunning", false);
            buttonIsPressed = false;
        }
    }

    /// <summary>
    /// Reading Shoot input
    /// </summary>
    /// <param name="context"></param>
    public void OnShootInput(InputAction.CallbackContext context)
    {
        //when pressing shoot button
        if(context.started)
        {
            //set charge for while loop in coroutine (probably redundant now)
            currentCharge = 1f;
            animator.SetInteger("Attack Stage", 1);
            
            
        }
        //when releasing shoot button
        else if(context.canceled)
        {
            //stopping coroutine, clamping charge, firing projectile, resetting charge
            StopCoroutine(AttackCharge());
            animator.SetInteger("Attack Stage", 2);
        }
    }

    public void OnRScrollWheel(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if (value > 0)
        {
            element++;
            if (element > playerSpells.Count - 1)
                element = 0;

            
        }
        else if(value<0)
        {
            element--;
            if (element < 0)
                element = playerSpells.Count - 1;

            loopyChangeVFX.SetActive(false);
        }

        if(!loopyChangeVFX.activeSelf)
            StartCoroutine(LoopyChangeVFX());

        canvas.UpdateSpellUI(element);

        loopyMesh.material = loopyElementMaterials[(int)playerSpells[element] - 1];
    }

    public void OnNumberInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            int value = (int)context.ReadValue<float>() - 1;
            if (value < playerSpells.Count)
            {
                element = value;
                canvas.UpdateSpellUI(element);
                loopyMesh.material = loopyElementMaterials[(int)playerSpells[element] - 1];
                if (!loopyChangeVFX.activeSelf)
                    StartCoroutine(LoopyChangeVFX());
            }
        }
    }

    /// <summary>
    /// Projectile firing logic
    /// </summary>
    private void ShootProjectile(float chargeScale)
    {
        //checking if enough time has passed since last attack
        if (Time.time > attackTimeStamp + attackCooldown)
        {
            //knockback attempt on player
            if (chargeScale >= 0.1f)/* magic number denoting charge for just pressing the button */
            {
                rb.AddForce(((transform.position -
                    new Vector3(cursorPosition.position.x, transform.position.y, cursorPosition.position.z))).normalized
                    * shootKnockback, ForceMode.VelocityChange);
            }

            //saving time attack was fired for cooldown
            attackTimeStamp = Time.time;

            //foreach(ElementTable.ElementName element in Enum.GetValues(typeof(ElementTable.ElementName)))
            //{
            //    if()
            //}

            GameObject projectile = Instantiate(elementProjectileArray[(int)playerSpells[element]], projectileSpawnPosition.position, Quaternion.identity);
            float newScale = baseSize * chargeScale;
            projectile.transform.localScale = new Vector3(newScale, newScale, newScale);
            Vector3 projectileDirection = (-(transform.position - cursorPosition.position)).normalized;
            projectile.GetComponent<Rigidbody>().AddForce(projectileDirection * projectileSpeed, ForceMode.Force);
            projectile.transform.forward = -transform.GetChild(0).right;

            float crit = 1;
            if (UnityEngine.Random.Range(0f, 1f) < critChance)
                crit = critDamageMultiplier;

            projectile.GetComponent<ProjectileDamage>().SetVariables(this.gameObject, 
                elementalDamage[(int)playerSpells[element]-1] * currentCharge * crit , projectileLifeTime, currentCharge);//-1 to account for null enum

            if (currentCharge <= 1.85)
                audioManager.PlayPorjectileSound((int)playerSpells[element] - 1);
            else
                audioManager.PlayChargedPorjectileSound((int)playerSpells[element]-1);

            StopCoroutine(AttackCharge());
        }
    }

    IEnumerator AttackCharge()
    {
        //add charge if it is above 0 or below max (currentCharge > 0 probably redundant now)
        while (currentCharge >= 1 && currentCharge < maxCharge) 
        {
            currentCharge += chargeRate;
            yield return new WaitForFixedUpdate();
        }
    }

    public void StartChargeCoroutine()
    {
        StartCoroutine(AttackCharge());
    }

    IEnumerator LoopyChangeVFX()
    {
        loopyChangeVFX.SetActive(false);
        loopyChangeVFX.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        loopyChangeVFX.SetActive(false);
    }

    public void ReleaseAttack()
    {
        currentCharge = Mathf.Clamp(currentCharge, 1, maxCharge);
        ShootProjectile(currentCharge);
        currentCharge = 1;
    }

    public void UpdateCanvas()
    {
        canvas.UpdatePlayerStats();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}