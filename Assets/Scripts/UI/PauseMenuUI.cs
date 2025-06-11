using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField]
    private Slider SFxVolume;
    [SerializeField]
    private Slider MusicVolume;
    [SerializeField]
    private Toggle SFxEnabled;
    [SerializeField]
    private Toggle MusicEnabled;
    [SerializeField]
    private Button ExitButton;
    
    protected void Start()
    {
        SFxVolume.value = AudioManager.Instance.SFxVolume;
        MusicVolume.value = AudioManager.Instance.MusicVolume;
        SFxEnabled.isOn = AudioManager.Instance.SFxEnabled;
        MusicEnabled.isOn = AudioManager.Instance.MusicEnabled;
        SFxVolume.onValueChanged.AddListener(delegate {GameManager.Instance.UpdatePlayerSettings(PlayerSettingsType.SFxVolume,SFxVolume.value);} );
        MusicVolume.onValueChanged.AddListener(delegate {GameManager.Instance.UpdatePlayerSettings(PlayerSettingsType.MusicVolume,MusicVolume.value);} );
        SFxEnabled.onValueChanged.AddListener(delegate {GameManager.Instance.UpdatePlayerSettings(PlayerSettingsType.SFxEnabled,SFxEnabled.isOn);} );
        MusicEnabled.onValueChanged.AddListener(delegate {GameManager.Instance.UpdatePlayerSettings(PlayerSettingsType.MusicEnabled,MusicEnabled.isOn);} );
        ExitButton.onClick.AddListener( delegate { SceneManager.LoadScene("MainMenu"); } );
    }

}
