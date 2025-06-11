using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;
    public Dictionary<StatType, Stat> stats;

    public int Health { get => stats[StatType.Health].StatValue; }
    public int Defence { get => stats[StatType.Defence].StatValue; }
    public int PhysicalDamage { get => stats[StatType.PhysicalDamage].StatValue; }
    public int MagicalDamage { get => stats[StatType.MagicalDamage].StatValue; }
    public int ElementalDamage { get => stats[StatType.ElementalDamage].StatValue; }
    public int AbilityCooldown { get => stats[StatType.AbilityCooldown].StatValue; }
    public int MovementSpeed { get => stats[StatType.MovementSpeed].StatValue; }
    public int DodgeChance { get => stats[StatType.DodgeChance].StatValue; }
    public int CriticalChance { get => stats[StatType.CriticalChance].StatValue; }
    public int CriticalDamage { get => stats[StatType.CriticalDamage].StatValue; }
    public int Block { get => stats[StatType.Block].StatValue; }
    public int Area { get => stats[StatType.Area].StatValue; }
    public int Projectiles { get => stats[StatType.Projectiles].StatValue; }
    public int ExperienceGain { get => stats[StatType.ExperienceGain].StatValue; }
    public int Multicast { get => stats[StatType.Multicast].StatValue; }


    protected void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        stats = new Dictionary<StatType, Stat>();
    }

    protected void Start()
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            BaseStat baseStat = PlayerMisc.Instance.soulContainer.StartStats.Find(stat => stat.StatType == statType);
            stats.Add(statType, 
                baseStat.hasCustomCap ?
            new Stat(statType, baseStat.StatValue, baseStat.CustomCap) :
            new Stat(statType, baseStat.StatValue));
        }
    }

    public void ApplyUpgrade(StatType statType, int value)
    {
        stats[statType].StatValue = value;
    }
}

public enum StatType
{
    Health,
    Defence,
    PhysicalDamage,
    MagicalDamage,
    ElementalDamage,
    AbilityCooldown,
    MovementSpeed,
    DodgeChance,
    CriticalChance,
    CriticalDamage,
    Area,
    Projectiles,
    ExperienceGain,
    Multicast,
    Block
}

public class Stat 
{
    private readonly StatType statType;
    private int statValue;
    private readonly int statCapp;

    public StatType StatType { get => statType; }
    public int StatValue { get => statValue; 
                        set {
                            if(IsCapped)
                                return;
                            statValue = statCapp != -1 && statValue + value >= statCapp ? statCapp : statValue + value;
                            OnStatValueChange?.Invoke(this, value);
                        } 
    }
    public int StatCapp { get => statCapp; }
    public bool IsCapped { get => statCapp != -1 && statValue >= statCapp; }

    public static EventHandler<int> OnStatValueChange;

    public Stat(StatType statType, int value)
    {
        this.statType = statType;
        this.statValue = value;
        statCapp = statType switch
        {
            StatType.DodgeChance => 60,
            StatType.AbilityCooldown => 60,
            StatType.CriticalChance => 100,
            StatType.Projectiles => 6,
            StatType.Multicast => 50,
            _ => -1,
        };
    }

    public Stat(StatType statType, int value, int statCapp)
    {
        this.statType = statType;
        this.statValue = value;
        this.statCapp = statCapp;
    }
}
