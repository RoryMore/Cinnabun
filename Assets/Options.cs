using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    Slider musicVolumeSlider = null;
    Slider sfxVolumeSlider = null;
    Slider cameraMoveSensSlider = null;

    Text musicVolumeText = null;
    Text sfxVolumeText = null;
    Text cameraMoveSensText = null;

    // Text elements for displaying keybinds
    Text kbInventoryToggle = null;
    Text kbPauseAbility = null;
    Text kbWeaponAttack = null;
    Text kbSkill2 = null;
    Text kbSkill3 = null;
    Text kbSkill4 = null;

    // May not need to exist in this script, they will be buttons in the options menu though
    // Buttons will call functions: ApplySettings(), and RestoreDefaultSettings()
    Button applySettingsButton = null;
    Button restoreDefaultsButton = null;

    SavedSettings optionsSettings;

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
        optionsSettings = SaveManager.GetSettings();

        // Set slider values to values stored in settings
        musicVolumeSlider.value = SaveManager.GetSettings().musicVolume;
        sfxVolumeSlider.value = SaveManager.GetSettings().sfxVolume;
        cameraMoveSensSlider.value = SaveManager.GetSettings().cameraMoveSensitivity;

        // TODO: Move this code into it's own function for a button to interact with
        // Untested idea code for dealing with keybinds
        //string fakeKBText;
        //switch (optionsSettings.keybindings.toggleInventory)
        //{
        //    case KeyCode.Space:
        //        {
        //            fakeKBText = "SPACE";
        //            break;
        //        }
        //    default:
        //        {
        //            fakeKBText = optionsSettings.keybindings.toggleInventory.ToString();
        //            break;
        //        }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        optionsSettings.cameraMoveSensitivity = cameraMoveSensSlider.value;
        optionsSettings.musicVolume = musicVolumeSlider.value;
        optionsSettings.sfxVolume = sfxVolumeSlider.value;
        //optionsSettings.keybindings = 
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
    }

    private void OnEnable()
    {
        optionsSettings = SaveManager.GetSettings();

        // Set slider values to values stored in settings
        musicVolumeSlider.value = SaveManager.GetSettings().musicVolume;
        sfxVolumeSlider.value = SaveManager.GetSettings().sfxVolume;
        cameraMoveSensSlider.value = SaveManager.GetSettings().cameraMoveSensitivity;
    }
}
