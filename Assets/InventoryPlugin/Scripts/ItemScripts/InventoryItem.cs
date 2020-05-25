using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public struct ItemInfoBlock
    {
        public int strength;
        public int agility;
        public int constitution;
        public int intellect;
        public int physicalArmour;
        public int magicalArmour;

        public string itemName;
        public ItemData.ItemRarity rarity;
        public EquipmentTrait equipmentTrait;
    }

    public ItemInfoBlock itemInfoBlock;

    // The item this inventoryItem is representing
    public ItemData itemData;

    float width;
    float height;

    RectTransform rect;

    public List<InventorySlot> slotsUsed;
    public bool isMouseItem = false;

    public bool isEquipped = false;
    public EquipmentSlot usedEquipSlot;

    Image image;

    //public EquipmentTrait equipmentTrait = null;

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Initialise(GameObject parentObj, Item insertedItem, Vector3 invPosition, Vector3 invScale, bool mouseItem = false)
    {
        slotsUsed = new List<InventorySlot>();
        rect = GetComponent<RectTransform>();
        rect.SetParent(parentObj.transform);
        image = GetComponent<Image>();

        SetItem(insertedItem);

        rect.position = invPosition;
        rect.localScale = invScale;

        isMouseItem = mouseItem;

        return true;
    }

    public void Initialise(GameObject parentObj, Item insertedItem, Vector3 invPosition, Vector3 invScale, Vector2 invPivot, bool mouseItem = false)
    {
        slotsUsed = new List<InventorySlot>();
        rect = GetComponent<RectTransform>();
        rect.SetParent(parentObj.transform);
        image = GetComponent<Image>();

        SetItem(insertedItem);

        rect.position = invPosition;
        rect.localScale = invScale;
        rect.pivot = invPivot;

        isMouseItem = mouseItem;
    }

    public void Initialise(GameObject parentObj, InventoryItem insertedItem, Vector3 invPosition, Vector3 invScale, Vector2 invPivot, bool mouseItem = false)
    {
        slotsUsed = new List<InventorySlot>();
        rect = GetComponent<RectTransform>();
        rect.SetParent(parentObj.transform);
        image = GetComponent<Image>();

        SetItem(insertedItem);

        rect.position = invPosition;
        rect.localScale = invScale;
        rect.pivot = invPivot;

        isMouseItem = mouseItem;
    }

    public void Initialise(GameObject parentObj, InventoryItem insertedItem, Vector3 invPosition, Vector3 invScale, bool mouseItem = false)
    {
        slotsUsed = new List<InventorySlot>();
        rect = GetComponent<RectTransform>();
        rect.SetParent(parentObj.transform);
        image = GetComponent<Image>();

        SetItem(insertedItem);

        rect.position = invPosition;
        rect.localScale = invScale;

        isMouseItem = mouseItem;
    }

    public void SetWidth(float width)
    {
        Vector2 updatedWidth = new Vector2(width, rect.sizeDelta.y);
        rect.sizeDelta = updatedWidth;
    }
    public void SetHeight(float height)
    {
        Vector2 updatedHeight = new Vector2(rect.sizeDelta.x, height);
        rect.sizeDelta = updatedHeight;
    }

    public void SetItem(Item givenItem)
    {
        itemData = givenItem.itemData;
        itemInfoBlock = givenItem.itemStatBlock;
        //equipmentTrait = givenItem.equipmentTrait;

        if (itemData != null)
        {
            image.sprite = itemData.inventorySprite;

            width = InventorySlot.width * itemData.inventorySpaceX;
            height = InventorySlot.height * itemData.inventorySpaceY;

            rect.sizeDelta = new Vector2(width, height);
        }
    }

    public void SetItem(InventoryItem givenItem, bool isEquipping = false)
    {
        if (givenItem != null)
        {
            itemData = givenItem.itemData;
            itemInfoBlock = givenItem.itemInfoBlock;
        }

        if (itemData != null)
        {

            if (isEquipping)
            {
                image.sprite = itemData.equippedSprite;

                width = EquipmentSlot.width * 0.9f;
                height = EquipmentSlot.height * 0.9f;
            }
            else
            {
                image.sprite = itemData.inventorySprite;

                width = InventorySlot.width * itemData.inventorySpaceX;
                height = InventorySlot.height * itemData.inventorySpaceY;
            }

            rect.sizeDelta = new Vector2(width, height);

            slotsUsed.Clear();
            slotsUsed.AddRange(givenItem.slotsUsed);
        }
        else
        {
            itemData = null;
            slotsUsed.Clear();
        }
    }

    public void ClearItem()
    {
        itemData = null;

        image.sprite = null;

        width = 0;
        height = 0;
        rect.sizeDelta = new Vector2(width, height);

        slotsUsed.Clear();
    }
}
