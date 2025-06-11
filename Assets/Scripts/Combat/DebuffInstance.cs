using System;
using UnityEngine;

public enum DebuffType{
    Bleed,
    Poison,
    Burn,
    Freeze,
}

public class DebuffInstance
{
    private readonly CombatEntity targetCombatEntity;
    private readonly CombatEntity sourceCombatEntity = null;
    private readonly double fixedDamage;
    private readonly float duration;
    private readonly float frequency;
    private float remainingCycleCooldown;
    private float remainingDuration;
    public DebuffType debuffType;

    public DebuffInstance(CombatEntity _targetCombatEntity, CombatEntity _sourceCombatEntity, DebuffType _debuffType, float _duration, float _frequency)
    {
        targetCombatEntity = _targetCombatEntity;
        sourceCombatEntity = _sourceCombatEntity;
        debuffType = _debuffType;
        duration = _duration;
        frequency = _frequency;
        remainingCycleCooldown = frequency;
        remainingDuration = duration;
    }

    public DebuffInstance(CombatEntity _targetCombatEntity, double _fixedDamage, DebuffType _debuffType, float _duration, float _frequency)
    {
        targetCombatEntity = _targetCombatEntity;
        fixedDamage = _fixedDamage;
        sourceCombatEntity = null;
        debuffType = _debuffType;
        duration = _duration;
        frequency = _frequency;
        remainingCycleCooldown = frequency;
        remainingDuration = duration;
    }

    public DebuffInstance(CombatEntity _targetCombatEntity, CombatEntity _sourceCombatEntity, DebuffType _debuffType)
    {
        targetCombatEntity = _targetCombatEntity;
        sourceCombatEntity = _sourceCombatEntity;
        debuffType = _debuffType;
        duration = debuffType switch {
            DebuffType.Bleed => 5,
            DebuffType.Poison => 5,
            DebuffType.Burn => 5,
            DebuffType.Freeze => 5,
            _ => 0
        };
        frequency = debuffType switch {
            DebuffType.Bleed => 1,
            DebuffType.Poison => 1,
            DebuffType.Burn => 1,
            DebuffType.Freeze => 0,
            _ => 0
        };
        remainingCycleCooldown = frequency;
        remainingDuration = duration;
    }

    public bool ManageDuration()
    {
        remainingCycleCooldown -= Time.deltaTime;
        remainingDuration -= Time.deltaTime;
        
        if(remainingCycleCooldown <= 0)
        {
            targetCombatEntity.OnCycleDebuff?.Invoke(this, EventArgs.Empty);
            if(debuffType != DebuffType.Freeze)
            {
                targetCombatEntity.ApplyDamage( sourceCombatEntity != null ? debuffType switch {
                    DebuffType.Bleed => sourceCombatEntity.PhysicalDamage / 0.2f,
                    DebuffType.Poison => sourceCombatEntity.ElementalDamage / 0.2f,
                    DebuffType.Burn => sourceCombatEntity.MagicalDamage / 0.2f,
                    _ => 0
                } : fixedDamage);
                remainingCycleCooldown = frequency;
            }
            
        }

        return remainingDuration <= 0;
    }

}
