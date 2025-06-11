using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoulContainer", menuName = "SoulContainer", order = 1)]
public class SoulContainer : ScriptableObject
{
    public string Name;
    [TextArea(3, 10)]
    public string Description;
    public Sprite SoulDisplayIcon;
    public string StartAbilityName;
    public Sprite StartAbilityIcon;
    public List<BaseStat> StartStats;
    public SoulContainer()
    {
        StartStats = new List<BaseStat>
        {
            new BaseStat(StatType.Health, 100),
            new BaseStat(StatType.Defence, 10),
            new BaseStat(StatType.PhysicalDamage, 70),
            new BaseStat(StatType.MagicalDamage, 70),
            new BaseStat(StatType.ElementalDamage, 70),
            new BaseStat(StatType.AbilityCooldown, 0),
            new BaseStat(StatType.MovementSpeed, 100),
            new BaseStat(StatType.DodgeChance, 0),
            new BaseStat(StatType.CriticalChance, 5),
            new BaseStat(StatType.CriticalDamage, 100),
            new BaseStat(StatType.Area, 100),
            new BaseStat(StatType.Projectiles, 0),
            new BaseStat(StatType.ExperienceGain, 100),
            new BaseStat(StatType.Multicast, 0),
            new BaseStat(StatType.Block, 10)
        };
    }
}

[Serializable]
public class BaseStat
{
    public StatType StatType;
    public int StatValue;
    public bool hasCustomCap;
    public int CustomCap;
    
    public BaseStat(StatType statType, int statValue)
    {
        StatType = statType;
        StatValue = statValue;
        hasCustomCap = false;
    }
    public BaseStat(StatType statType, int statValue, int customCap)
    {
        StatType = statType;
        StatValue = statValue;
        hasCustomCap = true;
        CustomCap = customCap;
    }
}