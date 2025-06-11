using System.Collections.Generic;
using UnityEngine;

public class StatisticTracker : MonoBehaviour
{
    public static StatisticTracker Instance;

    public List<StatisticsAbilityDamage> abilitiesDamage = new List<StatisticsAbilityDamage>();
    public List<StatisticsEnemiesKilled> enemiesKilled = new List<StatisticsEnemiesKilled>();

    protected void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        CombatEntity.OnEntityTakeDamageStatic += OnEntityTakeDamage;
        EnemyMisc.OnEnemyDeathStatic += OnEnemyDeath;
    }

    private void OnEntityTakeDamage(object sender, DamageInstance damageInstance)
    {
        if (damageInstance.skillContainer == null)
            return;
        if (abilitiesDamage.Exists(x => x.Name == damageInstance.skillContainer.Name))
            abilitiesDamage.Find(x => x.Name == damageInstance.skillContainer.Name).Damage += (double)damageInstance.HealthAlterationValue;
        else
            abilitiesDamage.Add(new StatisticsAbilityDamage(damageInstance.skillContainer.Name, damageInstance.skillContainer.SkillDamageType, damageInstance.HealthAlterationValue));
    }

    private void OnEnemyDeath(object sender, EnemyContainer enemyContainer)
    {
        if (enemiesKilled.Exists(x => x.Name == enemyContainer.enemyName))
            enemiesKilled.Find(x => x.Name == enemyContainer.enemyName).IncrementAmount();
        else
            enemiesKilled.Add(new StatisticsEnemiesKilled(enemyContainer.enemyName, enemyContainer.enemyType));
    }

    protected void OnDestroy()
    {
        CombatEntity.OnEntityTakeDamageStatic -= OnEntityTakeDamage;
        EnemyMisc.OnEnemyDeathStatic -= OnEnemyDeath;
    }
}

public class StatisticsAbilityDamage
{
    public string Name { get; set; }
    public SkillDamageType DamageType { get; set; }
    public double Damage { get; set; }

    public StatisticsAbilityDamage(string name, SkillDamageType damageType, double damage)
    {
        Name = name;
        DamageType = damageType;
        Damage = damage;
    }
}

public class StatisticsEnemiesKilled
{
    public string Name { get; set; }
    public int Amount { get; set; }
    public EnemyType EnemyType { get; set; }
    

    public StatisticsEnemiesKilled(string name, EnemyType enemyType)
    {
        Name = name;
        Amount = 1;
        EnemyType = enemyType;
    }

    public void IncrementAmount()
    {
        Amount++;
    }
}