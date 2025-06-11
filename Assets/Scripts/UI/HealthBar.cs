using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthDisplay;
    [SerializeField]
    private Image healthBarFill;

    protected void Start()
    {
        PlayerCombatEntity.OnPlayerHealthModification += OnPlayerHealthModification;
    }

    private void OnPlayerHealthModification(object sender,  Tuple<double, double> tuple)
    {
        healthDisplay.text = tuple.Item2 + "/ \n" + tuple.Item1;
        healthBarFill.fillAmount = (float)(tuple.Item2 / tuple.Item1);
    }

    protected void OnDestroy()
    {
        PlayerCombatEntity.OnPlayerHealthModification -= OnPlayerHealthModification;
    }
}
