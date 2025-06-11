using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class PlayerUpgradeUI : MonoBehaviour
{
    [SerializeField] 
    private GameObject upgradePanel;
    [SerializeField] 
    private GameObject option1Button;
    [SerializeField] 
    private GameObject option2Button;
    [SerializeField] 
    private GameObject option3Button;
    [SerializeField] 
    private TextMeshProUGUI option1Name;
    [SerializeField] 
    private TextMeshProUGUI option2Name;
    [SerializeField] 
    private TextMeshProUGUI option3Name;
    [SerializeField] 
    private TextMeshProUGUI option1Description;
    [SerializeField] 
    private TextMeshProUGUI option2Description;
    [SerializeField] 
    private TextMeshProUGUI option3Description;
    [SerializeField] 
    private TMP_ColorGradient statUpgradeColorGradient;
    [SerializeField] 
    private TMP_ColorGradient abilityUpgradeColorGradient;
    [SerializeField] 
    private TMP_ColorGradient evolutionUpgradeColorGradient;

    public static EventHandler<int> OnUpgradeSelected;

    protected void Awake()
    {
        AddListener(option1Button, 0);
        AddListener(option2Button, 1);
        AddListener(option3Button, 2);
        InitUpgradeSelection();
        GameManager.Instance.ChangeGamePauseState(GameState.UpgradeSelection);
    }

    private void InitUpgradeSelection()
    {
        UpgradeChoiceCardUpdate(0, option1Button, option1Name, option1Description);
        UpgradeChoiceCardUpdate(1, option2Button, option2Name, option2Description);
        UpgradeChoiceCardUpdate(2, option3Button, option3Name, option3Description);
        upgradePanel.SetActive(true);
        option1Button.GetComponent<Animator>().Play("Normal");
        option2Button.GetComponent<Animator>().Play("Normal");
        option3Button.GetComponent<Animator>().Play("Normal");
    }

    private void UpgradeChoiceCardUpdate(int index, GameObject optionButton, TextMeshProUGUI optionName, TextMeshProUGUI optionDescription)
    {
        LevelUpEmpowerment levelUpEmpowerment = PlayerUpgradesManager.Instance.CurrentUpgradeValues[index];
        SkillContainer skillContainer;
        string skillTypes;
        optionButton.SetActive(levelUpEmpowerment != null);
        if (levelUpEmpowerment != null)
        {
            optionName.text = levelUpEmpowerment.Name;
            Image background = optionButton.GetComponent<Image>();
            switch(levelUpEmpowerment.EmpowermentType)
            {
                case EmpowermentType.Stat:
                    background.color = Color.white;
                    optionDescription.text = string.Format(((StatEmpowerment)levelUpEmpowerment).Description, 
                    ((StatEmpowerment)levelUpEmpowerment).Value);
                    optionName.enableVertexGradient = true;
                    optionName.colorGradientPreset = statUpgradeColorGradient;
                    optionDescription.enableVertexGradient = true;
                    optionDescription.colorGradientPreset = statUpgradeColorGradient;
                    break;
                case EmpowermentType.Ability:
                    skillContainer = ((AbilityEmpowerment)levelUpEmpowerment).SkillContainer;
                    background.color = new Color(0.4f, 0.4f, 0.95f, 1);
                    skillTypes = skillContainer.Type.ToString() + ", " +
                        (skillContainer is AOEContainer AOEcontainerA ? AOEcontainerA.AOEType.ToString() : 
                        skillContainer is SkillShotContainer skillShotContainerA ? skillShotContainerA.LaunchType.ToString() : "");
                    optionDescription.text =  
                        "<b>Types: </b>" + skillTypes + "<br>" + 
                        "<b>Damage Type: </b>" + skillContainer.SkillDamageType.ToString() + "<br>" +
                        skillContainer.Description;
                    optionName.enableVertexGradient = true;
                    optionName.colorGradientPreset = abilityUpgradeColorGradient;
                    optionDescription.enableVertexGradient = true;
                    optionDescription.colorGradientPreset = abilityUpgradeColorGradient;
                    break;
                case EmpowermentType.Evolution:
                    skillContainer = ((EvolutionEmpowerment)levelUpEmpowerment).SkillContainer;
                    background.color = new Color(0.95f, 0.4f, 0.4f, 1);
                    skillTypes = skillContainer.Type.ToString() + "," +
                        (skillContainer is AOEContainer AOEcontainerE ? AOEcontainerE.AOEType.ToString() : 
                        skillContainer is SkillShotContainer skillShotContainerE ? skillShotContainerE.LaunchType.ToString() : "");
                    optionDescription.text = 
                        "<b>Evolves: </b>" + ((EvolutionEmpowerment)levelUpEmpowerment).AbilityReplaceName + "<br>" +
                        "<b>Types: </b>" + skillTypes + "<br>" + 
                        "<b>Damage Type: </b>" + skillContainer.SkillDamageType.ToString() + "<br>" +
                        skillContainer.Description;
                    optionName.enableVertexGradient = true;
                    optionName.colorGradientPreset = evolutionUpgradeColorGradient;
                    optionDescription.enableVertexGradient = true;
                    optionDescription.colorGradientPreset = evolutionUpgradeColorGradient;
                    break;
            }
        }
    }

    private void SelectUpgrade(int upgradeIndex)
    {
        OnUpgradeSelected?.Invoke(this, upgradeIndex);
        //upgradePanel.SetActive(false);
        Destroy(upgradePanel);
    }

    private void AddListener(GameObject obj, int index)
    {
        EventTrigger trigger = obj.AddComponent<EventTrigger>( );
		EventTrigger.Entry entry = new EventTrigger.Entry( );
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener( delegate { SelectUpgrade(index); } );
		trigger.triggers.Add( entry );
        entry = new EventTrigger.Entry( );
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener( delegate { obj.GetComponent<Animator>().Play("Highlighted"); } );
        trigger.triggers.Add( entry );
        entry = new EventTrigger.Entry( );
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener( delegate { Animator animator = obj.GetComponent<Animator>(); if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) animator.Play("Normal"); else
            obj.GetComponent<Animator>().Play("ExitHighlighted"); } );
        trigger.triggers.Add( entry );
    }

}
