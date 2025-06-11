using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelDisplay;
    [SerializeField]
    private Image expBarFill;

    protected void Start()
    {
        levelDisplay.text = "1";
        expBarFill.fillAmount = 0f;
        PlayerLevelManager.OnLevelChanged += OnLevelChanged;
        PlayerLevelManager.OnExperienceChanged += OnExperienceChanged;
    }

    private void OnLevelChanged(object sender, EventArgs e)
    {
        levelDisplay.text = PlayerLevelManager.Instance.Level.ToString();
        expBarFill.fillAmount = 0f;
    }

    private void OnExperienceChanged(object sender, EventArgs e)
    {
        expBarFill.fillAmount = PlayerLevelManager.Instance.Experience/PlayerLevelManager.Instance.ExperienceToNextLevel;
    }

    protected void OnDestroy()
    {
        PlayerLevelManager.OnLevelChanged -= OnLevelChanged;
        PlayerLevelManager.OnExperienceChanged -= OnExperienceChanged;
    }
}
