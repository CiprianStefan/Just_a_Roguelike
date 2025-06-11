using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerUpgradesManager : MonoBehaviour
{
    public static PlayerUpgradesManager Instance;

    [SerializeField]
    private LevelUpEmpowermentDatabase levelUpEmpowermentDatabase;

    private List<LevelUpEmpowerment> availableUpgrades;
    private List<LevelUpEmpowerment> currentPlayerUpgrades;
    private LevelUpEmpowerment[] currentUpgradeValues;
    private int upgradesAquiredAndNotChoosed = 0;
    private bool playerChoosingUpgarde = false;
    [SerializeField]
    private GameObject upgradePanel;

    public List<LevelUpEmpowerment> AvailableUpgrades { get => availableUpgrades; }
    public List<LevelUpEmpowerment> CurrentPlayerUpgrades { get => currentPlayerUpgrades; }
    public LevelUpEmpowerment[] CurrentUpgradeValues { get => currentUpgradeValues; }

    protected void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        PlayerLevelManager.OnLevelChanged += AquireUpgrade;
        PlayerUpgradeUI.OnUpgradeSelected += ApplyUpgrade;
    }

    protected void Start()
    {
        availableUpgrades = new List<LevelUpEmpowerment>(levelUpEmpowermentDatabase.levelUpEmpowerments);
        currentPlayerUpgrades = new List<LevelUpEmpowerment>();
        LevelUpEmpowerment startAbility = availableUpgrades.Find(x => ((AbilityEmpowerment)x).Name == PlayerMisc.Instance.soulContainer.StartAbilityName);
        currentPlayerUpgrades.Add(startAbility);
        AbilityEmpowerment abilityEmpowerment = (AbilityEmpowerment)startAbility;
        AbilitiesManager.Instance.AddAbility(abilityEmpowerment.abilityHolder);
        availableUpgrades.Remove(abilityEmpowerment);
        SkillHoldersUI.Instance.UpdateSkillHolder(null, new Tuple<int, SkillContainer>(0, abilityEmpowerment.abilityHolder.GetComponent<AbilityHolder>().skillContainer));
    }

    private void Update()
    {
        if(upgradesAquiredAndNotChoosed > 0 && !playerChoosingUpgarde)
            GenerateLevelUpgrades();
    }

    private void AquireUpgrade(object sender, EventArgs e)
    {
        upgradesAquiredAndNotChoosed++;
    }

    public void GenerateRandomUpgrades()
    {
        EmpowermentType empowermentType = AbilitiesManager.Instance.AbilitiesLimitReached && AbilitiesManager.Instance.EvolutionsLimitReached ? EmpowermentType.Stat : Random.Range(0, 100) > 70 ? EmpowermentType.Ability : EmpowermentType.Stat;
        currentUpgradeValues = new LevelUpEmpowerment[3];
        List<string> abilitiesNames = AbilitiesManager.Instance.OwnedAbilitiesNames;
        List<LevelUpEmpowerment> possibleUpgrades = availableUpgrades.FindAll(x => x.EmpowermentType == empowermentType || 
            (empowermentType == EmpowermentType.Ability && x.EmpowermentType == EmpowermentType.Evolution && abilitiesNames.Contains(((EvolutionEmpowerment)x).AbilityReplaceName)));
        int possibleUpgradesCount = possibleUpgrades.Count;
        int numberOfUpgrades = possibleUpgradesCount < 3 ? possibleUpgradesCount : 3;
        int randomIndex;
        for (int i = 0; i < numberOfUpgrades; i++)
        {
            while(true)
            {
                randomIndex = Random.Range(0, possibleUpgradesCount);
                if(!currentUpgradeValues.Contains(possibleUpgrades[randomIndex]))
                    break;
                if(possibleUpgrades[randomIndex].EmpowermentType == EmpowermentType.Evolution && Random.Range(0, 100) < 70)
                    break;
            }
            currentUpgradeValues[i] = possibleUpgrades[randomIndex];
        }
    }

    private void GenerateLevelUpgrades()
    {
        playerChoosingUpgarde = true;
        upgradesAquiredAndNotChoosed--;
        GenerateRandomUpgrades();
        Instantiate(upgradePanel, GameManager.Instance.MainUICanvas.transform);
    }
    
    private void ApplyUpgrade(object sender, int upgradeIndex)
    {
        currentPlayerUpgrades.Add(currentUpgradeValues[upgradeIndex]);
        switch(currentUpgradeValues[upgradeIndex].EmpowermentType)
        {
            case EmpowermentType.Stat:
                StatEmpowerment statEmpowerment = (StatEmpowerment)currentUpgradeValues[upgradeIndex];
                PlayerStatsManager.Instance.ApplyUpgrade(statEmpowerment.StatType, statEmpowerment.Value);
                break;
            case EmpowermentType.Ability:
                AbilityEmpowerment abilityEmpowerment = (AbilityEmpowerment)currentUpgradeValues[upgradeIndex];
                AbilitiesManager.Instance.AddAbility(abilityEmpowerment.abilityHolder);
                availableUpgrades.Remove(abilityEmpowerment);
                if(AbilitiesManager.Instance.AbilitiesLimitReached)
                    availableUpgrades.RemoveAll( x => x.EmpowermentType == EmpowermentType.Ability);
                break;
            case EmpowermentType.Evolution:
                EvolutionEmpowerment evolutionEmpowerment = (EvolutionEmpowerment)currentUpgradeValues[upgradeIndex];
                AbilitiesManager.Instance.EvolveAbility(evolutionEmpowerment.abilityHolder, evolutionEmpowerment.AbilityReplaceName);
                availableUpgrades.RemoveAll( x => x.EmpowermentType == EmpowermentType.Evolution && ((EvolutionEmpowerment)x).AbilityReplaceName == evolutionEmpowerment.AbilityReplaceName);
                if(AbilitiesManager.Instance.EvolutionsLimitReached)
                    availableUpgrades.RemoveAll( x => x.EmpowermentType == EmpowermentType.Evolution);
                break;
        }
        playerChoosingUpgarde = false;
        GameManager.Instance.ChangeGamePauseState(GameState.UpgradeSelection);
    }

    protected void OnDestroy()
    {
        PlayerLevelManager.OnLevelChanged -= AquireUpgrade;
        PlayerUpgradeUI.OnUpgradeSelected -= ApplyUpgrade;
    }

}
