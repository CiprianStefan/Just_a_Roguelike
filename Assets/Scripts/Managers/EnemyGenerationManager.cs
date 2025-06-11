using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyGenerationManager : MonoBehaviour
{
    public static EnemyGenerationManager Instance;

    [SerializeField]
    private LevelDesignInformation levelDesignInformation;
    [SerializeField]
    private GameObject EliteEnemyPrefab { get { return levelDesignInformation.eliteEnemyPrefab; } }
    [SerializeField]
    private float mapSize = 14f;
    [SerializeField]
    private int amountToPool = 50;

    protected List<EnemyObjectPool> enemyObjectPools;

    public int waveNumber = 0;
    private float waveCooldown = 0;
    private bool levelCompleted = false;
    private List<EnemyActiveIterationInformation> enemyActiveIterationInformation;

    public static EventHandler OnLevelCompleted;
    public static EventHandler OnWaveStart;
    public static EventHandler OnWaveCleanUp;

    public WaveType WaveType {
        get
        {
            return levelDesignInformation.waveDesignInformation[waveNumber-1].waveType;
        }
    }

    protected void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    protected void Start()
    {
        enemyObjectPools = new List<EnemyObjectPool>();
        enemyActiveIterationInformation = new List<EnemyActiveIterationInformation>();
        EnemyMisc.OnEnemyDeathStatic += OnEnemyDeathStatic;
    }

    protected void Update()
    {
        NewWaveInitialization();
        EnemySpawnRoutine();
    }

    private void NewWaveInitialization()
    {
        if(waveCooldown < 0f)
        {
            if(waveNumber < levelDesignInformation.waveDesignInformation.Count)
            {
                enemyActiveIterationInformation = new List<EnemyActiveIterationInformation>();
                if(levelDesignInformation.waveDesignInformation[waveNumber].waveNeedsToBeCleaned)
                    OnWaveCleanUp?.Invoke(this, EventArgs.Empty);
                if(levelDesignInformation.waveDesignInformation[waveNumber].spawnEliteEnemy)
                    SpawnEliteEnemy();
                if(levelDesignInformation.waveDesignInformation[waveNumber].waveType == WaveType.Normal)
                {
                    foreach (EnemyIterationInformation enemyIterationInformation in levelDesignInformation.waveDesignInformation[waveNumber].enemyIterationInformation)
                    {
                        if(enemyObjectPools.Find(x => x.name == enemyIterationInformation.enemyPrefab.name) == null)
                        {
                            CreateEnemyObjectPool(enemyIterationInformation.enemyPrefab);
                        }
                        enemyActiveIterationInformation.Add(new EnemyActiveIterationInformation(enemyIterationInformation.enemyPrefab.name, enemyIterationInformation.enemyCount, enemyIterationInformation.spawnRateInSeconds));
                    }
                }
                else if(levelDesignInformation.waveDesignInformation[waveNumber].waveType == WaveType.Boss)
                {
                    GameObject bossPrefab = levelDesignInformation.waveDesignInformation[waveNumber].enemyIterationInformation[0].enemyPrefab;
                    Instantiate(bossPrefab, Vector3.zero, Quaternion.identity);
                    enemyActiveIterationInformation.Add(new EnemyActiveIterationInformation(bossPrefab.name, 1, 0));
                    enemyActiveIterationInformation[0].amountSpawned = 1;
                }
                waveCooldown = levelDesignInformation.waveDesignInformation[waveNumber].waveCooldown;
                waveNumber++;
                OnWaveStart?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void EnemySpawnRoutine()
    {
        if(enemyActiveIterationInformation.Any(x => x.amountSpawned < x.amountNeededToSpawn))
            foreach (EnemyActiveIterationInformation enemyActiveIteration in enemyActiveIterationInformation)
            {
                if(enemyActiveIteration.amountSpawned < enemyActiveIteration.amountNeededToSpawn)
                {
                    if(levelDesignInformation.waveDesignInformation[waveNumber-1].waveType == WaveType.Normal)
                    {
                        if(enemyActiveIteration.spawnCooldown <= 0f)
                        {
                            Vector2 nextSpawnLocation;
                            do
                            {
                                nextSpawnLocation = new Vector2(Random.Range(-mapSize, mapSize), Random.Range(-mapSize, mapSize));
                            }while(Vector2.Distance(nextSpawnLocation, GameManager.Instance.Player.transform.position) < 10f);
                            foreach (GameObject obj in enemyObjectPools.Find(x => x.name == enemyActiveIteration.enemyName).objectsToPool)
                            {
                                if(obj.activeSelf)
                                    continue;
                                obj.transform.position = (Vector3)nextSpawnLocation;
                                obj.GetComponent<EnemyMisc>().UseObjectFromPool();
                                obj.SetActive(true);
                                enemyActiveIteration.amountSpawned++;
                                break;
                            }
                            enemyActiveIteration.spawnCooldown = enemyActiveIteration.spawnRateInSeconds;
                        }
                        enemyActiveIteration.spawnCooldown -= Time.deltaTime;
                    }
                }
            }
        else
            waveCooldown -= Time.deltaTime;
    }

    private void CreateEnemyObjectPool(GameObject enemyPrefab)
    {
        List<GameObject> objectsToPool = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            objectsToPool.Add(obj);
        }
        enemyObjectPools.Add(new EnemyObjectPool { name = enemyPrefab.name, objectsToPool = objectsToPool });
    }

    private void OnEnemyDeathStatic(object sender, EnemyContainer enemyContainer)
    {
        var enemyActiveIteration = enemyActiveIterationInformation.Find(x => x.enemyName == enemyContainer.name);
        if (enemyActiveIteration != null)
            enemyActiveIteration.amountKilled++;
        if(levelCompleted)
            return;
        if(waveNumber >= levelDesignInformation.waveDesignInformation.Count && enemyActiveIterationInformation.All(x => x.amountKilled == x.amountNeededToSpawn))
        {
            levelCompleted = true;
            OnLevelCompleted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpawnEliteEnemy()
    {
        if(EliteEnemyPrefab)
        {
            Vector2 spawnLocation = new Vector2(Random.Range(-mapSize, mapSize), Random.Range(-mapSize, mapSize));
            GameObject eliteEnemy = Instantiate(EliteEnemyPrefab, spawnLocation, Quaternion.identity);
            var (primaryType, secondaryType) = EliteEnemiesTypesGenerator.Instance.GetEliteEnemyTypes();
            eliteEnemy.GetComponent<EliteEnemyMisc>().OnInit(primaryType, secondaryType);
        }
    }
    protected void OnDestroy()
    {
        EnemyMisc.OnEnemyDeathStatic -= OnEnemyDeathStatic;
    }

}

public class EnemyObjectPool
{
    public string name;
    public List<GameObject> objectsToPool;
}

public class EnemyActiveIterationInformation
{
    public String enemyName;
    public int amountNeededToSpawn;
    public int amountSpawned;
    public int amountKilled;
    public float spawnRateInSeconds;
    public float spawnCooldown;

    public EnemyActiveIterationInformation(string _enemyName, int _amountNeededToSpawn, float _spawnRateInSeconds)
    {
        enemyName = _enemyName;
        amountNeededToSpawn = _amountNeededToSpawn;
        amountSpawned = 0;
        amountKilled = 0;
        spawnRateInSeconds = _spawnRateInSeconds;
        spawnCooldown = 0;
    }
}
