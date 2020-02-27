using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOrbDropControl : MonoBehaviour
{
    [Tooltip("The Prefab that's Instantiated on method call 'DropItem'")]
    [SerializeField]
    GameObject drop;

    [Header("Drop Chance %")]
    [SerializeField]
    [Tooltip("0-100 value. Values >= 100 means this will always drop")]
    int dropChance;

    [Tooltip("The range[-randomOffset, randomOffset] which a random positional offset is determined when the item is instantiated. \nMost likely the Y component can be left as '0' as the height may be changed to keep it from spawning within other objects")]
    [SerializeField]
    Vector3 randomOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DropItem(Vector3 position)
    {
        int randomChanceHit = Random.Range(1, 101);

        if (randomChanceHit <= dropChance)
        {
            Instantiate(drop, new Vector3(position.x + Random.Range(-randomOffset.x, randomOffset.x), position.y + Random.Range(-randomOffset.y, randomOffset.y), position.z + Random.Range(-randomOffset.z, randomOffset.z)), Quaternion.identity);
        }
    }
}
