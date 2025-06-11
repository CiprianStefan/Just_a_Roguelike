using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMainMenuUI : MonoBehaviour
{
    [SerializeField]
    private Slider SFxVolume;
    [SerializeField]
    private Slider MusicVolume;
    [SerializeField]
    private Toggle SFxEnable;
    [SerializeField]
    private Toggle MusicEnable;
    [SerializeField]
    private PlayerSettings playerSettings;
    [SerializeField]
    private Button exitButton;
    
    public static EventHandler OnUpdatePlayerSettings;

    protected void Start()
    {
        InitializePlayerSettings();
        SFxVolume.onValueChanged.AddListener( delegate { UpdatePlayerSettings(PlayerSettingsType.SFxVolume, SFxVolume.value); } );
        MusicVolume.onValueChanged.AddListener( delegate { UpdatePlayerSettings(PlayerSettingsType.MusicVolume, MusicVolume.value); } );
        SFxEnable.onValueChanged.AddListener( delegate { UpdatePlayerSettings(PlayerSettingsType.SFxEnabled, SFxEnable.isOn); } );
        MusicEnable.onValueChanged.AddListener( delegate { UpdatePlayerSettings(PlayerSettingsType.MusicEnabled, MusicEnable.isOn); } ); 
        exitButton.onClick.AddListener( delegate { Destroy(gameObject); } );
    }

    private void UpdatePlayerSettings(PlayerSettingsType playerSettingsType, float value)
    {
        switch(playerSettingsType)
        {
            case PlayerSettingsType.SFxVolume:
                playerSettings.SFxVolume = value;
                break;
            case PlayerSettingsType.MusicVolume:
                playerSettings.MusicVolume = value;
                break;
        }
        OnUpdatePlayerSettings?.Invoke(this, EventArgs.Empty);
    }

    private void UpdatePlayerSettings(PlayerSettingsType playerSettingsType, bool value)
    {
        switch(playerSettingsType)
        {
            case PlayerSettingsType.SFxEnabled:
                playerSettings.SFxEnabled = value;
                break;
            case PlayerSettingsType.MusicEnabled:
                playerSettings.MusicEnabled = value;
                break;
        }
        OnUpdatePlayerSettings?.Invoke(this, EventArgs.Empty);
    }

    private void InitializePlayerSettings()
    {
        SFxVolume.value = playerSettings.SFxVolume;
        MusicVolume.value = playerSettings.MusicVolume;
        SFxEnable.isOn = playerSettings.SFxEnabled;
        MusicEnable.isOn = playerSettings.MusicEnabled;
    }

}
