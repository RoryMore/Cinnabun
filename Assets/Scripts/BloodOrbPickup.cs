using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BloodOrbPickup : MonoBehaviour
{
    AttractedPickup attractor;

    public bool targetIsPlayer;
    Entity entityToHeal;

    [SerializeField]
    bool isPercentageHealed;
    [SerializeField]
    [Tooltip("If isPercentageHealed is TRUE; healAmount is a percentage of that entities maxHP. Else, healAmount is a flat heal value")]
    int healAmount;

    [Header("Dropped Height Above Ground")]
    [SerializeField]
    float droppedHeight;

    private void Awake()
    {
        attractor = GetComponent<AttractedPickup>();
        if (targetIsPlayer)
        {
            entityToHeal = FindObjectOfType<Player>();
            attractor.SetTarget(entityToHeal.transform);
        }

        // Make sure we are dropped on the NavMesh
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 20.0f, NavMesh.AllAreas))
        {
            transform.position = new Vector3(hit.position.x, hit.position.y + droppedHeight, hit.position.z);
        }
        else
        {
            Debug.LogError("BloodOrb drop failed to find a suitable position on the NavMesh");
            Destroy(gameObject);
        }
    }

    public void SetEntityToHeal(Entity entity)
    {
        entityToHeal = entity;
        attractor.SetTarget(entityToHeal.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attractor.targetInRange)
        {
            if (isPercentageHealed)
            {
                int percentageHealed = Mathf.RoundToInt(entityToHeal.maxHP * (healAmount / 100.0f));
                entityToHeal.currentHP += percentageHealed;
            }
            else
            {
                entityToHeal.currentHP += healAmount;
            }
            entityToHeal.currentHP = Mathf.Clamp(entityToHeal.currentHP, 0, entityToHeal.maxHP);
            Destroy(gameObject);
        }
    }
}
