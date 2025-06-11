using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button startGameButton;
    [SerializeField]
    private Button achievementsButton;
    [SerializeField]
    private Button optionsButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private GameObject settingsMenuPrefab;
    [SerializeField]
    private Canvas menuCanvas;
    [SerializeField]
    private PlayerSettings playerSettings;
    [SerializeField]
    private GameObject soulSelectorPanel;
    [SerializeField]
    private GameObject mainMenu;

    protected void Start()
    {
        startGameButton.onClick.AddListener(delegate{StartGame();});
        achievementsButton.onClick.AddListener(delegate{Achievements();});
        optionsButton.onClick.AddListener(delegate{Options();});
        exitButton.onClick.AddListener(delegate{Exit();});
        SettingsMainMenuUI.OnUpdatePlayerSettings += OnUpdatePlayerSettings;
        UpdateSound();
    }

    private void StartGame()
    {
        soulSelectorPanel.SetActive(true);
        mainMenu.SetActive(false);
    }

    private void Achievements()
    {
        print("Achievements");
    }

    private void Options()
    {
        Instantiate(settingsMenuPrefab, menuCanvas.transform);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void OnUpdatePlayerSettings(object sender, EventArgs e)
    {
        UpdateSound();
    }

    private void UpdateSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.mute = !playerSettings.MusicEnabled;
        audioSource.volume = playerSettings.MusicVolume;
    }

    protected void OnDestroy()
    {
        SettingsMainMenuUI.OnUpdatePlayerSettings -= OnUpdatePlayerSettings;
    }
}
