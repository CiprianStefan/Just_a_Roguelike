using UnityEngine;

public enum EnemyType
{
    BasicEnemy,
    Boss,
    Elite
}

[CreateAssetMenu(fileName = "EnemyContainer", menuName = "Enemy/EnemyContainer")]
public class EnemyContainer : ScriptableObject
{
    public string enemyName;
    public EnemyType enemyType = EnemyType.BasicEnemy;
    public float baseExperience = 10;
    public float baseMovementSpeed = 1.5f;
    public float attackCooldown = 1f;
    public float baseHealth = 100;
    public float baseDefence = 10;
    public float baseDamage = 10;
    public float baseDogeChance = 0;
    public float baseCriticalChance = 0;
    public float baseCriticalDamage = 0;
    public float baseBlock = 0;
    [Header("Just for ranged enemies")]
    public float attackRange = 1f;
    public GameObject projectilePrefab;
}
