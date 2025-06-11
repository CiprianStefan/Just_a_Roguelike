using System;
using UnityEngine;

public class EnemyCombatEntity : CombatEntity
{
    protected EnemyMisc enemyMisc;
    protected float enemyEmpowerment = 0;

    public override double Health {get {return Mathf.RoundToInt(enemyMisc.enemyContainer.baseHealth * (1 + enemyEmpowerment / 2.5f));}}
    public override double Defence {get {return Mathf.RoundToInt(enemyMisc.enemyContainer.baseDefence * (1 + Mathf.Sqrt(enemyEmpowerment) / 1.5f));}}
    public override double PhysicalDamage {get {return Mathf.RoundToInt(enemyMisc.enemyContainer.baseDamage * (1 + enemyEmpowerment / 6));}}
    public override double MagicalDamage {get {return Mathf.RoundToInt(enemyMisc.enemyContainer.baseDamage * (1 + enemyEmpowerment / 6));}}
    public override double ElementalDamage {get {return Mathf.RoundToInt(enemyMisc.enemyContainer.baseDamage * (1 + enemyEmpowerment / 6));}}
    public override double DodgeChance {get {return enemyMisc.enemyContainer.baseDogeChance;}}
    public override double CriticalChance {get {return enemyMisc.enemyContainer.baseCriticalChance;}}
    public override double CriticalDamage {get {return enemyMisc.enemyContainer.baseCriticalDamage;}}
    public override double Block {get {return enemyMisc.enemyContainer.baseBlock;}}

    public EventHandler OnEnemyDeath;
    public static EventHandler OnBossDamageTaken;

    protected void Awake()
    {
        enemyMisc = GetComponent<EnemyMisc>();
    }

    protected override void Start()
    {
        base.Start(); 
        if(enemyMisc.enemyContainer.enemyType == EnemyType.Boss)
            Init();
    }

    protected override void Init()
    {
        base.Init();
        enemyEmpowerment = Mathf.Max(GameManager.Instance.GameTimeInMinutes,1) * (1 + Mathf.Max(EnemyGenerationManager.Instance.waveNumber,1) / 5);
    }

    public override bool IsTarget(CombatEntity dealer)
    {
        return dealer is PlayerCombatEntity;
    }

    protected override void ModifyCurrentHealth()
    {
        if(enemyMisc.isDead)
            return;
        base.ModifyCurrentHealth();
        if(enemyMisc.enemyContainer.enemyType == EnemyType.Boss)
            OnBossDamageTaken?.Invoke(this, EventArgs.Empty);
        if(currentHealth <= 0)
            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
    }

    public void UseObjectFromPool()
    {
        Init();
    }

}
