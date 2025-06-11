using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.Text.RegularExpressions;
using System.Linq;

public class StatsDisplayUI : MonoBehaviour
{
    public static StatsDisplayUI Instance;

    [SerializeField]
    private GameObject statDisplayHolderPrefab;

    private Dictionary<StatType, GameObject> statsDisplaysDictionary;

    public EventHandler<KeyValuePair<StatType, Stat>> OnStatDisplayEnter;
    public EventHandler OnStatDisplayExit;

    protected void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    protected void Start()
    {
        statsDisplaysDictionary = new Dictionary<StatType, GameObject>();
        foreach(KeyValuePair<StatType, Stat> stat in PlayerStatsManager.Instance.stats)
        {
            GameObject obj = Instantiate(statDisplayHolderPrefab, transform);
            statsDisplaysDictionary.Add(stat.Key, obj);
            InitializeStatDisplay(obj, stat.Value);
            AddListener(obj, stat);
        }
        Stat.OnStatValueChange += OnStatValueChange;
    }

    private void InitializeStatDisplay(GameObject gameObject, Stat stat)
    {
        gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Join(" ", Regex.Matches(stat.StatType.ToString(), "[A-Z][a-z]*").Select(m => m.Value.First().ToString().ToUpper() + m.Value[1..].ToLower()));
        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = stat.StatValue.ToString();
    }

    private void AddListener(GameObject obj, KeyValuePair<StatType, Stat> keyValuePair)
    {
        EventTrigger trigger = obj.AddComponent<EventTrigger>( );
		EventTrigger.Entry entry = new EventTrigger.Entry( );
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener( delegate { OnPointerEnter(keyValuePair); } );
		trigger.triggers.Add( entry );
        entry = new EventTrigger.Entry( );
		entry.eventID = EventTriggerType.PointerExit;
		entry.callback.AddListener( delegate { OnPointerExit(); } );
		trigger.triggers.Add( entry );
    }

    private void OnPointerEnter(KeyValuePair<StatType, Stat> keyValuePair)
    {
        OnStatDisplayEnter?.Invoke(this, keyValuePair);
    }

    private void OnPointerExit()
    {
        OnStatDisplayExit?.Invoke(this, EventArgs.Empty);
    }

    private void OnStatValueChange(object sender, int value)
    {
        Stat stat = (Stat)sender;
        statsDisplaysDictionary[stat.StatType].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = stat.StatValue.ToString();
    }

    protected void OnDestroy()
    {
        Stat.OnStatValueChange -= OnStatValueChange;
    }
}
