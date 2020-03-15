using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedSettings
{
    [System.Serializable]
    public struct Keybindings
    {
        //public KeyCode gamePause;
        public KeyCode toggleInventory;
        public KeyCode pauseAbility;
        public KeyCode weaponAttack;
        public KeyCode skillSlot2;
        public KeyCode skillSlot3;
        public KeyCode skillSlot4;
    }

    public float musicVolume;
    public float sfxVolume;

    public float cameraMoveSensitivity;
    public Keybindings keybindings;
}
