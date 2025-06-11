using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIFXsFade : MonoBehaviour
{

    private Image fadeImage;
    private readonly float fadeDuration = 0.4f;
    private float fadeTimer = 0;
    private double lastPlayerHealth;
    private Color damageTakenColor = new Color(1, 0, 0, 0.2f);
    private Color healthRegenColor = new Color(0, 0.7f, 0, 0.2f);
    private Color normalColor = new Color(1, 1, 1, 0);
    private Color currentColor;


    protected void Start()
    {
        fadeImage = GetComponent<Image>();
        lastPlayerHealth = Mathf.Infinity;
        fadeTimer = fadeDuration;
        PlayerCombatEntity.OnPlayerHealthModification += OnPlayerHealthModification;
    }

    protected void Update()
    {
        if(fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            fadeImage.color = Color.Lerp(currentColor, normalColor, fadeTimer);
        }
        else
            fadeImage.color = normalColor;
    }

    private void OnPlayerHealthModification(object sender, Tuple<double, double> healthValues)
    {
        if(healthValues.Item2 < lastPlayerHealth && healthValues.Item2 != healthValues.Item1)
        {
            lastPlayerHealth = healthValues.Item2;
            fadeTimer = 0;
            currentColor = damageTakenColor;
            fadeImage.color = Color.Lerp(currentColor, normalColor, fadeTimer);
        }
        else if(healthValues.Item2 > lastPlayerHealth)
        {
            lastPlayerHealth = healthValues.Item2;
            fadeTimer = 0;
            currentColor = healthRegenColor;
            fadeImage.color = Color.Lerp(currentColor, normalColor, fadeTimer);
        }
    }

    protected void OnDestroy()
    {
        PlayerCombatEntity.OnPlayerHealthModification -= OnPlayerHealthModification;
    }
}
