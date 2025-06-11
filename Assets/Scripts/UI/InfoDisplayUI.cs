using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class InfoDisplayUI : MonoBehaviour
{
    [SerializeField]
    private GameObject skillInfoDisplayParent;
    [SerializeField]
    private TextMeshProUGUI nameTextMesh;
    [SerializeField]
    private TextMeshProUGUI descriptionTextMesh;

    protected void Start()
    {
        skillInfoDisplayParent.SetActive(false);
        SkillHoldersUI.Instance.OnSkillHolderEnter += OnSkillHolderEnter;
        SkillHoldersUI.Instance.OnSkillHolderExit += OnSkillHolderExit;
        StatsDisplayUI.Instance.OnStatDisplayEnter += OnStatDisplayEnter;
        StatsDisplayUI.Instance.OnStatDisplayExit += OnStatDisplayExit;    
    }

    private void OnSkillHolderEnter(object sender, SkillContainer skillContainer)
    {
        if(skillContainer == null)
            return;
        nameTextMesh.text = skillContainer.Name;
        descriptionTextMesh.text = 
            "Skill Type: \n" + 
                "\t" + string.Join(" ", Regex.Matches(skillContainer.Type.ToString(), "[A-Z][a-z]*").Select(m => m.Value.First().ToString().ToUpper() + m.Value[1..].ToLower())) + "\n" +
                "\t" + (skillContainer is AOEContainer AOEcontainerA ? string.Join(" ", Regex.Matches(AOEcontainerA.AOEType.ToString(), "[A-Z][a-z]*").Select(m => m.Value.First().ToString().ToUpper() + m.Value[1..].ToLower())) : 
                        skillContainer is SkillShotContainer skillShotContainerA ? string.Join(" ", Regex.Matches(skillShotContainerA.LaunchType.ToString(), "[A-Z][a-z]*").Select(m => m.Value.First().ToString().ToUpper() + m.Value[1..].ToLower())) : "") + "\n" +
            "Damage Type: " + 
                skillContainer.SkillDamageType switch
                {
                    SkillDamageType.PhysicalDamage => "<color=\"red\"><b>Physical</b></color>",
                    SkillDamageType.MagicalDamage => "<color=\"blue\"><b>Magical</b></color>",
                    SkillDamageType.ElementalDamage => "<color=\"orange\"><b>Elemental</b></color>",
                    _ => ""
                } + "\n" +
            (skillContainer.Cooldown != -1 ? "Base Cooldown: " + skillContainer.Cooldown + (skillContainer.Cooldown == 1 ? " second" : "seconds") + "\n\n" : "Triggers every 0.5 seconds \n\n") + 
            "\t" + skillContainer.Description;
        skillInfoDisplayParent.SetActive(true);
    }

    private void OnSkillHolderExit(object sender, EventArgs e)
    {
        skillInfoDisplayParent.SetActive(false);
    }

    private void OnStatDisplayEnter(object sender, KeyValuePair<StatType, Stat> stat)
    {
        nameTextMesh.text = string.Join(" ", Regex.Matches(stat.Key.ToString(), "[A-Z][a-z]*").Select(m => m.Value.First().ToString().ToUpper() + m.Value[1..].ToLower()));
        descriptionTextMesh.text = stat.Key switch
        {
            StatType.Health => stat.Value.StatValue + " Maximum <color=\"red\"><b>Health</b></color> points",
            StatType.Defence => "Reduce the damage taken by " + Mathf.RoundToInt(stat.Value.StatValue / 1.67f) + "%",
            StatType.PhysicalDamage => "Increase <color=\"red\">Physical Damage</color> by " + stat.Value.StatValue,
            StatType.MagicalDamage => "Increase <color=\"blue\">Magical Damage</color> by " + stat.Value.StatValue,
            StatType.ElementalDamage => "Increase <color=\"orange\">Elemental Damage</color> by " + stat.Value.StatValue,
            StatType.AbilityCooldown => "Reduce Ability Cooldown by " + stat.Value.StatValue + "%. \n Capped at: " + stat.Value.StatCapp + "%",
            StatType.MovementSpeed => "Increase Movement Speed by " + (stat.Value.StatValue - 100) + "%",
            StatType.DodgeChance => stat.Value.StatValue + "% Chance to dodge an attack. \n Capped at: " + stat.Value.StatCapp + "%",
            StatType.CriticalChance => stat.Value.StatValue + "% Chance to deal Critical Damage. \n Capped at: " + stat.Value.StatCapp + "%",
            StatType.CriticalDamage => "Increase Critical Damage by " + stat.Value.StatValue + "%",
            StatType.Area => "Increase Area of Effect by " + (stat.Value.StatValue - 100) + "%",
            StatType.Projectiles => "Increase the number of projectiles by " + stat.Value.StatValue + ". \n Capped at: " + stat.Value.StatCapp,
            StatType.ExperienceGain => "Increase Experience Gain by " + (stat.Value.StatValue - 100) + "%",
            StatType.Multicast => "Increase the chance to cast the same spell again by " + stat.Value.StatValue + "% \n Capped at: " + stat.Value.StatCapp + "%",
            StatType.Block => "Improve the amount of damage that can be blocked with a certain chance<br>"
                + "80% chance to block <= " + Mathf.RoundToInt(2.5f * Mathf.Sqrt(stat.Value.StatValue)) + " raw damage <br>"
                + "50% chance to block <= " + Mathf.RoundToInt(4f * Mathf.Sqrt(stat.Value.StatValue)) + " raw damage <br>"
                + "35% chance to block <= " + Mathf.RoundToInt(6f * Mathf.Sqrt(stat.Value.StatValue)) + " raw damage <br>"
                + "20% chance to block <= " + Mathf.RoundToInt(8f * Mathf.Sqrt(stat.Value.StatValue)) + " raw damage <br>",
            _ => "Stat: " + stat.Value.StatValue
        };
        skillInfoDisplayParent.SetActive(true);
    }

    private void OnStatDisplayExit(object sender, EventArgs e)
    {
        skillInfoDisplayParent.SetActive(false);
    }

    protected void OnDestroy()
    {
        SkillHoldersUI.Instance.OnSkillHolderEnter -= OnSkillHolderEnter;
        SkillHoldersUI.Instance.OnSkillHolderExit -= OnSkillHolderExit;
        StatsDisplayUI.Instance.OnStatDisplayEnter -= OnStatDisplayEnter;
        StatsDisplayUI.Instance.OnStatDisplayExit -= OnStatDisplayExit;
    }

}
