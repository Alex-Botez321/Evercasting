using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Scroll", menuName = "Srolls", order = 1)]
public class Scroll : ScriptableObject
{
    [HideInInspector] public enum Rarity { Mundane, Unique, Mythic, Archmage };
    [HideInInspector] public enum DamageType { Cold, Heat, Electric, Poison }

    public enum PlayerAttributes
    {
        IncreaseHealth,
        ColdResist,
        HeatResist,
        ElectricResist,
        PoisonResist,
        IncreaseChargeSpeed,
        MoveSpeed,
        Length
    }
    public enum ProjectileAttributes
    {
        ProjectileDuration,
        ProjectileSize,
        ColdDamage,
        HeatDamage,
        ElectricDamage,
        PoisonDamage,
        ProjectileSpeed,
        CritChance,
        CritDamage,
        Length
    }

    [Header("UI Variables")]
    [SerializeField] public string itemName;
    [SerializeField] public Sprite itemIcon;
    [SerializeField] public Sprite backgroundSprite;
    [SerializeField] public Sprite itemDescription;
    [SerializeField] public Sprite itemLore;
    [SerializeField] public Sprite shadowedScroll;

    [Header("Scroll Attributes")]

    [TextArea(1, 2)]
    [SerializeField] public Rarity rarity = Rarity.Mundane;
    [SerializeField] private string Description = "Assign desired attributes and add the same amount of modifiers with correct values attached. " +
        "Length is used back end do not use as modifier";

    [SerializeField] public PlayerAttributes[] playerAttributes;
    [SerializeField] public float[] playerAttributesValues;

    [SerializeField] public ProjectileAttributes[] projectileAttributes;
    [SerializeField] public float[] projectileAttributesValues;
}