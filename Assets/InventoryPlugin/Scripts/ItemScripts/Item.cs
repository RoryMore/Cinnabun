using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public InventoryItem.ItemStatBlock itemStatBlock;

    [Header("Item World-Object Settings")]
    [Tooltip("The amount of time (seconds) until the item will get destroyed if not picked up")]
    public float lifetime;
    // The amount of time the item has been alive
    [SerializeField]
    float timeAlive;
    [SerializeField]
    // If the item has been clicked & the Player is nearby, give the item to the Player. Reset the bool if a click has been input but not on the Item
    bool itemClicked;

    Player player;
    RespawnControl resCon;
    InventoryBase inventoryBase;

    bool isNewItem = true;

    Material material;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        itemClicked = false;

        timeAlive = 0.0f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        resCon = player.gameObject.GetComponent<RespawnControl>();
        if (resCon == null)
        {
            Debug.LogError("Spawned Item could not reference RespawnControl");
        }

        inventoryBase = resCon.inventoryBase;
        if (inventoryBase == null)
        {
            Debug.LogError("Spawned Item could not reference InventoryBase");
        }

        if (isNewItem)
        {
            if (itemData != null)
            {
                if (itemData.applyRandomStats)
                {
                    itemStatBlock = itemData.GetRandomItemStats();
                }
                else
                {
                    itemStatBlock = itemData.GetSetItemStats();
                }

                meshRenderer = GetComponent<MeshRenderer>();
                material = new Material(Shader.Find("Standard"));
                material.SetTexture("_MainTex", itemData.equippedSprite.texture);
                meshRenderer.material = material;
            }
        }
    }

    /// <summary>
    /// This Initialise method will only need to be called if an Item is being created not directly from the prefab; i.e when the player is dropping an item from their inventory
    /// </summary>
    public void Initialise(ItemData data, InventoryItem.ItemStatBlock stats, float setLifetime, bool isNew = false)
    {
        isNewItem = isNew;

        itemClicked = false;

        lifetime = setLifetime;
        timeAlive = 0.0f;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        resCon = player.gameObject.GetComponent<RespawnControl>();
        inventoryBase = resCon.inventoryBase;

        itemData = data;
        itemStatBlock = stats;

        meshRenderer = GetComponent<MeshRenderer>();
        material = new Material(Shader.Find("Standard"));
        material.SetTexture("_MainTex", itemData.equippedSprite.texture);
        meshRenderer.material = material;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLifetime();
        UpdateClickState();
        EvaluateItemGiven();
    }

    void UpdateLifetime()
    {
        if (timeAlive < lifetime)
        {
            if (!itemClicked)
            {
                timeAlive += Time.deltaTime;
            }

            // Make Item move
            transform.position = new Vector3(transform.position.x, transform.position.y + (Mathf.Sin(Time.time * 4.0f) * (0.5f * Time.deltaTime)), transform.position.z);
            transform.Rotate(0.0f, Time.deltaTime * 20.0f, 0.0f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void UpdateClickState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 400.0f))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    itemClicked = true;
                }
                else
                {
                    itemClicked = false;
                }
            }
        }
    }

    void EvaluateItemGiven()
    {
        // USING THIS FUNCTION INSTEAD OF Unity COLLISION DETECTION FUNCTIONS, BECAUSE OBJECT REQUIRES A Rigidbody COMPONENT FOR THOSE FUNCTIONS TO WORK
        if (itemClicked)
        {
            if (Mathf.Abs(transform.position.y - player.transform.position.y) <= 2.0f)
            {
                Vector3 pos, playerPos;
                pos = transform.position;
                pos.y = 0.0f;
                playerPos = player.transform.position;
                playerPos.y = 0.0f;

                float distance = Vector3.Distance(pos, playerPos);
                if (distance <= 1.0f)
                {
                    // Give item to Player
                    if (inventoryBase.AddItem(this))
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
