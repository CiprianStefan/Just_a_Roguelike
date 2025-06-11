using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public enum GameState
{
    PauseMenu,
    UpgradeSelection,
    GameOver,
    LevelConquered,
}
public enum PlayerSettingsType
{
    SFxVolume,
    MusicVolume,
    SFxEnabled,
    MusicEnabled,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Player;
    public PlayerCombatEntity playerCombatEntity;
    public Canvas MainUICanvas;
    public GameObject DisplayDamageParent;
    public bool playerDeath;
    public GameObject deathWindowPrefab;
    public float gameTime = 0;
    public float GameTimeInMinutes { get { return gameTime / 60; } }
    public PlayerSettings playerSettings;
    public GameObject pauseMenuPrefab;
    public GameObject pauseMenu;
    public bool playerCanUseKeyboard = true;
    private bool autoTarget = true;
    public Vector3 TargetPosition { get { return autoTarget ? PlayerToClosestEnemyCalculations.Instance.ClosestEnemyPosition : 
        new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0); } }
    public float ExperienceRate { get { return 1 + gameTime / 60 / 5; } }
    public SoulContainer baseSoulContainer;
    public GameState gameState;
    public GameObject RegenHealthPickupPrefab;
    public GameObject statisticDisplayResolutionWindowPrefab;

    public static EventHandler OnPlayerSettingsUpdate;


    protected void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        PlayerCombatEntity.OnPlayerDeath += OnPlayerDeath;
        EnemyGenerationManager.OnLevelCompleted += OnLevelCompleted;
        playerDeath = false;
        playerCanUseKeyboard = true;
        Time.timeScale = 1;
        Application.targetFrameRate = 60;
    }

    protected void Start()
    {
        playerCombatEntity = Player.GetComponent<PlayerCombatEntity>();
    }

    protected void Update()
    {
        gameTime += Time.deltaTime;
        ManagePlayerInput();
    }

    public void ChangeGamePauseState(GameState _gameState)
    {
        gameState = _gameState;
        switch(_gameState)
        {
            case GameState.PauseMenu:
                Time.timeScale = Mathf.Abs(Time.timeScale - 1);
                break;
            case GameState.UpgradeSelection:
            case GameState.GameOver:
            case GameState.LevelConquered:
                Time.timeScale = Mathf.Abs(Time.timeScale - 1);
                playerCanUseKeyboard = !playerCanUseKeyboard;
                break;
        }
        
    }

    void ManagePlayerInput()
    {
        if(playerCanUseKeyboard)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ChangeGamePauseState(GameState.PauseMenu);
                if(pauseMenu == null)
                    pauseMenu = Instantiate(pauseMenuPrefab, MainUICanvas.transform);
                else
                    Destroy(pauseMenu);
            }
            if(Input.GetKeyDown(KeyCode.Tab))
                autoTarget = !autoTarget;
        }
    }

    void OnPlayerDeath(object sender, EventArgs e)
    {
        if(playerDeath)
            return;
        playerDeath = true;
        ChangeGamePauseState(GameState.GameOver);
        CreateDeathWindow();
    }

    void CreateDeathWindow()
    {
        GameObject deathWindow = Instantiate(deathWindowPrefab, MainUICanvas.transform);
        string resolutionMessage = "";
        Color tintColor = Color.black;
        switch (gameState)
        {
            case GameState.GameOver:
                resolutionMessage = "You died";
                tintColor = new Color(1,0,0,0.2f);
                break;
            case GameState.LevelConquered:
                resolutionMessage = "Level Conquered";
                tintColor = new Color(1,1,1,0.2f);
                break;
        }
        deathWindow.transform.GetChild(0).GetComponent<Image>().color = tintColor;
        deathWindow.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = resolutionMessage;
        GameObject abilitiesDisplayParent = deathWindow.transform.GetChild(4).transform.GetChild(0).gameObject;
        List<StatisticsAbilityDamage> statisticsAbilitiesDamage = StatisticTracker.Instance.abilitiesDamage;
        statisticsAbilitiesDamage.OrderByDescending(x => x.Damage);
        foreach (var ability in statisticsAbilitiesDamage)
        {
            CreateStatisticEntryUI(abilitiesDisplayParent, ability.Name, ability.Damage.ToString("###,###,###,###"), Color.white);
        }
        double totalPhysicalDamage = statisticsAbilitiesDamage.Where(x => x.DamageType == SkillDamageType.PhysicalDamage).Sum(x => x.Damage);
        double totalMagicalDamage = statisticsAbilitiesDamage.Where(x => x.DamageType == SkillDamageType.MagicalDamage).Sum(x => x.Damage);
        double totalElementalDamage = statisticsAbilitiesDamage.Where(x => x.DamageType == SkillDamageType.ElementalDamage).Sum(x => x.Damage);
        double totalDamageDealt = statisticsAbilitiesDamage.Sum(x => x.Damage);
        if(totalPhysicalDamage > 0)CreateStatisticEntryUI(abilitiesDisplayParent, "Total Physical Damage", totalPhysicalDamage.ToString("###,###,###,###"), new Color(0.2f,0.2f,0.2f), 35);
        if(totalMagicalDamage > 0)CreateStatisticEntryUI(abilitiesDisplayParent, "Total Magical Damage", totalMagicalDamage.ToString("###,###,###,###"), new Color(0.2f,0.2f,0.2f), 35);
        if(totalElementalDamage > 0)CreateStatisticEntryUI(abilitiesDisplayParent, "Total Elemental Damage", totalElementalDamage.ToString("###,###,###,###"), new Color(0.2f,0.2f,0.2f), 35);
        CreateStatisticEntryUI(abilitiesDisplayParent, "Total Damage", totalDamageDealt.ToString("###,###,###,###"), new Color(0.2f,0.2f,0.2f), 40);
        GameObject enemiesDisplayParent = deathWindow.transform.GetChild(5).transform.GetChild(0).gameObject;
        List<StatisticsEnemiesKilled> statisticsEnemiesDamage = StatisticTracker.Instance.enemiesKilled;
        statisticsEnemiesDamage.OrderByDescending(x => x.Amount);
        foreach (var enemy in statisticsEnemiesDamage)
        {
            CreateStatisticEntryUI(enemiesDisplayParent, enemy.Name, enemy.Amount.ToString("###,###,###,###"), Color.white);
        }
        int totalEnemiesKilled = statisticsEnemiesDamage.Sum(x => x.Amount);
        CreateStatisticEntryUI(enemiesDisplayParent, "Total Enemies Killed", totalEnemiesKilled.ToString("###,###,###,###"), new Color(0.2f,0.2f,0.2f), 40);
        deathWindow.transform.GetChild(6).GetComponent<Button>().onClick.AddListener( delegate { SceneManager.LoadScene("MainMenu");} );
    }

    void CreateStatisticEntryUI(GameObject displayParent, string description, string amount, Color borderColor, int size = 30)
    {
        GameObject enemyDisplay = Instantiate(statisticDisplayResolutionWindowPrefab);
        enemyDisplay.transform.SetParent(displayParent.transform);
        enemyDisplay.transform.localScale = new Vector3(1,1,1);
        Image image = enemyDisplay.transform.GetComponent<Image>();
        image.color = borderColor;
        TextMeshProUGUI text = enemyDisplay.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        text.fontSizeMax = size;
        text.text = description + ": " + amount;
    }

    void OnLevelCompleted(object sender, EventArgs e)
    {
        ChangeGamePauseState(GameState.LevelConquered);
        CreateDeathWindow();
    }

    public void UpdatePlayerSettings(PlayerSettingsType setting, float value)
    {
        switch(setting)
        {
            case PlayerSettingsType.SFxVolume:
                playerSettings.SFxVolume = value;
                break;
            case PlayerSettingsType.MusicVolume:
                playerSettings.MusicVolume = value;
                break;
        }
        OnPlayerSettingsUpdate?.Invoke(this, EventArgs.Empty);
    }

    public void UpdatePlayerSettings(PlayerSettingsType setting, bool value)
    {
        switch(setting)
        {
            case PlayerSettingsType.SFxEnabled:
                playerSettings.SFxEnabled = value;
                break;
            case PlayerSettingsType.MusicEnabled:
                playerSettings.MusicEnabled = value;
                break;
        }
        OnPlayerSettingsUpdate?.Invoke(this, EventArgs.Empty);
    }

    protected void OnDestroy()
    {
        PlayerCombatEntity.OnPlayerDeath -= OnPlayerDeath;
        EnemyGenerationManager.OnLevelCompleted -= OnLevelCompleted;
    }
}
