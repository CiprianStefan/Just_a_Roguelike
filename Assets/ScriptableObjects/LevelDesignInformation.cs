using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesignInformation", menuName = "LevelDesignInformation", order = 1)]
public class LevelDesignInformation : ScriptableObject
{
    public GameObject eliteEnemyPrefab;
    public List<WaveDesignInformation> waveDesignInformation;
}

public enum WaveType
{
    Normal,
    Boss
}

[Serializable]
public class WaveDesignInformation
{
    public WaveType waveType;
    public List<EnemyIterationInformation> enemyIterationInformation;
    public float waveCooldown;
    public bool waveNeedsToBeCleaned;
    public bool spawnEliteEnemy;
}

[Serializable]
public class EnemyIterationInformation
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public float spawnRateInSeconds;
}