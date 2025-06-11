using UnityEngine;
using TMPro;

public class UITimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    protected void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    protected void Update()
    {
        timerText.text = GetFormattedTime(GameManager.Instance.gameTime);
    }

    private string GetFormattedTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600f);
        int minutes = Mathf.FloorToInt((time - hours * 3600f) / 60f);
        int seconds = Mathf.FloorToInt(time - hours * 3600f - minutes * 60f);

        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

        return formattedTime;
    }
}
