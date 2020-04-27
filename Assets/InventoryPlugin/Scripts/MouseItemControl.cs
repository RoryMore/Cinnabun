using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class MouseItemControl : MonoBehaviour
{
    public InventoryItem mouseItem;
    public ItemTooltip tooltip;
    public InventoryBase invBase;
    EquipmentPanelControl equipPanelControl = null;

    public Canvas inventoryCanvas;

    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    [SerializeField]
    GameObject itemDrop = null;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        raycaster = inventoryCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        player = FindObjectOfType<Player>();

        tooltip.gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (mouseItem != null)
        {
            mouseItem.Initialise(gameObject, null as InventoryItem, transform.position, new Vector3(1.0f, 1.0f), true);
            mouseItem.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckItemClick();
        if (mouseItem.enabled)
        {
            transform.position = Input.mousePosition;
        }

        if (CheckItemHovered())
        {
            tooltip.gameObject.SetActive(true);
        }
        else
        {
            tooltip.gameObject.SetActive(false);
        }
        if (tooltip.enabled)
        {
            tooltip.transform.position = Input.mousePosition;
        }
    }

    bool CheckItemHovered()
    {
        foreach (RaycastResult result in GetNewPointerEventRaycast())
        {
            if (result.gameObject.name.Contains("InventoryItem"))
            {
                tooltip.gameObject.SetActive(true);
                InventoryItem hoveredItem = result.gameObject.GetComponent<InventoryItem>();
                tooltip.SetHoveredItem(hoveredItem.itemInfoBlock, hoveredItem.isEquipped);

                InventoryItem equippedComparison = invBase.GetEquippedItem(hoveredItem.itemData.equipmentSlot);
                if (equippedComparison != null)
                {
                    tooltip.SetEquippedItemInfo(equippedComparison.itemInfoBlock);
                    //Debug.Log("Tooltip set real hover info");
                }
                else
                {
                    InventoryItem.ItemInfoBlock emptyInfo = new InventoryItem.ItemInfoBlock();
                    tooltip.SetEquippedItemInfo(emptyInfo);
                }
                
                return true;
            }
        }
        return false;
    }

    void CheckItemClick()
    {
        // On a left click
        if (Input.GetMouseButtonDown(0))
        {
            // If we need to grab an item
            if (mouseItem.itemData == null)
            {
                foreach (RaycastResult result in GetNewPointerEventRaycast())
                {
                    // Picking up Items in Inventory or from an Equip Slot
                    // Is the currently checked object an inventoryItem
                    if (result.gameObject.name.Contains("InventoryItem"))
                    {
                        //Debug.Log("InventoryItem hit with click");
                        InventoryItem checkedResult = result.gameObject.GetComponent<InventoryItem>();

                        // Put item on our mouse
                        mouseItem.gameObject.SetActive(true);

                        mouseItem.SetItem(checkedResult);

                        // Item was equipped - unequipping now
                        if (checkedResult.isEquipped)
                        {
                            //invBase.playerEquippedItems.Remove(checkedResult);
                            CharacterPanelStatControl.OnItemRemove(mouseItem);

                            checkedResult.usedEquipSlot.isUsed = false;
                            checkedResult.usedEquipSlot.equippedItem = null;

                            mouseItem.isEquipped = true;
                            mouseItem.usedEquipSlot = checkedResult.usedEquipSlot;
                            //checkedResult.usedEquipSlot = null;
                            
                            //
                        }
                        else // Item was in inventory
                        {
                            foreach (InventorySlot slot in checkedResult.slotsUsed)
                            {
                                slot.isUsed = false;
                                slot.storedItem = null;
                            }
                        }

                        invBase.playerOwnedItems.Remove(checkedResult);
                        Destroy(checkedResult.gameObject);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0)) // We have an item grabbed
        {
            if (mouseItem.itemData != null)
            {
                // Check if we are hovering over an inventory slot we can drop the item into
                List<RaycastResult> mouseRaycastList = GetNewPointerEventRaycast();
                int incrementsWithoutUsableSlot = 0;
                foreach (RaycastResult result in mouseRaycastList)
                {
                    incrementsWithoutUsableSlot++;
                    // Item is being put in inventory slot
                    if (result.gameObject.name.Contains("InventorySlot"))
                    {
                        incrementsWithoutUsableSlot--;
                        InventorySlot checkedSlot = result.gameObject.GetComponent<InventorySlot>();

                        if (invBase.AddItem(mouseItem, checkedSlot.slotID))
                        {
                            //Debug.Log("ITEM WAS ADDED FROM MOUSE");

                            mouseItem.ClearItem();
                            mouseItem.gameObject.SetActive(false);
                        }
                        else // Otherwise put the item back in the inventory slots it was previously in
                        {
                            invBase.AddItem(mouseItem, mouseItem.slotsUsed[0].slotID);

                            //Debug.Log("ITEM WAS ADDED FROM MOUSE");

                            mouseItem.ClearItem();
                            mouseItem.gameObject.SetActive(false);
                        }
                    }
                    // Hovering over character render, so dropping here equips the item. Equip the item
                    else if (result.gameObject.name.Contains("Character Render Surface"))
                    {
                        if (equipPanelControl == null)
                        {
                            equipPanelControl = result.gameObject.GetComponent<EquipmentPanelControl>();
                        }
                        
                        if (equipPanelControl.EquipItem(mouseItem))
                        {
                            incrementsWithoutUsableSlot--;
                            //invBase.playerEquippedItems.Add(mouseItem);
                            mouseItem.ClearItem();
                            mouseItem.gameObject.SetActive(false);
                        }
                        else
                        {
                            invBase.AddItem(mouseItem);
                            incrementsWithoutUsableSlot--;
                            mouseItem.ClearItem();
                            mouseItem.gameObject.SetActive(false);
                        }
                    }
                }
                // Checking if the number of increments done in the list equals the list size
                // Meaning no inventory slots were hovered over
                // We are placing the item back in the slot it was originally in
                if (incrementsWithoutUsableSlot == mouseRaycastList.Count)
                {
                    bool itemDroppedToGround = false;

                    if (mouseItem.itemData != null)
                    {
                        Vector3 dropLocation = player.transform.position;
                        dropLocation.x += Random.Range(-2.0f, 2.0f);
                        dropLocation.z += Random.Range(-2.0f, 2.0f);
                        

                        Item droppedItem = Instantiate(itemDrop, dropLocation, Quaternion.identity).GetComponent<Item>();
                        droppedItem.Initialise(mouseItem.itemData, mouseItem.itemInfoBlock, 30.0f);

                        itemDroppedToGround = true;
                        mouseItem.ClearItem();
                        mouseItem.gameObject.SetActive(false);
                    }

                    if (!itemDroppedToGround)
                    {
                        if (invBase.AddItem(mouseItem, mouseItem.slotsUsed[0].slotID))
                        {
                            //Debug.Log("ITEM WAS ADDED FROM MOUSE - OUT OF INVENTORY SLOT BOUNDS");

                            mouseItem.ClearItem();
                            mouseItem.gameObject.SetActive(false);
                        }
                        else
                        {
                            if (invBase.AddItem(mouseItem))
                            {
                                //Debug.Log("ITEM WAS ADDED FROM MOUSE - OUT OF INVENTORY SLOT BOUNDS");

                                mouseItem.ClearItem();
                                mouseItem.gameObject.SetActive(false);
                            }
                            else
                            {

                                if (mouseItem.isEquipped)
                                {
                                    //Debug.Log("MouseItemControl: Re-equipping item. Not enough free inventory space to unequip");
                                    // Re-equip item in its slot.
                                    // Not enough inventory space
                                    equipPanelControl.EquipItem(mouseItem);
                                    //invBase.playerEquippedItems.Add(mouseItem);

                                    mouseItem.ClearItem();
                                    mouseItem.gameObject.SetActive(false);
                                }
                                else
                                {
                                    Debug.Log("MouseItemControl: Item was out of bounds. Tried adding to own slots, then any slots. This shouldn't be hit");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    List<RaycastResult> GetNewPointerEventRaycast()
    {
        pointerEventData = new PointerEventData(eventSystem);

        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointerEventData, results);

        return results;
    }
}
