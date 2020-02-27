using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        attractor = GetComponent<AttractedPickup>();
        if (targetIsPlayer)
        {
            entityToHeal = FindObjectOfType<Player>();
            attractor.SetTarget(entityToHeal.transform);
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
