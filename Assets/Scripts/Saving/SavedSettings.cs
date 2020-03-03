using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedSettings
{
    [System.Serializable]
    public struct Keybindings
    {
        public KeyCode gamePause;
        public KeyCode toggleInventory;
        public KeyCode pauseAbility;
        public KeyCode weaponAttack;
        public KeyCode blast;
        public KeyCode teleport;
        public KeyCode rewind;
    }

    public float musicVolume;
    public float sfxVolume;

    public float cameraMoveSensitivity;
    public Keybindings keybindings;
}
