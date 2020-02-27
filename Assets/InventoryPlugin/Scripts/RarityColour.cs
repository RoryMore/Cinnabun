using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RarityColourCode", order = 1)]
public class RarityColour : ScriptableObject
{
    public Color commonColour;
    public Color uncommonColour;
    public Color rareColour;
    public Color ultraColour;
}
