using System;
using System.Collections;
using UnityEngine;

public class PlayerCombatEntity : CombatEntity
{
    public override double Health { get{return PlayerStatsManager.Instance.stats[StatType.Health].StatValue;} }
    public override double Defence { get{return PlayerStatsManager.Instance.stats[StatType.Defence].StatValue * (1 - (EliteLethalFieldAffected ? 0.7f : 0));} }
    public override double PhysicalDamage { get{return PlayerStatsManager.Instance.stats[StatType.PhysicalDamage].StatValue;} }
    public override double MagicalDamage { get{return PlayerStatsManager.Instance.stats[StatType.MagicalDamage].StatValue;} }
    public override double ElementalDamage { get{return PlayerStatsManager.Instance.stats[StatType.ElementalDamage].StatValue;} }
    public double AbilityCooldown { get{return PlayerStatsManager.Instance.stats[StatType.AbilityCooldown].StatValue * (1 - (EliteStaticFieldAffected ? 0.5f : 0));} }
    public override double DodgeChance { get{return PlayerStatsManager.Instance.stats[StatType.DodgeChance].StatValue * (1 - (EliteElectricFieldAffected ? 0.5f : 0));} }
    public override double CriticalChance { get{return PlayerStatsManager.Instance.stats[StatType.CriticalChance].StatValue;} }
    public override double CriticalDamage { get{return PlayerStatsManager.Instance.stats[StatType.CriticalDamage].StatValue;} }
    public override double Block { get{return PlayerStatsManager.Instance.stats[StatType.Block].StatValue * (1 - (EliteLethalFieldAffected ? 0.7f : 0));} }

    public override double MovementSpeedReducedFromDebuffs{get{ return Mathf.Min(activeDebuffs.FindAll(x => x.debuffType == DebuffType.Freeze).Count * 20f + 
        (EliteElectricFieldAffected ? 50 : 0), 90)/100; }}

    private bool EliteElectricFieldAffected;
    private bool EliteDrainFieldAffected;
    private bool EliteStaticFieldAffected;
    private bool EliteLethalFieldAffected;
    
    public static EventHandler OnPlayerDeath;
    public static EventHandler<Tuple<double, double>> OnPlayerHealthModification;
    
    protected override void Init()
    {
        base.Init();
        Stat.OnStatValueChange += OnHealthChange;
        StartCoroutine(OnDrainFieldEffect());
    }

    protected override void ModifyCurrentHealth()
    {
        
        base.ModifyCurrentHealth();
        OnPlayerHealthModification?.Invoke(this, new Tuple<double, double>(Health, currentHealth));
        if(currentHealth <= 0)
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }

    public override bool IsTarget(CombatEntity dealer)
    {
        return dealer is EnemyCombatEntity;
    }

    private void OnHealthChange(object sender, int addedValue)
    {
        if(((Stat)sender).StatType != StatType.Health)
            return;
        RegenHealth(addedValue);
    }

    public void ApplyEliteDebuff(int eliteDebuffType)
    {
        switch (eliteDebuffType)
        {
            case 1:
                EliteElectricFieldAffected = true;
                break;
            case 2:
                EliteDrainFieldAffected = true;
                break;
            case 3:
                EliteStaticFieldAffected = true;
                break;
            case 4:
                EliteLethalFieldAffected = true;
                break;
        }
    }
    public void RemoveEliteDebuff(int eliteDebuffType)
    {
        switch (eliteDebuffType)
        {
            case 1:
                EliteElectricFieldAffected = false;
                break;
            case 2:
                EliteDrainFieldAffected = false;
                break;
            case 3:
                EliteStaticFieldAffected = false;
                break;
            case 4:
                EliteLethalFieldAffected = false;
                break;
        }
    }

    protected IEnumerator OnDrainFieldEffect()
    {
        Debug.Log("Drain Field Effect");
        while(true)
        {
            if(EliteDrainFieldAffected)
                EliteDrainDamage(Mathf.Max(Mathf.RoundToInt((float)Health * 0.04f), 1));
            yield return new WaitForSeconds(1);
        }
    }

    public DamageInstance EliteDrainDamage(double damageValue)
    {
        DamageInstance damageInstance = new DamageInstance(ApplyDefenceStatusOnRawDamage(damageValue)); 
        HealthAlterationQ.Enqueue(damageInstance);
        return damageInstance;
    }

    protected void OnDestroy()
    {
        Stat.OnStatValueChange -= OnHealthChange;
        StopCoroutine(OnDrainFieldEffect());
    }
}
