using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// All logic and data for elemental collisions. Ensure only 1 ElementTable.cs is in the scene
/// Potential issues with collisions happening on the same frame and collisions not triggering correctly
/// Feel free to edit as needed
/// </summary>
public class ElementTable : MonoBehaviour
{
    public static ElementTable instance { get; private set; }

    public enum ElementName { Null, Ice, Fire, Lightning, Mercury, Length }
    public ElementName firstElement { get; set; }
    public ElementName secondElement { get; set; }
    public float damageMultiplier = 1f;
    public Vector3 reactionPosition { get; set; }

    public delegate void ElementReaction(Vector3 Position);
    ElementReaction elementReaction = null;
    private ElementReaction[,] elementTable = new ElementReaction[(int)ElementName.Length, (int)ElementName.Length];

    private int layerNumber = 8;
    private int layerMask;
    private float maxRayDistance = 100;

    //private ReactionTextPopUp popupTextCanvas;
    [SerializeField]private ReactionTextPopUp reactionTextCanvas;
    private float textLifetime = 2f;

    #region Reaction Results Prefabs

    private Quaternion rotation = Quaternion.identity;
    [SerializeField] GameObject WaterPrefab;
    [SerializeField] GameObject PlasmaPrefab;
    [SerializeField] GameObject TeslaPrefab;
    [SerializeField] GameObject BismuthPrefab;
    [SerializeField] GameObject DamascusPrefab;
    [SerializeField] GameObject FerroFluidPrefab;

    #endregion


    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else instance = this;

        firstElement = ElementName.Null;
        secondElement = ElementName.Null;

        elementTable = new ElementReaction[(int)ElementName.Length, (int)ElementName.Length];

        PopulateElementTable();

        //popupTextCanvas = FindFirstObjectByType<Canvas>().GetComponent<ReactionTextPopUp>();


        layerMask = 1 << layerNumber; //bitwise shift to represent the layer number 8(ground) for the raycasts on the water spawning
    }
    
    /// <summary>
    /// Assign the correct function to the delegate, call it then clean up
    /// </summary>
    public void ElementChecker()
    {
        //checking elements collided and assigning function to delegate
        if (elementTable[(int)firstElement, (int)secondElement] != null)
            elementReaction = elementTable[(int)firstElement, (int)secondElement];
        else if (elementTable[(int)secondElement, (int)firstElement] != null)
            elementReaction = elementTable[(int)secondElement, (int)firstElement];
        else
            elementReaction = NullReaction;

        Debug.Log(firstElement);
        Debug.Log(secondElement);

        elementReaction(reactionPosition);
        ElementCleanUp();
    }

    /// <summary>
    /// //TO WHOEVER WORKING: this is rection function to fix
    /// </summary>
    /// <param name="position"></param>
    /// <param name="reaction"></param>

    public void CreateReactionText(Vector3 position, string reaction)
    {
        //ReactionTextPopUp textPopUp = Instantiate(reactionText, position, Quaternion.identity).GetComponent<ReactionTextPopUp>();
        //GameObject newTextPopup = Instantiate(reactionTextPopup, position, rotation, GameObject.Find("DisplayTextCanvas1").transform);
        //newTextPopup.SetActive(true);
        reactionTextCanvas.SetText(reaction, position);
        ////newTextPopup.GetComponent<ReactionTextPopUp>().SetText(reaction);
        //Destroy(newTextPopup, textLifetime);

        //textPopUp.DisplayText(position, reaction);
    }


    #region Elemental Reactions
    private void IceFireReaction(Vector3 Position)
    {
        //Debug.Log("Water Created");
        //Debug.Log(reactionPosition);
        Position = new Vector3(Position.x, Position.y + 10, Position.z);
        RaycastHit groundHit;
        if(Physics.Raycast(Position, -Vector3.up, out groundHit, maxRayDistance, layerMask))
        {
            Debug.DrawLine(Position, groundHit.point, Color.magenta);
            Instantiate(WaterPrefab, groundHit.point, rotation);
            CreateReactionText(Position, "Water! Melt and Cleanse!"); //TO WHOEVER WORKING: this is rection function to fix
            ElementCleanUp();
        }
        //Instantiate(WaterPrefab, Position, rotation);
    }

    private void IceLightningReaction(Vector3 Position)
    {
        Position = new Vector3(Position.x, Position.y + 10, Position.z);
        RaycastHit groundHit;
        if (Physics.Raycast(Position, -Vector3.up, out groundHit, maxRayDistance, layerMask))
        {
            Debug.DrawLine(Position, groundHit.point, Color.green);
            GameObject tesla = Instantiate(TeslaPrefab, groundHit.point, rotation);
            tesla.GetComponent<TeslaDamage>().damageMultiplier = damageMultiplier;
            CreateReactionText(Position, "Tesla! Shockwave!");
            ElementCleanUp();
        }
    }

    private void FireLightningReaction(Vector3 Position)
    {
        Position = new Vector3(Position.x, Position.y + 10, Position.z);
        RaycastHit groundHit;
        if (Physics.Raycast(Position, -Vector3.up, out groundHit, maxRayDistance, layerMask))
        {
            Debug.DrawLine(Position, groundHit.point, Color.green);
            GameObject plasma = Instantiate(PlasmaPrefab, groundHit.point, rotation);
            plasma.GetComponent<PlasmaDamage>().damageMultiplier = damageMultiplier;
            CreateReactionText(Position, "Plasma! Expand and Ionize!");
            ElementCleanUp();
        }
    }

    private void IceMercuryReaction(Vector3 Position)
    {
        Position = new Vector3(Position.x, Position.y + 10, Position.z);
        RaycastHit groundHit;
        if (Physics.Raycast(Position, -Vector3.up, out groundHit, maxRayDistance, layerMask))
        {
            Debug.DrawLine(Position, groundHit.point, Color.green);
            Instantiate(BismuthPrefab, groundHit.point, rotation);
            CreateReactionText(Position, "Bismuth! Isn't it Pretty?" );
            ElementCleanUp();
        }
    }

    private void FireMercuryReaction(Vector3 Position)
    {
        Position = new Vector3(Position.x, Position.y + 10, Position.z);
        RaycastHit groundHit;
        if (Physics.Raycast(Position, -Vector3.up, out groundHit, maxRayDistance, layerMask))
        {
            Debug.DrawLine(Position, groundHit.point, Color.green);
            GameObject damascus = Instantiate(DamascusPrefab, groundHit.point, rotation);
            damascus.GetComponent<DamascusDamage>().damageMultiplier = damageMultiplier;
            CreateReactionText(Position, "Damascus! Disperce and Melt!");
            ElementCleanUp();
        }
    }
    private void LightningMercuryReaction(Vector3 Position)
    {
        Position = new Vector3(Position.x, Position.y + 20, Position.z);
        RaycastHit groundHit;
        if (Physics.Raycast(Position, -Vector3.up, out groundHit, maxRayDistance, layerMask))
        {
            Debug.DrawLine(Position, groundHit.point, Color.green);
            Instantiate(FerroFluidPrefab, groundHit.point, rotation);
            CreateReactionText(Position, "Ferrofluid! Magnetize and Slow!");
            ElementCleanUp();
        }
    }

    private void NullReaction(Vector3 Position)
    {
        elementReaction = null;
        ElementCleanUp();
    }
    #endregion

    /// <summary>
    /// Populating 2D Array with functions for each collision
    /// We can save memory at the cost of speed if we only populate 
    /// half the table and review the ElementChecker() logic
    /// </summary>
    private void PopulateElementTable()
    {
        elementTable[(int)ElementName.Ice, (int)ElementName.Fire] = IceFireReaction;
        elementTable[(int)ElementName.Fire, (int)ElementName.Ice] = IceFireReaction;
        elementTable[(int)ElementName.Ice, (int)ElementName.Lightning] = IceLightningReaction;
        elementTable[(int)ElementName.Lightning, (int)ElementName.Ice] = IceLightningReaction;
        elementTable[(int)ElementName.Fire, (int)ElementName.Lightning] = FireLightningReaction;
        elementTable[(int)ElementName.Lightning, (int)ElementName.Fire] = FireLightningReaction;
        elementTable[(int)ElementName.Ice, (int)ElementName.Mercury] = IceMercuryReaction;
        elementTable[(int)ElementName.Mercury, (int)ElementName.Ice] = IceMercuryReaction;
        elementTable[(int)ElementName.Fire, (int)ElementName.Mercury] = FireMercuryReaction;
        elementTable[(int)ElementName.Mercury, (int)ElementName.Fire] = FireMercuryReaction;
        elementTable[(int)ElementName.Lightning, (int)ElementName.Mercury] = LightningMercuryReaction;
        elementTable[(int)ElementName.Mercury, (int)ElementName.Lightning] = LightningMercuryReaction;
    }
    /// <summary>
    /// Empty first and second elements for the next collision
    /// </summary>
    private void ElementCleanUp()
    {
        firstElement = ElementName.Null;
        secondElement = ElementName.Null;
        damageMultiplier = 1;
    }
}
