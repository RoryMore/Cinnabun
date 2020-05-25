using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [Header("The type of slot this is")]
    public ItemData.EquipmentSlot equipmentSlot;
    [Header("If the slot is currently in use")]
    public bool isUsed;
    [Header("The currently equipped item in this slot")]
    public ItemData equippedItem;

    public static float width;
    public static float height;

    int id;

    RectTransform rect;
    Image image;

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Initialise(GameObject parent, RectTransform parentRect, float _width, float _height, float offset, ItemData.EquipmentSlot equipSlot, InventorySlot.SlotID slotID)
    {
        bool shiftedAcross = false;

        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        width = _width;
        height = _height;

        id = slotID.x;

        rect.SetParent(parent.transform);
        rect.anchorMin = new Vector2(0, 1.0f);
        rect.anchorMax = new Vector2(0, 1.0f);

        rect.sizeDelta = new Vector2(width, height);
        
        Vector2 slotPosition = new Vector2(offset, -offset);
        rect.anchoredPosition = slotPosition;

        slotPosition.y -= (height * id);

        // If this equipment slot is below the height of the parent RectTransform
        // Change its anchor to anchor to the right hand top corner
        // Then make it sit from the top right corner going down from there
        if (slotPosition.y - height < -parentRect.rect.height)
        {
            Debug.Log("This one is out of bounds");

            // Reset the height
            slotPosition.y = -offset;
            slotPosition.y -= (height * slotID.y);

            slotPosition.x += (parentRect.rect.width - width - offset);

            shiftedAcross = true;
        }

        rect.anchoredPosition = slotPosition;
        rect.localScale = new Vector2(1, 1);

        equipmentSlot = equipSlot;

        switch (equipmentSlot)
        {
            case ItemData.EquipmentSlot.Head:
                if (EquipPanelSprites.head != null)
                {
                    image.sprite = EquipPanelSprites.head;
                }
                break;

            case ItemData.EquipmentSlot.Shoulders:
                if (EquipPanelSprites.shoulders != null)
                {
                    image.sprite = EquipPanelSprites.shoulders;
                }
                break;

            case ItemData.EquipmentSlot.Chest:
                if (EquipPanelSprites.chest != null)
                {
                    image.sprite = EquipPanelSprites.chest;
                }
                break;

            case ItemData.EquipmentSlot.Legs:
                if (EquipPanelSprites.legs != null)
                {
                    image.sprite = EquipPanelSprites.legs;
                }
                break;

            case ItemData.EquipmentSlot.Feet:
                if (EquipPanelSprites.feet != null)
                {
                    image.sprite = EquipPanelSprites.feet;
                }
                break;

            case ItemData.EquipmentSlot.Hands:
                if (EquipPanelSprites.hands != null)
                {
                    image.sprite = EquipPanelSprites.hands;
                }
                break;

            case ItemData.EquipmentSlot.Weapon:
                if (EquipPanelSprites.weapon != null)
                {
                    image.sprite = EquipPanelSprites.weapon;
                }
                break;

            case ItemData.EquipmentSlot.Ring1:
                if (EquipPanelSprites.ring1 != null)
                {
                    image.sprite = EquipPanelSprites.ring1;
                }
                break;

            case ItemData.EquipmentSlot.Ring2:
                if (EquipPanelSprites.ring2 != null)
                {
                    image.sprite = EquipPanelSprites.ring2;
                }
                break;

            case ItemData.EquipmentSlot.Neck:
                if (EquipPanelSprites.neck != null)
                {
                    image.sprite = EquipPanelSprites.neck;
                }
                break;

            default:
                break;
        }

        return shiftedAcross;
    }
}
