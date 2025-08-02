using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ScrollManager : MonoBehaviour
{
    [Header("Scrolls")]
    [SerializeField] private List<Scroll> equippedScrolls;
    private List<List<Scroll>> scrolls;
    [SerializeField] private List<Scroll> mundaneScrolls;
    [SerializeField] private List<Scroll> uniqueScrolls;
    [SerializeField] private List<Scroll> mythicScrolls;
    [SerializeField] private List<Scroll> archmageScrolls;
    [SerializeField] private List<GameObject> ScrollChoices;
    [SerializeField] private GameObject ChooseYourSpell;
    [SerializeField] private GameObject DullBackground;

    private PlayerController playerController;
    private Health healthManager;
    [SerializeField] private float resistanceLimit = 0.9f;

    [Header("ChanceToSpawn")]
    [SerializeField] private float mundaneChance = 0.4f;
    [SerializeField] private float uniqueChance = 0.3f;
    [SerializeField] private float mythicChance = 0.2f;
    [SerializeField] private float archmageChance = 0.1f;
    private float totalChance;
    private float[] chanceArray; 

    [Header("Debug stats")]
    [SerializeField] public List<float> playerAttributes;
    [SerializeField] public List<float> projectileAttributes;

    [Header("Saving")]
    private int scrollCount = 0;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        healthManager = GetComponent<Health>();
    }

    private void Start()
    {
        totalChance = mundaneChance + uniqueChance + mythicChance + archmageChance;
        scrolls = new List<List<Scroll>> { mundaneScrolls, uniqueScrolls, mythicScrolls, archmageScrolls };
        chanceArray = new float[4] { mundaneChance, uniqueChance, mythicChance, archmageChance};

        for (int i = 0; i < (int)Scroll.PlayerAttributes.Length; i++)
        {
            playerAttributes.Add(0);
        }
        for (int i = 0; i < (int)Scroll.ProjectileAttributes.Length; i++)
        {
            projectileAttributes.Add(0);
        }

        for (int i = 0; i < PlayerPrefs.GetInt("count"); i++)
        {
            equippedScrolls.Add(scrolls[PlayerPrefs.GetInt(i.ToString()+"rarity")][PlayerPrefs.GetInt((i).ToString()+"index")]);
            scrolls[PlayerPrefs.GetInt(i.ToString() + "rarity")].RemoveAt(PlayerPrefs.GetInt((i).ToString() + "index"));
        }
        
        UpdateAtributes();
    }

    public void OnCheating(InputAction.CallbackContext context)
    {
        if(context.started)
            EnableChoiceScrolls();
    }

    public void EnableChoiceScrolls()
    {   
        foreach (GameObject choice in ScrollChoices)
        {
            int rarity = 0;
            bool scrollFound = false;
            int iterationCounter = 0;
            while (scrollFound == false)
            {
                float rarityRoll = Random.Range(0f, 1f);
                for (rarity = 0; rarity < chanceArray.Length; rarity++)
                {
                    if (rarityRoll <= chanceArray[rarity]) break;
                }
                if (scrolls[rarity].Count > 0)
                {
                    scrollFound = true;
                    break;
                }
                if (iterationCounter > 10) break;
                iterationCounter++;
            }
            

            int scrollIndex = Random.Range(0, scrolls[rarity].Count);
            ScrollChoiceItem item = choice.GetComponent<ScrollChoiceItem>();
            item.SetUp(scrolls[rarity][scrollIndex], scrollIndex, rarity);
            choice.SetActive(true);
        }

        ChooseYourSpell.SetActive(true);
        DullBackground.SetActive(true);

    }

    public void EquipScroll(int index, int rarity)
    {
        equippedScrolls.Add(scrolls[rarity][index]);
        scrolls[rarity].RemoveAt(index);
        foreach (GameObject choice in ScrollChoices)
        {
            choice.SetActive(false);
        }

        ChooseYourSpell.SetActive(false);
        DullBackground.SetActive(false);

        //Save Scrolls
        scrollCount++;
        PlayerPrefs.SetInt("count", scrollCount);
        PlayerPrefs.SetInt(scrollCount.ToString()+"rarity", rarity);
        PlayerPrefs.SetInt(scrollCount.ToString()+"index", index);

        UpdateAtributes();
    }

    public void UpdateAtributes()
    {
        //reset values to base value before re-calculating
        //0 for additive stacking
        //1 for multiplicative stacking
        playerAttributes[(int)Scroll.PlayerAttributes.IncreaseHealth] = 0;
        playerAttributes[(int)Scroll.PlayerAttributes.ColdResist] = 0;
        playerAttributes[(int)Scroll.PlayerAttributes.HeatResist] = 0;
        playerAttributes[(int)Scroll.PlayerAttributes.ElectricResist] = 0;
        playerAttributes[(int)Scroll.PlayerAttributes.PoisonResist] = 0;
        playerAttributes[(int)Scroll.PlayerAttributes.IncreaseChargeSpeed] = 1;
        playerAttributes[(int)Scroll.PlayerAttributes.MoveSpeed] = 0;

        projectileAttributes[(int)Scroll.ProjectileAttributes.ProjectileSpeed] = 0;
        projectileAttributes[(int)Scroll.ProjectileAttributes.CritChance] = 0;
        projectileAttributes[(int)Scroll.ProjectileAttributes.CritDamage] = 1;
        projectileAttributes[(int)Scroll.ProjectileAttributes.ColdDamage] = 1;
        projectileAttributes[(int)Scroll.ProjectileAttributes.HeatDamage] = 1;
        projectileAttributes[(int)Scroll.ProjectileAttributes.ElectricDamage] = 1;
        projectileAttributes[(int)Scroll.ProjectileAttributes.PoisonDamage] = 1;

        //adding scrolls to multipliers and stats
        foreach (Scroll scroll in equippedScrolls)
        {
            for(int i = 0; i < scroll.playerAttributes.Length; i++)
            {
                playerAttributes[(int)scroll.playerAttributes[i]] += scroll.playerAttributesValues[i];
            }
            for (int i = 0; i < scroll.projectileAttributes.Length; i++)
            {
                projectileAttributes[(int)scroll.projectileAttributes[i]] += scroll.projectileAttributesValues[i];
            }
        }

        healthManager.maxHealth = healthManager.baseMaxHealth + playerAttributes[(int)Scroll.PlayerAttributes.IncreaseHealth];
        healthManager.currentHealth = healthManager.maxHealth;//need to change as it's a health refresh

        for (int i = 0; i < healthManager.resistances.Length; i++)
        {
            if (playerAttributes[i+1] > resistanceLimit)
                healthManager.resistances[i] = resistanceLimit;
            else healthManager.resistances[i] = playerAttributes[i+1];//+1 accounts for null values in enum
        }

        playerController.chargeRate = playerController.baseChargeRate * playerAttributes[(int)Scroll.PlayerAttributes.IncreaseChargeSpeed];
        playerController.maxMoveSpeed = playerController.baseMoveSpeed + playerAttributes[(int)Scroll.PlayerAttributes.MoveSpeed];

        playerController.elementalDamage[0] = projectileAttributes[(int)Scroll.ProjectileAttributes.ColdDamage];
        playerController.elementalDamage[1] = projectileAttributes[(int)Scroll.ProjectileAttributes.HeatDamage];
        playerController.elementalDamage[2] = projectileAttributes[(int)Scroll.ProjectileAttributes.ElectricDamage];
        playerController.elementalDamage[3] = projectileAttributes[(int)Scroll.ProjectileAttributes.PoisonDamage];
        playerController.critChance = projectileAttributes[(int)Scroll.ProjectileAttributes.CritChance] + playerController.baseCritChance;
        playerController.projectileSpeed = projectileAttributes[(int)Scroll.ProjectileAttributes.ProjectileSpeed] + playerController.baseProjectileSpeed;
        playerController.projectileLifeTime = projectileAttributes[(int)Scroll.ProjectileAttributes.ProjectileDuration];
        playerController.critDamageMultiplier = projectileAttributes[(int)Scroll.ProjectileAttributes.CritDamage] * playerController.baseCritDamageMultiplier;

        playerController.UpdateCanvas();
    }
}
