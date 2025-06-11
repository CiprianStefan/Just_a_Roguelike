using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDamageManagerUI : MonoBehaviour
{
    public static DisplayDamageManagerUI Instance;

    private List<GameObject> objectsToPool = new List<GameObject>();
    private int amountToPool = 50;

    protected void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = CreateDisplayDamage();
            obj.SetActive(false);
            objectsToPool.Add(obj);
        }
    }

    private GameObject CreateDisplayDamage()
    {
        GameObject obj = new GameObject();
        obj.transform.SetParent(GameManager.Instance.DisplayDamageParent.transform);
        TextMeshProUGUI text =  obj.AddComponent<TextMeshProUGUI>();
        RectTransform rectTransform = (RectTransform)obj.transform;
        obj.AddComponent<DisplayDamageUIInstance>();
        rectTransform.sizeDelta = new Vector2(100, 40);
        rectTransform.localScale = Vector3.one;
        text.verticalAlignment = VerticalAlignmentOptions.Middle;
        text.horizontalAlignment = HorizontalAlignmentOptions.Center;
        text.color = Color.red;
        text.enableAutoSizing = true;
        text.fontSizeMin = 20;
        text.fontSizeMax = 40;
        text.fontStyle = FontStyles.Italic;
        return obj;
    }

    public void DisplayDamage(Vector3 spawnPosition, DamageInstance damageInstance)
    {
        GameObject obj = null;
        foreach (GameObject objToPool in objectsToPool)
        {
            if (!objToPool.activeInHierarchy)
            {
                obj = objToPool;
                break;
            }
        }
        if (obj == null)
        {
            obj = CreateDisplayDamage();
            objectsToPool.Add(obj);
        }
        obj.SetActive(true);
        DisplayDamageUIInstance displayDamageUIInstance = obj.GetComponent<DisplayDamageUIInstance>();
        TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
        RectTransform rectTransform = (RectTransform)obj.transform;
        Vector3 randomPosition = new Vector3(spawnPosition.x + Random.Range(-1f, 1f), spawnPosition.y + Random.Range(-1f, 1f));
        displayDamageUIInstance.spawnPosition = randomPosition;
        rectTransform.position = randomPosition;
        rectTransform.localScale = Vector3.one;
        text.text = damageInstance.HealthAlterationValue.ToString();
    }
}
