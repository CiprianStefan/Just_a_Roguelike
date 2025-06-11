using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class CombatEntity : MonoBehaviour
{
    protected double currentHealth;
    public Queue<HealthAlterationInstance>HealthAlterationQ;
    public List<DebuffInstance> activeDebuffs;

    public abstract double Health{get;}
    public double CurrentHealth{get{return currentHealth;}}
    public abstract double Defence{get;}
    public abstract double PhysicalDamage{get;}
    public abstract double MagicalDamage{get;}
    public abstract double ElementalDamage{get;}
    public abstract double DodgeChance{get;}
    public abstract double CriticalChance{get;}
    public abstract double CriticalDamage{get;}
    public abstract double Block{get;}

    public virtual double MovementSpeedReducedFromDebuffs{get{ return Mathf.Min(activeDebuffs.FindAll(x => x.debuffType == DebuffType.Freeze).Count * 20f, 90); }}

    public EventHandler<DamageInstance> OnEntityTakeDamage;
    public static EventHandler<DamageInstance> OnEntityTakeDamageStatic;
    public EventHandler OnCycleDebuff;

    protected virtual void Start()
    {
        Init();
    }

    protected void Update()
    {
        activeDebuffs.RemoveAll(x => x.ManageDuration());
        ModifyCurrentHealth();
    }

    protected virtual void Init()
    {
        HealthAlterationQ = new Queue<HealthAlterationInstance>();
        activeDebuffs = new List<DebuffInstance>();
        currentHealth = Health;
    }

    public void ApplyDebuff(CombatEntity dealer, DebuffType debuffType)
    {
        activeDebuffs.Add( new DebuffInstance(this, dealer, debuffType) );
    }

    public virtual DamageInstance ApplyDamage(double damageValue,CombatEntity dealer = null, bool canBeDodged = false, bool canBeCrit = false)
    {
        if(!IsTarget(dealer))
            return null;
        if(canBeDodged && Random.Range(1, 100) <= DodgeChance)
            return null;
        if(canBeCrit && dealer != null && Random.Range(1, 100) <= dealer.CriticalChance)
            damageValue *= (100 + dealer.CriticalDamage)/100;
        if(DamageIsBlocked((float)damageValue))
            return null;
        DamageInstance damageInstance = new DamageInstance(ApplyDefenceStatusOnRawDamage(damageValue)); 
        HealthAlterationQ.Enqueue(damageInstance);
        if(dealer != null)
            dealer.RegenHealth(damageInstance);
        return damageInstance;
    }
    public virtual DamageInstance ApplyDamage(CombatEntity dealer, SkillContainer skillContainer)
    {
        if(!IsTarget(dealer))
            return null;
        if(skillContainer.canBeDodged && Random.Range(1, 100) <= DodgeChance)
            return null;
        double damageValue = skillContainer.SkillDamageType switch
        {
            SkillDamageType.PhysicalDamage => dealer.PhysicalDamage * (skillContainer.DamagePercentage/100),
            SkillDamageType.MagicalDamage => dealer.MagicalDamage * (skillContainer.DamagePercentage/100),
            SkillDamageType.ElementalDamage => dealer.ElementalDamage * (skillContainer.DamagePercentage/100),
            _ => 0,
        };
        if(skillContainer.canBeCrit && Random.Range(1, 100) <= dealer.CriticalChance)
            damageValue *= (100 + dealer.CriticalDamage)/100;
        if(DamageIsBlocked((float)damageValue))
            return null;
        DamageInstance damageInstance = new DamageInstance(skillContainer, ApplyDefenceStatusOnRawDamage(damageValue)); 
        HealthAlterationQ.Enqueue(damageInstance);
        dealer.RegenHealth(damageInstance);
        return damageInstance;
    }

    public void RegenHealth(double regenValue)
    {
        HealthAlterationQ.Enqueue(new RegenerationInstance(Mathf.RoundToInt((float)regenValue)));
    }

    public void RegenHealth(DamageInstance damageInstance)
    {
    }

    protected virtual void ModifyCurrentHealth()
    {
        while(HealthAlterationQ.Count > 0)
        {
            object healthAlterationInstance = HealthAlterationQ.Dequeue();
            double updatedHealth;
            switch(healthAlterationInstance)
            {
                case DamageInstance damageInstance:
                    updatedHealth = currentHealth - damageInstance.HealthAlterationValue;
                    currentHealth = updatedHealth < 0 ? 0 : updatedHealth;
                    OnEntityTakeDamage?.Invoke(this, (DamageInstance)healthAlterationInstance);
                    OnEntityTakeDamageStatic?.Invoke(this, (DamageInstance)healthAlterationInstance);
                    break;
                case RegenerationInstance regenerationInstance:
                    updatedHealth = currentHealth + regenerationInstance.HealthAlterationValue;
                    currentHealth = updatedHealth > Health ? Health : updatedHealth;
                    break;
                default:
                    break;
            }
        }
    }

    public abstract bool IsTarget(CombatEntity dealer);

    public double ApplyDefenceStatusOnRawDamage(double damageValue)
    {
        float damageReductionValue = Mathf.Min((float)Defence /3.5f, 95);
        return Mathf.RoundToInt((float)(damageValue - (damageValue * damageReductionValue / 100)));
    }

    public virtual bool DamageIsBlocked(float damage)
    {
        return damage switch
        {
            float damageValue when damageValue <= Mathf.RoundToInt(2.5f * Mathf.Sqrt(Convert.ToSingle(Block))) => Random.Range(1, 100) <= 80,
            float damageValue when damageValue <= Mathf.RoundToInt(4f * Mathf.Sqrt(Convert.ToSingle(Block))) => Random.Range(1, 100) <= 50,
            float damageValue when damageValue <= Mathf.RoundToInt(6f * Mathf.Sqrt(Convert.ToSingle(Block))) => Random.Range(1, 100) <= 35,
            float damageValue when damageValue <= Mathf.RoundToInt(8f * Mathf.Sqrt(Convert.ToSingle(Block))) => Random.Range(1, 100) <= 20,
            _ => false,
        };
    }

}
