using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    enum KeybindChange
    {
        PauseAbility,
        Inventory,
        Skill1,
        Skill2,
        Skill3,
        Skill4
    }

    public Slider musicVolumeSlider = null;
    public Slider sfxVolumeSlider = null;
    public Slider cameraMoveSensSlider = null;

    // Text elements for displaying keybinds
    public Text kbInventoryToggle = null;
    public Text kbPauseAbility = null;
    public Text kbWeaponAttack = null;
    public Text kbBlast = null;
    public Text kbTeleport = null;
    public Text kbRewind = null;

    SavedSettings optionsSettings;

    bool waitingForInput = false;

    KeybindChange keybindChange;
    KeyCode newKey = KeyCode.None;

    [SerializeField]
    Sprite applyHoverSprite = null;
    [SerializeField]
    Sprite applySprite = null;
    [SerializeField]
    Sprite mainMenuHoverSprite = null;
    [SerializeField]
    Sprite mainMenuSprite = null;
    [SerializeField]
    Sprite resetHoverSprite = null;
    [SerializeField]
    Sprite resetSprite = null;

    [SerializeField]
    Image mainMenuImage = null;
    [SerializeField]
    Image applyImage = null;
    [SerializeField]
    Image resetImage = null;

    private void Awake()
    {
        // Initialise min/max values for sliders
        musicVolumeSlider.maxValue = 1.0f;
        musicVolumeSlider.minValue = 0.0f;

        sfxVolumeSlider.maxValue = 1.0f;
        sfxVolumeSlider.minValue = 0.0f;

        cameraMoveSensSlider.maxValue = 10.0f;
        cameraMoveSensSlider.minValue = 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        waitingForInput = false;

        optionsSettings = new SavedSettings();
        optionsSettings.keybindings = SaveManager.GetSettings().keybindings;
        optionsSettings.cameraMoveSensitivity = SaveManager.GetSettings().cameraMoveSensitivity;
        optionsSettings.musicVolume = SaveManager.GetSettings().musicVolume;
        optionsSettings.sfxVolume = SaveManager.GetSettings().sfxVolume;

        kbPauseAbility.text = optionsSettings.keybindings.pauseAbility.ToString();
        kbInventoryToggle.text = optionsSettings.keybindings.toggleInventory.ToString();
        kbWeaponAttack.text = optionsSettings.keybindings.weaponAttack.ToString();
        kbBlast.text = optionsSettings.keybindings.skillSlot2.ToString();
        kbTeleport.text = optionsSettings.keybindings.skillSlot3.ToString();
        kbRewind.text = optionsSettings.keybindings.skillSlot4.ToString();

        // Set slider values to values stored in settings
        musicVolumeSlider.value = SaveManager.GetSettings().musicVolume;
        sfxVolumeSlider.value = SaveManager.GetSettings().sfxVolume;
        cameraMoveSensSlider.value = SaveManager.GetSettings().cameraMoveSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        optionsSettings.cameraMoveSensitivity = cameraMoveSensSlider.value;
        optionsSettings.musicVolume = musicVolumeSlider.value;
        optionsSettings.sfxVolume = sfxVolumeSlider.value;

        UpdateKeybindText(ref kbPauseAbility, optionsSettings.keybindings.pauseAbility);
        UpdateKeybindText(ref kbInventoryToggle, optionsSettings.keybindings.toggleInventory);
        UpdateKeybindText(ref kbWeaponAttack, optionsSettings.keybindings.weaponAttack);
        UpdateKeybindText(ref kbBlast, optionsSettings.keybindings.skillSlot2);
        UpdateKeybindText(ref kbTeleport, optionsSettings.keybindings.skillSlot3);
        UpdateKeybindText(ref kbRewind, optionsSettings.keybindings.skillSlot4);

        if (waitingForInput)
        {
            switch (keybindChange)
            {
                case KeybindChange.PauseAbility:
                    {
                        ChangeKeybind(ref optionsSettings.keybindings.pauseAbility);
                        kbPauseAbility.text = "- - -";

                        if (newKey != KeyCode.None)
                        {
                            if (optionsSettings.keybindings.toggleInventory == optionsSettings.keybindings.pauseAbility)
                            {
                                optionsSettings.keybindings.toggleInventory = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.weaponAttack == optionsSettings.keybindings.pauseAbility)
                            {
                                optionsSettings.keybindings.weaponAttack = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot2 == optionsSettings.keybindings.pauseAbility)
                            {
                                optionsSettings.keybindings.skillSlot2 = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot3 == optionsSettings.keybindings.pauseAbility)
                            {
                                optionsSettings.keybindings.skillSlot3 = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot4 == optionsSettings.keybindings.pauseAbility)
                            {
                                optionsSettings.keybindings.skillSlot4 = KeyCode.None;
                            }
                        }
                        

                        break;
                    }
                case KeybindChange.Inventory:
                    {
                        ChangeKeybind(ref optionsSettings.keybindings.toggleInventory);
                        kbInventoryToggle.text = "- - -";

                        if (newKey != KeyCode.None)
                        {
                            if (optionsSettings.keybindings.pauseAbility == optionsSettings.keybindings.toggleInventory)
                            {
                                optionsSettings.keybindings.pauseAbility = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.weaponAttack == optionsSettings.keybindings.toggleInventory)
                            {
                                optionsSettings.keybindings.weaponAttack = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot2 == optionsSettings.keybindings.toggleInventory)
                            {
                                optionsSettings.keybindings.skillSlot2 = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot3 == optionsSettings.keybindings.toggleInventory)
                            {
                                optionsSettings.keybindings.skillSlot3 = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot4 == optionsSettings.keybindings.toggleInventory)
                            {
                                optionsSettings.keybindings.skillSlot4 = KeyCode.None;
                            }
                        }
                        break;
                    }
                case KeybindChange.Skill1:
                    {
                        ChangeKeybind(ref optionsSettings.keybindings.weaponAttack);
                        kbWeaponAttack.text = "- - -";

                        if (newKey != KeyCode.None)
                        {
                            if (optionsSettings.keybindings.pauseAbility == optionsSettings.keybindings.weaponAttack)
                            {
                                optionsSettings.keybindings.pauseAbility = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.toggleInventory == optionsSettings.keybindings.weaponAttack)
                            {
                                optionsSettings.keybindings.toggleInventory = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot2 == optionsSettings.keybindings.weaponAttack)
                            {
                                optionsSettings.keybindings.skillSlot2 = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot3 == optionsSettings.keybindings.weaponAttack)
                            {
                                optionsSettings.keybindings.skillSlot3 = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot4 == optionsSettings.keybindings.weaponAttack)
                            {
                                optionsSettings.keybindings.skillSlot4 = KeyCode.None;
                            }
                        }
                        break;
                    }
                case KeybindChange.Skill2:
                    {
                        ChangeKeybind(ref optionsSettings.keybindings.skillSlot2);
                        kbBlast.text = "- - -";

                        if (newKey != KeyCode.None)
                        {
                            if (optionsSettings.keybindings.pauseAbility == optionsSettings.keybindings.skillSlot2)
                            {
                                optionsSettings.keybindings.pauseAbility = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.toggleInventory == optionsSettings.keybindings.skillSlot2)
                            {
                                optionsSettings.keybindings.toggleInventory = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.weaponAttack == optionsSettings.keybindings.skillSlot2)
                            {
                                optionsSettings.keybindings.weaponAttack = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot3 == optionsSettings.keybindings.skillSlot2)
                            {
                                optionsSettings.keybindings.skillSlot3 = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot4 == optionsSettings.keybindings.skillSlot2)
                            {
                                optionsSettings.keybindings.skillSlot4 = KeyCode.None;
                            }
                        }
                        break;
                    }
                case KeybindChange.Skill3:
                    {
                        ChangeKeybind(ref optionsSettings.keybindings.skillSlot3);
                        kbTeleport.text = "- - -";

                        if (newKey != KeyCode.None)
                        {
                            if (optionsSettings.keybindings.pauseAbility == optionsSettings.keybindings.skillSlot3)
                            {
                                optionsSettings.keybindings.pauseAbility = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.toggleInventory == optionsSettings.keybindings.skillSlot3)
                            {
                                optionsSettings.keybindings.toggleInventory = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.weaponAttack == optionsSettings.keybindings.skillSlot3)
                            {
                                optionsSettings.keybindings.weaponAttack = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot2 == optionsSettings.keybindings.skillSlot3)
                            {
                                optionsSettings.keybindings.skillSlot2 = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot4 == optionsSettings.keybindings.skillSlot3)
                            {
                                optionsSettings.keybindings.skillSlot4 = KeyCode.None;
                            }
                        }
                        break;
                    }
                case KeybindChange.Skill4:
                    {
                        ChangeKeybind(ref optionsSettings.keybindings.skillSlot4);
                        kbRewind.text = "- - -";

                        if (newKey != KeyCode.None)
                        {
                            if (optionsSettings.keybindings.pauseAbility == optionsSettings.keybindings.skillSlot4)
                            {
                                optionsSettings.keybindings.pauseAbility = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.toggleInventory == optionsSettings.keybindings.skillSlot4)
                            {
                                optionsSettings.keybindings.toggleInventory = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.weaponAttack == optionsSettings.keybindings.skillSlot4)
                            {
                                optionsSettings.keybindings.weaponAttack = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot2 == optionsSettings.keybindings.skillSlot4)
                            {
                                optionsSettings.keybindings.skillSlot2 = KeyCode.None;
                            }
                            if (optionsSettings.keybindings.skillSlot3 == optionsSettings.keybindings.skillSlot4)
                            {
                                optionsSettings.keybindings.skillSlot3 = KeyCode.None;
                            }
                        }
                        break;
                    }
            }
        }
    }

    public void ApplySettings()
    {
        SaveManager.GetSettings().cameraMoveSensitivity = optionsSettings.cameraMoveSensitivity;
        SaveManager.GetSettings().keybindings = optionsSettings.keybindings;
        SaveManager.GetSettings().musicVolume = optionsSettings.musicVolume;
        SaveManager.GetSettings().sfxVolume = optionsSettings.sfxVolume;

        SaveManager.SaveSettings();
    }

    public void RestoreDefaultSettings()
    {
        SaveManager.GetSettings().cameraMoveSensitivity = SaveManager.GetDefaultSettings().cameraMoveSensitivity;
        SaveManager.GetSettings().keybindings = SaveManager.GetDefaultSettings().keybindings;
        SaveManager.GetSettings().musicVolume = SaveManager.GetDefaultSettings().musicVolume;
        SaveManager.GetSettings().sfxVolume = SaveManager.GetDefaultSettings().sfxVolume;

        SaveManager.SaveSettings();

        optionsSettings.cameraMoveSensitivity = SaveManager.GetSettings().cameraMoveSensitivity;
        optionsSettings.keybindings = SaveManager.GetSettings().keybindings;
        optionsSettings.musicVolume = SaveManager.GetSettings().musicVolume;
        optionsSettings.sfxVolume = SaveManager.GetSettings().sfxVolume;

        kbPauseAbility.text = optionsSettings.keybindings.pauseAbility.ToString();
        kbInventoryToggle.text = optionsSettings.keybindings.toggleInventory.ToString();
        kbWeaponAttack.text = optionsSettings.keybindings.weaponAttack.ToString();
        kbBlast.text = optionsSettings.keybindings.skillSlot2.ToString();
        kbTeleport.text = optionsSettings.keybindings.skillSlot3.ToString();
        kbRewind.text = optionsSettings.keybindings.skillSlot4.ToString();

        musicVolumeSlider.value = optionsSettings.musicVolume;
        sfxVolumeSlider.value = optionsSettings.sfxVolume;
        cameraMoveSensSlider.value = optionsSettings.cameraMoveSensitivity;
    }

    private void OnEnable()
    {
        waitingForInput = false;

        optionsSettings = new SavedSettings();
        optionsSettings.keybindings = SaveManager.GetSettings().keybindings;
        optionsSettings.cameraMoveSensitivity = SaveManager.GetSettings().cameraMoveSensitivity;
        optionsSettings.musicVolume = SaveManager.GetSettings().musicVolume;
        optionsSettings.sfxVolume = SaveManager.GetSettings().sfxVolume;

        // Set slider values to values stored in settings
        musicVolumeSlider.value = optionsSettings.musicVolume;
        sfxVolumeSlider.value = optionsSettings.sfxVolume;
        cameraMoveSensSlider.value = optionsSettings.cameraMoveSensitivity;
    }

    public void ChangePauseAbilityKeybind()
    {
        waitingForInput = true;
        keybindChange = KeybindChange.PauseAbility;
    }

    public void ChangeInventoryKeybind()
    {
        waitingForInput = true;
        keybindChange = KeybindChange.Inventory;
    }

    public void ChangeSkill1Keybind()
    {
        waitingForInput = true;
        keybindChange = KeybindChange.Skill1;
    }

    public void ChangeSkill2Keybind()
    {
        waitingForInput = true;
        keybindChange = KeybindChange.Skill2;
    }

    public void ChangeSkill3Keybind()
    {
        waitingForInput = true;
        keybindChange = KeybindChange.Skill3;
    }

    public void ChangeSkill4Keybind()
    {
        waitingForInput = true;
        keybindChange = KeybindChange.Skill4;
    }

    void ChangeKeybind(ref KeyCode key)
    {
        if (newKey != KeyCode.None)
        {
            key = newKey;
            waitingForInput = false;
        }
    }

    void UpdateKeybindText(ref Text text, KeyCode key)
    {
        
        if (key == KeyCode.None)
        {
            text.text = "UNBOUND!";
        }
        else if (key == KeyCode.Alpha0 || key == KeyCode.Alpha1 || key == KeyCode.Alpha2 || key == KeyCode.Alpha3 || key == KeyCode.Alpha4 || key == KeyCode.Alpha5 || key == KeyCode.Alpha6 || key == KeyCode.Alpha7 || key == KeyCode.Alpha8 || key == KeyCode.Alpha9)
        {
            switch (key)
            {
                case KeyCode.Alpha0:
                    {
                        text.text = "0";
                        break;
                    }
                case KeyCode.Alpha1:
                    {
                        text.text = "1";
                        break;
                    }
                case KeyCode.Alpha2:
                    {
                        text.text = "2";
                        break;
                    }
                case KeyCode.Alpha3:
                    {
                        text.text = "3";
                        break;
                    }
                case KeyCode.Alpha4:
                    {
                        text.text = "4";
                        break;
                    }
                case KeyCode.Alpha5:
                    {
                        text.text = "5";
                        break;
                    }
                case KeyCode.Alpha6:
                    {
                        text.text = "6";
                        break;
                    }
                case KeyCode.Alpha7:
                    {
                        text.text = "7";
                        break;
                    }
                case KeyCode.Alpha8:
                    {
                        text.text = "8";
                        break;
                    }
                case KeyCode.Alpha9:
                    {
                        text.text = "9";
                        break;
                    }
            }
        }
        else
        {
            text.text = key.ToString();
        }
    }

    private void OnGUI()
    {
        if (waitingForInput)
        {
            if (Event.current.isKey)
            {
               // if (Input.GetKeyDown(Event.current.keyCode))
                //{
                    //Debug.Log("Key Pressed: " + Event.current.keyCode.ToString());
                    newKey = Event.current.keyCode;
                //}
            }
        }
        else
        {
            newKey = KeyCode.None;
        }
    }

    public void OnMainMenuEnter()
    {
        mainMenuImage.sprite = mainMenuHoverSprite;
    }

    public void OnMainMenuExit()
    {
        mainMenuImage.sprite = mainMenuSprite;
    }

    public void OnResetEnter()
    {
        resetImage.sprite = resetHoverSprite;
    }

    public void OnResetExit()
    {
        resetImage.sprite = resetSprite;
    }

    public void OnApplyEnter()
    {
        applyImage.sprite = applyHoverSprite;
    }

    public void OnApplyExit()
    {
        applyImage.sprite = applySprite;
    }
}
