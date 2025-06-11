using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillHoldersUI : MonoBehaviour
{
    public static SkillHoldersUI Instance;

    [SerializeField] 
    private GameObject skillHolderPrefab;

    public GameObject [] skillHoldersDisplay;
    public SkillContainer[] skillContainers;

    public EventHandler<SkillContainer> OnSkillHolderEnter;
    public EventHandler OnSkillHolderExit;

    
    protected void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        skillHoldersDisplay = new GameObject[6];
        skillContainers = new SkillContainer[6];
        for (int i = 0; i < skillHoldersDisplay.Length; i++)
        {
            skillHoldersDisplay[i] = Instantiate(skillHolderPrefab, transform);
            AddListener(skillHoldersDisplay[i], i);
        }
        AbilitiesManager.Instance.OnAbilityUpdate += UpdateSkillHolder;
    }

    public void UpdateSkillHolder(object sender, Tuple<int, SkillContainer> skillContainer)
    {
        Image skillHolderDisplay = skillHoldersDisplay[skillContainer.Item1].transform.GetChild(0).GetComponent<Image>();
        skillHolderDisplay.sprite = skillContainer.Item2.Icon;
        skillHolderDisplay.color = new Color(1,1,1,1);
        skillContainers[skillContainer.Item1] = skillContainer.Item2;
    }

    protected void OnDestroy()
    {
        AbilitiesManager.Instance.OnAbilityUpdate -= UpdateSkillHolder;        
    }

    private void AddListener(GameObject obj, int index)
    {
        EventTrigger trigger = obj.AddComponent<EventTrigger>( );
		EventTrigger.Entry entry = new EventTrigger.Entry( );
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener( delegate { OnPointerEnter(index); } );
		trigger.triggers.Add( entry );
        entry = new EventTrigger.Entry( );
		entry.eventID = EventTriggerType.PointerExit;
		entry.callback.AddListener( delegate { OnPointerExit(); } );
		trigger.triggers.Add( entry );
    }

    private void OnPointerEnter(int index)
    {
        OnSkillHolderEnter?.Invoke(this, skillContainers[index]);
    }

    private void OnPointerExit()
    {
        OnSkillHolderExit?.Invoke(this, EventArgs.Empty);
    }
}
