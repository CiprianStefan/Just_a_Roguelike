using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class SoulSelectorPanelUI : MonoBehaviour
{
    [SerializeField]
    private SoulContainerDatabase soulContainerDatabase;
    [SerializeField]
    private SoulContainer selectedSoul;

    [SerializeField]
    private GameObject soulHolder;
    [SerializeField]
    private GameObject statsDisplay;
    [SerializeField]
    private TextMeshProUGUI soulName;
    [SerializeField]
    private TextMeshProUGUI soulDescription;
    [SerializeField]
    private TextMeshProUGUI startingAbilityName;
    [SerializeField]
    private Image startingAbilityIcon;
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private GameObject selectedSoulObject;

    [SerializeField]
    private Sprite soulBackground;

    [SerializeField]
    private List<GameObject> soulStatsDisplays = new List<GameObject>();

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private Button backButton;

    [SerializeField]
    private TMP_ColorGradient colorGradient;

    protected void Awake()
    {
        foreach(SoulContainer soulContainer in soulContainerDatabase.soulContainers)
        {
            CreateSoulDisplay(soulContainer);
        }
        selectedSoul = soulContainerDatabase.soulContainers[0];
        ChangeSelectedSoulDisplay(soulHolder.transform.GetChild(0).gameObject);
        UpdateSoulInformation(selectedSoul);
        playButton.onClick.AddListener(delegate{StartGame();});
        backButton.onClick.AddListener(delegate{BackToMainMenu();});
        gameObject.SetActive(false);
    }

    private void CreateSoulDisplay(SoulContainer soulContainer)
    {
        GameObject soulDisplay = new GameObject();
        soulDisplay.transform.SetParent(soulHolder.transform);
        RectTransform rectTransform = soulDisplay.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(100, 100);
        rectTransform.localScale = new Vector3(1, 1, 1);
        Image image = soulDisplay.AddComponent<Image>();
        image.sprite = soulBackground;
        image.color = new Color(1, 1, 1);
        GameObject soulIcon = new GameObject();
        soulIcon.transform.SetParent(soulDisplay.transform);
        rectTransform = soulIcon.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 200);
        rectTransform.localScale = new Vector3(1, 1, 1);
        Image iconImage = soulIcon.AddComponent<Image>();
        iconImage.sprite = soulContainer.SoulDisplayIcon;
        iconImage.raycastTarget = false;
        AddListener(soulDisplay, soulContainer);
    }

    private void AddListener(GameObject obj, SoulContainer soulContainer)
    {
        EventTrigger trigger = obj.AddComponent<EventTrigger>( );
		EventTrigger.Entry entry = new EventTrigger.Entry( );
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener( delegate { OnPointerClick(obj, soulContainer); } );
		trigger.triggers.Add( entry );
    }

    private void OnPointerClick(GameObject newSoulSelectedObj, SoulContainer soulContainer)
    {
        selectedSoul = soulContainer;
        ChangeSelectedSoulDisplay(newSoulSelectedObj);
        UpdateSoulInformation(soulContainer);
    }

    private void ChangeSelectedSoulDisplay(GameObject newSoulSelectedObj)
    {
        if(selectedSoulObject != null)
            selectedSoulObject.GetComponent<Image>().color = new Color(1, 1, 1);
        selectedSoulObject = newSoulSelectedObj;
        selectedSoulObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
    }

    private void UpdateSoulInformation(SoulContainer soulContainer)
    {
        soulName.text = soulContainer.Name;
        soulDescription.text = soulContainer.Description;
        startingAbilityName.text = soulContainer.StartAbilityName;
        startingAbilityIcon.sprite = soulContainer.StartAbilityIcon;
        foreach(GameObject obj in soulStatsDisplays)
            Destroy(obj);
        soulStatsDisplays.Clear();
        foreach(BaseStat stat in soulContainer.StartStats)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(statsDisplay.transform);
            RectTransform rectTransform = obj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(280, 30);
            rectTransform.localScale = new Vector3(1, 1, 1);
            TextMeshProUGUI text = obj.AddComponent<TextMeshProUGUI>();
            text.fontSize = 25;
            text.horizontalAlignment = HorizontalAlignmentOptions.Left;
            text.verticalAlignment = VerticalAlignmentOptions.Middle;
            text.enableVertexGradient = true;
            text.colorGradientPreset = colorGradient;
            text.text = string.Join(" ", System.Text.RegularExpressions.Regex.Matches(stat.StatType.ToString(), "[A-Z][a-z]*").Select(m => m.Value.First().ToString().ToUpper() + m.Value[1..].ToLower())) + ": " + stat.StatValue.ToString();
            soulStatsDisplays.Add(obj);
        }
    }

    private void StartGame()
    {
        PlayerPrefs.SetString("SelectedSoul", selectedSoul.Name);
        SceneManager.LoadScene("Main");
    }
    
    private void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
