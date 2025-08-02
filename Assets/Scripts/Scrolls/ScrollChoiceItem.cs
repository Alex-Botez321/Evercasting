using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollChoiceItem : MonoBehaviour
{
    //UI
    [SerializeField] private Image icon;
    [SerializeField] private Image description;
    [SerializeField] private Image lore;
    [SerializeField] private Image background;
    [SerializeField] private Sprite backgroundSprite;
    [SerializeField] private Sprite shadow;
    [SerializeField] private Button button;
    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private ScrollManager scrollManager;
    private int scrollIndex = 0;
    private int rarity = (int)Scroll.Rarity.Mundane;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void FixedUpdate()
    {
        if (eventSystem.currentSelectedGameObject == this.gameObject)
            background.sprite = shadow;
        else
            background.sprite = backgroundSprite;
    }

    public void SetUp(Scroll scroll, int index, int _rarity)
    {
        icon.sprite = scroll.itemIcon;
        description.sprite = scroll.itemDescription;
        lore.sprite = scroll.itemLore;
        backgroundSprite = scroll.backgroundSprite;
        scrollIndex = index;
        rarity = _rarity;
        shadow = scroll.shadowedScroll;
    }

    public void OnClick()
    {
        scrollManager.EquipScroll(scrollIndex, rarity);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(this.gameObject.name + " was selected");
    }
}
