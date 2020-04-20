using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEffects
{
    // The SetEffect class will hold all relevant data & methods for SetEffects.
    // The Inventory will have a property of SetEffects.
    // Items will have a property type of the SetEffect enum.

    public enum SetEffect
    {
        NONE
    }

    private bool isActive;

    public bool IsActive { get => isActive; }
}
