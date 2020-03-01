using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UpgradeMoneyData", order = 1)]
public class UpgradeMoney : ScriptableObject
{
    [SerializeField]
    int amount;

    public int GetAmount()
    {
        return amount;
    }

    public void AddMoney(int value)
    {
        amount += value;
        EditorUtility.SetDirty(this);
    }

    public void DeductMoney(int value)
    {
        amount -= value;
        EditorUtility.SetDirty(this);
    }
}
