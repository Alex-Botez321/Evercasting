using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateCanvas : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Sprite[] activeSpellIcons;
    [SerializeField] private Sprite[] inactiveSpellIcons;
    private List<ElementTable.ElementName> playerSpells;
    [SerializeField] private Image[] spellUI;
    [SerializeField] private Image[] flasks;
    [SerializeField] private Sprite fullFlask;
    [SerializeField] private Sprite emptyFlask;
    private int maxFlasks;
    [SerializeField] private TextMeshProUGUI playerStatsUI;
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private PageTurn pageTurn;

    private void Start()
    {
        Screen.SetResolution(3840, 2160, true);
    }

    public void UpdateHealth(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
    }

    public void UpdateSpellUI(int currentSpell)
    {

        int lastSpell = currentSpell-1;
        int nextSpell = currentSpell+1;

        if(lastSpell < 0)
            lastSpell = playerSpells.Count - 1;
        if (nextSpell > playerSpells.Count - 1)
            nextSpell = 0;

        spellUI[0].sprite = inactiveSpellIcons[(int)playerSpells[lastSpell]-1];
        spellUI[1].sprite = activeSpellIcons[(int)playerSpells[currentSpell]-1];
        spellUI[2].sprite = inactiveSpellIcons[(int)playerSpells[nextSpell]-1];
    }

    public void InitializeFlasks(int _maxFlasks)
    {
        maxFlasks = _maxFlasks;
        for(int i = 0; i < maxFlasks; i++)
        {
            flasks[i].enabled = true;
        }
    }

    public void UpdateFlasks(int currentFlasks)
    {
        for(int i = maxFlasks; i >= currentFlasks; i--)
        {
            flasks[i].sprite = emptyFlask;
        }
    }

    public void UpdateSpellList(List<ElementTable.ElementName> spellList)
    {
        playerSpells = spellList;
    }

    public void PageTurnAnimation()
    {
        pageTurn.OnTeleport();
    }

    public void UpdatePlayerStats()
    {
        /* Base -> Current
         * HP
         * Move speed
         * Charge Speed
         * Proj Speed
         * Proj Duration
         * Res Ice
         * Res Fire
         * Res Lightning
         * Res Mercury
         * DMG Ice
         * DMG Fire
         * DMG Lightning
         * DMG Mercury
         * Crit Chance
         * Crit Damage
         */

        playerStatsUI.text =
            "Health Points: 100 + " + (health.maxHealth - health.baseMaxHealth) + "\n" + 
            "Movement Speed: 7 + " + (controller.maxMoveSpeed - controller.baseMoveSpeed) + "\n" +
            "Charge Speed: 0.2 + " + (controller.chargeRate - controller.baseChargeRate) + "\n" +
            "Projectile Speed: 600 + " + (controller.baseProjectileSpeed - controller.projectileSpeed) + "\n" +
            "Projectile Duration: 1 + " + (controller.projectileLifeTime - controller.baseProjectileSpeed) + "\n" +
            "Ice Resistance: 0 + " + (health.resistances[0] - health.baseColdResistance) + "\n" +
            "Fire Resistance: 0 + " + (health.resistances[1] - health.baseHeatResistance) + "\n" +
            "Lightning Resistance: 0 + " + (health.resistances[2] - health.baseElectricResistance) + "\n" +
            "Mercury Resistance: 0 + " + (health.resistances[3] - health.basePoisonResistance) + "\n" +
            "Ice Damage: 10 + " + (controller.elementalDamage[0] - 10) + "\n" +
            "Fire Damage: 10 + " + (controller.elementalDamage[1] - 10) + "\n" +
            "Lightning Damage: 10 + " + (controller.elementalDamage[2] - 10) + "\n" +
            "Mercury Damage: 10 + " + (controller.elementalDamage[3] - 10) + "\n" +
            "Crit Chance: 0 + " + (controller.critChance - controller.baseCritChance) + "\n" +
            "Crit Damage: 2 + " + (controller.critDamageMultiplier - controller.baseCritDamageMultiplier) + "\n"
            ;

    }
}
